using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using D3DTX_Converter.ProgramRelease;
using D3DTX_Converter.ProgramDebug;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.TelltaleD3DTX;

namespace D3DTX_Converter
{
    class Program
    {
        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Program_Release.Main(args);
            Program_Debug.Main(args);
        }
    }
}
