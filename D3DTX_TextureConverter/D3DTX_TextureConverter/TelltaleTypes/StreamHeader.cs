using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace D3DTX_TextureConverter.TelltaleTypes
{
    public struct StreamHeader
    {
        public int mRegionCount;
        public int mAuxDataCount;
        public int mTotalDataSize;

        public StreamHeader(BinaryReader reader)
        {
            mRegionCount = reader.ReadInt32(); //[4 bytes]
            mAuxDataCount = reader.ReadInt32(); //[4 bytes]
            mTotalDataSize = reader.ReadInt32(); //[4 bytes]
        }

        public void WriteBinaryData(BinaryWriter writer)
        {
            writer.Write(mRegionCount); //mRegionCount [4 bytes]
            writer.Write(mAuxDataCount); //mAuxDataCount [4 bytes]
            writer.Write(mTotalDataSize); //mTotalDataSize [4 bytes]
        }

        public uint GetByteSize()
        {
            uint totalByteSize = 0;

            totalByteSize += (uint)Marshal.SizeOf(mRegionCount); //mRegionCount [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mAuxDataCount); //mAuxDataCount [4 bytes]
            totalByteSize += (uint)Marshal.SizeOf(mTotalDataSize); //mTotalDataSize [4 bytes]

            return totalByteSize;
        }

        public override string ToString() => string.Format("[StreamHeader] mRegionCount: {0} mAuxDataCount: {1} mTotalDataSize: {2}", mRegionCount, mAuxDataCount, mTotalDataSize);
    }
}
