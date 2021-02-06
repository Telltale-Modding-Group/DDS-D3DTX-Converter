using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TextureMod_GUI
{
    public class Read_DDS
    {
        private Converter_Utillities converter_utillities;

        public Read_DDS()
        {
            converter_utillities = new Converter_Utillities();
        }

        public bool HasMipMaps(string ddsPath)
        {
            //get the mip count
            int mipCount = MipMapCount(ddsPath);

            //return the final value (if its greater then 0, it has mips)
            return mipCount > 0;
        }

        public string DDSFormat(string ddsPath)
        {
            //read the source texture file into a byte array
            byte[] sourceTexFileData = File.ReadAllBytes(ddsPath);

            //initalize our variables for the dds header
            string texture_parsed_compressionType; //compression type of the dds file

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            int texture_bytePointerPosition = 0;

            //skip ahead to the mip map count
            texture_bytePointerPosition = 84;

            //allocate 4 byte array (int32)
            byte[] texture_source_compressionType = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_compressionType = Encoding.ASCII.GetString(texture_source_compressionType);

            //return the final value
            return texture_parsed_compressionType;
        }

        public int MipMapCount(string ddsPath)
        {
            //read the source texture file into a byte array
            byte[] sourceTexFileData = File.ReadAllBytes(ddsPath);

            //initalize our variables for the dds header
            int texture_parsed_mipMapCount; //total amount of mip maps in the dds file

            //which byte offset we are on for the source texture (will be changed as we go through the file)
            int texture_bytePointerPosition = 0;

            //skip ahead to the mip map count
            texture_bytePointerPosition = 28;

            //allocate 4 byte array (int32)
            byte[] texture_source_mipMapCount = converter_utillities.AllocateByteArray(4, sourceTexFileData, texture_bytePointerPosition);

            //parse the byte array to int32
            texture_parsed_mipMapCount = BitConverter.ToInt32(texture_source_mipMapCount);

            //return the final value
            return texture_parsed_mipMapCount;
        }
    }
}
