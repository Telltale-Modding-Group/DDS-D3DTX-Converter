using D3DTX_Converter.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using DDS_D3DTX_Converter.Views;


namespace DDS_D3DTX_Converter
{
    /// <summary>
    /// Handles main application functions and activity.
    /// MainManager is a singleton, meaning only one instance of the class exists.
    /// </summary>
    public sealed class MainManager
    {
        //note to self - move the converter script and functionality to a background worker to do 'work' on a different thread so we don't freeze the UI thread

        //app version (not used at the moment)
        public readonly string AppVersion = "v2.0.0";

        //web link for getting help with the application
        private const string AppHelpLink = "https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki";

        //our private objects
        private WorkingDirectory _workingDirectory;
        private static MainManager? _instance;

        private MainManager()
        {
            //create the rest of our objects
            _workingDirectory = new WorkingDirectory();
        }

        public static MainManager GetInstance()
        {
            return _instance ??= new MainManager();
        }

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
        /// Returns the output directory path using a folder picker.
        /// </summary>
        /// <param name="provider"></param>
        public async Task<string> GetOutputDirectoryPath(IStorageProvider provider)
        {
            string path = await IOManagement.GetFilePathAsync(provider,
                "Choose your output folder location.");

            return path;
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
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = directoryPath,
                UseShellExecute = true,
                Verb = "open"
            };

            //start the process
            Process.Start(processStartInfo);
        }

        /// <summary>
        /// Opens the file explorer
        /// (TODO Currently only Windows is supported)
        /// </summary>
        /// <param name="directoryPath"></param>
        public void OpenFileExplorerDirectory(string? directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                return;

            //create a windows explorer processInfo to be executed
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = directoryPath,
                UseShellExecute = true,
                Verb = "open"
            };

            //start the process
            Process.Start(processStartInfo);
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

        // public bool CanConvertTo_DDS() => IOManagement.GetFilesPathsByExtension(_workingDirectory.workingDirectoryPath, ".d3dtx").Count > 0;

        //public bool CanConvertTo_D3DTX() => IOManagement.GetFilesPathsByExtension(_workingDirectory.workingDirectoryPath, ".dds").Count > 0 && IOManagement.GetFilesPathsByExtension(_workingDirectory.workingDirectoryPath, ".header").Count > 0;

        public string GetWorkingDirectoryPath() => _workingDirectory.WorkingDirectoryPath;

        public List<WorkingDirectoryFile> GetWorkingDirectoryFiles() => _workingDirectory.WorkingDirectoryFiles;
    }
}