using System;
using System.Collections.Generic;
using System.IO;

namespace DDS_D3DTX_Converter;

public class WorkingDirectoryFile
{
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    
    public DateTime FileLastWrite { get; set; }
    public string? FilePath { get; set; }
}

public class WorkingDirectory
{
    public string WorkingDirectoryPath = string.Empty;
    public List<WorkingDirectoryFile> WorkingDirectoryFiles = [];

    /// <summary>
    /// Gets the files from the provided directory path.
    /// </summary>
    /// <param name="directoryPath"></param>
    /// <exception cref="DirectoryNotFoundException"></exception>
    public void GetFiles(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException("Selected directory cannot be found.");
        }

        if (WorkingDirectoryFiles.Count != 0)
            WorkingDirectoryFiles.Clear();

        WorkingDirectoryFiles = new List<WorkingDirectoryFile>();
        WorkingDirectoryPath = directoryPath;

        List<string> directoryFiles = new List<string>(Directory.GetFiles(WorkingDirectoryPath));
        List<string> directories = new List<string>(Directory.GetDirectories(WorkingDirectoryPath));

        foreach (string file in directoryFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file);
            
            WorkingDirectoryFile workingDirectoryFile = new WorkingDirectoryFile
            {
                FileName = fileName,
                FileType = fileExt,
                FilePath = file,
                FileLastWrite = File.GetLastWriteTime(file)
            };

            WorkingDirectoryFiles.Add(workingDirectoryFile);
        }

        foreach (string file in directories)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            WorkingDirectoryFile workingDirectoryFile = new WorkingDirectoryFile
            {
                FileName = fileName,
                FileType = string.Empty,
                FilePath = file,
                FileLastWrite = File.GetLastWriteTime(file)
            };

            WorkingDirectoryFiles.Add(workingDirectoryFile);
        }
    }
}