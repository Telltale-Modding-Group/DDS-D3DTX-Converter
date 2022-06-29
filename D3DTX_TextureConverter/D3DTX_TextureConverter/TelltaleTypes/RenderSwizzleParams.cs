using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_TextureConverter.TelltaleTypes
{
    public struct RenderSwizzleParams
    {
        public char mSwizzle1;
        public char mSwizzle2;
        public char mSwizzle3;
        public char mSwizzle4;

        public RenderSwizzleParams(BinaryReader reader)
        {
            mSwizzle1 = (char)reader.ReadByte();
            mSwizzle2 = (char)reader.ReadByte();
            mSwizzle3 = (char)reader.ReadByte();
            mSwizzle4 = (char)reader.ReadByte();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(mSwizzle1); //mSwizzle A [1 byte]
            writer.Write(mSwizzle1); //mSwizzle B [1 byte]
            writer.Write(mSwizzle1); //mSwizzle C [1 byte]
            writer.Write(mSwizzle1); //mSwizzle D [1 byte]
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
