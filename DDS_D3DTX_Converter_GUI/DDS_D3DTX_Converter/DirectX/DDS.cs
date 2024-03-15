using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using DirectXTexNet;
using System;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    public static class DDS
    {
        public static uint GetDDSBlockSize(DDS_HEADER header)
        {
            //Image image = null;
            //image.SlicePitch

            uint compressionValue = header.ddspf.dwFourCC;

            if (compressionValue == ByteFunctions.Convert_String_To_UInt32("DXT1"))
                return 8;
            if (compressionValue == ByteFunctions.Convert_String_To_UInt32("ATI1"))
                return 8;
            else
                return 16;
        }

        public static uint[,] CalculateMipResolutions(uint mipCount, uint width, uint height)
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

        public static uint[] GetImageByteSizes(uint[,] mipResolutions, uint baseLinearSize, uint blockSize)
        {
            uint[] byteSizes = new uint[mipResolutions.GetLength(0)];

            for(int i = 0; i < byteSizes.Length; i++)
            {
                uint mipWidth = mipResolutions[i, 0];
                uint mipHeight = mipResolutions[i, 1];

                byteSizes[i] = CalculateByteSize(mipWidth, mipHeight, blockSize);

                // if (mipWidth == mipHeight) //SQUARE SIZE
                // {
                //     computed linear size
                //     (mipWidth * mipWidth) / 2
                //
                //     byteSizes[i] = Calculate_ByteSize_Square(mipWidth, mipHeight, baseLinearSize, (uint)i, (uint)byteSizes.Length, blockSize);
                //     byteSizes[i] = Calculate_ByteSize(mipWidth, mipHeight, blockSize);
                // }   
                // else //NON SQUARE
                // {
                //     byteSizes[i] = Calculate_ByteSize_NonSquare(mipWidth, mipHeight, blockSize);
                // }
                //
                // original calculation
                // byteSizes[i] = CalculateDDS_ByteSize((int)mipResolutions[i, 0], (int)mipResolutions[i, 1], isDXT1);
            }

            return byteSizes;
        }

        /// <summary>
        /// The block-size is 8 bytes for DXT1, BC1, and BC4 formats, and 16 bytes for other block-compressed formats.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="blockSizeDouble"></param>
        /// <returns></returns>
        public static uint ComputePitchValue_BlockCompression(uint width, uint blockSize)
        {
            //max(1, ((width + 3) / 4)) * blocksize
            return (uint)(MathF.Max(1, (width + 3) / 4) * blockSize);
        }

        public static uint ComputePitchValue_Legacy(uint width)
        {
            //((width+1) >> 1) * 4
            return ((width + 1) >> 1) * 4;
        }

        public static uint ComputePitchValue_Other(uint width, uint bitsPerPixel)
        {
            //( width * bits-per-pixel + 7 ) / 8
            return (width * bitsPerPixel + 7) / 8;
        }

        public static bool CompressionBool(DDS_HEADER header) => header.ddspf.dwFourCC.Equals("DXT1");

        public static T3SurfaceFormatDesc GetSurfaceFormatDesc()
        {
            T3SurfaceFormatDesc result = new()
            {
                mBitsPerBlock = 0,
                mBitsPerPixel = 0,
                mBlockHeight = 1,
                mBlockWidth = 1,
                mMinBytesPerSurface = 1
            };

            uint test = 0;

            switch (test)
            {
                case 0xEu:
                case 0x10u:
                case 0x11u:
                    result.mBitsPerPixel = 8;
                    break;
                case 0xDu:
                case 0x25u:
                    result.mBitsPerPixel = 128;
                    break;
                case 2u:
                case 3u:
                case 4u:
                case 6u:
                case 9u:
                case 0x12u:
                case 0x13u:
                case 0x16u:
                case 0x20u:
                case 0x30u:
                case 0x32u:
                    result.mBitsPerPixel = 16;
                    break;
                case 1u:
                case 8u:
                case 0xCu:
                case 0x15u:
                case 0x22u:
                case 0x24u:
                case 0x36u:
                    result.mBitsPerPixel = 64;
                    break;
                case 0x40u:
                case 0x43u:
                case 0x45u:
                case 0x70u:
                case 0x71u:
                case 0x72u:
                case 0x74u:
                    result.mBitsPerPixel = 4;
                    result.mBlockWidth = 4;
                    result.mBlockHeight = 4;
                    result.mBitsPerBlock = 64;
                    break;
                case 0x50u:
                case 0x52u:
                    result.mBitsPerPixel = 2;
                    result.mBlockHeight = 8;
                    goto LABEL_8;
                case 0x51u:
                case 0x53u:
                    result.mBitsPerPixel = 4;
                    result.mBlockHeight = 4;
                LABEL_8:
                    result.mBlockWidth = 4;
                    result.mBitsPerBlock = 64;
                    result.mMinBytesPerSurface = 32;
                    break;
                case 0x60u:
                    result.mBitsPerPixel = 4;
                    result.mBlockWidth = 4;
                    result.mBlockHeight = 4;
                    result.mBitsPerBlock = 64;
                    result.mMinBytesPerSurface = 8;
                    break;
                case 0x61u:
                case 0x62u:
                    result.mBitsPerPixel = 8;
                    result.mBlockWidth = 4;
                    result.mBlockHeight = 4;
                    result.mBitsPerBlock = 128;
                    result.mMinBytesPerSurface = 16;
                    break;
                case 0x41u:
                case 0x42u:
                case 0x44u:
                case 0x46u:
                case 0x47u:
                case 0x73u:
                case 0x75u:
                case 0x80u:
                    result.mBitsPerPixel = 8;
                    result.mBlockWidth = 4;
                    result.mBlockHeight = 4;
                    result.mBitsPerBlock = 128;
                    break;
                case 0u:
                case 5u:
                case 7u:
                case 0xAu:
                case 0xBu:
                case 0xFu:
                case 0x14u:
                case 0x17u:
                case 0x21u:
                case 0x23u:
                case 0x26u:
                case 0x31u:
                case 0x33u:
                case 0x34u:
                case 0x35u:
                case 0x37u:
                case 0x90u:
                    result.mBitsPerPixel = 32;
                    break;
                default:
                    break;
            }

            /*
            v3 = result->mBitsPerBlock;
            v4 = 1;

            if (!v3)
            {
                v3 = v2->mBitsPerPixel;
                v2->mBitsPerBlock = v3;
                v2->mBlockWidth = 1;
                v2->mBlockHeight = 1;
            }

            if (!v2->mMinBytesPerSurface)
            {
                v5 = (v3 + 7) / 8;

                if (v5 > 1)
                    v4 = v5;

                v2->mMinBytesPerSurface = v4;
            }
            */

            return result;
        }

        /// <summary>
        /// Calculates the byte size of a DDS texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public static uint CalculateByteSize(uint width, uint height, uint bitPixelSize)
        {
            //formula (from microsoft docs)
            //max(1, ( (width + 3) / 4 ) ) x max(1, ( (height + 3) / 4 ) ) x 8(DXT1) or 16(DXT2-5)

            //formula (from here) - http://doc.51windows.net/directx9_sdk/graphics/reference/DDSFileReference/ddstextures.htm
            //max(1,width ?4)x max(1,height ?4)x 8 (DXT1) or 16 (DXT2-5)

            //do the micorosoft magic texture byte size calculation formula
            return Math.Max(1, ((width + 3) / 4)) * Math.Max(1, ((height + 3) / 4)) * bitPixelSize;

            //formula (from here) - http://doc.51windows.net/directx9_sdk/graphics/reference/DDSFileReference/ddstextures.htm
            //return Math.Max(1, width / 4) * Math.Max(1, height / 4) * bitPixelSize;
        }

        /// <summary>
        /// Converts an array of bytes into a DDS header object.
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static DDS_HEADER GetHeaderFromBytes(byte[] byteArray)
        {
            DDS_HEADER headerObject = new();

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
            return new()
            {
                dwSize = 124,
                ddspf = new()
                {
                    dwSize = 32,
                    dwFourCC = ByteFunctions.Convert_String_To_UInt32("DXT1"),
                },
            };
        }

        public static DDS_HEADER GetPresetHeader()
        {
            return new()
            {
                dwSize = 124,
                dwFlags = 528391,
                dwHeight = 1024,
                dwWidth = 1024,
                dwPitchOrLinearSize = 8192,
                dwDepth = 0,
                dwMipMapCount = 0,
                ddspf = new()
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
                default: return ByteFunctions.Convert_String_To_UInt32("DXT1");
                case T3SurfaceFormat.eSurface_DXT1: return ByteFunctions.Convert_String_To_UInt32("DXT1");
                case T3SurfaceFormat.eSurface_DXT3: return ByteFunctions.Convert_String_To_UInt32("DXT3");
                case T3SurfaceFormat.eSurface_DXT5: return ByteFunctions.Convert_String_To_UInt32("DXT5");
                case T3SurfaceFormat.eSurface_DXN: return ByteFunctions.Convert_String_To_UInt32("ATI2");
                case T3SurfaceFormat.eSurface_DXT5A: return ByteFunctions.Convert_String_To_UInt32("ATI1");
                case T3SurfaceFormat.eSurface_BC4: return ByteFunctions.Convert_String_To_UInt32("BC4S");
                case T3SurfaceFormat.eSurface_BC5: return ByteFunctions.Convert_String_To_UInt32("BC5S");
                case T3SurfaceFormat.eSurface_A8: return ByteFunctions.Convert_String_To_UInt32("DX10");
            }
        }

        public static T3SurfaceFormat Get_T3Format_FromFourCC(uint fourCC)
        {
            if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT1")) return T3SurfaceFormat.eSurface_DXT1;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT3")) return T3SurfaceFormat.eSurface_DXT3;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT5")) return T3SurfaceFormat.eSurface_DXT5;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI2")) return T3SurfaceFormat.eSurface_DXN;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI1")) return T3SurfaceFormat.eSurface_DXT5A;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC4S")) return T3SurfaceFormat.eSurface_BC4;
            else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC5S")) return T3SurfaceFormat.eSurface_BC5;

            else return T3SurfaceFormat.eSurface_DXT1;
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
