using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.ImageProcessing;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using D3DTX_Converter.ProgramDebug;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.TelltaleD3DTX;

namespace D3DTX_Converter.ProgramDebug
{
    public static class Program_Debug
    {
        //----------------------CONVERSION MODES----------------------

        /*
         * HOW TO USE: Change the value of a certain mode to 'true' to enable the desired mode.
         * ONLY 1 SHOULD BE 'true' AT A TIME
        */

        //main conversion to and from d3dtx
        public static bool mode_d3dtx_to_dds = false; //.d3dtx ---> .dds
        public static bool mode_dds_to_d3dtx = true; //.dds ---> .d3dtx

        public static bool fixes_generic_to_dds = true; //FIXES normal maps and swizzle channels
        public static bool fixes_dds_to_generic = true; //FIXES normal maps and swizzle channels

        //dds into standard images
        public static bool mode_dds_to_png = true; //.dds ---> .png
        public static bool mode_dds_to_bmp = false; //.dds ---> .bmp
        public static bool mode_dds_to_tiff = false; //.dds ---> .tiff
        public static bool mode_dds_to_jpeg = false; //.dds ---> .jpeg

        //standard images into dds
        public static bool mode_png_to_dds = false; //.png ---> .dds
        public static bool mode_bmp_to_dds = false; //.bmp ---> .dds
        public static bool mode_tiff_to_dds = false; //.tiff ---> .dds
        public static bool mode_jpeg_to_dds = false; //.jpeg ---> .dds

        //----------------------CONVERSION MODES END----------------------

        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        { 
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
            Console.WriteLine("D3DTX to DDS Texture Converter");

            ConsoleFunctions.SetConsoleColor(ConsoleColor.DarkGreen, ConsoleColor.White);
            Console.WriteLine("Enter 1 for d3dtx to dds. Enter 2 for dds to d3dtx. Enter 3 for extra - may not work correctly.");

            //texture folder path (containing the path to the textures to be converted)
            string val = Console.ReadLine();
            int a = Convert.ToInt32(val);
            //main
            if (a == 1) { Program_D3DTX_TO_DDS.Execute(); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
            else if (a == 2) { Program_DDS_TO_D3DTX.Execute(); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
            else if (a == 3)
            {
                
                Console.WriteLine("Enter 1 for dds to png. \nEnter 2 for dds to bmp. \nEnter 3 for dds to tiff. \nEnter 4 for dds to jpg.");
                Console.WriteLine("Enter 3 for png to dds. \nEnter 6 for bmp to dds. \nEnter 7 for tiff to dds. \nEnter 8 for jpg to dds.");
                string val2 = Console.ReadLine();
                int b = Convert.ToInt32(val2);
                //dds intoelse if (a == 2)  standard images
                if (b == 1) { Program_DDS_TO_PNG.Execute(fixes_dds_to_generic); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 2) { Program_DDS_TO_BMP.Execute(fixes_dds_to_generic); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 3) { Program_DDS_TO_TIFF.Execute(fixes_dds_to_generic); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 4) { Program_DDS_TO_JPEG.Execute(fixes_dds_to_generic); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                //standard images into dds
                else if (b == 5) { Program_PNG_TO_DDS.Execute(fixes_generic_to_dds); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 6) { Program_BMP_TO_DDS.Execute(fixes_generic_to_dds); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 7) { Program_TIFF_TO_DDS.Execute(fixes_generic_to_dds); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else if (b == 8) { Program_JPEG_TO_DDS.Execute(fixes_generic_to_dds); Console.WriteLine("Conversion finished. Program will close automatically."); Thread.Sleep(6000); Thread.Sleep(6000); return; }
                else { ConsoleFunctions.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); ; Console.WriteLine("Invalid Input. Program will close automatically."); Thread.Sleep(6000); return; }
            }

            else { ConsoleFunctions.SetConsoleColor(ConsoleColor.Red, ConsoleColor.White); ; Console.WriteLine("Invalid Input. Program will close automatically."); Thread.Sleep(6000); return; }



            //main conversion to and from d3dtx
            //if (mode_d3dtx_to_dds) { Program_D3DTX_TO_DDS.Execute(); return; }
            //if (mode_dds_to_d3dtx) { Program_DDS_TO_D3DTX.Execute(); return; }

            //dds into standard images
            //if (mode_dds_to_png) { Program_DDS_TO_PNG.Execute(fixes_dds_to_generic); return; }
            //if (mode_dds_to_bmp) { Program_DDS_TO_BMP.Execute(fixes_dds_to_generic); return; }
            //if (mode_dds_to_tiff) { Program_DDS_TO_TIFF.Execute(fixes_dds_to_generic); return; }
            //if (mode_dds_to_jpeg) { Program_DDS_TO_JPEG.Execute(fixes_dds_to_generic); return; }

            //standard images into dds
            //if (mode_png_to_dds) { Program_PNG_TO_DDS.Execute(fixes_generic_to_dds); return; }
            //if (mode_bmp_to_dds) { Program_BMP_TO_DDS.Execute(fixes_generic_to_dds); return; }
            //if (mode_tiff_to_dds) { Program_TIFF_TO_DDS.Execute(fixes_generic_to_dds); return; }
            //if (mode_jpeg_to_dds) { Program_JPEG_TO_DDS.Execute(fixes_generic_to_dds); return; }
        }
    }
}
