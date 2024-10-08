using System;
using System.IO;
using TelltaleTextureTool.TelltaleEnums;
using System.Runtime.InteropServices;

namespace TelltaleTextureTool.TelltaleTypes;

public struct T3SamplerStateBlock
{
  public uint mData;

  public T3SamplerStateBlock(BinaryReader reader)
  {
    mData = reader.ReadUInt32(); //mSamplerState [4 bytes]
  }

  public readonly uint GetByteSize()
  {
    uint totalByteSize = 0;

    totalByteSize += (uint)Marshal.SizeOf(mData); //mData [4 bytes]

    return totalByteSize;
  }

  public override readonly string ToString()
  {
    string enumFlags = "";

    var allEnums = Enum.GetValues(typeof(T3SamplerStateValue));

    foreach (var enumMask in allEnums)
    {
      if ((mData & (uint)(T3SamplerStateValue)enumMask) != 0)
        enumFlags += Enum.GetName((T3SamplerStateValue)enumMask).Remove(0, 14) + ": " + (mData & (uint)(T3SamplerStateValue)enumMask) + " | ";
    }

    enumFlags += "(" + (int)mData + ")";
    return string.Format("[T3SamplerStateBlock] mData: {0}", enumFlags);
  }
}

public struct T3SamplerStateBlock_SamplerStateEntry
{
  public int mShift;
  public int mMask;

  public override string ToString() => string.Format("[T3SamplerStateBlock::SamplerStateEntry] mShift: {0} mMask: {1}", mShift, mMask);
}

