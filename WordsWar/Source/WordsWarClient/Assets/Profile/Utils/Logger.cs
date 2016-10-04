using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Logger
{
    public enum LogLevels
    {
        Error   = 0,
        Info    = 1,
        Verbose = 2
    }

    public static LogLevels LogLevel = LogLevels.Error;
    private static void Log(LogLevels level, string message, params object[] args)
    {
        if (level >= LogLevel)
        {
            Debug.Log(FormatLogMessage(message, args));
        }
    }

    private static string FormatLogMessage(string format, params object[] args)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("LOGGER [{0}] - {1}", DateTime.Now.ToString("O"), string.Format(format, args));
        return sb.ToString();
    }

    public static void Error(string message, params object[] args)
    {
        Logger.Log(LogLevels.Error, message, args);
    }

    public static void Info(string message, params object[] args)
    {
        Logger.Log(LogLevels.Info, message, args);
    }

    public static void Verbose(string message, params object[] args)
    {
        Logger.Log(LogLevels.Verbose, message, args);
    }

}
