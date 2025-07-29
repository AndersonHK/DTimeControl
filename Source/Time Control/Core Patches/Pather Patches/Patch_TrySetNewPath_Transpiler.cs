using System.Collections.Generic;
using HarmonyLib;
using Verse.AI;

namespace DTimeControl.Core_Patches.Pather_Patches;

[HarmonyPatch(typeof(Pawn_PathFollower), nameof(Pawn_PathFollower.TrySetNewPathRequest))] //AHK: Was TrySetNewPath, BROKEN 1.6
internal class Patch_TrySetNewPath_Transpiler
{
    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return instructions; // return GenericTickReplacer.ReplaceTicks(instructions, "TrySetNewPath");
    }
}