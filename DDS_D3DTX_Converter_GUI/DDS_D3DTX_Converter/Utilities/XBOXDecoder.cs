using System;
using D3DTX_Converter.TelltaleEnums;

public static class XboxTextureDecoder
{
    // Credits to https://github.com/BinarySerializer
    // https://github.com/BinarySerializer/BinarySerializer.UbiArt/tree/7f8ea72c3b4ede832dd14511f332bceecd320c0a
    public static byte[] UnswizzleXbox(byte[] imageData, int width, int height, T3SurfaceFormat format, bool swapBytes)
    {
        byte[] imgData = swapBytes ? new byte[imageData.Length] : imageData;

        if (swapBytes)
        {
            for (int i = 0; i < imageData.Length / 2; i++)
            {
                imgData[i * 2] = imageData[i * 2 + 1];
                imgData[i * 2 + 1] = imageData[i * 2];
            }
        }

        CPixelFormatInfo info = GetPixelFormatInfoFromT3SurfaceFormat(format);

        return UntileTexture(imgData, width, height, info.BytesPerBlock, info.X360AlignX, info.X360AlignY, info.BlockSizeX, info.BlockSizeY);
    }

    public enum TextureCompressionType : byte
    {
        DXT1 = 0x52,
        DXT3 = 0x53,
        DXT5 = 0x54
    }

    // Based on https://github.com/gildor2/UEViewer/blob/eaba2837228f9fe39134616d7bff734acd314ffb/Unreal/UnrealMaterial/UnTexture.cpp#L562
    private static byte[] UntileTexture(byte[] srcData, int originalWidth, int originalHeight, int bytesPerBlock, int alignX, int alignY, int blockSizeX, int blockSizeY)
    {
        var dstData = new byte[srcData.Length];

        int alignedWidth = Align(originalWidth, alignX);
        int alignedHeight = Align(originalHeight, alignY);

        int tiledBlockWidth = alignedWidth / blockSizeX;       // width of image in blocks
        int originalBlockWidth = originalWidth / blockSizeX;   // width of image in blocks
        int tiledBlockHeight = alignedHeight / blockSizeY;     // height of image in blocks
        int originalBlockHeight = originalHeight / blockSizeY; // height of image in blocks
        int logBpp = AppLog2(bytesPerBlock);

        // XBox360 has packed multiple lower mip levels into a single tile - should use special code
        // to unpack it. Textures are aligned to bottom-right corder.
        // Packing looks like this:
        // ....CCCCBBBBBBBBAAAAAAAAAAAAAAAA
        // ....CCCCBBBBBBBBAAAAAAAAAAAAAAAA
        // E.......BBBBBBBBAAAAAAAAAAAAAAAA
        // ........BBBBBBBBAAAAAAAAAAAAAAAA
        // DD..............AAAAAAAAAAAAAAAA
        // ................AAAAAAAAAAAAAAAA
        // ................AAAAAAAAAAAAAAAA
        // ................AAAAAAAAAAAAAAAA
        // (Where mips are A,B,C,D,E - E is 1x1, D is 2x2 etc)
        // Force sxOffset=0 and enable DEBUG_MIPS in UnRender.cpp to visualize this layout.
        // So we should offset X coordinate when unpacking to the width of mip level.
        // Note: this doesn't work with non-square textures.
        var sxOffset = 0;
        var syOffset = 0;

        // We're handling only size=16 here.
        if (tiledBlockWidth >= originalBlockWidth * 2 && originalWidth == 16)
            sxOffset = originalBlockWidth;

        if (tiledBlockHeight >= originalBlockHeight * 2 && originalHeight == 16)
            syOffset = originalBlockHeight;

        int numImageBlocks = tiledBlockWidth * tiledBlockHeight;    // used for verification

        // Iterate over image blocks
        for (int dy = 0; dy < originalBlockHeight; dy++)
        {
            for (int dx = 0; dx < originalBlockWidth; dx++)
            {
                // Unswizzle only once for a whole block
                uint swzAddr = GetTiledOffset(dx + sxOffset, dy + syOffset, tiledBlockWidth, logBpp);

                if (swzAddr >= numImageBlocks)
                    throw new Exception("Error in Xbox 360 texture parsing");

                int sy = (int)(swzAddr / tiledBlockWidth);
                int sx = (int)(swzAddr % tiledBlockWidth);

                int dstStart = (dy * originalBlockWidth + dx) * bytesPerBlock;
                int srcStart = (sy * tiledBlockWidth + sx) * bytesPerBlock;
                Array.Copy(srcData, srcStart, dstData, dstStart, bytesPerBlock);
            }
        }

        return dstData;
    }

    private static uint GetTiledOffset(int x, int y, int width, int logBpb)
    {
        if (width > 8192)
            throw new Exception($"Xbox 360 texture: Width {width} too large");

        if (width <= x)
            throw new Exception($"Xbox 360 texture: X {x} too large for width {width}");

        int alignedWidth = Align(width, 32);
        // top bits of coordinates
        int macro = ((x >> 5) + (y >> 5) * (alignedWidth >> 5)) << (logBpb + 7);
        // lower bits of coordinates (result is 6-bit value)
        int micro = ((x & 7) + ((y & 0xE) << 2)) << logBpb;
        // mix micro/macro + add few remaining x/y bits
        int offset = macro + ((micro & ~0xF) << 1) + (micro & 0xF) + ((y & 1) << 4);
        // mix bits again
        return (uint)((((offset & ~0x1FF) << 3) +            // upper bits (offset bits [*-9])
                       ((y & 16) << 7) +                           // next 1 bit
                       ((offset & 0x1C0) << 2) +                   // next 3 bits (offset bits [8-6])
                       (((((y & 8) >> 2) + (x >> 3)) & 3) << 6) +  // next 2 bits
                       (offset & 0x3F)                             // lower 6 bits (offset bits [5-0])
            ) >> logBpb);
    }

    private static int Align(int value, int align) => (value % align != 0) ? ((value / align) + 1) * (align) : value;

    private static int AppLog2(int n)
    {
        int r;
        for (r = -1; n != 0; n >>= 1, r++) { /*empty*/ }
        return r;
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
        new CPixelFormatInfo(0, 4, 4, 8, 128, 128, 0, "DXT1"),
        new CPixelFormatInfo(0, 4, 4, 16, 128, 128, 0, "DXT3"),
        new CPixelFormatInfo(0, 4, 4, 16, 128, 128, 0, "DXT5"),
        new CPixelFormatInfo(0, 4, 4, 8, 0, 0, 0, "BC4"),
        new CPixelFormatInfo(0, 4, 4, 16, 0, 0, 0, "BC5"),
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

    // Other classes and enums needed (CPixelFormatInfo, CMipMap, TexturePixelFormat, etc.)
    // These need to be defined as per the actual implementation
}

