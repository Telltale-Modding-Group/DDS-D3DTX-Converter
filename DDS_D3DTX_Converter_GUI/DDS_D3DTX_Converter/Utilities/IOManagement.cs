using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace D3DTX_Converter.Utilities
{
    public static class IOManagement
    {
        /// <summary>
        /// Filters an array of files by provided file extension so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="filterExtension"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files, string filterExtension)
        {
            //the new filtered list
            var filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                if (extension.Equals(filterExtension))
                {
                    //add the matched extension to the list
                    filteredFiles.Add(file);
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Filters an array of files by provided file extensions so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="filterExtensions"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files, string[] filterExtensions)
        {
            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                foreach (var t in filterExtensions)
                {
                    if (extension.Equals(t))
                    {
                        //add the matched extension to the list
                        filteredFiles.Add(file);
                    }
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Opens a FolderBrowserDialog for the user to select a folder path and return it.
        /// </summary>
        /// <param name="dialogTitle"></param>
        public static async Task<string> GetFolderPathAsync(string dialogTitle = "Select a Folder Path")
        {
            // Create a TaskCompletionSource to handle the result of the folder dialog
            var tcs = new TaskCompletionSource<string>();

            // Run on the UI thread
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                // Create an instance of the OpenFolderDialog
                var openFolderDialog = new OpenFolderDialog
                {
                    Title = dialogTitle
                };

                // Open the dialog and cache the result
                var result = await openFolderDialog.ShowAsync(null);

                // If the user selects a folder, set the result
                tcs.SetResult(!string.IsNullOrWhiteSpace(result) ? result : null);
            });

            // Wait for the task to complete and return the result
            return await tcs.Task;
        }

        /// <summary>
        /// Opens a FileBrowserDialog for the user to select a file path and return it.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="dialogTitle"></param>
        public static async Task<string> GetFilePathAsync(IStorageProvider provider,
            string dialogTitle = "Select a File Path")
        {
            // Create a TaskCompletionSource to handle the result of the file dialog
            var tcs = new TaskCompletionSource<string>();

            // Run on the UI thread
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (provider == null)
                {
                    // Handle the case where the parent window is not found
                    tcs.SetResult(null);
                    return;
                }

                // Create an instance of the FileDialog
                var openFolderOptions = new FolderPickerOpenOptions
                {
                    Title = dialogTitle,
                    AllowMultiple = false,
                };

                // Open the dialog and cache the result
                var result = await provider.OpenFolderPickerAsync(openFolderOptions);

                // If the user selects a file, set the result
                if (result != null && result.Any())
                {
                    string systemPath = new Uri(result[0].Path.ToString()).LocalPath;
                    tcs.SetResult(systemPath);
                }
                else
                {
                    tcs.SetResult(null);
                }
            });

            // Wait for the task to complete and return the result
            return await tcs.Task;
        }
    }
}