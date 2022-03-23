using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.Telltale;
using DirectXTexNet;

namespace D3DTX_TextureConverter.DirectX
{
    public static class DDS_Functions
    {
        public static uint[,] DDS_CalculateMipResolutions(uint mipCount, uint width, uint height)
        {
            //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
            //first [] is the "resolution" index, and the second [] always has a length of 2, and contains the width and height
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

        public static uint[] DDS_GetImageByteSizes(uint[,] mipResolutions, uint baseLinearSize, bool isDXT1)
        {
            uint[] byteSizes = new uint[mipResolutions.GetLength(0)];

            for(int i = 0; i < byteSizes.Length; i++)
            {
                uint mipWidth = mipResolutions[i, 0];
                uint mipHeight = mipResolutions[i, 1];

                if(mipWidth == mipHeight)
                {
                    //computed linear size
                    //(mipWidth * mipWidth) / 2

                    byteSizes[i] = CalculateDDS_ByteSize_Square(mipWidth, mipHeight, baseLinearSize, (uint)i, (uint)byteSizes.Length, isDXT1);
                }   
                else
                {
                    byteSizes[i] = CalculateDDS_ByteSize_NonSquare(mipWidth, mipHeight, isDXT1);
                }

                //original calculation
                //byteSizes[i] = CalculateDDS_ByteSize((int)mipResolutions[i, 0], (int)mipResolutions[i, 1], isDXT1);
            }

            return byteSizes;
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

        public static bool DDS_CompressionBool(DDS_HEADER header) => header.ddspf.dwFourCC.Equals("DXT1");

        /// <summary>
        /// Calculates the byte size of a DDS non-square texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public static uint CalculateDDS_ByteSize_NonSquare(uint width, uint height, bool isDXT1)
        {
            uint compression = 0;

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
        /// Calculates the byte size of a DDS square texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public static uint CalculateDDS_ByteSize_Square(uint width, uint height, uint dwPitchOrLinearSize, uint mipLevel, uint maxMipLevel, bool isDXT1)
        {
            //reference - http://doc.51windows.net/directx9_sdk/graphics/reference/DDSFileReference/ddstextures.htm
            uint compression = 0;

            //according to formula, if the compression is dxt1 then the number needs to be 8
            if (isDXT1)
                compression = 8;
            else
                compression = 16;

            uint finalSize = dwPitchOrLinearSize;

            finalSize = (width * width) / 2;

            if (finalSize < compression)
                finalSize = compression;

            return finalSize;
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

        public static uint Get_FourCC_FromTellale(T3SurfaceFormat format)
        {
            switch (format)
            {
                default:
                    return ByteFunctions.Convert_String_To_UInt32("DXT1");
                case Telltale.T3SurfaceFormat.eSurface_DXT1:
                    return ByteFunctions.Convert_String_To_UInt32("DXT1");
                case Telltale.T3SurfaceFormat.eSurface_DXT3:
                    return ByteFunctions.Convert_String_To_UInt32("DXT3");
                case Telltale.T3SurfaceFormat.eSurface_DXT5:
                    return ByteFunctions.Convert_String_To_UInt32("DXT5");
                case Telltale.T3SurfaceFormat.eSurface_DXN:
                    return ByteFunctions.Convert_String_To_UInt32("ATI2");
                case Telltale.T3SurfaceFormat.eSurface_DXT5A:
                    return ByteFunctions.Convert_String_To_UInt32("ATI1");
                case Telltale.T3SurfaceFormat.eSurface_A8:
                    return 0;
            }
        }

        public static T3SurfaceFormat Get_T3Format_FromFourCC(uint fourCC)
        {
            if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT1"))
                return Telltale.T3SurfaceFormat.eSurface_DXT1;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT3"))
                return Telltale.T3SurfaceFormat.eSurface_DXT3;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT5"))
                return Telltale.T3SurfaceFormat.eSurface_DXT5;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI2"))
                return Telltale.T3SurfaceFormat.eSurface_DXN;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI1"))
                return Telltale.T3SurfaceFormat.eSurface_DXT5A;
            else
                return Telltale.T3SurfaceFormat.eSurface_DXT1;
        }

        public static DXGI_FORMAT GetSurfaceFormatAsDXGI(T3SurfaceFormat format, T3SurfaceGamma gamma = T3SurfaceGamma.eSurfaceGamma_sRGB)
        {
            switch(format)
            {
                default:
                    return DXGI_FORMAT.BC1_UNORM; //just choose classic DXT1 if the format isn't known

                //--------------------DXT1--------------------
                case T3SurfaceFormat.eSurface_BC1:
                    if(gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC1_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC1_UNORM;
                case T3SurfaceFormat.eSurface_DXT1:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC1_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC1_UNORM;

                //--------------------DXT2 and DXT3--------------------
                case T3SurfaceFormat.eSurface_BC2:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC2_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC2_UNORM;
                case T3SurfaceFormat.eSurface_DXT3:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC2_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC2_UNORM;

                //--------------------DXT4 and DXT5--------------------
                case T3SurfaceFormat.eSurface_BC3:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC3_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC3_UNORM;
                case T3SurfaceFormat.eSurface_DXT5:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC3_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC3_UNORM;

                //--------------------ATI1--------------------
                case T3SurfaceFormat.eSurface_BC4:
                    return DXGI_FORMAT.BC4_UNORM;
                case T3SurfaceFormat.eSurface_DXT5A:
                    return DXGI_FORMAT.BC4_UNORM;

                //--------------------ATI2--------------------
                case T3SurfaceFormat.eSurface_BC5:
                    return DXGI_FORMAT.BC5_UNORM;
                case T3SurfaceFormat.eSurface_DXN:
                    return DXGI_FORMAT.BC5_UNORM;


                case T3SurfaceFormat.eSurface_BC6:
                    return DXGI_FORMAT.BC6H_UF16;

                case T3SurfaceFormat.eSurface_BC7:
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return DXGI_FORMAT.BC7_UNORM_SRGB;
                    else
                        return DXGI_FORMAT.BC7_UNORM;
            }
        }
    }
}
