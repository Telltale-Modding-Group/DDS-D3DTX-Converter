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
        /// (Not actually in the header) Gets the length of the Meta Header in bytes.
        /// </summary>
        private uint MetaHeaderLength { get; set; }

        /// <summary>
        /// Parses the Meta Header from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytePointerPosition"></param>
        /// <param name="showConsole"></param>
        public MTRE(BinaryReader reader, bool showConsole = true)
        {
            mMetaStreamVersion += reader.ReadChars(4); //Meta Stream Keyword [4 bytes]
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
                Console.WriteLine("Meta mClassNamesLength = {0}", mClassNamesLength);

                for (int i = 0; i < mClassNames.Length; i++)
                {
                    Console.WriteLine("Meta mClassName {0} = {1}", i, mClassNames[i]);
                }
            }
        }

        /// <summary>
        /// Converts the data of this object into a byte array.
        /// </summary>
        /// <returns></returns>
        public void GetByteData(BinaryWriter writer)
        {
            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            writer.Write(mMetaStreamVersion[0]);
            writer.Write(mMetaStreamVersion[1]);
            writer.Write(mMetaStreamVersion[2]);
            writer.Write(mMetaStreamVersion[3]);

            writer.Write(mClassNamesLength); //mClassNamesLength [4 bytes]

            //--------------------------mClassNames--------------------------
            for (int i = 0; i < mClassNames.Length; i++)
            {
                //symbol
                writer.Write(mClassNames[i].mTypeNameCRC.mCrc64);

                //version crc
                writer.Write(mClassNames[i].mVersionCRC);
            }
        }

        public uint Get_MetaHeaderLength()
        {
            return MetaHeaderLength;
        }
    }
}
