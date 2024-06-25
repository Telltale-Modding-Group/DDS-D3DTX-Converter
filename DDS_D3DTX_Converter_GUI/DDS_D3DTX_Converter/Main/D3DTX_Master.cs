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
using Hexa.NET.DirectXTex;
using D3DTX_Converter.DirectX.Enums;
using Pvrtc;

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
        public MBIN? mbin;

        public MetaVersion meta;

        // D3DTX versions
        // Legacy D3DTX versions
        public D3DTX_LV1? d3dtxL1;
        public D3DTX_LV2? d3dtxL2;
        public D3DTX_LV3? d3dtxL3;
        public D3DTX_LV4? d3dtxL4;
        public D3DTX_LV5? d3dtxL5;
        public D3DTX_LV6? d3dtxL6;
        public D3DTX_LV7? d3dtxL7;
        public D3DTX_LV8? d3dtxL8;
        public D3DTX_LV9? d3dtxL9;
        public D3DTX_LV10? d3dtxL10;
        public D3DTX_LV11? d3dtxL11;

        // Newer D3DTX versions. They are used from Poker Night 2 and later games.
        public D3DTX_V3? d3dtx3;
        public D3DTX_V4? d3dtx4;
        public D3DTX_V5? d3dtx5;
        public D3DTX_V6? d3dtx6;
        public D3DTX_V7? d3dtx7;
        public D3DTX_V8? d3dtx8;
        public D3DTX_V9? d3dtx9;

        // Generic DDS object if the D3DTX version is not found. This is used for legacy D3DTX versions only since they use DDS headers in the pixel data.
        public ScratchImage ddsImage;

        public byte[]? ddsData;

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
                    meta = MetaVersion.MSV6;
                    break;
                case "5VSM":
                case "4VSM":
                    msv5 = new(reader);
                    meta = MetaVersion.MSV5;
                    break;
                case "ERTM":
                case "MOCM":
                    mtre = new(reader);
                    meta = MetaVersion.MTRE;
                    break;
                case "NIBM":
                case "SEBM":
                    mbin = new(reader);
                    meta = MetaVersion.MBIN;
                    break;
                case "DDS ":
                    // Find DDS in the the file (this try-catch is bad practice);
                    try
                    {
                        Span<byte> safeBytes = ByteFunctions.LoadTexture(filePath);
                        ddsData = safeBytes.ToArray();
                        ddsImage = DDS_DirectXTexNet.GetDDSImage(ddsData);
                        Console.WriteLine(ddsImage.IsNull);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid DDS!");
                    }

                    return;
                default:
                    Console.WriteLine("ERROR! '{0}' meta stream version is not supported!", metaVersion);
                    return;
            }

            // Attempt to read the d3dtx version of the file
            int d3dtxVersion = ReadD3DTXFileD3DTXVersionOnly(filePath);
            d3dtxConversionType = setD3DTXVersion;

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
                    // if (setD3DTXVersion == D3DTXConversionType.LV1)
                    // {
                    //     d3dtxL1 = new D3DTX_LV1(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV2)
                    // {
                    //     d3dtxL2 = new D3DTX_LV2(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV3)
                    // {
                    //     d3dtxL3 = new D3DTX_LV3(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV4)
                    // {
                    //     d3dtxL4 = new D3DTX_LV4(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV5)
                    // {
                    //     d3dtxL5 = new D3DTX_LV5(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV6)
                    // {
                    //     d3dtxL6 = new D3DTX_LV6(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV7)
                    // {
                    //     d3dtxL7 = new D3DTX_LV7(reader, true);
                    // }
                    // else if (setD3DTXVersion == D3DTXConversionType.LV8)
                    // {
                    //     d3dtxL8 = new D3DTX_LV8(reader, true);
                    // }
                    // else 

                    D3DTXConversionType t = TryToInitializeLegacyD3DTX(reader);
                    Console.WriteLine(t);

                    // if (setD3DTXVersion == D3DTXConversionType.DEFAULT)
                    // {
                    //     try
                    //     {
                    //         if (mbin != null)
                    //         {
                    //             throw new IOException("No DDS Header are found in MBIN meta files. Please use a different conversion option.");
                    //         }

                    //         ddsData = ByteFunctions.GetBytesAfterBytePattern(DDS.MAGIC_WORD, reader.ReadBytes((int)reader.BaseStream.Length));
                    //         ddsImage = DDS_DirectXTexNet.GetDDSImage(ddsData);
                    //     }
                    //     catch (Exception e)
                    //     {
                    //         Console.WriteLine("No DDS Header found");
                    //     }
                    // }
                    // else
                    // {
                    //     Console.WriteLine("ERROR! '{0}' d3dtx version is not supported!", d3dtxVersion);
                    // }
                    // break;
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
            else if (mbin != null) return mbin;
            else return null;
        }

        public D3DTXConversionType TryToInitializeLegacyD3DTX(BinaryReader reader)
        {
            var startPos = reader.BaseStream.Position;

            try
            {
                d3dtxL1 = new D3DTX_LV1(reader, true);
                return D3DTXConversionType.LV1;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL2 = new D3DTX_LV2(reader, true);
                return D3DTXConversionType.LV2;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL3 = new D3DTX_LV3(reader, true);
                return D3DTXConversionType.LV3;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL4 = new D3DTX_LV4(reader, true);
                return D3DTXConversionType.LV4;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL5 = new D3DTX_LV5(reader, true);
                return D3DTXConversionType.LV5;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL6 = new D3DTX_LV6(reader, true);
                return D3DTXConversionType.LV6;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL7 = new D3DTX_LV7(reader, true);
                return D3DTXConversionType.LV7;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL8 = new D3DTX_LV8(reader, true);
                return D3DTXConversionType.LV8;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL9 = new D3DTX_LV9(reader, true);
                return D3DTXConversionType.LV9;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL10 = new D3DTX_LV10(reader, true);
                return D3DTXConversionType.LV10;
            }
            catch { reader.BaseStream.Position = startPos; }

            try
            {
                d3dtxL11 = new D3DTX_LV11(reader, true);
                return D3DTXConversionType.LV11;
            }
            catch { reader.BaseStream.Position = startPos; }

            return D3DTXConversionType.DEFAULT;
        }

        public object GetD3DTXObject()
        {
            if (d3dtxL1 != null) return d3dtxL1;
            else if (d3dtxL2 != null) return d3dtxL2;
            else if (d3dtxL3 != null) return d3dtxL3;
            else if (d3dtxL4 != null) return d3dtxL4;
            else if (d3dtxL5 != null) return d3dtxL5;
            else if (d3dtxL6 != null) return d3dtxL6;
            else if (d3dtxL7 != null) return d3dtxL7;
            else if (d3dtxL8 != null) return d3dtxL8;
            else if (d3dtxL9 != null) return d3dtxL9;
            else if (d3dtxL10 != null) return d3dtxL10;
            else if (d3dtxL11 != null) return d3dtxL11;
            else if (d3dtx3 != null) return d3dtx3;
            else if (d3dtx4 != null) return d3dtx4;
            else if (d3dtx5 != null) return d3dtx5;
            else if (d3dtx6 != null) return d3dtx6;
            else if (d3dtx7 != null) return d3dtx7;
            else if (d3dtx8 != null) return d3dtx8;
            else if (d3dtx9 != null) return d3dtx9;
            else if (!ddsImage.IsNull) return ddsData;
            else return null;
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
            else if (mbin != null) mbin.WriteBinaryData(writer);
            else { Console.WriteLine("Error! No Meta object found!"); }

            if (d3dtxL1 != null) d3dtxL1.WriteBinaryData(writer);
            else if (d3dtxL2 != null) d3dtxL2.WriteBinaryData(writer);
            else if (d3dtxL3 != null) d3dtxL3.WriteBinaryData(writer);
            else if (d3dtxL4 != null) d3dtxL4.WriteBinaryData(writer);
            else if (d3dtxL5 != null) d3dtxL5.WriteBinaryData(writer);
            else if (d3dtxL6 != null) d3dtxL6.WriteBinaryData(writer);
            else if (d3dtxL7 != null) d3dtxL7.WriteBinaryData(writer);
            else if (d3dtxL8 != null) d3dtxL8.WriteBinaryData(writer);
            else if (d3dtxL9 != null) d3dtxL9.WriteBinaryData(writer);
            else if (d3dtxL10 != null) d3dtxL10.WriteBinaryData(writer);
            else if (d3dtxL11 != null) d3dtxL11.WriteBinaryData(writer);
            else if (d3dtx3 != null) d3dtx3.WriteBinaryData(writer);
            else if (d3dtx4 != null) d3dtx4.WriteBinaryData(writer);
            else if (d3dtx5 != null) d3dtx5.WriteBinaryData(writer);
            else if (d3dtx6 != null) d3dtx6.WriteBinaryData(writer);
            else if (d3dtx7 != null) d3dtx7.WriteBinaryData(writer);
            else if (d3dtx8 != null) d3dtx8.WriteBinaryData(writer);
            else if (d3dtx9 != null) d3dtx9.WriteBinaryData(writer);
            else { Console.WriteLine("Error! No D3DTX object found!"); }
        }

        public string GetD3DTXDebugInfo()
        {
            string allInfo = "";

            if (msv6 != null) allInfo += msv6.GetMSV6Info();
            else if (msv5 != null) allInfo += msv5.GetMSV5Info();
            else if (mtre != null) allInfo += mtre.GetMTREInfo();
            else if (mbin != null) allInfo += mbin.GetMBINInfo();
            else allInfo += "Error! Meta data not found!";

            if (d3dtxL1 != null) allInfo += d3dtxL1.GetD3DTXInfo(meta);
            else if (d3dtxL2 != null) allInfo += d3dtxL2.GetD3DTXInfo();
            else if (d3dtxL3 != null) allInfo += d3dtxL3.GetD3DTXInfo();
            else if (d3dtxL4 != null) allInfo += d3dtxL4.GetD3DTXInfo();
            else if (d3dtxL5 != null) allInfo += d3dtxL5.GetD3DTXInfo();
            else if (d3dtxL6 != null) allInfo += d3dtxL6.GetD3DTXInfo();
            else if (d3dtxL7 != null) allInfo += d3dtxL7.GetD3DTXInfo();
            else if (d3dtxL8 != null) allInfo += d3dtxL8.GetD3DTXInfo();
            else if (d3dtxL9 != null) allInfo += d3dtxL9.GetD3DTXInfo();
            else if (d3dtxL10 != null) allInfo += d3dtxL10.GetD3DTXInfo();
            else if (d3dtxL11 != null) allInfo += d3dtxL11.GetD3DTXInfo();
            else if (d3dtx3 != null) allInfo += d3dtx3.GetD3DTXInfo();
            else if (d3dtx4 != null) allInfo += d3dtx4.GetD3DTXInfo();
            else if (d3dtx5 != null) allInfo += d3dtx5.GetD3DTXInfo();
            else if (d3dtx6 != null) allInfo += d3dtx6.GetD3DTXInfo();
            else if (d3dtx7 != null) allInfo += d3dtx7.GetD3DTXInfo();
            else if (d3dtx8 != null) allInfo += d3dtx8.GetD3DTXInfo();
            else if (d3dtx9 != null) allInfo += d3dtx9.GetD3DTXInfo();
            else if (!ddsImage.IsNull) allInfo += DDS_DirectXTexNet.GetDDSDebugInfo(DirectXTex.GetMetadata(ddsImage));
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
            else if (d3dtxConversionType == D3DTXConversionType.LV4)
            {
                d3dtxL4 = d3dtxObject.ToObject<D3DTX_LV4>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV5)
            {
                d3dtxL5 = d3dtxObject.ToObject<D3DTX_LV5>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV6)
            {
                d3dtxL6 = d3dtxObject.ToObject<D3DTX_LV6>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV7)
            {
                d3dtxL7 = d3dtxObject.ToObject<D3DTX_LV7>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV8)
            {
                d3dtxL8 = d3dtxObject.ToObject<D3DTX_LV8>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV9)
            {
                d3dtxL9 = d3dtxObject.ToObject<D3DTX_LV9>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV10)
            {
                d3dtxL10 = d3dtxObject.ToObject<D3DTX_LV10>();
            }
            else if (d3dtxConversionType == D3DTXConversionType.LV11)
            {
                d3dtxL11 = d3dtxObject.ToObject<D3DTX_LV11>();
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
            else if (metaStreamVersion.Equals("NIBM")) mbin = metaObject.ToObject<MBIN>();
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
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL1.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL2 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL2.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL3 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL3.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL4 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL4.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL5 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL5.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL6 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL6.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL7 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL7.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL8 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL8.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL9 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL9.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL10 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL10.ModifyD3DTX(meta, dataArray);
            }
            else if (d3dtxL11 != null)
            {
                var dataArray = DDS_DirectXTexNet.GetDDSByteArray(image, DDSFlags.ForceDx9Legacy);
                d3dtxL11.ModifyD3DTX(meta, dataArray);
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
            MBIN metaMBIN = null;
            if (metaVersion.Equals("6VSM")) meta6VSM = new(reader, false);
            else if (metaVersion.Equals("5VSM")) meta5VSM = new(reader, false);
            else if (metaVersion.Equals("4VSM")) meta5VSM = new(reader, false);
            else if (metaVersion.Equals("ERTM")) metaERTM = new(reader, false);
            else if (metaVersion.Equals("MOCM")) metaERTM = new(reader, false);
            else if (metaVersion.Equals("NIBM")) metaMBIN = new(reader, false);
            else if (metaVersion.Equals("SEBM")) metaMBIN = new(reader, false);

            //read the first int (which is an mVersion d3dtx value)
            if (metaERTM != null)
            {
                return reader.ReadInt32() == 3 ? 3 : -1; //return -1 because d3dtx versions older than 3 don't have an mVersion variable (not that I know of atleast)
            }
            else if (metaMBIN != null) return -1;
            else
                return reader.ReadInt32();
        }

        public bool IsLegacyD3DTX()
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

        public bool IsMbin()
        {
            return mbin != null;
        }

        public bool IsVolumeTexture()
        {
            if (d3dtx5 != null)
                return d3dtx5.mTextureLayout == T3TextureLayout.eTextureLayout_3D;
            else if (d3dtx6 != null)
                return d3dtx6.mTextureLayout == T3TextureLayout.eTextureLayout_3D;
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
            else if (d3dtx6 != null)
                return d3dtx6.mTextureLayout == T3TextureLayout.eTextureLayout_Cube ||
                       d3dtx6.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
            else if (d3dtx7 != null)
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
            if (d3dtx6 != null)
                return d3dtx6.mTextureLayout == T3TextureLayout.eTextureLayout_2DArray ||
                       d3dtx6.mTextureLayout == T3TextureLayout.eTextureLayout_CubeArray;
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
            else if (d3dtxL4 != null)
                return d3dtxL4.mName;
            else if (d3dtxL5 != null)
                return d3dtxL5.mName;
            else if (d3dtxL6 != null)
                return d3dtxL6.mName;
            else if (d3dtxL7 != null)
                return d3dtxL7.mName;
            else if (d3dtxL8 != null)
                return d3dtxL8.mName;
            else if (d3dtxL9 != null)
                return d3dtxL9.mName;
            else if (d3dtxL10 != null)
                return d3dtxL10.mName;
            else if (d3dtxL11 != null)
                return d3dtxL11.mName;
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

        public nuint GetHeight()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mHeight;
            else if (d3dtxL2 != null)
                return d3dtxL2.mHeight;
            else if (d3dtxL3 != null)
                return d3dtxL3.mHeight;
            else if (d3dtxL4 != null)
                return d3dtxL4.mHeight;
            else if (d3dtxL5 != null)
                return d3dtxL5.mHeight;
            else if (d3dtxL6 != null)
                return d3dtxL6.mHeight;
            else if (d3dtxL7 != null)
                return d3dtxL7.mHeight;
            else if (d3dtxL8 != null)
                return d3dtxL8.mHeight;
            else if (d3dtxL9 != null)
                return d3dtxL9.mHeight;
            else if (d3dtxL10 != null)
                return d3dtxL10.mHeight;
            else if (d3dtxL11 != null)
                return d3dtxL11.mHeight;
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
            else if (!ddsImage.IsNull)
                return (nuint)DirectXTex.GetMetadata(ddsImage).Height;
            else
                return 0;
        }

        public nuint GetWidth()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mWidth;
            else if (d3dtxL2 != null)
                return d3dtxL2.mWidth;
            else if (d3dtxL3 != null)
                return d3dtxL3.mWidth;
            else if (d3dtxL4 != null)
                return d3dtxL4.mWidth;
            else if (d3dtxL5 != null)
                return d3dtxL5.mWidth;
            else if (d3dtxL6 != null)
                return d3dtxL6.mWidth;
            else if (d3dtxL7 != null)
                return d3dtxL7.mWidth;
            else if (d3dtxL8 != null)
                return d3dtxL8.mWidth;
            else if (d3dtxL9 != null)
                return d3dtxL9.mWidth;
            else if (d3dtxL10 != null)
                return d3dtxL10.mWidth;
            else if (d3dtxL11 != null)
                return d3dtxL11.mWidth;
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
            else if (!ddsImage.IsNull)
                return (nuint)DirectXTex.GetMetadata(ddsImage).Width;
            else
                return 0;
        }

        public nuint GetDepth()
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
            else if (d3dtxL4 != null)
                return d3dtxL4.mD3DFormat.ToString();
            else if (d3dtxL5 != null)
                return d3dtxL5.mD3DFormat.ToString();
            else if (d3dtxL6 != null)
                return d3dtxL6.mD3DFormat.ToString();
            else if (d3dtxL7 != null)
                return d3dtxL7.mD3DFormat.ToString();
            else if (d3dtxL8 != null)
                return d3dtxL8.mD3DFormat.ToString();
            else if (d3dtxL9 != null)
                return d3dtxL9.mD3DFormat.ToString();
            else if (d3dtxL10 != null)
                return d3dtxL10.mD3DFormat.ToString();
            else if (d3dtxL11 != null)
                return d3dtxL11.mD3DFormat.ToString();
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
            else if (!ddsImage.IsNull)
                return Enum.GetName((DXGIFormat)DirectXTex.GetMetadata(ddsImage).Format);
            else
                return "UNKNOWN COMPRESSION";
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

        public nuint GetArraySize()
        {
            if (d3dtx8 != null)
                return d3dtx8.mArraySize;
            else if (d3dtx9 != null)
                return d3dtx9.mArraySize;
            else
                return 1;
        }

        public RegionStreamHeader[] GetRegionStreamHeaders()
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
                return [];
        }

        public eTxAlpha GetAlpha()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mAlphaMode;
            else if (d3dtxL2 != null)
                return d3dtxL2.mAlphaMode;
            else if (d3dtxL3 != null)
                return d3dtxL3.mAlphaMode;
            else if (d3dtxL4 != null)
                return d3dtxL4.mAlphaMode;
            else if (d3dtxL5 != null)
                return d3dtxL5.mAlphaMode;
            else if (d3dtxL6 != null)
                return d3dtxL6.mAlphaMode;
            else if (d3dtxL7 != null)
                return d3dtxL7.mAlphaMode;
            else if (d3dtxL8 != null)
                return d3dtxL8.mAlphaMode;
            else if (d3dtxL9 != null)
                return d3dtxL9.mAlphaMode;
            else if (d3dtxL10 != null)
                return d3dtxL10.mAlphaMode;
            else if (d3dtxL11 != null)
                return d3dtxL11.mAlphaMode;
            else if (d3dtx3 != null)
                return d3dtx3.mAlphaMode;
            else if (d3dtx4 != null)
                return d3dtx4.mAlphaMode;
            else if (d3dtx5 != null)
                return d3dtx5.mAlphaMode;
            else if (d3dtx6 != null)
                return d3dtx6.mAlphaMode;
            else if (d3dtx7 != null)
                return d3dtx7.mAlphaMode;
            else if (d3dtx8 != null)
                return d3dtx8.mAlphaMode;
            else if (d3dtx9 != null)
                return d3dtx9.mAlphaMode;
            else
                return eTxAlpha.eTxAlphaUnknown;
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
            else if (!ddsImage.IsNull)
            {
                return DirectXTex.HasAlpha(DirectXTex.GetMetadata(ddsImage).Format).ToString();
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
            DXGIFormat format;

            if (!ddsImage.IsNull)
            {
                format = (DXGIFormat)DirectXTex.GetMetadata(ddsImage).Format;
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

        public nuint GetMipMapCount()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mNumMipLevels;
            else if (d3dtxL2 != null)
                return d3dtxL2.mNumMipLevels;
            else if (d3dtxL3 != null)
                return d3dtxL3.mNumMipLevels;
            else if (d3dtxL4 != null)
                return d3dtxL4.mNumMipLevels;
            else if (d3dtxL5 != null)
                return d3dtxL5.mNumMipLevels;
            else if (d3dtxL6 != null)
                return d3dtxL6.mNumMipLevels;
            else if (d3dtxL7 != null)
                return d3dtxL7.mNumMipLevels;
            else if (d3dtxL8 != null)
                return d3dtxL8.mNumMipLevels;
            else if (d3dtxL9 != null)
                return d3dtxL9.mNumMipLevels;
            else if (d3dtxL10 != null)
                return d3dtxL10.mNumMipLevels;
            else if (d3dtxL11 != null)
                return d3dtxL11.mNumMipLevels;
            else if (d3dtx3 != null)
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
            else if (!ddsImage.IsNull)
                return (nuint)DirectXTex.GetMetadata(ddsImage).MipLevels;
            else
                return 0;
        }

        public PlatformType GetPlatformType()
        {
            if (d3dtx4 != null)
                return d3dtx4.mPlatform;
            else if (d3dtx5 != null)
                return d3dtx5.mPlatform;
            else if (d3dtx6 != null)
                return d3dtx6.mPlatform;
            else if (d3dtx7 != null)
                return d3dtx7.mPlatform;
            else if (d3dtx8 != null)
                return d3dtx8.mPlatform;
            else if (d3dtx9 != null)
                return d3dtx9.mPlatform;
            else
                return PlatformType.ePlatform_All;
        }

        public D3DFormat GetD3DFORMAT()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mD3DFormat;
            else if (d3dtxL2 != null)
                return d3dtxL2.mD3DFormat;
            else if (d3dtxL3 != null)
                return d3dtxL3.mD3DFormat;
            else if (d3dtxL4 != null)
                return d3dtxL4.mD3DFormat;
            else if (d3dtxL5 != null)
                return d3dtxL5.mD3DFormat;
            else if (d3dtxL6 != null)
                return d3dtxL6.mD3DFormat;
            else if (d3dtxL7 != null)
                return d3dtxL7.mD3DFormat;
            else if (d3dtxL8 != null)
                return d3dtxL8.mD3DFormat;
            else if (d3dtxL9 != null)
                return d3dtxL9.mD3DFormat;
            else if (d3dtxL10 != null)
                return d3dtxL10.mD3DFormat;
            else if (d3dtxL11 != null)
                return d3dtxL11.mD3DFormat;
            else
                return D3DFormat.UNKNOWN;
        }

        public bool HasMipMaps()
        {
            return GetMipMapCount() > 1;
        }

        public List<byte[]> GetLegacyPixelData()
        {
            if (d3dtxL1 != null)
                return d3dtxL1.mPixelData;
            else if (d3dtxL2 != null)
                return d3dtxL2.mPixelData;
            else if (d3dtxL3 != null)
                return d3dtxL3.mPixelData;
            else if (d3dtxL4 != null)
                return d3dtxL4.mPixelData;
            else if (d3dtxL5 != null)
                return d3dtxL5.mPixelData;
            else if (d3dtxL6 != null)
                return d3dtxL6.mPixelData;
            else if (d3dtxL7 != null)
                return d3dtxL7.mPixelData;
            else if (d3dtxL8 != null)
                return d3dtxL8.mPixelData;
            else if (d3dtxL9 != null)
                return d3dtxL9.mPixelData;
            else if (d3dtxL10 != null)
                return d3dtxL10.mPixelData;
            else if (d3dtxL11 != null)
                return d3dtxL11.mPixelData;
            else
                return [];
        }

        public List<byte[]> GetPixelData()
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
                return [];
        }

        public byte[] GetPixelDataByFaceIndex(int faceIndex, T3SurfaceFormat surfaceFormat, int width, int height, PlatformType platformType)
        {
            List<byte[]> newPixelData = [];

            RegionStreamHeader[] regionHeaders = GetRegionStreamHeaders();

            int divideBy = 1;

            for (int i = 0; i < regionHeaders.Length; i++)
            {
                if (regionHeaders[i].mFaceIndex == faceIndex)
                {
                    if (regionHeaders[i].mMipCount > 1)
                    {
                        newPixelData.Add(GetPixelData()[i]); continue;
                    }

                    if (platformType == PlatformType.ePlatform_PS4)
                    {
                        Console.WriteLine("Unswizzling PS4 texture data..." + i);

                        GetPixelData()[i] = PS4TextureDecoder.UnswizzlePS4(GetPixelData()[i], DDS_HELPER.GetDXGIFromTelltaleSurfaceFormat(surfaceFormat), width / divideBy, height / divideBy);
                    }

                    else if (platformType == PlatformType.ePlatform_PS3 || platformType == PlatformType.ePlatform_WiiU)
                    {
                        Console.WriteLine("Unswizzling PS3 texture data..." + i);

                        GetPixelData()[i] = PS4TextureDecoder.UnswizzlePS3(GetPixelData()[i], DDS_HELPER.GetDXGIFromTelltaleSurfaceFormat(surfaceFormat), width / divideBy, height / divideBy);
                    }
                    else if (platformType == PlatformType.ePlatform_Xbox || platformType == PlatformType.ePlatform_XBOne)
                    {
                        Console.WriteLine("Unswizzling Xbox texture data..." + i);

                        GetPixelData()[i] = Xbox360Texture.DecodeXbox360(GetPixelData()[i], surfaceFormat, width / divideBy, height / divideBy);
                    }

                    if (surfaceFormat == T3SurfaceFormat.eSurface_PVRTC4 || surfaceFormat == T3SurfaceFormat.eSurface_PVRTC4a)
                    {
                        PvrtcDecoder pvrtcDecoder = new PvrtcDecoder();
                        GetPixelData()[i] = pvrtcDecoder.DecompressPVRTC(GetPixelData()[i], width / divideBy, height / divideBy, false);
                        Console.WriteLine("LENGTH " + GetPixelData()[i].Length);
                    }
                    else if (surfaceFormat == T3SurfaceFormat.eSurface_PVRTC2 || surfaceFormat == T3SurfaceFormat.eSurface_PVRTC2a)
                    {
                        PvrtcDecoder pvrtcDecoder = new PvrtcDecoder();
                        GetPixelData()[i] = pvrtcDecoder.DecompressPVRTC(GetPixelData()[i], width / divideBy, height / divideBy, true);
                        Console.WriteLine("LENGTH " + GetPixelData()[i].Length);
                    }

                    divideBy *= 2;

                    newPixelData.Add(GetPixelData()[i]);
                }
            }

            // Reverse the elements in the list to get the correct order.
            newPixelData.Reverse();

            return newPixelData.SelectMany(b => b).ToArray();
        }

        public byte[] GetPixelDataByMipmapIndex(int mipmapIndex, T3SurfaceFormat surfaceFormat, int width, int height, PlatformType platformType)
        {
            List<byte[]> newPixelData = [];

            RegionStreamHeader[] regionHeaders = GetRegionStreamHeaders();

            for (int i = 0; i < regionHeaders.Length; i++)
            {
                if (regionHeaders[i].mMipIndex == mipmapIndex)
                {
                    if (regionHeaders[i].mMipCount > 1)
                    {
                        newPixelData.Add(GetPixelData()[i]); continue;
                    }

                    if (platformType == PlatformType.ePlatform_PS4)
                    {
                        Console.WriteLine("Unswizzling PS4 texture data..." + i);

                        GetPixelData()[i] = PS4TextureDecoder.UnswizzlePS4(GetPixelData()[i], DDS_HELPER.GetDXGIFromTelltaleSurfaceFormat(surfaceFormat), width, height);
                    }

                    else if (platformType == PlatformType.ePlatform_PS3 || platformType == PlatformType.ePlatform_WiiU)
                    {
                        Console.WriteLine("Unswizzling PS3 texture data..." + i);

                        GetPixelData()[i] = PS4TextureDecoder.UnswizzlePS3(GetPixelData()[i], DDS_HELPER.GetDXGIFromTelltaleSurfaceFormat(surfaceFormat), width, height);
                    }

                    else if (platformType == PlatformType.ePlatform_Xbox || platformType == PlatformType.ePlatform_XBOne)
                    {
                        Console.WriteLine("Unswizzling Xbox texture data..." + i);

                        GetPixelData()[i] = Xbox360Texture.DecodeXbox360(GetPixelData()[i], surfaceFormat, width, height);
                    }

                    if (surfaceFormat == T3SurfaceFormat.eSurface_PVRTC4 || surfaceFormat == T3SurfaceFormat.eSurface_PVRTC4a)
                    {
                        PvrtcDecoder pvrtcDecoder = new PvrtcDecoder();
                        GetPixelData()[i] = pvrtcDecoder.DecompressPVRTC(GetPixelData()[i], width, height, false);
                        Console.WriteLine("LENGTH " + GetPixelData()[i].Length);
                    }
                    else if (surfaceFormat == T3SurfaceFormat.eSurface_PVRTC2 || surfaceFormat == T3SurfaceFormat.eSurface_PVRTC2a)
                    {
                        PvrtcDecoder pvrtcDecoder = new PvrtcDecoder();
                        GetPixelData()[i] = pvrtcDecoder.DecompressPVRTC(GetPixelData()[i], width, height, true);
                        Console.WriteLine("LENGTH " + GetPixelData()[i].Length);
                    }

                    newPixelData.Add(GetPixelData()[i]);
                }
            }

            return newPixelData.SelectMany(b => b).ToArray();
        }

        public bool IsTextureCompressed()
        {
            return IsTextureCompressed(GetCompressionType());
        }

        public byte[] GetPixelDataByFirstMipmapIndex(T3SurfaceFormat surfaceFormat, int width, int height, PlatformType platformType)
        {
            return GetPixelDataByMipmapIndex(0, surfaceFormat, width, height, platformType);
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
                T3SurfaceFormat.eSurface_ATC_RGB => true,
                T3SurfaceFormat.eSurface_ATC_RGBA => true,
                T3SurfaceFormat.eSurface_ATC_RGB1A => true,
                T3SurfaceFormat.eSurface_ETC1_RGB => true,
                T3SurfaceFormat.eSurface_ETC2_RGB => true,
                T3SurfaceFormat.eSurface_ETC2_RGBA => true,
                T3SurfaceFormat.eSurface_ETC2_RGB1A => true,
                T3SurfaceFormat.eSurface_ETC2_R => true,
                T3SurfaceFormat.eSurface_ETC2_RG => true,
                T3SurfaceFormat.eSurface_ATSC_RGBA_4x4 => true,
                T3SurfaceFormat.eSurface_PVRTC2 => true,
                T3SurfaceFormat.eSurface_PVRTC4 => true,
                T3SurfaceFormat.eSurface_PVRTC2a => true,
                T3SurfaceFormat.eSurface_PVRTC4a => true,
                _ => false,
            };
        }

        public bool IsFormatIncompatibleWithDDS(T3SurfaceFormat format)
        {
            return format switch
            {
                T3SurfaceFormat.eSurface_ATC_RGB => true,
                T3SurfaceFormat.eSurface_ATC_RGBA => true,
                T3SurfaceFormat.eSurface_ATC_RGB1A => true,
                T3SurfaceFormat.eSurface_ETC1_RGB => true,
                T3SurfaceFormat.eSurface_ETC2_RGB => true,
                T3SurfaceFormat.eSurface_ETC2_RGBA => true,
                T3SurfaceFormat.eSurface_ETC2_RGB1A => true,
                T3SurfaceFormat.eSurface_ETC2_R => true,
                T3SurfaceFormat.eSurface_ETC2_RG => true,
                T3SurfaceFormat.eSurface_ATSC_RGBA_4x4 => true,
                T3SurfaceFormat.eSurface_PVRTC2 => true,
                T3SurfaceFormat.eSurface_PVRTC4 => true,
                T3SurfaceFormat.eSurface_PVRTC2a => true,
                T3SurfaceFormat.eSurface_PVRTC4a => true,
                _ => false,
            };
        }

        ~D3DTX_Master()
        {
            if (!ddsImage.IsNull)
            {
                ddsImage.Release();
            }
        }
    }
}