using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using D3DTX_Converter.TexconvOptions;
using System.Threading.Tasks;

namespace D3DTX_Converter.Texconv;
/*
 * TEXCONV DOCS - https://github.com/Microsoft/DirectXTex/wiki/Texconv 
*/

public static class TexconvApp
{
    public static async Task<bool> RunTexconvAsync(string inputFilePath, MasterOptions options)
    {
        string solutionDir = AppDomain.CurrentDomain.BaseDirectory;
        string texconvApplicationDirectoryPath = Path.Combine(solutionDir, "ExternalDependencies", "texconv.exe");

        ProcessStartInfo textconvProcessStartInfo = new()
        {
            CreateNoWindow = true
        };

        // Determine what OS we are running on
        // Check if the current OS is Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // If it's not Unix-based OS, run the .exe file directly
            textconvProcessStartInfo.FileName = texconvApplicationDirectoryPath;
            textconvProcessStartInfo.Arguments = options.GetArguments(inputFilePath);
        }
        else
        {
            // If it's Linux or OSX, use Wine to run the .exe file
            textconvProcessStartInfo.FileName = "wine";
            textconvProcessStartInfo.Arguments = $"{texconvApplicationDirectoryPath} {options.GetArguments(inputFilePath)}";
        }

        Process texconvProcess = new()
        {
            StartInfo = textconvProcessStartInfo
        };
        texconvProcess.Start();

        // normally i'd let it run async but I like to work synchronously, just makes things easier.
        await texconvProcess.WaitForExitAsync();

        return true;
    }
}
