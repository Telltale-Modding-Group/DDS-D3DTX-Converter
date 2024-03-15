using System.IO;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.TelltaleTypes
{
    public struct Symbol
    {
        public ulong mCrc64;

        public Symbol(BinaryReader reader)
        {
            mCrc64 = reader.ReadUInt64();
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(mCrc64);
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mCrc64); //mCrc64 [8 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[Symbol] mCrc64: {0}", mCrc64);
    }
}
