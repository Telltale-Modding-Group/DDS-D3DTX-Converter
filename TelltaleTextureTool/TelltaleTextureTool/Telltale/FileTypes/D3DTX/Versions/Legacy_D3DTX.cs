using System;
using System.Collections.Generic;
using System.IO;
using TelltaleTextureTool.TelltaleEnums;
using TelltaleTextureTool.TelltaleTypes;
using TelltaleTextureTool.Utilities;
using TelltaleTextureTool.DirectX;
using TelltaleTextureTool.DirectX.Enums;
using TelltaleTextureTool.Telltale.FileTypes.D3DTX;

/*
 * NOTE:
 * 
 * This version of D3DTX is COMPLETE. 
 * 
 * COMPLETE meaning that all of the data is known and getting identified.
 * Just like the versions before and after, this D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Legacy Version 1 games
 * The Walking Dead Season 1 (TESTED)
*/

namespace TelltaleTextureTool.TelltaleD3DTX
{
    /// <summary>
    /// This is a custom class that matches what is serialized in a legacy D3DTX version supporting the listed titles. (COMPLETE)
    /// </summary>
    public class LegacyD3DTX : ID3DTX
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
        /// [1 byte] Indicates if the texture is wrapped horizontally.
        /// </summary>
        public TelltaleBoolean mbIsWrapU { get; set; }

        /// <summary>
        /// [1 byte] Indicates if the texture is wrapped vertically.
        /// </summary>
        public TelltaleBoolean mbIsWrapV { get; set; }

        /// <summary>
        /// [1 byte] Indicates if the texture is filtered.
        /// </summary>
        public TelltaleBoolean mbIsFiltered { get; set; }

        /// <summary>
        /// [1 byte] Indicates if the texture contains embedded mips.
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
        public TelltaleBoolean mbWiiForceUncompressed { get; set; }

        /// <summary>
        /// [4 bytes] The type of the texture. No enums were found, need more analyzing. Could be texture layout too.
        /// </summary>
        public uint mType { get; set; } //mTextureDataFormats?

        /// <summary>
        /// [4 bytes] The texture data format. No enums were found, need more analyzing. Could be a flag.
        /// </summary>
        public uint mTextureDataFormats { get; set; }

        /// <summary>
        /// [4 bytes] The TPL texture data size, used for Wii textures.
        /// </summary>
        public uint mTplTextureDataSize { get; set; }

        /// <summary>
        /// [4 bytes] The TPL alpha data size, used for Wii textures.
        /// </summary>
        public uint mTplAlphaDataSize { get; set; }

        /// <summary>
        /// [4 bytes] The JPEG texture data size.
        /// </summary>
        public uint mJPEGTextureDataSize { get; set; }

        /// <summary>
        /// [4 bytes] Defines the brightness scale of the texture. (used for lightmaps)
        /// </summary>
        public float mHDRLightmapScale { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines what kind of alpha the texture will have.
        /// </summary>
        public T3TextureAlphaMode mAlphaMode { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines what kind of *exact* alpha the texture will have. (no idea why this exists, wtf Telltale)
        /// </summary>
        public T3TextureAlphaMode mExactAlphaMode { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines the color range of the texture.
        /// </summary>
        public T3TextureColor mColorMode { get; set; }

        /// <summary>
        /// [4 bytes] The Wii texture format.
        /// </summary>
        public WiiTextureFormat mWiiTextureFormat { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture has alpha HDR?
        /// </summary>
        public TelltaleBoolean mbAlphaHDR { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture is encrypted.
        /// </summary>
        public TelltaleBoolean mbEncrypted { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture is used as a bumpmap.
        /// </summary>
        public TelltaleBoolean mbUsedAsBumpmap { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture is used as a detail map.
        /// </summary>
        public TelltaleBoolean mbUsedAsDetailMap { get; set; }

        /// <summary>
        /// [4 bytes] The detail map brightness.
        /// </summary>
        public float mDetailMapBrightness { get; set; }

        /// <summary>
        /// [4 bytes] The normal map format.
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
        /// [4 bytes] An empty buffer for legacy console editions. It should be always zero.
        /// </summary>
        public int mEmptyBuffer { get; set; }

        /// <summary>
        /// A byte array of the pixel regions in a texture.
        /// </summary>
        public TelltalePixelData mPixelData { get; set; }

        /// <summary>
        /// The TPL texture data.
        /// </summary>
        public byte[] mTplData { get; set; } = [];

        /// <summary>
        /// The TPL alpha data.
        /// </summary>
        public byte[] mTplAlphaData { get; set; } = [];

        /// <summary>
        /// The JPEG texture data.
        /// </summary>
        public byte[] mJPEGTextureData { get; set; } = [];

        public LegacyD3DTX() { }

        public void WriteToBinary(BinaryWriter writer, TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None, bool printDebug = false)
        {
            if (game == TelltaleToolGame.DEFAULT || game == TelltaleToolGame.UNKNOWN)
            {
                throw new Exception("The game is not supported.");
            }

            if (game == TelltaleToolGame.TEXAS_HOLD_EM_OG)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
            }

            if (game == TelltaleToolGame.THE_WALKING_DEAD)
            {
                writer.Write(mSamplerState_BlockSize);
                writer.Write(mSamplerState.mData);
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mFlags);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTplAlphaDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write(mHDRLightmapScale);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mExactAlphaMode);
                writer.Write((int)mColorMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
                writer.Write(mNormalMapFmt);
                mUVOffset.WriteBinaryData(writer);
                mUVScale.WriteBinaryData(writer);
            }

            if (game == TelltaleToolGame.PUZZLE_AGENT_2 ||
            game == TelltaleToolGame.LAW_AND_ORDER_LEGACIES ||
            game == TelltaleToolGame.JURASSIC_PARK_THE_GAME)
            {
                writer.Write(mSamplerState_BlockSize);
                writer.Write(mSamplerState.mData);
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mFlags);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTplAlphaDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write(mHDRLightmapScale);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mExactAlphaMode);
                writer.Write((int)mColorMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
                writer.Write(mNormalMapFmt);
                mUVOffset.WriteBinaryData(writer);
                mUVScale.WriteBinaryData(writer);
            }

            if (game == TelltaleToolGame.NELSON_TETHERS_PUZZLE_AGENT ||
            game == TelltaleToolGame.CSI_FATAL_CONSPIRACY ||
            game == TelltaleToolGame.HECTOR_BADGE_OF_CARNAGE ||
            game == TelltaleToolGame.BACK_TO_THE_FUTURE_THE_GAME ||
            game == TelltaleToolGame.POKER_NIGHT_AT_THE_INVENTORY)
            {
                writer.Write(mSamplerState_BlockSize);
                writer.Write(mSamplerState.mData);
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTplAlphaDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
                writer.Write(mNormalMapFmt);
            }

            if (game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_104 ||
            game == TelltaleToolGame.TALES_OF_MONKEY_ISLAND ||
            game == TelltaleToolGame.CSI_DEADLY_INTENT ||
            game == TelltaleToolGame.SAM_AND_MAX_THE_DEVILS_PLAYHOUSE ||
            game == TelltaleToolGame.SAM_AND_MAX_SAVE_THE_WORLD_2007)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTplAlphaDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
                writer.Write(mNormalMapFmt);
            }

            if (game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_101 ||
            game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_102 ||
            game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_103)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTplAlphaDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
                writer.Write(mNormalMapFmt);
            }

            if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_105)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_103 || game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_104)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_102 || game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_101)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write(mJPEGTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.TEXAS_HOLD_EM_V1 || game == TelltaleToolGame.SAM_AND_MAX_BEYOND_TIME_AND_SPACE_NEW)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mTplTextureDataSize);
                writer.Write(mTextureDataFormats);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.CSI_HARD_EVIDENCE || game == TelltaleToolGame.SAM_AND_MAX_BEYOND_TIME_AND_SPACE_OG)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                writer.Write(mTextureDataFormats);
                writer.Write(mTplTextureDataSize);
                writer.Write((int)mAlphaMode);
                writer.Write((int)mWiiTextureFormat);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.BONE_OUT_FROM_BONEVILLE || game == TelltaleToolGame.BONE_THE_GREAT_COW_RACE)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mType);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
                writer.Write(mDetailMapBrightness);
            }

            if (game == TelltaleToolGame.CSI_3_DIMENSIONS)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mType);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
            }

            if (game == TelltaleToolGame.SAM_AND_MAX_SAVE_THE_WORLD_2006)
            {
                writer.Write(mName_BlockSize);
                ByteFunctions.WriteString(writer, mName);
                writer.Write(mImportName_BlockSize);
                ByteFunctions.WriteString(writer, mImportName);
                ByteFunctions.WriteBoolean(writer, mbHasTextureData.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsMipMapped.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapU.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsWrapV.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbIsFiltered.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEmbedMipMaps.mbTelltaleBoolean);
                writer.Write(mNumMipLevels);
                writer.Write((uint)mD3DFormat);
                writer.Write(mWidth);
                writer.Write(mHeight);
                writer.Write(mWiiForceWidth);
                writer.Write(mWiiForceHeight);
                ByteFunctions.WriteBoolean(writer, mbWiiForceUncompressed.mbTelltaleBoolean);
                writer.Write(mType);
                ByteFunctions.WriteBoolean(writer, mbAlphaHDR.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbEncrypted.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsBumpmap.mbTelltaleBoolean);
                ByteFunctions.WriteBoolean(writer, mbUsedAsDetailMap.mbTelltaleBoolean);
            }

            if (platform != T3PlatformType.ePlatform_None)
            {
                writer.Write(mEmptyBuffer);
            }

            mPixelData.WriteBinaryData(writer);

            if (mTplTextureDataSize > 0)
            {
                writer.Write(mTplTextureDataSize);
            }

            if (mTplAlphaDataSize > 0)
            {
                writer.Write(mTplAlphaDataSize);
            }

            if (mJPEGTextureDataSize > 0)
            {
                writer.Write(mJPEGTextureData);
            }
        }

        public void ReadFromBinary(BinaryReader reader, TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None, bool printDebug = false)
        {
            if (game == TelltaleToolGame.DEFAULT || game == TelltaleToolGame.UNKNOWN)
            {
                throw new Exception();
            }

            bool read = true;
            bool isValid = true;

            while (read && isValid)
            {
                isValid = true;
                // I know there is a lot of repetition and ifs, but the way Telltale have updated their textures is unreliable and I would prefer to have an easier time reading the data.

                if (game == TelltaleToolGame.TEXAS_HOLD_EM_OG)
                {
                    mName_BlockSize = reader.ReadInt32();
                    mName = ByteFunctions.ReadString(reader);
                    mImportName_BlockSize = reader.ReadInt32();
                    mImportName = ByteFunctions.ReadString(reader);
                    mbHasTextureData = new TelltaleBoolean(reader);
                    mNumMipLevels = reader.ReadUInt32();
                    //  mNumMipLevels = 1; // The first version indicates that there are mips, but the actual texture doesn't. It applies to all textures.
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                }

                if (game == TelltaleToolGame.THE_WALKING_DEAD)
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
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                    mFlags = reader.ReadUInt32();
                    mWiiForceWidth = reader.ReadUInt32();
                    mWiiForceHeight = reader.ReadUInt32();
                    mbWiiForceUncompressed = new TelltaleBoolean(reader);
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTplAlphaDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mHDRLightmapScale = reader.ReadSingle();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mExactAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mColorMode = (T3TextureColor)reader.ReadInt32();
                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                    mNormalMapFmt = reader.ReadInt32();
                    mUVOffset = new Vector2(reader); //mUVOffset [8 bytes]
                    mUVScale = new Vector2(reader); //mUVScale [8 bytes]
                }

                if (game == TelltaleToolGame.PUZZLE_AGENT_2 ||
                game == TelltaleToolGame.LAW_AND_ORDER_LEGACIES ||
                game == TelltaleToolGame.JURASSIC_PARK_THE_GAME)
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
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                    mFlags = reader.ReadUInt32();
                    mWiiForceWidth = reader.ReadUInt32();
                    mWiiForceHeight = reader.ReadUInt32();
                    mbWiiForceUncompressed = new TelltaleBoolean(reader);
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTplAlphaDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mExactAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mColorMode = (T3TextureColor)reader.ReadUInt32();
                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                    mNormalMapFmt = reader.ReadInt32();
                    mUVOffset = new Vector2(reader); //mUVOffset [8 bytes]
                    mUVScale = new Vector2(reader); //mUVScale [8 bytes]
                }

                if (game == TelltaleToolGame.NELSON_TETHERS_PUZZLE_AGENT ||
                game == TelltaleToolGame.CSI_FATAL_CONSPIRACY ||
                game == TelltaleToolGame.HECTOR_BADGE_OF_CARNAGE ||
                game == TelltaleToolGame.BACK_TO_THE_FUTURE_THE_GAME ||
                game == TelltaleToolGame.POKER_NIGHT_AT_THE_INVENTORY)
                {
                    mSamplerState_BlockSize = reader.ReadInt32();
                    mSamplerState = new T3SamplerStateBlock(reader); //mSamplerState [4 bytes]

                    mName_BlockSize = reader.ReadInt32();
                    mName = ByteFunctions.ReadString(reader);
                    mImportName_BlockSize = reader.ReadInt32();
                    mImportName = ByteFunctions.ReadString(reader);

                    mToolProps = new ToolProps(reader);
                    mbHasTextureData = new TelltaleBoolean(reader);
                    mbIsMipMapped = new TelltaleBoolean(reader);
                    mbEmbedMipMaps = new TelltaleBoolean(reader);

                    mNumMipLevels = reader.ReadUInt32();
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                    mWiiForceWidth = reader.ReadUInt32();
                    mWiiForceHeight = reader.ReadUInt32();
                    mbWiiForceUncompressed = new TelltaleBoolean(reader);
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTplAlphaDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                    mNormalMapFmt = reader.ReadInt32();
                }

                if (game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_104 ||
                game == TelltaleToolGame.TALES_OF_MONKEY_ISLAND ||
                game == TelltaleToolGame.CSI_DEADLY_INTENT ||
                game == TelltaleToolGame.SAM_AND_MAX_THE_DEVILS_PLAYHOUSE ||
                game == TelltaleToolGame.SAM_AND_MAX_SAVE_THE_WORLD_2007)
                {
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
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTplAlphaDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                    mNormalMapFmt = reader.ReadInt32();
                }

                if (game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_101 ||
                game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_102 ||
                game == TelltaleToolGame.WALLACE_AND_GROMITS_GRAND_ADVENTURES_103)
                {
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
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                    mNormalMapFmt = reader.ReadInt32();
                }

                if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_105)
                {
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
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_103 || game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_104)
                {
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
                    mType = reader.ReadUInt32(); //???
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_102 || game == TelltaleToolGame.STRONG_BADS_COOL_GAME_FOR_ATTRACTIVE_PEOPLE_101)
                {
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
                    mJPEGTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.TEXAS_HOLD_EM_V1 || game == TelltaleToolGame.SAM_AND_MAX_BEYOND_TIME_AND_SPACE_NEW)
                {
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
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTextureDataFormats = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();

                    mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.CSI_HARD_EVIDENCE)
                {
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
                    mType = reader.ReadUInt32();
                    mTextureDataFormats = reader.ReadUInt32();
                    mTplTextureDataSize = reader.ReadUInt32();
                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.SAM_AND_MAX_BEYOND_TIME_AND_SPACE_OG)
                {
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
                    mTplTextureDataSize = reader.ReadUInt32();
                    mTextureDataFormats = reader.ReadUInt32();

                    mAlphaMode = (T3TextureAlphaMode)reader.ReadInt32();
                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.BONE_OUT_FROM_BONEVILLE || game == TelltaleToolGame.BONE_THE_GREAT_COW_RACE)
                {
                    mName_BlockSize = reader.ReadInt32();
                    mName = ByteFunctions.ReadString(reader);
                    mImportName_BlockSize = reader.ReadInt32();
                    mImportName = ByteFunctions.ReadString(reader);
                    mbHasTextureData = new TelltaleBoolean(reader);
                    mbIsMipMapped = new TelltaleBoolean(reader);
                    mbIsWrapU = new TelltaleBoolean(reader);
                    mbIsWrapV = new TelltaleBoolean(reader);
                    mbIsFiltered = new TelltaleBoolean(reader);
                    mNumMipLevels = reader.ReadUInt32();
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                    mType = reader.ReadUInt32();

                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);

                    mDetailMapBrightness = reader.ReadSingle();
                }

                if (game == TelltaleToolGame.CSI_3_DIMENSIONS)
                {
                    mName_BlockSize = reader.ReadInt32();
                    mName = ByteFunctions.ReadString(reader);
                    mImportName_BlockSize = reader.ReadInt32();
                    mImportName = ByteFunctions.ReadString(reader);
                    mbHasTextureData = new TelltaleBoolean(reader);
                    mbIsMipMapped = new TelltaleBoolean(reader);
                    mbIsWrapU = new TelltaleBoolean(reader);
                    mbIsWrapV = new TelltaleBoolean(reader);
                    mbIsFiltered = new TelltaleBoolean(reader);
                    mNumMipLevels = reader.ReadUInt32();
                    mD3DFormat = (LegacyFormat)reader.ReadUInt32();
                    mWidth = reader.ReadUInt32();
                    mHeight = reader.ReadUInt32();
                    mType = reader.ReadUInt32();

                    mbEncrypted = new TelltaleBoolean(reader);
                }

                if (game == TelltaleToolGame.SAM_AND_MAX_SAVE_THE_WORLD_2006)
                {
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

                    mWiiForceHeight = reader.ReadUInt32();
                    mWiiForceWidth = reader.ReadUInt32();
                    mbWiiForceUncompressed = new TelltaleBoolean(reader);

                    mType = reader.ReadUInt32();

                    mbAlphaHDR = new TelltaleBoolean(reader);
                    mbEncrypted = new TelltaleBoolean(reader);
                    mbUsedAsBumpmap = new TelltaleBoolean(reader);
                    mbUsedAsDetailMap = new TelltaleBoolean(reader);
                }

                if (platform != T3PlatformType.ePlatform_None)
                {
                    mEmptyBuffer = reader.ReadInt32(); //mEmptyBuffer [4 bytes]
                }

                PrintConsole(game);

                uint mTextureDataSize = reader.ReadUInt32();

                if (mTextureDataSize == 0 && mbHasTextureData.mbTelltaleBoolean)
                {
                    continue;
                }

                reader.BaseStream.Position -= 4;

                int magic = reader.ReadInt32();
                if (magic == 8 || magic == mName.Length + 8)
                {
                    reader.BaseStream.Position -= 4;
                    break;
                }

                reader.BaseStream.Position -= 4;

                mNumMipLevels = mbEncrypted.mbTelltaleBoolean ? 1 : mNumMipLevels;
                int skip = mbEncrypted.mbTelltaleBoolean ? 128 : 0;

                mPixelData = new TelltalePixelData(reader, skip);

                if (mTplTextureDataSize > 0)
                {
                    mTplData = new byte[mTplTextureDataSize];

                    for (int i = 0; i < mTplTextureDataSize; i++)
                    {
                        mTplData[i] = reader.ReadByte();
                    }
                }

                if (mTplAlphaDataSize > 0)
                {
                    mTplAlphaData = new byte[mTplAlphaDataSize];

                    for (int i = 0; i < mTplAlphaDataSize; i++)
                    {
                        mTplAlphaData[i] = reader.ReadByte();
                    }
                }

                if (mJPEGTextureDataSize > 0)
                {
                    mJPEGTextureData = new byte[mJPEGTextureDataSize];

                    for (int i = 0; i < mJPEGTextureDataSize; i++)
                    {
                        mJPEGTextureData[i] = reader.ReadByte();
                    }
                }

                read = false;
            }

            // if (!mbHasTextureData.mbTelltaleBoolean)
            // {
            //     PrintConsole();
            //     throw new PixelDataNotFoundException("The texture does not have any pixel data!");
            // }

            if (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                PrintConsole();
                throw new Exception("We did not reach the end of the file!");
            }

            if (printDebug)
                PrintConsole(game);
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

            var textureData = TextureManager.GetPixelDataArrayFromSections(imageSections);

            if (mTextureDataFormats > 0x200)
            {
                // Attempt to write pixel data for PS3 and other console games
                mPixelData = new TelltalePixelData(textureData, 128, 128);
            }
            else
            {
                mPixelData = new TelltalePixelData()
                {
                    length = (uint)textureData.Length,
                    pixelData = textureData
                };
            }

            PrintConsole();
        }

        public D3DTXMetadata GetD3DTXMetadata()
        {
            D3DTXMetadata metadata = new()
            {
                TextureName = mName,
                Width = mWidth,
                Height = mHeight,
                MipLevels = mNumMipLevels,
                Dimension = T3TextureLayout.Texture2D,
                AlphaMode = mAlphaMode,
                D3DFormat = mD3DFormat,
                SurfaceGamma = T3SurfaceGamma.Unknown,
            };

            return metadata;
        }

        public List<byte[]> GetPixelData()
        {
            return [mPixelData.pixelData];
        }

        public string GetDebugInfo(TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None)
        {
            if (game == TelltaleToolGame.DEFAULT)
            {
                return string.Empty;
            }

            string d3dtxInfo = "Game: " + Enum.GetName(typeof(TelltaleToolGame), game) + "\n";

            d3dtxInfo = "|||||| D3DTX Info ||||||\n";
            d3dtxInfo += "Name: " + mName + "\n";
            d3dtxInfo += "Import Name: " + mImportName + "\n";
            d3dtxInfo += "Has Texture Data: " + mbHasTextureData.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Is Mip Mapped: " + mbIsMipMapped.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Is Wrap U: " + mbIsWrapU.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Is Wrap V: " + mbIsWrapV.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Is Filtered: " + mbIsFiltered.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Embed Mip Maps: " + mbEmbedMipMaps.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Num Mip Levels: " + mNumMipLevels + "\n";
            d3dtxInfo += "D3D Format: " + mD3DFormat + "\n";
            d3dtxInfo += "Width: " + mWidth + "\n";
            d3dtxInfo += "Height: " + mHeight + "\n";
            d3dtxInfo += "Wii Force Width: " + mWiiForceWidth + "\n";
            d3dtxInfo += "Wii Force Height: " + mWiiForceHeight + "\n";
            d3dtxInfo += "Wii Force Uncompressed: " + mbWiiForceUncompressed.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Type: " + mType + "\n";
            d3dtxInfo += "Texture Data Formats: " + mTextureDataFormats + "\n";
            d3dtxInfo += "TPL Texture Data Size: " + mTplTextureDataSize + "\n";
            d3dtxInfo += "TPL Alpha Data Size: " + mTplAlphaDataSize + "\n";
            d3dtxInfo += "JPEG Texture Data Size: " + mJPEGTextureDataSize + "\n";
            d3dtxInfo += "Alpha Mode: " + mAlphaMode + "\n";
            d3dtxInfo += "Wii Texture Format: " + mWiiTextureFormat + "\n";
            d3dtxInfo += "Alpha HDR: " + mbAlphaHDR.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Encrypted: " + mbEncrypted.mbTelltaleBoolean + "\n";
            d3dtxInfo += "Detail Map Brightness: " + mDetailMapBrightness + "\n";
            d3dtxInfo += "Normal Map Format: " + mNormalMapFmt + "\n";
            d3dtxInfo += "UV Offset: " + mUVOffset + "\n";
            d3dtxInfo += "UV Scale: " + mUVScale + "\n";
            d3dtxInfo += "Empty Buffer: " + mEmptyBuffer + "\n";

            d3dtxInfo += "|||||||||||||||||||||||||||||||||||||||";

            return d3dtxInfo;
        }

        public uint GetHeaderByteSize()
        {
            return 0;
        }

        public void PrintConsole(TelltaleToolGame game = TelltaleToolGame.DEFAULT, T3PlatformType platform = T3PlatformType.ePlatform_None)
        {
            Console.WriteLine(GetDebugInfo(game, platform));
        }
    }
}