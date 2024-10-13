using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TextureMod_GUI
{
    public class WorkingDirectory
    {
        public string workingDirectoryPath;
        public List<WorkingDirectory_File> workingDirectory_files;

        public void GetFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            if(workingDirectory_files != null)
                workingDirectory_files.Clear();

            workingDirectory_files = new List<WorkingDirectory_File>();

            workingDirectoryPath = path;

            List<string> directoryFiles = new List<string>(Directory.GetFiles(workingDirectoryPath));

            foreach (string file in directoryFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string fileExt = Path.GetExtension(file);

                WorkingDirectory_File workingDirectory_file = new WorkingDirectory_File();

                workingDirectory_file.FileName = fileName;
                workingDirectory_file.FileType = fileExt;
                workingDirectory_file.FilePath = file;

                workingDirectory_files.Add(workingDirectory_file);
            }
        }
    }
}
