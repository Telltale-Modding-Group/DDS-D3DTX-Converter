using System;
using System.Collections.Generic;
using System.IO;
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

/* - D3DTX Old Unknown Version games
 * Telltale Texas Hold'em  (UNTESTED)
 * Bone: Out from Boneville  (UNTESTED)
 * CSI: 3 Dimensions of Murder  (UNTESTED)
 * Bone: The Great Cow Race  (UNTESTED)
 * Sam & Max Save the World  (UNTESTED)
 * CSI: Hard Evidence  (UNTESTED)
 * Sam & Max Beyond Time and Space  (UNTESTED)
 * Strong Bad's Cool Game for Attractive People  (UNTESTED)
 * Wallace & Gromit's Grand Adventures  (UNTESTED)
 * Tales of Monkey Island  (UNTESTED)
 * CSI: Deadly Intent  (UNTESTED)
 * Sam & Max: The Devil's Playhouse  (UNTESTED)
 * Nelson Tethers: Puzzle Agent  (UNTESTED)
 * CSI: Fatal Conspiracy  (UNTESTED)
 * Poker Night at the Inventory  (UNTESTED)
 * Back to the Future: The Game  (UNTESTED)
 * Puzzle Agent 2 (UNTESTED)
*/

/* - D3DTX Legacy Version 2 game
 * Jurassic Park: The Game (UNTESTED)
*/

namespace D3DTX_Converter.TelltaleD3DTX
{
    /// <summary>
    /// This is a custom class that matches what is serialized in a D3DTX version 3? class. (INCOMPLETE)
    /// </summary>
    public class D3DTX_V2_OLD
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
        /// [1 byte] No idea.
        /// </summary>
        public byte mbEmbedMipMaps { get; set; }

        /// <summary>
        /// [4 bytes] Number of mipmaps in the texture.
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
        public uint mType { get; set; } //mTextureDataFormats?

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
        /// [4 bytes] An enum, defines what kind of alpha the texture will have.
        /// </summary>
        public eTxAlpha mAlphaMode { get; set; }

        /// <summary>
        /// [4 bytes] The Wii texture format.
        /// </summary>
        public WiiTextureFormat mWiiTextureFormat { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture encrypted.
        /// </summary>
        public byte mbAlphaHDR { get; set; }

        /// <summary>
        /// [1 byte] Whether or not the texture encrypted.
        /// </summary>
        public byte mbEncrypted { get; set; }

        /// <summary>
        /// [4 bytes] Map brightness for the Detail map type.
        /// </summary>
        public float mDetailMapBrightness { get; set; }

        /// <summary>
        /// [4 bytes] Normal map related stuff. 
        /// </summary>
        public int mNormalMapFmt { get; set; }

        /// <summary>
        /// [4 bytes] The size of the texture data.
        /// </summary>
        public uint mTextureDataSize { get; set; }

        /// <summary>
        /// [128 bytes] The DDS header of the texture.
        /// </summary>
        public DDS_HEADER mDDSHeader { get; set; }

        /// <summary>
        /// A byte array of the pixel regions in a texture. Starts from smallest mip map to largest mip map. (Since this is a pure dds, this statement could be wrong)
        /// </summary>
        public List<byte[]> mPixelData { get; set; }

        /// <summary>
        /// D3DTX V3 Header (empty constructor, only used for json deserialization)
        /// </summary>
        /// 
        public D3DTX_V2_OLD(BinaryReader reader, bool showConsole = true)
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
            mbHasTextureData = reader.ReadByte();
            mbIsMipMapped = reader.ReadByte();
            mbEmbedMipMaps = reader.ReadByte();

            mNumMipLevels = reader.ReadUInt32();
            mD3DFormat = (D3DFORMAT)reader.ReadUInt32();
            mWidth = reader.ReadUInt32();
            mHeight = reader.ReadUInt32();
            mWiiForceWidth = reader.ReadUInt32();
            mWiiForceHeight = reader.ReadUInt32();
            mbWiiForceUncompressed = reader.ReadByte();
            mType = reader.ReadUInt32(); //???
            mTextureDataFormats = reader.ReadUInt32();
            mTplTexutreDataSize = reader.ReadUInt32();
            mTplAlphaDataSize = reader.ReadUInt32();
            mJPEGTextureDataSize = reader.ReadUInt32();
            mAlphaMode = (eTxAlpha)reader.ReadInt32();

            mWiiTextureFormat = (WiiTextureFormat)reader.ReadInt32();
            mbAlphaHDR = reader.ReadByte();
            mbEncrypted = reader.ReadByte();
            mDetailMapBrightness = reader.ReadSingle();
            mNormalMapFmt = reader.ReadInt32();

            mTextureDataSize = reader.ReadUInt32();
            if (reader.ReadUInt32() != ByteFunctions.Convert_String_To_UInt32(DDS.MAGIC_WORD))
            {
                PrintConsole();
                throw new Exception("Invalid DDS Header");
            }
            mDDSHeader = new DDS_HEADER(reader);

            mPixelData = [reader.ReadBytes((int)mTextureDataSize)];

            if (showConsole)
                PrintConsole();
        }

        public void ModifyD3DTX(DDS_Master ddsMaster, DDS_DirectXTexNet_ImageSection[] sections)
        {
            DDS dds = ddsMaster.dds;
            mWidth = dds.header.dwWidth;
            mHeight = dds.header.dwHeight;
            //  mSurfaceFormat = DDS.Get_T3Format_FromFourCC(dds.header.ddspf.dwFourCC, dds);
            mNumMipLevels = dds.header.dwMipMapCount;

            mPixelData.Clear();
            mPixelData = ddsMaster.textureData;

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
            writer.Write(mbHasTextureData); //mbHasTextureData [1 byte]
            writer.Write(mbIsMipMapped); //mbIsMipMapped [1 byte]
            writer.Write(mbEmbedMipMaps); //mbEmbedMipMaps [1 byte]
            writer.Write(mNumMipLevels); //mNumMipLevels [4 bytes]
            writer.Write((int)mD3DFormat); //mD3DFormat [4 bytes]
            writer.Write(mWidth); //mWidth [4 bytes]
            writer.Write(mHeight); //mHeight [4 bytes]
            writer.Write(mWiiForceWidth); //mWiiForceWidth [4 bytes]
            writer.Write(mWiiForceHeight); //mWiiForceHeight [4 bytes]
            writer.Write(mbWiiForceUncompressed); //mbWiiForceUncompressed [1 byte]
            writer.Write(mType); //mTextureDataFormats [4 bytes]
            writer.Write(mTextureDataFormats); //mTextureDataFormats [4 bytes]
            writer.Write(mTplTexutreDataSize); //mTplTexutreDataSize [4 bytes]
            writer.Write(mTplAlphaDataSize); //mTplAlphaDataSize [4 bytes]
            writer.Write(mJPEGTextureDataSize); //mJPEGTextureDataSize [4 bytes]
            writer.Write((int)mAlphaMode); //mAlphaMode [4 bytes]
            writer.Write((int)mWiiTextureFormat); //mWiiTextureFormat [4 bytes]
            writer.Write(mbAlphaHDR); //mbAlphaHDR [1 byte]
            writer.Write(mbEncrypted); //mbEncrypted [1 byte]
            writer.Write(mDetailMapBrightness); //mDetailMapBrightness [4 bytes]
            writer.Write(mNormalMapFmt); //mNormalMapFmt [4 bytes]
            writer.Write(mTextureDataSize); //mTextureDataSize [4 bytes]
            mDDSHeader.Write(writer); //mHeader [128 bytes]

            for (int i = 0; i < mPixelData.Count; i++)
            {
                writer.Write(mPixelData[i]);
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
            totalSize += 4; //mTplTexutreDataSize [4 bytes]
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
            Console.WriteLine("||||||||||| D3DTX_v2_OLD Header |||||||||||");
            Console.WriteLine("D3DTX_v2_OLD mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);
            Console.WriteLine("D3DTX_v2_OLD mSamplerState = {0}", mSamplerState);
            Console.WriteLine("D3DTX_v2_OLD mName_BlockSize = {0}", mName_BlockSize);
            Console.WriteLine("D3DTX_v2_OLD mName = {0}", mName);
            Console.WriteLine("D3DTX_v2_OLD mImportName_BlockSize = {0}", mImportName_BlockSize);
            Console.WriteLine("D3DTX_v2_OLD mImportName = {0}", mImportName);
            Console.WriteLine("D3DTX_v2_OLD mToolProps = {0}", mToolProps);
            Console.WriteLine("D3DTX_v2_OLD mbHasTextureData = {0}", mbHasTextureData);
            Console.WriteLine("D3DTX_v2_OLD mbIsMipMapped = {0}", mbIsMipMapped);
            Console.WriteLine("D3DTX_v2_OLD mbEmbedMipMaps = {0}", mbEmbedMipMaps);
            Console.WriteLine("D3DTX_v2_OLD mNumMipLevels = {0}", mNumMipLevels);
            Console.WriteLine("D3DTX_v2_OLD mD3DFormat = {0}", mD3DFormat);
            Console.WriteLine("D3DTX_v2_OLD mWidth = {0}", mWidth);
            Console.WriteLine("D3DTX_v2_OLD mHeight = {0}", mHeight);
            Console.WriteLine("D3DTX_v2_OLD mWiiForceWidth = {0}", mWiiForceWidth);
            Console.WriteLine("D3DTX_v2_OLD mWiiForceHeight = {0}", mWiiForceHeight);
            Console.WriteLine("D3DTX_v2_OLD mbWiiForceUncompressed = {0}", mbWiiForceUncompressed);
            Console.WriteLine("D3DTX_v2_OLD mType = {0}", mType);
            Console.WriteLine("D3DTX_v2_OLD mTextureDataFormats = {0}", mTextureDataFormats);
            Console.WriteLine("D3DTX_v2_OLD mTplTexutreDataSize = {0}", mTplTexutreDataSize);
            Console.WriteLine("D3DTX_v2_OLD mTplAlphaDataSize = {0}", mTplAlphaDataSize);
            Console.WriteLine("D3DTX_v2_OLD mJPEGTextureDataSize = {0}", mJPEGTextureDataSize);
            Console.WriteLine("D3DTX_v2_OLD mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), (int)mAlphaMode), mAlphaMode);
            Console.WriteLine("D3DTX_v2_OLD mWiiTextureFormat = {0}", mWiiTextureFormat);
            Console.WriteLine("D3DTX_v2_OLD mbAlphaHDR = {0}", mbAlphaHDR);
            Console.WriteLine("D3DTX_v2_OLD mbEncrypted = {0}", mbEncrypted);
            Console.WriteLine("D3DTX_v2_OLD mDetailMapBrightness = {0}", mDetailMapBrightness);
            Console.WriteLine("D3DTX_v2_OLD mNormalMapFmt = {0}", mNormalMapFmt);
            Console.WriteLine("D3DTX_v2_OLD mTextureDataSize = {0}", mTextureDataSize);
            Console.WriteLine("D3DTX_v2_OLD mDDSHeader = {0}", mDDSHeader);
            Console.WriteLine("D3DTX_v2_OLD mPixelData = {0}", mPixelData.Count);
        }

        public string GetD3DTXInfo()
        {
            string d3dtxInfo = "";

            d3dtxInfo += "||||||||||| D3DTX_v2_OLD Header |||||||||||" + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mSamplerState_BlockSize = " + mSamplerState_BlockSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mSamplerState = " + mSamplerState + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mName_BlockSize = " + mName_BlockSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mName = " + mName + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mImportName_BlockSize = " + mImportName_BlockSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mImportName = " + mImportName + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mToolProps = " + mToolProps + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbHasTextureData = " + mbHasTextureData + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbIsMipMapped = " + mbIsMipMapped + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbEmbedMipMaps = " + mbEmbedMipMaps + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mNumMipLevels = " + mNumMipLevels + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mD3DFormat = " + mD3DFormat + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mWidth = " + mWidth + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mHeight = " + mHeight + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mWiiForceWidth = " + mWiiForceWidth + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mWiiForceHeight = " + mWiiForceHeight + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbWiiForceUncompressed = " + mbWiiForceUncompressed + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mType = " + mType + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mTextureDataFormats = " + mTextureDataFormats + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mTplTexutreDataSize = " + mTplTexutreDataSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mTplAlphaDataSize = " + mTplAlphaDataSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mJPEGTextureDataSize = " + mJPEGTextureDataSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mAlphaMode = " + Enum.GetName(typeof(eTxAlpha), (int)mAlphaMode) + " (" + mAlphaMode + ")" + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mWiiTextureFormat = " + mWiiTextureFormat + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbAlphaHDR = " + mbAlphaHDR + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mbEncrypted = " + mbEncrypted + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mDetailMapBrightness = " + mDetailMapBrightness + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mNormalMapFmt = " + mNormalMapFmt + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mTextureDataSize = " + mTextureDataSize + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mDDSHeader = " + mDDSHeader + Environment.NewLine;
            d3dtxInfo += "D3DTX_v2_OLD mPixelData = " + mPixelData.Count + Environment.NewLine;

            return d3dtxInfo;
        }
    }
}