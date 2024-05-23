﻿using System;
using System.Collections.Generic;
using System.IO;
using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.TelltaleTypes;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using System.Runtime.InteropServices;
using DirectXTexNet;

/*
 * NOTE:
 *
 * This version of D3DTX is INCOMPLETE.
 *
 * This version may not exist or have existed in previous patches, which are not available now.
 * INCOMPLETE meaning that all of the data isn't known and getting identified correctly. We still parse, identify, and modify if needed but it's just named as "Unknown".
 * The reason being we don't have "full knowledge" of it, given that games that shipped with this version haven't shipped with a PDB.
 * So the only source is looking through the strings in the game exe through a hex editor to identify what variables might be in the file.
 * This D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
 */

/* --- D3DTX Version 8 games ---
 * Batman: The Telltale Series (PROBABLY BEFORE DECEMBER PATCH) (UNTESTED)
 */

namespace D3DTX_Converter.TelltaleD3DTX;

/// <summary>
/// This is a custom class that matches what is serialized in a D3DTX version 8 class. (INCOMPLETE)
/// </summary>
public class D3DTX_V8
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
    /// [4 bytes] The mPlatform block size in bytes. Note: the parsed value is always 8.
    /// </summary>
    public int mPlatform_BlockSize { get; set; }

    /// <summary>
    /// [4 bytes] The platform type enum value.
    /// </summary>
    public PlatformType mPlatform { get; set; }

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
    /// [4 bytes] The import scale of the texture file.
    /// </summary>
    public float mImportScale { get; set; }

    /// <summary>
    /// [1 byte] Whether or not the d3dtx contains a Tool Properties. [PropertySet] (Always false)
    /// </summary>
    public ToolProps mToolProps { get; set; }

    /// <summary>
    /// [4 bytes] The number of mip maps in the texture.
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
    /// [4 bytes] The depth of a volume texture in pixels.
    /// </summary>
    public int mDepth { get; set; }

    /// <summary>
    /// [4 bytes] mArraySize, not sure what this is for yet.
    /// </summary>
    public int mArraySize { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the compression used for the texture.
    /// </summary>
    public T3SurfaceFormat mSurfaceFormat { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the dimension of the texture.
    /// </summary>
    public T3TextureLayout mTextureLayout { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the gamma of the texture.
    /// </summary>
    public T3SurfaceGamma mSurfaceGamma { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the multisample (anisotropic) level of the texture.
    /// </summary>
    public T3SurfaceMultisample mSurfaceMultisample { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines the resource type of the texture.
    /// </summary>
    public T3ResourceUsage mResourceUsage { get; set; }

    /// <summary>
    /// [4 bytes] An enum, defines what kind of texture it is.
    /// </summary>
    public T3TextureType mType { get; set; }

    /// <summary>
    /// [4 bytes] The size of the mSwizzle block data.
    /// </summary>
    public int mSwizzleSize { get; set; }

    /// <summary>
    /// [4 bytes] mSwizzle compression parameters. (usually used for consoles).
    /// </summary>
    public RenderSwizzleParams mSwizzle { get; set; }

    /// <summary>
    /// [4 bytes] Defines how glossy the texture is.
    /// </summary>
    public float mSpecularGlossExponent { get; set; }

    /// <summary>
    /// [4 bytes] Defines the brightness scale of the texture. (used for lightmaps)
    /// </summary>
    public float mHDRLightmapScale { get; set; }

    /// <summary>
    /// [4 bytes] Defines the toon cutoff gradient of the texture.
    /// </summary>
    public float mToonGradientCutoff { get; set; }

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
    /// [4 bytes] The size in bytes of the mArrayFrameNames block.
    /// </summary>
    public uint mArrayFrameNames_ArrayCapacity { get; set; }

    /// <summary>
    /// [4 bytes] The amount of elements in the mArrayFrameNames array.
    /// </summary>
    public int mArrayFrameNames_ArrayLength { get; set; }

    /// <summary>
    /// [8 bytes for each element] An array containing frame names. (Usually unused)
    /// </summary>
    public List<Symbol> mArrayFrameNames { get; set; }

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
    /// [24 bytes for each element] An array containing each pixel region in the texture.
    /// </summary>
    public RegionStreamHeader[] mRegionHeaders { get; set; }

    /// <summary>
    /// A byte array of the pixel regions in a texture. Starts from smallest mip map to largest mip map.
    /// </summary>
    public List<byte[]> mPixelData { get; set; }

    /// <summary>
    /// D3DTX V8 Header (empty constructor, only used for json deserialization)
    /// </summary>
    public D3DTX_V8() { }

    /// <summary>
    /// Deserializes a D3DTX Object from a byte array.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="bytePointerPosition"></param>
    public D3DTX_V8(BinaryReader reader, bool showConsole = false)
    {
        mVersion = reader.ReadInt32(); //mVersion [4 bytes]
        mSamplerState_BlockSize = reader.ReadInt32(); //mSamplerState Block Size [4 bytes]
        mSamplerState = new T3SamplerStateBlock() //mSamplerState [4 bytes]
        {
            mData = reader.ReadUInt32()
        };
        mPlatform_BlockSize = reader.ReadInt32(); //mPlatform Block Size [4 bytes]
        mPlatform = (PlatformType)reader.ReadInt32(); //mPlatform [4 bytes]
        mName_BlockSize = reader.ReadInt32(); //mName Block Size [4 bytes] //mName block size (size + string len)
        mName = ByteFunctions.ReadString(reader); //mName [x bytes]
        mImportName_BlockSize =
            reader.ReadInt32(); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
        mImportName = ByteFunctions.ReadString(reader); //mImportName [x bytes] (this is always 0)
        mImportScale = reader.ReadSingle(); //mImportScale [4 bytes]
        mToolProps = new ToolProps(reader); //mToolProps [1 byte]
        mNumMipLevels = reader.ReadInt32(); //mNumMipLevels [4 bytes]
        mWidth = reader.ReadInt32(); //mWidth [4 bytes]
        mHeight = reader.ReadInt32(); //mHeight [4 bytes]
        mDepth = reader.ReadInt32(); //mDepth [4 bytes]
        mArraySize = reader.ReadInt32(); //mArraySize [4 bytes]
        mSurfaceFormat = (T3SurfaceFormat)reader.ReadInt32(); //mSurfaceFormat [4 bytes]
        mTextureLayout = (T3TextureLayout)reader.ReadInt32(); //mTextureLayout [4 bytes]
        mSurfaceGamma = (T3SurfaceGamma)reader.ReadInt32(); //mSurfaceGamma [4 bytes]
        mSurfaceMultisample = (T3SurfaceMultisample)reader.ReadInt32(); //mSurfaceMultisample [4 bytes]
        mResourceUsage = (T3ResourceUsage)reader.ReadInt32(); //mResourceUsage [4 bytes]
        mType = (T3TextureType)reader.ReadInt32(); //mType [4 bytes]
        mSwizzleSize = reader.ReadInt32(); //mSwizzleSize [4 bytes]
        mSwizzle = new(reader); //mSwizzle [4 bytes]
        mSpecularGlossExponent = reader.ReadSingle(); //mSpecularGlossExponent [4 bytes]
        mHDRLightmapScale = reader.ReadSingle(); //mHDRLightmapScale [4 bytes]
        mToonGradientCutoff = reader.ReadSingle(); //mToonGradientCutoff [4 bytes]
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

        //--------------------------mArrayFrameNames--------------------------
        mArrayFrameNames_ArrayCapacity = reader.ReadUInt32(); //mArrayFrameNames DCArray Capacity [4 bytes]
        mArrayFrameNames_ArrayLength =
            reader.ReadInt32(); //mArrayFrameNames DCArray Length [4 bytes] //ADD 1 BECAUSE COUNTING STARTS AT 0
        mArrayFrameNames = new List<Symbol>();
        for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
        {
            Symbol newSymbol = new Symbol()
            {
                mCrc64 = reader.ReadUInt64()
            };

            mArrayFrameNames.Add(newSymbol);
        }

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

        //--------------------------StreamHeader--------------------------
        mStreamHeader = new StreamHeader()
        {
            mRegionCount = reader.ReadInt32(), //[4 bytes]
            mAuxDataCount = reader.ReadInt32(), //[4 bytes]
            mTotalDataSize = reader.ReadInt32() //[4 bytes]
        };

        //--------------------------mRegionHeaders--------------------------
        mRegionHeaders = new RegionStreamHeader[mStreamHeader.mRegionCount];
        for (int i = 0; i < mStreamHeader.mRegionCount; i++)
        {
            mRegionHeaders[i] = new RegionStreamHeader()
            {
                mFaceIndex = reader.ReadInt32(), //[4 bytes]
                mMipIndex = reader.ReadInt32(), //[4 bytes] 
                mMipCount = reader.ReadInt32(), //[4 bytes]
                mDataSize = reader.ReadUInt32(), //[4 bytes]
                mPitch = reader.ReadInt32(), //[4 bytes]
                mSlicePitch = reader.ReadInt32() //[4 bytes]
            };
        }

        //-----------------------------------------D3DTX HEADER END-----------------------------------------
        //--------------------------STORING D3DTX IMAGE DATA--------------------------
        mPixelData = [];

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
        mDepth = metadata.Depth;
        mArraySize = metadata.ArraySize;
        mSurfaceGamma = DDS_DirectXTexNet.IsSRGB(metadata.Format) ? T3SurfaceGamma.eSurfaceGamma_sRGB : T3SurfaceGamma.eSurfaceGamma_Linear;

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
                mMipCount = 1, // mMipCount is a strange variable, it is always 1 for every single texture
                mPitch = (int)sections[i].RowPitch,
                mSlicePitch = (int)sections[i].SlicePitch,
            };
        }

        if (metadata.IsCubemap())
        {
            mArraySize /= 6;
            mTextureLayout = mArraySize > 1 ? T3TextureLayout.eTextureLayout_CubeArray : T3TextureLayout.eTextureLayout_Cube;

            int interval = mStreamHeader.mRegionCount / mNumMipLevels;
            // Example a cube array textures with 5 mips will have 30 regions (6 faces * 5 mips)
            // If the array is 2 element there will be 60 regions (6 faces * 5 mips * 2 elements)
            // The mip index will be the region index % interval
            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                mRegionHeaders[i].mFaceIndex = i % (6 * mArraySize); //Unknown guess, it could be 6 or 6 * mArraySize
                mRegionHeaders[i].mMipIndex = i / interval;
            }
        }

        else if (metadata.IsVolumemap())
        {
            mTextureLayout = T3TextureLayout.eTextureLayout_3D;

            int currDepth = metadata.Depth;
            int currentMipIndex = mNumMipLevels - 1;
            int copyOfDepth = currDepth;

            int depthIndex = 0;
            int faceIndex = 0;

            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                mRegionHeaders[mStreamHeader.mRegionCount - currDepth + depthIndex++].mFaceIndex = faceIndex++; //NOTE: This could be different for 3D textures. I don't have information at the moment.
                mRegionHeaders[mStreamHeader.mRegionCount - i - 1].mMipIndex = currentMipIndex;

                if (depthIndex == copyOfDepth)
                {
                    currentMipIndex--;

                    copyOfDepth = Math.Max(copyOfDepth / 2, 1);
                    currDepth += copyOfDepth;

                    depthIndex = 0;
                    faceIndex = 0;
                }
            }
        }
        else
        {
            mTextureLayout = mArraySize > 1 ? T3TextureLayout.eTextureLayout_2DArray : T3TextureLayout.eTextureLayout_2D;

            int interval = mStreamHeader.mRegionCount / mNumMipLevels;

            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                mRegionHeaders[i].mFaceIndex = i % mArraySize;
                mRegionHeaders[i].mMipIndex = i / interval;
            }
        }

        UpdateArrayCapacities();
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
        writer.Write(mPlatform_BlockSize); //mPlatform Block Size [4 bytes]
        writer.Write((int)mPlatform); //mPlatform [4 bytes]
        writer.Write(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
        ByteFunctions.WriteString(writer, mName); //mName [x bytes]
        writer.Write(mImportName_BlockSize); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
        ByteFunctions.WriteString(writer, mImportName); //mImportName [x bytes] (this is always 0)
        writer.Write(mImportScale); //mImportScale [4 bytes]
        ByteFunctions.WriteBoolean(writer, mToolProps.mbHasProps); //mToolProps mbHasProps [1 byte]
        writer.Write(mNumMipLevels); //mNumMipLevels [4 bytes]
        writer.Write(mWidth); //mWidth [4 bytes]
        writer.Write(mHeight); //mHeight [4 bytes]
        writer.Write(mDepth); //mDepth [4 bytes]
        writer.Write(mArraySize); //mArraySize [4 bytes]
        writer.Write((int)mSurfaceFormat); //mSurfaceFormat [4 bytes]
        writer.Write((int)mTextureLayout); //mTextureLayout [4 bytes]
        writer.Write((int)mSurfaceGamma); //mSurfaceGamma [4 bytes]
        writer.Write((int)mSurfaceMultisample); //mSurfaceMultisample [4 bytes]
        writer.Write((int)mResourceUsage); //mResourceUsage [4 bytes]
        writer.Write((int)mType); //mType [4 bytes]
        writer.Write(mSwizzleSize); //mSwizzleSize [4 bytes]
        writer.Write(mSwizzle.mSwizzle1); //mSwizzle A [1 byte]
        writer.Write(mSwizzle.mSwizzle1); //mSwizzle B [1 byte]
        writer.Write(mSwizzle.mSwizzle1); //mSwizzle C [1 byte]
        writer.Write(mSwizzle.mSwizzle1); //mSwizzle D [1 byte]
        writer.Write(mSpecularGlossExponent); //mSpecularGlossExponent [4 bytes]
        writer.Write(mHDRLightmapScale); //mHDRLightmapScale [4 bytes]
        writer.Write(mToonGradientCutoff); //mToonGradientCutoff [4 bytes]
        writer.Write((int)mAlphaMode); //mAlphaMode [4 bytes]
        writer.Write((int)mColorMode); //mColorMode [4 bytes]
        writer.Write(mUVOffset.x); //mUVOffset X [4 bytes]
        writer.Write(mUVOffset.y); //mUVOffset Y [4 bytes]
        writer.Write(mUVScale.x); //mUVScale X [4 bytes]
        writer.Write(mUVScale.y); //mUVScale Y [4 bytes]

        writer.Write(mArrayFrameNames_ArrayCapacity); //mArrayFrameNames DCArray Capacity [4 bytes]
        writer.Write(
            mArrayFrameNames_ArrayLength); //mArrayFrameNames DCArray Length [4 bytes] //ADD 1 BECAUSE COUNTING STARTS AT 0
        for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
        {
            writer.Write(mArrayFrameNames[i].mCrc64); //Symbol [8 bytes]
        }

        writer.Write(mToonRegions_ArrayCapacity); //mToonRegions DCArray Capacity [4 bytes]
        writer.Write(mToonRegions_ArrayLength); //mToonRegions DCArray Length [4 bytes]
        for (int i = 0; i < mToonRegions_ArrayLength; i++)
        {
            mToonRegions[i].WriteBinaryData(writer);
        }

        writer.Write(mStreamHeader.mRegionCount); //mRegionCount [4 bytes]
        writer.Write(mStreamHeader.mAuxDataCount); //mAuxDataCount [4 bytes]
        writer.Write(mStreamHeader.mTotalDataSize); //mTotalDataSize [4 bytes]

        for (int i = 0; i < mStreamHeader.mRegionCount; i++)
        {
            writer.Write(mRegionHeaders[i].mFaceIndex); //[4 bytes]
            writer.Write(mRegionHeaders[i].mMipIndex); //[4 bytes]
            writer.Write(mRegionHeaders[i].mMipCount); //[4 bytes]
            writer.Write(mRegionHeaders[i].mDataSize); //[4 bytes]
            writer.Write(mRegionHeaders[i].mPitch); //[4 bytes]
            writer.Write(mRegionHeaders[i].mSlicePitch); //[4 bytes]
        }

        for (int i = 0; i < mPixelData.Count; i++)
        {
            writer.Write(mPixelData[i]);
        }
    }

    public uint GetHeaderByteSize()
    {
        uint totalSize = 0;

        totalSize += (uint)Marshal.SizeOf(mVersion); //mVersion [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mSamplerState_BlockSize); //mSamplerState Block Size [4 bytes]
        totalSize += mSamplerState.GetByteSize(); //mSamplerState mData [4 bytes] 
        totalSize += (uint)Marshal.SizeOf(mPlatform_BlockSize); //mPlatform Block Size [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mPlatform); //mPlatform [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mName_BlockSize); //mName Block Size [4 bytes] //mName block size (size + string len)
        totalSize += (uint)Marshal.SizeOf(mName.Length); //mName (strength length prefix) [4 bytes]
        totalSize += (uint)mName.Length;  //mName [x bytes]
        totalSize += (uint)Marshal.SizeOf(mImportName_BlockSize); //mImportName Block Size [4 bytes] //mImportName block size (size + string len)
        totalSize += (uint)Marshal.SizeOf(mImportName.Length); //mImportName (strength length prefix) [4 bytes] (this is always 0)
        totalSize += (uint)mImportName.Length; //mImportName [x bytes] (this is always 0)
        totalSize += (uint)Marshal.SizeOf(mImportScale); //mImportScale [4 bytes]
        totalSize += mToolProps.GetByteSize();
        totalSize += (uint)Marshal.SizeOf(mNumMipLevels); //mNumMipLevels [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mWidth); //mWidth [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mHeight); //mHeight [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mDepth); //mDepth [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mArraySize); //mArraySize [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mSurfaceFormat); //mSurfaceFormat [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mTextureLayout); //mTextureLayout [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mSurfaceGamma); //mSurfaceGamma [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mSurfaceMultisample); //mSurfaceMultisample [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mResourceUsage); //mResourceUsage [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mType); //mType [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mSwizzleSize); //mSwizzleSize [4 bytes]
        totalSize += mSwizzle.GetByteSize();
        totalSize += (uint)Marshal.SizeOf(mSpecularGlossExponent); //mSpecularGlossExponent [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mHDRLightmapScale); //mHDRLightmapScale [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mToonGradientCutoff); //mToonGradientCutoff [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mAlphaMode); //mAlphaMode [4 bytes]
        totalSize += (uint)Marshal.SizeOf((int)mColorMode); //mColorMode [4 bytes]
        totalSize += mUVOffset.GetByteSize(); //[4 bytes]
        totalSize += mUVScale.GetByteSize(); //[4 bytes]

        totalSize += (uint)Marshal.SizeOf(mArrayFrameNames_ArrayCapacity); //mArrayFrameNames DCArray Capacity [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mArrayFrameNames_ArrayLength); //mArrayFrameNames DCArray Length [4 bytes] //ADD 1 BECAUSE COUNTING STARTS AT 0
        for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
        {
            totalSize += mArrayFrameNames[i].GetByteSize(); //Symbol [8 bytes]
        }

        totalSize += (uint)Marshal.SizeOf(mToonRegions_ArrayCapacity); //mToonRegions DCArray Capacity [4 bytes]
        totalSize += (uint)Marshal.SizeOf(mToonRegions_ArrayLength); //mToonRegions DCArray Length [4 bytes]
        for (int i = 0; i < mToonRegions_ArrayLength; i++)
        {
            totalSize += mToonRegions[i].GetByteSize();
        }

        totalSize += mStreamHeader.GetByteSize();

        for (int i = 0; i < mStreamHeader.mRegionCount; i++)
        {
            totalSize += 4; //[4 bytes]
            totalSize += 4; //[4 bytes]
            totalSize += 4; //[4 bytes]
            totalSize += 4; //[4 bytes]
            totalSize += 4; //[4 bytes]
            totalSize += 4; //[4 bytes]
        }

        return totalSize;
    }

    public void PrintConsole()
    {
        Console.WriteLine("||||||||||| D3DTX Header |||||||||||");
        Console.WriteLine("D3DTX mVersion = {0}", mVersion);
        Console.WriteLine("D3DTX mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);
        Console.WriteLine("D3DTX mSamplerState = {0}", mSamplerState.ToString());
        Console.WriteLine("D3DTX mPlatform_BlockSize = {0}", mPlatform_BlockSize);
        Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), (int)mPlatform),
            mPlatform);
        Console.WriteLine("D3DTX mName Block Size = {0}", mName_BlockSize);
        Console.WriteLine("D3DTX mName = {0}", mName);
        Console.WriteLine("D3DTX mImportName Block Size = {0}", mImportName_BlockSize);
        Console.WriteLine("D3DTX mImportName = {0}", mImportName);
        Console.WriteLine("D3DTX mImportScale = {0}", mImportScale);
        Console.WriteLine("D3DTX mToolProps = {0}", mToolProps);
        Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);
        Console.WriteLine("D3DTX mWidth = {0}", mWidth);
        Console.WriteLine("D3DTX mHeight = {0}", mHeight);
        Console.WriteLine("D3DTX mDepth = {0}", mDepth);
        Console.WriteLine("D3DTX mArraySize = {0}", mArraySize);
        Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat),
            (int)mSurfaceFormat);
        Console.WriteLine("D3DTX mTextureLayout = {0} ({1})", Enum.GetName(typeof(T3TextureLayout), mTextureLayout),
            (int)mTextureLayout);
        Console.WriteLine("D3DTX mSurfaceGamma = {0} ({1})", Enum.GetName(typeof(T3SurfaceGamma), mSurfaceGamma),
            (int)mSurfaceGamma);
        Console.WriteLine("D3DTX mSurfaceMultisample = {0} ({1})",
            Enum.GetName(typeof(T3SurfaceMultisample), mSurfaceMultisample), (int)mSurfaceMultisample);
        Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), mResourceUsage),
            (int)mResourceUsage);
        Console.WriteLine("D3DTX mType = {0} ({1})", Enum.GetName(typeof(T3TextureType), mType), (int)mType);
        Console.WriteLine("D3DTX mSwizzleSize = {0}", mSwizzleSize);
        Console.WriteLine("D3DTX mSwizzle = {0}", mSwizzle);
        Console.WriteLine("D3DTX mSpecularGlossExponent = {0}", mSpecularGlossExponent);
        Console.WriteLine("D3DTX mHDRLightmapScale = {0}", mHDRLightmapScale);
        Console.WriteLine("D3DTX mToonGradientCutoff = {0}", mToonGradientCutoff);
        Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), mAlphaMode),
            (int)mAlphaMode);
        Console.WriteLine("D3DTX mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), mColorMode),
            (int)mColorMode);
        Console.WriteLine("D3DTX mUVOffset = {0}", mUVOffset);
        Console.WriteLine("D3DTX mUVScale = {0}", mUVScale);

        Console.WriteLine("----------- mArrayFrameNames -----------");
        Console.WriteLine("D3DTX mArrayFrameNames_ArrayCapacity = {0}", mArrayFrameNames_ArrayCapacity);
        Console.WriteLine("D3DTX mArrayFrameNames_ArrayLength = {0}", mArrayFrameNames_ArrayLength);
        for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
        {
            Console.WriteLine("D3DTX mArrayFrameName {0} = {1}", i, mArrayFrameNames[i]);
        }

        Console.WriteLine("----------- mToonRegions -----------");
        Console.WriteLine("D3DTX mToonRegions_ArrayCapacity = {0}", mToonRegions_ArrayCapacity);
        Console.WriteLine("D3DTX mToonRegions_ArrayLength = {0}", mToonRegions_ArrayLength);
        for (int i = 0; i < mToonRegions_ArrayLength; i++)
        {
            Console.WriteLine("D3DTX mToonRegion {0} = {1}", i, mToonRegions[i]);
        }

        Console.WriteLine("D3DTX mRegionCount = {0}", mStreamHeader.mRegionCount);
        Console.WriteLine("D3DTX mAuxDataCount = {0}", mStreamHeader.mAuxDataCount);
        Console.WriteLine("D3DTX mTotalDataSize = {0}", mStreamHeader.mTotalDataSize);

        Console.WriteLine("----------- mRegionHeaders -----------");
        for (int i = 0; i < mStreamHeader.mRegionCount; i++)
        {
            Console.WriteLine("[mRegionHeader {0}]", i);
            Console.WriteLine("D3DTX mFaceIndex = {0}", mRegionHeaders[i].mFaceIndex);
            Console.WriteLine("D3DTX mMipIndex = {0}", mRegionHeaders[i].mMipIndex);
            Console.WriteLine("D3DTX mMipCount = {0}", mRegionHeaders[i].mMipCount);
            Console.WriteLine("D3DTX mDataSize = {0}", mRegionHeaders[i].mDataSize);
            Console.WriteLine("D3DTX mPitch = {0}", mRegionHeaders[i].mPitch);
            Console.WriteLine("D3DTX mSlicePitch = {0}", mRegionHeaders[i].mSlicePitch);
        }
    }
}