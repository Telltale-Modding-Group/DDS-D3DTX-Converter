// //TODO ADD IMAGESECTIONS AND SORT BY DATASIZE
//         /// <summary>
//         /// Parses the data from a DDS byte array. (Can also read just the header only)
//         /// </summary>
//         /// <param name="fileData"></param>
//         /// <param name="headerOnly"></param>
//         private void GetData(byte[] fileData, bool headerOnly = false)
//         {
//             Console.WriteLine("Total Source Texture Byte Size = {0}", fileData.Length);

//             //which byte offset we are on for the source texture (will be changed as we go through the file)
//             byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, 4); //skip past the 'DDS '

//             dds = new DDS();

//             //this will automatically read all of the byte data in the header
//             dds.header = DDS_HEADER.GetHeaderFromBytes(headerBytes);

//             Console.WriteLine("DDS Height = {0}", dds.header.dwHeight);
//             Console.WriteLine("DDS Width = {0}", dds.header.dwWidth);
//             Console.WriteLine("DDS Mip Map Count = {0}", dds.header.dwMipMapCount);
//             Console.WriteLine("DDS Compression = {0}", dds.header.ddspf.dwFourCC);
//             Console.WriteLine("DDS F = {0}", dds.header.ddspf.dwFlags);
//             Console.WriteLine("DDS RGB = {0}", dds.header.ddspf.dwRGBBitCount);
//             Console.WriteLine("DDS A = {0}", dds.header.ddspf.dwABitMask);
//             Console.WriteLine("DDS R = {0}", dds.header.ddspf.dwRBitMask);
//             Console.WriteLine("DDS G = {0}", dds.header.ddspf.dwGBitMask);
//             Console.WriteLine("DDS B = {0}", dds.header.ddspf.dwBBitMask);

//             //get dxt10 header if it exists
//             if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
//             {
//                 byte[] dxt10headerBytes = ByteFunctions.AllocateBytes(20, fileData, 128); //skip the main header
//                 dds.dxt10Header = DDS_HEADER_DXT10.GetHeaderFromBytes(dxt10headerBytes);
//             }

//             if (headerOnly)
//                 return;

//             //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
//             //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
//             uint ddsHeaderLength = 4 + dds.header.dwSize;

//             //if dxt10Header is present, add additional 20 bytes
//             uint dxt10HeaderLength = (uint)((dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10")) ? 20 : 0);

//             //calculate the length of just the dds texture data
//             uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength - dxt10HeaderLength;

//             //allocate a byte array of dds texture length
//             byte[] ddsTextureData = new byte[ddsTextureDataLength];

//             //copy the data from the source byte array past the header (so we are only getting texture data)
//             Array.Copy(sourceFileData, ddsHeaderLength + dxt10HeaderLength, ddsTextureData, 0, ddsTextureData.Length);

//             textureData = [];

//             //NOTE: MIPMAPS CAN EXIST IN CUBE TEXTURES AND ARRAY TEXTURES
//             //if there are no mip maps. Works for all types of textures
//             if (dds.header.dwMipMapCount <= 1)
//             {
//                 textureData.Add(ddsTextureData);
//                 Console.WriteLine("DDS Texture Byte Size = {0}", textureData[0].Length);
//             }
//             else //if there are mip maps
//             {
//                 //get mip resolutions
//                 //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
//                 mipMapResolutions =
//                     DDS_HELPER.CalculateMipResolutions(dds.header.dwMipMapCount, dds.header.dwWidth, dds.header.dwHeight);

//                 uint byteSize;
//                 bool isCompressed = false;
//                 bool isFormatLegacy = true;
//                 uint format = dds.header.ddspf.dwFourCC;

//                 if (dds.header.ddspf.dwFourCC == ByteFunctions.Convert_String_To_UInt32("DX10"))
//                 {
//                     format = (uint)dds.dxt10Header.dxgiFormat;
//                     isFormatLegacy = false;
//                 }

//                 //check if it's compressed
//                 //get the block size if true
//                 //get the pixel size if false
//                 if (DDS_HELPER.IsTextureFormatCompressed(format))
//                 {
//                     isCompressed = true;
//                     byteSize = DDS_HELPER.GetDDSBlockSize(dds.header, dds.dxt10Header);
//                 }
//                 else
//                 {
//                     if (isFormatLegacy)
//                     {
//                         byteSize = DDS_HELPER.GetD3D9FORMATBitsPerPifxel((D3DFORMAT)dds.header.ddspf.dwFourCC) / 8; //this needs reformating
//                     }
//                     else
//                     {
//                         byteSize = DDS_HELPER.GetDXGIBitsPerPixel(dds.dxt10Header.dxgiFormat) / 8;
//                     }
//                 }

//                 uint[] byteSizes = DDS_HELPER.GetImageByteSizes(mipMapResolutions, dds.header.dwPitchOrLinearSize, byteSize, isCompressed);

//                 int offset = 0;
//                 //DETERMINE TEXTURE LAYOUT AND TEXTURE TYPE
//                 //if it's a 2d texture
//                 if (dds.header.dwCaps2 == 0)
//                 {

//                     //get the mipmaps from larger to smaller
//                     for (int i = 0; i < byteSizes.Length; i++)
//                     {
//                         //get the largest mipmap
//                         byte[] temp = new byte[byteSizes[i]];

//                         //issue length
//                         Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

//                         offset += temp.Length;

//                         textureData.Add(temp);
//                     }

//                     //reverse the mipmaps to match the Telltale type
//                     textureData.Reverse();
//                 }
//                 // if it's a cubemap texture
//                 else if ((dds.header.dwCaps2 & DDSCAPS2.CUBEMAP) != 0)
//                 {
//                     for (int faceIndex = 0; faceIndex < 6; faceIndex++)
//                     {
//                         for (int i = 0; i < byteSizes.Length; i++)
//                         {
//                             //get the largest mipmap
//                             byte[] temp = new byte[byteSizes[i]];

//                             //issue length
//                             Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

//                             offset += temp.Length;

//                             textureData.Add(temp);
//                         }
//                     }

//                     List<byte[]> tempData = [];

//                     for (int i = 0; i < 6; i++)
//                     {
//                         for (int j = 0; j < dds.header.dwMipMapCount; j++)
//                         {
//                             tempData.Add(textureData[i + (int)(j * dds.header.dwMipMapCount)]);
//                         }
//                     }

//                     textureData.Reverse();
//                 }
//                 //UNFINISHED
//                 else if ((dds.header.dwCaps2 & DDSCAPS2.VOLUME) != 0)
//                 {
//                     //TODO RENAME SOME VARIABLES
//                     mipMapResolutions = DDS_HELPER.CalculateVolumeMipResolutions(dds.header.dwMipMapCount, dds.header.dwWidth, dds.header.dwHeight, dds.header.dwDepth);
//                     byteSizes = DDS_HELPER.GetImageByteSizes(mipMapResolutions, dds.header.dwPitchOrLinearSize, byteSize, isCompressed);

//                     for (int i = 0; i < byteSizes.Length; i++)
//                     {
//                         byte[] temp = new byte[byteSizes[i]];

//                         //issue length
//                         Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

//                         offset += temp.Length;

//                         textureData.Add(temp);
//                     }

//                     int faceCountIndex = (int)DDS_HELPER.GetVolumeFaceCount(dds.header.dwDepth, dds.header.dwMipMapCount) - 1;
//                     uint depthCopy = dds.header.dwDepth;

//                     for (int i = 0; i < dds.header.dwMipMapCount; i++)
//                     {
//                         List<byte[]> tempData = [];
//                         for (int j = 0; j < depthCopy; j++)
//                         {
//                             tempData.Add(textureData[faceCountIndex]);
//                             faceCountIndex--;
//                         }
//                         tempData.Reverse();
//                         textureData.InsertRange(0, tempData);

//                         depthCopy = Math.Max(1, depthCopy / 2);
//                     }
//                 }
//             }
//         }

//         //Volume texture archived code
//         // uint depthCopy = d3dtx.GetDepth();
//         // uint mipMap = d3dtx.GetMipMapCount();

//         // int faceCountIndex = d3dtx.GetRegionCount() - 1;
//         // List<byte[]> pixelData = d3dtx.GetPixelData();

//         // for (int i = 0; i < mipMap; i++)
//         // {
//         //     List<byte[]> tempData = [];
//         //     for (int j = 0; j < depthCopy; j++)
//         //     {
//         //         tempData.Add(pixelData[faceCountIndex]);
//         //         faceCountIndex--;
//         //     }
//         //     tempData.Reverse();

//         //     finalData = ByteFunctions.Combine(finalData, tempData.SelectMany(x => x).ToArray());

//         //     depthCopy = Math.Max(1, depthCopy / 2);
//         // }

//         /// <summary>
//     /// Returns the corresponding Telltale surface format from a .dds four-character code.
//     /// </summary>
//     /// <param name="fourCC"></param>
//     /// <param name="dds"></param>
//     /// <returns></returns>
//     public static T3SurfaceFormat Get_T3Format_FromFourCC(uint fourCC, DDS_Master ddsMaster)
//     {
//         if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT1")) return T3SurfaceFormat.eSurface_BC1;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT3")) return T3SurfaceFormat.eSurface_BC2;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DXT5")) return T3SurfaceFormat.eSurface_BC3;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI1")) return T3SurfaceFormat.eSurface_BC4;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("ATI2")) return T3SurfaceFormat.eSurface_BC5;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC4S")) return T3SurfaceFormat.eSurface_BC4;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC4U")) return T3SurfaceFormat.eSurface_BC4;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC5S")) return T3SurfaceFormat.eSurface_BC5;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("BC5U")) return T3SurfaceFormat.eSurface_BC5;
//         else if (fourCC == ByteFunctions.Convert_String_To_UInt32("DX10")) return GetTelltaleSurfaceFormatFromDXGI(ddsMaster.dds.dxt10Header.dxgiFormat);
//         else if (fourCC == 0) return Parse_T3Format_FromD3FORMAT(D3D9FormatConverter.GetD3D9Format(ddsMaster.dds.header.ddspf));
//         else return T3SurfaceFormat.eSurface_BC1;
//     }


//     public static T3SurfaceFormat GetFromT3SurfaceFormatFromD3FORMAT(D3DFORMAT format)
//     {
//         return format switch
//         {
//             D3DFORMAT.UNKNOWN => T3SurfaceFormat.eSurface_Unknown,
//             D3DFORMAT.A8R8G8B8 => T3SurfaceFormat.eSurface_ARGB8,
//             D3DFORMAT.R5G6B5 => T3SurfaceFormat.eSurface_RGB565,
//             D3DFORMAT.A1R5G5B5 => T3SurfaceFormat.eSurface_ARGB1555,
//             D3DFORMAT.A4R4G4B4 => T3SurfaceFormat.eSurface_ARGB4,
//             D3DFORMAT.A8 => T3SurfaceFormat.eSurface_A8,
//             D3DFORMAT.A2B10G10R10 => T3SurfaceFormat.eSurface_ARGB2101010,
//             D3DFORMAT.A8B8G8R8 => T3SurfaceFormat.eSurface_RGBA8,
//             D3DFORMAT.G16R16 => T3SurfaceFormat.eSurface_RG16,
//             D3DFORMAT.A2R10G10B10 => T3SurfaceFormat.eSurface_ARGB2101010,
//             D3DFORMAT.A16B16G16R16 => T3SurfaceFormat.eSurface_RGBA16,
//             D3DFORMAT.L8 => T3SurfaceFormat.eSurface_L8,
//             D3DFORMAT.A8L8 => T3SurfaceFormat.eSurface_AL8,
//             // D3DFORMAT.DXT1 => T3SurfaceFormat.eSurface_DXT1,
//             // D3DFORMAT.DXT2 => T3SurfaceFormat.eSurface_DXT3,
//             // D3DFORMAT.DXT3 => T3SurfaceFormat.eSurface_DXT3,
//             // D3DFORMAT.DXT4 => T3SurfaceFormat.eSurface_DXT5,
//             // D3DFORMAT.DXT5 => T3SurfaceFormat.eSurface_DXT5,
//             // D3DFORMAT.ATI1 => T3SurfaceFormat.eSurface_DXT5A,
//             // D3DFORMAT.ATI2 => T3SurfaceFormat.eSurface_DXN,
//             D3DFORMAT.D24S8 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
//             D3DFORMAT.D24X8 => T3SurfaceFormat.eSurface_Depth24,
//             D3DFORMAT.D24X4S4 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
//             D3DFORMAT.D16_LOCKABLE => T3SurfaceFormat.eSurface_Depth16,
//             D3DFORMAT.D32 => T3SurfaceFormat.eSurface_DepthStencil32,
//             D3DFORMAT.D16 => T3SurfaceFormat.eSurface_Depth16,
//             D3DFORMAT.D32F_LOCKABLE => T3SurfaceFormat.eSurface_Depth32F,
//             D3DFORMAT.D24FS8 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
//             D3DFORMAT.D32_LOCKABLE => T3SurfaceFormat.eSurface_Depth32F,
//             D3DFORMAT.L16 => T3SurfaceFormat.eSurface_L16,
//             D3DFORMAT.R16F => T3SurfaceFormat.eSurface_R16F,
//             D3DFORMAT.G16R16F => T3SurfaceFormat.eSurface_RG16F,
//             D3DFORMAT.A16B16G16R16F => T3SurfaceFormat.eSurface_RGBA16F,
//             D3DFORMAT.R32F => T3SurfaceFormat.eSurface_R32F,
//             D3DFORMAT.G32R32F => T3SurfaceFormat.eSurface_RG32F,
//             D3DFORMAT.A32B32G32R32F => T3SurfaceFormat.eSurface_RGBA32F,
//             D3DFORMAT.A2B10G10R10_XR_BIAS => T3SurfaceFormat.eSurface_RGBA1010102F,
//             _ => T3SurfaceFormat.eSurface_Unknown
//         };
//     }

//     public static D3DFORMAT GetD3D9Format(DDS_PIXELFORMAT ddpf)
//     {
//         if ((ddpf.dwFlags & (uint)DDPF.RGB) != 0)
//         {
//             switch (ddpf.dwRGBBitCount)
//             {
//                 case 32:
//                     if (ddpf.IsBitMask(0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000))
//                     {
//                         return D3DFORMAT.A8R8G8B8;
//                     }
//                     if (ddpf.IsBitMask(0x00ff0000, 0x0000ff00, 0x000000ff, 0))
//                     {
//                         return D3DFORMAT.X8R8G8B8;
//                     }
//                     if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000))
//                     {
//                         return D3DFORMAT.A8B8G8R8;
//                     }
//                     if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0))
//                     {
//                         return D3DFORMAT.X8B8G8R8;
//                     }

//                     // Note that many common DDS reader/writers (including D3DX) swap the
//                     // the RED/BLUE masks for 10:10:10:2 formats. We assume
//                     // below that the 'backwards' header mask is being used since it is most
//                     // likely written by D3DX.

//                     // For 'correct' writers this should be 0x3ff00000,0x000ffc00,0x000003ff for BGR data
//                     if (ddpf.IsBitMask(0x000003ff, 0x000ffc00, 0x3ff00000, 0xc0000000))
//                     {
//                         return D3DFORMAT.A2R10G10B10;
//                     }

//                     // For 'correct' writers this should be 0x000003ff,0x000ffc00,0x3ff00000 for RGB data
//                     if (ddpf.IsBitMask(0x3ff00000, 0x000ffc00, 0x000003ff, 0xc0000000))
//                     {
//                         return D3DFORMAT.A2B10G10R10;
//                     }

//                     if (ddpf.IsBitMask(0x0000ffff, 0xffff0000, 0x00000000, 0x00000000))
//                     {
//                         return D3DFORMAT.G16R16;
//                     }
//                     if (ddpf.IsBitMask(0xffffffff, 0x00000000, 0x00000000, 0x00000000))
//                     {
//                         return D3DFORMAT.R32F; // D3DX writes this out as a FourCC of 114
//                     }
//                     break;

//                 case 24:
//                     if (ddpf.IsBitMask(0xff0000, 0x00ff00, 0x0000ff, 0))
//                     {
//                         return D3DFORMAT.R8G8B8;
//                     }
//                     break;

//                 case 16:
//                     if (ddpf.IsBitMask(0xf800, 0x07e0, 0x001f, 0x0000))
//                     {
//                         return D3DFORMAT.R5G6B5;
//                     }
//                     if (ddpf.IsBitMask(0x7c00, 0x03e0, 0x001f, 0x8000))
//                     {
//                         return D3DFORMAT.A1R5G5B5;
//                     }
//                     if (ddpf.IsBitMask(0x7c00, 0x03e0, 0x001f, 0))
//                     {
//                         return D3DFORMAT.X1R5G5B5;
//                     }
//                     if (ddpf.IsBitMask(0x0f00, 0x00f0, 0x000f, 0xf000))
//                     {
//                         return D3DFORMAT.A4R4G4B4;
//                     }
//                     if (ddpf.IsBitMask(0x0f00, 0x00f0, 0x000f, 0))
//                     {
//                         return D3DFORMAT.X4R4G4B4;
//                     }
//                     if (ddpf.IsBitMask(0x00e0, 0x001c, 0x0003, 0xff00))
//                     {
//                         return D3DFORMAT.A8R3G3B2;
//                     }

//                     // NVTT versions 1.x wrote these as RGB instead of LUMINANCE
//                     if (ddpf.IsBitMask(0xffff, 0, 0, 0))
//                     {
//                         return D3DFORMAT.L16;
//                     }
//                     if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00))
//                     {
//                         return D3DFORMAT.A8L8;
//                     }
//                     break;

//                 case 8:
//                     if (ddpf.IsBitMask(0xe0, 0x1c, 0x03, 0))
//                     {
//                         return D3DFORMAT.R3G3B2;
//                     }

//                     // NVTT versions 1.x wrote these as RGB instead of LUMINANCE
//                     if (ddpf.IsBitMask(0xff, 0, 0, 0))
//                     {
//                         return D3DFORMAT.L8;
//                     }

//                     // Paletted texture formats are typically not supported on modern video cards aka D3DFMT_P8, D3DFMT_A8P8
//                     break;

//                 default:
//                     return D3DFORMAT.UNKNOWN;
//             }
//         }
//         else if ((ddpf.dwFlags & (uint)DDPF.LUMINANCE) != 0)
//         {
//             switch (ddpf.dwRGBBitCount)
//             {
//                 case 16:
//                     if (ddpf.IsBitMask(0xffff, 0, 0, 0) || ddpf.IsBitMask(0xffff, 0xffff, 0xffff, 0))
//                     {
//                         return D3DFORMAT.L16;
//                     }
//                     if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00))
//                     {
//                         return D3DFORMAT.A8L8;
//                     }
//                     break;

//                 case 8:
//                     if (ddpf.IsBitMask(0x0f, 0, 0, 0xf0))
//                     {
//                         return D3DFORMAT.A4L4;
//                     }
//                     if (ddpf.IsBitMask(0xff, 0, 0, 0) || ddpf.IsBitMask(0xff, 0xff, 0xff, 0)) //GIMP for some reason writes this as RGB
//                     {
//                         return D3DFORMAT.L8;
//                     }
//                     if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00) || ddpf.IsBitMask(0x00ff, 0x00ff, 0x00ff, 0)) //GIMP for some reason writes this as RGBA
//                     {
//                         return D3DFORMAT.A8L8; //Some DDS writers assume the bitcount should be 8 instead of 16
//                     }
//                     break;

//                 default:
//                     return D3DFORMAT.UNKNOWN;
//             }
//         }
//         else if ((ddpf.dwFlags & (uint)DDPF.ALPHA) != 0)
//         {
//             if (8 == ddpf.dwRGBBitCount)
//             {
//                 return D3DFORMAT.A8;
//             }
//             if (ddpf.IsBitMask(0xff, 0, 0, 0) || ddpf.IsBitMask(0xff, 0xff, 0xff, 0)) //GIMP for some reason writes this as RGB
//             {
//                 return D3DFORMAT.L8;
//             }
//         }
//         else if ((ddpf.dwFlags & (uint)DDPF.YUV) != 0)
//         {
//             switch (ddpf.dwRGBBitCount)
//             {
//                 case 32:
//                     if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000))
//                     {
//                         return D3DFORMAT.Q8W8V8U8;
//                     }
//                     if (ddpf.IsBitMask(0x0000ffff, 0xffff0000, 0x00000000, 0x00000000))
//                     {
//                         return D3DFORMAT.V16U16;
//                     }
//                     if (ddpf.IsBitMask(0x3ff00000, 0x000ffc00, 0x000003ff, 0xc0000000))
//                     {
//                         return D3DFORMAT.A2W10V10U10;
//                     }
//                     break;

//                 case 16:
//                     if (ddpf.IsBitMask(0x00ff, 0xff00, 0, 0))
//                     {
//                         return D3DFORMAT.V8U8;
//                     }
//                     break;

//                 default:
//                     return D3DFORMAT.UNKNOWN;
//             }
//         }

//         else if ((ddpf.dwFlags & (uint)DDPF.FOURCC) != 0)
//         {
//             if (ByteFunctions.Convert_String_To_UInt32("DXT1") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.DXT1;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("DXT2") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.DXT2;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("DXT3") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.DXT3;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("DXT4") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.DXT4;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("DXT5") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.DXT5;
//             }

//             if (ByteFunctions.Convert_String_To_UInt32("RGBG") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.R8G8_B8G8;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("GRGB") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.G8R8_G8B8;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("UYVY") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.UYVY;
//             }
//             if (ByteFunctions.Convert_String_To_UInt32("YUY2") == ddpf.dwFourCC)
//             {
//                 return D3DFORMAT.YUY2;
//             }

//             // Check for D3DFORMAT enums being set here
//             return ddpf.dwFourCC switch
//             {
//                 (uint)D3DFORMAT.A16B16G16R16 or
//                 (uint)D3DFORMAT.Q16W16V16U16 or
//                 (uint)D3DFORMAT.R16F or
//                 (uint)D3DFORMAT.G16R16F or
//                 (uint)D3DFORMAT.A16B16G16R16F or
//                 (uint)D3DFORMAT.R32F or
//                 (uint)D3DFORMAT.G32R32F or
//                 (uint)D3DFORMAT.A32B32G32R32F or
//                 (uint)D3DFORMAT.CxV8U8 => (D3DFORMAT)ddpf.dwFourCC,
//                 _ => D3DFORMAT.UNKNOWN,
//             };
//         }

//         return D3DFORMAT.UNKNOWN;
//     }

// public static uint GetDXGIBitsPerPixel(DXGI_FORMAT fmt)
//     {
//         switch (fmt)
//         {
//             case DXGI_FORMAT.R32G32B32A32_TYPELESS:
//             case DXGI_FORMAT.R32G32B32A32_FLOAT:
//             case DXGI_FORMAT.R32G32B32A32_UINT:
//             case DXGI_FORMAT.R32G32B32A32_SINT:
//                 return 128;

//             case DXGI_FORMAT.R32G32B32_TYPELESS:
//             case DXGI_FORMAT.R32G32B32_FLOAT:
//             case DXGI_FORMAT.R32G32B32_UINT:
//             case DXGI_FORMAT.R32G32B32_SINT:
//                 return 96;

//             case DXGI_FORMAT.R16G16B16A16_TYPELESS:
//             case DXGI_FORMAT.R16G16B16A16_FLOAT:
//             case DXGI_FORMAT.R16G16B16A16_UNORM:
//             case DXGI_FORMAT.R16G16B16A16_UINT:
//             case DXGI_FORMAT.R16G16B16A16_SNORM:
//             case DXGI_FORMAT.R16G16B16A16_SINT:
//             case DXGI_FORMAT.R32G32_TYPELESS:
//             case DXGI_FORMAT.R32G32_FLOAT:
//             case DXGI_FORMAT.R32G32_UINT:
//             case DXGI_FORMAT.R32G32_SINT:
//             case DXGI_FORMAT.R32G8X24_TYPELESS:
//             case DXGI_FORMAT.D32_FLOAT_S8X24_UINT:
//             case DXGI_FORMAT.R32_FLOAT_X8X24_TYPELESS:
//             case DXGI_FORMAT.X32_TYPELESS_G8X24_UINT:
//             case DXGI_FORMAT.Y416:
//             case DXGI_FORMAT.Y210:
//             case DXGI_FORMAT.Y216:
//                 return 64;

//             case DXGI_FORMAT.R10G10B10A2_TYPELESS:
//             case DXGI_FORMAT.R10G10B10A2_UNORM:
//             case DXGI_FORMAT.R10G10B10A2_UINT:
//             case DXGI_FORMAT.R11G11B10_FLOAT:
//             case DXGI_FORMAT.R8G8B8A8_TYPELESS:
//             case DXGI_FORMAT.R8G8B8A8_UNORM:
//             case DXGI_FORMAT.R8G8B8A8_UNORM_SRGB:
//             case DXGI_FORMAT.R8G8B8A8_UINT:
//             case DXGI_FORMAT.R8G8B8A8_SNORM:
//             case DXGI_FORMAT.R8G8B8A8_SINT:
//             case DXGI_FORMAT.R16G16_TYPELESS:
//             case DXGI_FORMAT.R16G16_FLOAT:
//             case DXGI_FORMAT.R16G16_UNORM:
//             case DXGI_FORMAT.R16G16_UINT:
//             case DXGI_FORMAT.R16G16_SNORM:
//             case DXGI_FORMAT.R16G16_SINT:
//             case DXGI_FORMAT.R32_TYPELESS:
//             case DXGI_FORMAT.D32_FLOAT:
//             case DXGI_FORMAT.R32_FLOAT:
//             case DXGI_FORMAT.R32_UINT:
//             case DXGI_FORMAT.R32_SINT:
//             case DXGI_FORMAT.R24G8_TYPELESS:
//             case DXGI_FORMAT.D24_UNORM_S8_UINT:
//             case DXGI_FORMAT.R24_UNORM_X8_TYPELESS:
//             case DXGI_FORMAT.X24_TYPELESS_G8_UINT:
//             case DXGI_FORMAT.R9G9B9E5_SHAREDEXP:
//             case DXGI_FORMAT.R8G8_B8G8_UNORM:
//             case DXGI_FORMAT.G8R8_G8B8_UNORM:
//             case DXGI_FORMAT.B8G8R8A8_UNORM:
//             case DXGI_FORMAT.B8G8R8X8_UNORM:
//             case DXGI_FORMAT.R10G10B10_XR_BIAS_A2_UNORM:
//             case DXGI_FORMAT.B8G8R8A8_TYPELESS:
//             case DXGI_FORMAT.B8G8R8A8_UNORM_SRGB:
//             case DXGI_FORMAT.B8G8R8X8_TYPELESS:
//             case DXGI_FORMAT.B8G8R8X8_UNORM_SRGB:
//             case DXGI_FORMAT.AYUV:
//             case DXGI_FORMAT.Y410:
//             case DXGI_FORMAT.YUY2:
//                 return 32;

//             case DXGI_FORMAT.P010:
//             case DXGI_FORMAT.P016:
//             case DXGI_FORMAT.V408:
//                 return 24;

//             case DXGI_FORMAT.R8G8_TYPELESS:
//             case DXGI_FORMAT.R8G8_UNORM:
//             case DXGI_FORMAT.R8G8_UINT:
//             case DXGI_FORMAT.R8G8_SNORM:
//             case DXGI_FORMAT.R8G8_SINT:
//             case DXGI_FORMAT.R16_TYPELESS:
//             case DXGI_FORMAT.R16_FLOAT:
//             case DXGI_FORMAT.D16_UNORM:
//             case DXGI_FORMAT.R16_UNORM:
//             case DXGI_FORMAT.R16_UINT:
//             case DXGI_FORMAT.R16_SNORM:
//             case DXGI_FORMAT.R16_SINT:
//             case DXGI_FORMAT.B5G6R5_UNORM:
//             case DXGI_FORMAT.B5G5R5A1_UNORM:
//             case DXGI_FORMAT.A8P8:
//             case DXGI_FORMAT.B4G4R4A4_UNORM:
//             case DXGI_FORMAT.P208:
//             case DXGI_FORMAT.V208:
//                 return 16;

//             case DXGI_FORMAT.NV12:
//             case DXGI_FORMAT.OPAQUE_420:
//             case DXGI_FORMAT.NV11:
//                 return 12;

//             case DXGI_FORMAT.R8_TYPELESS:
//             case DXGI_FORMAT.R8_UNORM:
//             case DXGI_FORMAT.R8_UINT:
//             case DXGI_FORMAT.R8_SNORM:
//             case DXGI_FORMAT.R8_SINT:
//             case DXGI_FORMAT.A8_UNORM:
//             case DXGI_FORMAT.BC2_TYPELESS:
//             case DXGI_FORMAT.BC2_UNORM:
//             case DXGI_FORMAT.BC2_UNORM_SRGB:
//             case DXGI_FORMAT.BC3_TYPELESS:
//             case DXGI_FORMAT.BC3_UNORM:
//             case DXGI_FORMAT.BC3_UNORM_SRGB:
//             case DXGI_FORMAT.BC5_TYPELESS:
//             case DXGI_FORMAT.BC5_UNORM:
//             case DXGI_FORMAT.BC5_SNORM:
//             case DXGI_FORMAT.BC6H_TYPELESS:
//             case DXGI_FORMAT.BC6H_UF16:
//             case DXGI_FORMAT.BC6H_SF16:
//             case DXGI_FORMAT.BC7_TYPELESS:
//             case DXGI_FORMAT.BC7_UNORM:
//             case DXGI_FORMAT.BC7_UNORM_SRGB:
//             case DXGI_FORMAT.AI44:
//             case DXGI_FORMAT.IA44:
//             case DXGI_FORMAT.P8:
//                 return 8;

//             case DXGI_FORMAT.R1_UNORM:
//                 return 1;

//             case DXGI_FORMAT.BC1_TYPELESS:
//             case DXGI_FORMAT.BC1_UNORM:
//             case DXGI_FORMAT.BC1_UNORM_SRGB:
//             case DXGI_FORMAT.BC4_TYPELESS:
//             case DXGI_FORMAT.BC4_UNORM:
//             case DXGI_FORMAT.BC4_SNORM:
//                 return 4;

//             default:
//                 return 0;
//         }
//     }

//     public static uint GetD3D9FORMATBitsPerPifxel(D3DFORMAT fmt)
//     {
//         switch (fmt)
//         {
//             case D3DFORMAT.A32B32G32R32F:
//                 return 128;

//             case D3DFORMAT.A16B16G16R16:
//             case D3DFORMAT.Q16W16V16U16:
//             case D3DFORMAT.A16B16G16R16F:
//             case D3DFORMAT.G32R32F:
//                 return 64;

//             case D3DFORMAT.A8R8G8B8:
//             case D3DFORMAT.X8R8G8B8:
//             case D3DFORMAT.A2B10G10R10:
//             case D3DFORMAT.A8B8G8R8:
//             case D3DFORMAT.X8B8G8R8:
//             case D3DFORMAT.G16R16:
//             case D3DFORMAT.A2R10G10B10:
//             case D3DFORMAT.Q8W8V8U8:
//             case D3DFORMAT.V16U16:
//             case D3DFORMAT.X8L8V8U8:
//             case D3DFORMAT.A2W10V10U10:
//             case D3DFORMAT.D32:
//             case D3DFORMAT.D24S8:
//             case D3DFORMAT.D24X8:
//             case D3DFORMAT.D24X4S4:
//             case D3DFORMAT.D32F_LOCKABLE:
//             case D3DFORMAT.D24FS8:
//             case D3DFORMAT.INDEX32:
//             case D3DFORMAT.G16R16F:
//             case D3DFORMAT.R32F:
//             case D3DFORMAT.D32_LOCKABLE:
//                 return 32;

//             case D3DFORMAT.R8G8B8:
//                 return 24;

//             case D3DFORMAT.A4R4G4B4:
//             case D3DFORMAT.X4R4G4B4:
//             case D3DFORMAT.R5G6B5:
//             case D3DFORMAT.L16:
//             case D3DFORMAT.A8L8:
//             case D3DFORMAT.X1R5G5B5:
//             case D3DFORMAT.A1R5G5B5:
//             case D3DFORMAT.A8R3G3B2:
//             case D3DFORMAT.V8U8:
//             case D3DFORMAT.CxV8U8:
//             case D3DFORMAT.L6V5U5:
//             case D3DFORMAT.G8R8_G8B8:
//             case D3DFORMAT.R8G8_B8G8:
//             case D3DFORMAT.D16_LOCKABLE:
//             case D3DFORMAT.D15S1:
//             case D3DFORMAT.D16:
//             case D3DFORMAT.INDEX16:
//             case D3DFORMAT.R16F:
//             case D3DFORMAT.YUY2:
//             // From DX docs, reference/d3d/enums/d3dformat.asp
//             // (note how it says that D3DFMT_R8G8_B8G8 is "A 16-bit packed RGB format analogous to UYVY (U0Y0, V0Y1, U2Y2, and so on)")
//             case D3DFORMAT.UYVY:
//                 return 16;

//             case D3DFORMAT.R3G3B2:
//             case D3DFORMAT.A8:
//             case D3DFORMAT.A8P8:
//             case D3DFORMAT.P8:
//             case D3DFORMAT.L8:
//             case D3DFORMAT.A4L4:
//             case D3DFORMAT.DXT2:
//             case D3DFORMAT.DXT3:
//             case D3DFORMAT.DXT4:
//             case D3DFORMAT.DXT5:
//             // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/directshow/htm/directxvideoaccelerationdxvavideosubtypes.asp
//             case D3DFORMAT.AI44:
//             case D3DFORMAT.IA44:
//             case D3DFORMAT.S8_LOCKABLE:
//                 return 8;

//             case D3DFORMAT.DXT1:
//                 return 4;

//             case D3DFORMAT.YV12:
//                 return 12;

//             case D3DFORMAT.A1:
//                 return 1;

//             default:
//                 return 0;
//         }
//     }

//     public static uint GetD3D9FORMATChannelCount(D3DFORMAT fmt)
//     {
//         switch (fmt)
//         {
//             case D3DFORMAT.A32B32G32R32F:
//                 return 128;

//             case D3DFORMAT.A16B16G16R16:
//             case D3DFORMAT.Q16W16V16U16:
//             case D3DFORMAT.A16B16G16R16F:
//             case D3DFORMAT.G32R32F:
//                 return 64;

//             case D3DFORMAT.A8R8G8B8:
//             case D3DFORMAT.X8R8G8B8:
//             case D3DFORMAT.A2B10G10R10:
//             case D3DFORMAT.A8B8G8R8:
//             case D3DFORMAT.X8B8G8R8:
//             case D3DFORMAT.G16R16:
//             case D3DFORMAT.A2R10G10B10:
//             case D3DFORMAT.Q8W8V8U8:
//             case D3DFORMAT.V16U16:
//             case D3DFORMAT.X8L8V8U8:
//             case D3DFORMAT.A2W10V10U10:
//             case D3DFORMAT.D32:
//             case D3DFORMAT.D24S8:
//             case D3DFORMAT.D24X8:
//             case D3DFORMAT.D24X4S4:
//             case D3DFORMAT.D32F_LOCKABLE:
//             case D3DFORMAT.D24FS8:
//             case D3DFORMAT.INDEX32:
//             case D3DFORMAT.G16R16F:
//             case D3DFORMAT.R32F:
//             case D3DFORMAT.D32_LOCKABLE:
//                 return 32;

//             case D3DFORMAT.R8G8B8:
//                 return 24;

//             case D3DFORMAT.A4R4G4B4:
//             case D3DFORMAT.X4R4G4B4:
//             case D3DFORMAT.R5G6B5:
//             case D3DFORMAT.L16:
//             case D3DFORMAT.A8L8:
//             case D3DFORMAT.X1R5G5B5:
//             case D3DFORMAT.A1R5G5B5:
//             case D3DFORMAT.A8R3G3B2:
//             case D3DFORMAT.V8U8:
//             case D3DFORMAT.CxV8U8:
//             case D3DFORMAT.L6V5U5:
//             case D3DFORMAT.G8R8_G8B8:
//             case D3DFORMAT.R8G8_B8G8:
//             case D3DFORMAT.D16_LOCKABLE:
//             case D3DFORMAT.D15S1:
//             case D3DFORMAT.D16:
//             case D3DFORMAT.INDEX16:
//             case D3DFORMAT.R16F:
//             case D3DFORMAT.YUY2:
//             // From DX docs, reference/d3d/enums/d3dformat.asp
//             // (note how it says that D3DFMT_R8G8_B8G8 is "A 16-bit packed RGB format analogous to UYVY (U0Y0, V0Y1, U2Y2, and so on)")
//             case D3DFORMAT.UYVY:
//                 return 16;

//             case D3DFORMAT.R3G3B2:
//             case D3DFORMAT.A8:
//             case D3DFORMAT.A8P8:
//             case D3DFORMAT.P8:
//             case D3DFORMAT.L8:
//             case D3DFORMAT.A4L4:
//             case D3DFORMAT.DXT2:
//             case D3DFORMAT.DXT3:
//             case D3DFORMAT.DXT4:
//             case D3DFORMAT.DXT5:
//             // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/directshow/htm/directxvideoaccelerationdxvavideosubtypes.asp
//             case D3DFORMAT.AI44:
//             case D3DFORMAT.IA44:
//             case D3DFORMAT.S8_LOCKABLE:
//                 return 8;

//             case D3DFORMAT.DXT1:
//                 return 4;

//             case D3DFORMAT.YV12:
//                 return 12;

//             case D3DFORMAT.A1:
//                 return 1;

//             default:
//                 return 0;
//         }
//     }

//     public static bool IsTextureFormatCompressed(uint format)
//     {
//         switch (format)
//         {
//             // DXGI formats
//             case (uint)DXGI_FORMAT.BC1_TYPELESS:
//             case (uint)DXGI_FORMAT.BC1_UNORM:
//             case (uint)DXGI_FORMAT.BC1_UNORM_SRGB:
//             case (uint)DXGI_FORMAT.BC2_TYPELESS:
//             case (uint)DXGI_FORMAT.BC2_UNORM:
//             case (uint)DXGI_FORMAT.BC2_UNORM_SRGB:
//             case (uint)DXGI_FORMAT.BC3_TYPELESS:
//             case (uint)DXGI_FORMAT.BC3_UNORM:
//             case (uint)DXGI_FORMAT.BC3_UNORM_SRGB:
//             case (uint)DXGI_FORMAT.BC4_TYPELESS:
//             case (uint)DXGI_FORMAT.BC4_UNORM:
//             case (uint)DXGI_FORMAT.BC4_SNORM:
//             case (uint)DXGI_FORMAT.BC5_TYPELESS:
//             case (uint)DXGI_FORMAT.BC5_UNORM:
//             case (uint)DXGI_FORMAT.BC5_SNORM:
//             case (uint)DXGI_FORMAT.BC6H_TYPELESS:
//             case (uint)DXGI_FORMAT.BC6H_UF16:
//             case (uint)DXGI_FORMAT.BC6H_SF16:
//             case (uint)DXGI_FORMAT.BC7_TYPELESS:
//             case (uint)DXGI_FORMAT.BC7_UNORM:
//             case (uint)DXGI_FORMAT.BC7_UNORM_SRGB:

//             // D3D9 formats
//             case (uint)D3DFORMAT.DXT1:
//             case (uint)D3DFORMAT.DXT2:
//             case (uint)D3DFORMAT.DXT3:
//             case (uint)D3DFORMAT.DXT4:
//             case (uint)D3DFORMAT.DXT5:

//             // FourCC formats
//             // Note: Other FourCC compressed formats do exist, but they are rare: https://github.com/microsoft/DirectXTex/blob/fa22a4ec53dcc67505e66eca0c788ad8feed6b34/DirectXTex/DirectXTexDDS.cpp#L60
//             // TODO: Refactor code
//             case 0x31495441: // "ATI1"
//             case 0x32495441: // "ATI2"
//             case 0x55344342: // "BC4U"
//             case 0x53344342: // "BC4S"
//             case 0x55354342: // "BC5U"
//             case 0x53354342: // "BC5S"
//                 return true;
//         }

//         return false;
//     }

//     public static uint GetDXGICompressionBlockSize(uint format)
//     {
//         switch (format)
//         {
//             case (uint)D3DFORMAT.DXT1:
//             case (uint)DXGI_FORMAT.BC1_TYPELESS:
//             case (uint)DXGI_FORMAT.BC1_UNORM:
//             case (uint)DXGI_FORMAT.BC1_UNORM_SRGB:
//             case (uint)DXGI_FORMAT.BC4_TYPELESS:
//             case (uint)DXGI_FORMAT.BC4_UNORM:
//             case (uint)DXGI_FORMAT.BC4_SNORM:
//             case 0x31495441: // "ATI1"
//             case 0x55344342: // "BC4U"
//             case 0x53344342: // "BC4S"
//                 return 8;
//         }

//         return 16;
//     }
//  public static uint GetDDSBlockSize(DDS_HEADER header, DDS_HEADER_DXT10 dx10_header)
//     {
//         uint compressionValue = header.ddspf.dwFourCC;

//         if (compressionValue == ByteFunctions.Convert_String_To_UInt32("DX10"))
//         {
//             compressionValue = (uint)dx10_header.dxgiFormat;
//         }

//         return GetDXGICompressionBlockSize(compressionValue);
//     }

//     public static uint[,] CalculateMipResolutions(uint mipCount, uint width, uint height)
//     {
//         //because I suck at math, we will generate our mip map resolutions using the same method we did in d3dtx to dds (can't figure out how to calculate them in reverse properly)
//         //first [] is the "resolution" index, and the second [] always has a length of 2, and contains the width and height
//         uint[,] mipResolutions = new uint[mipCount, 2];

//         //get our mip image dimensions (have to multiply by 2 as the mip calculations will be off by half)
//         uint mipImageWidth = width * 2;
//         uint mipImageHeight = height * 2;

//         //add the resolutions in reverse ( largest mipmap - first index, smallest mipmap will be last index)
//         for (int i = 0; i < mipCount; i++)
//         {
//             //divide the resolutions by 2
//             mipImageWidth = Math.Max(1, mipImageWidth / 2);
//             mipImageHeight = Math.Max(1, mipImageHeight / 2);

//             //assign the resolutions
//             mipResolutions[i, 0] = mipImageWidth;
//             mipResolutions[i, 1] = mipImageHeight;
//         }

//         return mipResolutions;
//     }
//      /// <summary>
//     /// Calculates the mip resolutions for a volume texture.
//     /// </summary>
//     /// <param name="mipCount">The number of mip levels.</param>
//     /// <param name="width">The width of the base level texture.</param>
//     /// <param name="height">The height of the base level texture.</param>
//     /// <param name="depth">The depth of the base level texture.</param>
//     /// <returns>A 3D array containing the mip resolutions for each level.</returns>
//     public static uint[,] CalculateVolumeMipResolutions(uint mipCount, uint width, uint height, uint depth)
//     {
//         uint count = GetVolumeFaceCount(depth, mipCount);

//         uint[,] mipResolutions = new uint[count, 2];
//         uint mipImageWidth = width;
//         uint mipImageHeight = height;

//         uint depthCopy = depth;

//         for (int i = 0; i < mipCount; i++)
//         {
//             for (int j = 0; j < depthCopy; j++)
//             {
//                 mipResolutions[i, 0] = mipImageWidth;
//                 mipResolutions[i, 1] = mipImageHeight;
//             }

//             mipImageWidth = Math.Max(1, mipImageWidth / 2);
//             mipImageHeight = Math.Max(1, mipImageHeight / 2);
//             depthCopy = Math.Max(1, depthCopy / 2);
//         }

//         return mipResolutions;
//     }


//     public static uint[] GetImageByteSizes(uint[,] mipResolutions, uint baseLinearSize, uint bitPixelSize, bool isCompressed)
//     {
//         uint[] byteSizes = new uint[mipResolutions.GetLength(0)];

//         //Get the byte sizes for each mip map, first index - largest mip map, last index - smallest mip map
//         for (int i = 0; i < byteSizes.Length; i++)
//         {
//             uint mipWidth = mipResolutions[i, 0];
//             uint mipHeight = mipResolutions[i, 1];

//             // It works for square textures
//             byteSizes[i] = CalculateByteSize(mipWidth, mipHeight, bitPixelSize, isCompressed);
//             Console.WriteLine("Mip " + i + " size: " + byteSizes[i]);

//             // This is outdated code, used only for reference
//             // if (mipWidth == mipHeight) //SQUARE SIZE
//             // {
//             //     computed linear size
//             //     (mipWidth * mipWidth) / 2

//             //     byteSizes[i] = Calculate_ByteSize_Square(mipWidth, mipHeight, baseLinearSize, (uint)i, (uint)byteSizes.Length, blockSize);
//             //     byteSizes[i] = Calculate_ByteSize(mipWidth, mipHeight, blockSize);
//             // }   
//             // else //NON SQUARE
//             // {
//             //     byteSizes[i] = Calculate_ByteSize_NonSquare(mipWidth, mipHeight, blockSize);
//             // }
//             //
//             // original calculation
//             // byteSizes[i] = CalculateDDS_ByteSize((int)mipResolutions[i, 0], (int)mipResolutions[i, 1], isDXT1);
//         }

//         return byteSizes;
//     }

//     public static uint[] GetVolumeImageByteSizes(uint[,,] mipResolutions, uint bitPixelSize, bool isCompressed)
//     {
//         uint[] byteSizes = new uint[mipResolutions.GetLength(0)];

//         //Get the byte sizes for each mip map, first index - largest mip map, last index - smallest mip map
//         for (int i = 0; i < byteSizes.Length; i++)
//         {
//             uint mipWidth = mipResolutions[i, 0, 0];
//             uint mipHeight = mipResolutions[i, 0, 1];

//             // It works for square textures
//             byteSizes[i] = CalculateByteSize(mipWidth, mipHeight, bitPixelSize, isCompressed);
//             Console.WriteLine("Mip " + i + " size: " + byteSizes[i]);
//         }

//         return byteSizes;
//     }

// public static uint GetVolumeFaceCount(uint depth, uint mipCount)
// {
//     uint faceCount = 0;

//     for (int i = 0; i < mipCount; i++)
//     {
//         faceCount += depth;
//         depth = Math.Max(1, depth / 2);
//     }

//     return faceCount;
// }

// /// <summary>
// /// Calculates the byte size of a DDS texture
// /// </summary>
// /// <param name="width"></param>
// /// <param name="height"></param>
// /// <param name="isDXT1"></param>
// /// <returns></returns>
// public static uint CalculateByteSize(uint width, uint height, uint bitPixelSize, bool isCompressed)
// {
//     //formula (from microsoft docs)
//     //max(1, ( (width + 3) / 4 ) ) x max(1, ( (height + 3) / 4 ) ) x 8(DXT1) or 16(DXT2-5)

//     //formula (from here) - http://doc.51windows.net/directx9_sdk/graphics/reference/DDSFileReference/ddstextures.htm
//     //max(1,width ?4)x max(1,height ?4)x 8 (DXT1) or 16 (DXT2-5)

//     //do the micorosoft magic texture byte size calculation formula

//     if (isCompressed)
//     {
//         return Math.Max(1, ((width + 3) / 4)) * Math.Max(1, ((height + 3) / 4)) * bitPixelSize;
//     }
//     else
//     {
//         return width * height * bitPixelSize;
//     }

//     //formula (from here) - http://doc.51windows.net/directx9_sdk/graphics/reference/DDSFileReference/ddstextures.htm
//     //return Math.Max(1, width / 4) * Math.Max(1, height / 4) * bitPixelSize;
// }
//     //TODO ADD GET BITS PER PIXEL TO REFACTOR
// public static uint GetPitchOrLinearSizeFromD3DTX(T3SurfaceFormat format, uint width)
// {
//     if (D3DTX_Master.IsTextureCompressed(format))
//     {
//         return Math.Max(1, (width + 3) / 4 * D3DTX_Master.GetD3DTXBlockSize(format));
//     }
//     // check for legacy formats
//     else return (width * D3DTX_Master.GetBitsPerPixel(format) + 7) / 8;
// }

// public static T3SurfaceFormat Parse_T3Format_FromD3FORMAT(D3DFORMAT d3dformat_format)
// {
//     //TODO Check if other formats are needed
//     return (int)d3dformat_format switch
//     {
//         (int)D3DFORMAT.A8R8G8B8 => T3SurfaceFormat.eSurface_ARGB8,
//         (int)D3DFORMAT.X8R8G8B8 => T3SurfaceFormat.eSurface_ARGB8,
//         (int)D3DFORMAT.A16B16G16R16 => T3SurfaceFormat.eSurface_ARGB16,
//         (int)D3DFORMAT.R5G6B5 => T3SurfaceFormat.eSurface_RGB565,
//         (int)D3DFORMAT.A1R5G5B5 => T3SurfaceFormat.eSurface_ARGB1555,
//         (int)D3DFORMAT.A4R4G4B4 => T3SurfaceFormat.eSurface_ARGB4,
//         (int)D3DFORMAT.A2B10G10R10 => T3SurfaceFormat.eSurface_ARGB2101010,
//         (int)D3DFORMAT.G16R16 => T3SurfaceFormat.eSurface_RG16,
//         //  (int)D3DFORMAT.A16B16G16R16 => T3SurfaceFormat.eSurface_RGBA16, swap color channels?
//         (int)D3DFORMAT.A8B8G8R8 => T3SurfaceFormat.eSurface_RGBA8,
//         //  (int)D3DFORMAT.X8R8G8B8 => T3SurfaceFormat.eSurface_RGBA8,
//         (int)D3DFORMAT.D32 => T3SurfaceFormat.eSurface_DepthStencil32,
//         (int)D3DFORMAT.A32B32G32R32F => T3SurfaceFormat.eSurface_RGBA32F,
//         //(int)D3DFORMAT.A8 => T3SurfaceFormat.eSurface_R8, check channels?
//         // (int)D3DFORMAT.A8R8G8B8 => T3SurfaceFormat.eSurface_RGBA8S,
//         (int)D3DFORMAT.A8 => T3SurfaceFormat.eSurface_A8,
//         //(int)D3DFORMAT.D3DFMT_R8 =>T3SurfaceFormat.eSurface_L8,
//         //(int)D3DFORMAT.D3DFMT_G8R8 => T3SurfaceFormat.eSurface_AL8,
//         //(int) D3DFORMAT.D3DFMT_R16 => T3SurfaceFormat.eSurface_L16,
//         //(int)D3DFORMAT.D3DFMT_A16B16G16R16=>T3SurfaceFormat.eSurface_RGBA16S,
//         (int)D3DFORMAT.R16F => T3SurfaceFormat.eSurface_R16F,
//         (int)D3DFORMAT.A16B16G16R16F => T3SurfaceFormat.eSurface_RGBA16F,
//         (int)D3DFORMAT.R32F => T3SurfaceFormat.eSurface_R32F,
//         (int)D3DFORMAT.G32R32F => T3SurfaceFormat.eSurface_RG32F,
//         //(int)D3DFORMAT.D3DFMT_A32B32G32R32F=>T3SurfaceFormat.eSurface_RGBA32F,
//         //TODO SAME HERE, IS IT INT?
//         // (int)D3DFORMAT.D3DFMT_A2B10G10R10=>T3SurfaceFormat.eSurface_RGBA1010102F,
//         // (int)D3DFORMAT.R11G11B10 => T3SurfaceFormat.eSurface_RGB111110F,
//         (int)D3DFORMAT.D16 => T3SurfaceFormat.eSurface_Depth16, //check for maps
//         (int)D3DFORMAT.D24S8 => T3SurfaceFormat.eSurface_Depth24, //check for maps
//                                                                   //??
//                                                                   //(int)D3DFORMAT.D3DFMT_D16 =>T3SurfaceFormat.eSurface_Depth16,
//                                                                   //(int)D3DFORMAT.D3DFMT_D24S8 => T3SurfaceFormat.eSurface_Depth24,
//         (int)D3DFORMAT.D32F_LOCKABLE => T3SurfaceFormat.eSurface_Depth32F,
//         // (int)D3DFORMAT.D32FS8_TEXTURE => T3SurfaceFormat.eSurface_Depth32F_Stencil8,
//         (int)D3DFORMAT.D24FS8 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
//         //TODO ADD BC1, BC2, BC3, BC4, BC5, BC6H, BC7
//         (int)D3DFORMAT.DXT1 => T3SurfaceFormat.eSurface_BC1,
//         (int)D3DFORMAT.DXT2 => T3SurfaceFormat.eSurface_BC1,
//         (int)D3DFORMAT.DXT3 => T3SurfaceFormat.eSurface_BC2,
//         (int)D3DFORMAT.DXT4 => T3SurfaceFormat.eSurface_BC2,
//         (int)D3DFORMAT.DXT5 => T3SurfaceFormat.eSurface_BC3, //alpha?
//         _ => T3SurfaceFormat.eSurface_Unknown,
//     };
// }

// public static DDS_DirectXTexNet_ImageSection[] GetDDSImageSections(string ddsFilePath)
// {
//     ScratchImage ddsImage = TexHelper.Instance.LoadFromDDSFile(ddsFilePath, DDS_FLAGS.NONE);

//     DDS_DirectXTexNet_ImageSection[] section = new DDS_DirectXTexNet_ImageSection[ddsImage.GetImageCount()];

//     for (int i = 0; i < section.Length; i++)
//     {
//         Image image = ddsImage.GetImage(i);

//         var pixelPointer = ddsImage.GetImage(i).Pixels;

//         byte[] pixels = new byte[image.SlicePitch];

//         Marshal.Copy(pixelPointer, pixels, 0, pixels.Length);

//         section[i] = new()
//         {
//             Width = image.Width,
//             Height = image.Height,
//             Format = image.Format,
//             SlicePitch = image.SlicePitch,
//             RowPitch = image.RowPitch,
//             Pixels = pixels
//         };

//         Console.WriteLine($"Image {i} - Width: {section[i].Width}, Height: {section[i].Height}, Format: {section[i].Format}, SlicePitch: {section[i].SlicePitch}, RowPitch: {section[i].RowPitch}");
//         Console.WriteLine($"Image {i} - Pixels: {section[i].Pixels.Length}");
//     }

//     return section;
// }

// /// <summary>
// /// Converts a DXGI format to an sRGB format.
// /// </summary>
// /// <param name="format"></param>
// /// <returns></returns>
// public static DXGI_FORMAT GetSRGBFormat(DXGI_FORMAT dxgiFormat)
// {
//     return TexHelper.Instance.MakeSRGB(dxgiFormat);
// }
//     public static byte[] GetDDSByteArray(string ddsFilePath, DDS_FLAGS flags = DDS_FLAGS.NONE)
// {
//     var ddsFile = TexHelper.Instance.LoadFromDDSFile(ddsFilePath, DDS_FLAGS.NONE);

//     var ddsMemory = ddsFile.SaveToDDSMemory(DDS_FLAGS.FORCE_DX9_LEGACY);

//     ddsMemory.Position = 0;

//     // Create a byte array to hold the data
//     byte[] ddsArray = new byte[ddsMemory.Length];

//     // Read the data from the UnmanagedMemoryStream into the byte array
//     ddsMemory.Read(ddsArray, 0, ddsArray.Length);
//     ddsMemory.Close();
//     return ddsArray;
// }
//    static DDS_DirectXTexNet()
// {
//     string solutionDir = AppDomain.CurrentDomain.BaseDirectory;
//     string texconvApplicationDirectoryPath =
//         Path.Combine(solutionDir, "ExternalDependencies", "DirectXTexNetImpl.dll");
//     Console.WriteLine(texconvApplicationDirectoryPath);
//     //TexHelper.LoadInstanceFrom(texconvApplicationDirectoryPath);
//     Console.WriteLine("DirectXTexNet Loaded");
// }

// public static void SaveToDDSFile(string ddsFilePath, DDS_FLAGS flags = DDS_FLAGS.NONE)
// {
//     ScratchImage ddsImage = TexHelper.Instance.LoadFromDDSFile(ddsFilePath, DDS_FLAGS.NONE);

//     ddsImage.SaveToDDSFile(flags, ddsFilePath);
// }

// public static DXGI_FORMAT GetDDSImageDXGI(byte[] array, DDS_FLAGS flags = DDS_FLAGS.NONE)
// {
//     GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
//     try
//     {
//         // Obtain a pointer to the data
//         IntPtr ptr = handle.AddrOfPinnedObject();
//         var image = TexHelper.Instance.LoadFromDDSMemory(ptr, array.Length, flags);

//         return image.GetMetadata().Format;
//     }
//     finally
//     {
//         // Release the handle to allow the garbage collector to reclaim the memory
//         handle.Free();
//     }
// }

// public void GetData(DDS_DirectXTexNet_ImageSection[] imageSections)
// {
//     textureData = [];
//     IEnumerable<DDS_DirectXTexNet_ImageSection> newImageSections = imageSections.OrderBy(section => section.Pixels.Length);

//     foreach (DDS_DirectXTexNet_ImageSection imageSection in newImageSections)
//     {
//         textureData.Add(imageSection.Pixels);
//     }

//     // int counter = 0;
//     // foreach (DDS_DirectXTexNet_ImageSection imageSection in newImageSections)
//     // {
//     //     Console.WriteLine("DDS Image Section {0}", counter);
//     //     Console.WriteLine("DDS Texture Byte Size = {0}", imageSection.Pixels.Length);
//     //     Console.WriteLine("DDS Texture Width = {0}", imageSection.Width);
//     //     Console.WriteLine("DDS Texture Height = {0}", imageSection.Height);
//     //     Console.WriteLine("DDS Texture Format = {0}", imageSection.Format.ToString());
//     //     Console.WriteLine("DDS Row Pitch = {0}", imageSection.RowPitch);
//     //     Console.WriteLine("DDS Slice Pitch = {0}", imageSection.SlicePitch);
//     //     counter++;
//     // }
// }
// public T3TextureLayout GetTextureLayout()
//         {
//             if (d3dtx3 != null)
//             {
//                 if (d3dtx3.mRegionHeaders.Length == 6 * d3dtx3.mNumMipLevels)
//                 {
//                     return T3TextureLayout.eTextureLayout_Cube;
//                 }

//                 if (d3dtx3.mRegionHeaders.Length == d3dtx3.mNumMipLevels)
//                 {
//                     return T3TextureLayout.eTextureLayout_2D;
//                 }

//                 return T3TextureLayout.eTextureLayout_3D; //Cube textures may not be in the game, but if they are, it's supported
//             }
//             else if (d3dtx4 != null)
//             {
//                 if (d3dtx4.mRegionHeaders.Length == 6 * d3dtx4.mNumMipLevels)
//                 {
//                     return T3TextureLayout.eTextureLayout_Cube;
//                 }

//                 if (d3dtx4.mRegionHeaders.Length == d3dtx4.mNumMipLevels)
//                 {
//                     return T3TextureLayout.eTextureLayout_2D;
//                 }

//                 return T3TextureLayout.eTextureLayout_3D; //Cube textures may not be in the game, but if they are, it's supported
//             }
//             else if (d3dtx5 != null)
//                 return d3dtx5.mTextureLayout;
//             // else if (d3dtx6 != null)
//             // return d3dtx6.mTextureLayout;
//             else if (d3dtx7 != null)
//                 return d3dtx7.mTextureLayout;
//             else if (d3dtx8 != null)
//                 return d3dtx8.mTextureLayout;
//             else if (d3dtx9 != null)
//                 return d3dtx9.mTextureLayout;
//             else
//                 return T3TextureLayout.eTextureLayout_2D;
//         }

//         public uint TotalTextureDataWithMipMaps(uint mipmaps, uint height, uint width, uint bitsPerPixel)
//         {
//             uint total = 0;
//             for (int i = 0; i < mipmaps; i++)
//             {
//                 total += (height * width * bitsPerPixel) / 8;
//                 height /= 2;
//                 width /= 2;
//             }

//             return total;
//         }
// public static uint GetD3DTXBlockSize(T3SurfaceFormat format)
//         {
//             if (IsTextureCompressed(format))
//             {
//                 return format switch
//                 {
//                     T3SurfaceFormat.eSurface_BC1 => 8,
//                     T3SurfaceFormat.eSurface_BC3 => 8,
//                     _ => 16,
//                 };
//             }

//             return 0;
//         }

//         public static uint GetBitsPerPixel(T3SurfaceFormat surfaceFormat)
//         {
//             switch (surfaceFormat)
//             {
//                 case T3SurfaceFormat.eSurface_L8:
//                 case T3SurfaceFormat.eSurface_A8:
//                 case T3SurfaceFormat.eSurface_L16:
//                     return 8;

//                 case T3SurfaceFormat.eSurface_RG8:
//                 case T3SurfaceFormat.eSurface_AL8:
//                 case T3SurfaceFormat.eSurface_R16:
//                 case T3SurfaceFormat.eSurface_R16F:
//                 case T3SurfaceFormat.eSurface_R16UI:
//                 case T3SurfaceFormat.eSurface_RGB565:
//                 case T3SurfaceFormat.eSurface_ARGB1555:
//                 case T3SurfaceFormat.eSurface_ARGB4:
//                     return 16;

//                 case T3SurfaceFormat.eSurface_ARGB8:
//                 case T3SurfaceFormat.eSurface_RGBA8:
//                 case T3SurfaceFormat.eSurface_ARGB2101010:
//                 case T3SurfaceFormat.eSurface_RG16:
//                 case T3SurfaceFormat.eSurface_RG16S:
//                 case T3SurfaceFormat.eSurface_RGBA16S:
//                 case T3SurfaceFormat.eSurface_RG16UI:
//                 case T3SurfaceFormat.eSurface_RGBA8S:
//                 case T3SurfaceFormat.eSurface_RGBA1010102F:
//                 case T3SurfaceFormat.eSurface_RGB111110F:
//                 case T3SurfaceFormat.eSurface_RGB9E5F:
//                 case T3SurfaceFormat.eSurface_RG16F:
//                 case T3SurfaceFormat.eSurface_R32:
//                 case T3SurfaceFormat.eSurface_R32F:
//                     return 32;

//                 case T3SurfaceFormat.eSurface_ARGB16:
//                 case T3SurfaceFormat.eSurface_RGBA16:
//                 case T3SurfaceFormat.eSurface_RG32:
//                 case T3SurfaceFormat.eSurface_RG32F:
//                     return 64; // 16 bits per channel * 4 channels (RGBA)

//                 case T3SurfaceFormat.eSurface_RGBA32:
//                 case T3SurfaceFormat.eSurface_RGBA32F:
//                     return 128; // 32 bits per channel * 4 channels (RGBA)

//                 default:
//                     return 0; // Unknown format or unsupported format
//             }
//         }

//         public List<byte[]> GetPixelDataByFaceIndex(int faceIndex, int mipLevel = 1)
//         {
//             List<byte[]> newPixelData = [];

//             if (d3dtx5 != null)
//             {
//                 for (int i = 0; i < d3dtx5.mRegionHeaders.Length; i++)
//                 {
//                     if (d3dtx5.mRegionHeaders[i].mFaceIndex == faceIndex)
//                     {
//                         newPixelData.Add(d3dtx5.mPixelData[i]);
//                     }
//                     {
//                         newPixelData.Add(d3dtx5.mPixelData[i]);
//                     }
//                 }
//             }
//             if (d3dtx7 != null)
//             {
//                 for (int i = 0; i < d3dtx7.mRegionHeaders.Length; i++)
//                 {
//                     if (d3dtx7.mRegionHeaders[i].mFaceIndex == faceIndex)
//                     {
//                         newPixelData.Add(d3dtx7.mPixelData[i]);
//                     }
//                 }
//             }
//             else if (d3dtx8 != null)
//             {
//                 for (int i = 0; i < d3dtx8.mRegionHeaders.Length; i++)
//                 {
//                     if (d3dtx8.mRegionHeaders[i].mFaceIndex == faceIndex)
//                     {
//                         newPixelData.Add(d3dtx8.mPixelData[i]);
//                     }
//                 }
//             }
//             else if (d3dtx9 != null)
//             {
//                 for (int i = 0; i < d3dtx9.mRegionHeaders.Length; i++)
//                 {
//                     if (d3dtx9.mRegionHeaders[i].mFaceIndex == faceIndex)
//                     {
//                         newPixelData.Add(d3dtx9.mPixelData[i]);
//                     }
//                 }
//             }

//             return newPixelData;
//         }
//     }
/// <summary>
/// Gets the channel count of .d3dtx surface format. Needs verification for some formats. eTxColor could also play part for the unknown formats.
/// </summary>
/// <param name="format"></param>
/// <param name="alpha"></param>
/// <returns></returns>
// public static uint GetChannelCount(T3SurfaceFormat format, eTxAlpha alpha = eTxAlpha.eTxAlphaUnknown)
// {
//     switch (format)
//     {
//         default:
//             return 0;

//         case T3SurfaceFormat.eSurface_R8:
//         case T3SurfaceFormat.eSurface_A8:
//         case T3SurfaceFormat.eSurface_L8:
//         case T3SurfaceFormat.eSurface_R16:
//         case T3SurfaceFormat.eSurface_R32:
//         case T3SurfaceFormat.eSurface_R16UI:
//         case T3SurfaceFormat.eSurface_R16F:
//         case T3SurfaceFormat.eSurface_R32F:
//         case T3SurfaceFormat.eSurface_BC4:
//         case T3SurfaceFormat.eSurface_ETC2_R: //Needs verification
//         case T3SurfaceFormat.eSurface_Depth16:
//         case T3SurfaceFormat.eSurface_Depth24:
//         case T3SurfaceFormat.eSurface_DepthPCF16: //Percentage-closer filtering?
//         case T3SurfaceFormat.eSurface_DepthPCF24: //Percentage-closer filtering
//         case T3SurfaceFormat.eSurface_DepthStencil32:
//         case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
//         case T3SurfaceFormat.eSurface_Depth32F:
//         case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
//             return 1;

//         case T3SurfaceFormat.eSurface_RG16:
//         case T3SurfaceFormat.eSurface_RG32:
//         case T3SurfaceFormat.eSurface_RG16UI:
//         case T3SurfaceFormat.eSurface_RG16S:
//         case T3SurfaceFormat.eSurface_RG16F:
//         case T3SurfaceFormat.eSurface_RG32F:
//         case T3SurfaceFormat.eSurface_AL8:
//         case T3SurfaceFormat.eSurface_BC5:
//         case T3SurfaceFormat.eSurface_ETC2_RG: //Needs verification
//             return 2;

//         case T3SurfaceFormat.eSurface_RGB565:
//         case T3SurfaceFormat.eSurface_RGB111110F:
//         case T3SurfaceFormat.eSurface_RGB9E5F:
//         case T3SurfaceFormat.eSurface_BC6:
//         case T3SurfaceFormat.eSurface_ETC1_RGB:
//         case T3SurfaceFormat.eSurface_ETC2_RGB:
//         case T3SurfaceFormat.eSurface_PVRTC2: //Needs verification
//         case T3SurfaceFormat.eSurface_PVRTC4: //Needs verification
//         case T3SurfaceFormat.eSurface_ATC_RGB:
//         case T3SurfaceFormat.eSurface_FrontBuffer: //Needs verification
//         case T3SurfaceFormat.eSurface_CTX1://Needs verification
//             return 3;

//         case T3SurfaceFormat.eSurface_ARGB8:
//         case T3SurfaceFormat.eSurface_ARGB16:
//         case T3SurfaceFormat.eSurface_ARGB1555:
//         case T3SurfaceFormat.eSurface_ARGB4:
//         case T3SurfaceFormat.eSurface_ARGB2101010:
//         case T3SurfaceFormat.eSurface_RGBA16:
//         case T3SurfaceFormat.eSurface_RGBA8:
//         case T3SurfaceFormat.eSurface_RGBA32:
//         case T3SurfaceFormat.eSurface_RGBA8S:
//         case T3SurfaceFormat.eSurface_RGBA16S:
//         case T3SurfaceFormat.eSurface_RGBA16F:
//         case T3SurfaceFormat.eSurface_RGBA32F:
//         case T3SurfaceFormat.eSurface_RGBA1010102F:
//         case T3SurfaceFormat.eSurface_BC2:
//         case T3SurfaceFormat.eSurface_BC3:
//         case T3SurfaceFormat.eSurface_ETC2_RGB1A:
//         case T3SurfaceFormat.eSurface_ETC2_RGBA:
//         case T3SurfaceFormat.eSurface_PVRTC2a: //Needs verification
//         case T3SurfaceFormat.eSurface_PVRTC4a: //Needs verification
//         case T3SurfaceFormat.eSurface_ATC_RGB1A:
//         case T3SurfaceFormat.eSurface_ATC_RGBA:
//         case T3SurfaceFormat.eSurface_ATSC_RGBA_4x4: //Needs verification
//             return 4;

//         case T3SurfaceFormat.eSurface_BC1: //Needs checking (alpha is optional)
//         case T3SurfaceFormat.eSurface_BC7: //Needs checking (alpha is optional)
//             if (alpha > 0)
//             {
//                 return 4;
//             }
//             return 3;
//         case T3SurfaceFormat.eSurface_Count: //Needs verification
//         case T3SurfaceFormat.eSurface_Unknown:
//             return 0;
//     }
// }
// using D3DTX_Converter.TelltaleEnums;

// namespace D3DTX_Converter.TelltaleTypes
// {
//     public class EnumPlatformType
//     {
//         public static PlatformType GetPlatformType(int value)
//         {
//             switch (value)
//             {
//                 default: return PlatformType.ePlatform_All;
//                 case 0: return PlatformType.ePlatform_None;
//                 case 1: return PlatformType.ePlatform_All;
//                 case 2: return PlatformType.ePlatform_PC;
//                 case 3: return PlatformType.ePlatform_Wii;
//                 case 4: return PlatformType.ePlatform_Xbox;
//                 case 5: return PlatformType.ePlatform_PS3;
//                 case 6: return PlatformType.ePlatform_Mac;
//                 case 7: return PlatformType.ePlatform_iPhone;
//                 case 8: return PlatformType.ePlatform_Android;
//                 case 9: return PlatformType.ePlatform_Vita;
//                 case 10: return PlatformType.ePlatform_Linux;
//                 case 11: return PlatformType.ePlatform_PS4;
//                 case 12: return PlatformType.ePlatform_XBOne;
//                 case 13: return PlatformType.ePlatform_WiiU;
//                 case 14: return PlatformType.ePlatform_Win10;
//                 case 15: return PlatformType.ePlatform_NX;
//                 case 16: return PlatformType.ePlatform_Count;
//             }
//         }
//     }
// }
// namespace D3DTX_Converter.TelltaleFunctions;

// public static class T3Texture
// {
//     public static string GetTextureTypeName(int mType)
//     {
//         switch (mType)
//         {
//             case 0: return "Unknown";
//             case 1: return "LightMapV0";
//             case 2: return "BumpMap";
//             case 3: return "NormalMap";
//             case 4: return "UNUSED0";
//             case 5: return "UNUSED1";
//             case 6: return "SubsurfaceScatteringMapV0";
//             case 7: return "SubsurfaceScatteringMap";
//             case 8: return "DetailMap";
//             case 9: return "StaticShadowMap";
//             case 10: return "LightmapHDR";
//             case 11: return "SharpDetailMap";
//             case 12: return "EnvMap";
//             case 13: return "SpecularColorMap";
//             case 14: return "ToonLookupMap";
//             case 15: return "DiffuseColorMap";
//             case 16: return "OutlineMap";
//             case 17: return "LightmapHDRScaled";
//             case 18: return "EmissiveMap";
//             case 19: return "ParticleProperties";
//             case 20: return "BrushNormalMap";
//             case 21: return "UNUSED2";
//             case 22: return "NormalGlossMap";
//             case 23: return "LookupMap";
//             case 40: return "LookupXYMap";
//             case 24: return "AmbientOcclusionMap";
//             case 25: return "PrefilteredEnvCubeMapHDR";
//             case 35: return "PrefilteredEnvCubeMapHDRScaled";
//             case 26: return "BrushLookupMap";
//             case 27: return "Vector2Map";
//             case 28: return "NormalDxDyMap";
//             case 29: return "PackedSDFMap";
//             case 30: return "SingleChannelSDFMap";
//             case 31: return "LightmapDirection";
//             case 32: return "LightmapStaticShadows";
//             case 33: return "LightStaticShadowMapAtlas";
//             case 34: return "LightStaticShadowMap";
//             case 38: return "NormalXYMap";
//             case 41: return "ObjectNormalMap";
//             default: return "InvalidMap";
//         }
//     }
// }
    // /// <summary>
    // /// Combines two byte arrays into one. Used for cubefaces. Memory intensive.
    // /// </summary>
    // /// <param name="first"></param>
    // /// <param name="second"></param>
    // /// <returns></returns>
    // public static byte[] CombineCubeface(byte[] first, List<byte[]> second)
    // {
    //     byte[] bytes = [];

    //     //allocate a byte array with both total lengths combined to accomodate both
    //     for (int i = second.Count - 1; i >= 0; i--)
    //     {
    //         bytes = new byte[first.Length + second[i].Length];

    //         //copy the data from the first array into the new array
    //         Buffer.BlockCopy(first, 0, bytes, 0, first.Length);

    //         //copy the data from the second array into the new array (offset by the total length of the first array)
    //         Buffer.BlockCopy(second[i], 0, bytes, first.Length, second[i].Length);

    //         first = Combine(first, second[i]);
    //     }

    //     //return the final byte array
    //     return bytes;
    // }
      /*
void __fastcall T3SamplerStateBlock::SetStateMask(T3SamplerStateBlock *this, unsigned int state)
{
this->mData |= dword_14102C98C[2 * state];
}

T3SamplerStateBlock *__fastcall T3SamplerStateBlock::Merge(T3SamplerStateBlock *this, T3SamplerStateBlock *result, T3SamplerStateBlock *rhs, T3SamplerStateBlock *mask)
{
T3SamplerStateBlock *v4; // rax@1

v4 = result;
result->mData = mask->mData & rhs->mData | this->mData & ~mask->mData;
return v4;
}

__int64 __fastcall T3SamplerStateBlock::InternalGetSamplerState(T3SamplerStateBlock *this, unsigned int state)
{
return (this->mData & HIDWORD((&T3SamplerStateBlock::smEntries)[state])) >> LODWORD((&T3SamplerStateBlock::smEntries)[state]);
}

void __fastcall T3SamplerStateBlock::InternalSetSamplerState(T3SamplerStateBlock *this, unsigned int state, unsigned int value)
{
T3SamplerStateBlock *v3; // r9@1
struct T3SamplerStateBlock::SamplerStateEntry near **v4; // rcx@1

v3 = this;
v4 = &(&T3SamplerStateBlock::smEntries)[state];
v3->mData &= ~*(v4 + 1);
v3->mData |= value << *v4;
}

__int64 __fastcall T3SamplerStateBlock::DecrementMipBias(T3SamplerStateBlock *this, unsigned int steps)
{
float v2; // xmm0_4@1
unsigned int v3; // eax@1
__int64 result; // rax@3
unsigned int v5; // er8@3

v2 = FLOAT_8_0;
v3 = steps + ((dword_14102C9B4 & this->mData) >> dword_14102C9B0);
if ( v3 < 8.0 )
v2 = v3;
result = ffloor(v2);
v5 = this->mData & ~dword_14102C9B4;
this->mData = v5;
this->mData = v5 | (result << dword_14102C9B0);
return result;
}

void T3SamplerStateBlock::Initialize(void)
{
LODWORD(T3SamplerStateBlock::smEntries) = 0;
dword_14102C98C[0] = 15;
dword_14102C990 = 4;
dword_14102C994 = 240;
dword_14102C998 = 8;
dword_14102C99C = 256;
dword_14102C9A0 = 9;
T3SamplerStateBlock::kDefault.mData = (((T3SamplerStateBlock::kDefault.mData & 0xFFFFFFF0 | 1) & 0xFFFFFF0F | 0x10) & 0xFFFFFEFF | 0x100) & 0xFFC001FF;
dword_14102C9A4 = 7680;
dword_14102C9A8 = 13;
dword_14102C9AC = 0x2000;
dword_14102C9B0 = 14;
dword_14102C9B4 = 4177920;
}

public static SKColorType GetSKColorType(DXGIFormat format) => format switch
    {
        DXGIFormat.R32G32B32A32_TYPELESS => SKColorType.RgbaF32,
        DXGIFormat.R32G32B32A32_FLOAT => SKColorType.RgbaF32,
        DXGIFormat.R32G32B32A32_UINT => SKColorType.RgbaF32,
        DXGIFormat.R32G32B32A32_SINT => SKColorType.RgbaF32,
        //DXGIFormat.R32G32B32_TYPELESS => SKColorType.RgbaF32,
        //DXGIFormat.R32G32B32_FLOAT => SKColorType.RgbaF32,
        //DXGIFormat.R32G32B32_UINT => SKColorType.RgbaF32,
        //DXGIFormat.R32G32B32_SINT => SKColorType.RgbaF32,
        DXGIFormat.R16G16B16A16_TYPELESS => SKColorType.Rgba16161616,
        DXGIFormat.R16G16B16A16_FLOAT => SKColorType.Rgba16161616,
        DXGIFormat.R16G16B16A16_UNORM => SKColorType.Rgba16161616,
        DXGIFormat.R16G16B16A16_UINT => SKColorType.Rgba16161616,
        DXGIFormat.R16G16B16A16_SNORM => SKColorType.Rgba16161616,
        DXGIFormat.R16G16B16A16_SINT => SKColorType.Rgba16161616,
        DXGIFormat.R10G10B10A2_TYPELESS => SKColorType.Rgba1010102,
        DXGIFormat.R10G10B10A2_UNORM => SKColorType.Rgba1010102,
        DXGIFormat.R10G10B10A2_UINT => SKColorType.Rgba1010102,
        DXGIFormat.R8G8B8A8_TYPELESS => SKColorType.Rgba8888,
        DXGIFormat.R8G8B8A8_UNORM => SKColorType.Rgba8888,
        DXGIFormat.R8G8B8A8_UNORM_SRGB => SKColorType.Rgba8888,
        DXGIFormat.R8G8B8A8_UINT => SKColorType.Rgba8888,
        DXGIFormat.R8G8B8A8_SNORM => SKColorType.Rgba8888,
        DXGIFormat.R8G8B8A8_SINT => SKColorType.Rgba8888,
        DXGIFormat.R16G16_TYPELESS => SKColorType.Rg1616,
        DXGIFormat.R16G16_FLOAT => SKColorType.Rg1616,
        DXGIFormat.R16G16_UNORM => SKColorType.Rg1616,
        DXGIFormat.R16G16_UINT => SKColorType.Rg1616,
        DXGIFormat.R16G16_SNORM => SKColorType.Rg1616,
        DXGIFormat.R16G16_SINT => SKColorType.Rg1616,
        DXGIFormat.R8G8_TYPELESS => SKColorType.Rg88,
        DXGIFormat.R8G8_UNORM => SKColorType.Rg88,
        DXGIFormat.R8G8_UINT => SKColorType.Rg88,
        DXGIFormat.R8G8_SNORM => SKColorType.Rg88,
        DXGIFormat.R8G8_SINT => SKColorType.Rg88,
        DXGIFormat.R8_TYPELESS => SKColorType.Gray8,
        DXGIFormat.R8_UNORM => SKColorType.Gray8,
        DXGIFormat.R8_UINT => SKColorType.Gray8,
        DXGIFormat.R8_SNORM => SKColorType.Gray8,
        DXGIFormat.R8_SINT => SKColorType.Gray8,
        DXGIFormat.A8_UNORM => SKColorType.Alpha8,
        DXGIFormat.B5G6R5_UNORM => SKColorType.Rgba8888,
        DXGIFormat.B8G8R8A8_UNORM => SKColorType.Bgra8888,
        DXGIFormat.B8G8R8A8_TYPELESS => SKColorType.Bgra8888,
        DXGIFormat.B8G8R8A8_UNORM_SRGB => SKColorType.Bgra8888,
        DXGIFormat.B4G4R4A4_UNORM => SKColorType.Argb4444,
        DXGIFormat.A4B4G4R4_UNORM => SKColorType.Argb4444,
        _ => SKColorType.Unknown // Default or unknown format
    };

    public static byte[] LoadTexture(string path) => File.ReadAllBytes(path);

    public static byte[] GetBytesAfterBytePattern(string searchString, byte[] fileBytes)
    {
        byte[] searchBytes = Encoding.ASCII.GetBytes(searchString);

        int position = SearchBytePattern(searchBytes, fileBytes);

        if (position != -1)
        {
            byte[] resultBytes = new byte[fileBytes.Length - position];
            Array.Copy(fileBytes, position, resultBytes, 0, resultBytes.Length);
            return resultBytes;
        }

        return [];
    }

    public static int SearchBytePattern(byte[] pattern, byte[] bytes)
    {
        int patternLen = pattern.Length;
        int totalLen = bytes.Length;
        byte firstMatchByte = pattern[0];

        for (int i = 0; i < totalLen; i++)
        {
            if (firstMatchByte == bytes[i] && totalLen - i >= patternLen)
            {
                byte[] match = new byte[patternLen];
                Array.Copy(bytes, i, match, 0, patternLen);
                if (AreArraysEqual(match, pattern))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public static bool AreArraysEqual(byte[] a1, byte[] a2)
    {
        if (a1.Length != a2.Length)
            return false;

        for (int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != a2[i])
                return false;
        }

        return true;
    }
  */
//}
