﻿using GTFO.CustomObjectives.Inject;
using LevelGeneration;
using System;

namespace GTFO.CustomObjectives.Utils
{
    public static class TerminalUtil
    {
        public static void AddCommand(LG_ComputerTerminal terminal, string cmdText, string helpText, Action<LG_ComputerTerminal, string, string> onCmdReceived)
        {
            terminal.m_command.AddCommand(TERM_Command.Override, cmdText, helpText);
            Inject_Terminal.AddCustomCommandHandler(cmdText, onCmdReceived);
        }

        public static void AddStandardCommand(LG_ComputerTerminal terminal, string cmdText, string helpText, TERM_Command type)
        {
            terminal.m_command.AddCommand(type, cmdText, helpText);
        }
    }
}