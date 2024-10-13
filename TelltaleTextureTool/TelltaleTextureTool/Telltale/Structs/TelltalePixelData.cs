using System;
using System.IO;
using TelltaleTextureTool.Utilities;

namespace TelltaleTextureTool.TelltaleTypes;

public struct TelltalePixelData
{
    public uint length;
    public byte[] pixelData;

    public TelltalePixelData(BinaryReader reader, bool IsEncrypted = false)
    {
        length = reader.ReadUInt32();

        if (IsEncrypted)
        {
            for (int i = 0; i < 16; i++)
            {
                reader.ReadUInt32();
            }
            length -= 128;
        }

        pixelData = reader.ReadBytes((int)length);
    }

    public TelltalePixelData(BinaryReader reader, int skip)
    {
        length = reader.ReadUInt32();
        reader.BaseStream.Position += skip;
        pixelData = reader.ReadBytes((int)length - skip);
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(length);
        writer.Write(pixelData);
    }

    public TelltalePixelData(byte[] ddsData, int skippedOriginalBytes = 0, int skippedDDSBytes = 0)
    {
        if (skippedOriginalBytes > ddsData.Length || skippedDDSBytes > ddsData.Length)
            throw new Exception("One of the parameters is larger than the data size.");

        byte[] copyBuffer = new byte[skippedOriginalBytes];

        Array.Copy(pixelData, 0, copyBuffer, 0, skippedOriginalBytes);

        byte[] copyDDSBuffer = new byte[ddsData.Length - skippedDDSBytes];

        Array.Copy(ddsData, skippedDDSBytes, copyDDSBuffer, 0, ddsData.Length - skippedDDSBytes);

        pixelData = ByteFunctions.Combine(copyBuffer, copyDDSBuffer);
        length = (uint)pixelData.Length;
    }

    public uint GetByteSize()
    {
        uint totalByteSize = 0;

        totalByteSize += sizeof(uint); // length [4 bytes]
        totalByteSize += (uint)pixelData.Length; // pixelData [n bytes]

        return totalByteSize;
    }

    public override string ToString() => string.Format("Pixel Data: {0} bytes", length);
}
