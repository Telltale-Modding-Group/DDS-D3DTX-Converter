using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDS_D3DTX_Converter;

public class WorkingDirectoryFile : IEquatable<WorkingDirectoryFile>
{
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public DateTime FileLastWrite { get; set; }
    public string? FilePath { get; set; }

    public bool Equals(WorkingDirectoryFile? other)
    {
        return this.FileName == other.FileName &&
                 this.FileType == other.FileType &&
                 this.FilePath == other.FilePath;
    }
}

public class WorkingDirectory
{
    public string WorkingDirectoryPath = string.Empty;
    public List<WorkingDirectoryFile> WorkingDirectoryFiles = [];

    //hardcoded filters
    public List<string> filterFileExtensions = [".d3dtx", ".dds", ".png", ".jpg", ".jpeg", ".tiff", ".tif", ".bmp", ".json", ".ktx", ".ktx2", ".tga", ""];

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

        if (directoryPath != WorkingDirectoryPath)
        {
            WorkingDirectoryFiles.Clear();
        }

        var deletedFiles = new List<WorkingDirectoryFile>();

        foreach (var file in WorkingDirectoryFiles)
        {
            if (!File.Exists(file.FilePath) && !Directory.Exists(file.FilePath))
            {
                deletedFiles.Add(file);
            }
            else if (File.Exists(file.FilePath) && file.FileType == "")
            {
                deletedFiles.Add(file);
            }
        }

        WorkingDirectoryPath = directoryPath;

        List<string> directoryItems = new(Directory.GetFiles(WorkingDirectoryPath).Concat(Directory.GetDirectories(WorkingDirectoryPath)));

        foreach (string file in directoryItems)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            string fileExt = Path.GetExtension(file).ToLower();

            if (!filterFileExtensions.Contains(fileExt))
            {
                continue;
            }

            WorkingDirectoryFile workingDirectoryFile = new()
            {
                FileName = fileName,
                FileType = fileExt,
                FilePath = file,
                FileLastWrite = File.GetLastWriteTime(file)
            };

            if (!WorkingDirectoryFiles.Contains(workingDirectoryFile))
            {
                Console.WriteLine("Adding file: " + fileName);
                WorkingDirectoryFiles.Add(workingDirectoryFile);
            }
            else
            {
                WorkingDirectoryFiles[WorkingDirectoryFiles.IndexOf(workingDirectoryFile)].FileLastWrite = File.GetLastWriteTime(file);
            }
        }

        WorkingDirectoryFiles = WorkingDirectoryFiles.Except(deletedFiles).ToList();
    }

    public string GetWorkingDirectoryPath() => WorkingDirectoryPath;
}