#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScriptsGeneratorRoslynTools
{
    private const string MenuRoot = "Scripts Generator/Roslyn/";
    private const string ToolProjectPath = "Tools/ScriptsGenerator.Roslyn/ScriptsGenerator.Roslyn.csproj";

    [MenuItem(MenuRoot + "Format Selected C# File", true)]
    private static bool ValidateFormatSelectedFile()
    {
        return GetSelectedCSharpPath() != null;
    }

    [MenuItem(MenuRoot + "Format Selected C# File")]
    private static void FormatSelectedFile()
    {
        string filePath = GetSelectedCSharpPath();
        if (filePath == null)
        {
            throw new InvalidOperationException("Select a C# file in the Project window.");
        }

        RunRoslynCommand("format", filePath, filePath);
        AssetDatabase.Refresh();
    }

    [MenuItem(MenuRoot + "Validate Selected C# File", true)]
    private static bool ValidateSelectedFile()
    {
        return GetSelectedCSharpPath() != null;
    }

    [MenuItem(MenuRoot + "Validate Selected C# File")]
    private static void ValidateSelectedFileCommand()
    {
        string filePath = GetSelectedCSharpPath();
        if (filePath == null)
        {
            throw new InvalidOperationException("Select a C# file in the Project window.");
        }

        RunRoslynCommand("validate", filePath, null);
    }

    private static string GetSelectedCSharpPath()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(assetPath) || !assetPath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        return Path.GetFullPath(Path.Combine(projectRoot, assetPath));
    }

    private static void RunRoslynCommand(string command, string inputPath, string outputPath)
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string toolProjectPath = Path.Combine(projectRoot, ToolProjectPath);
        string arguments = $"run --project \"{toolProjectPath}\" -- {command} \"{inputPath}\"";

        if (outputPath != null)
        {
            arguments += $" \"{outputPath}\"";
        }

        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = projectRoot,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using Process process = Process.Start(startInfo);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"Roslyn command failed with exit code {process.ExitCode}.\n{error}{output}");
        }

        if (!string.IsNullOrWhiteSpace(output))
        {
            UnityEngine.Debug.Log(output.Trim());
        }
    }
}
#endif
