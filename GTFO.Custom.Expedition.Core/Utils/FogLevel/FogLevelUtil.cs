﻿using CustomExpeditions.CustomReplicators;
using CustomExpeditions.Messages;
using GameData;
using SNetwork;
using System.Collections.Generic;

namespace CustomExpeditions.Utils
{
    public static class FogLevelUtil
    {
        private static bool _IsInGame = false;
        private static float _EndTime = 0.0f;
        private readonly static List<FogLevelSetting> _FogSettings;
        private readonly static FogLevelReplicator _Replicator;

        public static int CurrentLevel { get; private set; }
        public static int LevelCount { get; private set; }

        static FogLevelUtil()
        {
            _Replicator = new FogLevelReplicator();
            _FogSettings = new List<FogLevelSetting>();

            _Replicator.OnLevelChanged += OnLevelChanged;

            GlobalMessage.OnBuildDoneLate += () =>
            {
                GlobalMessage.OnUpdate += OnUpdate;
                _IsInGame = true;
            };

            GlobalMessage.OnLevelCleanup += () =>
            {
                GlobalMessage.OnUpdate -= OnUpdate;
                _IsInGame = false;
            };
        }

        public static void Setup() //We need it since Replicator heavily relys on Creation order.
        {
            _Replicator.Setup(ReplicatorType.Manager, ReplicatorCHType.GameOrderCritical);
        }

        private static void OnLevelChanged(int level)
        {
            if (!(0 <= level && level < LevelCount))
            {
                return;
            }

            CurrentLevel = level;
            _EndTime = Clock.Time + _FogSettings[CurrentLevel].TransitionTime;

            var block = GameDataBlockBase<FogSettingsDataBlock>.GetBlock(_FogSettings[CurrentLevel].FogSetting);
            LocalPlayerAgentSettings.Current.SetTargetFogSettings(block);
        }

        private static void OnUpdate()
        {
            if (!_IsInGame)
                return;

            return; //TODO: Wip

            var remainingTime = _EndTime - Clock.TimeInSessionHub;
            var percent = 1.0f - (remainingTime / _FogSettings[CurrentLevel].TransitionTime);

            LocalPlayerAgentSettings.Current.UpdateBlendTowardsTargetFogSetting(percent, true);

            if (percent >= 1.0f)
            {
            }
        }

        //TODO: Allow user to jump without increament or decreament(ex: 1 -> 3)
        public static void SetLevel(int level)
        {
            if (!(0 <= level && level < LevelCount))
            {
                return;
            }
        }
    }
}