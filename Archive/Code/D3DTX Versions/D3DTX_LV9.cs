using System;
using System.Collections.Generic;
using System.IO;
using TelltaleTextureTool.TelltaleEnums;
using TelltaleTextureTool.TelltaleTypes;
using TelltaleTextureTool.Utilities;
using TelltaleTextureTool.DirectX;
using TelltaleTextureTool.DirectX.Enums;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;
using System.Linq;

/*
 * NOTE:
 * 
 * This version of D3DTX is COMPLETE. 
 * 
 * COMPLETE meaning that all of the data is known and getting identified.
 * Just like the versions before and after, this D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Legacy Version 9 games
 * Telltale Texas Hold'em  (UNTESTED)
 * Sam & Max Beyond Time and Space (TESTED)
*/

namespace TelltaleTextureTool.TelltaleD3DTX;

/// <summary>
/// This is a custom class that matches what is serialized in a legacy D3DTX version supporting the listed titles. (COMPLETE)
/// </summary>
public class D3DTX_LV9 : ID3DTX
{
    /// <summary>
    /// [4 bytes] The mName block size in bytes.
    /// </summary>
    public int mName_BlockSize { get; set; }

    /// <summary>
    /// [mName_StringLength bytes] The string mName.
    /// </summary>
    public string mName { get; set; } = string.Empty;

    /// <summary>
    /// [4 bytes] The mImportName block size in bytes.
    /// </summary>
    public int mImportName_BlockSize { get; set; }

    /// <summary>
    /// [mImportName_StringLength bytes] The mImportName string.
    /// </summary>
    public string mImportName { get; set; } = string.Empty;

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips. (what? need further research)
    /// </summary>
    public TelltaleBoolean mbHasTextureData { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips.
    /// </summary>
    public TelltaleBoolean mbIsMipMapped { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips.
    /// </summary>
    public TelltaleBoolean mbIsWrapU { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips.
    /// </summary>
    public TelltaleBoolean mbIsWrapV { get; set; }

    /// <summary>
    /// [1 byte] Indicates whether or not the texture contains mips.
    /// </summary>
    public TelltaleBoolean mbIsFiltered { get; set; }

    /// <summary>
    /// [1 byte] No idea.
    /// </summary>
    public TelltaleBoolean mbEmbedMipMaps { get; set; }

    /// <summary>
    /// [4 bytes] Number of mips in the texture.
    /// </summary>
    public uint mNumMipLevels { get; set; }

    /// <summary>
    /// [4 bytes] The old T3SurfaceFormat. Makes use of FourCC but it can be an integer as well. Enums could not be found.
    /// </summary>
    public LegacyFormat mD3DFormat { get; set; }

    /// <summary>
    /// [4 bytes] The pixel width of the texture.
    /// </summary>
    public uint mWidth { get; set; }

    /// <summary>
    /// [4 bytes] The pixel height of the texture.
    /// </summary>
    public uint mHeight { get; set; }

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
    /// [4 bytes] The texture data format. No enums were found, need more analyzing. Could be a flag.
    /// </summary>
    public uint mTextureDataFormats { get; set; }

    /// <summary>
    /// [4 bytes] The texture data size (tpl?). 
    /// </summary>
    public uint mTplTextureDataSize { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines what kind of alpha the texture will have.
    /// </summary>
    public T3TextureAlphaMode mAlphaMode { get; set; }

    /// <summary>
    /// [4 bytes] The Wii texture format.
    /// </summary>
    public WiiTextureFormat mWiiTextureFormat { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture has alpha HDR?
    /// </summary>
    public TelltaleBoolean mbAlphaHDR { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture encrypted.
    /// </summary>
    public TelltaleBoolean mbEncrypted { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture has alpha HDR?
    /// </summary>
    public TelltaleBoolean mbUsedAsBumpmap { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the texture has alpha HDR?
    /// </summary>
    public TelltaleBoolean mbUsedAsDetailMap { get; set; }

    /// <summary>
    /// [4 bytes] Map brightness for the Detail map type.
    /// </summary>
    public float mDetailMapBrightness { get; set; }

    /// <summary>
    /// A byte array of the pixel regions in a texture. 
    /// </summary>
    public List<TelltalePixelData> mPixelData { get; set; } = [];

    public D3DTX_LV9() { }

    public void WriteToBinary(BinaryWriter writer, TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None,bool printDebug = false)
    {
        writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
        ByteFunctions.WriteString(writer, mName); //mName [x bytes]
        writer.Write(mImportName_BlockSize); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
        ByteFunctions.WriteString(writer, mImportName); //mImportName [x bytes] (this is always 0)
        ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean); //mbHasTextureData [1 byte]
        ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean); //mbIsMipMapped [1 byte]
        ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean); //mbEmbedMipMaps [1 byte]
        ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean); //mbEmbedMipMaps [1 byte]
        ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean); //mbEmbedMipMaps [1 byte]
        ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean); //mbEmbedMipMaps [1 byte]
        writer.Write(mNumMipLevels); //mNumMipLevels [4 bytes]
        writer.Write((int)mD3DFormat); //mD3DFormat [4 bytes]
        writer.Write(mWidth); //mWidth [4 bytes]
        writer.Write(mHeight); //mHeight [4 bytes]
        writer.Write(mWiiForceWidth); //mWiiForceWidth [4 bytes]
        writer.Write(mWiiForceHeight); //mWiiForceHeight [4 bytes]
        ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean); //mbWiiForceUncompressed [1 byte]
        writer.Write(mTextureDataFormats); //mTextureDataFormats [4 bytes]
        writer.Write(mTplTextureDataSize); //mTplTextureDataSize [4 bytes]
        writer.Write((int)mAlphaMode); //mAlphaMode [4 bytes]
        writer.Write((int)mWiiTextureFormat); //mWiiTextureFormat [4 bytes]
        ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean); //mbAlphaHDR [1 byte]
        ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean); //mbEncrypted [1 byte]
        ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean); //mbUsedAsBumpmap [1 byte]
        ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean); //mbUsedAsDetailMap [1 byte]
        writer.Write(mDetailMapBrightness); //mDetailMapBrightness [4 bytes]

        for (int i = 0; i < mPixelData.Count; i++) //DDS file including header [mTextureDataSize bytes]
        {
            mPixelData[i].WriteBinaryData(writer);
        }
    }

    public void ReadFromBinary(BinaryReader reader, TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None,bool printDebug = false)
    {
        bool read = true;
        bool isValid = true;

        while (read && isValid)
        {
            isValid = true;
            mName_BlockSize = reader.ReadInt32();
            mName = ByteFunctions.ReadString(reader);
            mImportName_BlockSize = reader.ReadInt32();
            mImportName = ByteFunctions.ReadString(reader);
            mbHasTextureData = new TelltaleBoolean(reader);
            mbIsMipMapped = new TelltaleBoolean(reader);
            mbIsWrapU = new TelltaleBoolean(reader);
            mbIsWrapV = new TelltaleBoolean(reader);
            mbIsFiltered = new TelltaleBoolean(reader);
            mbEmbedMipMaps = new TelltaleBoolean(reader);

            mNumMipLevels = reader.ReadUInt32();
            mD3DFormat = (LegacyFormat)reader.ReadUInt32();
            mWidth = reader.ReadUInt32();
            mHeight = reader.ReadUInt32();
            mWiiForceWidth = reader.ReadUInt32();
            mWiiForceHeight = reader.ReadUInt32();
            mbWiiForceUncompressed = new TelltaleBoolean(reader);
            mTextureDataFormats = reader.ReadUInt32();
            mTplTextureDataSize = reader.ReadUInt32();
            mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

            mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
            mbAlphaHDR = new TelltaleBoolean(reader);
            mbEncrypted = new TelltaleBoolean(reader);
            mbUsedAsBumpmap = new TelltaleBoolean(reader);
            mbUsedAsDetailMap = new TelltaleBoolean(reader);
            mDetailMapBrightness = reader.ReadSingle();

            if (!mbHasTextureData.mbTelltaleBoolean)
            {
                PrintConsole();
                throw new PixelDataNotFoundException("The texture does not have any pixel data!");
            }

            uint mTextureDataSize = reader.ReadUInt32();

            if (mTextureDataSize == 0 && mbHasTextureData.mbTelltaleBoolean)
            {
                continue;
            }

            if (mTextureDataSize > reader.BaseStream.Length - reader.BaseStream.Position || reader.BaseStream.Position == reader.BaseStream.Length)
            {
                PrintConsole();
                throw new Exception("Invalid DDS Header! The texture's header is corrupted!");
            }

            reader.BaseStream.Position -= 4;

            mPixelData = [];

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int magic = reader.ReadInt32();
                if (magic == 8 || magic == mName.Length + 8)
                {
                    isValid = false;
                    reader.BaseStream.Position -= 4;
                    break;
                }

                reader.BaseStream.Position -= 4;

                mNumMipLevels = mbEncrypted.mbTelltaleBoolean ? 1 : mNumMipLevels;
                int skip = mbEncrypted.mbTelltaleBoolean ? 128 : 0;

                TelltalePixelData telltalePixelData = new(reader, skip);
                mPixelData.Add(telltalePixelData);
            }

            read = false;
        }

        if (printDebug)
            PrintConsole();
    }

    public void ModifyD3DTX(D3DTXMetadata metadata, ImageSection[] imageSections, bool printDebug = false)
    {
        mWidth = metadata.Width;
        mHeight = metadata.Height;
        mD3DFormat = metadata.D3DFormat;
        mNumMipLevels = metadata.MipLevels;
        mbHasTextureData = new TelltaleBoolean(true);
        mbIsMipMapped = new TelltaleBoolean(metadata.MipLevels > 1);
        mbEmbedMipMaps = new TelltaleBoolean(metadata.MipLevels > 1);

        var textureData = DDS_DirectXTexNet.GetPixelDataArrayFromSections(imageSections);

        mPixelData.Clear();

        TelltalePixelData telltalePixelData = new TelltalePixelData()
        {
            length = (uint)textureData.Length,
            pixelData = textureData
        };

        mPixelData.Add(telltalePixelData);

        PrintConsole();
    }

    public D3DTXMetadata GetD3DTXMetadata()
    {
        D3DTXMetadata metadata = new D3DTXMetadata()
        {
            TextureName = mName,
            Width = mWidth,
            Height = mHeight,
            MipLevels = mNumMipLevels,
            Dimension = T3TextureLayout.Texture2D,
            AlphaMode = mAlphaMode,
            D3DFormat = mD3DFormat,
        };

        return metadata;
    }

    public List<byte[]> GetPixelData()
    {
        return mPixelData.Select(x => x.pixelData).ToList();
    }

    public string GetDebugInfo(TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None)
    {
        string d3dtxInfo = "";

        d3dtxInfo += "||||||||||| D3DTX Legacy Version 9 Header |||||||||||" + Environment.NewLine;
        d3dtxInfo += "mName_BlockSize = " + mName_BlockSize + Environment.NewLine;
        d3dtxInfo += "mName = " + mName + Environment.NewLine;
        d3dtxInfo += "mImportName_BlockSize = " + mImportName_BlockSize + Environment.NewLine;
        d3dtxInfo += "mImportName = " + mImportName + Environment.NewLine;
        d3dtxInfo += "mbHasTextureData = " + mbHasTextureData + Environment.NewLine;
        d3dtxInfo += "mbIsMipMapped = " + mbIsMipMapped + Environment.NewLine;
        d3dtxInfo += "mbIsWrapU = " + mbIsWrapU + Environment.NewLine;
        d3dtxInfo += "mbIsWrapV = " + mbIsWrapV + Environment.NewLine;
        d3dtxInfo += "mbIsFiltered = " + mbIsFiltered + Environment.NewLine;
        d3dtxInfo += "mNumMipLevels = " + mNumMipLevels + Environment.NewLine;
        d3dtxInfo += "mD3DFormat = " + mD3DFormat + Environment.NewLine;
        d3dtxInfo += "mWidth = " + mWidth + Environment.NewLine;
        d3dtxInfo += "mHeight = " + mHeight + Environment.NewLine;
        d3dtxInfo += "mWiiForceWidth = " + mWiiForceWidth + Environment.NewLine;
        d3dtxInfo += "mWiiForceHeight = " + mWiiForceHeight + Environment.NewLine;
        d3dtxInfo += "mbWiiForceUncompressed = " + mbWiiForceUncompressed + Environment.NewLine;
        d3dtxInfo += "mTextureDataFormats = " + mTextureDataFormats + Environment.NewLine;
        d3dtxInfo += "mTplTextureDataSize = " + mTplTextureDataSize + Environment.NewLine;
        d3dtxInfo += "mAlphaMode = " + Enum.GetName(typeof(T3TextureAlphaMode), (int)mAlphaMode) + " (" + mAlphaMode + ")" + Environment.NewLine;
        d3dtxInfo += "mWiiTextureFormat = " + mWiiTextureFormat + Environment.NewLine;
        d3dtxInfo += "mbAlphaHDR = " + mbAlphaHDR + Environment.NewLine;
        d3dtxInfo += "mbEncrypted = " + mbEncrypted + Environment.NewLine;
        d3dtxInfo += "mbUsedAsBumpmap = " + mbUsedAsBumpmap + Environment.NewLine;
        d3dtxInfo += "mbUsedAsDetailMap = " + mbUsedAsDetailMap + Environment.NewLine;
        d3dtxInfo += "mDetailMapBrightness = " + mDetailMapBrightness + Environment.NewLine;

        for (int i = 0; i < mPixelData.Count; i++)
        {
            d3dtxInfo += "mPixelData[" + i + "] = " + mPixelData[i].ToString() + Environment.NewLine;
        }

        d3dtxInfo += "|||||||||||||||||||||||||||||||||||||||";

        return d3dtxInfo;
    }


    public uint GetHeaderByteSize()
    {
        return 0;
    }

    public void PrintConsole()
    {
        Console.WriteLine(GetDebugInfo());
    }
}
