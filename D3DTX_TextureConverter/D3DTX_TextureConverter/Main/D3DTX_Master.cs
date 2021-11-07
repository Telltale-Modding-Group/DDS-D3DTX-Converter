using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.Telltale;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft;

namespace D3DTX_TextureConverter.Main
{
    /// <summary>
    /// This is the master class object for a D3DTX file. Reads a file and automatically parses the data into the correct version.
    /// </summary>
    public class D3DTX_Master
    {
        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        //json file extension used for 6VSM and 5VSM d3dtx versions
        public static string jsonExtension = ".json";

        //custom header file extension (used for d3dtx versions older than 5VSM)
        public static string headerExtension = ".header";

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

            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath)))
            {
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
                    case 4:
                        d3dtx4 = new(reader);
                        break;
                    case 5:
                        d3dtx5 = new(reader);
                        break;
                    case 6:
                        d3dtx6 = new(reader);
                        break;
                    case 7:
                        d3dtx7 = new(reader);
                        break;
                    case 8:
                        d3dtx8 = new(reader);
                        break;
                    case 9:
                        d3dtx9 = new(reader);
                        break;
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
            if (msv6 != null)
                return msv6;
            else if (msv5 != null)
                return msv5;
            else if (mtre != null)
                return mtre;
            else
                return null;
        }

        public object Get_D3DTX_Object()
        {
            if (d3dtx4 != null)
                return d3dtx4;
            else if (d3dtx5 != null)
                return d3dtx5;
            else if (d3dtx6 != null)
                return d3dtx6;
            else if (d3dtx7 != null)
                return d3dtx7;
            else if (d3dtx8 != null)
                return d3dtx8;
            else if (d3dtx9 != null)
                return d3dtx9;
            else
                return null;
        }

        /// <summary>
        /// Writes a final .d3dtx file to disk
        /// </summary>
        /// <param name="destinationPath"></param>
        public void Write_Final_D3DTX(string destinationPath)
        {
            byte[] finalData = new byte[0];

            using(BinaryWriter writer = new BinaryWriter(File.OpenWrite(destinationPath)))
            {
                if (msv6 != null)
                    msv6.GetByteData(writer);
                else if (msv5 != null)
                    msv5.GetByteData(writer);
                else if (mtre != null)
                    mtre.GetByteData(writer);

                if (d3dtx4 != null)
                    d3dtx4.WriteBinaryData(writer);
                else if (d3dtx5 != null)
                    d3dtx5.WriteBinaryData(writer);
                else if (d3dtx6 != null)
                    d3dtx6.WriteBinaryData(writer);
                else if (d3dtx7 != null)
                    d3dtx7.WriteBinaryData(writer);
                else if (d3dtx8 != null)
                    d3dtx8.WriteBinaryData(writer);
                else if (d3dtx9 != null)
                    d3dtx9.WriteBinaryData(writer);
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

            //loop through each property to get the data in the meta object
            foreach (JProperty property in metaObject.Properties())
            {
                //get the name of the property from the json object
                string name = property.Name;

                if (name.Equals("mMetaStreamVersion"))
                {
                    metaStreamVersion = (string)property.Value;
                    break;
                }
            }

            //deserialize the appropriate json object
            if (metaStreamVersion.Equals("6VSM"))
                msv6 = JsonConvert.DeserializeObject<MSV6>(jsonText);
            else if (metaStreamVersion.Equals("5VSM"))
                msv5 = JsonConvert.DeserializeObject<MSV5>(jsonText);
            else if(metaStreamVersion.Equals("ERTM"))
                mtre = JsonConvert.DeserializeObject<MTRE>(jsonText);

            //d3dtx object
            JObject d3dtxObject = jarray[1] as JObject;

            //loop through each property to get the data in the meta object
            foreach (JProperty property in d3dtxObject.Properties())
            {
                //get the name of the property from the json object
                string name = property.Name;

                if (name.Equals("mMetaStreamVersion"))
                {
                    metaStreamVersion = (string)property.Value;
                    break;
                }
            }
        }

        public void Write_D3DTX_JSON(string destinationPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(destinationPath);
            string fileDirectory = Path.GetDirectoryName(destinationPath);
            string newPath = fileDirectory + "/" + fileName + jsonExtension;

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

        public void Modify_D3DTX(DDS_Master dds)
        {
            if (d3dtx4 != null)
                d3dtx4.ModifyD3DTX(dds);
            else if (d3dtx5 != null)
                d3dtx5.ModifyD3DTX(dds);
            else if (d3dtx6 != null)
                d3dtx6.ModifyD3DTX(dds);
            else if (d3dtx7 != null)
                d3dtx7.ModifyD3DTX(dds);
            else if (d3dtx8 != null)
                d3dtx8.ModifyD3DTX(dds);
            else if (d3dtx9 != null)
                d3dtx9.ModifyD3DTX(dds);
        }

        /// <summary>
        /// Reads a d3dtx file on the disk and returns the meta version that is being used.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public static string Read_D3DTX_File_MetaVersionOnly(string sourceFile)
        {
            string metaStreamVersion = "";

            using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceFile)))
            {
                metaStreamVersion += reader.ReadChars(4);
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
            int mVersion = -1;

            using (BinaryReader reader = new BinaryReader(File.OpenRead(sourceFile)))
            {
                if (metaVersion.Equals("6VSM"))
                {
                    MSV6 meta6VSM = new(reader, false);
                }
                else if (metaVersion.Equals("5VSM"))
                {
                    MSV5 meta5VSM = new(reader, false);
                }
                else if (metaVersion.Equals("ERTM"))
                {
                    MTRE metaERTM = new(reader, false);

                    return mVersion; //return -1 because d3dtx versions older than 4 don't have an mVersion variable (not that I know of atleast)
                }

                //read the first int (which is an mVersion d3dtx value)
                mVersion = reader.ReadInt32();
            }

            return mVersion;
        }
    }
}
