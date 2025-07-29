using HarmonyLib;
using RimWorld;

namespace DTimeControl.Core_Patches.Pawn_Timer_Adjustments;

[HarmonyPatch(typeof(Pawn_GuestTracker), nameof(Pawn_GuestTracker.GuestTrackerTickInterval))] //AHK: Was GuestTrackerTick, BROKEN 1.6
internal class Patch_GuestTrackerTick_Prefix
{
    public static bool Prefix()
    {
        return !(TimeControlBase.partialTick < 1.0);
    }
}