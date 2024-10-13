using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
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
            string currentApplicationExecutablePath = Assembly.GetExecutingAssembly().Location;
            string currentApplicationDirectoryPath = Path.GetDirectoryName(currentApplicationExecutablePath);
            string texconvApplicationDirectoryPath = Path.Combine(currentApplicationDirectoryPath, "ExternalDependencies/texconv.exe");

            ProcessStartInfo textconvProcessStartInfo = new(texconvApplicationDirectoryPath)
            {
                Arguments = options.GetArguments(inputFilePath),
                CreateNoWindow = true,
            };

            Process texconvProcess = new();
            texconvProcess.StartInfo = textconvProcessStartInfo;
            texconvProcess.Start();
            texconvProcess.WaitForExit(); //normally i'd let it run async but I like to work syncrhonsouly, just makes things easier.
        }
    }
}
