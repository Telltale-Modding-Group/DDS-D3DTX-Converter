using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using D3DTX_TextureConverter.Utilities;
using D3DTX_TextureConverter.Telltale;

namespace D3DTX_TextureConverter
{
    public class D3DTX_File
    {
        //telltale d3dtx texture file extension
        public static string d3dtxExtension = ".d3dtx";

        //custom header file extension (generated from d3dtx to dds, used to convert dds back to d3dtx)
        public static string headerExtension = ".header";

        //main byte data
        public string sourceFile;
        public string sourceFileName;
        public byte[] sourceByteFile;
        public byte[] headerData;
        public byte[] textureData; //allocate our byte array to contain our texture data




        //this data will be parsed and assigned
        //(some of the variable names are named to match what is found when getting strings from the game exe)
        public string dword; //(IN THE FILE) magic dword
        public int mVersion; //(IN THE FILE) file version? not 100% certain
        public int mDataSize; //total byte size of the texture data, used to calculate the header length
        public int headerLength; //length of the telltale d3dtx header

        public int mNumMipLevels_decremented; //image mip map count - 1 (since images with no mip maps have this value set to 1)




        public T3Texture T3Texture;
        public T3TextureBase T3TextureBase;
        public T3Texture_DX11 T3Texture_DX11;




        public void Write_D3DTX_Header(string destinationFile)
        {
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Generating .header file to store the .d3dtx header data.");

            //build the header destination path, assuming the extnesion of the destination file path is .dds
            string headerFilePath = string.Format("{0}{1}", destinationFile.Remove(destinationFile.Length - 4, 4), headerExtension);

            if (headerLength < 0)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red); 
                Console.WriteLine("ERROR, we were off with calculating header length! {0}", headerLength);

                return;
            }

            //allocate a byte array to contain the header data
            byte[] headerData = new byte[headerLength];

            //copy all the bytes from the source byte file after the header length, and copy that data to the texture data byte array
            Array.Copy(sourceByteFile, 0, headerData, 0, headerData.Length);

            //write the header data to a file
            File.WriteAllBytes(headerFilePath, headerData);

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green); 
            Console.WriteLine(".d3dtx data stored in {0}", headerFilePath);
        }

        public void Apply_DDS_Data_To_D3DTX_Data(DDS_File ddsFile, bool applyToHeader)
        {
            //Utilities.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            //Console.WriteLine("Experimental Resolution Changed enabled, changing values on the original D3DTX header...");

            //byte[] mainData = applyToHeader ? headerData : sourceByteFile;

            //--------------------------3 = mDataSize--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, ddsFile.ddsTextureData.Length, byteLocation_mDataSize);
            //--------------------------9 = mWidth--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, mWidth, byteLocation_mWidth);
            //--------------------------10 = mHeight--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, mHeight, byteLocation_mHeight);
            //--------------------------12 = mTotalDataSize--------------------------
            //modify the bytes in the header
            //mainData = ByteFunctions.ModifyBytes(mainData, ddsFile.ddsTextureData.Length, byteLocation_mTotalDataSize);
        }

        public static string Read_D3DTX_File_DWORD_Only(string sourceFile)
        {
            byte[] sourceByteFile = File.ReadAllBytes(sourceFile);

            uint bytePointerPosition = 0;

            return ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
        }

        public void Read_D3DTX_File_6VSM(string sourceFileName, string sourceFile, bool readHeaderOnly)
        {
            //read the source file into a byte array
            this.sourceByteFile = File.ReadAllBytes(sourceFile);
            this.sourceFileName = sourceFileName;
            this.sourceFile = sourceFile;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Total File Byte Size = {0}", sourceByteFile.Length);

            //which byte offset we are on (will be changed as we go through the file)
            uint bytePointerPosition = 0;

            T3Texture = new T3Texture();
            T3TextureBase = new T3TextureBase();
            T3Texture_DX11 = new T3Texture_DX11();

            //--------------------------MAGIC HEADER--------------------------
            dword = ByteFunctions.ReadFixedString(sourceByteFile, 4, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX DWORD = {0}", dword);

            //--------------------------mVersion--------------------------
            mVersion = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mVersion = {0}", mVersion);

            //--------------------------mDataSize?--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition = 12;

            mDataSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mDataSize = {0}", mDataSize);

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
                //just read the 
                headerLength = sourceByteFile.Length;
            }
            else
            {
                //calculating header length, parsed texture byte size - source byte size
                headerLength = sourceByteFile.Length - mDataSize;
            }

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX Header Byte Size = {0}", headerLength);

            //--------------------------classes--------------------------
            //offset byte pointer location
            bytePointerPosition = 20;

            //move the pointer past the data
            bytePointerPosition += 84;

            //offset byte pointer location to start at where we need to be
            bytePointerPosition += 20;

            //--------------------------mPlatform--------------------------
            int mPlatform = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            T3Texture.mPlatform = EnumPlatformType.GetPlatformType(mPlatform);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mPlatform = {0} ({1})", Enum.GetName(typeof(PlatformType), T3Texture.mPlatform), mPlatform);

            //--------------------------mImportName String Length--------------------------
            int mImportName_length = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName Length = {0}", mImportName_length);

            //--------------------------mImportName--------------------------
            T3Texture.mImportName = ByteFunctions.ReadFixedString(sourceByteFile, mImportName_length, ref bytePointerPosition);
            T3TextureBase.mName = T3Texture.mImportName;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportName = '{0}'", T3Texture.mImportName);

            //--------------------------mImportScale--------------------------
            T3Texture.mImportScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mImportScale = {0}", T3Texture.mImportScale);

            //--------------------------mToolProps-------------------------- (NEEDS WORK)
            bytePointerPosition += 8; //skip 8 bytes
            ToolProps toolProps = new ToolProps()
            {
                mbHasProps = ByteFunctions.ReadBool(sourceByteFile, ref bytePointerPosition)
            };

            T3Texture.mToolProps = toolProps;

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mbHasProps = {0}", T3Texture.mToolProps.mbHasProps);

            //--------------------------mNumMipLevels--------------------------
            //NOTE: According to meta function, this is an unsigned long

            T3TextureBase.mNumMipLevels = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mNumMipLevels = {0}", T3TextureBase.mNumMipLevels);

            //--------------------------mWidth--------------------------
            //NOTE: According to meta function, this is an unsigned long

            T3TextureBase.mWidth = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mWidth = {0}", T3TextureBase.mWidth);

            //--------------------------mHeight--------------------------
            //NOTE: According to meta function, this is an unsigned long

            T3TextureBase.mHeight = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHeight = {0}", T3TextureBase.mHeight);

            //--------------------------mDepth--------------------------
            //NOTE: According to meta function, this is an unsigned long

            T3TextureBase.mDepth = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mDepth = {0}", T3TextureBase.mDepth);

            //--------------------------mArraySize--------------------------
            //NOTE: According to meta function, this is an unsigned long

            T3TextureBase.mArraySize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mArraySize = {0}", T3TextureBase.mArraySize);

            //--------------------------mSurfaceFormat--------------------------
            //NOTE: According to meta function, this is a long

            T3TextureBase.mSurfaceFormat = T3TextureBase_Functions.GetSurfaceFormat(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceFormat = {0} ({1})", Enum.GetName(typeof(T3SurfaceFormat), T3TextureBase.mSurfaceFormat), (int)T3TextureBase.mSurfaceFormat);

            //--------------------------mTextureLayout--------------------------
            //NOTE: According to meta function, this is a long

            T3TextureBase.mTextureLayout = T3TextureBase_Functions.GetTextureLayout(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mTextureLayout = {0} ({1})", Enum.GetName(typeof(T3TextureLayout), T3TextureBase.mTextureLayout), (int)T3TextureBase.mTextureLayout);

            //--------------------------mSurfaceGamma--------------------------
            //NOTE: According to meta function, this is a long

            T3TextureBase.mSurfaceGamma = T3TextureBase_Functions.GetSurfaceGamma(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceGamma = {0} ({1})", Enum.GetName(typeof(T3SurfaceGamma), T3TextureBase.mSurfaceGamma), (int)T3TextureBase.mSurfaceGamma);

            //--------------------------mSurfaceMultisample--------------------------
            //NOTE: According to meta function, this is a long

            T3TextureBase.mSurfaceMultisample = T3TextureBase_Functions.GetSurfaceMultisample(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSurfaceMultisample = {0} ({1})", Enum.GetName(typeof(T3SurfaceMultisample), T3TextureBase.mSurfaceMultisample), (int)T3TextureBase.mSurfaceMultisample);

            //--------------------------mResourceUsage--------------------------
            //NOTE: According to meta function, this is a long

            T3TextureBase.mResourceUsage = T3TextureBase_Functions.GetResourceUsage(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mResourceUsage = {0} ({1})", Enum.GetName(typeof(T3ResourceUsage), T3TextureBase.mResourceUsage), (int)T3TextureBase.mResourceUsage);

            //--------------------------mType--------------------------
            //NOTE: According to meta function, this is a long

            T3Texture.mType = T3Texture_Functions.GetTextureType(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mType = {0} ({1})", Enum.GetName(typeof(T3TextureType), T3Texture.mType), (int)T3Texture.mType);

            //--------------------------mSwizzle--------------------------
            //NOTE: According to meta function, this is 4 unsigned chars

            //skippin 4 bytes
            int mSwizzleSize = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);

            RenderSwizzleParams mSwizzle = new RenderSwizzleParams()
            {
                mSwizzle1 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle2 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle3 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition),
                mSwizzle4 = (char)ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition)
            };

            T3Texture.mSwizzle = mSwizzle;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSwizzle = {0} {1} {2} {3}", T3Texture.mSwizzle.mSwizzle1, T3Texture.mSwizzle.mSwizzle2, T3Texture.mSwizzle.mSwizzle3, T3Texture.mSwizzle.mSwizzle4);

            //--------------------------mSpecularGlossExponent--------------------------
            T3Texture.mSpecularGlossExponent = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mSpecularGlossExponent = {0}", T3Texture.mSpecularGlossExponent);

            //--------------------------mHDRLightmapScale--------------------------
            T3Texture.mHDRLightmapScale = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mHDRLightmapScale = {0}", T3Texture.mHDRLightmapScale);

            //--------------------------mToonGradientCutoff--------------------------
            T3Texture.mToonGradientCutoff = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonGradientCutoff = {0}", T3Texture.mToonGradientCutoff);

            //--------------------------mAlphaMode--------------------------
            //NOTE: According to meta function, this is a long

            T3Texture.mAlphaMode = T3Texture_Functions.GetAlphaMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mAlphaMode = {0} ({1})", Enum.GetName(typeof(eTxAlpha), T3Texture.mAlphaMode), (int)T3Texture.mAlphaMode);

            //--------------------------mColorMode--------------------------
            //NOTE: According to meta function, this is a long

            T3Texture.mColorMode = T3Texture_Functions.GetColorMode(ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition));
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mColorMode = {0} ({1})", Enum.GetName(typeof(eTxColor), T3Texture.mColorMode), (int)T3Texture.mColorMode);

            //--------------------------mUVOffset--------------------------
            Vector2 mUVOffset = new Vector2()
            {
                x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition),
                y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition)
            };

            T3Texture.mUVOffset = mUVOffset;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mUVOffset = {0} {1}", mUVOffset.x, mUVOffset.y);

            //--------------------------mUVScale--------------------------
            Vector2 mUVScale = new Vector2()
            {
                x = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition),
                y = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition)
            };

            T3Texture.mUVScale = mUVScale;
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mUVScale = {0} {1}", mUVScale.x, mUVScale.y);

            //--------------------------mNumMipLevelsAllocated--------------------------
            //NOTE: According to meta function, this is an unsigned long

            //T3TextureBase.mNumMipLevelsAllocated = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            //ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            //Console.WriteLine("D3DTX mNumMipLevelsAllocated = {0}", T3TextureBase.mNumMipLevelsAllocated);

            //--------------------------mNumSurfacesRequested--------------------------
            //NOTE: According to meta function, this is an unsigned long

            //T3TextureBase.mNumSurfacesRequested = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            //ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            //Console.WriteLine("D3DTX mNumSurfacesRequested = {0}", T3TextureBase.mNumSurfacesRequested);

            //--------------------------mNumSurfacesRequired--------------------------
            //NOTE: According to meta function, this is an unsigned long

            //T3TextureBase.mNumSurfacesRequired = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            //ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            //Console.WriteLine("D3DTX mNumSurfacesRequired = {0}", T3TextureBase.mNumSurfacesRequired);

            //--------------------------mNumSurfacesLoaded--------------------------
            //NOTE: According to meta function, this is an unsigned long

            //T3TextureBase.mNumSurfacesLoaded = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            //ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            //Console.WriteLine("D3DTX mNumSurfacesLoaded = {0}", T3TextureBase.mNumSurfacesLoaded);

            //---------------------------NEDS WORK BELOW-----------------------------------------


            //--------------------------mArrayFrameNames DCArray Capacity--------------------------
            int mArrayFrameNames_ArrayCapacity = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayCapacity = {0}", mArrayFrameNames_ArrayCapacity);

            uint bytePointerPosition_before_mArrayFrameNames_ArrayLength = bytePointerPosition;

            //--------------------------mArrayFrameNames DCArray Length--------------------------
            int mArrayFrameNames_ArrayLength = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition); //ADD 1 BECAUSE COUNTING STARTS AT 0
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mArrayFrameNames_ArrayLength = {0}", mArrayFrameNames_ArrayLength);

            //--------------------------mArrayFrameNames--------------------------
            //NOTE: According to meta function, this is a DCArray<Symbol>
            T3Texture.mArrayFrameNames = new List<Symbol>();

            for (int i = 0; i < mArrayFrameNames_ArrayLength; i++)
            {
                Symbol newSymbol = new Symbol()
                {
                    mCrc64 = ByteFunctions.ReadLong(sourceByteFile, ref bytePointerPosition)
                };

                T3Texture.mArrayFrameNames.Add(newSymbol);
            }

            //not necessary, but to check if we are where we should be at
            uint estimatedOffPoint = bytePointerPosition_before_mArrayFrameNames_ArrayLength + ((uint)mArrayFrameNames_ArrayCapacity - 4);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow);
            Console.WriteLine("(Array Check) Estimated to be at = {0}", estimatedOffPoint);

            if(bytePointerPosition != estimatedOffPoint)
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Red);
                Console.WriteLine("(Array Check) Left off at = {0}", bytePointerPosition);
                Console.WriteLine("(Array Check) Skipping by using the estimated position...", bytePointerPosition);
            }
            else
            {
                ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Green);
                Console.WriteLine("(Array Check) Left off at = {0}", bytePointerPosition);
            }
            //not necessary, but to check if we are where we should be at (END)


            //--------------------------mToonRegions DCArray Capacity--------------------------
            int mToonRegions_ArrayCapacity = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayCapacity = {0}", mToonRegions_ArrayCapacity);

            //--------------------------mToonRegions DCArray Length--------------------------
            int mToonRegions_ArrayLength = ByteFunctions.ReadInt(sourceByteFile, ref bytePointerPosition);
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
            Console.WriteLine("D3DTX mToonRegions_ArrayLength = {0}", mToonRegions_ArrayLength);

            //--------------------------mToonRegions-------------------------- (NEED ASSISTANCE HERE)
            //NOTE: According to meta function, this is a DCArray<T3ToonGradientRegion>
            T3Texture.mToonRegions = new List<T3ToonGradientRegion>();

            for (int i = 0; i < mToonRegions_ArrayLength; i++)
            {
                T3ToonGradientRegion toonGradientRegion = new T3ToonGradientRegion()
                {
                    mColor = new Color()
                    {
                        r = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition),
                        g = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition),
                        b = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition)
                    },

                    mSize = ByteFunctions.ReadFloat(sourceByteFile, ref bytePointerPosition)
                };

                //ByteFunctions.ReadByte(sourceByteFile, ref bytePointerPosition);

                T3Texture.mToonRegions.Add(toonGradientRegion);
            }

            Console.WriteLine("(Array Check) Left off at = {0}", bytePointerPosition);


            //--------------------------GETTING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Storing the .d3dtx header data.");

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
        }
        /*
        public void Read_D3DTX_File_5VSM(string sourceFileName, string sourceFile, bool readHeaderOnly)
        {
            //read the source file into a byte array
            this.sourceByteFile = File.ReadAllBytes(sourceFile);
            this.sourceFileName = sourceFileName;
            this.sourceFile = sourceFile;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Total File Byte Size = {0}", sourceByteFile.Length);

            //which byte offset we are on (will be changed as we go through the file)
            uint bytePointerPosition = 0;

            //--------------------------DWORD--------------------------
            //offset byte pointer location to get the DWORD
            bytePointerPosition = 0;
            byteLocation_dword = 0;

            //allocate 4 byte array (string)
            byte[] source_dword = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to string
            string parsed_dword = Encoding.ASCII.GetString(source_dword);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX DWORD = {0}", parsed_dword);

            //assign the parsed value
            dword = parsed_dword;

            //--------------------------mVersion--------------------------
            //offset byte pointer location to get the COMPRESISON TYPE?
            bytePointerPosition = 4;
            byteLocation_mVersion = 4;

            //allocate 4 byte array (int32)
            byte[] source_mVersion = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mVersion = BitConverter.ToInt32(source_mVersion);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mVersion = {0}", parsed_mVersion.ToString());

            //assign the parsed value
            mVersion = parsed_mVersion;

            //--------------------------mDataSize--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition = 12;
            byteLocation_mDataSize = 12;

            //allocate 4 byte array (int32)
            byte[] source_mDataSize = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mDataSize = BitConverter.ToInt32(source_mDataSize);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mDataSize = {0}", parsed_mDataSize.ToString());

            //assign the parsed value
            mDataSize = parsed_mDataSize;

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
                //just read the 
                headerLength = sourceByteFile.Length;
            }
            else
            {
                //calculating header length, parsed texture byte size - source byte size
                headerLength = sourceByteFile.Length - mDataSize;
            }

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Header Byte Size = {0}", headerLength.ToString());

            //--------------------------mRecords--------------------------
            //NOTE TO SELF - no need to parse this byte data, we can extract the entire header later with this info included and just change what we need

            //offset byte pointer location to get the TELLTALE CLASS NAMES DATA
            bytePointerPosition = 20;
            byteLocation_mRecords = 20;

            //parse the data
            mRecords = D3DTX_GetRecordData(dword, sourceByteFile, (int)bytePointerPosition);

            //move the pointer past the data
            bytePointerPosition += (uint)mRecords.Length;

            //--------------------------mImportName_length--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition += 24;

            byteLocation_mImportName_length = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mImportName_length = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mImportName_length = BitConverter.ToInt32(source_mImportName_length);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mImportName Length = {0}", parsed_mImportName_length.ToString());

            //assign the parsed value
            mImportName_length = parsed_mImportName_length;

            //--------------------------mImportName--------------------------
            //offset byte pointer location to get the TEXTURE FILE NAME
            bytePointerPosition += 4;
            byteLocation_mImportName = bytePointerPosition;

            //allocate a byte array to get the bytes
            byte[] source_mImportName = ByteFunctions.AllocateBytes(mImportName_length, sourceByteFile, bytePointerPosition);

            //parse it to a string
            string parsed_mImportName = Encoding.ASCII.GetString(source_mImportName);

            //show it to the user
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mImportName = '{0}'", parsed_mImportName);

            //assign the parsed value
            mImportName = parsed_mImportName;

            //move the cursor past the filename.extension byte string
            bytePointerPosition += (uint)mImportName_length;

            //--------------------------mNumMipLevels--------------------------
            //offset byte pointer location to get the MIP MAP COUNT
            bytePointerPosition += 13;
            byteLocation_mNumMipLevels = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mNumMipLevels = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mNumMipLevels = BitConverter.ToInt32(source_mNumMipLevels);
            int parsed_mNumMipLevels_decremented = parsed_mNumMipLevels - 1;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mNumMipLevels = {0} ({1} - according to DDS)", parsed_mNumMipLevels.ToString(), parsed_mNumMipLevels_decremented.ToString());

            //assign the parsed data
            mNumMipLevels = parsed_mNumMipLevels;
            mNumMipLevels_decremented = parsed_mNumMipLevels_decremented;

            //--------------------------mWidth--------------------------
            //offset byte pointer location to get the MAIN IMAGE WIDTH
            bytePointerPosition += 4;
            byteLocation_mWidth = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mWidth = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mWidth = BitConverter.ToInt32(source_mWidth);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mWidth = {0}", parsed_mWidth.ToString());

            //assign the parsed data
            mWidth = parsed_mWidth;
            //--------------------------mHeight--------------------------
            //offset byte pointer location to get the MAIN IMAGE HEIGHT
            bytePointerPosition += 4;
            byteLocation_mHeight = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mHeight = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mHeight = BitConverter.ToInt32(source_mHeight);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mHeight = {0}", parsed_mHeight.ToString());

            //assign the parsed data
            mHeight = parsed_mHeight;
            //--------------------------mSurfaceFormat--------------------------
            //offset byte pointer location to get the DXT TYPE
            bytePointerPosition += 4;
            byteLocation_mSurfaceFormat = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mSurfaceFormat = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mSurfaceFormat = BitConverter.ToInt32(source_mSurfaceFormat);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mSurfaceFormat = {0}", parsed_mSurfaceFormat.ToString());

            //assign the parsed data
            mSurfaceFormat = parsed_mSurfaceFormat;

            //convert the value into a string for the DDS
            mSurfaceFormat_converted = D3DTX_GetDXTType_ForDDS(dword, mSurfaceFormat);

            //get the boolean value for the calculation later
            mSurfaceFormat_dxtTypeBoolSize = mSurfaceFormat_converted.Equals("DXT1");

            //offset byte pointer location
            bytePointerPosition += 8;

            //--------------------------mTotalDataSize--------------------------
            //offset byte pointer location past the DXT TYPE
            bytePointerPosition += 4;

            //skip away ahead to the texture byte size
            bytePointerPosition += 52;
            byteLocation_mTotalDataSize = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mTotalDataSize = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mTotalDataSize = BitConverter.ToInt32(source_mTotalDataSize);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Parsed Texture Byte Size = {0}", parsed_mTotalDataSize.ToString());

            //assign the parsed value
            mTotalDataSize = parsed_mTotalDataSize;

            //--------------------------GETTING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Storing the .d3dtx header data.");

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
        }

        public void Read_D3DTX_File_ERTM(string sourceFileName, string sourceFile, bool readHeaderOnly, out byte[] dds_data)
        {
            dds_data = null;

            //read the source file into a byte array
            this.sourceByteFile = File.ReadAllBytes(sourceFile);
            this.sourceFileName = sourceFileName;
            this.sourceFile = sourceFile;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Total File Byte Size = {0}", sourceByteFile.Length);

            //which byte offset we are on (will be changed as we go through the file)
            uint bytePointerPosition = 0;

            //--------------------------DWORD--------------------------
            //offset byte pointer location to get the DWORD
            bytePointerPosition = 0;
            byteLocation_dword = 0;

            //allocate 4 byte array (string)
            byte[] source_dword = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to string
            string parsed_dword = Encoding.ASCII.GetString(source_dword);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX DWORD = {0}", parsed_dword);

            //assign the parsed value
            dword = parsed_dword;

            //--------------------------mVersion--------------------------
            //offset byte pointer location to get the COMPRESISON TYPE?
            bytePointerPosition = 4;
            byteLocation_mVersion = 4;

            //allocate 4 byte array (int32)
            byte[] source_mVersion = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mVersion = BitConverter.ToInt32(source_mVersion);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mVersion = {0}", parsed_mVersion.ToString());

            //assign the parsed value
            mVersion = parsed_mVersion;

            //--------------------------mRecords--------------------------
            //NOTE TO SELF - no need to parse this byte data, we can extract the entire header later with this info included and just change what we need

            //offset byte pointer location to get the TELLTALE CLASS NAMES DATA
            bytePointerPosition = 68;
            byteLocation_mRecords = 68;

            //parse the data
            mRecords = D3DTX_GetRecordData(dword, sourceByteFile, (int)bytePointerPosition);

            //move the pointer past the data
            bytePointerPosition += (uint)mRecords.Length;

            //--------------------------mImportName_length--------------------------
            //offset byte pointer location to get the TEXTURE BYTE SIZE
            bytePointerPosition += 0;
            byteLocation_mImportName_length = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mImportName_length = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mImportName_length = BitConverter.ToInt32(source_mImportName_length);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mImportName Length = {0}", parsed_mImportName_length.ToString());

            //assign the parsed value
            mImportName_length = parsed_mImportName_length;

            //--------------------------mImportName--------------------------
            //offset byte pointer location to get the TEXTURE FILE NAME
            bytePointerPosition += 4;
            byteLocation_mImportName = bytePointerPosition;

            //allocate a byte array to get the bytes
            byte[] source_mImportName = ByteFunctions.AllocateBytes(mImportName_length, sourceByteFile, bytePointerPosition);

            //parse it to a string
            string parsed_mImportName = Encoding.ASCII.GetString(source_mImportName);

            //show it to the user
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mImportName = '{0}'", parsed_mImportName);

            //assign the parsed value
            mImportName = parsed_mImportName;

            //move the cursor past the filename.extension byte string
            bytePointerPosition += (uint)mImportName_length;
            //--------------------------mNumMipLevels--------------------------
            //offset byte pointer location to get the MIP MAP COUNT
            bytePointerPosition += 11;
            byteLocation_mNumMipLevels = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mNumMipLevels = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mNumMipLevels = BitConverter.ToInt32(source_mNumMipLevels);
            int parsed_mNumMipLevels_decremented = parsed_mNumMipLevels - 1;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mNumMipLevels = {0} ({1} - according to DDS)", parsed_mNumMipLevels.ToString(), parsed_mNumMipLevels_decremented.ToString());

            //assign the parsed data
            mNumMipLevels = parsed_mNumMipLevels;
            mNumMipLevels_decremented = parsed_mNumMipLevels_decremented;

            //--------------------------mSurfaceFormat--------------------------
            //offset byte pointer location to get the DXT TYPE
            bytePointerPosition += 4;
            byteLocation_mSurfaceFormat = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mSurfaceFormat = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            string parsed_mSurfaceFormat = Encoding.ASCII.GetString(source_mSurfaceFormat);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mSurfaceFormat = {0}", parsed_mSurfaceFormat.ToString());


            //assign the parsed data
            mSurfaceFormat = D3DTX_GetDXTType_ForD3DTX(dword, parsed_mSurfaceFormat);

            //convert the value into a string for the DDS
            mSurfaceFormat_converted = parsed_mSurfaceFormat;

            //get the boolean value for the calculation later
            mSurfaceFormat_dxtTypeBoolSize = parsed_mSurfaceFormat.Equals("DXT1");

            //--------------------------mWidth--------------------------
            //offset byte pointer location to get the MAIN IMAGE WIDTH
            bytePointerPosition += 4;

            byteLocation_mWidth = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mWidth = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mWidth = BitConverter.ToInt32(source_mWidth);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mWidth = {0}", parsed_mWidth.ToString());

            //assign the parsed data
            mWidth = parsed_mWidth;
            //--------------------------mHeight--------------------------
            //offset byte pointer location to get the MAIN IMAGE HEIGHT
            bytePointerPosition += 4;
            byteLocation_mHeight = bytePointerPosition;

            //allocate 4 byte array (int32)
            byte[] source_mHeight = ByteFunctions.AllocateBytes(4, sourceByteFile, bytePointerPosition);

            //parse the byte array to int32
            int parsed_mHeight = BitConverter.ToInt32(source_mHeight);

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX mHeight = {0}", parsed_mHeight.ToString());

            //assign the parsed data
            mHeight = parsed_mHeight;
            //--------------------------GETTING THE REST OF THE HEADER--------------------------
            //offset byte pointer location 
            //bytePointerPosition += 4;

            uint searchLength = 10000;
            uint bytePointerOffset = 0;

            for (int i = 0; i < searchLength; i++)
            {
                uint newBytePosition = bytePointerPosition + (uint)i;

                //allocate 4 byte array (int32)
                byte[] source_bytes = ByteFunctions.AllocateBytes(4, sourceByteFile, newBytePosition);

                //parse the byte array to int32
                string parsed_bytes = Encoding.ASCII.GetString(source_bytes);

                if (parsed_bytes.Equals("DDS "))
                {
                    bytePointerOffset = (uint)i;
                    break;
                }
            }

            bytePointerPosition += bytePointerOffset;
            headerLength = (int)bytePointerPosition;

            //write the result to the console for viewing
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.White); 
            Console.WriteLine("D3DTX Header Length = {0}", headerLength.ToString());

            //--------------------------GETTING D3DTX HEADER DATA--------------------------
            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Storing the .d3dtx header data.");

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

            //copy the DDS data after the telltale header because it's literally just a DDS file
            int ddsDataLength = sourceByteFile.Length - headerLength;

            dds_data = new byte[ddsDataLength];

            Array.Copy(sourceByteFile, headerLength, dds_data, 0, ddsDataLength);
        }
        */
/*
 * DXT1 - DXGI_FORMAT_BC1_UNORM / D3DFMT_DXT1
 * DXT2 - D3DFMT_DXT2
 * DXT3 - DXGI_FORMAT_BC2_UNORM / D3DFMT_DXT3
 * DXT4 - D3DFMT_DXT4
 * DXT5 - DXGI_FORMAT_BC3_UNORM / D3DFMT_DXT5
 * ATI1
 * ATI2 - DXGI_FORMAT_BC5_UNORM
 * BC5U - 
 * BC5S - DXGI_FORMAT_BC5_SNORM
 * BC4U - DXGI_FORMAT_BC4_UNORM
 * BC4S - DXGI_FORMAT_BC4_SNORM
 * RGBG - DXGI_FORMAT_R8G8_B8G8_UNORM / D3DFMT_R8G8_B8G8
 * GRGB - DXGI_FORMAT_G8R8_G8B8_UNORM / D3DFMT_G8R8_G8B8
 * UYVY - D3DFMT_UYVY
 * YUY2 - D3DFMT_YUY2
 * DX10 - Any DXGI format
 * 36 - DXGI_FORMAT_R16G16B16A16_UNORM / D3DFMT_A16B16G16R16
 * 110 - DXGI_FORMAT_R16G16B16A16_SNORM / D3DFMT_Q16W16V16U16
 * 111 - DXGI_FORMAT_R16_FLOAT / D3DFMT_R16F
 * 112 - DXGI_FORMAT_R16G16_FLOAT / D3DFMT_G16R16F
 * 113 - DXGI_FORMAT_R16G16B16A16_FLOAT / D3DFMT_A16B16G16R16F
 * 114 - DXGI_FORMAT_R32_FLOAT / D3DFMT_R32F
 * 115 - DXGI_FORMAT_R32G32_FLOAT / D3DFMT_G32R32F
 * 116 - DXGI_FORMAT_R32G32B32A32_FLOAT / D3DFMT_A32B32G32R32F
 * 117 - D3DFMT_CxV8U8
*/
/*
        public static string D3DTX_GetDXTType_ForDDS(string parsed_dword, int parsed_dxtType)
        {
            string result = "DXT1";

            //this section needs some reworking, still can't track down exactly what the compression types are, parsed_compressionType and parsed_dxtType are close
            //SET DDS COMPRESSION TYPES
            if (parsed_dxtType == 66)
            {
                //DXT5 COMPRESSION
                result = "DXT5";
            }
            else if (parsed_dxtType == 68)
            {
                //DDSPF_BC5_UNORM COMPRESSION
                result = "BC5U";
            }
            else if (parsed_dxtType == 69)
            {
                //DDSPF_BC4_UNORM COMPRESSION
                result = "BC4U";
            }
            else if (parsed_dxtType == 646)
            {
                //DDSPF_BC4_UNORM COMPRESSION
                result = "BC5S";
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Selecting '{0}' DDS Compression.", result);

            return result;
        }

        public static int D3DTX_GetDXTType_ForD3DTX(string parsed_dword, string parsed_dxtType)
        {
            int result = 64; //DXT1

            //this section needs some reworking, still can't track down exactly what the compression types are, parsed_compressionType and parsed_dxtType are close
            //SET DDS COMPRESSION TYPES
            if (parsed_dxtType.Equals("DXT5"))
            {
                //DXT5 COMPRESSION
                result = 66;
            }
            else if (parsed_dxtType.Equals("BC5U"))
            {
                //DDSPF_BC5_UNORM COMPRESSION
                result = 68;
            }
            else if (parsed_dxtType.Equals("BC4U"))
            {
                //DDSPF_BC4_UNORM COMPRESSION
                result = 69;
            }

            ConsoleFunctions.SetConsoleColor(ConsoleColor.Black, ConsoleColor.Yellow); 
            Console.WriteLine("Selecting '{0}' for D3DTX Compression.", result.ToString());

            return result;
        }
*/
    }
}
