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
        public static bool mode_d3dtx_to_dds = true; //.d3dtx ---> .dds
        public static bool mode_dds_to_d3dtx = false; //.dds ---> .d3dtx

        public static bool fixes_generic_to_dds = true; //FIXES normal maps and swizzle channels
        public static bool fixes_dds_to_generic = true; //FIXES normal maps and swizzle channels

        //dds into standard images
        public static bool mode_dds_to_png = false; //.dds ---> .png
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
            //main conversion to and from d3dtx
            if (mode_d3dtx_to_dds) { Program_D3DTX_TO_DDS.Execute(); return; }
            if (mode_dds_to_d3dtx) { Program_DDS_TO_D3DTX.Execute(); return; }

            //dds into standard images
            if (mode_dds_to_png) { Program_DDS_TO_PNG.Execute(fixes_dds_to_generic); return; }
            if (mode_dds_to_bmp) { Program_DDS_TO_BMP.Execute(fixes_dds_to_generic); return; }
            if (mode_dds_to_tiff) { Program_DDS_TO_TIFF.Execute(fixes_dds_to_generic); return; }
            if (mode_dds_to_jpeg) { Program_DDS_TO_JPEG.Execute(fixes_dds_to_generic); return; }

            //standard images into dds
            if (mode_png_to_dds) { Program_PNG_TO_DDS.Execute(fixes_generic_to_dds); return; }
            if (mode_bmp_to_dds) { Program_BMP_TO_DDS.Execute(fixes_generic_to_dds); return; }
            if (mode_tiff_to_dds) { Program_TIFF_TO_DDS.Execute(fixes_generic_to_dds); return; }
            if (mode_jpeg_to_dds) { Program_JPEG_TO_DDS.Execute(fixes_generic_to_dds); return; }
        }
    }
}
