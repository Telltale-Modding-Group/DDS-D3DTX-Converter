using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.TelltaleMeta;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleD3DTX;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using D3DTX_Converter.DirectX;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;

namespace D3DTX_Converter.Main
{
    /// <summary>
    /// This is the master class object for a D3DTX file. Reads a file and automatically parses the data into the correct version.
    /// </summary>
    public class D3DTX_Master
    {
        public string filePath;

        //meta header versions (objects at the top of the file)
        public MSV6? msv6;
        public MSV5? msv5;
        public MTRE? mtre;

        //d3dtx versions
        public D3DTX_V_OLD d3dtxOLD;
        public D3DTX_V4? d3dtx4;
        public D3DTX_V5? d3dtx5;
        public D3DTX_V6? d3dtx6;
        public D3DTX_V7? d3dtx7;
        public D3DTX_V8? d3dtx8;
        public D3DTX_V9? d3dtx9;

        /// <summary>
        /// Reads in a D3DTX file from the disk.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read_D3DTX_File(string filePath)
        {
            this.filePath = filePath;

            //read meta version of the file
            string metaVersion = Read_D3DTX_File_MetaVersionOnly(filePath);

            //read the d3dtx version of the file
            int d3dtxVersion = Read_D3DTX_File_D3DTXVersionOnly(filePath);

            using BinaryReader reader = new(File.OpenRead(filePath));
            //read meta header
            switch (metaVersion)
            {
                case "6VSM":
                    msv6 = new(reader);
                    break;
                case "5VSM":
                    msv5 = new(reader);
                    break;
                case "ERTM":
                    mtre = new(reader);
                    break;
                default:
                    Console.WriteLine("ERROR! '{0}' meta stream version is not supported!", metaVersion);
                    return;
            }

            if (d3dtxVersion == 5 || d3dtxVersion == 6 || d3dtxVersion == 7 || d3dtxVersion == 8)
            {
                Console.WriteLine(
                    "Warning! '{0}' version is not fully complete/tested! There may be some issues with converting.",
                    d3dtxVersion);
            }

            //read d3dtx header
            switch (d3dtxVersion)
            {
                case 4:
                    d3dtx4 = new(reader, true);
                    break;
                case 5:
                    d3dtx5 = new(reader, true);
                    break;
                case 6:
                    d3dtx6 = new(reader, true);
                    break;
                case 7:
                    d3dtx7 = new(reader, true);
                    break;
                case 8:
                    d3dtx8 = new(reader, true);
                    break;
                case 9:
                    d3dtx9 = new(reader, true);
                    break;
                default:
                    Console.WriteLine("ERROR! '{0}' d3dtx version is not supported!", d3dtxVersion);
                    break;
            }
        }

        public object Get_Meta_Object()
        {
            if (msv6 != null) return msv6;
            else if (msv5 != null) return msv5;
            else if (mtre != null) return mtre;
            else return null;
        }

        public object Get_D3DTX_Object()
        {
            if (d3dtx4 != null) return d3dtx4;
            else if (d3dtx5 != null) return d3dtx5;
            else if (d3dtx6 != null) return d3dtx6;
            else if (d3dtx7 != null) return d3dtx7;
            else if (d3dtx8 != null) return d3dtx8;
            else if (d3dtx9 != null) return d3dtx9;
            else return null;
        }

        /// <summary>
        /// Writes a final .d3dtx file to disk
        /// </summary>
        /// <param name="destinationPath"></param>
        public void Write_Final_D3DTX(string destinationPath)
        {
            using BinaryWriter writer = new(File.OpenWrite(destinationPath));

            if (msv6 != null) msv6.WriteBinaryData(writer);
            else if (msv5 != null) msv5.WriteBinaryData(writer);
            else if (mtre != null) mtre.WriteBinaryData(writer);

            if (d3dtx4 != null) d3dtx4.WriteBinaryData(writer);
            else if (d3dtx5 != null) d3dtx5.WriteBinaryData(writer);
            else if (d3dtx6 != null) d3dtx6.WriteBinaryData(writer);
            else if (d3dtx7 != null) d3dtx7.WriteBinaryData(writer);
            else if (d3dtx8 != null) d3dtx8.WriteBinaryData(writer);
            else if (d3dtx9 != null) d3dtx9.WriteBinaryData(writer);
        }

        /// <summary>
        /// Reads a json file and serializes it into the appropriate d3dtx version that was serialized in the json file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read_D3DTX_JSON(string filePath)
        {
            //read the data from the json file
            string jsonText = File.ReadAllText(filePath);

            //parse the data into a json array
            JArray jarray = JArray.Parse(jsonText);

            //meta object
            JObject metaObject = jarray[0] as JObject;

            //parsed meta stream version from the json document
            string metaStreamVersion = "";

            //loop through each property to get the value of the variable 'mMetaStreamVersion' to determine what version of the meta header to parse.
            foreach (JProperty property in metaObject.Properties())
            {
                if (property.Name.Equals("mMetaStreamVersion")) metaStreamVersion = (string)property.Value;
                break;
            }

            //deserialize the appropriate json object
            if (metaStreamVersion.Equals("6VSM")) msv6 = metaObject.ToObject<MSV6>();
            else if (metaStreamVersion.Equals("5VSM")) msv5 = metaObject.ToObject<MSV5>();
            else if (metaStreamVersion.Equals("ERTM")) mtre = metaObject.ToObject<MTRE>();

            //d3dtx object
            JObject d3dtxObject = jarray[1] as JObject;

            //d3dtx version value
            int d3dtxVersion = 0;

            //loop through each property to get the value of the variable 'mVersion' to determine what version of the d3dtx header to parse.
            foreach (JProperty property in d3dtxObject.Properties())
            {
                if (property.Name.Equals("mVersion")) d3dtxVersion = (int)property.Value;
                break;
            }

            //deserialize the appropriate json object
            switch (d3dtxVersion)
            {
                case 4:
                    d3dtx4 = d3dtxObject.ToObject<D3DTX_V4>();
                    break;
                case 5:
                    d3dtx5 = d3dtxObject.ToObject<D3DTX_V5>();
                    break;
                case 6:
                    d3dtx6 = d3dtxObject.ToObject<D3DTX_V6>();
                    break;
                case 7:
                    d3dtx7 = d3dtxObject.ToObject<D3DTX_V7>();
                    break;
                case 8:
                    d3dtx8 = d3dtxObject.ToObject<D3DTX_V8>();
                    break;
                case 9:
                    d3dtx9 = d3dtxObject.ToObject<D3DTX_V9>();
                    break;
                default:
                    d3dtxOLD = d3dtxObject.ToObject<D3DTX_V_OLD>();
                    break;
            }
        }

        public void Write_D3DTX_JSON(string fileName, string destinationDirectory)
        {
            string newPath = destinationDirectory + Path.DirectorySeparatorChar + fileName + Main_Shared.jsonExtension;

            //open a stream writer to create the text file and write to it
            using StreamWriter file = File.CreateText(newPath);
            //get our json seralizer
            JsonSerializer serializer = new();

            List<object> jsonObjects = new();
            jsonObjects.Add(Get_Meta_Object());
            jsonObjects.Add(Get_D3DTX_Object());

            //seralize the data and write it to the configruation file
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, jsonObjects);
        }

        public void SetMetaChunkSizes(uint mDefaultSectionChunkSize, uint mAsyncSectionChunkSize)
        {
            if (msv5 != null)
            {
                msv5.mDefaultSectionChunkSize = mDefaultSectionChunkSize;
                msv5.mAsyncSectionChunkSize = mAsyncSectionChunkSize;
            }
            else if (msv6 != null)
            {
                msv6.mDefaultSectionChunkSize = mDefaultSectionChunkSize;
                msv6.mAsyncSectionChunkSize = mAsyncSectionChunkSize;
            }
        }

        public void Modify_D3DTX(DDS_Master dds, DDS_DirectXTexNet_ImageSection[] sections)
        {
            if (d3dtx4 != null)
            {
                d3dtx4.ModifyD3DTX(dds);

                SetMetaChunkSizes(d3dtx4.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx4.mPixelData));
            }
            else if (d3dtx5 != null)
            {
                d3dtx5.ModifyD3DTX(dds);

                SetMetaChunkSizes(d3dtx5.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx5.mPixelData));
            }
            else if (d3dtx6 != null)
            {
                d3dtx6.ModifyD3DTX(dds);

                SetMetaChunkSizes(d3dtx6.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx6.mPixelData));
            }
            else if (d3dtx7 != null)
            {
                d3dtx7.ModifyD3DTX(dds);

                //SetMetaChunkSizes(d3dtx7.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx7.mPixelData));
            }
            else if (d3dtx8 != null)
            {
                d3dtx8.ModifyD3DTX(dds);

                //SetMetaChunkSizes(d3dtx8.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx8.mPixelData));
            }
            else if (d3dtx9 != null)
            {
                d3dtx9.ModifyD3DTX(dds, sections); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES

                SetMetaChunkSizes(d3dtx9.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx9.mPixelData));
            }
        }

        /// <summary>
        /// Reads a d3dtx file on the disk and returns the meta version that is being used.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static string Read_D3DTX_File_MetaVersionOnly(string sourceFile)
        {
            string metaStreamVersion = "";

            using BinaryReader reader = new(File.OpenRead(sourceFile));

            for (int i = 0; i < 4; i++) metaStreamVersion += reader.ReadChar();

            return metaStreamVersion;
        }

        /// <summary>
        /// Reads a d3dtx file on the disk and returns the D3DTX version.
        /// <para>NOTE: This only works with d3dtx meta version 5VSM and 6VSM</para>
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static int Read_D3DTX_File_D3DTXVersionOnly(string sourceFile)
        {
            string metaVersion = Read_D3DTX_File_MetaVersionOnly(sourceFile);
            MSV6 meta6VSM = null;
            MSV5 meta5VSM = null;
            MTRE metaERTM = null;

            using BinaryReader reader = new(File.OpenRead(sourceFile));

            if (metaVersion.Equals("6VSM")) meta6VSM = new(reader, false);
            else if (metaVersion.Equals("5VSM")) meta5VSM = new(reader, false);
            else if (metaVersion.Equals("ERTM")) metaERTM = new(reader, false);

            //read the first int (which is an mVersion d3dtx value)
            return
                metaERTM == null
                    ? reader.ReadInt32()
                    : -1; //return -1 because d3dtx versions older than 4 don't have an mVersion variable (not that I know of atleast)
        }

        public bool IsCubeTexture()
        {
            if (d3dtx7 != null)
                return d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_Cube ||
                       d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if (d3dtx8 != null)
                return d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_Cube ||
                       d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if (d3dtx9 != null)
                return d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_Cube ||
                       d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else
                return false;
        }


        public T3TextureType GetTextureType()
        {
            if (d3dtx4 != null)
                return d3dtx4.mType;
            else if (d3dtx5 != null)
                return d3dtx5.mType;
            else if (d3dtx6 != null)
                return d3dtx6.mType;
            else if (d3dtx7 != null)
                return d3dtx7.mType;
            else if (d3dtx8 != null)
                return d3dtx8.mType;
            else if (d3dtx9 != null)
                return d3dtx9.mType;
            else
                return T3TextureType.eTxUnknown;
        }

        public string GetTextureName()
        {
            if (d3dtx4 != null)
                return d3dtx4.mName;
            else if (d3dtx5 != null)
                return d3dtx5.mName;
            else if (d3dtx6 != null)
                return d3dtx6.mName;
            else if (d3dtx7 != null)
                return d3dtx7.mName;
            else if (d3dtx8 != null)
                return d3dtx8.mName;
            else if (d3dtx9 != null)
                return d3dtx9.mName;
            else
                return "Not Available";
        }

        public uint GetHeight()
        {
            if (d3dtx4 != null)
                return d3dtx4.mHeight;
            else if (d3dtx5 != null)
                return d3dtx5.mHeight;
            else if (d3dtx6 != null)
                return d3dtx6.mHeight;
            else if (d3dtx7 != null)
                return d3dtx7.mHeight;
            else if (d3dtx8 != null)
                return d3dtx8.mHeight;
            else if (d3dtx9 != null)
                return d3dtx9.mHeight;
            else
                return 0;
        }

        public uint GetWidth()
        {
            if (d3dtx4 != null)
                return d3dtx4.mWidth;
            else if (d3dtx5 != null)
                return d3dtx5.mWidth;
            else if (d3dtx6 != null)
                return d3dtx6.mWidth;
            else if (d3dtx7 != null)
                return d3dtx7.mWidth;
            else if (d3dtx8 != null)
                return d3dtx8.mWidth;
            else if (d3dtx9 != null)
                return d3dtx9.mWidth;
            else
                return 0;
        }

        public string GetStringCompressionType()
        {
            if (d3dtx4 != null)
                return Enum.GetName(d3dtx4.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx5 != null)
                return Enum.GetName(d3dtx5.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx6 != null)
                return Enum.GetName(d3dtx6.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx7 != null)
                return Enum.GetName(d3dtx7.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx8 != null)
                return Enum.GetName(d3dtx8.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx9 != null)
                return Enum.GetName(d3dtx9.mSurfaceFormat).Remove(0, 9);
            else
                return "Not Available";
        }

        public T3SurfaceFormat GetCompressionType()
        {
            if (d3dtx4 != null)
                return d3dtx4.mSurfaceFormat;
            else if (d3dtx5 != null)
                return d3dtx5.mSurfaceFormat;
            else if (d3dtx6 != null)
                return d3dtx6.mSurfaceFormat;
            else if (d3dtx7 != null)
                return d3dtx7.mSurfaceFormat;
            else if (d3dtx8 != null)
                return d3dtx8.mSurfaceFormat;
            else if (d3dtx9 != null)
                return d3dtx9.mSurfaceFormat;
            else
                return T3SurfaceFormat.eSurface_Unknown;
        }

        /// <summary>
        /// Gets the channel count of .d3dtx surface format. Needs verification for some formats. eTxColor could also play part for the unknown formats.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static uint GetChannelCount(T3SurfaceFormat format, eTxAlpha alpha = eTxAlpha.eTxAlphaUnknown)
        {
            switch (format)
            {
                default:
                    return 0;

                case T3SurfaceFormat.eSurface_R8:
                case T3SurfaceFormat.eSurface_A8:
                case T3SurfaceFormat.eSurface_L8:
                case T3SurfaceFormat.eSurface_R16:
                case T3SurfaceFormat.eSurface_R32:
                case T3SurfaceFormat.eSurface_R16UI:
                case T3SurfaceFormat.eSurface_R16F:
                case T3SurfaceFormat.eSurface_R32F:
                case T3SurfaceFormat.eSurface_BC4:
                case T3SurfaceFormat.eSurface_DXT5A:
                case T3SurfaceFormat.eSurface_ETC2_R: //Needs verification
                case T3SurfaceFormat.eSurface_Depth16:
                case T3SurfaceFormat.eSurface_Depth24:
                case T3SurfaceFormat.eSurface_DepthPCF16: //Percentage-closer filtering?
                case T3SurfaceFormat.eSurface_DepthPCF24: //Percentage-closer filtering
                case T3SurfaceFormat.eSurface_DepthStencil32:
                case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
                case T3SurfaceFormat.eSurface_Depth32F:
                case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
                    return 1;

                case T3SurfaceFormat.eSurface_RG16:
                case T3SurfaceFormat.eSurface_RG32:
                case T3SurfaceFormat.eSurface_RG16UI:
                case T3SurfaceFormat.eSurface_RG16S:
                case T3SurfaceFormat.eSurface_RG16F:
                case T3SurfaceFormat.eSurface_RG32F:
                case T3SurfaceFormat.eSurface_AL8:
                case T3SurfaceFormat.eSurface_DXN:
                case T3SurfaceFormat.eSurface_BC5:
                case T3SurfaceFormat.eSurface_ETC2_RG: //Needs verification
                    return 2;

                case T3SurfaceFormat.eSurface_RGB565:
                case T3SurfaceFormat.eSurface_RGB111110F:
                case T3SurfaceFormat.eSurface_RGB9E5F:
                case T3SurfaceFormat.eSurface_BC6:
                case T3SurfaceFormat.eSurface_ETC1_RGB:
                case T3SurfaceFormat.eSurface_ETC2_RGB:
                case T3SurfaceFormat.eSurface_PVRTC2: //Needs verification
                case T3SurfaceFormat.eSurface_PVRTC4: //Needs verification
                case T3SurfaceFormat.eSurface_ATC_RGB:
                case T3SurfaceFormat.eSurface_FrontBuffer: //Needs verification
                case T3SurfaceFormat.eSurface_CTX1://Needs verification
                    return 3;

                case T3SurfaceFormat.eSurface_ARGB8:
                case T3SurfaceFormat.eSurface_ARGB16:
                case T3SurfaceFormat.eSurface_ARGB1555:
                case T3SurfaceFormat.eSurface_ARGB4:
                case T3SurfaceFormat.eSurface_ARGB2101010:
                case T3SurfaceFormat.eSurface_RGBA16:
                case T3SurfaceFormat.eSurface_RGBA8:
                case T3SurfaceFormat.eSurface_RGBA32:
                case T3SurfaceFormat.eSurface_RGBA8S:
                case T3SurfaceFormat.eSurface_RGBA16S:
                case T3SurfaceFormat.eSurface_RGBA16F:
                case T3SurfaceFormat.eSurface_RGBA32F:
                case T3SurfaceFormat.eSurface_RGBA1010102F:
                case T3SurfaceFormat.eSurface_DXT3:
                case T3SurfaceFormat.eSurface_BC2:
                case T3SurfaceFormat.eSurface_DXT5:
                case T3SurfaceFormat.eSurface_BC3:
                case T3SurfaceFormat.eSurface_ETC2_RGB1A:
                case T3SurfaceFormat.eSurface_ETC2_RGBA:
                case T3SurfaceFormat.eSurface_PVRTC2a: //Needs verification
                case T3SurfaceFormat.eSurface_PVRTC4a: //Needs verification
                case T3SurfaceFormat.eSurface_ATC_RGB1A:
                case T3SurfaceFormat.eSurface_ATC_RGBA:
                case T3SurfaceFormat.eSurface_ATSC_RGBA_4x4: //Needs verification
                    return 4;

                case T3SurfaceFormat.eSurface_DXT1: //Needs checking (alpha is optional)
                case T3SurfaceFormat.eSurface_BC1: //Needs checking (alpha is optional)
                case T3SurfaceFormat.eSurface_BC7: //Needs checking (alpha is optional)
                    if (alpha > 0)
                    {
                        return 4;
                    }
                    return 3;
                case T3SurfaceFormat.eSurface_Count: //Needs verification
                case T3SurfaceFormat.eSurface_Unknown:
                    return 0;
            }
        }

        public string GetHasAlpha()
        {
            if (d3dtx4 != null)
                return d3dtx4.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx5 != null)
                return d3dtx5.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx6 != null)
                return d3dtx6.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx7 != null)
                return d3dtx7.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx8 != null)
                return d3dtx8.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx9 != null)
                return d3dtx9.mAlphaMode > 0 ? "True" : "False";
            else
                return "Not Available";
        }

        /// <summary>
        /// Gets a string version of the channel count of .d3dtx surface format. 
        /// </summary>
        /// <returns></returns>
        public string GetChannelCount()
        {

            uint channelCount = 0;

            T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_Unknown;
            eTxAlpha alpha = eTxAlpha.eTxAlphaUnknown;

            if (d3dtx4 != null)
            {
                surfaceFormat = d3dtx4.mSurfaceFormat;
                alpha = d3dtx4.mAlphaMode;
            }
            else if (d3dtx5 != null)
            {
                surfaceFormat = d3dtx5.mSurfaceFormat;
                alpha = d3dtx5.mAlphaMode;
            }

            else if (d3dtx6 != null)
            {
                surfaceFormat = d3dtx6.mSurfaceFormat;
                alpha = d3dtx6.mAlphaMode;
            }

            else if (d3dtx7 != null)
            {
                surfaceFormat = d3dtx7.mSurfaceFormat;
                alpha = d3dtx7.mAlphaMode;
            }

            else if (d3dtx8 != null)
            {
                surfaceFormat = d3dtx8.mSurfaceFormat;
                alpha = d3dtx8.mAlphaMode;
            }

            else if (d3dtx9 != null)
            {
                surfaceFormat = d3dtx9.mSurfaceFormat;
                alpha = d3dtx9.mAlphaMode;
            }

            channelCount = D3DTX_Master.GetChannelCount(surfaceFormat, alpha);

            return channelCount.ToString();
        }

        public int GetRegionCount()
        {
            if (d3dtx4 != null)
                return d3dtx4.mStreamHeader.mRegionCount;
            else if (d3dtx5 != null)
                return d3dtx5.mStreamHeader.mRegionCount;
            else if (d3dtx6 != null)
                return d3dtx6.mStreamHeader.mRegionCount;
            else if (d3dtx7 != null)
                return d3dtx7.mStreamHeader.mRegionCount;
            else if (d3dtx8 != null)
                return d3dtx8.mStreamHeader.mRegionCount;
            else if (d3dtx9 != null)
                return d3dtx9.mStreamHeader.mRegionCount;
            else
                return -1;
        }

        public uint GetMipMapCount()
        {
            if (d3dtx4 != null)
                return d3dtx4.mNumMipLevels;
            else if (d3dtx5 != null)
                return d3dtx5.mNumMipLevels;
            else if (d3dtx6 != null)
                return d3dtx6.mNumMipLevels;
            else if (d3dtx7 != null)
                return d3dtx7.mNumMipLevels;
            else if (d3dtx8 != null)
                return d3dtx8.mNumMipLevels;
            else if (d3dtx9 != null)
                return d3dtx9.mNumMipLevels;
            else
                return 0;
        }

        public bool HasMipMaps()
        {
            return GetMipMapCount() > 1;
        }

        public List<byte[]> GetPixelData()
        {
            if (d3dtx4 != null)
                return d3dtx4.mPixelData;
            else if (d3dtx5 != null)
                return d3dtx5.mPixelData;
            else if (d3dtx6 != null)
                return d3dtx6.mPixelData;
            else if (d3dtx7 != null)
                return d3dtx7.mPixelData;
            else if (d3dtx8 != null)
                return d3dtx8.mPixelData;
            else if (d3dtx9 != null)
                return d3dtx9.mPixelData;
            else
                return null;
        }

        public static bool IsTextureCompressed(T3SurfaceFormat format)
        {

            return format switch
            {
                T3SurfaceFormat.eSurface_DXT1 => true,
                T3SurfaceFormat.eSurface_DXT3 => true,
                T3SurfaceFormat.eSurface_DXT5 => true,
                T3SurfaceFormat.eSurface_DXT5A => true,
                T3SurfaceFormat.eSurface_BC1 => true,
                T3SurfaceFormat.eSurface_BC2 => true,
                T3SurfaceFormat.eSurface_BC3 => true,
                T3SurfaceFormat.eSurface_BC4 => true,
                T3SurfaceFormat.eSurface_BC5 => true,
                T3SurfaceFormat.eSurface_BC6 => true,
                T3SurfaceFormat.eSurface_BC7 => true,
                T3SurfaceFormat.eSurface_DXN => true,
                T3SurfaceFormat.eSurface_CTX1 => true,
                _ => false,
            };
        }

        public static uint GetD3DTXBlockSize(T3SurfaceFormat format)
        {
            if (IsTextureCompressed(format))
            {
                return format switch
                {
                    T3SurfaceFormat.eSurface_DXT1 => 8,
                    T3SurfaceFormat.eSurface_DXT3 => 8,
                    T3SurfaceFormat.eSurface_BC4 => 8,
                    _ => 16,
                };
            }

            return 0;
        }

        public static uint GetBitsPerPixel(T3SurfaceFormat surfaceFormat)
        {
            switch (surfaceFormat)
            {
                case T3SurfaceFormat.eSurface_L8:
                case T3SurfaceFormat.eSurface_A8:
                case T3SurfaceFormat.eSurface_L16:
                    return 8;

                case T3SurfaceFormat.eSurface_RG8:
                case T3SurfaceFormat.eSurface_AL8:
                case T3SurfaceFormat.eSurface_R16:
                case T3SurfaceFormat.eSurface_R16F:
                case T3SurfaceFormat.eSurface_R16UI:
                case T3SurfaceFormat.eSurface_RGB565:
                case T3SurfaceFormat.eSurface_ARGB1555:
                case T3SurfaceFormat.eSurface_ARGB4:
                    return 16;

                case T3SurfaceFormat.eSurface_ARGB8:
                case T3SurfaceFormat.eSurface_RGBA8:
                case T3SurfaceFormat.eSurface_ARGB2101010:
                case T3SurfaceFormat.eSurface_RG16:
                case T3SurfaceFormat.eSurface_RG16S:
                case T3SurfaceFormat.eSurface_RGBA16S:
                case T3SurfaceFormat.eSurface_RG16UI:
                case T3SurfaceFormat.eSurface_RGBA8S:
                case T3SurfaceFormat.eSurface_RGBA1010102F:
                case T3SurfaceFormat.eSurface_RGB111110F:
                case T3SurfaceFormat.eSurface_RGB9E5F:
                case T3SurfaceFormat.eSurface_RG16F:
                case T3SurfaceFormat.eSurface_R32:
                case T3SurfaceFormat.eSurface_R32F:
                    return 32;

                case T3SurfaceFormat.eSurface_ARGB16:
                case T3SurfaceFormat.eSurface_RGBA16:
                case T3SurfaceFormat.eSurface_RG32:
                case T3SurfaceFormat.eSurface_RG32F:
                    return 64; // 16 bits per channel * 4 channels (RGBA)

                case T3SurfaceFormat.eSurface_RGBA32:
                case T3SurfaceFormat.eSurface_RGBA32F:
                    return 128; // 32 bits per channel * 4 channels (RGBA)

                default:
                    return 0; // Unknown format or unsupported format
            }
        }

        public List<byte[]> GetPixelDataByFaceIndex(int faceIndex)
        {
            List<byte[]> newPixelData = new();

            if (d3dtx7 != null)
            {
                for (int i = 0; i < d3dtx7.mRegionHeaders.Length; i++)
                {
                    if (d3dtx7.mRegionHeaders[i].mFaceIndex == faceIndex)
                    {
                        newPixelData.Add(d3dtx7.mPixelData[i]);
                    }
                }
            }
            else if (d3dtx8 != null)
            {
                for (int i = 0; i < d3dtx8.mRegionHeaders.Length; i++)
                {
                    if (d3dtx8.mRegionHeaders[i].mFaceIndex == faceIndex)
                    {
                        newPixelData.Add(d3dtx8.mPixelData[i]);
                    }
                }
            }
            else if (d3dtx9 != null)
            {
                for (int i = 0; i < d3dtx9.mRegionHeaders.Length; i++)
                {
                    if (d3dtx9.mRegionHeaders[i].mFaceIndex == faceIndex)
                    {
                        newPixelData.Add(d3dtx9.mPixelData[i]);
                    }
                }
            }

            return newPixelData;
        }
    }
}