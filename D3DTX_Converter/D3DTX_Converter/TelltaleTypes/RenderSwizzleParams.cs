using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.TelltaleTypes
{
    public struct RenderSwizzleParams
    {
        public byte mSwizzle1;
        public byte mSwizzle2;
        public byte mSwizzle3;
        public byte mSwizzle4;

        public RenderSwizzleParams(BinaryReader reader)
        {
            mSwizzle1 = reader.ReadByte();
            mSwizzle2 = reader.ReadByte();
            mSwizzle3 = reader.ReadByte();
            mSwizzle4 = reader.ReadByte();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            byte[] combinedData = new byte[4];
            combinedData[0] = mSwizzle1; //mSwizzle A [1 byte]
            combinedData[1] = mSwizzle2; //mSwizzle B [1 byte]
            combinedData[2] = mSwizzle3; //mSwizzle C [1 byte]
            combinedData[3] = mSwizzle4; //mSwizzle D [1 byte]

            writer.Write(combinedData);
            //note to self: writing this as a combined byte array because for some reason writing the bytes by themself the values don't appear to be the same in a hex editor.
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mSwizzle1); //mSwizzle1 [1 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mSwizzle2); //mSwizzle2 [1 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mSwizzle3); //mSwizzle3 [1 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mSwizzle4); //mSwizzle4 [1 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[RenderSwizzleParams]: {0} {1} {2} {3}", (int)mSwizzle1, (int)mSwizzle2, (int)mSwizzle3, (int)mSwizzle4);
    }
}
