using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DirectXTexNet;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;
using D3DTX_TextureConverter.Telltale;

/*
 * DXT1 - DXGI_FORMAT_BC1_UNORM / D3DFMT_DXT1
 * DXT2 - D3DFMT_DXT2
 * DXT3 - DXGI_FORMAT_BC2_UNORM / D3DFMT_DXT3
 * DXT4 - D3DFMT_DXT4
 * DXT5 - DXGI_FORMAT_BC3_UNORM / D3DFMT_DXT5
 * ATI1
 * ATI2 - DXGI_FORMAT_BC5_UNORM
 * BC5U - 
 * BC5S - DXGI_FORMAT_BC5_SNORM
 * BC4U - DXGI_FORMAT_BC4_UNORM
 * BC4S - DXGI_FORMAT_BC4_SNORM
 * RGBG - DXGI_FORMAT_R8G8_B8G8_UNORM / D3DFMT_R8G8_B8G8
 * GRGB - DXGI_FORMAT_G8R8_G8B8_UNORM / D3DFMT_G8R8_G8B8
 * UYVY - D3DFMT_UYVY
 * YUY2 - D3DFMT_YUY2
 * DX10 - Any DXGI format
 * 36 - DXGI_FORMAT_R16G16B16A16_UNORM / D3DFMT_A16B16G16R16
 * 110 - DXGI_FORMAT_R16G16B16A16_SNORM / D3DFMT_Q16W16V16U16
 * 111 - DXGI_FORMAT_R16_FLOAT / D3DFMT_R16F
 * 112 - DXGI_FORMAT_R16G16_FLOAT / D3DFMT_G16R16F
 * 113 - DXGI_FORMAT_R16G16B16A16_FLOAT / D3DFMT_A16B16G16R16F
 * 114 - DXGI_FORMAT_R32_FLOAT / D3DFMT_R32F
 * 115 - DXGI_FORMAT_R32G32_FLOAT / D3DFMT_G32R32F
 * 116 - DXGI_FORMAT_R32G32B32A32_FLOAT / D3DFMT_A32B32G32R32F
 * 117 - D3DFMT_CxV8U8
*/

namespace D3DTX_TextureConverter.Main
{
    /// <summary>
    /// Main class for generating a dds byte header
    /// </summary>
    public class DDS_File
    {
        //dds image file extension
        public static string ddsExtension = ".dds";

        //main ddsPrefix (with space) [4 bytes]
        public readonly string ddsPrefix = "DDS ";

        public string sourceFileName; //file name + extension
        public string sourceFile; //file path
        public byte[] sourceFileData; //file data
        public List<byte[]> textureData;
        public byte[,] mipMapResolutions;

        public DDS_HEADER header;

        private void GetData(byte[] fileData, bool headerOnly)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("Total Source Texture Byte Size = {0}", fileData.Length);

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            uint bytePointerPosition = 4; //skip past the 'DDS '
            byte[] headerBytes = ByteFunctions.AllocateBytes(124, fileData, bytePointerPosition);

            //this will automatically read all of the byte data in the header
            header = DDS_Functions.GetHeaderFromBytes(headerBytes);

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("DDS Height = {0}", header.dwHeight);
            Console.WriteLine("DDS Width = {0}", header.dwWidth);
            Console.WriteLine("DDS Mip Map Count = {0}", header.dwMipMapCount);
            Console.WriteLine("DDS Compression = {0}", header.ddspf.dwFourCC);

            if (headerOnly)
                return;

            //--------------------------EXTRACT DDS TEXTURE DATA--------------------------
            //calculate dds header length (we add 4 because we skipped the 4 bytes which contain the ddsPrefix, it isn't necessary to parse this data)
            uint ddsHeaderLength = 4 + header.dwSize;

            //calculate the length of just the dds texture data
            uint ddsTextureDataLength = (uint)sourceFileData.Length - ddsHeaderLength;

            //allocate a byte array of dds texture length
            byte[] ddsTextureData = new byte[ddsTextureDataLength];

            //copy the data from the source byte array past the header (so we are only getting texture data)
            Array.Copy(sourceFileData, ddsHeaderLength, ddsTextureData, 0, ddsTextureData.Length);

            //if there are no mip maps
            if(header.dwMipMapCount <= 0)
            {
                textureData = new List<byte[]>();
                textureData.Add(ddsTextureData);
            }
            else //if there are mip maps
            {
                //get mip resolutions
                //calculated mip resolutions [Pixel Value, Width or Height (0 or 1)]
                uint[,] mipImageResolutions = DDS_Functions.DDS_CalculateMipResolutions(header.dwMipMapCount, header.dwWidth, header.dwHeight);

                //get byte sizes
                uint[] byteSizes = DDS_Functions.DDS_GetImageByteSizes(mipImageResolutions, DDS_Functions.DDS_CompressionBool(header));

                textureData = new List<byte[]>();
                int offset = 0;

                for (int i = 0; i < byteSizes.Length; i++)
                {
                    byte[] temp = new byte[byteSizes[i]];

                    Array.Copy(ddsTextureData, offset, temp, 0, temp.Length);

                    offset += temp.Length;

                    textureData.Add(temp);
                }
            }
        }

        /// <summary>
        /// Reads a DDS file from disk. (Can also read just the header only)
        /// </summary>
        /// <param name="ddsFilePath"></param>
        /// <param name="headerOnly"></param>
        public DDS_File(string ddsFilePath, bool headerOnly)
        {
            //read the source texture file into a byte array
            sourceFileData = File.ReadAllBytes(ddsFilePath);
            sourceFileName = Path.GetFileName(ddsFilePath);
            sourceFile = ddsFilePath;

            //read the DDS file
            GetData(sourceFileData, headerOnly);
        }

        /// <summary>
        /// Reads a DDS file from a byte array. (Can also just read the header only)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="headerOnly"></param>
        public DDS_File(byte[] data, bool headerOnly)
        {
            //get the byte data
            sourceFileData = data;

            //read the DDS file
            GetData(sourceFileData, headerOnly);
        }

        public DDS_File Match_DDS_With_D3DTX(string ddsPath, D3DTX_File d3dtx)
        {
            ScratchImage scratchImage = TexHelper.Instance.LoadFromDDSFile(ddsPath, DDS_FLAGS.NONE);
            TexMetadata texMetadata = TexHelper.Instance.GetMetadataFromDDSFile(ddsPath, DDS_FLAGS.NONE);

            //6VSM
            if(d3dtx.D3DTX_6VSM != null)
            {
                //change the compression if needed
                //if (d3dtx.D3DTX_6VSM.mSurfaceFormat != texMetadata.Format)
                //{
                //  scratchImage.Convert((int)d3dtx.D3DTX_6VSM.mWidth, (int)d3dtx.D3DTX_6VSM.mHeight, TEX_FILTER_FLAGS.CUBIC);
                //}

                //rescale the image to match if needed
                if (d3dtx.D3DTX_6VSM.mHeight != texMetadata.Height || d3dtx.D3DTX_6VSM.mWidth != texMetadata.Width)
                {
                    scratchImage.Resize((int)d3dtx.D3DTX_6VSM.mWidth, (int)d3dtx.D3DTX_6VSM.mHeight, TEX_FILTER_FLAGS.CUBIC);
                }

                //if there are mip maps
                if (d3dtx.D3DTX_6VSM.mNumMipLevels > 1)
                {
                    //generate mip maps
                    scratchImage.GenerateMipMaps(0, TEX_FILTER_FLAGS.CUBIC, 0, false);
                }

                //resave the newly modified DDS
                scratchImage.SaveToDDSFile(DDS_FLAGS.NONE, ddsPath);
            }


            return null;
        }

        public T3SurfaceFormat Get_TellaleFormat_FromFourCC(uint fourCC)
        {
            switch (fourCC)
            {
                default:
                    return Telltale.T3SurfaceFormat.eSurface_DXT1;//ByteFunctions.Convert_String_To_UInt32("DXT1");
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

        public uint Get_FourCC_FromTellale(T3SurfaceFormat format)
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

        public DDS_File(D3DTX_File d3dtx)
        {
            header = DDS_Functions.GetPresetHeader();

            if (d3dtx.D3DTX_6VSM != null) //6VSM
            {
                header.dwWidth = (uint)d3dtx.D3DTX_6VSM.mWidth;
                header.dwHeight = (uint)d3dtx.D3DTX_6VSM.mHeight;
                header.dwMipMapCount = (uint)d3dtx.D3DTX_6VSM.mNumMipLevels;
                header.dwDepth = (uint)d3dtx.D3DTX_6VSM.mDepth;
                header.ddspf.dwFourCC = Get_FourCC_FromTellale(d3dtx.D3DTX_6VSM.mSurfaceFormat);
            }
            else if (d3dtx.D3DTX_5VSM != null) //5VSM
            {
                header.dwWidth = (uint)d3dtx.D3DTX_5VSM.mWidth;
                header.dwHeight = (uint)d3dtx.D3DTX_5VSM.mHeight;
                header.dwMipMapCount = (uint)d3dtx.D3DTX_5VSM.mNumMipLevels;
                header.ddspf.dwFourCC = Get_FourCC_FromTellale(d3dtx.D3DTX_5VSM.mSurfaceFormat);

                switch (d3dtx.D3DTX_5VSM.mSurfaceFormat)
                {
                    case Telltale.T3SurfaceFormat.eSurface_A8:
                        header.ddspf.dwABitMask = 255;
                        header.dwCaps = 4198408; //DDSCAPS_COMPLEX | DDSCAPS_TEXTURE | DDSCAPS_MIPMAP
                        break;
                }
            }
        }

        public void Write_D3DTX_AsDDS(D3DTX_File d3dtx, string destinationPath)
        {
            byte[] finalData = new byte[0];

            //turn our header data into bytes to be written into a file
            byte[] dds_header = ByteFunctions.Combine(ByteFunctions.GetBytes("DDS "), DDS_Functions.GetHeaderBytes(header));

            //copy the dds header to the file
            finalData = ByteFunctions.Combine(finalData, dds_header);

            //6VSM
            if (d3dtx.D3DTX_6VSM != null)
            {
                //copy the images
                for(int i = d3dtx.D3DTX_6VSM.T3Texture_Data.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.D3DTX_6VSM.T3Texture_Data[i]);
                }
            }
            //5VSM
            else if (d3dtx.D3DTX_5VSM != null)
            {
                //copy the images
                for (int i = d3dtx.D3DTX_5VSM.T3Texture_Data.Count - 1; i >= 0; i--)
                {
                    finalData = ByteFunctions.Combine(finalData, d3dtx.D3DTX_5VSM.T3Texture_Data[i]);
                }
            }
            //ERTM
            else if (d3dtx.D3DTX_ERTM != null)
            {
                //copy the images
                //for (int i = d3dtx.D3DTX_ERTM.T3Texture_Data.Count - 1; i >= 0; i--)
                //{
                //    finalData = ByteFunctions.Combine(finalData, d3dtx.D3DTX_ERTM.T3Texture_Data[i]);
                //}
            }

            //write the file
            File.WriteAllBytes(destinationPath, finalData);
        }

        /// <summary>
        /// Manually builds a byte array of a dds header
        /// </summary>
        /// <returns></returns>
        public byte[] Build_DDSHeader_ByteArray()
        {
            //allocate our header byte array
            byte[] ddsHeader = new byte[128];

            //return the result
            return ddsHeader;
        }
    }
}
