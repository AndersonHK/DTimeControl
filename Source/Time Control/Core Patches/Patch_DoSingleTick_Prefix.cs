using HarmonyLib;
using Verse;

namespace DTimeControl.Core_Patches;

/// <summary>Redirects every vanilla game‑tick into Time Control’s scheduler.</summary>
[HarmonyPatch(typeof(TickManager), nameof(TickManager.DoSingleTick))]
internal static class Patch_DoSingleTick
{
    /// <remarks>
    /// RimWorld 1.6 keeps the parameter‑less   <c>void DoSingleTick()</c>.  
    /// Our mod still wants a <paramref name="delta"/> value, so we just
    /// forward <c>1</c> each time.
    /// </remarks>
    private static bool Prefix(TickManager __instance)
    {
        TimeControlBase.TickManagerTick(__instance, 1, true);
        return false;                       // skip original implementation
    }
}