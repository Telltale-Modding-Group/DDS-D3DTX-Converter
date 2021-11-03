using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using D3DTX_TextureConverter.Telltale;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.DirectX;

/*
 * NOTE:
 * 
 * This version of D3DTX is INCOMPLETE.
 * 
 * INCOMPLETE meaning that all of the data isn't known and getting identified correctly. We still parse, identify, and modify if needed but it's just named as "Unknown".
 * The reason being we don't have "full knowledge" of it, given that games that shipped with this version haven't shipped with a PDB. 
 * So the only source is looking through the strings in the game exe through a hex editor to identify what variables might be in the file.
 * This D3DTX version derives from version 9 and has been 'stripped' or adjusted to suit this version of D3DTX.
 * Also, Telltale uses Hungarian Notation for variable naming.
*/

/* - D3DTX Version 7 games
 * The Walking Dead: Michonne (TESTED)
 * Tales from the Borderlands (Re-Release?) (TESTED)
*/

namespace D3DTX_TextureConverter.Main
{
    /// <summary>
    /// This is a custom class that matches what is serialized in a D3DTX version 7 class. (INCOMPLETE)
    /// </summary>
    public class D3DTX_V7
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
        /// [4 bytes] The length of the mName string value.
        /// </summary>
        public uint mName_StringLength { get; set; }

        /// <summary>
        /// [mName_StringLength bytes] The string mName.
        /// </summary>
        public string mName { get; set; }

        /// <summary>
        /// [4 bytes] The mImportName block size in bytes.
        /// </summary>
        public int mImportName_BlockSize { get; set; }

        /// <summary>
        /// [4 bytes] The length of the mImportName string value. 
        /// </summary>
        public uint mImportName_StringLength { get; set; }

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
        public uint mNumMipLevels { get; set; }

        /// <summary>
        /// [4 bytes] The pixel width of the texture.
        /// </summary>
        public uint mWidth { get; set; }

        /// <summary>
        /// [4 bytes] The pixel height of the texture.
        /// </summary>
        public uint mHeight { get; set; }

        /// <summary>
        /// [4 bytes] The depth of a volume texture in pixels.
        /// </summary>
        public uint mDepth { get; set; }

        /// <summary>
        /// [4 bytes] mArraySize, not sure what this is for yet.
        /// </summary>
        public uint mArraySize { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines the compression used for the texture.
        /// </summary>
        public T3SurfaceFormat mSurfaceFormat { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines the dimesion of the texture.
        /// </summary>
        public T3TextureLayout mTextureLayout { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines the gamma of the texture.
        /// </summary>
        public T3SurfaceGamma mSurfaceGamma { get; set; }

        /// <summary>
        /// [4 bytes] An enum, defines the multisample (anistrophic) level of the texture.
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
        /// [4 bytes] Defines the format of the normal map.
        /// </summary>
        public float mNormalMapFormat { get; set; }

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
        /// [4 bytes] The amount of elements in the mToonRegsions array.
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
        /// Parses a D3DTX Object from a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bytePointerPosition"></param>
        /// <param name="headerLength"></param>
        public D3DTX_V7(byte[] data, ref uint bytePointerPosition, uint headerLength = 0)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("||||||||||| D3DTX Header |||||||||||");

            //--------------------------mVersion-------------------------- [4 bytes]
            mVersion = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mVersion = {0}", mVersion);

            //--------------------------mSamplerState Block Size-------------------------- [4 bytes]
            mSamplerState_BlockSize = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mSamplerState_BlockSize = {0}", mSamplerState_BlockSize);

            //--------------------------mSamplerState-------------------------- [4 bytes]
            mSamplerState = new T3SamplerStateBlock()
            {
                mData = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition)
            };

            Console.WriteLine("D3DTX mSamplerState = {0}", mSamplerState);

            //--------------------------mPlatform Block Size-------------------------- [4 bytes]
            mPlatform_BlockSize = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mPlatform_BlockSize = {0}", mPlatform_BlockSize);

            //--------------------------mPlatform-------------------------- [4 bytes]
            mPlatform = EnumPlatformType.GetPlatformType(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), (int)mPlatform), mPlatform);

            //--------------------------mName Block Size-------------------------- [4 bytes] //mName block size (size + string len)
            mName_BlockSize = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName Block Size = {0}", mName_BlockSize);

            //--------------------------mName String Length-------------------------- [4 bytes]
            mName_StringLength = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName String Length = {0}", mName_StringLength);

            //--------------------------mName-------------------------- [mName_StringLength bytes]
            mName = ByteFunctions.ReadFixedString(data, mName_StringLength, ref bytePointerPosition);
            Console.WriteLine("D3DTX mName = {0}", mName);

            //--------------------------mImportName Block Size-------------------------- [4 bytes] //mImportName block size (size + string len)
            mImportName_BlockSize = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName Block Size = {0}", mImportName_BlockSize);

            //--------------------------mImportName String Length-------------------------- [4 bytes]
            mImportName_StringLength = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName String Length = {0}", mImportName_StringLength);

            //--------------------------mImportName-------------------------- [mImportName_StringLength bytes] (this is always 0)
            mImportName = ByteFunctions.ReadFixedString(data, mImportName_StringLength, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportName = {0}", mImportName);

            //--------------------------mImportScale-------------------------- [4 bytes]
            mImportScale = ByteFunctions.ReadFloat(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mImportScale = {0}", mImportScale);

            //--------------------------mToolProps-------------------------- (NEEDS WORK) [1 byte]
            //get tool props
            ToolProps toolProps = new ToolProps()
            {
                mbHasProps = ByteFunctions.GetBool(ByteFunctions.ReadByte(data, ref bytePointerPosition) - 48)
            };

            mToolProps = toolProps;
            Console.WriteLine("D3DTX mToolProps = {0}", mToolProps);

            //--------------------------mNumMipLevels-------------------------- [4 bytes]
            mNumMipLevels = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mNumMipLevels = {0}", mNumMipLevels);

            //--------------------------mWidth-------------------------- [4 bytes]
            mWidth = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mWidth = {0}", mWidth);

            //--------------------------mHeight-------------------------- [4 bytes]
            mHeight = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mHeight = {0}", mHeight);

            //--------------------------mSurfaceFormat-------------------------- [4 bytes] 
            mSurfaceFormat = T3TextureBase_Functions.GetSurfaceFormat(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), mSurfaceFormat), (int)mSurfaceFormat);

            //--------------------------mTextureLayout-------------------------- [4 bytes]
            mTextureLayout = T3TextureBase_Functions.GetTextureLayout(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mTextureLayout = {0} ({1})", Enum.GetName(typeof(T3TextureLayout), mTextureLayout), (int)mTextureLayout);

            //--------------------------mSurfaceGamma-------------------------- [4 bytes]
            mSurfaceGamma = T3TextureBase_Functions.GetSurfaceGamma(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mSurfaceGamma = {0} ({1})", Enum.GetName(typeof(T3SurfaceGamma), mSurfaceGamma), (int)mSurfaceGamma);

            //--------------------------mSurfaceMultisample-------------------------- [4 bytes]
            mSurfaceMultisample = T3TextureBase_Functions.GetSurfaceMultisample(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mSurfaceMultisample = {0} ({1})", Enum.GetName(typeof(T3SurfaceMultisample), mSurfaceMultisample), (int)mSurfaceMultisample);

            //--------------------------mResourceUsage-------------------------- [4 bytes]
            mResourceUsage = T3TextureBase_Functions.GetResourceUsage(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), mResourceUsage), (int)mResourceUsage);

            //--------------------------mType-------------------------- [4 bytes]
            mType = T3Texture_Functions.GetTextureType(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mType = {0} ({1})", Enum.GetName(typeof(T3TextureType), mType), (int)mType);

            //--------------------------mSwizzleSize-------------------------- [4 bytes]
            mSwizzleSize = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mSwizzleSize = {0}", Enum.GetName(typeof(T3TextureType), mType), (int)mType);

            //--------------------------mSwizzle-------------------------- [4 bytes]
            mSwizzle = new RenderSwizzleParams()
            {
                mSwizzle1 = (char)ByteFunctions.ReadByte(data, ref bytePointerPosition),
                mSwizzle2 = (char)ByteFunctions.ReadByte(data, ref bytePointerPosition),
                mSwizzle3 = (char)ByteFunctions.ReadByte(data, ref bytePointerPosition),
                mSwizzle4 = (char)ByteFunctions.ReadByte(data, ref bytePointerPosition)
            };

            Console.WriteLine("D3DTX mSwizzle = {0}", mSwizzle);

            //--------------------------mNormalMapFormat-------------------------- [4 bytes]
            mNormalMapFormat = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mNormalMapFormat = {0}", mNormalMapFormat);

            //--------------------------mHDRLightmapScale-------------------------- [4 bytes]
            mHDRLightmapScale = ByteFunctions.ReadFloat(data, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHDRLightmapScale = {0}", mHDRLightmapScale);

            //--------------------------mToonGradientCutoff-------------------------- [4 bytes]
            mToonGradientCutoff = ByteFunctions.ReadFloat(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mToonGradientCutoff = {0}", mToonGradientCutoff);

            //--------------------------mAlphaMode-------------------------- [4 bytes]
            mAlphaMode = T3Texture_Functions.GetAlphaMode(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), mAlphaMode), (int)mAlphaMode);

            //--------------------------mColorMode-------------------------- [4 bytes]
            mColorMode = T3Texture_Functions.GetColorMode(ByteFunctions.ReadInt(data, ref bytePointerPosition));
            Console.WriteLine("D3DTX mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), mColorMode), (int)mColorMode);

            //--------------------------mUVOffset-------------------------- [8 bytes]
            mUVOffset = new Vector2()
            {
                x = ByteFunctions.ReadFloat(data, ref bytePointerPosition), //[4 bytes]
                y = ByteFunctions.ReadFloat(data, ref bytePointerPosition) //[4 bytes]
            };

            Console.WriteLine("D3DTX mUVOffset = {0} {1}", mUVOffset.x, mUVOffset.y);

            //--------------------------mUVScale-------------------------- [8 bytes]
            mUVScale = new Vector2()
            {
                x = ByteFunctions.ReadFloat(data, ref bytePointerPosition), //[4 bytes]
                y = ByteFunctions.ReadFloat(data, ref bytePointerPosition) //[4 bytes]
            };

            Console.WriteLine("D3DTX mUVScale = {0} {1}", mUVScale.x, mUVScale.y);

            //--------------------------mArrayFrameNames--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Cyan);
            Console.WriteLine("----------- mArrayFrameNames -----------");
            //--------------------------mArrayFrameNames DCArray Capacity-------------------------- [4 bytes]
            uint bytePointerPostion_before_mArrayFrameNames = bytePointerPosition;
            mArrayFrameNames_ArrayCapacity = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayCapacity = {0}", mArrayFrameNames_ArrayCapacity);

            //--------------------------mArrayFrameNames DCArray Length-------------------------- [4 bytes]
            mArrayFrameNames_ArrayLength = ByteFunctions.ReadInt(data, ref bytePointerPosition); //ADD 1 BECAUSE COUNTING STARTS AT 0
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayLength = {0}", mArrayFrameNames_ArrayLength);

            //--------------------------mArrayFrameNames DCArray--------------------------
            //NOTE: According to meta function, this is a DCArray<Symbol>
            mArrayFrameNames = new List<Symbol>();

            for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
            {
                Symbol newSymbol = new Symbol()
                {
                    mCrc64 = ByteFunctions.ReadUnsignedLong(data, ref bytePointerPosition) //[8 bytes]
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
            mToonRegions_ArrayCapacity = ByteFunctions.ReadUnsignedInt(data, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayCapacity = {0}", mToonRegions_ArrayCapacity);

            //--------------------------mToonRegions DCArray Length-------------------------- [4 bytes]
            mToonRegions_ArrayLength = ByteFunctions.ReadInt(data, ref bytePointerPosition);
            Console.WriteLine("D3DTX mToonRegions_ArrayLength = {0}", mToonRegions_ArrayLength);

            //--------------------------mToonRegions DCArray--------------------------
            mToonRegions = new T3ToonGradientRegion[mToonRegions_ArrayLength];

            for (int i = 0; i < mToonRegions_ArrayLength; i++)
            {
                mToonRegions[i] = new T3ToonGradientRegion()
                {
                    mColor = new Color()
                    {
                        r = ByteFunctions.ReadFloat(data, ref bytePointerPosition), //[4 bytes]
                        g = ByteFunctions.ReadFloat(data, ref bytePointerPosition), //[4 bytes]
                        b = ByteFunctions.ReadFloat(data, ref bytePointerPosition), //[4 bytes]
                        a = ByteFunctions.ReadFloat(data, ref bytePointerPosition) //[4 bytes]
                    },

                    mSize = ByteFunctions.ReadFloat(data, ref bytePointerPosition) //[4 bytes]
                };

                Console.WriteLine("D3DTX mToonRegion {0} = {1}", i, mToonRegions[i]);
            }

            //check if we are at the offset we should be after going through the array
            ByteFunctions.DCArrayCheckAdjustment(bytePointerPostion_before_mToonRegions, mToonRegions_ArrayCapacity, ref bytePointerPosition);

            //--------------------------StreamHeader--------------------------
            mStreamHeader = new StreamHeader()
            {
                mRegionCount = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                mAuxDataCount = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                mTotalDataSize = ByteFunctions.ReadInt(data, ref bytePointerPosition) //[4 bytes]
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
                mRegionHeaders[i] = new RegionStreamHeader()
                {
                    mFaceIndex = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                    mMipIndex = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes] 
                    mMipCount = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                    mDataSize = (uint)ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                    mPitch = ByteFunctions.ReadInt(data, ref bytePointerPosition), //[4 bytes]
                    mSlicePitch = ByteFunctions.ReadInt(data, ref bytePointerPosition) //[4 bytes]
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
            //-----------------------------------------D3DTX HEADER END-----------------------------------------
            //--------------------------STORING D3DTX IMAGE DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Blue);
            Console.WriteLine("Storing the .d3dtx image data...");

            mPixelData = new List<byte[]>();

            for (int i = 0; i < mStreamHeader.mRegionCount; i++)
            {
                int dataSize = (int)mRegionHeaders[i].mDataSize;
                byte[] imageData = ByteFunctions.AllocateBytes(dataSize, data, bytePointerPosition);

                bytePointerPosition += (uint)dataSize;

                mPixelData.Add(imageData);
            }

            //do a quick check to see if we reached the end of the file
            ByteFunctions.ReachedEndOfFile(bytePointerPosition, (uint)data.Length);
        }

        public void ModifyD3DTX(DDS_Master dds)
        {

        }

        /// <summary>
        /// Converts the data of this object into a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetByteData()
        {
            byte[] finalData = new byte[0];

            return finalData;
        }
    }
}
