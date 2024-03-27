using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.TelltaleFunctions;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;

/*
 * NOTE:
 * 
 * This version of D3DTX is СХCOMPLETE. 
 * 
 * COMPLETE meaning that all of the data is known and getting identified.
 * Just like the versions before and after, this D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Version 3 games
 * The Walking Dead
*/

namespace D3DTX_Converter.TelltaleD3DTX
{
  /// <summary>
  /// This is a custom class that matches what is serialized in a D3DTX version 3? class. (INCOMPLETE)
  /// </summary>
  public class D3DTX_V3
  {
    //AFTER 64 bytes from the whole meta header

    /// <summary>
    /// [4 bytes] The mSamplerState state block size in bytes. Note: the parsed value is always 8.
    /// </summary>
    public int mSamplerState_BlockSize { get; set; }

    /// <summary>
    /// [4 bytes] The sampler state, bitflag value that contains values from T3SamplerStateValue.
    /// </summary>
    public T3SamplerStateBlock mSamplerState { get; set; }

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
    /// [1 byte] Whether or not the d3dtx contains a Tool Properties. [PropertySet] (Always false)
    /// </summary>
    public ToolProps mToolProps { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mipmaps. (what? need further research)
    /// </summary>
    public byte mbHasTextureData { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mipmaps.
    /// </summary>
    public byte mbIsMipMapped { get; set; }

    /// <summary>
    /// [4 bytes] Number of mipmaps in the texture.
    /// </summary>
    public uint mNumMipLevels { get; set; }

    /// <summary>
    /// [4 bytes] The old T3SurfaceFormat. Makes use of FourCC but it can be an integer as well. Enums could not be found.
    /// </summary>
    public uint mD3DFormat { get; set; }

    /// <summary>
    /// [4 bytes] The pixel width of the texture.
    /// </summary>
    public uint mWidth { get; set; }

    /// <summary>
    /// [4 bytes] The pixel height of the texture.
    /// </summary>
    public uint mHeight { get; set; }

    /// <summary>
    /// [4 bytes] Indicates the texture flags using bitwise OR operation. 0x1 is "Low quality", 0x2 is "Locked size" and 0x4 is "Generated mips".
    /// </summary>
    public uint mFlags { get; set; }

    /// <summary>
    /// [4 bytes] The pixel width of the texture when loaded on Wii platform.
    /// </summary>
    public uint mWiiForceWidth { get; set; }

    /// <summary>
    /// [4 bytes] The pixel height of the texture when loaded on Wii platform.
    /// </summary>
    public uint mWiiForceHeight { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture is forced to compressed when on.
    /// </summary>
    public byte mbWiiForceUncompressed { get; set; }

    /// <summary>
    /// [4 bytes] The type of the texture. No enums were found, need more anylizing. Could be texture layout too.
    /// </summary>
    public uint mType { get; set; }

    /// <summary>
    /// [4 bytes] The texture data format. No enums were found, need more anylizing. Could be a flag.
    /// </summary>
    public uint mTextureDataFormats { get; set; }

    /// <summary>
    /// [4 bytes] The texture data size (tpl?). 
    /// </summary>
    public uint mTplTexutreDataSize { get; set; }

    /// <summary>
    /// [4 bytes] The alpha size of the texture? No idea why this exists.
    /// </summary>
    public uint mTplAlphaDataSize { get; set; }

    /// <summary>
    /// [4 bytes] The JPEG texture data size? (There were some screenshots of the game in the ttarch archives)
    /// </summary>
    public uint mJPEGTextureDataSize { get; set; }

    /// <summary>
    /// [4 bytes] Defines the brightness scale of the texture. (used for lightmaps)
    /// </summary>
    public uint mHDRLightmapScale { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines what kind of alpha the texture will have.
    /// </summary>
    public eTxAlpha mAlphaMode { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines what kind of *exact* alpha the texture will have. (no idea why this exists, wtf Telltale)
    /// </summary>
    public eTxAlpha mExactAlphaMode { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the color range of the texture.
    /// </summary>
    public eTxColor mColorMode { get; set; }

    /// <summary>
    /// [4 bytes] The Wii texture format.
    /// </summary>
    public WiiTextureFormat mWiiTextureFormat { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture encrypted.
    /// </summary>
    public byte mbEncrypted { get; set; }

    /// <summary>
    /// [4 bytes] Map brightness for the Detail map type.
    /// </summary>
    public uint mDetailMapBrightness { get; set; }

    /// <summary>
    /// [4 bytes] Unknown flag. It's used with bitwise OR 1 (_DAT_00ab1d84 | 1)
    /// </summary>
    public uint unknownFlag { get; set; }

    /// <summary>
    /// [4 bytes] Normal map related stuff. 
    /// </summary>
    public uint mNormalMapFmt { get; set; }

    /// <summary>
    /// [8 bytes] A vector, defines the UV offset values when the shader on a material samples the texture.
    /// </summary>
    public Vector2 mUVOffset { get; set; }

    /// <summary>
    /// [8 bytes] A vector, defines the UV scale values when the shader on a material samples the texture.
    /// </summary>
    public Vector2 mUVScale { get; set; }

    /// <summary>
    /// [1 byte] Force Prev rebuild? Tool related stuff probably.
    /// </summary>
    public byte mbForcePreviewRebuild { get; set; }


    public List<byte[]> mPixelData { get; set; }

    /// <summary>
    /// D3DTX V3 Header (empty constructor, only used for json deserialization)
    /// </summary>
    /// 
    public D3DTX_V3(BinaryReader reader, bool showConsole = false)
    {
      mName_BlockSize = reader.ReadInt32();
      mName = ByteFunctions.ReadString(reader);
      mImportName_BlockSize = reader.ReadInt32();
      mImportName = ByteFunctions.ReadString(reader);
      mSamplerState_BlockSize = reader.ReadInt32();
      mSamplerState = new T3SamplerStateBlock() //mSamplerState [4 bytes]
      {
        mData = reader.ReadUInt32()
      };
      mToolProps = new ToolProps(reader);
      mbHasTextureData = reader.ReadByte();
      mbIsMipMapped = reader.ReadByte();
      mNumMipLevels = reader.ReadUInt32();
      mD3DFormat = reader.ReadUInt32();
      mWidth = reader.ReadUInt32();
      mHeight = reader.ReadUInt32();
      mFlags = reader.ReadUInt32();
      mWiiForceWidth = reader.ReadUInt32();
      mWiiForceHeight = reader.ReadUInt32();
      mbWiiForceUncompressed = reader.ReadByte();
      mType = reader.ReadUInt32();
      mTextureDataFormats = reader.ReadUInt32();
      mTplTexutreDataSize = reader.ReadUInt32();
      mTplAlphaDataSize = reader.ReadUInt32();
      mJPEGTextureDataSize = reader.ReadUInt32();
      mHDRLightmapScale = reader.ReadUInt32();
      mAlphaMode = (eTxAlpha)reader.ReadInt32();
      mExactAlphaMode = (eTxAlpha)reader.ReadInt32();
      mColorMode = (eTxColor)reader.ReadInt32();
      mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
      mbEncrypted = reader.ReadByte();
      mDetailMapBrightness = reader.ReadUInt32();
      unknownFlag = reader.ReadUInt32();
      mNormalMapFmt = reader.ReadUInt32();
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
      mbForcePreviewRebuild = reader.ReadByte();

      if (showConsole)
        PrintConsole();

    }

    public void PrintConsole()
    {
      Console.WriteLine("||||||||||| D3DTX_v3 Header |||||||||||");
      Console.WriteLine("D3DTX_v3 mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);
      Console.WriteLine("D3DTX_v3 mSamplerState = {0}", mSamplerState);
      Console.WriteLine("D3DTX_v3 mName_BlockSize = {0}", mName_BlockSize);
      Console.WriteLine("D3DTX_v3 mName = {0}", mName);
      Console.WriteLine("D3DTX_v3 mImportName_BlockSize = {0}", mImportName_BlockSize);
      Console.WriteLine("D3DTX_v3 mImportName = {0}", mImportName);
      Console.WriteLine("D3DTX_v3 mToolProps = {0}", mToolProps);
      Console.WriteLine("D3DTX_v3 mbHasTextureData = {0}", mbHasTextureData);
      Console.WriteLine("D3DTX_v3 mbIsMipMapped = {0}", mbIsMipMapped);
      Console.WriteLine("D3DTX_v3 mNumMipLevels = {0}", mNumMipLevels);
      Console.WriteLine("D3DTX_v3 mD3DFormat = {0}", mD3DFormat);
      Console.WriteLine("D3DTX_v3 mWidth = {0}", mWidth);
      Console.WriteLine("D3DTX_v3 mHeight = {0}", mHeight);
      Console.WriteLine("D3DTX_v3 mFlags = {0}", mFlags);
      Console.WriteLine("D3DTX_v3 mWiiForceWidth = {0}", mWiiForceWidth);
      Console.WriteLine("D3DTX_v3 mWiiForceHeight = {0}", mWiiForceHeight);
      Console.WriteLine("D3DTX_v3 mbWiiForceUncompressed = {0}", mbWiiForceUncompressed);
      Console.WriteLine("D3DTX_v3 mType = {0}", mType);
      Console.WriteLine("D3DTX_v3 mTextureDataFormats = {0}", mTextureDataFormats);
      Console.WriteLine("D3DTX_v3 mTplTexutreDataSize = {0}", mTplTexutreDataSize);
      Console.WriteLine("D3DTX_v3 mTplAlphaDataSize = {0}", mTplAlphaDataSize);
      Console.WriteLine("D3DTX_v3 mJPEGTextureDataSize = {0}", mJPEGTextureDataSize);
      Console.WriteLine("D3DTX_v3 mHDRLightmapScale = {0}", mHDRLightmapScale);
      Console.WriteLine("D3DTX_v3 mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), (int)mAlphaMode), mAlphaMode);
      Console.WriteLine("D3DTX_v3 mExactAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), (int)mExactAlphaMode), mExactAlphaMode);
      Console.WriteLine("D3DTX_v3 mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), (int)mColorMode), mColorMode);
      Console.WriteLine("D3DTX_v3 mWiiTextureFormat = {0}", mWiiTextureFormat);
      Console.WriteLine("D3DTX_v3 mbEncrypted = {0}", mbEncrypted);
      Console.WriteLine("D3DTX_v3 mDetailMapBrightness = {0}", mDetailMapBrightness);
      Console.WriteLine("D3DTX_v3 unknownFlags = {0}", unknownFlag);
      Console.WriteLine("D3DTX_v3 mNormalMapFmt = {0}", mNormalMapFmt);
      Console.WriteLine("D3DTX_v3 mUVOffset = {0}", mUVOffset);
      Console.WriteLine("D3DTX_v3 mUVScale = {0}", mUVScale);
      Console.WriteLine("D3DTX_v3 mbForcePreviewRebuild = {0}", mbForcePreviewRebuild);
    }
  }
}