using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace DTimeControl;

/// <summary>Utility to convert TimeControlSettings.speedMultiplier into true longer days.</summary>
[StaticConstructorOnStartup]
public static class DeltaScalerBootstrap
{
    static DeltaScalerBootstrap()
    {
        new Harmony("io.github.dametri.timecontrol.delta").PatchAll();
    }
}

// 1) Make sure on‑screen pawns tick with delta ≥ ceil(S)
[HarmonyPatch(typeof(Pawn), "get_UpdateRateTicks")]
public static class Patch_UpdateRateTicks
{
    static void Postfix(ref int __result)
    {
        float S = TimeControlSettings.speedMultiplier;
        if (S <= 1f) return;

        int min = Math.Min(15, (int)Math.Ceiling(S));
        if (__result < min) __result = min;
    }
}

// 2) Scale the delta before pawn subsystems use it
[HarmonyPatch(typeof(Pawn), nameof(Pawn.TickInterval))]
public static class Patch_Pawn_TickInterval
{
    private static readonly ConditionalWeakTable<Pawn, Acc> Accu = new();
    private class Acc { public double carry; }

    static void Prefix(ref int delta, Pawn __instance)
    {
        float S = TimeControlSettings.speedMultiplier;
        if (S <= 1f) return;

        var acc = Accu.GetOrCreateValue(__instance);
        double tgt = delta / S + acc.carry;

        int scaled = (int)Math.Floor(tgt);
        acc.carry = tgt - scaled;

        if (scaled < 1)
        {
            scaled = 1;
            acc.carry -= 1.0;
        }
        delta = scaled;
    }
}

// 3) Optional work‑speed patch (honours settings.slowWork)
[HarmonyPatch(typeof(Toils_Ingest), nameof(Toils_Ingest.ChewIngestible))]
public static class Patch_ScaleWork
{
    static void Prefix(ref float durationMultiplier)
    {
        if (!TimeControlSettings.slowWork) return;  // keep vanilla pace if user wants
        float S = TimeControlSettings.speedMultiplier;
        durationMultiplier *= 1/S;
    }
}
