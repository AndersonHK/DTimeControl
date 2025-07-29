using System.Collections.Generic;
using HarmonyLib;
using Verse.AI;

namespace DTimeControl.Core_Patches;

[HarmonyPatch(typeof(Pawn_MindState), nameof(Pawn_MindState.StartFleeingBecauseOfPawnAction))] //AHK: Was CanStartFleeingBecauseOfPawnAction, BROKEN 1.6
internal class Patch_StartFleeingBecauseOfPawnAction_Transpiler //AHK: Was Patch_CanStartFleeingBecauseOfPawnAction_Transpiler
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions; // return GenericTickReplacer.ReplaceTicks(instructions, "StartFleeingBecauseOfPawnAction"); //AHK: Was CanStartFleeingBecauseOfPawnAction
    }
}