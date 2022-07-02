using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using D3DTX_Converter.ProgramModes;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.TelltaleD3DTX;

namespace D3DTX_Converter
{
    class Program
    {
        //----------------------CONVERSION MODES----------------------

        /*
         * HOW TO USE: Change the value of a certain mode to 'true' to enable the desired mode.
         * ONLY 1 SHOULD BE 'true' AT A TIME
        */

        public static bool dds_to_d3dtx = false; //.dds ---> .d3dtx
        public static bool d3dtx_to_dds = false; //.d3dtx ---> .dds

        public static bool dds_to_png = true; //.dds ---> .png
        public static bool dds_to_tga = false; //.dds ---> .tga
        public static bool dds_to_bmp = false; //.dds ---> .bmp
        public static bool dds_to_tiff = false; //.dds ---> .tiff
        public static bool dds_to_jpeg = false; //.dds ---> .jpeg
        public static bool png_to_dds = false; //.png ---> .dds
        public static bool tga_to_dds = false; //.tga ---> .dds
        public static bool bmp_to_dds = false; //.bmp ---> .dds
        public static bool tiff_to_dds = false; //.tiff ---> .dds
        public static bool jpeg_to_dds = false; //.jpeg ---> .dds

        //----------------------CONVERSION MODES END----------------------

        /// <summary>
        /// Main application method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (d3dtx_to_dds) { Program_D3DTX_TO_DDS.Execute(); return; }
            if (dds_to_d3dtx) { Program_DDS_TO_D3DTX.Execute(); return; }

            if (dds_to_png) { Program_DDS_TO_PNG.Execute(); return; }
            if (dds_to_tga) { Program_DDS_TO_TGA.Execute(); return; }
            if (dds_to_bmp) { Program_DDS_TO_BMP.Execute(); return; }
            if (dds_to_tiff) { Program_DDS_TO_TIFF.Execute(); return; }
            if (dds_to_jpeg) { Program_DDS_TO_JPEG.Execute(); return; }

            if (png_to_dds) { Program_PNG_TO_DDS.Execute(); return; }
            if (tga_to_dds) { Program_TGA_TO_DDS.Execute(); return; }
            if (bmp_to_dds) { Program_BMP_TO_DDS.Execute(); return; }
            if (tiff_to_dds) { Program_TIFF_TO_DDS.Execute(); return; }
            if (jpeg_to_dds) { Program_JPEG_TO_DDS.Execute(); return; }
        }
    }
}
