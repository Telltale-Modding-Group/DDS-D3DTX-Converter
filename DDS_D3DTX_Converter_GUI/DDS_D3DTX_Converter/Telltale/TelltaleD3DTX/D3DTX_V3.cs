using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using DirectXTexNet;

/*
 * NOTE:
 * 
 * This version of D3DTX is COMPLETE. 
 * 
 * COMPLETE meaning that all of the data is known and getting identified.
 * Just like the versions before and after, this D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* --- D3DTX Version 3 games ---
 * Poker Knight 2 (TESTED)
*/

namespace D3DTX_Converter.TelltaleD3DTX;

/// <summary>
/// This is a custom class that matches what is serialized in a D3DTX version 3 class. (COMPLETE)
/// </summary>
public class D3DTX_V3
{
  /// <summary>
  /// [4 bytes] The header version of this class.
  /// </summary>
  public int mVersion { get; set; }

  /// <summary>
  /// [4 bytes] The mSamplerState state block size in bytes. Note: the parsed value is always 8.
  /// </summary>
  public int mSamplerState_BlockSize { get; set; }

  /// <summary>
  /// [4 bytes] The sampler state, bitflag value that contains values from T3SamplerStateValue.
  /// </summary>
  public T3SamplerStateBlock mSamplerState { get; set; }

  /// <summary>
  /// [4 bytes] The mPlatformType state block size in bytes. Note: the parsed value is always 8.
  /// </summary>
  public uint mPlatformType_BlockSize { get; set; }

  /// <summary>
  /// [4 bytes] The platform type, an enum that defines the platform the texture is used on.
  /// </summary>
  public PlatformType mPlatformType { get; set; }

  /// <summary>
  /// [4 bytes] The mName block size in bytes.
  /// </summary>
  public int mName_BlockSize { get; set; }

  /// <summary>
  /// [mName_StringLength bytes] The string mName.
  /// </summary>
  public string mName { get; set; }

  /// <summary>
  /// [4 bytes] The mImportName block size in bytes.
  /// </summary>
  public int mImportName_BlockSize { get; set; }

  /// <summary>
  /// [mImportName_StringLength bytes] The mImportName string.
  /// </summary>
  public string mImportName { get; set; }

  /// <summary>
  /// [4 bytes] The import scale of the texture.
  /// </summary>
  public float mImportScale { get; set; }

  /// <summary>
  /// [1 byte] Whether or not the d3dtx contains a Tool Properties. [PropertySet] (Always false)
  /// </summary>
  public ToolProps mToolProps { get; set; }

  /// <summary>
  /// [4 bytes] Number of mips in the texture.
  /// </summary>
  public int mNumMipLevels { get; set; }

  /// <summary>
  /// [4 bytes] The pixel width of the texture.
  /// </summary>
  public int mWidth { get; set; }

  /// <summary>
  /// [4 bytes] The pixel height of the texture.
  /// </summary>
  public int mHeight { get; set; }

  /// <summary>
  /// [4 bytes] An enum, defines the compression used for the texture.
  /// </summary>
  public T3SurfaceFormat mSurfaceFormat { get; set; }

  /// <summary>
  /// [4 bytes] An enum, defines the resource type of the texture.
  /// </summary>
  public T3ResourceUsage mResourceUsage { get; set; }

  /// <summary>
  /// [4 bytes] An enum, defines what kind of texture it is.
  /// </summary>
  public T3TextureType mType { get; set; }

  /// <summary>
  /// [4 bytes] Defines the format of the normal map.
  /// </summary>
  public int mNormalMapFormat { get; set; }

  /// <summary>
  /// [4 bytes] Defines the brightness scale of the texture. (used for lightmaps)
  /// </summary>
  public float mHDRLightmapScale { get; set; }

  /// <summary>
  /// [4 bytes] An enum, defines what kind of alpha the texture will have.
  /// </summary>
  public eTxAlpha mAlphaMode { get; set; }

  /// <summary>
  /// [4 bytes] An enum, defines the color range of the texture.
  /// </summary>
  public eTxColor mColorMode { get; set; }

  /// <summary>
  /// [8 bytes] A vector, defines the UV offset values when the shader on a material samples the texture.
  /// </summary>
  public Vector2 mUVOffset { get; set; }

  /// <summary>
  /// [8 bytes] A vector, defines the UV scale values when the shader on a material samples the texture.
  /// </summary>
  public Vector2 mUVScale { get; set; }

  /// <summary>
  /// [4 bytes] The mToonRegions block size in bytes. Note: the parsed value is always 8. Or it could be a bitflag, needs further testing
  /// </summary>
  public uint mToonRegions_BlockSize { get; set; }

  /// <summary>
  /// [4 bytes] The size in bytes of the mToonRegions block.
  /// </summary>
  public uint mToonRegions_ArrayCapacity { get; set; }

  /// <summary>
  /// [4 bytes] The amount of elements in the mToonRegions array.
  /// </summary>
  public int mToonRegions_ArrayLength { get; set; }

  /// <summary>
  /// [16 bytes for each element] An array containing a toon gradient region.
  /// </summary>
  public T3ToonGradientRegion[] mToonRegions { get; set; }

  /// <summary>
  /// [12 bytes] A struct for StreamHeader
  /// </summary>
  public StreamHeader mStreamHeader { get; set; }

  /// <summary>
  /// [16 bytes for each element] An array containing each pixel region in the texture.
  /// </summary>
  public RegionStreamHeader[] mRegionHeaders { get; set; }

  /// <summary>
  /// A byte array of the pixel regions in a texture. Starts from smallest mip map to largest mip map. (Since this is a pure dds, this statement could be wrong)
  /// </summary>
  public List<byte[]> mPixelData { get; set; }

  /// <summary>
  /// D3DTX V3 Header (empty constructor, only used for json deserialization)
  /// </summary>
  /// 
  public D3DTX_V3(BinaryReader reader, bool showConsole = false)
  {
    mVersion = reader.ReadInt32(); //mVersion [4 bytes]
    mSamplerState_BlockSize = reader.ReadInt32(); //mSamplerState Block Size [4 bytes]
    mSamplerState = new T3SamplerStateBlock() //mSamplerState [4 bytes]
    {
      mData = reader.ReadUInt32()
    };
    mPlatformType_BlockSize = reader.ReadUInt32(); //mPlatform Block Size [4 bytes]
    mPlatformType = (PlatformType)reader.ReadInt32(); //mPlatform [4 bytes]
    mName_BlockSize = reader.ReadInt32(); //mName Block Size [4 bytes] //mName block size (size + string len)
    mName = ByteFunctions.ReadString(reader); //mName [x bytes]
    mImportName_BlockSize = reader.ReadInt32(); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
    mImportName = ByteFunctions.ReadString(reader); //mImportName [x bytes] (this is always 0)
    mImportScale = reader.ReadSingle(); //mImportScale [4 bytes]
    mToolProps = new ToolProps(reader); //mToolProps [1 byte]
    mNumMipLevels = reader.ReadInt32(); //mNumMipLevels [4 bytes]
    mWidth = reader.ReadInt32(); //mWidth [4 bytes]
    mHeight = reader.ReadInt32(); //mHeight [4 bytes]
    mSurfaceFormat = (T3SurfaceFormat)reader.ReadInt32(); //mSurfaceFormat [4 bytes]
    mResourceUsage = (T3ResourceUsage)reader.ReadInt32(); //mResourceUsage [4 bytes]
    mType = (T3TextureType)reader.ReadInt32(); //mType [4 bytes]
    mNormalMapFormat = reader.ReadInt32(); //mNormalMapFormat [4 bytes]
    mHDRLightmapScale = reader.ReadSingle(); //mHDRLightmapScale [4 bytes]
    mAlphaMode = (eTxAlpha)reader.ReadInt32(); //mAlphaMode [4 bytes]
    mColorMode = (eTxColor)reader.ReadInt32(); //mColorMode [4 bytes]

    mUVOffset = new Vector2() //mUVOffset [8 bytes]
    {
      x = reader.ReadSingle(), //[4 bytes]
      y = reader.ReadSingle() //[4 bytes]
    };
    mUVScale = new Vector2() //mUVScale [8 bytes]
    {
      x = reader.ReadSingle(), //[4 bytes]
      y = reader.ReadSingle() //[4 bytes]
    };

    //--------------------------mToonRegions--------------------------
    mToonRegions_ArrayCapacity = reader.ReadUInt32(); //mToonRegions DCArray Capacity [4 bytes]
    mToonRegions_ArrayLength = reader.ReadInt32(); //mToonRegions DCArray Length [4 bytes]
    mToonRegions = new T3ToonGradientRegion[mToonRegions_ArrayLength];

    for (int i = 0; i < mToonRegions_ArrayLength; i++)
    {
      mToonRegions[i] = new T3ToonGradientRegion()
      {
        mColor = new Color()
        {
          r = reader.ReadSingle(), //[4 bytes]
          g = reader.ReadSingle(), //[4 bytes]
          b = reader.ReadSingle(), //[4 bytes]
          a = reader.ReadSingle() //[4 bytes]
        },

        mSize = reader.ReadSingle() //[4 bytes]
      };
    }

    //--------------------------StreamHeader----------------------------
    mStreamHeader = new StreamHeader(reader); //[12 bytes]

    //--------------------------mRegionHeaders--------------------------
    mRegionHeaders = new RegionStreamHeader[mStreamHeader.mRegionCount];
    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      mRegionHeaders[i] = new RegionStreamHeader()
      {
        mMipIndex = reader.ReadInt32(), //[4 bytes] 
        mDataSize = reader.ReadUInt32(), //[4 bytes]
        mPitch = reader.ReadInt32(), //[4 bytes]
      };
    }
    //-----------------------------------------D3DTX HEADER END-----------------------------------------
    //--------------------------STORING D3DTX IMAGE DATA--------------------------
    mPixelData = [];

    // Skip the AUX data (WTF is this i have no idea)
    if (mStreamHeader.mAuxDataCount > 0)
    {
      uint size = reader.ReadUInt32();
      reader.ReadUInt32();
      reader.ReadUInt32();
      reader.ReadUInt32();
      reader.ReadUInt32();
      reader.ReadUInt32();
      List<byte> array = new List<byte>();
      for (int i = 0; i < size - 4 - 20; i++)
      {
        array.Add(reader.ReadByte());
      }
      Console.WriteLine("AUX DATA: " + BitConverter.ToString(array.ToArray()));
    }

    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      int dataSize = (int)mRegionHeaders[i].mDataSize;
      byte[] imageData = reader.ReadBytes(dataSize);

      mPixelData.Add(imageData);
    }

    if (showConsole)
      PrintConsole();
  }

  public void ModifyD3DTX(TexMetadata metadata, DDS_DirectXTexNet_ImageSection[] sections)
  {
    mWidth = metadata.Width;
    mHeight = metadata.Height;
    mSurfaceFormat = DDS_HELPER.GetTelltaleSurfaceFormatFromDXGI(metadata.Format, mSurfaceFormat);
    mNumMipLevels = metadata.MipLevels > 0 ? metadata.MipLevels : 1;

    mPixelData.Clear();
    mPixelData = DDS_DirectXTexNet.GetPixelDataFromSections(sections);

    mStreamHeader = new()
    {
      mRegionCount = sections.Length,
      mAuxDataCount = mStreamHeader.mAuxDataCount,
      mTotalDataSize = (int)ByteFunctions.Get2DByteArrayTotalSize(mPixelData)
    };

    mRegionHeaders = new RegionStreamHeader[mStreamHeader.mRegionCount];

    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      mRegionHeaders[i] = new()
      {
        mDataSize = (uint)mPixelData[i].Length,
        mPitch = (int)sections[i].RowPitch,
      };
    }

    if (metadata.IsCubemap())
    {
      throw new Exception("Cubemap textures are not supported on this version!");
    }
    else if (metadata.IsVolumemap())
    {
      throw new ArgumentException("Volumemap textures are not supported on this version!");
    }
    else
    {
      if (metadata.ArraySize > 1)
      {
        throw new ArgumentException("2D Array textures are not supported on this version!");
      }

      for (int i = 0; i < mStreamHeader.mRegionCount; i++)
      {
        mRegionHeaders[i].mMipIndex = i;
      }
    }

    UpdateArrayCapacities();
    PrintConsole();
  }

  public void UpdateArrayCapacities()
  {
    mToonRegions_ArrayCapacity = 8 + (uint)(20 * mToonRegions.Length);
    mToonRegions_ArrayLength = mToonRegions.Length;
  }

  public void WriteBinaryData(BinaryWriter writer)
  {
    writer.Write(mVersion); //mVersion [4 bytes]
    writer.Write(mSamplerState_BlockSize); //mSamplerState Block Size [4 bytes]
    writer.Write(mSamplerState.mData); //mSamplerState mData [4 bytes] 
    writer.Write(mPlatformType_BlockSize); //mPlatform Block Size [4 bytes]
    writer.Write((int)mPlatformType); //mPlatform [4 bytes]
    writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
    ByteFunctions.WriteString(writer, mName); //mName [x bytes]
    writer.Write(mImportName_BlockSize); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
    ByteFunctions.WriteString(writer, mImportName); //mImportName [x bytes] (this is always 0)
    writer.Write(mImportScale); //mImportScale [4 bytes]
    ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps); //mToolProps mbHasProps [1 byte]
    writer.Write(mNumMipLevels); //mNumMipLevels [4 bytes]
    writer.Write(mWidth); //mWidth [4 bytes]
    writer.Write(mHeight); //mHeight [4 bytes]
    writer.Write((int)mSurfaceFormat); //mSurfaceFormat [4 bytes]
    writer.Write((int)mResourceUsage); //mResourceUsage [4 bytes]
    writer.Write((int)mType); //mResourceUsage [4 bytes]
    writer.Write(mNormalMapFormat); //mNormalMapFormat [4 bytes]
    writer.Write(mHDRLightmapScale); //mHDRLightmapScale [4 bytes]
    writer.Write((int)mAlphaMode); //mAlphaMode [4 bytes]
    writer.Write((int)mColorMode); //mColorMode [4 bytes]
    writer.Write(mUVOffset.x); //mUVOffset X [4 bytes]
    writer.Write(mUVOffset.y); //mUVOffset Y [4 bytes]
    writer.Write(mUVScale.x); //mUVScale X [4 bytes]
    writer.Write(mUVScale.y); //mUVScale Y [4 bytes]

    writer.Write(mToonRegions_BlockSize); //mToonRegions DCArray Capacity [4 bytes]
    writer.Write(mToonRegions_ArrayCapacity); //mToonRegions DCArray Capacity [4 bytes]
    writer.Write(mToonRegions_ArrayLength); //mToonRegions DCArray Length [4 bytes]
    for (int i = 0; i < mToonRegions_ArrayLength; i++)
    {
      writer.Write(mToonRegions[i].mColor.r); //[4 bytes]
      writer.Write(mToonRegions[i].mColor.g); //[4 bytes]
      writer.Write(mToonRegions[i].mColor.b); //[4 bytes]
      writer.Write(mToonRegions[i].mColor.a); //[4 bytes]
      writer.Write(mToonRegions[i].mSize); //[4 bytes]
    }

    writer.Write(mStreamHeader.mRegionCount); //mRegionCount [4 bytes]
    writer.Write(mStreamHeader.mAuxDataCount); //mAuxDataCount [4 bytes]
    writer.Write(mStreamHeader.mTotalDataSize); //mTotalDataSize [4 bytes]

    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      writer.Write(mRegionHeaders[i].mMipIndex); //[4 bytes]
      writer.Write(mRegionHeaders[i].mDataSize); //[4 bytes]
      writer.Write(mRegionHeaders[i].mPitch); //[4 bytes]
    }

    for (int i = 0; i < mPixelData.Count; i++)
    {
      writer.Write(mPixelData[i]);
    }
  }

  public uint GetHeaderByteSize()
  {
    uint totalSize = 0;

    totalSize += 4; //mVersion [4 bytes]
    totalSize += 4; //mSamplerState Block Size [4 bytes]
    totalSize += 4; //mSamplerState mData [4 bytes] 
    totalSize += 4; //mPlatform Block Size [4 bytes]
    totalSize += 4; //mPlatform [4 bytes]
    totalSize += 4; //mName Block Size [4 bytes] //mName block size (size + string len)
    totalSize += 4; //mName (strength length prefix) [4 bytes]
    totalSize += (uint)mName.Length;  //mName [x bytes]
    totalSize += 4; //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
    totalSize += 4; //mImportName (strength length prefix) [4 bytes] (this is always 0)
    totalSize += (uint)mImportName.Length; //mImportName [x bytes] (this is always 0)
    totalSize += 4; //mImportScale [4 bytes]
    totalSize += 1; //mToolProps mbHasProps [1 byte]
    totalSize += 4; //mNumMipLevels [4 bytes]
    totalSize += 4; //mWidth [4 bytes]
    totalSize += 4; //mHeight [4 bytes]
    totalSize += 4; //mSurfaceFormat [4 bytes]
    totalSize += 4; //mResourceUsage [4 bytes]
    totalSize += 4; //mType [4 bytes]
    totalSize += 4; //mNormalMapFormat [4 bytes]
    totalSize += 4; //mHDRLightmapScale [4 bytes]
    totalSize += 4; //mAlphaMode [4 bytes]
    totalSize += 4; //mColorMode [4 bytes]
    totalSize += 4; //mUVOffset X [4 bytes]
    totalSize += 4; //mUVOffset Y [4 bytes]
    totalSize += 4; //mUVScale X [4 bytes]
    totalSize += 4; //mUVScale Y [4 bytes]

    totalSize += 4; //mToonRegions Block Size [4 bytes]
    totalSize += 4; //mToonRegions DCArray Capacity [4 bytes]
    totalSize += 4; //mToonRegions DCArray Length [4 bytes]
    for (int i = 0; i < mToonRegions_ArrayLength; i++)
    {
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
    }

    totalSize += 4; //mRegionCount [4 bytes]
    totalSize += 4; //mAuxDataCount [4 bytes]
    totalSize += 4; //mTotalDataSize [4 bytes]

    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
      totalSize += 4; //[4 bytes]
    }

    return totalSize;
  }

  public void PrintConsole()
  {
    Console.WriteLine(GetD3DTXInfo());
  }

  public string GetD3DTXInfo()
  {
    string d3dtxInfo = "";

    d3dtxInfo += "||||||||||| D3DTX Version 3 Header |||||||||||" + Environment.NewLine;
    d3dtxInfo += "mVersion = " + mVersion + Environment.NewLine;
    d3dtxInfo += "mSamplerState_BlockSize = " + mSamplerState_BlockSize + Environment.NewLine;
    d3dtxInfo += "mSamplerState = " + mSamplerState + Environment.NewLine;
    d3dtxInfo += "mPlatformType_BlockSize = " + mPlatformType_BlockSize + Environment.NewLine;
    d3dtxInfo += "mPlatformType = " + Enum.GetName(typeof(PlatformType), (int)mPlatformType) + " (" + mPlatformType + ")" + Environment.NewLine;
    d3dtxInfo += "mName_BlockSize = " + mName_BlockSize + Environment.NewLine;
    d3dtxInfo += "mName = " + mName + Environment.NewLine;
    d3dtxInfo += "mImportName_BlockSize = " + mImportName_BlockSize + Environment.NewLine;
    d3dtxInfo += "mImportName = " + mImportName + Environment.NewLine;
    d3dtxInfo += "mImportScale = " + mImportScale + Environment.NewLine;
    d3dtxInfo += "mToolProps = " + mToolProps + Environment.NewLine;
    d3dtxInfo += "mNumMipLevels = " + mNumMipLevels + Environment.NewLine;
    d3dtxInfo += "mWidth = " + mWidth + Environment.NewLine;
    d3dtxInfo += "mHeight = " + mHeight + Environment.NewLine;
    d3dtxInfo += "mSurfaceFormat = " + Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat) + " (" + (int)mSurfaceFormat + ")" + Environment.NewLine;
    d3dtxInfo += "mResourceUsage = " + Enum.GetName(typeof(T3ResourceUsage), mResourceUsage) + " (" + (int)mResourceUsage + ")" + Environment.NewLine;
    d3dtxInfo += "mType = " + Enum.GetName(typeof(T3TextureType), mType) + " (" + (int)mType + ")" + Environment.NewLine;
    d3dtxInfo += "mNormalMapFormat = " + mNormalMapFormat + Environment.NewLine;
    d3dtxInfo += "mHDRLightmapScale = " + mHDRLightmapScale + Environment.NewLine;
    d3dtxInfo += "mAlphaMode = " + Enum.GetName(typeof(eTxAlpha), (int)mAlphaMode) + " (" + mAlphaMode + ")" + Environment.NewLine;
    d3dtxInfo += "mColorMode = " + Enum.GetName(typeof(eTxColor), (int)mColorMode) + " (" + mColorMode + ")" + Environment.NewLine;
    d3dtxInfo += "mUVOffset = " + mUVOffset + Environment.NewLine;
    d3dtxInfo += "mUVScale = " + mUVScale + Environment.NewLine;

    d3dtxInfo += "----------- mToonRegions -----------" + Environment.NewLine;
    d3dtxInfo += "mToonRegions_BlockSize = " + mToonRegions_BlockSize + Environment.NewLine;
    d3dtxInfo += "mToonRegions_ArrayCapacity = " + mToonRegions_ArrayCapacity + Environment.NewLine;
    d3dtxInfo += "mToonRegions_ArrayLength = " + mToonRegions_ArrayLength + Environment.NewLine;
    for (int i = 0; i < mToonRegions_ArrayLength; i++)
    {
      d3dtxInfo += "mToonRegion " + i + " = " + mToonRegions[i] + Environment.NewLine;
    }

    d3dtxInfo += "----------- mStreamHeader -----------" + Environment.NewLine;
    d3dtxInfo += "mRegionCount = " + mStreamHeader.mRegionCount + Environment.NewLine;
    d3dtxInfo += "mAuxDataCount = " + mStreamHeader.mAuxDataCount + Environment.NewLine;
    d3dtxInfo += "mTotalDataSize = " + mStreamHeader.mTotalDataSize + Environment.NewLine;

    d3dtxInfo += "----------- mRegionHeaders -----------" + Environment.NewLine;
    for (int i = 0; i < mStreamHeader.mRegionCount; i++)
    {
      d3dtxInfo += "[mRegionHeader " + i + "]" + Environment.NewLine;
      d3dtxInfo += "mMipIndex = " + mRegionHeaders[i].mMipIndex + Environment.NewLine;
      d3dtxInfo += "mDataSize = " + mRegionHeaders[i].mDataSize + Environment.NewLine;
      d3dtxInfo += "mPitch = " + mRegionHeaders[i].mPitch + Environment.NewLine;
    }
    d3dtxInfo += "|||||||||||||||||||||||||||||||||||||||";

    return d3dtxInfo;
  }
}
