using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;   // Mathf

namespace DTimeControl
{
    // --------------------------------------------------------------------
    //  Bootstrap
    // --------------------------------------------------------------------
    [StaticConstructorOnStartup]
    public static class GenDateScaledCalendar
    {
        static GenDateScaledCalendar()
        {
            new Harmony("io.dtimecontrol.gendate.scaled").PatchAll();
        }
    }

    // --------------------------------------------------------------------
    //  Shared helpers
    // --------------------------------------------------------------------
    static class ScaleUtil
    {
        internal static float Mul => Mathf.Max(0.0001f, TimeControlSettings.speedMultiplier);

        internal static void DivTicks(ref long t) => t = (long)(t / Mul);
        internal static void DivTicks(ref int t) => t = (int)(t / Mul);

        // For property getters returning int / float
        internal static int Div(int v) => (int)(v / Mul);
        internal static float Div(float v) => v / Mul;
    }

    // --------------------------------------------------------------------
    //  ***  Properties  ***
    //  (postfix patches: divide return value)
    // --------------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysPassed), MethodType.Getter)]
    class Prop_DaysPassed { static void Postfix(ref int __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysPassedFloat), MethodType.Getter)]
    class Prop_DaysPassedF { static void Postfix(ref float __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysPassedSinceSettle), MethodType.Getter)]
    class Prop_DaysSettled { static void Postfix(ref int __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysPassedSinceSettleFloat), MethodType.Getter)]
    class Prop_DaysSettledF { static void Postfix(ref float __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TwelfthsPassed), MethodType.Getter)]
    class Prop_Twelfths { static void Postfix(ref int __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TwelfthsPassedFloat), MethodType.Getter)]
    class Prop_TwelfthsF { static void Postfix(ref float __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.YearsPassed), MethodType.Getter)]
    class Prop_Years { static void Postfix(ref int __result) => __result = ScaleUtil.Div(__result); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.YearsPassedFloat), MethodType.Getter)]
    class Prop_YearsF { static void Postfix(ref float __result) => __result = ScaleUtil.Div(__result); }

    // --------------------------------------------------------------------
    //  ***  Methods with long absTicks  ***
    // --------------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.HourFloat))]
    class M_HourFloat { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.HourInteger))]
    class M_HourInt { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.HourOfDay))]
    class M_HourOfDay { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayPercent))]
    class M_DayPercent { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayTick))]
    class M_DayTick { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayOfQuadrum))]
    class M_DayQuadrum { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayOfSeason))]
    class M_DaySeason { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayOfTwelfth))]
    class M_DayTwelfth { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayOfYear))]
    class M_DayYear { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.Quadrum))]
    class M_Quadrum { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.Season), new[] { typeof(long), typeof(Vector2) })]
    class M_SeasonVec { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.Season), new[] { typeof(long), typeof(float), typeof(float) })]
    class M_SeasonLat { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.Twelfth))]
    class M_Twelfth { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.Year))]
    class M_Year { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.YearPercent))]
    class M_YearPct { static void Prefix(ref long absTicks) => ScaleUtil.DivTicks(ref absTicks); }

    // --------------------------------------------------------------------
    //  ***  Methods with int gameTicks ***  (DaysPassedAt, YearsPassedAt)
    // --------------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysPassedAt))]
    class M_DaysAt { static void Prefix(ref int gameTicks) => ScaleUtil.DivTicks(ref gameTicks); }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.YearsPassedAt))]
    class M_YearsAt { static void Prefix(ref int gameTicks) => ScaleUtil.DivTicks(ref gameTicks); }

    // --------------------------------------------------------------------
    //  ***  Converters: DaysToTicks, TickGameToAbs, TickAbsToGame, etc.  ***
    //  We *multiply* when converting calendar back INTO ticks.
    // --------------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.DaysToTicks))]
    class M_DaysToTicks
    {
        static void Prefix(ref float days) => days *= ScaleUtil.Mul;
    }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TickGameToAbs))]
    class M_TickGameToAbs
    {
        static void Prefix(ref int gameTick) => gameTick = (int)(gameTick * ScaleUtil.Mul);
    }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TickAbsToGame))]
    class M_TickAbsToGame
    {
        static void Prefix(ref int absTick) => absTick = (int)(absTick / ScaleUtil.Mul);
    }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TickGameToSettled))]
    class M_TickSettled
    {
        static void Prefix(ref int gameTick) => gameTick = (int)(gameTick * ScaleUtil.Mul);
    }

    // --------------------------------------------------------------------
    //  ***  Extension methods  ***  (TicksToDays, TicksToPeriod, etc.)
    // --------------------------------------------------------------------
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TicksToDays))]
    class M_TicksToDays
    {
        static void Prefix(ref int numTicks) => ScaleUtil.DivTicks(ref numTicks);
    }
    /*
    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TicksToPeriod), new[] { typeof(int), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(float).MakeByRefType() })]
    class M_TicksToPeriodInt
    {
        static void Prefix(ref int numTicks) => ScaleUtil.DivTicks(ref numTicks);
    }

    [HarmonyPatch(typeof(GenDate), nameof(GenDate.TicksToPeriod), new[] { typeof(long), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(int).MakeByRefType(), typeof(float).MakeByRefType() })]
    class M_TicksToPeriodLong
    {
        static void Prefix(ref long numTicks) => ScaleUtil.DivTicks(ref numTicks);
    }*/
}
