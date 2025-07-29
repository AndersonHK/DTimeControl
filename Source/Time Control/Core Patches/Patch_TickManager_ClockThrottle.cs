using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace DTimeControl
{
    [HarmonyPatch(typeof(TickManager), "DoSingleTick")]
    public static class Patch_TickManager_ClockThrottle
    {
        // integer accumulator: 0 … (divisor‑1)
        private static int remainder;
        private static int divisorCached = 1;
        private static int preTick;

        // ----- prefix --------------------------------------------------------
        [HarmonyPriority(Priority.First)]
        static void Prefix()
        {
            preTick = Find.TickManager.ticksGameInt;   // value before vanilla +1
            // cache divisor so we don’t divide every frame
            divisorCached = Math.Max(1, (int)Math.Round(TimeControlSettings.speedMultiplier));
        }

        // ----- postfix -------------------------------------------------------
        [HarmonyPriority(Priority.Last)]
        static void Postfix()
        {
            if (divisorCached <= 1) return;            // vanilla pace (100 %)

            remainder++;
            if (remainder < divisorCached)
            {
                // cancel this frame’s +1
                Find.TickManager.ticksGameInt = preTick;
            }
            else
            {
                // allow exactly one tick and wrap
                remainder = 0;
            }
        }
    }
}
