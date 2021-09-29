using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using D3DTX_TextureConverter.Telltale;
using D3DTX_TextureConverter.Utilities;

namespace D3DTX_TextureConverter
{
    /// <summary>
    /// This is a custom class that actually matches what is inside a D3DTX [5VSM] file (this exact struct doesn't exist within telltale).
    /// </summary>
    public class D3DTX_5VSM
    {
        //meta header
        public string mMetaStreamVersion; //[4 bytes]
        public uint mDefaultSectionChunkSize; //[4 bytes]
        public uint mDebugSectionChunkSize; //[4 bytes]
        public uint mAsyncSectionChunkSize; //[4 bytes]
        public uint mClassNamesLength; //[4 bytes]
        public ClassNames[] mClassNames;//[12 bytes for each element]

        //d3dtx header
        public int mVersion; //[4 bytes]
        public int mSamplerState_BlockSize; //[4 bytes]
        public T3SamplerStateBlock mSamplerState; //[4 bytes]
        public int mPlatform_BlockSize; //[4 bytes]
        public PlatformType mPlatform; //[4 bytes]
        public int mName_BlockSize; //[4 bytes]
        public uint mName_StringLength; //[4 bytes]
        public string mName; //[mName_StringLength bytes]
        public int mImportName_BlockSize; //[4 bytes]
        public uint mImportName_StringLength; //[4 bytes]
        public string mImportName; //[mImportName_StringLength bytes]
        public float mImportScale; //[4 bytes]
        public ToolProps mToolProps; //[1 byte]
        public uint mNumMipLevels; //[4 bytes]
        public uint mWidth; //[4 bytes]
        public uint mHeight; //[4 bytes]
        public T3SurfaceFormat mSurfaceFormat; //[4 bytes]
        public T3ResourceUsage mResourceUsage; //[4 bytes]
        public T3TextureType mType; //[4 bytes]
        public int mNormalMapFormat; //[4 bytes]
        public float mHDRLightmapScale; //[4 bytes]
        public float mToonGradientCutoff; //[4 bytes]
        public eTxAlpha mAlphaMode; //[4 bytes]
        public eTxColor mColorMode; //[4 bytes]
        public Vector2 mUVOffset; //[8 bytes]
        public Vector2 mUVScale; //[8 bytes]
        public uint mToonRegions_ArrayCapacity; //[4 bytes]
        public int mToonRegions_ArrayLength; //[4 bytes]
        public List<T3ToonGradientRegion> mToonRegions; //(varies, each element is 16 bytes long)
        public StreamHeader StreamHeader; // [12 bytes]
        public List<RegionStreamHeader> mRegionHeaders; //(varies, each element is 24 bytes long)
        public List<byte[]> T3Texture_Data; //each image data, starts from smallest mip map to largest mip map

        public byte[] Data_OriginalBytes;
        public byte[] Data_OriginalHeader;

        public D3DTX_5VSM(string sourceFilePath, bool readHeaderOnly)
        {
            //read the source file into a byte array
            byte[] sourceByteFile = File.ReadAllBytes(sourceFilePath);
            byte[] headerData = new byte[0];
            int calculated_HeaderLength = 0;

            Data_OriginalBytes = sourceByteFile;

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
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Default Section Chunk Size = {0}", mDefaultSectionChunkSize);

            //-------------------------Debug Section Chunk Size-------------------------- [4 bytes] //debug section chunk size (always zero)
            mDebugSectionChunkSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Debug Section Chunk Size = {0}", mDebugSectionChunkSize);

            //--------------------------Async Section Chunk Size-------------------------- [4 bytes] //async section chunk size (size of the bytes after the file header)
            mAsyncSectionChunkSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Async Section Chunk Size = {0}", mAsyncSectionChunkSize);

            //if the 'parsed' texture byte size in the file is actually supposedly bigger than the file itself
            if (mAsyncSectionChunkSize > sourceByteFile.Length && !readHeaderOnly)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("Can't continue reading the file because the values we are reading are incorrect! This can be due to the byte data being shifted in the file or non-existant, and this is likley because the file version has changed.");

                return; //don't continue
            }

            //--------------------------CALCULATING HEADER LENGTH--------------------------
            if (readHeaderOnly)
                calculated_HeaderLength = sourceByteFile.Length;
            else
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
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);

            //--------------------------mSamplerState-------------------------- [4 bytes]
            mSamplerState = new T3SamplerStateBlock()
            {
                mData = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition)
            };

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSamplerState = {0}", mSamplerState.mData);

            //--------------------------mPlatform Block Size-------------------------- [4 bytes]
            mPlatform_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mPlatform_BlockSize = {0}", mPlatform_BlockSize);

            //--------------------------mPlatform-------------------------- [4 bytes]
            mPlatform = EnumPlatformType.GetPlatformType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), (int)mPlatform), mPlatform);

            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            mName_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName Block Size = {0}", mName_BlockSize);

            //--------------------------mName String Length-------------------------- [4 bytes]
            mName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mName String Length = {0}", mName_StringLength);

            //--------------------------mName-------------------------- [mName_StringLength bytes]
            mName = ByteFunctions.ReadFixedString(sourceByteFile, mName_StringLength, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mName = {0}", mName);

            //--------------------------mImportName Block Size-------------------------- [4 bytes] //mImportName block size (size + string len)
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            mImportName_BlockSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName Block Size = {0}", mImportName_BlockSize);

            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName String Length = {0}", mImportName_StringLength);

            //--------------------------mImportName String Length-------------------------- [mImportName_StringLength bytes] (this is always 0)
            mImportName = ByteFunctions.ReadFixedString(sourceByteFile, mImportName_StringLength, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName = {0}", mImportName);

            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportScale = {0}", mImportScale);

            //--------------------------mToolProps-------------------------- (NEEDS WORK) [1 byte]
            //get tool props
            ToolProps toolProps = new ToolProps()
            {
                mbHasProps = ByteFunctions.GetBool(ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition) - 48)
            };

            mToolProps = toolProps;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToolProps = {0}", mToolProps);

            //--------------------------mNumMipLevels-------------------------- [4 bytes]
            mNumMipLevels = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);

            //--------------------------mWidth-------------------------- [4 bytes]
            mWidth = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mWidth = {0}", mWidth);

            //--------------------------mHeight-------------------------- [4 bytes]
            mHeight = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHeight = {0}", mHeight);

            //--------------------------mSurfaceFormat-------------------------- [4 bytes] 
            mSurfaceFormat = T3TextureBase_Functions.GetSurfaceFormat(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat), (int)mSurfaceFormat);

            //--------------------------mResourceUsage-------------------------- [4 bytes]
            mResourceUsage = T3TextureBase_Functions.GetResourceUsage(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), mResourceUsage), (int)mResourceUsage);

            //--------------------------mType-------------------------- [4 bytes]
            mType = T3Texture_Functions.GetTextureType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mType = {0} ({1})", Enum.GetName(typeof(T3TextureType), mType), (int)mType);

            //--------------------------mNormalMapFormat-------------------------- [4 bytes]
            mNormalMapFormat = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mNormalMapFormat = {0}", mNormalMapFormat);

            //--------------------------mHDRLightmapScale-------------------------- [4 bytes]
            mHDRLightmapScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHDRLightmapScale = {0}", mHDRLightmapScale);

            //--------------------------mToonGradientCutoff-------------------------- [4 bytes]
            mToonGradientCutoff = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonGradientCutoff = {0}", mToonGradientCutoff);

            //--------------------------mAlphaMode-------------------------- [4 bytes]
            mAlphaMode = T3Texture_Functions.GetAlphaMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), mAlphaMode), (int)mAlphaMode);

            //--------------------------mColorMode-------------------------- [4 bytes]
            mColorMode = T3Texture_Functions.GetColorMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), mColorMode), (int)mColorMode);

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
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mUVScale = {0} {1}", mUVScale.x, mUVScale.y);

            //--------------------------mToonRegions--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mToonRegions -----------");
            //--------------------------mToonRegions DCArray Capacity-------------------------- [4 bytes]
            uint bytePointerPostion_before_mToonRegions = bytePointerPosition;
            uint mToonRegions_ArrayCapacity = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayCapacity = {0}", mToonRegions_ArrayCapacity);

            //--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
            int mToonRegions_ArrayLength = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayLength = {0}", mToonRegions_ArrayLength);

            //--------------------------mToonRegions DCArray--------------------------
            mToonRegions = new List<T3ToonGradientRegion>();

            for (int i = 0; i < mToonRegions_ArrayLength; i++)
            {
                T3ToonGradientRegion toonGradientRegion = new T3ToonGradientRegion()
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

                mToonRegions.Add(toonGradientRegion);
            }

            //check if we are at the offset we should be after going through the array
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPostion_before_mToonRegions, mToonRegions_ArrayCapacity, ref bytePointerPosition);
            
            //--------------------------StreamHeader--------------------------
            StreamHeader = new StreamHeader()
            {
                mRegionCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                mAuxDataCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                mTotalDataSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition) //[4 bytes]
            };

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mRegionCount = {0}", StreamHeader.mRegionCount);
            Console.WriteLine("D3DTX mAuxDataCount {0}", StreamHeader.mAuxDataCount);
            Console.WriteLine("D3DTX mTotalDataSize {0}", StreamHeader.mTotalDataSize);

            //--------------------------mRegionHeaders--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mRegionHeaders -----------");
            mRegionHeaders = new List<RegionStreamHeader>();
            for (int i = 0; i < StreamHeader.mRegionCount; i++)
            {
                if (bytePointerPosition > calculated_HeaderLength)
                {
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                    Console.WriteLine("Pointer position is beyond the header length!");
                    Console.ResetColor();
                    break;
                }

                RegionStreamHeader mRegionHeader = new RegionStreamHeader() //no mFaceIndex or mSlicePitch compared to 6VSM
                {
                    mMipIndex = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes] 
                    mMipCount = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mDataSize = (uint)ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                    mPitch = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition), //[4 bytes]
                };

                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
                Console.WriteLine("[mRegionHeader {0}]", i);
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                Console.WriteLine("D3DTX mMipIndex = {0}", mRegionHeader.mMipIndex);
                Console.WriteLine("D3DTX mMipCount = {0}", mRegionHeader.mMipCount);
                Console.WriteLine("D3DTX mDataSize = {0}", mRegionHeader.mDataSize);
                Console.WriteLine("D3DTX mPitch = {0}", mRegionHeader.mPitch);

                mRegionHeaders.Add(mRegionHeader);
            }

            //do a quick check to see if we reached the end of the file header
            ByteFunctions.ReachedOffset(bytePointerPosition, (uint)calculated_HeaderLength);

            //--------------------------STORING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx header data...");

            if (calculated_HeaderLength < 0)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR, we were off with calculating header length! {0}", calculated_HeaderLength);

                return;
            }

            //allocate a byte array to contain the header data
            headerData = new byte[calculated_HeaderLength];

            //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(sourceByteFile, 0, headerData, 0, headerData.Length);

            Data_OriginalHeader = headerData;

            //--------------------------STORING D3DTX IMAGE DATA--------------------------
            //if we are reading the header only, dont continue past this point
            if (readHeaderOnly)
                return;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx image data...");

            T3Texture_Data = new List<byte[]>();

            for (int i = 0; i < StreamHeader.mRegionCount; i++)
            {
                int dataSize = (int)mRegionHeaders[i].mDataSize;
                byte[] imageData = ByteFunctions.AllocateBytes(dataSize, sourceByteFile, bytePointerPosition);

                bytePointerPosition += (uint)dataSize;

                T3Texture_Data.Add(imageData);
            }

            //do a quick check to see if we reached the end of the file
            ByteFunctions.ReachedEndOfFile(bytePointerPosition, (uint)sourceByteFile.Length);
        }

        public byte[] Get_Modified_D3DTX(DDS_File DDS_File, bool headerOnly)
        {
            byte[] Copied_Data = null;

            if (headerOnly)
                Copied_Data = Data_OriginalHeader;
            else
                Copied_Data = Data_OriginalBytes;



            return Copied_Data;
        }
    }
}
