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
using D3DTX_Converter.TelltaleTypes;
using System.Linq;
using DirectXTexNet;

namespace D3DTX_Converter.Main
{
    /// <summary>
    /// This is the master class object for a D3DTX file. Reads a file and automatically parses the data into the correct version.
    /// </summary>
    public class D3DTX_Master
    {
        public string? filePath { get; set; }

        public string? fileName;

        // Meta header versions (objects at the top of the file)
        public MSV6? msv6;
        public MSV5? msv5;
        public MTRE? mtre;

        // D3DTX versions
        // Legacy D3DTX versions
        public D3DTX_LV1? d3dtxL1;
        public D3DTX_LV2? d3dtxL2;
        public D3DTX_LV3? d3dtxL3;

        // Newer D3DTX versions. They are used from Poker Night 2 and later games.
        public D3DTX_V3? d3dtx3;
        public D3DTX_V4? d3dtx4;
        public D3DTX_V5? d3dtx5;
        public D3DTX_V6? d3dtx6;
        public D3DTX_V7? d3dtx7;
        public D3DTX_V8? d3dtx8;
        public D3DTX_V9? d3dtx9;

        // Generic DDS object if the D3DTX version is not found. This is used for legacy D3DTX versions only since they use DDS headers in the pixel data.
        public ScratchImage? ddsImage;

        public D3DTXConversionType d3dtxConversionType;

        public struct D3DTX_JSON
        {
            public D3DTXConversionType ConversionType;
        }

        /// <summary>
        /// Reads in a D3DTX file from the disk.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="setD3DTXVersion"></param>
        public void Read_D3DTX_File(string filePath, D3DTXConversionType setD3DTXVersion = D3DTXConversionType.DEFAULT)
        {
            this.filePath = filePath;
            fileName = Path.GetFileNameWithoutExtension(filePath);

            // Read meta version of the file
            string metaVersion = ReadD3DTXFileMetaVersionOnly(filePath);

            using BinaryReader reader = new(File.OpenRead(filePath));
            // Read meta header
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
                case "DDS ":
                    // Find DDS in the the file (this try-catch is bad practice);
                    try
                    {
                        byte[] ddsData = ByteFunctions.GetBytesAfterBytePattern(DDS.MAGIC_WORD, reader.ReadBytes((int)reader.BaseStream.Length));
                        ddsImage = DDS_DirectXTexNet.GetDDSImage(ddsData);
                    //    metadata = ddsImage.GetMetadata();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("No DDS Header found");
                    }

                    return;
                default:
                    Console.WriteLine("ERROR! '{0}' meta stream version is not supported!", metaVersion);
                    return;
            }

            // Attempt to read the d3dtx version of the file
            int d3dtxVersion = ReadD3DTXFileD3DTXVersionOnly(filePath);
            d3dtxConversionType = setD3DTXVersion;
            // D3DTX version 6 and 8 are not fully tested, because no texture samples were found in Telltale game files
            // Presumably they were used during development or existed in earlier versions of the games, but were replaced with newer versions
            if (d3dtxVersion == 8)
            {
                Console.WriteLine(
                    "Warning! '{0}' version is not fully complete/tested! There may be some issues with converting.",
                    d3dtxVersion);
            }

            switch (d3dtxVersion)
            {
                case 3:
                    d3dtx3 = new(reader, true);
                    break;
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
                case -1:
                    if (setD3DTXVersion == D3DTXConversionType.LV1)
                    {
                        d3dtxL1 = new D3DTX_LV1(reader, true);
                    }
                    else if (setD3DTXVersion == D3DTXConversionType.LV2)
                    {
                        d3dtxL2 = new D3DTX_LV2(reader, true);
                    }
                    else if (setD3DTXVersion == D3DTXConversionType.LV3)
                    {
                        d3dtxL3 = new D3DTX_LV3(reader, true);
                    }
                    else if (setD3DTXVersion == D3DTXConversionType.DEFAULT)
                    {
                        try
                        {
                            byte[] ddsData = ByteFunctions.GetBytesAfterBytePattern(DDS.MAGIC_WORD, reader.ReadBytes((int)reader.BaseStream.Length));
                            ddsImage = DDS_DirectXTexNet.GetDDSImage(ddsData);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("No DDS Header found");
                        }
                    }
                    else
                    {
                        Console.WriteLine("ERROR! '{0}' d3dtx version is not supported!", d3dtxVersion);
                    }
                    break;
                default:
                    Console.WriteLine("ERROR! '{0}' d3dtx version is not supported!", d3dtxVersion);
                    break;
            }

        }

        public object GetMetaObject()
        {
            if (msv6 != null) return msv6;
            else if (msv5 != null) return msv5;
            else if (mtre != null) return mtre;
            else return null;
        }

        public object GetD3DTXObject()
        {
            if (d3dtxL1 != null) return d3dtxL1;
            else if (d3dtxL2 != null) return d3dtxL2;
            else if (d3dtxL3 != null) return d3dtxL3;
            else if (d3dtx3 != null) return d3dtx3;
            else if (d3dtx4 != null) return d3dtx4;
            else if (d3dtx5 != null) return d3dtx5;
            else if (d3dtx6 != null) return d3dtx6;
            else if (d3dtx7 != null) return d3dtx7;
            else if (d3dtx8 != null) return d3dtx8;
            else if (d3dtx9 != null) return d3dtx9;
            else if (ddsImage != null) return ddsImage;
            else return null;
        }

        public DDS_HEADER GetDDSHeaderFromLegacyD3DTX()
        {
            // if (metadata != null) return genericDDS.header;
            // else 
            if (d3dtxL1 != null) return d3dtxL1.mDDSHeader;
            else if (d3dtxL2 != null) return d3dtxL2.mDDSHeader;
            else if (d3dtxL3 != null) return d3dtxL3.mDDSHeader;
            else return new DDS_HEADER();
        }

        /// <summary>
        /// Writes a final .d3dtx file to disk
        /// </summary>
        /// <param name="destinationPath"></param>
        public void WriteFinalD3DTX(string destinationPath)
        {
            using BinaryWriter writer = new(File.OpenWrite(destinationPath));

            if (msv6 != null) msv6.WriteBinaryData(writer);
            else if (msv5 != null) msv5.WriteBinaryData(writer);
            else if (mtre != null) mtre.WriteBinaryData(writer);

            if (d3dtxL1 != null) d3dtxL1.WriteBinaryData(writer);
            else if (d3dtxL2 != null) d3dtxL2.WriteBinaryData(writer);
            else if (d3dtxL3 != null) d3dtxL3.WriteBinaryData(writer);
            else if (d3dtx3 != null) d3dtx3.WriteBinaryData(writer);
            else if (d3dtx4 != null) d3dtx4.WriteBinaryData(writer);
            else if (d3dtx5 != null) d3dtx5.WriteBinaryData(writer);
            else if (d3dtx6 != null) d3dtx6.WriteBinaryData(writer);
            else if (d3dtx7 != null) d3dtx7.WriteBinaryData(writer);
            else if (d3dtx8 != null) d3dtx8.WriteBinaryData(writer);
            else if (d3dtx9 != null) d3dtx9.WriteBinaryData(writer);
        }

        public string GetD3DTXDebugInfo()
        {
            string allInfo = "";

            if (msv6 != null) allInfo += msv6.GetMSV6Info();
            else if (msv5 != null) allInfo += msv5.GetMSV5Info();
            else if (mtre != null) allInfo += mtre.GetMTREInfo();

            if (d3dtxL1 != null) allInfo += d3dtxL1.GetD3DTXInfo();
            else if (d3dtxL2 != null) allInfo += d3dtxL2.GetD3DTXInfo();
            else if (d3dtxL3 != null) allInfo += d3dtxL3.GetD3DTXInfo();
            else if (d3dtx3 != null) allInfo += d3dtx3.GetD3DTXInfo();
            else if (d3dtx4 != null) allInfo += d3dtx4.GetD3DTXInfo();
            else if (d3dtx5 != null) allInfo += d3dtx5.GetD3DTXInfo();
            else if (d3dtx6 != null) allInfo += d3dtx6.GetD3DTXInfo();
            else if (d3dtx7 != null) allInfo += d3dtx7.GetD3DTXInfo();
            //else if (d3dtx8 != null) allInfo += d3dtx8.GetD3DTXInfo();
            else if (d3dtx9 != null) allInfo += d3dtx9.GetD3DTXInfo();
            else if (ddsImage.GetMetadata() != null) allInfo += DDS_DirectXTexNet.GetDDSDebugInfo(ddsImage.GetMetadata());
            else allInfo += "Error! Data not found!";

            return allInfo;
        }

        /// <summary>
        /// Reads a json file and serializes it into the appropriate d3dtx version that was serialized in the json file.
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadD3DTXJSON(string filePath)
        {
            string jsonText = File.ReadAllText(filePath);

            // parse the data into a json array
            JArray jarray = JArray.Parse(jsonText);

            // read the first object in the array to determine if the json file is a legacy json file or not
            JObject firstObject = jarray[0] as JObject;

            bool isLegacyJSON = true;

            foreach (JProperty property in firstObject.Properties())
            {
                if (property.Name.Equals("ConversionType")) isLegacyJSON = false;
                break;
            }

            int metaObjectIndex = isLegacyJSON ? 0 : 1;
            int d3dtxObjectIndex = isLegacyJSON ? 1 : 2;

            d3dtxConversionType = isLegacyJSON ? D3DTXConversionType.DEFAULT : firstObject.ToObject<D3DTX_JSON>().ConversionType;

            // I am creating the metaObject again instead of using the firstObject variable and i am aware of the performance hit.
            JObject metaObject = jarray[metaObjectIndex] as JObject;
            ConvertJSONObjectToMeta(metaObject);

            // d3dtx object
            JObject d3dtxObject = jarray[d3dtxObjectIndex] as JObject;

            //deserialize the appropriate json object
            if (d3dtxConversionType == D3DTXConversionType.DEFAULT)
            {
                ConvertJSONObjectToD3dtx(d3dtxObject);
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV1)
            {
                d3dtxL1 = d3dtxObject.ToObject<D3DTX_LV1>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV2)
            {
                d3dtxL2 = d3dtxObject.ToObject<D3DTX_LV2>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV3)
            {
                d3dtxL3 = d3dtxObject.ToObject<D3DTX_LV3>();
            }
        }

        public void ConvertJSONObjectToD3dtx(JObject d3dtxObject)
        {
            // d3dtx version value
            int d3dtxVersion = 0;

            // loop through each property to get the value of the variable 'mVersion' to determine what version of the d3dtx header to parse.
            foreach (JProperty property in d3dtxObject.Properties())
            {
                if (property.Name.Equals("mVersion")) d3dtxVersion = (int)property.Value;
                break;
            }

            switch (d3dtxVersion)
            {
                case 3:
                    d3dtx3 = d3dtxObject.ToObject<D3DTX_V3>();
                    break;
                case 4:
                    d3dtx4 = d3dtxObject.ToObject<D3DTX_V4>();
                    break;
                case 5:
                    d3dtx5 = d3dtxObject.ToObject<D3DTX_V5>();
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
                    throw new DataMisalignedException("Invalid d3dtx version. Please convert this to the newer .json format.");
            }
        }

        public void ConvertJSONObjectToMeta(JObject metaObject)
        {
            // parsed meta stream version from the json document
            string metaStreamVersion = "";

            // loop through each property to get the value of the variable 'mMetaStreamVersion' to determine what version of the meta header to parse.
            foreach (JProperty property in metaObject.Properties())
            {
                if (property.Name.Equals("mMetaStreamVersion")) metaStreamVersion = (string)property.Value;
                break;
            }

            // deserialize the appropriate json object
            if (metaStreamVersion.Equals("6VSM")) msv6 = metaObject.ToObject<MSV6>();
            else if (metaStreamVersion.Equals("5VSM")) msv5 = metaObject.ToObject<MSV5>();
            else if (metaStreamVersion.Equals("ERTM")) mtre = metaObject.ToObject<MTRE>();
        }

        public void WriteD3DTXJSON(string fileName, string destinationDirectory)
        {
            if (GetD3DTXObject() == null)
            {
                return;
            }

            string newPath = destinationDirectory + Path.DirectorySeparatorChar + fileName + Main_Shared.jsonExtension;

            //open a stream writer to create the text file and write to it
            using StreamWriter file = File.CreateText(newPath);
            //get our json serializer
            JsonSerializer serializer = new();

            D3DTX_JSON conversionTypeObject = new()
            {
                ConversionType = d3dtxConversionType
            };

            List<object> jsonObjects = [conversionTypeObject, GetMetaObject(), GetD3DTXObject()];
            //serialize the data and write it to the configuration file
            serializer.Formatting = Formatting.Indented;
            serializer.Serialize(file, jsonObjects);
        }

        public void SetMetaChunkSizes(uint mDefaultSectionChunkSize, uint mAsyncSectionChunkSize)
        {
            if (mtre != null)
            {
                //
            }

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

        public void Modify_D3DTX(ScratchImage image)
        {
            var meta = DDS_DirectXTexNet.GetDDSMetaData(image);
            if (d3dtxL1 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDS_FLAGS.FORCE_DX9_LEGACY);
                d3dtxL1.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL2 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDS_FLAGS.FORCE_DX9_LEGACY);
                d3dtxL2.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL3 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDS_FLAGS.FORCE_DX9_LEGACY);
                d3dtxL3.ModifyD3DTX(meta, dataArray);
            }

            // If they are not legacy version, stable sort the image sections by size. (Smallest to Largest)
            var sections = DDS_DirectXTexNet.GetDDSImageSections(image);
            IEnumerable<DDS_DirectXTexNet_ImageSection> newImageSections = sections;
            newImageSections = sections.OrderBy(section => section.Pixels.Length);

            if (d3dtx3 != null)
            {
                d3dtx3.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx3.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx3.mPixelData));
            }
            else if (d3dtx4 != null)
            {
                d3dtx4.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx4.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx4.mPixelData));
            }
            else if (d3dtx5 != null)
            {
                d3dtx5.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx5.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx5.mPixelData));
            }
            else if (d3dtx6 != null)
            {
                d3dtx6.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx6.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx6.mPixelData));
            }
            else if (d3dtx7 != null)
            {
                d3dtx7.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx7.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx7.mPixelData));
            }
            else if (d3dtx8 != null)
            {
                d3dtx8.ModifyD3DTX(meta, newImageSections.ToArray());

                SetMetaChunkSizes(d3dtx8.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx8.mPixelData));
            }
            else if (d3dtx9 != null)
            {
                d3dtx9.ModifyD3DTX(meta, newImageSections.ToArray()); //ISSUE HERE WITH DXT5 AND MIP MAPS WITH UPSCALED TEXTURES

                SetMetaChunkSizes(d3dtx9.GetHeaderByteSize(), ByteFunctions.Get2DByteArrayTotalSize(d3dtx9.mPixelData));
            }
        }

        /// <summary>
        /// Reads a d3dtx file on the disk and returns the meta version that is being used.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static string ReadD3DTXFileMetaVersionOnly(string sourceFile)
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
        public static int ReadD3DTXFileD3DTXVersionOnly(string sourceFile)
        {
            string metaVersion = ReadD3DTXFileMetaVersionOnly(sourceFile);

            using BinaryReader reader = new(File.OpenRead(sourceFile));

            MSV6 meta6VSM;
            MSV5 meta5VSM;
            MTRE metaERTM = null;
            if (metaVersion.Equals("6VSM")) meta6VSM = new(reader, false);
            else if (metaVersion.Equals("5VSM")) meta5VSM = new(reader, false);
            else if (metaVersion.Equals("ERTM")) metaERTM = new(reader, false);

            //read the first int (which is an mVersion d3dtx value)
            if (metaERTM != null)
            {
                return reader.ReadInt32() == 3 ? 3 : -1; //return -1 because d3dtx versions older than 3 don't have an mVersion variable (not that I know of atleast)
            }
            else
                return reader.ReadInt32();
        }

        public bool isLegacyD3DTX()
        {
            if (d3dtx3 != null)
                return false;
            else if (d3dtx4 != null)
                return false;
            else if (d3dtx5 != null)
                return false;
            else if (d3dtx6 != null)
                return false;
            else if (d3dtx7 != null)
                return false;
            else if (d3dtx8 != null)
                return false;
            else if (d3dtx9 != null)
                return false;

            return true;
        }


        public bool IsVolumeTexture()
        {
            if (d3dtx5 != null)
                return d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_3D;
            else if (d3dtx7 != null)
                return d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_3D;
            else if (d3dtx8 != null)
                return d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_3D;
            else if (d3dtx9 != null)
                return d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_3D || d3dtx9.mDepth > 1;
            else
                return false;
        }

        public bool IsCubeTexture()
        {
            if (d3dtx5 != null)
                return d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_Cube ||
                       d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
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

        public bool IsArrayTexture()
        {
            if (d3dtx5 != null)
                return d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_2DArray ||
                       d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            if (d3dtx7 != null)
                return d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_2DArray ||
                       d3dtx7.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if (d3dtx8 != null)
                return d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_2DArray ||
                       d3dtx8.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if (d3dtx9 != null)
                return d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_2DArray ||
                       d3dtx9.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else
                return false;
        }

        public T3TextureType GetTextureType()
        {
            if (d3dtx3 != null)
                return d3dtx3.mType;
            else if (d3dtx4 != null)
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
            if (d3dtxL1 != null)
                return d3dtxL1.mName;
            else if (d3dtxL2 != null)
                return d3dtxL2.mName;
            else if (d3dtxL3 != null)
                return d3dtxL3.mName;
            else if (d3dtx3 != null)
                return d3dtx3.mName;
            else if (d3dtx4 != null)
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
                return fileName;
        }

        public int GetHeight()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mHeight;
            else if (d3dtxL2 != null)
                return d3dtxL2.mHeight;
            else if (d3dtxL3 != null)
                return d3dtxL3.mHeight;
            else if (d3dtx3 != null)
                return d3dtx3.mHeight;
            else if (d3dtx4 != null)
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
            else if (ddsImage.GetMetadata() != null)
                return ddsImage.GetMetadata().Height;
            else
                return 0;
        }

        public int GetWidth()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mWidth;
            else if (d3dtxL2 != null)
                return d3dtxL2.mWidth;
            else if (d3dtxL3 != null)
                return d3dtxL3.mWidth;
            else if (d3dtx3 != null)
                return d3dtx3.mWidth;
            else if (d3dtx4 != null)
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
            else if (ddsImage.GetMetadata() != null)
                return ddsImage.GetMetadata().Width;
            else
                return 0;
        }

        public int GetDepth()
        {
            if (d3dtx8 != null)
                return d3dtx8.mDepth;
            else if (d3dtx9 != null)
                return d3dtx9.mDepth;
            else
                return 0;
        }

        public string GetStringCompressionType()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mD3DFormat.ToString();
            else if (d3dtxL2 != null)
                return d3dtxL2.mD3DFormat.ToString();
            else if (d3dtxL3 != null)
                return d3dtxL3.mD3DFormat.ToString();
            else if (d3dtx3 != null)
                return Enum.GetName(d3dtx3.mSurfaceFormat).Remove(0, 9);
            else if (d3dtx4 != null)
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
            else if (ddsImage != null)
                return ddsImage.GetMetadata().Format.ToString();
            else
                return null;
        }

        public T3SurfaceFormat GetCompressionType()
        {
            if (d3dtx3 != null)
                return d3dtx3.mSurfaceFormat;
            else if (d3dtx4 != null)
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

        public T3SurfaceGamma GetSurfaceGamma()
        {
            if (d3dtx3 != null)
                return T3SurfaceGamma.eSurfaceGamma_Linear;
            else if (d3dtx4 != null)
                return T3SurfaceGamma.eSurfaceGamma_Linear;
            else if (d3dtx5 != null)
                return d3dtx5.mSurfaceGamma;
            else if (d3dtx6 != null)
                return T3SurfaceGamma.eSurfaceGamma_Linear;
            else if (d3dtx7 != null)
                return d3dtx7.mSurfaceGamma;
            else if (d3dtx8 != null)
                return d3dtx8.mSurfaceGamma;
            else if (d3dtx9 != null)
                return d3dtx9.mSurfaceGamma;
            else
                return T3SurfaceGamma.eSurfaceGamma_Linear;
        }

        public int GetArraySize()
        {
            if (d3dtx8 != null)
                return d3dtx8.mArraySize;
            else if (d3dtx9 != null)
                return d3dtx9.mArraySize;
            else
                return 1;
        }

        public RegionStreamHeader[]? GetRegionStreamHeaders()
        {
            if (d3dtx3 != null)
                return d3dtx3.mRegionHeaders;
            else if (d3dtx4 != null)
                return d3dtx4.mRegionHeaders;
            else if (d3dtx5 != null)
                return d3dtx5.mRegionHeaders;
            else if (d3dtx6 != null)
                return d3dtx6.mRegionHeaders;
            else if (d3dtx7 != null)
                return d3dtx7.mRegionHeaders;
            else if (d3dtx8 != null)
                return d3dtx8.mRegionHeaders;
            else if (d3dtx9 != null)
                return d3dtx9.mRegionHeaders;
            else
                return null;
        }

        public string GetHasAlpha()
        {
            if (d3dtx3 != null)
                return d3dtx3.mAlphaMode > 0 ? "True" : "False";
            else if (d3dtx4 != null)
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
            else if (ddsImage != null)
            {
                return ddsImage.GetMetadata().GetAlphaMode().ToString();
            }
            else
                return "Unknown";
        }

        /// <summary>
        /// Gets a string version of the channel count of .d3dtx surface format. 
        /// </summary>
        /// <returns></returns>
        public string GetChannelCount()
        {
            DXGI_FORMAT format;

            if (ddsImage != null)
            {
                format = ddsImage.GetMetadata().Format;
            }
            else if (GetD3DTXObject != null)
            {
                format = DDS_HELPER.GetDXGIFromTelltaleSurfaceFormat(GetCompressionType());
            }
            else return null;

            return DDS_DirectXTexNet.GetChannelCount(format).ToString();
        }

        public int GetRegionCount()
        {
            if (d3dtx3 != null)
                return d3dtx3.mStreamHeader.mRegionCount;
            else if (d3dtx4 != null)
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

        public int GetMipMapCount()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mNumMipLevels;
            else if (d3dtxL2 != null)
                return d3dtxL2.mNumMipLevels;
            if (d3dtx3 != null)
                return d3dtx3.mNumMipLevels;
            else if (d3dtx4 != null)
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
            else if (ddsImage.GetMetadata() != null)
                return ddsImage.GetMetadata().MipLevels;
            else
                return 0;
        }

        public bool HasMipMaps()
        {
            return GetMipMapCount() > 1;
        }

        public List<byte[]>? GetLegacyPixelData()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mPixelData;
            else if (d3dtxL2 != null)
                return d3dtxL2.mPixelData;
            else
                return null;
        }

        public List<byte[]>? GetPixelData()
        {
            if (d3dtx3 != null)
                return d3dtx3.mPixelData;
            else if (d3dtx4 != null)
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

        public bool IsTextureCompressed()
        {
            return IsTextureCompressed(GetCompressionType());
        }

        public static bool IsTextureCompressed(T3SurfaceFormat format)
        {
            return format switch
            {
                T3SurfaceFormat.eSurface_BC1 => true,
                T3SurfaceFormat.eSurface_BC2 => true,
                T3SurfaceFormat.eSurface_BC3 => true,
                T3SurfaceFormat.eSurface_BC4 => true,
                T3SurfaceFormat.eSurface_BC5 => true,
                T3SurfaceFormat.eSurface_BC6 => true,
                T3SurfaceFormat.eSurface_BC7 => true,
                T3SurfaceFormat.eSurface_CTX1 => true,
                _ => false,
            };
        }
    }
}