using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace DTimeControl
{
    [StaticConstructorOnStartup]
    public static class DayLengthCoreOverrides
    {
        static DayLengthCoreOverrides()
        {
            new Harmony("io.dtimecontrol.daylength.core").PatchAll();
        }
    }

    internal static class DayScale
    {
        // Multiplier: 1.0 = vanilla; 10 = 10× longer day
        public static float M => TimeControlSettings.speedMultiplier <= 0f
                                    ? 1f
                                    : TimeControlSettings.speedMultiplier;

        // Safely compute positive modulo for long
        public static long ModPos(long value, long mod)
        {
            long r = value % mod;
            return r < 0 ? r + mod : r;
        }
    }

    // ------------------------------------------------------------
    // 1) DayTick(long absTicks, float longitude) -> int [0..TicksPerDay)
    //    Override the *result* using a scaled day length.
    // ------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayTick))]
    public static class Patch_DayTick_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(long absTicks, float longitude, ref int __result)
        {
            // Recompute dayTick from scratch, *not* from __result.
            // Let vanilla keep its internal path; we just produce the scaled answer.

            float m = DayScale.M;
            if (m <= 0.9999f || m >= 1.0001f)
            {
                long dayLen = (long)(60000L * m); // scaled ticks-per-day
                long local = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
                long scaledDayTick = DayScale.ModPos(local, dayLen);

                // range fits int for any reasonable m (e.g., m <= 100)
                __result = (int)scaledDayTick;
            }
            // else vanilla 1:1 result remains
        }
    }

    // ------------------------------------------------------------
    // 2) HourFloat(long absTicks, float longitude) -> 0..24f
    //    Derive from our scaled dayTick: hour = dayTick / (2500f * m)
    // ------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.HourFloat))]
    public static class Patch_HourFloat_Postfix
    {
        [HarmonyPostfix]
        public static void Postfix(long absTicks, float longitude, ref float __result)
        {
            float m = DayScale.M;
            if (m <= 0.9999f || m >= 1.0001f)
            {
                // Reuse the logic above to get the scaled dayTick
                long dayLen = (long)(60000L * m);
                long local = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
                long scaledDayTick = DayScale.ModPos(local, dayLen);

                __result = (float)(scaledDayTick / (2500f * m));
                // Note: no wrap risk because numerator & denominator share the same scale
            }
        }
    }
}
