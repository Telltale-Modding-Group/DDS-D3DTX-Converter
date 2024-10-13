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
        public MSV5(BinaryReader reader, bool showConsole = true)
        {
            mMetaStreamVersion += ByteFunctions.ReadFixedString(reader, 4); //Meta Stream Keyword [4 bytes]
            mDefaultSectionChunkSize = reader.ReadUInt32(); //Default Section Chunk Size [4 bytes] //default section chunk size
            mDebugSectionChunkSize = reader.ReadUInt32(); //Debug Section Chunk Size [4 bytes] //debug section chunk size (always zero)
            mAsyncSectionChunkSize = reader.ReadUInt32(); //Async Section Chunk Size [4 bytes] //async section chunk size (size of the bytes after the file header)
            mClassNamesLength = reader.ReadUInt32(); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            mClassNames = new ClassNames[mClassNamesLength];

            for (int i = 0; i < mClassNames.Length; i++)
            {
                mClassNames[i] = new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = reader.ReadUInt64()
                    },
                    mVersionCRC = reader.ReadUInt32()
                };
            }

            MetaHeaderLength = (4 * 5) + (12 * mClassNamesLength);

            if (showConsole)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
                Console.WriteLine("||||||||||| Meta Header |||||||||||");
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                Console.WriteLine("Meta Stream Keyword = {0}", mMetaStreamVersion);
                Console.WriteLine("Meta Default Section Chunk Size = {0}", mDefaultSectionChunkSize);
                Console.WriteLine("Meta Debug Section Chunk Size = {0}", mDebugSectionChunkSize);
                Console.WriteLine("Meta Async Section Chunk Size = {0}", mAsyncSectionChunkSize);
                Console.WriteLine("Meta mClassNamesLength = {0}", mClassNamesLength);

                for (int i = 0; i < mClassNames.Length; i++)
                {
                    Console.WriteLine("Meta mClassName {0} = {1}", i, mClassNames[i]);
                }
            }
        }

        /// <summary>
        /// Meta Stream Header version 5 (empty constructor, only used for json deserialization)
        /// </summary>
        public MSV5() { }

        /// <summary>
        /// Converts the data of this object into a byte array.
        /// </summary>
        /// <returns></returns>
        public void GetByteData(BinaryWriter writer)
        {
            ByteFunctions.WriteFixedString(writer, mMetaStreamVersion); //Meta Stream Keyword [4 bytes]
            writer.Write(mDefaultSectionChunkSize); //Default Section Chunk Size [4 bytes] default section chunk size
            writer.Write(mDebugSectionChunkSize); //Debug Section Chunk Size [4 bytes] debug section chunk size (always zero)
            writer.Write(mAsyncSectionChunkSize); //Async Section Chunk Size [4 bytes] async section chunk size (size of the bytes after the file header)
            writer.Write(mClassNamesLength); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            for (int i = 0; i < mClassNames.Length; i++)
            {
                writer.Write(mClassNames[i].mTypeNameCRC.mCrc64); //symbol
                writer.Write(mClassNames[i].mVersionCRC); //version crc
            }
        }

        public uint Get_MetaHeaderLength()
        {
            return MetaHeaderLength;
        }
    }
}
