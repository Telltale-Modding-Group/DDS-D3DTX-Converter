using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using D3DTX_TextureConverter.Telltale;
using D3DTX_TextureConverter.Utilities;

/*
 * This is a meta stream header.
 * These objects are often at the top of every telltale file.
 * They also contain info regarding the byte size of certain data chunks, along with the classes that are used (which are CRC64'd sadly)
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

namespace D3DTX_TextureConverter.Main
{
    /// <summary>
    /// Meta Stream Version 5 (MSV5 or 5VSM), a meta header often used in telltale files
    /// </summary>
    public class MSV5
    {
        /// <summary>
        /// [4 bytes] The version of the meta stream version.
        /// </summary>
        public string mMetaStreamVersion { get; set; }

        /// <summary>
        /// [4 bytes] The size of the data 'header' after the meta header.
        /// </summary>
        public uint mDefaultSectionChunkSize { get; set; }

        /// <summary>
        /// [4 bytes] The size of the debug data chunk after the meta header, which is always 0.
        /// </summary>
        public uint mDebugSectionChunkSize { get; set; }

        /// <summary>
        /// [4 bytes] The size of the asynchronous data chunk (not the meta header, or the data chunk header, but the data itself).
        /// </summary>
        public uint mAsyncSectionChunkSize { get; set; }

        /// <summary>
        /// [4 bytes] Amount of class name elements (CRC64 Class Names) used in the file.
        /// </summary>
        public uint mClassNamesLength { get; set; }

        /// <summary>
        /// [12 bytes for each element] An array of class names (CRC64 Class Names) that are used in the file.
        /// </summary>
        public ClassNames[] mClassNames { get; set; }

        /// <summary>
        /// (Not actually in the header) Gets the length of the Meta Header in bytes.
        /// </summary>
        private uint MetaHeaderLength { get; set; }

        /// <summary>
        /// Parses the Meta Header from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytePointerPosition"></param>
        /// <param name="showConsole"></param>
        public MSV5(byte[] data, ref uint bytePointerPosition, bool showConsole = true)
        {
            MetaHeaderLength = 0;

            if(showConsole)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
                Console.WriteLine("||||||||||| Meta Header |||||||||||");
            }

            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            mMetaStreamVersion = ByteFunctions.ReadFixedString(data, 4, ref bytePointerPosition);

            if (showConsole)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                Console.WriteLine("D3DTX Meta Stream Keyword = {0}", mMetaStreamVersion);
            }

            MetaHeaderLength += 4;

            //--------------------------Default Section Chunk Size-------------------------- [4 bytes] //default section chunk size
            mDefaultSectionChunkSize = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);

            if (showConsole)
                Console.WriteLine("D3DTX Default Section Chunk Size = {0}", mDefaultSectionChunkSize);

            MetaHeaderLength += 4;

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
            mDebugSectionChunkSize = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);

            if (showConsole)
                Console.WriteLine("D3DTX Debug Section Chunk Size = {0}", mDebugSectionChunkSize);

            MetaHeaderLength += 4;

            //--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
            mAsyncSectionChunkSize = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);

            if (showConsole)
                Console.WriteLine("D3DTX Async Section Chunk Size = {0}", mAsyncSectionChunkSize);

            MetaHeaderLength += 4;

            //--------------------------mClassNamesLength-------------------------- [4 bytes]
            mClassNamesLength = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);

            if (showConsole)
                Console.WriteLine("D3DTX mClassNamesLength = {0}", mClassNamesLength);

            MetaHeaderLength += 4;

            //--------------------------mClassNames--------------------------
            mClassNames = new ClassNames[mClassNamesLength];

            for (int i = 0; i < mClassNames.Length; i++)
            {
                mClassNames[i] = new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = ByteFunctions.ReadUnsignedLong(data, ref bytePointerPosition)
                    },
                    mVersionCRC = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition)
                };

                MetaHeaderLength += 12;

                if (showConsole)
                    Console.WriteLine("D3DTX mClassName {0} = {1}", i, mClassNames[i]);
            }
        }

        /// <summary>
        /// Converts the data of this object into a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteData()
        {
            byte[] finalData = new byte[0];

            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            finalData = ByteFunctions.Combine(finalData, ByteFunctions.GetBytes(mMetaStreamVersion));

            //--------------------------Default Section Chunk Size-------------------------- [4 bytes] //default section chunk size
            finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mDefaultSectionChunkSize));

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
            finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mDebugSectionChunkSize));

            //--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
            finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mAsyncSectionChunkSize));

            //--------------------------mClassNamesLength-------------------------- [4 bytes]
            finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mClassNamesLength));

            //--------------------------mClassNames--------------------------
            for (int i = 0; i < mClassNames.Length; i++)
            {
                //symbol
                finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mClassNames[i].mTypeNameCRC.mCrc64));

                //version crc
                finalData = ByteFunctions.Combine(finalData, BitConverter.GetBytes(mClassNames[i].mVersionCRC));
            }

            return finalData;
        }

        public uint Get_MetaHeaderLength()
        {
            return MetaHeaderLength;
        }
    }
}
