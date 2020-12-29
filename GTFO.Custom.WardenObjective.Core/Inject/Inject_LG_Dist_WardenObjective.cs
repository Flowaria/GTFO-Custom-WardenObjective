﻿using Harmony;
using LevelGeneration;
using System;

namespace GTFO.CustomObjectives.Inject
{
    [HarmonyPatch(typeof(LG_Distribute_WardenObjective), "Build")]
    internal static class Inject_LG_Dist_WardenObjective
    {
        internal static void Postfix(LG_Distribute_WardenObjective __instance)
        {
            if (__instance.m_dataBlockData == null)
            {
                //We don't have datablock data, Can't do anything :(
                return;
            }

            var type = (byte)__instance.m_dataBlockData.Type;
            if (!Enum.IsDefined(typeof(eWardenObjectiveType), type))
            {
                //Custom Handler!
                CustomObjectiveManager.FireHandler(type, __instance.m_layer, __instance.m_dataBlockData);
            }

            CustomObjectiveManager.FireAllGlobalHandler(__instance.m_layer, __instance.m_dataBlockData);
        }
    }
}