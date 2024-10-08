using System.IO;
using System.Runtime.InteropServices;

namespace TelltaleTextureTool.TelltaleTypes;

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

    public readonly void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mRegionCount); //mRegionCount [4 bytes]
        writer.Write(mAuxDataCount); //mAuxDataCount [4 bytes]
        writer.Write(mTotalDataSize); //mTotalDataSize [4 bytes]
    }

    public readonly uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += (uint)Marshal.SizeOf(mRegionCount); //mRegionCount [4 bytes]
        totalByteSize += (uint)Marshal.SizeOf(mAuxDataCount); //mAuxDataCount [4 bytes]
        totalByteSize += (uint)Marshal.SizeOf(mTotalDataSize); //mTotalDataSize [4 bytes]

        return totalByteSize;
    }

    public override readonly string ToString() => string.Format("[StreamHeader] mRegionCount: {0} mAuxDataCount: {1} mTotalDataSize: {2}", mRegionCount, mAuxDataCount, mTotalDataSize);
}
