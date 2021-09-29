using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using D3DTX_TextureConverter.Utilities;

namespace D3DTX_TextureConverter.DirectX
{
    public static class DDS_Functions
    {
        public static uint[,] DDS_CalculateMipResolutions(uint mipCount, uint width, uint height)
        {
            //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
            uint[,] mipResolutions = new uint[mipCount + 1, 2];

            //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
            uint mipImageWidth = width * 2;
            uint mipImageHeight = height * 2;

            //add the resolutions in reverse
            for (uint i = mipCount; i > 0; i--)
            {
                //divide the resolutions by 2
                mipImageWidth /= 2;
                mipImageHeight /= 2;

                //assign the resolutions
                mipResolutions[i, 0] = mipImageWidth;
                mipResolutions[i, 1] = mipImageHeight;
            }

            return mipResolutions;
        }

        public static uint[] DDS_GetImageByteSizes(uint[,] mipResolutions, bool isDXT1)
        {
            uint[] byteSizes = new uint[mipResolutions.Length];

            for(int i = 0; i < byteSizes.Length; i++)
            {
                byteSizes[i] = (uint)CalculateDDS_ByteSize((int)mipResolutions[i, 0], (int)mipResolutions[i, 1], isDXT1);
            }

            return new uint[0];
        }

        /// <summary>
        /// The block-size is 8 bytes for DXT1, BC1, and BC4 formats, and 16 bytes for other block-compressed formats.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="blockSizeDouble"></param>
        /// <returns></returns>
        public static int DDS_ComputePitchValue(uint width, bool blockSizeDouble)
        {
            int blockSizeValue = 8;

            if (blockSizeDouble)
                blockSizeValue = 16;

            //max(1, ((width + 3) / 4)) * blocksize
            return (int)MathF.Max(1, ((width + 3) / 4)) * blockSizeValue;
        }

        public static bool DDS_CompressionBool(DDS_HEADER header)
        {
            return header.ddspf.dwFourCC.Equals("DXT1");
        }

        /// <summary>
        /// Calculates the byte size of a DDS texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public static int CalculateDDS_ByteSize(int width, int height, bool isDXT1)
        {
            int compression = 0;

            //according to formula, if the compression is dxt1 then the number needs to be 8
            if (isDXT1)
                compression = 8;
            else
                compression = 16;

            //formula (from microsoft docs)
            //max(1, ( (width + 3) / 4 ) ) x max(1, ( (height + 3) / 4 ) ) x 8(DXT1) or 16(DXT2-5)

            //do the micorosoft magic texture byte size calculation formula
            return Math.Max(1, ((width + 3) / 4)) * Math.Max(1, ((height + 3) / 4)) * compression;
        }

        /// <summary>
        /// Converts an array of bytes into a DDS header object.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static DDS_HEADER GetHeaderFromBytes(byte[] byteArray)
        {
            DDS_HEADER headerObject = new DDS_HEADER();

            int size = Marshal.SizeOf(headerObject);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(byteArray, 0, ptr, size);

            headerObject = (DDS_HEADER)Marshal.PtrToStructure(ptr, headerObject.GetType());
            Marshal.FreeHGlobal(ptr);

            return headerObject;
        }

        /// <summary>
        /// Converts a DDS_HEADER object into a byte array.
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] GetHeaderBytes(DDS_HEADER header)
        {
            int size = Marshal.SizeOf(header);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(header, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static DDS_HEADER GetBlankHeader()
        {
            return new DDS_HEADER()
            {
                dwSize = 124,
                ddspf = new DDS_PIXELFORMAT()
                {
                    dwSize = 32,
                    dwFourCC = ByteFunctions.Convert_String_To_UInt32("DXT1"),
                },
            };
        }

        public static DDS_HEADER GetPresetHeader()
        {
            return new DDS_HEADER()
            {
                dwSize = 124,
                dwFlags = 528391,
                dwHeight = 1024,
                dwWidth = 1024,
                dwPitchOrLinearSize = 8192,
                dwDepth = 0,
                dwMipMapCount = 0,
                ddspf = new DDS_PIXELFORMAT()
                {
                    dwSize = 32,
                    dwFlags = 4,
                    dwFourCC = ByteFunctions.Convert_String_To_UInt32("DXT1"),
                    dwRGBBitCount = 0,
                },
                dwCaps = 4096,
                dwCaps2 = 0,
                dwCaps3 = 0,
                dwCaps4 = 0,
            };
        }
    }
}
