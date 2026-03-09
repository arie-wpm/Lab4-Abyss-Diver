using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

public static class DebugTool {
    private static HashSet<string> disabledScripts = new HashSet<string>();

    public static void Log<T>(
        T value,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;

        string message = value != null ? value.ToString() : "null";
        Debug.Log($"{className}.{member}(): {message} (line {line})");
    }

    public static void LogWarning<T>(
        T value,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;

        string message = value != null ? value.ToString() : "null";
        Debug.LogWarning($"{className}.{member}(): {message} (line {line})");
    }

    public static void LogError<T>(
        T value,
        [CallerFilePath] string file = "",
        [CallerMemberName] string member = "",
        [CallerLineNumber] int line = 0)
    {
        string className = Path.GetFileNameWithoutExtension(file);
        if (disabledScripts.Contains(className)) return;

        string message = value != null ? value.ToString() : "null";
        Debug.LogError($"{className}.{member}(): {message} (line {line})");
    }

    public static void EnableLogging(string scriptName, bool enable)
    {
        if (enable) disabledScripts.Remove(scriptName);
        else disabledScripts.Add(scriptName);
    }
}