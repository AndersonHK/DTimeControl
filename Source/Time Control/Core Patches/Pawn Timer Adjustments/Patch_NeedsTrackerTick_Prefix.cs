using HarmonyLib;
using RimWorld;

namespace DTimeControl.Core_Patches.Pawn_Timer_Adjustments;

[HarmonyPatch(typeof(Pawn_NeedsTracker), nameof(Pawn_NeedsTracker.NeedsTrackerTickInterval))] //AHK: Was NeedsTrackerTick, BROKEN 1.6
internal class Patch_NeedsTrackerTick_Prefix
{
    public static bool Prefix()
    {
        return !(TimeControlBase.partialTick < 1.0) || !TimeControlSettings.scalePawns;
    }
}