/*
 * NOTE: This is now placed in an unused folder for archival purposes.
 * This class is not used in the app, however it still contains some useful info and code.

/// <summary>
/// (UNFINISHED) This is a custom class that matches what is serialized in a D3DTX [ERTM] file.
/// </summary>
public class D3DTX_ERTM
{
    public string MetaStreamVersion; //[4 bytes]
    public uint mClassNames_Length; //[4 bytes]
    public ClassNames[] mClassNames;//[12 bytes for each element]

    //----------D3DTX HEADER START----------
    public int mSamplerState_BlockSize; //[4 bytes] SAMPLER STATE BLOCK SIZE
    public T3SamplerStateBlock mSamplerState; //[4 bytes]

    public int mImportName_BlockSize; //[4 bytes]
    public uint mImportName_StringLength; //[4 bytes]
    public string mImportName; //[mImportName_StringLength bytes]

    public uint mToolProps_BlockSize; //[4 bytes]
    public uint mToolProps_mFlags; //[4 bytes] (NOT SURE)
    public ToolProps mToolProps; //[1 byte]

    public bool mbHasTextureData; //[1 byte]
    public bool mbIsMipMapped; //[1 byte]

    public uint mNumMipLevels; //[4 bytes]
    public uint mD3DFormat; //[4 bytes]
    public uint mWidth; //[4 bytes]
    public uint mHeight; //[4 bytes]

    //bunch of data here that I got no clue what is what
    //mWiiForceWidth
    //mWiiForceHeight
    //mbWiiForceUncompressed
    //mTextureDataFormats
    //mTplTexutreDataSize
    //mTplAlphaDataSize
    //mJPEGTextureDataSize
    //mHDRLightmapScale
    //public eTxAlpha mExactAlphaMode;
    //public eTxColor mColorMode;
    //public WiiTextureFormat mWiiTextureFormat;
    //mbEncrypted
    //mDetailMapBrightness
    //mNormalMapFmt
    //mbForcePreviewRebuild

    public Vector2 mUVOffset; //[8 bytes]
    public Vector2 mUVScale; //[8 bytes]
    public uint mDataSize; //[4 bytes]
    //----------D3DTX HEADER END----------

    //DDS FILE (which is literally at the end of the d3dtx header)
    public DDS_Master mInnerDDS;






    //public int mPlatform_BlockSize; //[4 bytes]
    //public PlatformType mPlatform; //[4 bytes]
    //public T3TextureType mType; //[4 bytes]
    //public eTxAlpha mAlphaMode; //[4 bytes]

    public List<byte[]> T3Texture_Data; //each image data, starts from smallest mip map to largest mip map
    public byte[] Data_OriginalHeader;

    public D3DTX_ERTM(string sourceFilePath, bool readHeaderOnly)
    {
        //read the source file into a byte array
        byte[] sourceByteFile = File.ReadAllBytes(sourceFilePath);
        int headerLength = 0;

        //which byte offset we are on (will be changed as we go through the file)
        uint bytePointerPosition = 0;

        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
        Console.WriteLine("D3DTX Total File Size = {0}", sourceByteFile.Length);

        //--------------------------Meta Stream Version-------------------------- [4 bytes]
        MetaStreamVersion = ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX Meta Stream Version = {0}", MetaStreamVersion);

        //--------------------------mClassNames_Length-------------------------- [4 bytes]
        mClassNames_Length = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mClassNames_Length = {0}", mClassNames_Length);

        //--------------------------mClassNames--------------------------
        mClassNames = new ClassNames[mClassNames_Length];

        for (int i = 0; i < mClassNames.Length; i++)
        {
            mClassNames[i] = new ClassNames()
            {
                mTypeNameCRC = new Symbol()
                {
                    mCrc64 = ByteFunctions.ReadUnsignedLong(sourceByteFile, ref bytePointerPosition)
                },
                mVersionCRC = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)
            };

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mClassName {0} = {1}", i, mClassNames[i]);
        }

        //--------------------------mSamplerState_BlockSize-------------------------- [4 bytes]
        mSamplerState_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mSamplerState Block Size = {0}", mSamplerState_BlockSize);

        //--------------------------mSamplerState-------------------------- [4 bytes]
        mSamplerState = new T3SamplerStateBlock()
        {
            mData = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)
        };

        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mSamplerState = {0}", mSamplerState.mData);

        //--------------------------mImportName Block Size-------------------------- [4 bytes] //importname block size (size + string len)
        mImportName_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mImportName Block Size = {0}", mImportName_BlockSize);

        //--------------------------mImportName String Length-------------------------- [4 bytes]
        mImportName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mImportName String Length = {0}", mImportName_StringLength);

        //--------------------------mImportName-------------------------- [mImportName_length bytes]
        mImportName = ByteFunctions.ReadFixedString(sourceByteFile, mImportName_StringLength, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mImportName = '{0}'", mImportName);

        //--------------------------mToolProps-------------------------- [4 bytes]
        mToolProps_BlockSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition); //Always 8
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX [mToolProps] mToolProps_BlockSize = {0}", mToolProps_BlockSize);

        mToolProps_mFlags = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition); //Always 0
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX [mToolProps] mToolProps_mFlags? = {0}", mToolProps_mFlags);

        //get tool props
        mToolProps = new ToolProps()
        {
            mbHasProps = ByteFunctions.GetBool(ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition) - 48) //Always 48 (0, false)
        };

        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX [mToolProps] mbHasProps = {0}", mToolProps.mbHasProps);

        //--------------------------mbHasTextureData-------------------------- [1 byte]
        mbHasTextureData = ByteFunctions.GetBool(ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition) - 48);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mbHasTextureData = {0}", mbHasTextureData);

        //--------------------------mbIsMipMapped-------------------------- [1 byte]
        mbIsMipMapped = ByteFunctions.GetBool(ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition) - 48);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mbIsMipMapped = {0}", mbIsMipMapped);

        //--------------------------mNumMipLevels-------------------------- [4 bytes]
        mNumMipLevels = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);

        //--------------------------mD3DFormat-------------------------- [4 bytes]
        string fourCC = ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
        mD3DFormat = ByteFunctions.Convert_String_To_UInt32(fourCC);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mD3DFormat = {0}", fourCC);

        //NOTE TO SELF: REMOVE LATER, THIS WORKS AS FROM THIS POINT THERE IS ALWAYS 90 BYTES UNTIL THE D3DTX HEADER ENDS
        headerLength = (int)bytePointerPosition + 90;

        //--------------------------mWidth-------------------------- [4 bytes]
        mWidth = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mWidth = {0}", mWidth);

        //--------------------------mHeight-------------------------- [4 bytes]
        mHeight = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
        Console.WriteLine("D3DTX mHeight = {0}", mHeight);







        //NOTE TO SELF: THIS BIG CHUNK OF DATA NEEDS TO STAY THE SAME SIZE AS THE VALUES AFTER THIS CHUNK ARE CORRECT
        ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);

        Console.WriteLine("D3DTX Unknown [1] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)); //6 on ui, 4 on regular/alpha/normal, 0 on lightmap
        Console.WriteLine("D3DTX Unknown [5] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX Unknown [9] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));

        Console.WriteLine("D3DTX Unknown [13] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition)); //ALWAYS 48 (THIS IS A BOOL)

        Console.WriteLine("D3DTX Unknown [14] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX mNormalMapFmt [18] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)); //33 on normal map textures, 1 on regular textures
        Console.WriteLine("D3DTX Unknown [22] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX Unknown [26] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX Unknown [30] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX mHDRLightmapScale [34] (Float) = {0}", ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition)); //1086324736 (6.0f) on non lightmap textures (and around 1.3f on lightmapped textures)

        uint mExactAlphaMode_rawValue = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition); //(2 on alpha based textures, 0 on non alpha) (ON LIGHTMAPS THIS IS 1) (on an alpha based UI messagebox this is 1)
        uint mExactAlphaMode_incremented = mExactAlphaMode_rawValue + 1;
        string mExactAlphaMode_name = Enum.GetName(typeof(eTxAlpha), mExactAlphaMode_incremented);
        Console.WriteLine("D3DTX mExactAlphaMode [38] (UINT32) = {0} ({1}) ({2})", mExactAlphaMode_rawValue, mExactAlphaMode_incremented, mExactAlphaMode_name);


        Console.WriteLine("D3DTX Unknown [42] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)); //(2 on alpha based textures, 0 on non alpha) (ON LIGHTMAPS THIS IS 1)
        Console.WriteLine("D3DTX Unknown [46] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)); //(was 2 on additive textures?) (was 2 on bmap files but not normal)
        Console.WriteLine("D3DTX Unknown [50] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));

        Console.WriteLine("D3DTX Unknown [54] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition)); //ALWAYS 48 (THIS IS A BOOL)

        Console.WriteLine("D3DTX Unknown [55] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));
        Console.WriteLine("D3DTX Unknown [59] (UINT32) = {0}", ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition));

Console.WriteLine("D3DTX Unknown [1] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [2] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [3] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [4] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [5] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [6] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [7] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [8] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [9] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [10] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [11] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [12] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [13] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [14] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [15] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [16] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [17] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [18] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [19] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [20] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [21] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [22] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [23] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [24] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [25] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [26] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [27] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [28] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [29] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [30] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [31] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [32] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [33] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [34] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [35] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [36] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [37] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [38] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [39] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [40] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [41] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [42] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [43] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [44] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [45] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [46] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [47] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [48] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [49] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [50] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [51] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [52] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [53] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [54] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [55] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [56] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [57] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [58] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [59] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [60] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [61] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));
Console.WriteLine("D3DTX Unknown [62] (Byte) = {0}", ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition));

* d3dtx
* mSamplerState [DONE]
* mImportName [DONE]
* mbHasTextureData
* mbIsMipMapped [DONE]
* mNumMipLevels [DONE]
* mD3DFormat [DONE]
* mWidth [DONE]
* mHeight [DONE]
* mWiiForceWidth
* mWiiForceHeight
* mbWiiForceUncompressed
* mTextureDataFormats
* mTplTexutreDataSize
* mTplAlphaDataSize
* mJPEGTextureDataSize
* mHDRLightmapScale
* mExactAlphaMode (enum)
* mColorMode (enum)
* mWiiTextureFormat (enum)
* mbEncrypted
* mDetailMapBrightness
* mNormalMapFmt
* mUVOffset [DONE]
* mUVScale [DONE]
* mbForcePreviewRebuild


 - FROM EXE HEX
 * mbForcePreviewRebuild
 * ���mUVScale
 * ����mUVOffset
 * ���mNormalMapFmt
 * ���mDetailMapBrightness
 * ����mbEncrypted
 * �eTxWiiFormatRGBA24
 * ��eTxWiiFormatAlphaOnly
 * ���eTxWiiFormatDefault
 * �mWiiTextureFormat
 * ���eTxColorGrayscaleAlpha
 * ��eTxColorGrayscale
 * ���eTxColorFull
 * ����eTxColorUnknown
 * �mColorMode
 * ��mExactAlphaMode
 * �eTxAlphaBlend
 * ���eTxAlphaTest
 * ����eTxNoAlpha
 * ��eTxAlphaUnkown
 * ��mHDRLightmapScale
 * ���mJPEGTextureDataSize
 * ����mTplAlphaDataSize
 * ���mTplTexutreDataSize
 * �mTextureDataFormats
 * �mbWiiForceUncompressed
 * ��mWiiForceHeight
 * �mWiiForceWidth
 * ��Generated mips
 * ��Locked size
 * �Low quality
 * �mHeight
 * �mWidth
 * ��mD3DFormat
 * ��mNumMipLevels
 * ���mbIsMipMapped
 * ���mbHasTextureData
 * ����mImportName
 * �mSamplerState
 * ���d3dtx


//--------------------------mUVOffset-------------------------- [8 bytes]
Vector2 mUVOffset = new Vector2()
{
    x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
    y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
};

this.mUVOffset = mUVOffset;
ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
Console.WriteLine("D3DTX mUVOffset = {0} {1}", mUVOffset.x, mUVOffset.y);

//--------------------------mUVScale-------------------------- [8 bytes]
Vector2 mUVScale = new Vector2()
{
    x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
    y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
};

this.mUVScale = mUVScale;
Console.WriteLine("D3DTX mUVScale = {0} {1}", mUVScale.x, mUVScale.y);

//--------------------------mDataSize-------------------------- [4 bytes]
mDataSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
Console.WriteLine("D3DTX mDataSize = {0}", mDataSize); //----------------THIS IS THE TOTAL DDS FILE SIZE

//do a quick check to see if we reached the end of the D3DTX header
ByteFunctions.ReachedOffset(bytePointerPosition, (uint)headerLength);

//--------------------------END OF D3DTX HEADER--------------------------
//--------------------------STORE D3DTX HEADER DATA--------------------------
Data_OriginalHeader = new byte[headerLength];
Array.Copy(sourceByteFile, 0, Data_OriginalHeader, 0, headerLength);
//--------------------------START OF DDS FILE (right after d3dtx header)--------------------------
int ddsFileOffsetStart = sourceByteFile.Length - (int)mDataSize;

//get the bytes of the dds file from the d3dtx
byte[] DDS_Data = ByteFunctions.AllocateBytes((int)mDataSize, sourceByteFile, bytePointerPosition);

//parse the byte data into a DDS file
mInnerDDS = new DDS_Master(DDS_Data, false);
}


public byte[] Get_Modified_D3DTX(DDS_File DDS_File, bool headerOnly)
{
byte[] NewData = null;

//||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
//||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
//||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
//--------------------------Meta Stream Keyword-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, ByteFunctions.GetBytes(mMetaStreamVersion));

//--------------------------Default Section Chunk Size-------------------------- [4 bytes] //default section chunk size (THIS IS THE SIZE OF THE FULL D3DTX HEADER MINUS THIS META STREAM HEADER)
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mDefaultSectionChunkSize));

//-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mDebugSectionChunkSize));

//--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mAsyncSectionChunkSize));

//--------------------------mClassNamesLength-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mClassNamesLength));

//--------------------------mClassNames--------------------------
for (int i = 0; i < mClassNames.Length; i++)
{
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mClassNames[i].mTypeNameCRC.mCrc64));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mClassNames[i].mVersionCRC));
}

//||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
//||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
//||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
//--------------------------mVersion-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mVersion));

//--------------------------mSamplerState Block Size-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSamplerState_BlockSize));

//--------------------------mSamplerState-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSamplerState.mData));

//--------------------------mPlatform Block Size-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mPlatform_BlockSize));

//--------------------------mPlatform-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mPlatform));

//--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mName_BlockSize));

//--------------------------mName String Length-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mName_StringLength));

//--------------------------mName-------------------------- [mName_StringLength bytes]
NewData = ByteFunctions.Combine(NewData, ByteFunctions.GetBytes(mName));

//--------------------------mImportName Block Size-------------------------- [4 bytes] //mImportName block size (size + string len)
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mImportName_BlockSize));

//--------------------------mImportName String Length-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mImportName_StringLength));

//--------------------------mImportName-------------------------- [mImportName_StringLength bytes] (this is always 0)
NewData = ByteFunctions.Combine(NewData, ByteFunctions.GetBytes(mImportName));

//--------------------------mImportScale-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mImportScale));

//--------------------------mToolProps-------------------------- (NEEDS WORK) [1 byte]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToolProps.mbHasProps));

//--------------------------mNumMipLevels-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mNumMipLevels));

//--------------------------mWidth-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mWidth));

//--------------------------mHeight-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mHeight));

//--------------------------mDepth-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mDepth));

//--------------------------mArraySize-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mArraySize));

//--------------------------mSurfaceFormat-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mSurfaceFormat));

//--------------------------mTextureLayout-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mTextureLayout));

//--------------------------mSurfaceGamma-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mSurfaceGamma));

//--------------------------mSurfaceMultisample-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mSurfaceMultisample));

//--------------------------mResourceUsage-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mResourceUsage));

//--------------------------mType-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mType));

//--------------------------mSwizzleSize-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSwizzleSize));

//--------------------------mSwizzle-------------------------- [4 bytes]
for(int i = 0; i < mSwizzleSize; i++)
{
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSwizzle.mSwizzle1));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSwizzle.mSwizzle2));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSwizzle.mSwizzle3));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSwizzle.mSwizzle4));
}

//--------------------------mSpecularGlossExponent-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mSpecularGlossExponent));

//--------------------------mHDRLightmapScale-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mHDRLightmapScale));

//--------------------------mToonGradientCutoff-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonGradientCutoff));

//--------------------------mAlphaMode-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mAlphaMode));

//--------------------------mColorMode-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes((int)mColorMode));

//--------------------------mUVOffset-------------------------- [8 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mUVOffset.x));
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mUVOffset.y));

//--------------------------mUVScale-------------------------- [8 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mUVScale.x));
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mUVScale.y));

//--------------------------mArrayFrameNames--------------------------
//--------------------------mArrayFrameNames DCArray Capacity-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mArrayFrameNames_ArrayCapacity));

//--------------------------mArrayFrameNames DCArray Length-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mArrayFrameNames_ArrayLength));

//--------------------------mArrayFrameNames DCArray--------------------------
for(int i = 0; i < mArrayFrameNames_ArrayLength; i++)
{
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mArrayFrameNames[i].mCrc64));
}

//--------------------------mToonRegions--------------------------
//--------------------------mToonRegions DCArray Capacity-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions_ArrayCapacity));

//--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions_ArrayLength));

//--------------------------mToonRegions DCArray--------------------------
for(int i = 0; i < mToonRegions_ArrayLength; i++)
{
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.r));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.g));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.b));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.a));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mSize));
}

//--------------------------StreamHeader--------------------------
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(StreamHeader.mRegionCount));
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(StreamHeader.mAuxDataCount));
NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(StreamHeader.mTotalDataSize));

//--------------------------mRegionHeaders--------------------------
for (int i = 0; i < StreamHeader.mRegionCount; i++)
{
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mFaceIndex));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mMipIndex));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mMipCount));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mDataSize));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mPitch));
    NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mSlicePitch));
}

if(headerOnly)
    return NewData;

for(int i = DDS_File.textureData.Count - 1; i >= 0; i--)
{
    NewData = ByteFunctions.Combine(NewData, DDS_File.textureData[i]);
}

return NewData;
}

}
*/
