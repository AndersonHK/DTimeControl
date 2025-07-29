using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace DTimeControl;

[StaticConstructorOnStartup]
public static class TickUtility
{
    public static TickManager tickManager;

    public static TCTickList tickListNormal;
    public static TCTickList tickListRare;
    public static TCTickList tickListLong;

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

        /* AHK: this was giving an error in 1.6 when loading a game:
         #####
            System.InvalidCastException: Specified cast is not valid.
            [Ref DEDBEDF4]
              at DTimeControl.TickUtility.GetManagerData (Verse.Game currentGame) [0x0000b] in <e0856f1ff75744a58dffabca825a1466>:0 
              at DTimeControl.TimeControlGameComponent.LoadedGame () [0x00000] in <e0856f1ff75744a58dffabca825a1466>:0 
              at Verse.GameComponentUtility.LoadedGame () [0x0001a] in <ed371ab4349b419183d9be3af652e6dc>:0 
                - POSTFIX com.yayo.yayoAni: Void YayoAnimation.HarmonyPatches.GameComponentUtilityPatch+ResetOnStartedOrLoaded:Postfix()
                - POSTFIX SmashPhil.VehicleFramework: Void SmashTools.GameEvent:RaiseOnLoadGame()
                - POSTFIX smashphil.updatelog: Void UpdateLogTool.UpdateHandler:UpdateOnLoadedGame()
        #####
        So I changed the cast to use the as operator, which returns null if the cast fails.
        tickListNormal = (TCTickList)tickManager.tickListNormal;
        tickListRare = (TCTickList)tickManager.tickListRare;
        tickListLong = (TCTickList)tickManager.tickListLong;
        */

        tickListNormal = tickManager.tickListNormal as TCTickList;
        tickListRare = tickManager.tickListRare as TCTickList;
        tickListLong = tickManager.tickListLong as TCTickList;

        if (tickListNormal == null)
        {
            // We’re on 1.6 and the lists were not swapped.  Log once and continue.
            Log.Message("[D] Time Control: custom TickLists not installed – "
                      + "falling back to vanilla lists.");
        }
    }
}