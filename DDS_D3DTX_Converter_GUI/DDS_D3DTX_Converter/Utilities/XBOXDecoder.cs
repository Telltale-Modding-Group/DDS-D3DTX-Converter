

using System;
using System.Diagnostics;
using D3DTX_Converter.TelltaleEnums;

public static class Xbox360Texture
{
    public static int AppLog2(int n)
    {
        int r;
        for (r = -1; n != 0; n >>= 1, r++) { /*empty*/ }
        return r;
    }

    public static uint GetXbox360TiledOffset(int x, int y, int width, int logBpb)
    {
        Debug.Assert(width <= 8192);
        Debug.Assert(x < width);

        int alignedWidth = Align(width, 32);
        int macro = ((x >> 5) + (y >> 5) * (alignedWidth >> 5)) << (logBpb + 7);
        int micro = ((x & 7) + ((y & 0xE) << 2)) << logBpb;
        int offset = macro + ((micro & ~0xF) << 1) + (micro & 0xF) + ((y & 1) << 4);

        return (uint)((((offset & ~0x1FF) << 3) +
                        ((y & 16) << 7) +
                        ((offset & 0x1C0) << 2) +
                        ((((y & 8) >> 2) + (x >> 3)) & 3) << 6) +
                        (offset & 0x3F)) >> logBpb;
    }

    public static void UntileXbox360Texture(uint[] src, uint[] dst, int tiledWidth, int originalWidth, int height, int blockSizeX, int blockSizeY, int bytesPerBlock)
    {
        int blockWidth = tiledWidth / blockSizeX;
        int originalBlockWidth = originalWidth / blockSizeX;
        int blockHeight = height / blockSizeY;
        int logBpp = AppLog2(bytesPerBlock);

        int numImageBlocks = blockWidth * blockHeight;

        for (int y = 0; y < blockHeight; y++)
        {
            for (int x = 0; x < originalBlockWidth; x++)
            {
                uint swzAddr = GetXbox360TiledOffset(x, y, blockWidth, logBpp);
                Debug.Assert(swzAddr < numImageBlocks);
                int sy = (int)swzAddr / blockWidth;
                int sx = (int)swzAddr % blockWidth;

                int y2 = y * blockSizeY;
                int y3 = sy * blockSizeY;

                for (int y1 = 0; y1 < blockSizeY; y1++, y2++, y3++)
                {
                    int x2 = x * blockSizeX;
                    int x3 = sx * blockSizeX;
                    for (int x1 = 0; x1 < blockSizeX; x1++)
                        dst[y2 * originalWidth + x2 + x1] = src[y3 * tiledWidth + x3 + x1];
                }
            }
        }
    }

    public static byte[] UntileCompressedXbox360Texture(byte[] src, int originalWidth, int originalHeight, CPixelFormatInfo info)
    {
        byte[] dst = new byte[src.Length];

        int alignedWidth = Align(originalWidth, info.X360AlignX);
        int alignedHeight = Align(originalHeight, info.X360AlignY);

        int tiledBlockWidth = alignedWidth / info.BlockSizeX;
        int originalBlockWidth = originalWidth / info.BlockSizeX;
        int tiledBlockHeight = alignedHeight / info.BlockSizeY;
        int originalBlockHeight = originalHeight / info.BlockSizeY;
        int logBpp = AppLog2(info.BytesPerBlock);

        int sxOffset = 0, syOffset = 0;
        if (tiledBlockWidth >= originalBlockWidth * 2 && originalWidth == 16)
            sxOffset = originalBlockWidth;
        if (tiledBlockHeight >= originalBlockHeight * 2 && originalHeight == 16)
            syOffset = originalBlockHeight;

        int numImageBlocks = tiledBlockWidth * tiledBlockHeight;

        int bytesPerBlock = info.BytesPerBlock;
        for (int dy = 0; dy < originalBlockHeight; dy++)
        {
            for (int dx = 0; dx < originalBlockWidth; dx++)
            {
                uint swzAddr = GetXbox360TiledOffset(dx + sxOffset, dy + syOffset, tiledBlockWidth, logBpp);
                Debug.Assert(swzAddr < numImageBlocks);
                int sy = (int)swzAddr / tiledBlockWidth;
                int sx = (int)swzAddr % tiledBlockWidth;

                Array.Copy(src, (sy * tiledBlockWidth + sx) * bytesPerBlock, dst, (dy * originalBlockWidth + dx) * bytesPerBlock, bytesPerBlock);
            }
        }

        return dst;
    }

    public static byte[] DecodeXbox360(byte[] src, T3SurfaceFormat format, int width, int height)
    {
        CPixelFormatInfo info = GetPixelFormatInfoFromT3SurfaceFormat(format);

        if (info.X360AlignX == 0)
        {
            throw new Exception("ERROR: DecodeXBox360 {0}'{1}' mip {2} ({3}={4}): unsupported texture format");
        }

        int USize1 = Align(width, info.X360AlignX);
        int VSize1 = Align(height, info.X360AlignY);
        int UBlockSize = USize1 / info.BlockSizeX;
        int VBlockSize = VSize1 / info.BlockSizeY;
        int TotalBlocks = src.Length / info.BytesPerBlock;

        float bpp = (float)src.Length / (USize1 * VSize1) * info.BlockSizeX * info.BlockSizeY;

        if (UBlockSize * VBlockSize > TotalBlocks)
            throw new Exception("ERROR: DecodeXBox360 {0}'{1}' mip {2} ({3}={4}): not enough data");

        if (Math.Abs(bpp - info.BytesPerBlock) > 0.001f)
        {
            if (height >= 32 || (bpp * 2 != info.BytesPerBlock && bpp * 4 != info.BytesPerBlock))
            {
                throw new Exception("ERROR: DecodeXBox360 {0}'{1}' mip {2} ({3}={4}): unsupported texture format");
            }
        }

        //START UNTILING 
        src = UntileCompressedXbox360Texture(src, width, height, info);

        if (format == T3SurfaceFormat.eSurface_ARGB8 || format == T3SurfaceFormat.eSurface_RGBA8 || format == T3SurfaceFormat.eSurface_RGBA8S)
        {
            AppReverseBytes(src, src.Length / 4, 4);
        }
        else if (info.BytesPerBlock > 1)
        {
            AppReverseBytes(src, src.Length / 2, 2);
        }

        return src;
        //END UNTILING
    }

    private static int Align(int value, int alignment)
    {
        return (value + alignment - 1) & ~(alignment - 1);
    }

    private static void AppReverseBytes(byte[] buf, int count, int size)
    {
        for (int i = 0; i < count; i++)
        {
            Array.Reverse(buf, i * size, size);
        }
    }

    private static void AppNotify(string format, params object[] args)
    {
        Console.WriteLine(format, args);
    }

    public struct CPixelFormatInfo
    {
        public int FourCC;
        public int BlockSizeX;
        public int BlockSizeY;
        public int BytesPerBlock;
        public int X360AlignX;
        public int X360AlignY;
        public int Float;
        public string Name;

        public CPixelFormatInfo(int fourCC, int blockSizeX, int blockSizeY, int bytesPerBlock, int x360AlignX, int x360AlignY, int floatFlag, string name)
        {
            FourCC = fourCC;
            BlockSizeX = blockSizeX;
            BlockSizeY = blockSizeY;
            BytesPerBlock = bytesPerBlock;
            X360AlignX = x360AlignX;
            X360AlignY = x360AlignY;
            Float = floatFlag;
            Name = name;
        }
    }

    public static CPixelFormatInfo[] PixelFormatInfo =
    [
        new CPixelFormatInfo(0, 0, 0, 0, 0, 0, 0, "UNKNOWN"),
        new CPixelFormatInfo(MakeFourCC('D', 'X', 'T', '1'), 4, 4, 8, 128, 128, 0, "DXT1"),
        new CPixelFormatInfo(MakeFourCC('D', 'X', 'T', '3'), 4, 4, 16, 128, 128, 0, "DXT3"),
        new CPixelFormatInfo(MakeFourCC('D', 'X', 'T', '5'), 4, 4, 16, 128, 128, 0, "DXT5"),
        new CPixelFormatInfo(MakeFourCC('A', 'T', 'I', '1'), 4, 4, 8, 0, 0, 0, "BC4"),
        new CPixelFormatInfo(MakeFourCC('A', 'T', 'I', '2'), 4, 4, 16, 0, 0, 0, "BC5"),
        new CPixelFormatInfo(0, 4, 4, 16, 0, 0, 1, "BC6H"),
        new CPixelFormatInfo(0, 4, 4, 16, 0, 0, 0, "BC7"),
        new CPixelFormatInfo(0, 1, 1, 4, 32, 32, 0, "RGBA8"),
        new CPixelFormatInfo(0, 1, 1, 4, 32, 32, 0, "BGRA8"),
        new CPixelFormatInfo(0, 1, 1, 2, 0, 0, 0, "RGBA4"),
        new CPixelFormatInfo(0, 1, 1, 1, 64, 64, 0, "G8"),
        new CPixelFormatInfo(0, 1, 1, 2, 64, 32, 0, "V8U8"),

        new CPixelFormatInfo(0, 8, 1, 1, 0, 0, 0, "A1"),

        new CPixelFormatInfo(0, 8, 4, 8, 0, 0, 0, "PVRTC2"),    // TPF_PVRTC2
        new CPixelFormatInfo(0, 4, 4, 8, 0, 0, 0, "PVRTC4"),    // TPF_PVRTC4
        new CPixelFormatInfo(0, 4, 4, 8, 0, 0, 0, "ETC1"),      // TPF_ETC1
        new CPixelFormatInfo(0, 4, 4, 8, 0, 0, 0, "ETC2_RGB"),  // TPF_ETC2_RGB
        new CPixelFormatInfo(0, 4, 4, 16, 0, 0, 0, "ETC2_RGBA"),// TPF_ETC2_RGBA
        new CPixelFormatInfo(0, 4, 4, 16, 0, 0, 0, "ATC_4x4"),  // TPF_ASTC_4x4

        new CPixelFormatInfo(0, 1, 1, 3, 0, 0, 0, "RGB8"), //I need to figure out legacy versions of this
        new CPixelFormatInfo(0, 1, 1, 16, 0, 0, 1, "FLOAT_RGBA"),
        new CPixelFormatInfo(0, 1, 1, 0, 0, 0, 0, "PNG_BGRA"),
        new CPixelFormatInfo(0, 1, 1, 0, 0, 0, 0, "PNG_RGBA"),
    ];

    private static CPixelFormatInfo GetPixelFormatInfoFromT3SurfaceFormat(T3SurfaceFormat format)
    {
        return format switch
        {
            T3SurfaceFormat.eSurface_BC1 => PixelFormatInfo[1],
            T3SurfaceFormat.eSurface_BC2 => PixelFormatInfo[2],
            T3SurfaceFormat.eSurface_BC3 => PixelFormatInfo[3],
            T3SurfaceFormat.eSurface_BC4 => PixelFormatInfo[4],
            T3SurfaceFormat.eSurface_BC5 => PixelFormatInfo[5],
            T3SurfaceFormat.eSurface_BC6 => PixelFormatInfo[6],
            T3SurfaceFormat.eSurface_BC7 => PixelFormatInfo[7],
            T3SurfaceFormat.eSurface_RGBA8 => PixelFormatInfo[8],
            T3SurfaceFormat.eSurface_RGBA8S => PixelFormatInfo[8],
            T3SurfaceFormat.eSurface_ARGB8 => PixelFormatInfo[9],
            T3SurfaceFormat.eSurface_ARGB4 => PixelFormatInfo[10],
            T3SurfaceFormat.eSurface_A8 => PixelFormatInfo[11], //??
            T3SurfaceFormat.eSurface_L8 => PixelFormatInfo[11],
            T3SurfaceFormat.eSurface_R8 => PixelFormatInfo[11],
            T3SurfaceFormat.eSurface_AL8 => PixelFormatInfo[12],
            T3SurfaceFormat.eSurface_RG8 => PixelFormatInfo[12],

            _ => PixelFormatInfo[0],
        };
    }

    private static int MakeFourCC(char ch0, char ch1, char ch2, char ch3)
    {
        return (int)(byte)ch0 | ((int)(byte)ch1 << 8) | ((int)(byte)ch2 << 16) | ((int)(byte)ch3 << 24);
    }

    // Other classes and enums needed (CPixelFormatInfo, CMipMap, TexturePixelFormat, etc.)
    // These need to be defined as per the actual implementation
}

