using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.TelltaleMeta;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.TelltaleD3DTX;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft;
using DirectXTexNet;
using D3DTX_Converter.DirectX;

namespace D3DTX_Converter.Main
{
    /// <summary>
    /// This is the master class object for a D3DTX file. Reads a file and automatically parses the data into the correct version.
    /// </summary>
    public class D3DTX_Master
    {
        //meta header versions (objects at the top of the file)
        public MSV6 msv6;
        public MSV5 msv5;
        public MTRE mtre;

        //d3dtx versions
        public D3DTX_V_OLD d3dtxOLD;
        public D3DTX_V4 d3dtx4;
        public D3DTX_V5 d3dtx5;
        public D3DTX_V6 d3dtx6;
        public D3DTX_V7 d3dtx7;
        public D3DTX_V8 d3dtx8;
        public D3DTX_V9 d3dtx9;

        /// <summary>
        /// Reads in a D3DTX file from the disk.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read_D3DTX_File(string filePath)
        {
            //read meta version of the file
            string metaVersion = Read_D3DTX_File_MetaVersionOnly(filePath);

            //read the d3dtx version of the file
            int d3dtxVersion = Read_D3DTX_File_D3DTXVersionOnly(filePath);

            using (BinaryReader reader = new(File.OpenRead(filePath)))
            {
                //read meta header
                switch (metaVersion)
                {
                    case "6VSM": msv6 = new(reader); break;
                    case "5VSM": msv5 = new(reader); break;
                    case "ERTM": mtre = new(reader); break;
                    default:
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                        Console.WriteLine("ERROR! '{0}' meta stream version is not supported!", metaVersion);
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                        return;
                }

                if (d3dtxVersion == 5 || d3dtxVersion == 6 || d3dtxVersion == 7 || d3dtxVersion == 8)
                {
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
                    Console.WriteLine("Warning! '{0}' version is not fully complete/tested! There may be some issues with converting.", d3dtxVersion);
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                }

                //read d3dtx header
                switch (d3dtxVersion)
                {
                    case 4: d3dtx4 = new(reader, true); break;
                    case 5: d3dtx5 = new(reader, true); break;
                    case 6: d3dtx6 = new(reader, true); break;
                    case 7: d3dtx7 = new(reader, true); break;
                    case 8: d3dtx8 = new(reader, true); break;
                    case 9: d3dtx9 = new(reader, true); break;
                    default:
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                        Console.WriteLine("ERROR! '{0}' d3dtx version is not supported!", d3dtxVersion);
                        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                        break;
                }
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
            using(BinaryWriter writer = new(File.OpenWrite(destinationPath)))
            {
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
                if (property.Name.Equals("mMetaStreamVersion")) metaStreamVersion = (string)property.Value; break;
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
                if (property.Name.Equals("mVersion")) d3dtxVersion = (int)property.Value; break;
            }

            //deserialize the appropriate json object
            switch (d3dtxVersion)
            {
                case 4: d3dtx4 = d3dtxObject.ToObject<D3DTX_V4>(); break;
                case 5: d3dtx5 = d3dtxObject.ToObject<D3DTX_V5>(); break;
                case 6: d3dtx6 = d3dtxObject.ToObject<D3DTX_V6>(); break;
                case 7: d3dtx7 = d3dtxObject.ToObject<D3DTX_V7>(); break;
                case 8: d3dtx8 = d3dtxObject.ToObject<D3DTX_V8>(); break;
                case 9: d3dtx9 = d3dtxObject.ToObject<D3DTX_V9>(); break;
                default: d3dtxOLD = d3dtxObject.ToObject<D3DTX_V_OLD>(); break;
            }
        }

        public void Write_D3DTX_JSON(string destinationPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(destinationPath);
            string fileDirectory = Path.GetDirectoryName(destinationPath);
            string newPath = fileDirectory + "/" + fileName + Main_Shared.jsonExtension;

            //open a stream writer to create the text file and write to it
            using (StreamWriter file = File.CreateText(newPath))
            {
                //get our json seralizer
                JsonSerializer serializer = new();

                List<object> jsonObjects = new();
                jsonObjects.Add(Get_Meta_Object());
                jsonObjects.Add(Get_D3DTX_Object());

                //seralize the data and write it to the configruation file
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, jsonObjects);
            }
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

                //SetMetaChunkSizes(d3dtx5.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx5.mPixelData));
            }
            else if (d3dtx6 != null)
            {
                d3dtx6.ModifyD3DTX(dds);

                //SetMetaChunkSizes(d3dtx6.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx6.mPixelData));
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

            using (BinaryReader reader = new(File.OpenRead(sourceFile)))
            {
                for(int i = 0; i < 4; i++) metaStreamVersion += reader.ReadChar();
            }

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

            using (BinaryReader reader = new(File.OpenRead(sourceFile)))
            {
                if (metaVersion.Equals("6VSM")) meta6VSM = new(reader, false);
                else if (metaVersion.Equals("5VSM")) meta5VSM = new(reader, false);
                else if (metaVersion.Equals("ERTM")) metaERTM = new(reader, false);

                //read the first int (which is an mVersion d3dtx value)
                return metaERTM == null ? reader.ReadInt32() : -1; //return -1 because d3dtx versions older than 4 don't have an mVersion variable (not that I know of atleast)
            }
        }

        public bool IsCubeTexture()
        {
            if (d3dtx7 != null)
                return d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_Cube || d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if(d3dtx8 != null)
                return d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_Cube || d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if(d3dtx9 != null)
                return d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_Cube || d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else
                return false;
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

        public List<byte[]> GetPixelDataByFaceIndex(int faceIndex)
        {
            List<byte[]> newPixelData = new();

            if (d3dtx7 != null)
            {
                for(int i = 0; i < d3dtx7.mRegionHeaders.Length; i++)
                {
                    if(d3dtx7.mRegionHeaders[i].mFaceIndex == faceIndex)
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
