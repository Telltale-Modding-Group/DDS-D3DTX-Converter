using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using D3DTX_TextureConverter.Telltale;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;
using D3DTX_TextureConverter.Main;

/*
 * NOTE: This is now placed in an unused folder for archival purposes.
 * This class is not used in the app, however it still contains some useful info and code.
*/

namespace D3DTX_TextureConverter.Unused
{
    /*
    /// <summary>
    /// This is a custom class that matches what is serialized in a D3DTX [6VSM] file.
    /// </summary>
    public class D3DTX_6VSM
    {
        //meta header
        public string mMetaStreamVersion { get; set; } //[4 bytes]
        public uint mDefaultSectionChunkSize { get; set; } //[4 bytes] size of the d3dtx header after the meta header
        public uint mDebugSectionChunkSize { get; set; } //[4 bytes] (always 0)
        public uint mAsyncSectionChunkSize { get; set; } //[4 bytes] size of texture data itself (not the header)
        public uint mClassNamesLength { get; set; } //[4 bytes]
        public ClassNames[] mClassNames { get; set; } //[12 bytes for each element]

        //d3dtx header
        public int mVersion { get; set; } //[4 bytes]
        public int mSamplerState_BlockSize { get; set; } //[4 bytes] (always 8)
        public T3SamplerStateBlock mSamplerState { get; set; } //[4 bytes]
        public int mPlatform_BlockSize { get; set; } //[4 bytes] (always 8)
        public PlatformType mPlatform { get; set; } //[4 bytes]
        public int mName_BlockSize { get; set; } //[4 bytes]
        public uint mName_StringLength { get; set; } //[4 bytes]
        public string mName { get; set; } //[mName_StringLength bytes]
        public int mImportName_BlockSize { get; set; } //[4 bytes]
        public uint mImportName_StringLength { get; set; } //[4 bytes]
        public string mImportName { get; set; } //[mImportName_StringLength bytes]
        public float mImportScale { get; set; } //[4 bytes]
        public ToolProps mToolProps { get; set; } //[1 byte]
        public uint mNumMipLevels { get; set; } //[4 bytes]
        public uint mWidth { get; set; } //[4 bytes]
        public uint mHeight { get; set; } //[4 bytes]
        public uint mDepth { get; set; } //[4 bytes]
        public uint mArraySize { get; set; } //[4 bytes]
        public T3SurfaceFormat mSurfaceFormat { get; set; } //[4 bytes]
        public T3TextureLayout mTextureLayout { get; set; } //[4 bytes]
        public T3SurfaceGamma mSurfaceGamma { get; set; } //[4 bytes]
        public T3SurfaceMultisample mSurfaceMultisample { get; set; } //[4 bytes]
        public T3ResourceUsage mResourceUsage { get; set; } //[4 bytes]
        public T3TextureType mType { get; set; } //[4 bytes]
        public int mSwizzleSize { get; set; } //[4 bytes]
        public RenderSwizzleParams mSwizzle { get; set; } //[4 bytes]
        public float mSpecularGlossExponent { get; set; } //[4 bytes]
        public float mHDRLightmapScale { get; set; } //[4 bytes]
        public float mToonGradientCutoff { get; set; } //[4 bytes]
        public eTxAlpha mAlphaMode { get; set; } //[4 bytes]
        public eTxColor mColorMode { get; set; } //[4 bytes]
        public Vector2 mUVOffset { get; set; } //[8 bytes]
        public Vector2 mUVScale { get; set; } //[8 bytes]
        public uint mArrayFrameNames_ArrayCapacity { get; set; } //[4 bytes]
        public int mArrayFrameNames_ArrayLength { get; set; } //[4 bytes]
        public List<Symbol> mArrayFrameNames { get; set; } //(varies, each element is 8 bytes long)
        public uint mToonRegions_ArrayCapacity { get; set; } //[4 bytes]
        public int mToonRegions_ArrayLength { get; set; } //[4 bytes]
        public T3ToonGradientRegion[] mToonRegions { get; set; } //(varies, each element is 16 bytes long)
        public StreamHeader mStreamHeader { get; set; } // [12 bytes]
        public RegionStreamHeader[] mRegionHeaders { get; set; } //(varies, each element is 24 bytes long)

        //d3dtx byte data
        public List<byte[]> mPixelData { get; set; } //each image data, starts from smallest mip map to largest mip map

        public D3DTX_6VSM()
        {

        }

        public D3DTX_6VSM(string sourceFilePath)
        {
            //read the source file into a byte array
            byte[] sourceByteFile = File.ReadAllBytes(sourceFilePath);
            int calculated_HeaderLength = 0;

            //which byte offset we are on (will be changed as we go through the file)
            uint bytePointerPosition = 0;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("D3DTX Total File Size = {0}", sourceByteFile.Length);

            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| Meta Header |||||||||||");
            //--------------------------Meta Stream Keyword-------------------------- [4 bytes]
            mMetaStreamVersion = ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Meta Stream Keyword = {0}", mMetaStreamVersion);

            //--------------------------Default Section Chunk Size-------------------------- [4 bytes] //default section chunk size (THIS IS THE SIZE OF THE FULL D3DTX HEADER MINUS THIS META STREAM HEADER)
            mDefaultSectionChunkSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX Default Section Chunk Size = {0}", mDefaultSectionChunkSize);

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
            mDebugSectionChunkSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX Debug Section Chunk Size = {0}", mDebugSectionChunkSize);

            //--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
            mAsyncSectionChunkSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX Async Section Chunk Size = {0}", mAsyncSectionChunkSize);

            //if the 'parsed' texture byte size in the file is actually supposedly bigger than the file itself
            if (mAsyncSectionChunkSize > sourceByteFile.Length)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("Can't continue reading the file because the values we are reading are incorrect! This can be due to the byte data being shifted in the file or non-existant, and this is likley because the file version has changed.");

                return; //don't continue
            }

            //--------------------------CALCULATING HEADER LENGTH--------------------------
            calculated_HeaderLength = sourceByteFile.Length - (int)mAsyncSectionChunkSize;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("D3DTX (Calculated) FULL Header Size = {0}", calculated_HeaderLength);
            Console.WriteLine("D3DTX Meta Header Size = {0}", calculated_HeaderLength - mDefaultSectionChunkSize);
            Console.WriteLine("D3DTX D3DTX Header Size = {0}", mDefaultSectionChunkSize);

            //--------------------------mClassNamesLength-------------------------- [4 bytes]
            mClassNamesLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mClassNamesLength = {0}", mClassNamesLength);

            //--------------------------mClassNames--------------------------
            mClassNames = new ClassNames[mClassNamesLength];

            for(int i = 0; i < mClassNames.Length; i++)
            {
                mClassNames[i] = new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = ByteFunctions.ReadUnsignedLong(sourceByteFile, ref bytePointerPosition)
                    },
                    mVersionCRC = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)
                };

                Console.WriteLine("D3DTX mClassName {0} = {1}", i, mClassNames[i]);
            }

            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| D3DTX Header |||||||||||");

            //--------------------------mVersion-------------------------- [4 bytes]
            mVersion = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mVersion = {0}", mVersion);

            //--------------------------mSamplerState Block Size-------------------------- [4 bytes]
            mSamplerState_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);

            //--------------------------mSamplerState-------------------------- [4 bytes]
            mSamplerState = new T3SamplerStateBlock()
            {
                mData = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)
            };

            Console.WriteLine("D3DTX mSamplerState = {0}", mSamplerState.mData);

            //--------------------------mPlatform Block Size-------------------------- [4 bytes]
            mPlatform_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mPlatform_BlockSize = {0}", mPlatform_BlockSize);

            //--------------------------mPlatform-------------------------- [4 bytes]
            mPlatform = EnumPlatformType.GetPlatformType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), (int)mPlatform), mPlatform);

            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            mName_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName Block Size = {0}", mName_BlockSize);

            //--------------------------mName String Length-------------------------- [4 bytes]
            mName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName String Length = {0}", mName_StringLength);

            //--------------------------mName-------------------------- [mName_StringLength bytes]
            mName = ByteFunctions.ReadFixedString(sourceByteFile, mName_StringLength, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName = {0}", mName);

            //--------------------------mImportName Block Size-------------------------- [4 bytes] //mImportName block size (size + string len)
            mImportName_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName Block Size = {0}", mImportName_BlockSize);

            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName String Length = {0}", mImportName_StringLength);

            //--------------------------mImportName-------------------------- [mImportName_StringLength bytes] (this is always 0)
            mImportName = ByteFunctions.ReadFixedString(sourceByteFile, mImportName_StringLength, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName = {0}", mImportName);

            //--------------------------mImportScale-------------------------- [4 bytes]
            mImportScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportScale = {0}", mImportScale);

            //--------------------------mToolProps-------------------------- (NEEDS WORK) [1 byte]
            //get tool props
            ToolProps toolProps = new ToolProps()
            {
                mbHasProps = ByteFunctions.GetBool(ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition) - 48)
            };

            mToolProps = toolProps;
            Console.WriteLine("D3DTX mToolProps = {0}", mToolProps);

            //--------------------------mNumMipLevels-------------------------- [4 bytes]
            mNumMipLevels = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);

            //--------------------------mWidth-------------------------- [4 bytes]
            mWidth = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mWidth = {0}", mWidth);

            //--------------------------mHeight-------------------------- [4 bytes]
            mHeight = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mHeight = {0}", mHeight);

            //--------------------------mDepth-------------------------- [4 bytes]
            mDepth = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mDepth = {0}", mDepth);

            //--------------------------mArraySize-------------------------- [4 bytes]
            mArraySize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mArraySize = {0}", mArraySize);

            //--------------------------mSurfaceFormat-------------------------- [4 bytes]
            mSurfaceFormat = T3TextureBase_Functions.GetSurfaceFormat(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat), (int)mSurfaceFormat);

            //--------------------------mTextureLayout-------------------------- [4 bytes]
            mTextureLayout = T3TextureBase_Functions.GetTextureLayout(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mTextureLayout = {0} ({1})", Enum.GetName(typeof(T3TextureLayout), mTextureLayout), (int)mTextureLayout);

            //--------------------------mSurfaceGamma-------------------------- [4 bytes]
            mSurfaceGamma = T3TextureBase_Functions.GetSurfaceGamma(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mSurfaceGamma = {0} ({1})", Enum.GetName(typeof(T3SurfaceGamma), mSurfaceGamma), (int)mSurfaceGamma);

            //--------------------------mSurfaceMultisample-------------------------- [4 bytes]
            mSurfaceMultisample = T3TextureBase_Functions.GetSurfaceMultisample(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mSurfaceMultisample = {0} ({1})", Enum.GetName(typeof(T3SurfaceMultisample), mSurfaceMultisample), (int)mSurfaceMultisample);

            //--------------------------mResourceUsage-------------------------- [4 bytes]
            mResourceUsage = T3TextureBase_Functions.GetResourceUsage(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), mResourceUsage), (int)mResourceUsage);

            //--------------------------mType-------------------------- [4 bytes]
            mType = T3Texture_Functions.GetTextureType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mType = {0} ({1})", Enum.GetName(typeof(T3TextureType), mType), (int)mType);

            //--------------------------mSwizzleSize-------------------------- [4 bytes]
            mSwizzleSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mSwizzleSize = {0}", Enum.GetName(typeof(T3TextureType), mType), (int)mType);

            //--------------------------mSwizzle-------------------------- [4 bytes]
            RenderSwizzleParams mSwizzle = new RenderSwizzleParams()
            {
                mSwizzle1 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle2 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle3 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle4 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition)
            };

            this.mSwizzle = mSwizzle;
            Console.WriteLine("D3DTX mSwizzle = {0}", mSwizzle);

            //--------------------------mSpecularGlossExponent-------------------------- [4 bytes]
            mSpecularGlossExponent = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mSpecularGlossExponent = {0}", mSpecularGlossExponent);

            //--------------------------mHDRLightmapScale-------------------------- [4 bytes]
            mHDRLightmapScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mHDRLightmapScale = {0}", mHDRLightmapScale);

            //--------------------------mToonGradientCutoff-------------------------- [4 bytes]
            mToonGradientCutoff = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mToonGradientCutoff = {0}", mToonGradientCutoff);

            //--------------------------mAlphaMode-------------------------- [4 bytes]
            mAlphaMode = T3Texture_Functions.GetAlphaMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), mAlphaMode), (int)mAlphaMode);

            //--------------------------mColorMode-------------------------- [4 bytes]
            mColorMode = T3Texture_Functions.GetColorMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            Console.WriteLine("D3DTX mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), mColorMode), (int)mColorMode);

            //--------------------------mUVOffset-------------------------- [8 bytes]
            Vector2 mUVOffset = new Vector2()
            {
                x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
            };

            this.mUVOffset = mUVOffset;
            Console.WriteLine("D3DTX mUVOffset = {0} {1}", mUVOffset.x, mUVOffset.y);

            //--------------------------mUVScale-------------------------- [8 bytes]
            Vector2 mUVScale = new Vector2()
            {
                x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
            };

            this.mUVScale = mUVScale;
            Console.WriteLine("D3DTX mUVScale = {0} {1}", mUVScale.x, mUVScale.y);

            //--------------------------mArrayFrameNames--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mArrayFrameNames -----------");
            //--------------------------mArrayFrameNames DCArray Capacity-------------------------- [4 bytes]
            uint bytePointerPostion_before_mArrayFrameNames = bytePointerPosition;
            mArrayFrameNames_ArrayCapacity = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayCapacity = {0}", mArrayFrameNames_ArrayCapacity);

            //--------------------------mArrayFrameNames DCArray Length-------------------------- [4 bytes]
            mArrayFrameNames_ArrayLength = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition); //ADD 1 BECAUSE COUNTING STARTS AT 0
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayLength = {0}", mArrayFrameNames_ArrayLength);

            //--------------------------mArrayFrameNames DCArray--------------------------
            //NOTE: According to meta function, this is a DCArray<Symbol>
            mArrayFrameNames = new List<Symbol>();

            for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
            {
                Symbol newSymbol = new Symbol()
                {
                    mCrc64 = ByteFunctions.ReadUnsignedLong(sourceByteFile, ref bytePointerPosition) //[8 bytes]
                };

                mArrayFrameNames.Add(newSymbol);
            }

            //check if we are at the offset we should be after going through the array
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPostion_before_mArrayFrameNames, mArrayFrameNames_ArrayCapacity, ref bytePointerPosition);

            //--------------------------mToonRegions--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mToonRegions -----------");
            //--------------------------mToonRegions DCArray Capacity-------------------------- [4 bytes]
            uint bytePointerPostion_before_mToonRegions = bytePointerPosition;
            mToonRegions_ArrayCapacity = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayCapacity = {0}", mToonRegions_ArrayCapacity);

            //--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
            mToonRegions_ArrayLength = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mToonRegions_ArrayLength = {0}", mToonRegions_ArrayLength);

            //--------------------------mToonRegions DCArray--------------------------
            mToonRegions = new T3ToonGradientRegion[mToonRegions_ArrayLength];

            for (int i = 0; i < mToonRegions_ArrayLength; i++)
            {
                mToonRegions[i] = new T3ToonGradientRegion()
                {
                    mColor = new Color()
                    {
                        r = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                        g = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                        b = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                        a = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
                    },

                    mSize = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
                };

                Console.WriteLine("D3DTX mToonRegion {0} = {1}", i, mToonRegions[i]);
            }

            //check if we are at the offset we should be after going through the array
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPostion_before_mToonRegions, mToonRegions_ArrayCapacity, ref bytePointerPosition);

            //--------------------------StreamHeader--------------------------
            mStreamHeader = new StreamHeader()
            {
                mRegionCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                mAuxDataCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                mTotalDataSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition) //[4 bytes]
            };

            Console.WriteLine("D3DTX mRegionCount = {0}", mStreamHeader.mRegionCount);
            Console.WriteLine("D3DTX mAuxDataCount {0}", mStreamHeader.mAuxDataCount);
            Console.WriteLine("D3DTX mTotalDataSize {0}", mStreamHeader.mTotalDataSize);

            //--------------------------mRegionHeaders--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mRegionHeaders -----------");
            mRegionHeaders = new RegionStreamHeader[mStreamHeader.mRegionCount];
            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                if (bytePointerPosition > calculated_HeaderLength)
                {
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                    Console.WriteLine("Pointer position is beyond the header length!");
                    Console.ResetColor();
                    break;
                }

                mRegionHeaders[i] = new RegionStreamHeader()
                {
                    mFaceIndex = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mMipIndex = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes] 
                    mMipCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mDataSize = (uint)ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mPitch = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mSlicePitch = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition) //[4 bytes]
                };

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
                Console.WriteLine("[mRegionHeader {0}]", i);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                Console.WriteLine("D3DTX mFaceIndex = {0}", mRegionHeaders[i].mFaceIndex);
                Console.WriteLine("D3DTX mMipIndex = {0}", mRegionHeaders[i].mMipIndex);
                Console.WriteLine("D3DTX mMipCount = {0}", mRegionHeaders[i].mMipCount);
                Console.WriteLine("D3DTX mDataSize = {0}", mRegionHeaders[i].mDataSize);
                Console.WriteLine("D3DTX mPitch = {0}", mRegionHeaders[i].mPitch);
                Console.WriteLine("D3DTX mSlicePitch = {0}", mRegionHeaders[i].mSlicePitch);
            }

            //do a quick check to see if we reached the end of the file header
            ByteFunctions.ReachedOffset(bytePointerPosition, (uint)calculated_HeaderLength);
            //-----------------------------------------D3DTX HEADER END-----------------------------------------

            //--------------------------STORING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx header data...");

            if (calculated_HeaderLength < 0)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR, we were off with calculating header length! {0}", calculated_HeaderLength);

                return;
            }

            //--------------------------STORING D3DTX IMAGE DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx image data...");

            mPixelData = new List<byte[]>();

            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                int dataSize = (int)mRegionHeaders[i].mDataSize;
                byte[] imageData = ByteFunctions.AllocateBytes(dataSize, sourceByteFile, bytePointerPosition);

                bytePointerPosition += (uint)dataSize;

                mPixelData.Add(imageData);
            }

            //do a quick check to see if we reached the end of the file
            ByteFunctions.ReachedEndOfFile(bytePointerPosition, (uint)sourceByteFile.Length);
        }

        public void WriteD3DTX(string destinationPath)
        {
            byte[] NewData = new byte[0];

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
            for (int i = 0; i < mSwizzleSize; i++)
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
            for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
            {
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mArrayFrameNames[i].mCrc64));
            }

            //--------------------------mToonRegions--------------------------
            //--------------------------mToonRegions DCArray Capacity-------------------------- [4 bytes]
            NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions_ArrayCapacity));

            //--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
            NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions_ArrayLength));

            //--------------------------mToonRegions DCArray--------------------------
            for (int i = 0; i < mToonRegions_ArrayLength; i++)
            {
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.r));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.g));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.b));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mColor.a));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mToonRegions[i].mSize));
            }

            //--------------------------StreamHeader--------------------------
            NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mStreamHeader.mRegionCount));
            NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mStreamHeader.mAuxDataCount));
            NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mStreamHeader.mTotalDataSize));

            //--------------------------mRegionHeaders--------------------------
            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mFaceIndex));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mMipIndex));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mMipCount));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mDataSize));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mPitch));
                NewData = ByteFunctions.Combine(NewData, BitConverter.GetBytes(mRegionHeaders[i].mSlicePitch));
            }

            //--------------------------mPixelData--------------------------
            for (int i = 0; i < mPixelData.Count; i++)
            {
                NewData = ByteFunctions.Combine(NewData, mPixelData[i]);
            }

            //write the final to disk
            File.WriteAllBytes(destinationPath, NewData);
        }

        public void ModifyD3DTX(DDS_Master dds)
        {
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| META HEADER |||||||||||||||||||||||||||||||||||||||||
            //--------------------------Default Section Chunk Size-------------------------- [4 bytes] //default section chunk size (THIS IS THE SIZE OF THE FULL D3DTX HEADER MINUS THIS META STREAM HEADER)
            //NOTE TO SELF: I know this is inefficent as hell, and I definetly plan to simplify it but I do it just so I can keep track of the values correctly
            mDefaultSectionChunkSize = 0;
            mDefaultSectionChunkSize += 4; //mVersion
            mDefaultSectionChunkSize += 4; //mSamplerState Block Size
            mDefaultSectionChunkSize += 4; //mSamplerState
            mDefaultSectionChunkSize += 4; //mPlatform Block Size
            mDefaultSectionChunkSize += 4; //mPlatform
            mDefaultSectionChunkSize += 4; //mName Block Size
            mDefaultSectionChunkSize += 4; //mName String Length
            mDefaultSectionChunkSize += (uint)mName.Length;
            mDefaultSectionChunkSize += 4; //mImportName Block Size
            mDefaultSectionChunkSize += 4; //mImportName String Length
            mDefaultSectionChunkSize += (uint)mImportName.Length;
            mDefaultSectionChunkSize += 4; //mImportScale
            mDefaultSectionChunkSize += 1; //mToolProps
            mDefaultSectionChunkSize += 4; //mNumMipLevels
            mDefaultSectionChunkSize += 4; //mWidth
            mDefaultSectionChunkSize += 4; //mHeight
            mDefaultSectionChunkSize += 4; //mDepth
            mDefaultSectionChunkSize += 4; //mArraySize
            mDefaultSectionChunkSize += 4; //mSurfaceFormat
            mDefaultSectionChunkSize += 4; //mTextureLayout
            mDefaultSectionChunkSize += 4; //mSurfaceGamma
            mDefaultSectionChunkSize += 4; //mSurfaceMultisample
            mDefaultSectionChunkSize += 4; //mResourceUsage
            mDefaultSectionChunkSize += 4; //mType
            mDefaultSectionChunkSize += 4; //mSwizzleSize
            mDefaultSectionChunkSize += 4; //mSwizzle
            mDefaultSectionChunkSize += 4; //mSpecularGlossExponent
            mDefaultSectionChunkSize += 4; //mHDRLightmapScale
            mDefaultSectionChunkSize += 4; //mToonGradientCutoff
            mDefaultSectionChunkSize += 4; //mAlphaMode
            mDefaultSectionChunkSize += 4; //mColorMode
            mDefaultSectionChunkSize += 8; //mUVOffset
            mDefaultSectionChunkSize += 8; //mUVScale
            mDefaultSectionChunkSize += 4; //mArrayFrameNames DCArray Capacity
            mDefaultSectionChunkSize += 4; //mArrayFrameNames DCArray Length
            mDefaultSectionChunkSize += mArrayFrameNames_ArrayCapacity; //mArrayFrameNames DCArray
            mDefaultSectionChunkSize += 4; //mToonRegions DCArray Capacity
            mDefaultSectionChunkSize += 4; //mToonRegions DCArray Length
            mDefaultSectionChunkSize += mToonRegions_ArrayCapacity; //mToonRegions DCArray
            mDefaultSectionChunkSize += 4; //StreamHeader mRegionCount
            mDefaultSectionChunkSize += 4; //StreamHeader mAuxDataCount
            mDefaultSectionChunkSize += 4; //StreamHeader mTotalDataSize

            for(int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                mDefaultSectionChunkSize += 4; //mFaceIndex
                mDefaultSectionChunkSize += 4; //mMipIndex
                mDefaultSectionChunkSize += 4; //mMipCount
                mDefaultSectionChunkSize += 4; //mDataSize
                mDefaultSectionChunkSize += 4; //mPitch
                mDefaultSectionChunkSize += 4; //mSlicePitch
            }

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
            mDebugSectionChunkSize = 0;
            //--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
            mAsyncSectionChunkSize = 0;

            foreach (byte[] dds_textureChunk in dds.textureData)
            {
                mAsyncSectionChunkSize += (uint)dds_textureChunk.Length;
            }
            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            //||||||||||||||||||||||||||||||||||||||||| D3DTX HEADER |||||||||||||||||||||||||||||||||||||||||
            //--------------------------mSamplerState Block Size-------------------------- [4 bytes]
            mSamplerState_BlockSize = 8;
            //--------------------------mSamplerState-------------------------- [4 bytes]
            //--------------------------mPlatform Block Size-------------------------- [4 bytes]
            mPlatform_BlockSize = 8;
            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            mName_BlockSize = mName.Length + 8;
            //--------------------------mName String Length-------------------------- [4 bytes]
            mName_StringLength = (uint)mName.Length;
            //--------------------------mImportName Block Size-------------------------- [4 bytes] //mImportName block size (size + string len)
            mImportName_BlockSize = mImportName.Length + 8;
            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportName_StringLength = (uint)mImportName.Length;
            //--------------------------mImportName-------------------------- [mImportName_StringLength bytes] (this is always 0)
            //--------------------------mImportScale-------------------------- [4 bytes]
            //--------------------------mToolProps-------------------------- [1 byte]
            //--------------------------mNumMipLevels-------------------------- [4 bytes]
            mNumMipLevels = dds.header.dwMipMapCount + 1;
            //--------------------------mWidth-------------------------- [4 bytes]
            mWidth = dds.header.dwWidth;
            //--------------------------mHeight-------------------------- [4 bytes]
            mHeight = dds.header.dwHeight;
            //--------------------------mDepth-------------------------- [4 bytes]
            mDepth = 1 + dds.header.dwDepth; //DOUBLE CHECK but even on regular texture this seems to be 1
            //--------------------------mArraySize-------------------------- [4 bytes]
            mArraySize = 1;
            //--------------------------mSurfaceFormat-------------------------- [4 bytes] 
            mSurfaceFormat = DDS_Master.Get_T3Format_FromFourCC(dds.header.ddspf.dwFourCC);
            //--------------------------mTextureLayout-------------------------- [4 bytes]

            //NEEDS WORK HERE

            //--------------------------mSurfaceGamma-------------------------- [4 bytes]
            //--------------------------mSurfaceMultisample-------------------------- [4 bytes]
            //--------------------------mResourceUsage-------------------------- [4 bytes]
            //--------------------------mType-------------------------- [4 bytes]
            //--------------------------mSwizzleSize-------------------------- [4 bytes]
            //--------------------------mSwizzle-------------------------- [4 bytes]
            //--------------------------mSpecularGlossExponent-------------------------- [4 bytes]
            //--------------------------mHDRLightmapScale-------------------------- [4 bytes]
            //--------------------------mToonGradientCutoff-------------------------- [4 bytes]
            //--------------------------mAlphaMode-------------------------- [4 bytes]
            //--------------------------mColorMode-------------------------- [4 bytes]
            //--------------------------mUVOffset-------------------------- [8 bytes]
            //--------------------------mUVScale-------------------------- [8 bytes]
            //--------------------------mArrayFrameNames DCArray Capacity-------------------------- [4 bytes]
            //--------------------------mArrayFrameNames DCArray Length-------------------------- [4 bytes]
            //--------------------------mArrayFrameNames DCArray--------------------------
            //--------------------------mToonRegions DCArray Capacity-------------------------- [4 bytes]
            //--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
            //--------------------------mToonRegions DCArray--------------------------
            //--------------------------StreamHeader--------------------------
            mStreamHeader = new StreamHeader()
            {
                mRegionCount = (int)dds.header.dwMipMapCount + 1, //[4 bytes]
                mAuxDataCount = 0, //[4 bytes]
                mTotalDataSize = (int)mAsyncSectionChunkSize //[4 bytes]
            };

            //--------------------------mRegionHeaders--------------------------
            mRegionHeaders = new RegionStreamHeader[mStreamHeader.mRegionCount];

            for(int i = 0; i < mRegionHeaders.Length; i++)
            {
                int reverseIndex = (mRegionHeaders.Length - 1) - i;

                mRegionHeaders[i] = new RegionStreamHeader()
                {
                    mDataSize = (uint)dds.textureData[reverseIndex].Length,
                    mFaceIndex = 0, //seems to be zero most of the time
                    mMipCount = 1, //seems to be 1 most of the time
                    mMipIndex = (mRegionHeaders.Length - 1) - i, 
                    mPitch = DDS_Functions.DDS_ComputePitchValue(dds.mipMapResolutions[reverseIndex, 0], DDS_Functions.DDS_CompressionBool(dds.header)),
                    mSlicePitch = dds.textureData[reverseIndex].Length
                };
            }
            //--------------------------Texture Data--------------------------
            mPixelData = new List<byte[]>();

            //add the DDS data in reverse
            for(int i = mRegionHeaders.Length - 1; i >= 0; i--)
            {
                mPixelData.Add(dds.textureData[i]);
            }
        }

        /// <summary>
        /// Set 6VSM Class Names data
        /// </summary>
        public void GenerateClassNames()
        {
            //7 since there are always 7 class names in a 6VSM header
            mClassNamesLength = 7;

            //7 preassigned classname elements that are the exact same in a 6VSM header
            mClassNames = new ClassNames[7]
            {
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 4477878195382176994
                    },
                    mVersionCRC = 2804984673
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 10628316211484133603
                    },
                    mVersionCRC = 3001274032
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 10270735715830946188
                    },
                    mVersionCRC = 2762767791
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 18044947931230812794
                    },
                    mVersionCRC = 1531463941
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 8916179779089013255
                    },
                    mVersionCRC = 3785346050
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 17267287096777831439
                    },
                    mVersionCRC = 2599746624
                },
                new ClassNames()
                {
                    mTypeNameCRC = new Symbol()
                    {
                        mCrc64 = 6360800049507602154
                    },
                    mVersionCRC = 1347938913
                },
            };
        }
    }
    */
}
