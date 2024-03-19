using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using D3DTX_Converter.TexconvOptions;

namespace D3DTX_Converter.Texconv
{
    /*
     * TEXCONV DOCS - https://github.com/Microsoft/DirectXTex/wiki/Texconv 
    */

    public static class TexconvApp
    {
        public static void RunTexconv(string inputFilePath, MasterOptions options)
        {
            string solutionDir = AppDomain.CurrentDomain.BaseDirectory;
            string texconvApplicationDirectoryPath = Path.Combine(solutionDir, "ExternalDependencies", "texconv.exe");

            ProcessStartInfo textconvProcessStartInfo = new();
            textconvProcessStartInfo.CreateNoWindow = true;

            // Check if the current OS is Linux
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

            Process texconvProcess = new();
            texconvProcess.StartInfo = textconvProcessStartInfo;
            texconvProcess.Start();
            texconvProcess.WaitForExit(); //normally i'd let it run async but I like to work synchronously, just makes things easier.
        }
    }
}
