using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace DTimeControl
{
    [StaticConstructorOnStartup]
    public static class DayLengthScaler
    {
        static DayLengthScaler()
        {
            var harm = new Harmony("io.dtimecontrol.daylength");
            harm.PatchAll();
        }

        // --------------------------------------------------------------------
        // 1)  Change GenDate constants at game start
        // --------------------------------------------------------------------
        [HarmonyPatch(typeof(Verse.Root), "Start")]
        [HarmonyPriority(Priority.First)]
        public static class Patch_Root_Start
        {
            static void Prefix()
            {
                float S = TimeControlSettings.speedMultiplier;   // 100 % = 1.0

                Type gd = typeof(GenDate);

                void scale(string name, float mult)
                {
                    FieldInfo f = AccessTools.Field(gd, name);
                    int val = (int)f.GetRawConstantValue();
                    f.SetValue(null, (int)Math.Round(val * mult));
                }

                //scale(nameof(GenDate.TicksPerMinute), S); doesn't exist
                scale(nameof(GenDate.TicksPerHour), S);
                scale(nameof(GenDate.TicksPerDay), S);
                scale(nameof(GenDate.TicksPerQuadrum), S);
                scale(nameof(GenDate.TicksPerYear), S);

                Log.Message($"[D] Time Control: day length scaled ×{S:0.##}");
            }
        }

        // --------------------------------------------------------------------
        // 2)  After a save finishes loading, rescale ticksGameInt so
        //     the calendar date doesn’t jump.
        // --------------------------------------------------------------------
        [HarmonyPatch(typeof(GameComponentUtility), nameof(GameComponentUtility.LoadedGame))]
        public static class Patch_AfterLoadedGame
        {
            static void Postfix()
            {
                float S = TimeControlSettings.speedMultiplier;

                TickManager tm = Find.TickManager;
                long newTicks = (long)(tm.TicksGame * S);
                tm.ticksGameInt = (int)newTicks; // okay up to year ≈ 7 000

                Type gd = typeof(GenDate);

                void scale(string name, float mult)
                {
                    FieldInfo f = AccessTools.Field(gd, name);
                    int val = (int)f.GetRawConstantValue();
                    f.SetValue(null, (int)Math.Round(val * mult));
                }

                //scale(nameof(GenDate.TicksPerMinute), S); doesn't exist
                scale(nameof(GenDate.TicksPerHour), S);
                scale(nameof(GenDate.TicksPerDay), S);
                scale(nameof(GenDate.TicksPerQuadrum), S);
                scale(nameof(GenDate.TicksPerYear), S);

                Log.Message($"[D] Time Control: ticksGameInt rescaled to {newTicks}");
            }
        }
    }
}
