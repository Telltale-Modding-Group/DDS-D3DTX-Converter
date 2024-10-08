using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.TelltaleTypes
{
    /// <summary>
    /// This is a class name struct used in a meta header.
    /// This contains a CRC64'd string of a class name used in the file.
    /// The CRC64 string of a classname is usually all lowercase, and uses 
    /// </summary>
    public struct ClassNames
    {
        public Symbol mTypeNameCRC;
        public uint mVersionCRC;

        public ClassNames(BinaryReader reader)
        {
            mTypeNameCRC = new Symbol(reader);
            mVersionCRC = reader.ReadUInt32();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            mTypeNameCRC.WriteBinaryData(writer);
            writer.Write(mVersionCRC);
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += mTypeNameCRC.GetByteSize(); //mTypeNameCRC
            totalByteSize += (uint)Marshal.SizeOf(mVersionCRC); //mVersionCRC [4 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[ClassNames] mTypeNameCRC: ({0}) mVersionCRC: {1}", mTypeNameCRC, mVersionCRC);
    }
}
