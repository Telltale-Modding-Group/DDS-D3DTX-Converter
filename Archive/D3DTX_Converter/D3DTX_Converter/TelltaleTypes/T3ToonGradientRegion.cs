using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.TelltaleTypes
{
    public struct T3ToonGradientRegion
    {
        public Color mColor;
        public float mSize;

        public T3ToonGradientRegion(BinaryReader reader)
        {
            mColor = new Color(reader);
            mSize = reader.ReadUInt32();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            mColor.WriteBinaryData(writer);
            writer.Write(mSize); //[4 bytes]
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += mColor.GetByteSize(); //mColor
            totalByteSize += (uint)Marshal.SizeOf(mSize); //mSize [4 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[T3ToonGradientRegion] mColor: {0} mSize: {1}", mColor, mSize);
    }
}
