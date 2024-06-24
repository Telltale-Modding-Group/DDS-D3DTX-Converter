using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using Hexa.NET.DirectXTex;

/*
 * NOTE:
 * 
 * This version of D3DTX is COMPLETE. 
 * 
 * COMPLETE meaning that all of the data is known and getting identified.
 * Just like the versions before and after, this D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Legacy Version 2 games
 * Puzzle Agent 2 (TESTED)
 * Jurassic Park: The Game (TESTED)
 * Law & Order: Legacies (TESTED)
*/

namespace D3DTX_Converter.TelltaleD3DTX;

/// <summary>
/// This is a custom class that matches what is serialized in a legacy D3DTX version supporting the listed titles. (COMPLETE)
/// </summary>
public class D3DTX_LV2
{
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
    /// [1 byte] Indicates whether or not the texture contains mips. (what? need further research)
    /// </summary>
    public TelltaleBoolean mbHasTextureData { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips.
    /// </summary>
    public TelltaleBoolean mbIsMipMapped { get; set; }

    /// <summary>
    /// [4 bytes] Number of mips in the texture.
    /// </summary>
    public uint mNumMipLevels { get; set; }

    /// <summary>
    /// [4 bytes] The old T3SurfaceFormat. Makes use of FourCC but it can be an integer as well. Enums could not be found.
    /// </summary>
    public D3DFORMAT mD3DFormat { get; set; }

    /// <summary>
    /// [4 bytes] The pixel width of the texture.
    /// </summary>
    public uint mWidth { get; set; }

    /// <summary>
    /// [4 bytes] The pixel height of the texture.
    /// </summary>
    public uint mHeight { get; set; }

    /// <summary>
    /// [4 bytes] The pixel height of the texture.
    /// </summary>
    public int mFlags { get; set; }

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
    public TelltaleBoolean mbWiiForceUncompressed { get; set; }

    /// <summary>
    /// [4 bytes] The type of the texture. No enums were found, need more analyzing. Could be texture layout too.
    /// </summary>
    public uint mType { get; set; } //mTextureDataFormats?

    /// <summary>
    /// [4 bytes] The texture data format. No enums were found, need more analyzing. Could be a flag. OH NO IT'S NOT, IT COULD BE EARLY AUX
    /// </summary>
    public uint mTextureDataFormats { get; set; }

    /// <summary>
    /// [4 bytes] The texture data size (tpl?). 
    /// </summary>
    public uint mTplTextureDataSize { get; set; }

    /// <summary>
    /// [4 bytes] The alpha size of the texture? No idea why this exists.
    /// </summary>
    public uint mTplAlphaDataSize { get; set; }

    /// <summary>
    /// [4 bytes] The JPEG texture data size? (There were some screenshots of the game in the ttarch archives)
    /// </summary>
    public uint mJPEGTextureDataSize { get; set; }

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
    public TelltaleBoolean mbAlphaHDR { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture encrypted.
    /// </summary>
    public TelltaleBoolean mbEncrypted { get; set; }

    /// <summary>
    /// [4 bytes] Map brightness for the Detail map type.
    /// </summary>
    public float mDetailMapBrightness { get; set; }

    /// <summary>
    /// [4 bytes] Normal map related stuff. 
    /// </summary>
    public int mNormalMapFmt { get; set; }

    /// <summary>
    /// [8 bytes] A vector, defines the UV offset values when the shader on a material samples the texture.
    /// </summary>
    public Vector2 mUVOffset { get; set; }

    /// <summary>
    /// [8 bytes] A vector, defines the UV scale values when the shader on a material samples the texture.
    /// </summary>
    public Vector2 mUVScale { get; set; }

    /// <summary>
    /// [4 bytes] The size of the texture data.
    /// </summary>
    public int mTextureDataSize { get; set; }

    /// <summary>
    /// A byte array of the pixel regions in a texture. 
    /// </summary>
    public List<byte[]> mPixelData { get; set; }

    public D3DTX_LV2() { }

    /// <summary>
    /// Deserializes a D3DTX Object from a byte array.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bytePointerPosition"></param>
    public D3DTX_LV2(BinaryReader reader, bool showConsole = true)
    {
        bool read = true;

        while (read)
        {
            mSamplerState_BlockSize = reader.ReadInt32();
            mSamplerState = new T3SamplerStateBlock() //mSamplerState [4 bytes]
            {
                mData = reader.ReadUInt32()
            };

            mName_BlockSize = reader.ReadInt32();
            mName = ByteFunctions.ReadString(reader);
            mImportName_BlockSize = reader.ReadInt32();
            mImportName = ByteFunctions.ReadString(reader);

            mToolProps = new ToolProps(reader);
            mbHasTextureData = new TelltaleBoolean(reader);
            mbIsMipMapped = new TelltaleBoolean(reader);
            mNumMipLevels = reader.ReadUInt32();
            mD3DFormat = (D3DFORMAT)reader.ReadUInt32();
            mWidth = reader.ReadUInt32();
            mHeight = reader.ReadUInt32();
            mFlags = reader.ReadInt32();
            mWiiForceWidth = reader.ReadUInt32();
            mWiiForceHeight = reader.ReadUInt32();
            mbWiiForceUncompressed = new TelltaleBoolean(reader);
            mType = reader.ReadUInt32(); //???
            mTextureDataFormats = reader.ReadUInt32();
            mTplTextureDataSize = reader.ReadUInt32();
            mTplAlphaDataSize = reader.ReadUInt32();
            mJPEGTextureDataSize = reader.ReadUInt32();
            mAlphaMode = (eTxAlpha)reader.ReadInt32();
            mExactAlphaMode = (eTxAlpha)reader.ReadInt32();
            mColorMode = (eTxColor)reader.ReadUInt32();
            mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
            mbAlphaHDR = new TelltaleBoolean(reader);
            mbEncrypted = new TelltaleBoolean(reader);
            mDetailMapBrightness = reader.ReadSingle();
            mNormalMapFmt = reader.ReadInt32();

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

            if (!mbHasTextureData.mbTelltaleBoolean)
            {
                Console.WriteLine("NO TEXTURE DATA!");
                break;
            }

            mTextureDataSize = reader.ReadInt32();

            if (mTextureDataSize == 0)
            {
                continue;
            }

            if (reader.ReadUInt32() != ByteFunctions.Convert_String_To_UInt32(DDS.MAGIC_WORD))
            {
                PrintConsole();
                throw new Exception("Invalid DDS Header! The texture's header is corrupted!");
            }
            
            mPixelData = [];

            byte[] pixelArray = new byte[mTextureDataSize];
            for (int i = 0; i < mTextureDataSize; i++)
            {
                pixelArray[i] = reader.ReadByte();
            }
            mPixelData.Add(pixelArray);
            break;
        }

        if (showConsole)
            PrintConsole();
    }

    public void ModifyD3DTX(TexMetadata metadata, byte[] ddsData)
    {
        mWidth = (uint)metadata.Width;
        mHeight = (uint)metadata.Height;
        mD3DFormat = DDS_HELPER.GetD3DFORMATFromDXGIFormat((DXGIFormat)metadata.Format, metadata);
        mNumMipLevels = (uint)metadata.MipLevels;
        mbHasTextureData = new TelltaleBoolean(true);
        mbIsMipMapped = new TelltaleBoolean(metadata.MipLevels > 1);

        mTextureDataSize = ddsData.Length;
        mPixelData.Clear();
        mPixelData.Add(ddsData);

        PrintConsole();
    }

    public void WriteBinaryData(BinaryWriter writer)
    {
        writer.Write(mSamplerState_BlockSize); //mSamplerState Block Size [4 bytes]
        writer.Write(mSamplerState.mData); //mSamplerState mData [4 bytes] 
        writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
        ByteFunctions.WriteString(writer, mName); //mName [x bytes]
        writer.Write(mImportName_BlockSize); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
        ByteFunctions.WriteString(writer, mImportName); //mImportName [x bytes] (this is always 0)
        ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps); //mToolProps mbHasProps [1 byte]
        ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean); //mbHasTextureData [1 byte]
        ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean); //mbIsMipMapped [1 byte]

        writer.Write(mNumMipLevels); //mNumMipLevels [4 bytes]
        writer.Write((int)mD3DFormat); //mD3DFormat [4 bytes]
        writer.Write(mWidth); //mWidth [4 bytes]
        writer.Write(mHeight); //mHeight [4 bytes]
        writer.Write(mFlags); //mFlags [4 bytes]
        writer.Write(mWiiForceWidth); //mWiiForceWidth [4 bytes]
        writer.Write(mWiiForceHeight); //mWiiForceHeight [4 bytes]
        ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean); //mbWiiForceUncompressed [1 byte]
        writer.Write(mType); //mTextureDataFormats [4 bytes]
        writer.Write(mTextureDataFormats); //mTextureDataFormats [4 bytes]
        writer.Write(mTplTextureDataSize); //mTplTextureDataSize [4 bytes]
        writer.Write(mTplAlphaDataSize); //mTplAlphaDataSize [4 bytes]
        writer.Write(mJPEGTextureDataSize); //mJPEGTextureDataSize [4 bytes]
        writer.Write((int)mAlphaMode); //mAlphaMode [4 bytes]
        writer.Write((int)mExactAlphaMode); //mExactAlphaMode [4 bytes]
        writer.Write((int)mColorMode); //mColorMode [4 bytes]
        writer.Write((int)mWiiTextureFormat); //mWiiTextureFormat [4 bytes]
        ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean); //mbAlphaHDR [1 byte]
        ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean); //mbEncrypted [1 byte]
        writer.Write(mDetailMapBrightness); //mDetailMapBrightness [4 bytes]
        writer.Write(mNormalMapFmt); //mNormalMapFmt [4 bytes]
        writer.Write(mUVOffset.x); //mUVOffset X [4 bytes]
        writer.Write(mUVOffset.y); //mUVOffset Y [4 bytes]
        writer.Write(mUVScale.x); //mUVScale X [4 bytes]
        writer.Write(mUVScale.y); //mUVScale Y [4 bytes]
        writer.Write(mTextureDataSize); //mTextureDataSize [4 bytes]

        for (int i = 0; i < mTextureDataSize; i++) //DDS file including header [mTextureDataSize bytes]
        {
            writer.Write(mPixelData[0][i]);
        }
    }

    public uint GetHeaderByteSize()
    {
        uint totalSize = 0;

        totalSize += 4; //mSamplerState Block Size [4 bytes]
        totalSize += 4; //mSamplerState mData [4 bytes]
        totalSize += 4; //mName Block Size [4 bytes]
        totalSize += 4; //mNameSize [4 bytes]
        totalSize += (uint)mName.Length; //mName [x bytes]
        totalSize += 4; //mImportName Block Size [4 bytes]
        totalSize += 4; //mImportNameSize [4 bytes]
        totalSize += (uint)mImportName.Length; //mImportName [x bytes]
        totalSize += 1; //mToolProps mbHasProps [1 byte]
        totalSize += 1; //mbHasTextureData [1 byte]
        totalSize += 1; //mbIsMipMapped [1 byte]
        totalSize += 1; //mbEmbedMipMaps [1 byte]
        totalSize += 4; //mNumMipLevels [4 bytes]
        totalSize += 4; //mD3DFormat [4 bytes]
        totalSize += 4; //mWidth [4 bytes]
        totalSize += 4; //mHeight [4 bytes]
        totalSize += 4; //mWiiForceWidth [4 bytes]
        totalSize += 4; //mWiiForceHeight [4 bytes]
        totalSize += 1; //mbWiiForceUncompressed [1 byte]
        totalSize += 4; //mType [4 bytes]
        totalSize += 4; //mTextureDataFormats [4 bytes]
        totalSize += 4; //mTplTextureDataSize [4 bytes]
        totalSize += 4; //mTplAlphaDataSize [4 bytes]
        totalSize += 4; //mJPEGTextureDataSize [4 bytes]
        totalSize += 4; //mAlphaMode [4 bytes]
        totalSize += 4; //mWiiTextureFormat [4 bytes]
        totalSize += 1; //mbAlphaHDR [1 byte]
        totalSize += 1; //mbEncrypted [1 byte]
        totalSize += 4; //mDetailMapBrightness [4 bytes]
        totalSize += 4; //mNormalMapFmt [4 bytes]
        totalSize += 4; //mTextureDataSize [4 bytes]

        return totalSize;
    }

    public void PrintConsole()
    {
        Console.WriteLine(GetD3DTXInfo());
    }

    public string GetD3DTXInfo(MetaVersion metaVersion = MetaVersion.UNKNOWN)
    {
        string d3dtxInfo = "";

        d3dtxInfo += "||||||||||| D3DTX Legacy Version 2 Header |||||||||||" + Environment.NewLine;
        d3dtxInfo += "mSamplerState_BlockSize = " + mSamplerState_BlockSize + Environment.NewLine;
        d3dtxInfo += "mSamplerState = " + mSamplerState.ToString() + Environment.NewLine;
        d3dtxInfo += "mName_BlockSize = " + mName_BlockSize + Environment.NewLine;
        d3dtxInfo += "mName = " + mName + Environment.NewLine;
        d3dtxInfo += "mImportName_BlockSize = " + mImportName_BlockSize + Environment.NewLine;
        d3dtxInfo += "mImportName = " + mImportName + Environment.NewLine;
        d3dtxInfo += "mToolProps = " + mToolProps + Environment.NewLine;
        d3dtxInfo += "mbHasTextureData = " + mbHasTextureData + Environment.NewLine;
        d3dtxInfo += "mbIsMipMapped = " + mbIsMipMapped + Environment.NewLine;
        d3dtxInfo += "mNumMipLevels = " + mNumMipLevels + Environment.NewLine;
        d3dtxInfo += "mD3DFormat = " + mD3DFormat + Environment.NewLine;
        d3dtxInfo += "mWidth = " + mWidth + Environment.NewLine;
        d3dtxInfo += "mHeight = " + mHeight + Environment.NewLine;
        d3dtxInfo += "mFlags = " + mFlags + Environment.NewLine;
        d3dtxInfo += "mWiiForceWidth = " + mWiiForceWidth + Environment.NewLine;
        d3dtxInfo += "mWiiForceHeight = " + mWiiForceHeight + Environment.NewLine;
        d3dtxInfo += "mbWiiForceUncompressed = " + mbWiiForceUncompressed + Environment.NewLine;
        d3dtxInfo += "mType = " + mType + Environment.NewLine;
        d3dtxInfo += "mTextureDataFormats = " + mTextureDataFormats + Environment.NewLine;
        d3dtxInfo += "mTplTextureDataSize = " + mTplTextureDataSize + Environment.NewLine;
        d3dtxInfo += "mTplAlphaDataSize = " + mTplAlphaDataSize + Environment.NewLine;
        d3dtxInfo += "mJPEGTextureDataSize = " + mJPEGTextureDataSize + Environment.NewLine;
        d3dtxInfo += "mAlphaMode = " + Enum.GetName(typeof(eTxAlpha), (int)mAlphaMode) + " (" + mAlphaMode + ")" + Environment.NewLine;
        d3dtxInfo += "mExactAlphaMode = " + Enum.GetName(typeof(eTxAlpha), (int)mExactAlphaMode) + " (" + mExactAlphaMode + ")" + Environment.NewLine;
        d3dtxInfo += "mColorMode = " + Enum.GetName(typeof(eTxColor), (int)mColorMode) + " (" + mColorMode + ")" + Environment.NewLine;
        d3dtxInfo += "mWiiTextureFormat = " + mWiiTextureFormat + Environment.NewLine;
        d3dtxInfo += "mbAlphaHDR = " + mbAlphaHDR + Environment.NewLine;
        d3dtxInfo += "mbEncrypted = " + mbEncrypted + Environment.NewLine;
        d3dtxInfo += "mDetailMapBrightness = " + mDetailMapBrightness + Environment.NewLine;
        d3dtxInfo += "mNormalMapFmt = " + mNormalMapFmt + Environment.NewLine;
        d3dtxInfo += "mUVOffset = " + mUVOffset + Environment.NewLine;
        d3dtxInfo += "mUVScale = " + mUVScale + Environment.NewLine;
        d3dtxInfo += "mTextureDataSize = " + mTextureDataSize + Environment.NewLine;

        if (mbHasTextureData.mbTelltaleBoolean)
        {
            d3dtxInfo += "mPixelData Count = " + mPixelData[0].Length + Environment.NewLine;
        }

        d3dtxInfo += "|||||||||||||||||||||||||||||||||||||||";

        return d3dtxInfo;
    }
}
