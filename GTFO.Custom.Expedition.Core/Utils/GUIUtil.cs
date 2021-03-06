﻿namespace CustomExpeditions.Utils
{
    public static class GUIUtil
    {
        public static void SetMessage(string message, ePUIMessageStyle style = ePUIMessageStyle.BioscanAlarm, int priority = -1)
        {
            GuiManager.InteractionLayer.SetMessage(message, style, priority);
        }

        public static void SetTimer(float fillPercent)
        {
            GuiManager.InteractionLayer.SetMessageTimer(fillPercent);
        }

        public static void SetMessageVisible(bool visible)
        {
            GuiManager.InteractionLayer.MessageVisible = visible;
        }

        public static void SetTimerVisible(bool visible)
        {
            GuiManager.InteractionLayer.MessageTimerVisible = visible;
        }

        public static void SetTimedMessage(string message, float duration, ePUIMessageStyle style = ePUIMessageStyle.Default, int priority = -1)
        {
            GuiManager.InteractionLayer.SetTimedMessage(message, duration, style, priority);
        }
    }
}