﻿using CustomExpeditions.Messages;
using LevelGeneration;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExpeditions.CustomReplicators
{
    internal static class CustomReplicatorManager
    {
        public static readonly Dictionary<ushort, ReplicatorInfo> Replicators;

        private const ushort _ManagedIDInitialValue = 15000;
        private static ushort _ManagerIDBuffer = 1;
        private static ushort _ManagedIDBuffer = _ManagedIDInitialValue;

        static CustomReplicatorManager()
        {
            Replicators = new Dictionary<ushort, ReplicatorInfo>();


            GlobalMessage.OnLevelCleanup += () =>
            {
                _ManagedIDBuffer = _ManagedIDInitialValue;


                var idsToRemove = new List<ushort>();
                foreach(var item in Replicators)
                {
                    var replicator = item.Value.Replicator;

                    if(replicator == null)
                        idsToRemove.Add(item.Key);

                    if (replicator.Type == SNet_ReplicatorType.SelfManaged)
                        idsToRemove.Add(item.Key);
                }

                foreach (var id in idsToRemove)
                {
                    Replicators.Remove(id);
                }
            };
        }

        public static void RegisterReplicator(IReplicator replicator, string guid, Action<pDoorState, pDoorState, bool> onStateChanged, out ushort newKey)
        {
            var info = new ReplicatorInfo()
            {
                Replicator = replicator,
                GUID = guid,
                OnStateChanged = onStateChanged
            };

            newKey = 0;
            switch(replicator.Type)
            {
                case SNet_ReplicatorType.Manager:
                    newKey = _ManagerIDBuffer++;
                    replicator.Key = newKey;
                    Replicators.Add(newKey, info);
                    break;

                case SNet_ReplicatorType.SelfManaged:
                    newKey = _ManagedIDBuffer++;
                    replicator.Key = newKey;
                    Replicators.Add(newKey, info);
                    break;
            }
        }

        public static bool TryGetReplicator(int id, out IReplicator replicator)
        {
            var result = Replicators.TryGetValue((ushort)id, out var info);
            replicator = info.Replicator;
            return result;
        }

        public static bool TryInvokeStateChange(string guid, pDoorState oldState, pDoorState newState, bool recall)
        {
            try
            {
                var info = Replicators.Single(i => i.Value.GUID.Equals(guid)).Value;
                info.OnStateChanged?.Invoke(oldState, newState, recall);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
