﻿using CustomExpeditions.Messages;
using HarmonyLib;
using LevelGeneration;

namespace CustomExpeditions.Messages.Inject.Global
{
    [HarmonyPatch(typeof(LG_Factory), "FactoryDone")]
    internal class Inject_LG_Factory
    {
        internal static void Postfix()
        {
            Logger.Verbose("Global: OnBuildDone");
            GlobalMessage.OnBuildDone?.Invoke();
            GlobalMessage.OnBuildDoneLate?.Invoke();
        }
    }
}