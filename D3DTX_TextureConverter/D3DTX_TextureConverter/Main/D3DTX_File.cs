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

/*
 * NOTE:
 * When it comes to texture conversion for versions 6VSM and 5VSM a json file is generated along with the extracted DDS.
 * However, for versions OLDER THAN 5VSM, a header file is generated which contains the original d3dtx header data.
 * 
 * The reason for this is that thanks to the PDB that shipped with TWD DE, we were able to reverse engineer the D3DTX file format.
 * Using that file format we were also able to figure out 5VSM quickly as well given that the structure was very similar to 6VSM.
 * 
 * Unfortunately for older game versions, the structure is mostly different, and we can only geuss as to how the file is structured given that we don't have access to a PDB.
 * To add to that also, none of the older games shipped with a PDB (not that we know of atleast).
 * So because of that, we instead opt to copy of the original d3dtx header and store it in a seperate .header file.
 * When rebuilding the old file, only a set amount of bytes are modified in that original header before we rebuild the file.
*/

namespace D3DTX_TextureConverter.Main
{
    public class D3DTX_File
    {
        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        //json file extension used for 6VSM and 5VSM d3dtx versions
        public static string jsonExtension = ".json";

        //custom header file extension (used for d3dtx versions older than 5VSM)
        public static string headerExtension = ".header";

        //6VSM version of a .d3dtx
        public D3DTX_6VSM D3DTX_6VSM;

        //5VSM version of a .d3dtx
        public D3DTX_5VSM D3DTX_5VSM;

        //ERTM version of a .d3dtx
        public D3DTX_ERTM D3DTX_ERTM;

        public void Read_D3DTX_File(string filePath)
        {
            //read the DWORD to determine the meta version of the file
            string file_dword = Read_D3DTX_File_MetaVersionOnly(filePath);

            //read the file depending on its respective version
            if (file_dword.Equals("6VSM"))
            {
                D3DTX_6VSM = new D3DTX_6VSM(filePath);
            }
            else if (file_dword.Equals("5VSM"))
            {
                D3DTX_5VSM = new D3DTX_5VSM(filePath);
            }
            else if (file_dword.Equals("ERTM"))
            {
                D3DTX_ERTM = new D3DTX_ERTM(filePath, false);

                return;
            }
        }

        public void Write_D3DTX_Header(string destinationFile)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Generating .header file to store the .d3dtx header data.");

            //build the header destination path, assuming the extnesion of the destination file path is .dds
            string headerFilePath = string.Format("{0}{1}", destinationFile.Remove(destinationFile.Length - 4, 4), headerExtension);

            //write the header data to a file
            //NOTE TO SELF: REMOVED 6VSM AND 5VSM because we can actually rebuild the header from scratch, for older versions however we need to keep the headers
            if (D3DTX_ERTM != null)
            {
                File.WriteAllBytes(headerFilePath, D3DTX_ERTM.Data_OriginalHeader);
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine(".d3dtx data stored in {0}", headerFilePath);
        }

        public static string Read_D3DTX_File_MetaVersionOnly(string sourceFile)
        {
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

            uint bytePointerPosition = 0;

            return ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
        }

        public object Get_D3DTX_Object()
        {
            if (D3DTX_6VSM != null)
                return D3DTX_6VSM;
            else if (D3DTX_5VSM != null)
                return D3DTX_5VSM;
            else
                return null;
        }

        /// <summary>
        /// Writes a final .d3dtx file to disk
        /// </summary>
        /// <param name="destinationPath"></param>
        public void Write_Final_D3DTX(string destinationPath)
        {
            if (D3DTX_6VSM != null)
                D3DTX_6VSM.WriteD3DTX(destinationPath);
            else if (D3DTX_5VSM != null)
                D3DTX_5VSM.WriteD3DTX(destinationPath);
            //else if (D3DTX_ERTM != null)
            //    Write_D3DTX_Header(destinationPath);
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
            JObject obj = JObject.Parse(jsonText);

            string metaStreamVersion = "";

            //loop through each property to get the data
            foreach (JProperty property in obj.Properties())
            {
                //get the name of the property from the json object
                string name = property.Name;

                if (name.Equals("mMetaStreamVersion"))
                {
                    metaStreamVersion = (string)property.Value;
                    break;
                }
            }

            if (metaStreamVersion.Equals("6VSM"))
                D3DTX_6VSM = JsonConvert.DeserializeObject<D3DTX_6VSM>(jsonText);
            else if (metaStreamVersion.Equals("5VSM"))
                D3DTX_5VSM = JsonConvert.DeserializeObject<D3DTX_5VSM>(jsonText);
        }

        /// <summary>
        /// Writes a companion file as a .json/.header depending on the d3dtx version
        /// </summary>
        /// <param name="destinationPath"></param>
        public void Write_D3DTX_CompanionFile(string destinationPath)
        {
            if(D3DTX_6VSM != null)
            {
                Write_D3DTX_JSON(destinationPath);
            }
            else if(D3DTX_5VSM != null)
            {
                Write_D3DTX_JSON(destinationPath);
            }
            else
            {
                Write_D3DTX_Header(destinationPath);
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
                JsonSerializer serializer = new JsonSerializer();

                //seralize the data and write it to the configruation file
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, Get_D3DTX_Object());
            }
        }

        public void Modify_D3DTX(DDS_File dds)
        {
            if (D3DTX_6VSM != null)
                D3DTX_6VSM.ModifyD3DTX(dds);
            else if (D3DTX_5VSM != null)
                D3DTX_5VSM.ModifyD3DTX(dds);
        }
    }
}
