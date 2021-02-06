using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TextureMod_GUI
{
    public class Converter_Utillities
    {
        //-----------------------------------------------UTILLITIES-----------------------------------------------
        //-----------------------------------------------UTILLITIES-----------------------------------------------
        //-----------------------------------------------UTILLITIES-----------------------------------------------

        /// <summary>
        /// Modifies Bytes in a byte array.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newBytes"></param>
        /// <param name="indexOffset"></param>
        /// <returns></returns>
        public byte[] ModifyBytes(byte[] source, byte[] newBytes, int indexOffset)
        {
            //run a loop and begin going through for the lenght of the bytes
            for (int i = 0; i < newBytes.Length; i++)
            {
                //assign the value from the source byte array with the offset
                source[indexOffset + i] = newBytes[i];
            }

            //return the final byte array
            return source;
        }

        /// <summary>
        /// Filters an array of files by 'filterExtension' so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> FilterFiles(List<string> files, string filterExtension)
        {
            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                if (extension.Equals(filterExtension))
                {
                    //add the matched extension to the list
                    filteredFiles.Add(file);
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Calculates the byte size of a DDS texture
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="isDXT1"></param>
        /// <returns></returns>
        public int CalculateDDS_ByteSize(int width, int height, bool isDXT1)
        {
            int compression = 0;

            //according to formula, if the compression is dxt1 then the number needs to be 8
            if (isDXT1)
                compression = 8;
            else
                compression = 16;

            //formula (from microsoft docs)
            //max(1, ( (width + 3) / 4 ) ) x max(1, ( (height + 3) / 4 ) ) x 8(DXT1) or 16(DXT2-5)

            //do the micorosoft magic texture byte size calculation formula
            return Math.Max(1, ((width + 3) / 4)) * Math.Max(1, ((height + 3) / 4)) * compression;
        }

        /// <summary>
        /// Combines two byte arrays into one.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public byte[] Combine(byte[] first, byte[] second)
        {
            //allocate a byte array with both total lengths combined to accomodate both
            byte[] bytes = new byte[first.Length + second.Length];

            //copy the data from the first array into the new array
            Buffer.BlockCopy(first, 0, bytes, 0, first.Length);

            //copy the data from the second array into the new array (offset by the total length of the first array)
            Buffer.BlockCopy(second, 0, bytes, first.Length, second.Length);

            //return the final byte array
            return bytes;
        }

        /// <summary>
        /// Allocates a byte array of 'size' length. 
        /// <para>Depending on 'size' it allocates 'size' amount of bytes from 'sourceByteArray' offset by 'offsetLocation'</para>
        /// </summary>
        /// <param name="size"></param>
        /// <param name="sourceByteArray"></param>
        /// <param name="offsetLocation"></param>
        /// <returns></returns>
        public byte[] AllocateByteArray(int size, byte[] sourceByteArray, int offsetLocation)
        {
            //allocate byte array of fixed length
            byte[] source_imageHeight = new byte[size];

            //run a loop and begin gathering values
            for (int i = 0; i < size; i++)
            {
                //assign the value from the source byte array with the offset
                source_imageHeight[i] = sourceByteArray[offsetLocation + i];
            }

            //return the final byte array
            return source_imageHeight;
        }
    }
}
