using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

public static class DebugTool 
{
    private static HashSet<string> disabledScripts = new HashSet<string>();

    public static void Log(
        string message,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;
        Debug.Log($"{className}.{member}(): {message} (line {line})");
    }

    public static void LogWarning(
        string message,
        [CallerFilePath] string file = "")
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;
        Debug.LogWarning($"{className}: {message}");
    }

    public static void LogError(
        string message,
        [CallerFilePath] string file = "")
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;
        Debug.LogError($"{className}: {message}");
    }

    public static void EnableLogging(string scriptName, bool enable)
    {
        if (enable) disabledScripts.Remove(scriptName);
        else disabledScripts.Add(scriptName);
    }
}