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
    /// Meta Type REference (MTRE, or ERTM), a meta header often used in telltale files
    /// </summary>
    public class MTRE
    {
        /// <summary>
        /// [4 bytes] The version of the meta stream version.
        /// </summary>
        public string mMetaStreamVersion { get; set; }

        /// <summary>
        /// [4 bytes] Amount of class name elements (CRC64 Class Names) used in the file.
        /// </summary>
        public uint mClassNamesLength { get; set; }

        /// <summary>
        /// [12 bytes for each element] An array of class names (CRC64 Class Names) that are used in the file.
        /// </summary>
        public ClassNames[] mClassNames { get; set; }

        /// <summary>
        /// Meta Header (empty constructor, only used for json deserialization)
        /// </summary>
        public MTRE() { }

        /// <summary>
        /// Parses the Meta Header from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytePointerPosition"></param>
        /// <param name="showConsole"></param>
        public MTRE(BinaryReader reader, bool showConsole = true)
        {
            mMetaStreamVersion += ByteFunctions.ReadFixedString(reader, 4); //Meta Stream Keyword [4 bytes]
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
            Console.WriteLine("Meta mClassNamesLength = {0}", mClassNamesLength);

            for (int i = 0; i < mClassNames.Length; i++)
            {
                Console.WriteLine("Meta mClassName {0} = {1}", i, mClassNames[i]);
            }
        }
    }
}
