using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.Telltale;

namespace D3DTX_TextureConverter
{
    public class D3DTX_File
    {
        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        //custom header file extension (generated from d3dtx to dds, used to convert dds back to d3dtx)
        public static string headerExtension = ".header";

        //6VSM version of a .d3dtx
        public D3DTX_6VSM D3DTX_6VSM;

        //5VSM version of a .d3dtx
        public D3DTX_5VSM D3DTX_5VSM;

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

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine(".d3dtx data stored in {0}", headerFilePath);
        }

        public void Apply_DDS_Data_To_D3DTX_Data(DDS_File ddsFile, bool applyToHeader)
        {
            //Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            //Console.WriteLine("Experimental Resolution Changed enabled, changing values on the original D3DTX header...");

            //byte[] mainData = applyToHeader ? headerData : sourceByteFile;

            //--------------------------3 = mDataSize--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, ddsFile.ddsTextureData.Length, byteLocation_mDataSize);
            //--------------------------9 = mWidth--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, mWidth, byteLocation_mWidth);
            //--------------------------10 = mHeight--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, mHeight, byteLocation_mHeight);
            //--------------------------12 = mTotalDataSize--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, ddsFile.ddsTextureData.Length, byteLocation_mTotalDataSize);
        }

        public static string Read_D3DTX_File_MetaVersionOnly(string sourceFile)
        {
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

            uint bytePointerPosition = 0;

            return ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
        }
    }
}
