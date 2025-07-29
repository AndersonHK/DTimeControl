using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace DTimeControl;

[StaticConstructorOnStartup]
public static class TickUtility
{
    public static TickManager tickManager;

    public static TickList tickListNormal;
    public static TickList tickListRare;
    public static TickList tickListLong;

    public static readonly Type FilthMonitor;
    public static readonly MethodInfo fmt;

    public static int adjustedTicksGameInt = 0;

    static TickUtility()
    {
        FilthMonitor = AccessTools.TypeByName("RimWorld.FilthMonitor");
        fmt = AccessTools.Method(FilthMonitor, "FilthMonitorTick");
    }

    public static int ticksGameInt
    {
        get => tickManager.TicksGame;
        set => tickManager.ticksGameInt = value;
    }

    public static int AdjustedTicksGame => adjustedTicksGameInt;

    public static int lastAutoScreenshot
    {
        get => tickManager.lastAutoScreenshot;
        set => tickManager.lastAutoScreenshot = value;
    }


    public static bool AdjustedIsHashIntervalTick(this Thing t, int interval)
    {
        return t.AdjustedHashOffsetTicks() % interval == 0;
    }

    public static int AdjustedHashOffsetTicks(this Thing t)
    {
        return adjustedTicksGameInt + t.thingIDNumber.HashOffset();
    }

    public static bool NoOverlapAdjustedIsHashIntervalTick(this Thing t, int interval)
    {
        return t.AdjustedIsHashIntervalTick(interval) && !t.IsHashIntervalTick(interval);
    }

    public static bool NoOverlapTickMod(int interval)
    {
        return adjustedTicksGameInt % interval == 0 && ticksGameInt % interval != 0;
    }

    public static void FilthMonitorTick()
    {
        fmt.Invoke(null, null);
    }

    public static void GetManagerData(Game currentGame)
    {
        tickManager = currentGame.tickManager;

        tickListNormal = currentGame.tickManager.tickListNormal;
        tickListRare = currentGame.tickManager.tickListRare;
        tickListLong = currentGame.tickManager.tickListLong;

        if (tickListNormal == null)
            Log.Warning("[D] Time Control: vanilla TickLists captured (custom lists removed).");
    }
}