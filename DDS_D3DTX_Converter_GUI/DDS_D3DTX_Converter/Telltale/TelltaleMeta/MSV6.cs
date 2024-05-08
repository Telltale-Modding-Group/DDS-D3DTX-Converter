using System;
using System.IO;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using System.Runtime.InteropServices;

/*
 * This is a meta stream header.
 * These objects are often at the top of every telltale file.
 * They also contain info regarding the byte size of certain data chunks, along with the classes that are used (which are CRC64'd sadly)
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

namespace D3DTX_Converter.TelltaleMeta
{
    /// <summary>
    /// Meta Stream Version 6 (MSV6, or 6VSM), a meta header often used in telltale files
    /// </summary>
    public class MSV6
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
        /// Meta Stream Header version 6 (empty constructor, only used for json deserialization)
        /// </summary>
        public MSV6() { }

        /// <summary>
        /// Parses the Meta Header from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="showConsole"></param>
        public MSV6(BinaryReader reader, bool showConsole = true)
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
                mClassNames[i] = new ClassNames(reader);
            }

            if (showConsole)
                PrintConsole();
        }

        /// <summary>
        /// Converts the data of this object into a byte array.
        /// </summary>
        /// <returns></returns>
        public void WriteBinaryData(BinaryWriter writer)
        {
            ByteFunctions.WriteFixedString(writer, mMetaStreamVersion); //Meta Stream Keyword [4 bytes]
            writer.Write(mDefaultSectionChunkSize); //Default Section Chunk Size [4 bytes] default section chunk size
            writer.Write(mDebugSectionChunkSize); //Debug Section Chunk Size [4 bytes] debug section chunk size (always zero)
            writer.Write(mAsyncSectionChunkSize); //Async Section Chunk Size [4 bytes] async section chunk size (size of the bytes after the file header)
            writer.Write(mClassNamesLength); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            for (int i = 0; i < mClassNames.Length; i++)
            {
                mClassNames[i].WriteBinaryData(writer);
            }
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mMetaStreamVersion); //mMetaStreamVersion [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mDefaultSectionChunkSize); //mDefaultSectionChunkSize [4 bytes] //default section chunk size
            totalByteSize += (uint)Marshal.SizeOf(mDebugSectionChunkSize); //mDebugSectionChunkSize [4 bytes] //debug section chunk size (always zero)
            totalByteSize += (uint)Marshal.SizeOf(mAsyncSectionChunkSize); //mAsyncSectionChunkSize [4 bytes] //async section chunk size (size of the bytes after the file header)
            totalByteSize += (uint)Marshal.SizeOf(mClassNamesLength); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            for (int i = 0; i < mClassNames.Length; i++)
            {
                totalByteSize += mClassNames[i].GetByteSize();
            }

            return totalByteSize;
        }

        public void PrintConsole()
        {
            Console.WriteLine("||||||||||| Meta Header |||||||||||");
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

        public string GetMSV6Info()
        {
            string metaInfo = "||||||||||| Meta Header |||||||||||" + Environment.NewLine;
            
            metaInfo += string.Format("Meta Stream Keyword = {0}", mMetaStreamVersion) + Environment.NewLine;
            metaInfo += string.Format("Meta Default Section Chunk Size = {0}", mDefaultSectionChunkSize) + Environment.NewLine;
            metaInfo += string.Format("Meta Debug Section Chunk Size = {0}", mDebugSectionChunkSize) + Environment.NewLine;
            metaInfo += string.Format("Meta Async Section Chunk Size = {0}", mAsyncSectionChunkSize) + Environment.NewLine;
            metaInfo += string.Format("Meta mClassNamesLength = {0}", mClassNamesLength) + Environment.NewLine;

            for (int i = 0; i < mClassNames.Length; i++)
            {
                metaInfo += string.Format("Meta mClassName {0} = {1}", i, mClassNames[i]) + Environment.NewLine;
            }

            return metaInfo;
        }
    }
}
