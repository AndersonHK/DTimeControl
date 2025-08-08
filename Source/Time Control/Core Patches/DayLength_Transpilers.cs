/*using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DTimeControl
{
    [StaticConstructorOnStartup]
    public static class DayLengthTranspilers
    {
        static DayLengthTranspilers()
        {
            new Harmony("io.dtimecontrol.daylength.il").PatchAll();
        }

        private static float Mult => TimeControlSettings.speedMultiplier;

        // ------------------------------------------------------------------
        //  Transpiler helper: replace 60000 / 2500 with scaled value
        // ------------------------------------------------------------------
        private static IEnumerable<CodeInstruction> ReplaceConst(
            IEnumerable<CodeInstruction> orig, float literal)
        {
            foreach (var ci in orig)
            {
                if (ci.opcode == OpCodes.Ldc_R4 && (float)ci.operand == literal)
                    yield return new CodeInstruction(OpCodes.Ldc_R4, literal * Mult);
                else if (ci.opcode == OpCodes.Ldc_I4 && (int)ci.operand == (int)literal)
                    yield return new CodeInstruction(OpCodes.Ldc_I4, (int)(literal * Mult));
                else
                    yield return ci;
            }
        }

        // ------------------------------------------------------------------
        //  1)  GenDate.DayTick(long absTicks, float longitude)
        //      constants: 60000
        // ------------------------------------------------------------------
        [HarmonyPatch(typeof(GenDate), nameof(GenDate.DayTick))]
        static class T_DayTick
        {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> instr)
                    => ReplaceConst(instr, 60000f);
        }

        // ------------------------------------------------------------------
        //  2)  GenDate.HourFloat(long absTicks, float longitude)
        //      constants: 2500
        // ------------------------------------------------------------------
        [HarmonyPatch(typeof(GenDate), nameof(GenDate.HourFloat))]
        static class T_HourFloat
        {
            static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> instr)
                    => ReplaceConst(instr, 2500f);
        }
    }
}
*/