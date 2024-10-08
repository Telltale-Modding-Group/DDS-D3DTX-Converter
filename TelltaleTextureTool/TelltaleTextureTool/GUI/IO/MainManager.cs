using TelltaleTextureTool.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using System;
using System.Runtime.InteropServices;

namespace TelltaleTextureTool;

/// <summary>
/// Handles main application functions and activity.
/// MainManager is a singleton, meaning only one instance of the class exists.
/// </summary>
public sealed class MainManager
{
    // App version
    public readonly string AppVersion = "v2.5.0";

    // Weblink for getting help with the application
    private const string AppHelpLink = "https://github.com/iMrShadow/DDS-D3DTX-Converter/blob/main/wiki/home.md";

    private WorkingDirectory _workingDirectory;
    private static MainManager? _instance;

    private MainManager()
    {
        //create the rest of our objects
        _workingDirectory = new WorkingDirectory();
    }

    public static MainManager GetInstance() => _instance ??= new MainManager();

    /// <summary>
    /// Sets the current working directory path using a folder picker.
    /// </summary>
    /// <param name="provider"></param>
    public async Task SetWorkingDirectoryPath(IStorageProvider provider)
    {
        string path = await IOManagement.GetFilePathAsync(provider,
            "Locate your folder containing your extracted .d3dtx textures.");

        if (string.IsNullOrEmpty(path))
            return;

        _workingDirectory.GetFiles(path);
    }

    /// <summary>
    /// Sets the current working directory path with the provided path.
    /// </summary>
    /// <param name="path"></param>
    public Task SetWorkingDirectoryPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return Task.CompletedTask;

        _workingDirectory.GetFiles(path);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Opens a file using its preferred software.
    /// </summary>
    /// <param name="directoryPath"></param>
    public void OpenFile(string? directoryPath)
    {
        if (string.IsNullOrEmpty(directoryPath))
            return;

        //create a windows explorer processInfo to be executed
        ProcessStartInfo processStartInfo = new()
        {
            FileName = directoryPath,
            UseShellExecute = true,
            Verb = "open"
        };

        //start the process
        Process.Start(processStartInfo);
    }

    /// <summary>
    /// Opens the file explorer.
    /// </summary>
    /// <param name="directoryPath"></param>
    public static void OpenFileExplorer(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Process.Start("explorer.exe", $"/select,\"{filePath}\"");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", $"-R \"{filePath}\"");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", $"\"{Path.GetDirectoryName(filePath)}\"");
        }
        else
        {
            throw new NotSupportedException("Unknown operating system.");
        }
    }

    /// <summary>
    /// Opens the default web explorer and directs the user to the help page.
    /// </summary>
    public void OpenAppHelp()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = AppHelpLink,
            UseShellExecute = true
        };

        Process.Start(processStartInfo);
    }

    /// <summary>
    /// Refreshes the current directory, in case the user adds or deletes files using other software.
    /// </summary>
    public void RefreshWorkingDirectory()
    {
        _workingDirectory.GetFiles(_workingDirectory.WorkingDirectoryPath);
    }

    public bool WorkingDirectory_Path_Exists() => Directory.Exists(_workingDirectory.WorkingDirectoryPath);

    public string GetWorkingDirectoryPath() => _workingDirectory.WorkingDirectoryPath;

    public List<WorkingDirectoryFile> GetWorkingDirectoryFiles() => _workingDirectory.WorkingDirectoryFiles;
}