﻿using Harmony;
using MelonLoader;

namespace GTFO.CustomObjectives.Inject.Global
{
    [HarmonyPatch(typeof(GS_StopElevatorRide), "OnElevatorHasArrived")]
    internal static class Inject_GS_Elevator
    {
        internal static void Postfix()
        {
            MelonLogger.Log("Global: OnElevatorArrive");
            GlobalMessage.OnElevatorArrive?.Invoke();
        }
    }
}