﻿using CustomObjective.DoorMultipleWaves.DoorWaves;
using CustomObjective.DoorMultipleWaves.PuzzleContext;
using GameData;
using GTFO.CustomObjectives;
using GTFO.CustomObjectives.Utils;
using LevelGeneration;
using SNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomObjective.DoorMultipleWaves
{
    public class MainHandler : CustomObjectiveHandlerBase
    {
        public override void OnSetup()
        {
            if(this.LayerType != LG_LayerType.MainLayer)
            {
                UnloadSelf();
                return;
            }

            DoorWaveManager.Setup();

            var manager = DoorWaveManager.Current;

            int count = ObjectiveData.ReactorWaves.Count;
            manager.Waves = new DoorWaveData[count];
            manager.MainZone = ObjectiveLayerData.ZonePlacementDatas[0][0].LocalIndex;

            for (int i = 0; i < count; i++)
            {
                var ReactWaveData = ObjectiveData.ReactorWaves[i];

                manager.Waves[i] = new DoorWaveData()
                {
                    PuzzleID = ReactWaveData.EnemyWaves[0].WaveSettings,
                    EventData = ReactWaveData.Events.ToArray(),

                    Time_Init = ReactWaveData.Warmup,
                    Time_Search = ReactWaveData.Verify,

                    Time_FailInit = ReactWaveData.WarmupFail,
                    Time_FailSearch = ReactWaveData.VerifyFail,

                    GeneratedFileName = $"autogen_{SerialGenerator.GetCodeWordPrefix()}.key",
                    GeneratedFileContent = $"//Generated Key file for BlockCipher:\n\n{GetBase64(GetBase64(GetBase64(SomeBigLongText())))}",
                    TerminalZone = ReactWaveData.ZoneForVerification
                };
            }

            RegisterUpdateEvent(update);
        }

        public void update()
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                DoorWaveManager.Current.AttemptInteract(DoorWaveInteractionType.Complete);
            }

            if(Input.GetKeyDown(KeyCode.F2))
            {
                DoorWaveManager.Current.AttemptInteract(DoorWaveInteractionType.FailedVerify);
            }
        }

        public override void OnBuildDone()
        {
            /*
            int count = DoorWaveManager.Waves.Length;

            var doorZone = PlacementUtil.GetZone(LayerType, DoorWaveManager.MainZone);
            DoorWaveManager.ChainedDoor = PlacementUtil.GetSpawnedDoor(doorZone);

            for (int i = 0; i < count; i++)
            {
                var waveData = DoorWaveManager.Waves[i];
                if(!Layer.m_zonesByLocalIndex.TryGetValue(waveData.TerminalZone, out var zone))
                {
                    continue;
                }

                var terminal = RandomUtil.PickFromList(zone.TerminalsSpawnedInZone);
                if(terminal != null)
                {
                    waveData.Terminal = terminal;

                    terminal.AddLocalLog(new TerminalLogFileData()
                    {
                        FileName = waveData.GeneratedFileName,
                        FileContent = waveData.GeneratedFileContent
                    });

                    TerminalUtil.AddCommand(terminal, "MASTERDOOR_PUSH_PUBLIC_KEY", "Push public key by filename.", DoorCommandHandler);
                }
            }

            if (SNet.IsMaster)
                DoorWaveManager.Current.AttemptInteract(DoorWaveInteractionType.Startup);
            */
        }

        public void DoorCommandHandler(LG_ComputerTerminal terminal, string param1, string param2)
        {
            /*
            var cmd = terminal.m_command;
            if (string.IsNullOrEmpty(param1))
            {
                cmd.AddOutput("Argument length mismatching!", true);
                cmd.AddOutput("Command Usage: MASTERDOOR_PUSH_PUBLIC_KEY -File <FileName>", true);
                return;
            }

            if(!param1.ToLower().Equals("-file"))
            {
                cmd.AddOutput($"There is no such argument named: '{param1.ToUpper()}'", true);
                cmd.AddOutput("Command Usage: MASTERDOOR_PUSH_PUBLIC_KEY -File <FileName>", true);
                return;
            }

            if(terminal.m_localLogs.TryGetValue(param2.ToUpper(), out var data))
            {
                cmd.AddOutput(TerminalLineType.SpinningWaitNoDone, "Opening connection towards Master Door Controller", 4f);

                if (terminal != DoorWaveManager.CurrentWave.Terminal)
                {
                    cmd.AddOutput("", true);
                    cmd.AddOutput(TerminalLineType.Warning, "ACCESS DENIED :: Push URL is blocked from Door Controller. Try again with other allowed Terminal.", 0.0f);
                    return;
                }

                cmd.AddOutput(TerminalLineType.ProgressWait, "Pushing Key file content", 6.5f);
                cmd.AddOutput(TerminalLineType.SpinningWaitDone, "Waiting for Controllers respond", 3f);

                if (!DoorWaveManager.CurrentWave.GeneratedFileName.ToUpper().Equals(param2.ToUpper()))
                {
                    cmd.AddOutput(TerminalLineType.Warning, "ACCESS DENIED :: Given Key file was not correct!", 1.0f);
                    return;
                }

                cmd.AddOutput(TerminalLineType.ProgressWait, "Parsing the Controllers respond", 1.0f);
                cmd.AddOutput("", true);
                if(GameDataBlockBase<ChainedPuzzleDataBlock>.HasBlock(ObjectiveData.ChainedPuzzleMidObjective))
                {
                    var puzzle = ChainedPuzzleUtil.Setup<TerminalPuzzleContext>(
                        ObjectiveData.ChainedPuzzleMidObjective,
                        terminal.SpawnNode.m_area,
                        terminal.m_wardenObjectiveSecurityScanAlign);

                    puzzle.Terminal = terminal;
                    puzzle.Handler = this;

                    if (puzzle.Instance.Data.TriggerAlarmOnActivate)
                    {
                        cmd.AddOutput(TerminalLineType.Warning, $"Security Scan with [{puzzle.Instance.Data.PublicAlarmName}] is required!");
                    }
                    else
                    {
                        cmd.AddOutput("Security Scan is required!", true);
                    }
                    cmd.AddOutput("", true);
                    cmd.OnEndOfQueue = new Action(() =>
                    {
                        puzzle.Trigger();
                    });
                }
                else
                {
                    cmd.AddOutput("SUCCESS! Master Door's Security Scan will be resume in few moment...");
                    cmd.OnEndOfQueue = new Action(() =>
                    {
                        DoorWaveManager.JumpToNextWave();
                    });
                }
            }
            else
            {
                cmd.AddOutput($"There is no such file named: '{param2.ToUpper()}'", true);
                return;
            }
            */
        }

        public void OnPushSuccess()
        {

        }

        public string SomeBigLongText()
        {
            var builder = new StringBuilder();
            builder.Append("random code is: ");
            builder.Append(SerialGenerator.GetCodeWord());
            builder.AppendLine(", your welcome :)");

            builder.AppendLine("Btw did you realized this is base64 strings huh?");

            builder.AppendLine("Congratulation you actually figured out!");

            builder.AppendLine("Now go back to the game.");

            return builder.ToString();
        }

        public string GetBase64(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
    }
}