﻿using CustomExpeditions.Messages;
using LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomExpeditions.Messages.Inject.MarkerItem
{
    internal class Inject_LG_GateKeyItemBuilder
    {
        internal static void Postfix(LG_GateKeyMarkerBuilder __instance, LG_LayerType layer, GameObject GO)
        {
            var casted = __instance.Cast<LG_FunctionMarkerBuilder>();
            var result = Inject_LG_FunctionMarkerBuilder.TryGetGUID(casted, out var guid);
            if (result)
            {
                ItemMessage.OnItemSpawned?.Invoke(guid, GO);
                ItemMessage.OnGateKeyItemSpawned?.Invoke(guid, GO);
            }
        }
    }
}
