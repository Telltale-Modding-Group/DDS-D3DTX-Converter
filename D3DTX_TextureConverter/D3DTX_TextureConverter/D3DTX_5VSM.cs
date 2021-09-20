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
    public class D3DTX_5VSM
    {
        public string MetaStreamVersion; //[4 bytes]
        public int mVersion; //[4 bytes]
        public uint Unknown1; //[4 bytes]
        public uint mDataSize; //[4 bytes]
        public uint Unknown2; //[4 bytes]

        //we skip 72 bytes of crc class names (NOTE TO SELF: perhaps 1 extra int? as 84 isn't cleanly divisible by 8)

        public uint Unknown3; //[4 bytes]
        public uint Unknown4; //[4 bytes]
        public uint Unknown5; //[4 bytes]
        public uint Unknown6; //[4 bytes]
        public PlatformType mPlatform; //[4 bytes]
        public uint Unknown7; //[4 bytes]
        public uint mImportName_StringLength; //[4 bytes]

        public string mName;

        public string mImportName; //[mImportName_StringLength bytes]
        public float mImportScale; //[4 bytes]

        //skipped 8 bytes (NOTE TO SELF: FIGURE OUT WHAT IS HERE)

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
            int headerLength = 0;

            Data_OriginalBytes = sourceByteFile;

            //which byte offset we are on (will be changed as we go through the file)
            uint bytePointerPosition = 0;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Total File Size = {0}", sourceByteFile.Length);

            //--------------------------Meta Stream Version-------------------------- [4 bytes]
            MetaStreamVersion = ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Meta Stream Version = {0}", MetaStreamVersion);

            //--------------------------mVersion-------------------------- [4 bytes]
            mVersion = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mVersion = {0}", mVersion);

            //--------------------------Unknown 1-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 1 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------mDataSize-------------------------- [4 bytes]
            mDataSize = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mDataSize = {0}", mDataSize);
            Console.WriteLine("at {0}", bytePointerPosition - 4);

            //if the 'parsed' texture byte size in the file is actually supposedly bigger than the file itself
            if (mDataSize > sourceByteFile.Length && !readHeaderOnly)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("Can't continue reading the file because the values we are reading are incorrect! This can be due to the byte data being shifted in the file or non-existant, and this is likley because the file version has changed.");

                return; //don't continue
            }

            //--------------------------CALCULATING HEADER LENGTH--------------------------
            if (readHeaderOnly)
            {
                headerLength = sourceByteFile.Length;
            }
            else
            {
                //calculating header length, parsed texture byte size - source byte size
                headerLength = sourceByteFile.Length - (int)mDataSize;
            }

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX (Calculated) Header Size = {0}", headerLength);

            //--------------------------Unknown 2-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 2 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------classes--------------------------
            //move the pointer past the data
            bytePointerPosition += 72;

            //--------------------------Unknown 3-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 3 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------Unknown 4-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 4 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------Unknown 5-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 5 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------Unknown 6-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 6 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------mPlatform-------------------------- [4 bytes]
            mPlatform = EnumPlatformType.GetPlatformType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), (int)mPlatform), mPlatform);

            //--------------------------Unknown 7-------------------------- [4 bytes]
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("D3DTX Unknown 7 = {0}", ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));

            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportName_StringLength = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName String Length = {0}", mImportName_StringLength);

            //--------------------------mImportName-------------------------- [mImportName_length bytes]
            mImportName = ByteFunctions.ReadFixedString(sourceByteFile, mImportName_StringLength, ref bytePointerPosition);
            mName = mImportName;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName = '{0}'", mImportName);

            //--------------------------mImportScale-------------------------- [4 bytes]
            mImportScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportScale = {0}", mImportScale);

            //--------------------------mToolProps-------------------------- (NEEDS WORK) [1 byte]
            //skip 8 bytes
            ByteFunctions.ReadLong(sourceByteFile, ref bytePointerPosition);

            //get tool props
            ToolProps toolProps = new ToolProps()
            {
                mbHasProps = ByteFunctions.ReadBool(sourceByteFile, ref bytePointerPosition)
            };

            mToolProps = toolProps;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mbHasProps = {0}", mToolProps.mbHasProps);

            //--------------------------mNumMipLevels-------------------------- [4 bytes]
            //NOTE: According to meta function, this is an unsigned long
            mNumMipLevels = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);

            //--------------------------mWidth-------------------------- [4 bytes]
            //NOTE: According to meta function, this is an unsigned long
            mWidth = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mWidth = {0}", mWidth);
            Console.WriteLine("at {0}", bytePointerPosition - 4);

            //--------------------------mHeight-------------------------- [4 bytes]
            //NOTE: According to meta function, this is an unsigned long
            mHeight = ByteFunctions.ReadUnsignedInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHeight = {0}", mHeight);
            Console.WriteLine("at {0}", bytePointerPosition - 4);

            //--------------------------mSurfaceFormat-------------------------- [4 bytes] 
            //NOTE: According to meta function, this is a long
            mSurfaceFormat = T3TextureBase_Functions.GetSurfaceFormat(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat), (int)mSurfaceFormat);

            //--------------------------mResourceUsage-------------------------- [4 bytes]
            //NOTE: According to meta function, this is a long
            mResourceUsage = T3TextureBase_Functions.GetResourceUsage(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), mResourceUsage), (int)mResourceUsage);

            //--------------------------mType-------------------------- [4 bytes]
            //NOTE: According to meta function, this is a long
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
            //NOTE: According to meta function, this is a long
            mAlphaMode = T3Texture_Functions.GetAlphaMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), mAlphaMode), (int)mAlphaMode);

            //--------------------------mColorMode-------------------------- [4 bytes]
            //NOTE: According to meta function, this is a long
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
                        b = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition) //[4 bytes]
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
            Console.WriteLine("at {0}", bytePointerPosition - 4);
            Console.WriteLine("D3DTX mTotalDataSize {0}", StreamHeader.mTotalDataSize);

            //--------------------------mRegionHeaders--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mRegionHeaders -----------");
            mRegionHeaders = new List<RegionStreamHeader>();
            for (int i = 0; i < StreamHeader.mRegionCount; i++)
            {
                if (bytePointerPosition > headerLength)
                {
                    ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                    Console.WriteLine("Pointer position is beyond the header length!");
                    Console.ResetColor();
                    break;
                }

                RegionStreamHeader mRegionHeader = new RegionStreamHeader() //no mFaceIndex or mSlicePitch
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

                Console.WriteLine("at {0}", bytePointerPosition - 8);
                Console.WriteLine("D3DTX mDataSize = {0}", mRegionHeader.mDataSize);

                Console.WriteLine("at {0}", bytePointerPosition - 4);
                Console.WriteLine("D3DTX mPitch = {0}", mRegionHeader.mPitch);

                mRegionHeaders.Add(mRegionHeader);
            }

            //do a quick check to see if we reached the end of the D3DTX header
            ByteFunctions.ReachedOffset(bytePointerPosition, (uint)headerLength);

            //--------------------------STORING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx header data...");

            if (headerLength < 0)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("ERROR, we were off with calculating header length! {0}", headerLength);

                return;
            }

            //allocate a byte array to contain the header data
            headerData = new byte[headerLength];

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
