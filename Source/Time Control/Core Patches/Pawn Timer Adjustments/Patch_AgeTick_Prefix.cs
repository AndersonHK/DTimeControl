using HarmonyLib;
using Verse;

namespace DTimeControl.Core_Patches;

[HarmonyPatch(typeof(Pawn_AgeTracker), nameof(Pawn_AgeTracker.AgeTickInterval))] //AHK: Was AgeTick, BROKEN 1.6
internal class Patch_AgeTick_Prefix
{
    public static bool Prefix()
    {
        return !(TimeControlBase.partialTick < 1.0) || !TimeControlSettings.scalePawns;
    }
}