using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;

namespace TextureMod_GUI
{
    /// <summary>
    /// This class contains common System.IO functions but 'enhanced' with safety checks, warnings, and error messages.
    /// <para>It also contains other additional custom System.IO functions</para>
    /// <para>We recomend you use this class's functions in-place of the traditional System.IO functions</para>
    /// </summary>
    public class IOManagement
    {
        /// <summary>
        /// Duplicate/Copy a file, this will output the copied file path
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="copiedFilePath"></param>
        /// <param name="copiedSuffix"></param>
        public void DuplicateFile(string originalFilePath, out string copiedFilePath, string copiedSuffix = "_duplicated")
        {
            copiedFilePath = "null";

            //check if the file exists as a safety measure, if not then stop
            if (File.Exists(originalFilePath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrWhiteSpace(originalFilePath))
                    message = "No given file to duplicate!";
                else
                    message = originalFilePath + " - given file does not exist! Can't duplicate this file!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue
                return;
            }

            //build the new file path string from the original
            string originalFileExtension = Path.GetExtension(originalFilePath);
            string originalFilename = Path.GetFileNameWithoutExtension(originalFilePath);
            string originalPath = Path.GetDirectoryName(originalFilePath);
            string newFileName = originalFilename + copiedSuffix + originalFileExtension;
            copiedFilePath = originalPath + "/" + newFileName;

            try
            {
                //duplicate the file
                File.Copy(originalFilePath, copiedFilePath);
            }
            catch (Exception e)
            {
                //we have an exception
                string message = string.Format("Can't Copy the file! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Duplicate/Copy a file to a specified directory
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="outputDirectory"></param>
        /// <param name="sameNameAsOriginal"></param>
        public void DuplicateFileToDirectory(string originalFilePath, string outputDirectory, bool sameNameAsOriginal = true)
        {
            string duplicatedFilePath = "";
            string newFileName = "";

            DuplicateFile(originalFilePath, out duplicatedFilePath);

            if (sameNameAsOriginal)
            {
                newFileName = Path.GetFileName(originalFilePath);
            }
            else
            {
                newFileName = Path.GetFileName(duplicatedFilePath);
            }

            string newOutputPath = Path.Combine(outputDirectory, newFileName);

            MoveFile(duplicatedFilePath, newOutputPath);
        }

        /// <summary>
        /// Duplicate/Copy a file and replace it with the original file
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="newFilePath"></param>
        public void ReplaceFile(string originalFilePath, string newFilePath)
        {
            string duplicatedArchive = "";

            DuplicateFile(newFilePath, out duplicatedArchive);
            DeleteFile(originalFilePath);
            MoveFile(duplicatedArchive, originalFilePath);
        }

        /// <summary>
        /// Move a given file to a new location.
        /// This can also be used to re-name files.
        /// <para>Note: the new file path must contain the filename and extension.</para>
        /// <para>Example: originalFilePath = "OldPath/File.txt" newFilePath = "NewPath/File.txt"</para>
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="newFilePath"></param>
        /// <param name="giveWarning"></param>
        public void MoveFile(string originalFilePath, string newFilePath, bool giveWarning = false)
        {
            //check if the file exists as a safety measure, if not then stop
            if (File.Exists(originalFilePath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrWhiteSpace(originalFilePath))
                    message = "No given file to move!";
                else
                    message = originalFilePath + " - given file does not exist! Can't move this file!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to move the following file? " + originalFilePath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //move the file
                File.Move(originalFilePath, newFilePath);
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't Move the file! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Create a given directory
        /// </summary>
        /// <param name="newDirectoryPath"></param>
        /// <param name="giveWarning"></param>
        public void CreateDirectory(string newDirectoryPath, bool giveWarning = false)
        {
            //check if the directory exists as a safety measure, if not then stop
            if (string.IsNullOrEmpty(newDirectoryPath) || string.IsNullOrWhiteSpace(newDirectoryPath))
            {
                //get our message
                string message = "No given directory path to create!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to create the following directory? " + newDirectoryPath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //delete the file
                Directory.CreateDirectory(newDirectoryPath);
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't create the directory! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Move a given directory.
        /// </summary>
        /// <param name="oldDirectoryPath"></param>
        /// <param name="newDirectoryPath"></param>
        /// <param name="giveWarning"></param>
        public void MoveDirectory(string oldDirectoryPath, string newDirectoryPath, bool giveWarning = false)
        {
            //check if the irectory exists as a safety measure, if not then stop
            if (Directory.Exists(oldDirectoryPath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(oldDirectoryPath) || string.IsNullOrWhiteSpace(oldDirectoryPath))
                    message = "No given directory path to move!";
                else
                    message = oldDirectoryPath + " - given create does not exist! Can't delete this directory!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to move the following directory? " + newDirectoryPath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //delete the file
                Directory.Move(oldDirectoryPath, newDirectoryPath);
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't move the directory! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete a given directory
        /// </summary>
        /// <param name="originalDirectoryPath"></param>
        /// <param name="giveWarning"></param>
        public void DeleteDirectory(string originalDirectoryPath, bool giveWarning = false, bool recursiveDelete = false)
        {
            //check if the file exists as a safety measure, if not then stop
            if (Directory.Exists(originalDirectoryPath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(originalDirectoryPath) || string.IsNullOrWhiteSpace(originalDirectoryPath))
                    message = "No given file to delete!";
                else
                    message = originalDirectoryPath + " - given directory does not exist! Can't delete this directory!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to delete the following directory? " + originalDirectoryPath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //delete the file
                Directory.Delete(originalDirectoryPath, recursiveDelete);
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't Delete the directory! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Deletes all subdirectory/contents in directory
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="giveWarning"></param>
        /// <param name="recursiveDelete"></param>
        public void DeleteDirectoryContents(string directoryPath, bool giveWarning = false, bool recursiveDelete = false)
        {
            //check if the file exists as a safety measure, if not then stop
            if (Directory.Exists(directoryPath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrWhiteSpace(directoryPath))
                    message = "No given file to delete!";
                else
                    message = directoryPath + " - given directory does not exist! Can't delete this directory!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to delete the contents in the following directory? " + directoryPath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //delete the directory
                foreach (string subDirectory in Directory.GetDirectories(directoryPath))
                {
                    Directory.Delete(subDirectory, recursiveDelete);
                }
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't delete the directory contents! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Delete a given file.
        /// </summary>
        /// <param name="originalFilePath"></param>
        /// <param name="giveWarning"></param>
        public void DeleteFile(string originalFilePath, bool giveWarning = false)
        {
            //check if the file exists as a safety measure, if not then stop
            if (File.Exists(originalFilePath) == false)
            {
                //get our message
                string message = "";

                if (string.IsNullOrEmpty(originalFilePath) || string.IsNullOrWhiteSpace(originalFilePath))
                    message = "No given file to delete!";
                else
                    message = originalFilePath + " - given file does not exist! Can't delete this file!";

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //dont continue with the rest of the function
                return;
            }

            if (giveWarning)
            {
                //get our message
                string message = "Are you sure you want to delete the following file? " + originalFilePath;

                //warn the user about the action
                DialogResult dialogResult = MessageBox.Show(message, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.No)
                {
                    //dont continue with the rest of the function
                    return;
                }
            }

            try
            {
                //delete the file
                File.Delete(originalFilePath);
            }
            catch (Exception e)
            {
                //we have an IO exception
                string message = string.Format("Can't Delete the file! Exception: {0}. Exception Message: {1}", e, e.Message);

                //notify the user that this action is not possible
                MessageBox.Show(message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Get the number of files in a directory by a specified file extension.
        /// </summary>
        /// <param name="givenDirectory"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public int GetNumberOfFilesByExtension(string givenDirectory, string fileExtension)
        {
            //the return value
            int numberOfFiles = 0;

            //lets get our file list
            List<string> filesInDirectory = new List<string>(Directory.GetFiles(givenDirectory));

            //loop through the list
            foreach (string file in filesInDirectory)
            {
                //increment our variable whenever we see what we are looking for
                if (Path.GetExtension(file).Equals(fileExtension))
                {
                    numberOfFiles++;
                }
            }

            return numberOfFiles;
        }

        /// <summary>
        /// Get the file paths in a given directory by a specified file extension. (include the period on the extension, like .txt)
        /// </summary>
        /// <param name="givenDirectory"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public List<string> GetFilesPathsByExtension(string givenDirectory, string fileExtension)
        {
            //the list we will be returning
            List<string> filesPaths = new List<string>();

            //prepare our file list (only assigns it when there are files in the directory)
            List<string> filesInDirectory = new List<string>();

            if (string.IsNullOrEmpty(givenDirectory) == false || string.IsNullOrWhiteSpace(givenDirectory) == false)
                filesInDirectory = new List<string>(Directory.GetFiles(givenDirectory));

            //loop through the list
            foreach (string file in filesInDirectory)
            {
                //add to the list whenever we see what we are looking for
                if (Path.GetExtension(file).Equals(fileExtension))
                {
                    filesPaths.Add(file);
                }
            }

            return filesPaths;
        }

        /// <summary>
        /// Delete the files in a given directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="warning"></param>
        /// <param name="promptDescription"></param>
        /// <param name="promptTitle"></param>
        public void DeleteFilesInDirectory(string directory, bool warning = false, string promptDescription = "", string promptTitle = "")
        {
            if (warning)
            {
                if (MessageBox_Confirmation(promptDescription, promptTitle) == false)
                    return;
            }

            foreach (string filePath in Directory.GetFiles(directory))
            {
                DeleteFile(filePath);
            }
        }

        /// <summary>
        /// Delete the sub directories in a given directory.
        /// </summary>
        /// <param name="mainDir"></param>
        /// <param name="warning"></param>
        /// <param name="promptDescription"></param>
        /// <param name="promptTitle"></param>
        public void DeleteDirectoriesInDirectory(string mainDir, bool warning = false, string promptDescription = "", string promptTitle = "")
        {
            if (warning)
            {
                if (MessageBox_Confirmation(promptDescription, promptTitle) == false)
                    return;
            }

            foreach (string directory in Directory.GetDirectories(mainDir))
            {
                DeleteDirectory(directory);
            }
        }

        //------------------------------ PROMPTS ------------------------------ 
        //------------------------------ PROMPTS ------------------------------ 
        //------------------------------ PROMPTS ------------------------------ 

        /// <summary>
        /// Opens a FolderBrowserDialog for the user to select a folder path.
        /// </summary>
        /// <param name="newFolderPath"></param>
        /// <param name="dialogTitle"></param>
        public void GetFolderPath(ref string newFolderPath, string dialogTitle = "Select a Folder Path")
        {
            //open a folder dialog
            VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog();
            folderBrowserDialog.Description = dialogTitle;

            //open the dialog and cache the result
            var result = folderBrowserDialog.ShowDialog();

            //if the user selected a path, return that string.
            if (result.HasValue && result.Value == true && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
            {
                newFolderPath = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Opens a FileBrowserDialog for the user to select a file path.
        /// </summary>
        /// <param name="newFilePath"></param>
        /// <param name="dialogTitle"></param>
        public void GetFilePath(ref string newFilePath, string dialogTitle = "Select a File Path")
        {
            //open a file dialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //set our file dialog options here
                openFileDialog.Multiselect = false;
                openFileDialog.Title = dialogTitle;

                //open the dialog and cache the result
                DialogResult result = openFileDialog.ShowDialog();

                //if the user selects a file, return that string.
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    newFilePath = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Opens a FileBrowserDialog for the user to select a file path with a specified file extension.
        /// </summary>
        /// <param name="newFilePath"></param>
        /// <param name="extensionFilter"></param>
        /// <param name="dialogTitle"></param>
        public void GetFilePath(ref string newFilePath, string extensionFilter, string dialogTitle = "Select a File Path")
        {
            //open a file dialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //set our file dialog options here
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = dialogTitle;
                openFileDialog.Filter = extensionFilter;

                //open the dialog and cache the result
                DialogResult result = openFileDialog.ShowDialog();

                //if the user selects a file, return that string.
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    newFilePath = openFileDialog.FileName;
                }
            }
        }

        /// <summary>
        /// Opens a MessageBox that prompts the user if they want to proceed with said action.
        /// <para>DialogResult.Yes - returns true</para>
        /// <para>DialogResult.No - returns false</para>
        /// </summary>
        /// <param name="description"></param>
        /// <param name="title"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public bool MessageBox_Confirmation(string description, string title, MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            //get our message box
            DialogResult messageBox = MessageBox.Show(description, title, MessageBoxButtons.YesNo, icon);

            if (messageBox == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
