using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.Telltale;

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
            string file_dword = Read_D3DTX_File_MetaVersionOnly(filePath);

            if (file_dword.Equals("6VSM"))
            {
                //read D3DTX file
                D3DTX_6VSM = new D3DTX_6VSM(filePath, false);
            }
            else if (file_dword.Equals("5VSM"))
            {
                //read D3DTX file
                D3DTX_5VSM = new D3DTX_5VSM(filePath, false);
            }
            else if (file_dword.Equals("ERTM"))
            {
                //read D3DTX file
                D3DTX_ERTM = new D3DTX_ERTM(filePath, false);

                return;
            }

            //create a DDS file in memory based off the d3dtx
            //DDS_File ddsFile = new DDS_File(this);

            //write the dds file to disk
            //ddsFile.Write_D3DTX_AsDDS(this, destinationFile);

            //write the d3dtx header to disk
            //d3dtx_file.Write_D3DTX_Header(destinationFile);

            //GenericImageFormats.ConvertDDS_To_PSD(destinationFile);
        }

        public void Write_D3DTX_Header(string destinationFile)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Generating .header file to store the .d3dtx header data.");

            //build the header destination path, assuming the extnesion of the destination file path is .dds
            string headerFilePath = string.Format("{0}{1}", destinationFile.Remove(destinationFile.Length - 4, 4), headerExtension);

            //write the header data to a file
            if(D3DTX_6VSM != null)
            {
                File.WriteAllBytes(headerFilePath, D3DTX_6VSM.Data_OriginalHeader);
            }
            else if(D3DTX_5VSM != null)
            {
                File.WriteAllBytes(headerFilePath, D3DTX_5VSM.Data_OriginalHeader);
            }
            else if (D3DTX_ERTM != null)
            {
                File.WriteAllBytes(headerFilePath, D3DTX_ERTM.Data_OriginalHeader);
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine(".d3dtx data stored in {0}", headerFilePath);
        }

        public void Apply_DDS_Data_To_D3DTX_Data(DDS_File ddsFile, bool applyToHeader)
        {

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
    }
}
