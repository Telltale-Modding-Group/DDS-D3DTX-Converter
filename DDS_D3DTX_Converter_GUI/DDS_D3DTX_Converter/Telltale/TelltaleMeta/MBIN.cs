using System;
using System.IO;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using System.Runtime.InteropServices;

/*
 * This is a meta stream header.
 * These objects are often at the top of every telltale file.
 * They also contain info regarding the byte size of certain data chunks, along with the classes that are used (which are CRC64'd sadly).
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

namespace D3DTX_Converter.TelltaleMeta
{
    /// <summary>
    /// Meta Type REference (MTRE, or ERTM), a meta header often used in telltale files
    /// </summary>
    public class MBIN
    {

        public bool isHashed = false;

        /// <summary>
        /// [4 bytes] The version of the meta stream version.
        /// </summary>
        public string mMetaStreamVersion { get; set; }

        /// <summary>
        /// [4 bytes] Amount of class name elements (CRC64 Class Names) used in the file.
        /// </summary>
        public uint mClassNamesLength { get; set; }

        /// <summary>
        /// [4 + string length + 4 bytes for each element] An array of class names (Unhashed Class Names) that are used in the file.
        /// </summary>
        public UnhashedClassNames[] mUnhashedClassNames { get; set; }

        /// <summary>
        /// [4 + string length + 4 bytes for each element] An array of class names (Unhashed Class Names) that are used in the file.
        /// </summary>
        public ClassNames[] mClassNames { get; set; }

        /// <summary>
        /// Meta Header (empty constructor, only used for json deserialization)
        /// </summary>
        public MBIN() { }

        /// <summary>
        /// Parses the Meta Header from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="showConsole"></param>
        public MBIN(BinaryReader reader, bool showConsole = true)
        {
            mMetaStreamVersion += ByteFunctions.ReadFixedString(reader, 4); //Meta Stream Keyword [4 bytes]
            mClassNamesLength = reader.ReadUInt32(); //mClassNamesLength [4 bytes]

            long pos = reader.BaseStream.Position;

            uint nextNum = reader.ReadUInt32();

            if (nextNum >= 60)
            {
                isHashed = true;
            }

            reader.BaseStream.Position = pos;

            Console.WriteLine(isHashed);

            //--------------------------mClassNames--------------------------
            if (isHashed)
            {
                mClassNames = new ClassNames[mClassNamesLength];

                for (int i = 0; i < mClassNames.Length; i++)
                {
                    mClassNames[i] = new ClassNames(reader);
                }
            }
            else
            {
                mUnhashedClassNames = new UnhashedClassNames[mClassNamesLength];

                for (int i = 0; i < mUnhashedClassNames.Length; i++)
                {
                    mUnhashedClassNames[i] = new UnhashedClassNames(reader);
                }
            }

            mUnhashedClassNames = new UnhashedClassNames[mClassNamesLength];

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
            for (int i = 0; i < mUnhashedClassNames.Length; i++)
            {
                mUnhashedClassNames[i].WriteBinaryData(writer);
            }
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mMetaStreamVersion); //mMetaStreamVersion [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mClassNamesLength); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            if (isHashed)
            {
                for (int i = 0; i < mClassNames.Length; i++)
                {
                    totalByteSize += mClassNames[i].GetByteSize();
                }
            }
            else
            {
                for (int i = 0; i < mUnhashedClassNames.Length; i++)
                {
                    totalByteSize += mUnhashedClassNames[i].GetByteSize();
                }
            }

            return totalByteSize;
        }

        public void PrintConsole()
        {
            Console.WriteLine("||||||||||| Meta Header |||||||||||");
            Console.WriteLine("Meta Stream Keyword = {0}", mMetaStreamVersion);
            Console.WriteLine("Meta mClassNamesLength = {0}", mClassNamesLength);

            for (int i = 0; i < mUnhashedClassNames.Length; i++)
            {
                Console.WriteLine("Meta mClassName {0} = {1}", i, mUnhashedClassNames[i]);
            }
        }

        public string GetMBINInfo()
        {
            string metaInfo = "||||||||||| Meta Header |||||||||||" + Environment.NewLine;

            metaInfo += "Meta Stream Keyword = " + mMetaStreamVersion + Environment.NewLine;
            metaInfo += "Meta mClassNamesLength = " + mClassNamesLength + Environment.NewLine;

            if (isHashed)
            {
                for (int i = 0; i < mClassNames.Length; i++)
                {
                    metaInfo += "Meta mClassName " + i + " = " + mClassNames[i] + Environment.NewLine;
                }
            }
            else
            {
                for (int i = 0; i < mUnhashedClassNames.Length; i++)
                {
                    metaInfo += "Meta mClassName " + i + " = " + mUnhashedClassNames[i] + Environment.NewLine;
                }
            }

            return metaInfo;
        }
    }
}
