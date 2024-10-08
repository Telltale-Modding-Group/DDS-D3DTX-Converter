//using System;
//using System.Collections.Generic;
//using System.IO;
//using TelltaleTextureTool.DirectX;
//using System.Linq;
//using static Ktx.Ktx2;
//using TelltaleTextureTool.TelltaleEnums;
//using Ktx;
//using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
//using TelltaleTextureTool.Utilities;
//using Texture = Ktx.Ktx2.Texture;

//namespace TelltaleTextureTool.Main
//{
//    /// <summary>
//    /// Main class for managing D3DTX files and converting them to KTX2.
//    /// </summary>
//    public unsafe class KTX2_Master
//    {
//        //TextureCreateInfo ktx2TextureInfo;
//        Texture* texture;

//        public byte[] header = [];
//        public byte[] pixelData = [];

//        /// <summary>
//        /// Create a DDS file from a D3DTX.
//        /// </summary>
//        /// <param name="d3dtx">The D3DTX data that will be used.</param>
//        public KTX2_Master(D3DTX_Master d3dtx)
//        {
//            if (d3dtx.HasDDSHeader())
//            {
//                return;
//            }

//            InitializeKTX2Header(d3dtx);
//            InitializeKTX2PixelData(d3dtx);
//        }

//        private void InitializeKTX2Header(D3DTX_Master d3dtx)
//        {
//            D3DTXMetadata d3dtxMetadata = d3dtx.GetMetadata();

//            Console.WriteLine("D3dtx width: " + d3dtxMetadata.Width);
//            Console.WriteLine("D3dtx height: " + d3dtxMetadata.Height);
//            Console.WriteLine("D3dtx mip map count: " + d3dtxMetadata.MipLevels);
//            Console.WriteLine("D3dtx d3d format: " + d3dtxMetadata.D3DFormat);

//            T3SurfaceFormat surfaceFormat = d3dtxMetadata.Format;
//            T3SurfaceGamma surfaceGamma = d3dtxMetadata.SurfaceGamma;
//            T3PlatformType platformType = d3dtxMetadata.Platform;
//            T3TextureAlphaMode alphaMode = d3dtxMetadata.AlphaMode;

//            // ktx2TextureInfo = new()
//            // {
//            //     BaseWidth = d3dtxMetadata.Width,
//            //     BaseHeight = d3dtxMetadata.Height,
//            //     BaseDepth = d3dtxMetadata.Depth,
//            //     NumLevels = d3dtxMetadata.MipLevels,
//            //     NumLayers = d3dtxMetadata.ArraySize,
//            //     NumFaces = (uint)(d3dtxMetadata.IsCubemap() ? 6 : 1),
//            //     NumDimensions = (uint)(d3dtxMetadata.IsVolumemap() ? 3 : 2),
//            //     IsArray = (byte)(d3dtxMetadata.IsArrayTexture() ? 1 : 0),
//            //     GenerateMipmaps = (byte)(d3dtxMetadata.MipLevels > 1 ? 1 : 0),
//            //     VkFormat = KTX2_HELPER.GetVkFormatFromTelltaleSurfaceFormat(surfaceFormat, surfaceGamma, platformType, alphaMode)
//            // };

//            // if (D3DTX_Master.IsFormatIncompatibleWithDDS(surfaceFormat))
//            // {
//            //     ktx2TextureInfo.NumLevels = 1;
//            //     ktx2TextureInfo.GenerateMipmaps = 0;
//            // }
//            // ktx2TextureInfo.GenerateMipmaps = 0;
//            // Ktx2.ErrorCode err = Ktx2.Create(ktx2TextureInfo, Ktx2.TextureCreateStorage.NoStorage, out texture);

//            // List<byte[]> textureData = d3dtx.GetPixelData();

//            // byte[] pixelData = textureData.SelectMany(x => x).ToArray();

//            // fixed (byte* pData = pixelData)
//            // {
//            //     texture->PData = pData;
//            // }

//            // if (err != Ktx2.ErrorCode.Success)
//            // {
//            //     throw new Exception("Failed to create KTX2 texture.");
//            // }
//        }

//        private void InitializeKTX2PixelData(D3DTX_Master d3dtx)
//        {
//            // NOTES: Telltale mip levels are reversed in Poker Night 2 and above. The first level are the smallest mip levels and the last level is the largest mip level.
//            // The faces are NOT reverse.
//            // This is likely corelating with the way that KTX and KTX2 files are written.
//            // Some normal maps specifically with type 4 (eTxNormalMap) channels are all reversed (ABGR instead of RGBA) (Only applies for newer games)
//            // Some surface formats are dependant on platforms. For example, iOS textures have their R and B channels swapped.
//            // Some surface formats are not supported by DDS. In this case, the texture will be written as a raw texture.

//            if (d3dtx.IsLegacyD3DTX())
//            {
//                throw new Exception("KTX2 does not support legacy D3DTX files.");

//            }

//            D3DTXMetadata metadata = d3dtx.GetMetadata();

//            T3SurfaceFormat surfaceFormat = metadata.Format;

//            //             List<byte[]> textureData = d3dtx.GetPixelData();

//            //             byte[] pixelData = textureData.SelectMany(x => x).ToArray();

//            // texture.p

//            // textureData.Reverse();

//            // for (int i = 0; i < ktx2TextureInfo.NumLevels; i++)
//            // {
//            //     for (int j = 0; j < ktx2TextureInfo.NumLayers; j++)
//            //     {
//            //         for (int k = 0; k < ktx2TextureInfo.NumFaces; k++)
//            //         {
//            //             Ktx2.ErrorCode err = Ktx2.SetImageFromMemory(texture, (uint)i, (uint)j, (uint)k, textureData[i + j + k][0], (nuint)textureData[i + j + k].Length);

//            //             if (err != Ktx2.ErrorCode.Success)
//            //             {
//            //                 throw new Exception("Failed to set image from memory.");
//            //             }
//            //         }
//            //     }
//            // }
//        }

//        /// <summary>
//        /// Writes a D3DTX into a KTX2 file on the disk.
//        /// </summary>
//        /// <param name="d3dtx"></param>
//        /// <param name="destinationDirectory"></param>
//        public void WriteD3DTXAsKTX2(D3DTX_Master d3dtx, string destinationDirectory)
//        {
//            var watch = System.Diagnostics.Stopwatch.StartNew();
//            string d3dtxFilePath = d3dtx.FilePath;
//            string fileName = Path.GetFileNameWithoutExtension(d3dtxFilePath);

//            string newKTX2Path = destinationDirectory + Path.DirectorySeparatorChar + fileName +
//                                                       Main_Shared.ktx2Extension;

//            Ktx2.WriteToNamedFile(texture, newKTX2Path);
//            watch.Stop();
//            var elapsedMs = watch.ElapsedMilliseconds;
//            Console.WriteLine("Time to get data: {0}", elapsedMs);
//        }

//        /// <summary>
//        /// Get the correct DDS data from a Telltale texture.
//        /// </summary>
//        /// <param name="d3dtx"></param>
//        /// <returns></returns>
//        public byte[] GetData(D3DTX_Master d3dtx)
//        {
//            Console.WriteLine("Getting data for: " + d3dtx.FilePath);

//            // If the D3DTX exists, return the pixel data.
//            if (d3dtx.d3dtxObject == null)
//            {
//                throw new InvalidDataException("There is no pixel data to be written.");
//            }

//            // If the D3DTX has a DDS header, return the whole pixel data.
//            if (d3dtx.HasDDSHeader())
//            {
//                return d3dtx.GetPixelData()[d3dtx.GetPixelData().Count - 1];
//            }

//            // Return the created DDS file.
//            return ByteFunctions.Combine(header, pixelData);
//        }

//        ~KTX2_Master()
//        {
//            Destroy(texture);
//        }
//    }
//}

