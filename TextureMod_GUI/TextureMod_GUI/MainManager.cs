﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using D3DTX_TextureConverter.Utilities;

namespace TextureMod_GUI
{
    /// <summary>
    /// Handles main application functions and activity
    /// </summary>
    public class MainManager
    {
        //note to self - move the converter script and functionality to a background worker to do 'work' on a different thread so we don't freeze the UI thread

        //app version
        public readonly string appVersion = "v2.0.0";

        //web link for getting help with the application
        public readonly string appHelp_link = "https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki";

        //our private objects
        private Converter converter;
        private WorkingDirectory workingDirectory;
        private MainWindow mainWindow;

        public MainManager(MainWindow mainWindow, ConsoleWriter consoleWriter)
        {
            //get our main window ui
            this.mainWindow = mainWindow;

            //create the rest of our objects
            workingDirectory = new WorkingDirectory();
            converter = new Converter(workingDirectory, consoleWriter);
        }

        /// <summary>
        /// Calls the main method to convert a D3DTX to DDS 
        /// </summary>
        public void ConvertToDDS()
        {
            //call the main function
            converter.App_Convert_D3DTX_Mode();

            //after we finish converting, delete the original .d3dtx files
            foreach (string path in IOManagement.GetFilesPathsByExtension(workingDirectory.workingDirectoryPath, ".d3dtx"))
            {
                IOManagement.DeleteFile(path);
            }

            //refresh the directory
            Refresh_WorkingDirectory();

            //update our UI
            mainWindow.UpdateUI();
        }

        /// <summary>
        /// Calls the main method to convert a DDS to D3DTX
        /// </summary>
        public void ConvertToD3DTX()
        {
            //create a temp variable for our user selected path
            //string path = "";

            //open a folder browser dialog
            //IOManagement.GetFolderPath(ref path, "Select a folder where the converted d3dtx will be stored.");

            //if the user didn't select anything, the path will be null and therefore they have cancled the action, so don't continue
            //if (string.IsNullOrEmpty(path))
            //    return;

            //if they selected a folder, call the main function for converting the textures
            converter.App_Convert_DDS_Mode();

            //after conversion
            //create a windows explorer processinfo and we will open the final path where the fianl converted textures are.
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = workingDirectory.workingDirectoryPath;
            processStartInfo.UseShellExecute = true;
            processStartInfo.Verb = "open";

            //start the process
            Process.Start(processStartInfo);

            //refresh the directory
            Refresh_WorkingDirectory();

            //update our UI
            mainWindow.UpdateUI();
        }

        public void Set_WorkingDirectory_Path()
        {
            string path = "";

            IOManagement.GetFolderPath(ref path, "Locate your folder containg your extracted .d3dtx textures.");

            if (string.IsNullOrEmpty(path))
                return;

            workingDirectory.GetFiles(path);

            mainWindow.UpdateUI();
        }

        public void WorkingDirectory_OpenFileExplorer()
        {
            if (workingDirectory == null || String.IsNullOrEmpty(workingDirectory.workingDirectoryPath))
                return;

            //create a windows explorer processinfo to be exectued
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = workingDirectory.workingDirectoryPath;
            processStartInfo.UseShellExecute = true;
            processStartInfo.Verb = "open";

            //start the process
            Process.Start(processStartInfo);
        }

        /// <summary>
        /// Opens up the default web explorer and directs the user to the help page
        /// </summary>
        public void Open_AppHelp()
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = appHelp_link,
                UseShellExecute = true
            };

            Process.Start(processStartInfo);
        }

        public void Refresh_WorkingDirectory()
        {
            workingDirectory.GetFiles(workingDirectory.workingDirectoryPath);
        }

        public bool WorkingDirectory_Path_Exists() => Directory.Exists(workingDirectory.workingDirectoryPath);

        public bool CanConvertTo_DDS() => IOManagement.GetFilesPathsByExtension(workingDirectory.workingDirectoryPath, ".d3dtx").Count > 0;

        public bool CanConvertTo_D3DTX() => IOManagement.GetFilesPathsByExtension(workingDirectory.workingDirectoryPath, ".dds").Count > 0 && IOManagement.GetFilesPathsByExtension(workingDirectory.workingDirectoryPath, ".header").Count > 0;

        public string Get_WorkingDirectory_Path() => workingDirectory.workingDirectoryPath;

        public List<WorkingDirectory_File> Get_WorkingDirectory_Files() => workingDirectory.workingDirectory_files;
    }
}
