﻿using CustomExpeditions.Utils;
using UnhollowerBaseLib;

namespace CustomExpeditions.HandlerBase
{
    public class WinConditionProxy
    {
        private readonly CustomExpHandlerBase Base;

        internal WinConditionProxy(CustomExpHandlerBase b)
        {
            Base = b;
        }

        public ObjectiveItem CreateEmptyObjectiveItem()
        {
            var item = new ObjectiveItem(Base.LayerType);

            RegisterObjectiveItemForCollection(item.ItemCast);

            return item;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        public void RegisterObjectiveItem(iWardenObjectiveItem item)
        {
            WardenObjectiveManager.RegisterObjectiveItem(Base.LayerType, item);
        }

        public void RegisterObjectiveItemForCollection(iWardenObjectiveItem item)
        {
            WardenObjectiveManager.RegisterObjectiveItemForCollection(Base.LayerType, item);
        }

        public void FoundObjectiveItem(iWardenObjectiveItem item)
        {
            if (Base.IsDefaultObjective)
            {
                WardenObjectiveManager.OnLocalPlayerFoundObjectiveItem(Base.LayerType, item);
            }
            else
            {
                //TODO: Behaviour for Custom Objective
            }
        }

        public void SolvedObjectiveItem(iWardenObjectiveItem item)
        {
            if (Base.IsDefaultObjective)
            {
                WardenObjectiveManager.OnLocalPlayerSolvedObjectiveItem(Base.LayerType, item);
            }
            else
            {
                //TODO: Behaviour for Custom Objective
            }
        }

        public void RegisterRequiredScanItem(iWardenObjectiveItem item)
        {
            var array = new Il2CppReferenceArray<iWardenObjectiveItem>(1);
            array[0] = item;

            ElevatorShaftLanding.Current.AddRequiredScanItems(array);
        }
    }
}