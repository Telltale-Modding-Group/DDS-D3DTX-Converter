namespace D3DTX_Converter.TelltaleEnums
{
    public enum T3SurfaceFormat
    {

        #region DXGI_FORMATS
        eSurface_ARGB8 = 0,
        eSurface_ARGB16 = 1,
        eSurface_RGB565 = 2,
        eSurface_ARGB1555 = 3,
        eSurface_ARGB4 = 4,
        eSurface_ARGB2101010 = 5,
        eSurface_R16 = 6,
        eSurface_RG16 = 7,
        eSurface_RGBA16 = 8,
        eSurface_RG8 = 9,
        eSurface_RGBA8 = 10, //0Ah
        eSurface_R32 = 11, //0Bh
        eSurface_RG32 = 12, //0Ch
        eSurface_RGBA32 = 13, //0Dh
        eSurface_R8 = 14, //0Eh
        eSurface_RGBA8S = 15, //0Fh
        eSurface_A8 = 16, //10h
        eSurface_L8 = 17, //11h
        eSurface_AL8 = 18, //12h
        eSurface_L16 = 19, //13h
        eSurface_RG16S = 20, //14h
        eSurface_RGBA16S = 21, //15h
        eSurface_R16UI = 22, //16h
        eSurface_RG16UI = 23, //17h
        eSurface_R16F = 32, //20h
        eSurface_RG16F = 33, //21h
        eSurface_RGBA16F = 34, //22h
        eSurface_R32F = 35, //23h
        eSurface_RG32F = 36, //24h
        eSurface_RGBA32F = 37, //25h
        eSurface_RGBA1010102F = 30, //26h
        eSurface_RGB111110F = 31, //27h
        eSurface_RGB9E5F = 200, //28h

        #endregion
        /*
        PCF probably stands for percentage-closer filtering, which is used for shadow mapping. No idea why it's different than Depth16 and Depth24.
        */
        eSurface_DepthPCF16 = 48, //30h
        eSurface_DepthPCF24 = 201, //31h
        eSurface_Depth16 = 50, //32h
        eSurface_Depth24 = 51, //33h
        eSurface_DepthStencil32 = 52, //34h
        eSurface_Depth32F = 53, //35h
        eSurface_Depth32F_Stencil8 = 54, //36h
        eSurface_Depth24F_Stencil8 = 40, //37h
        eSurface_BC1 = 41, //40h
        eSurface_DXT1 = 64, //40h
        eSurface_BC2 = 43, //41h
        eSurface_DXT3 = 65, //41h
        eSurface_BC3 = 45, //42h
        eSurface_DXT5 = 66, //42h
        eSurface_BC4 = 47, //43h
        eSurface_DXT5A = 67, //43h
        eSurface_BC5 = 49, //44h

        /*
        Variant of BC5 compression
        */
        eSurface_DXN = 68, //44h 

        /*
        CTX1 is a format that according to the limited information that exists online, is specific to the Xbox360 platform. 
        CTX1 is similar to DXN format in that it is a two channel texture designed for tangent space normal maps, but it is lower quality. 
        Now information for this format is scarce, and so are tools regarding compressing/decompressing the format. 
        So with that we will ignore support for this format for the time being especially as Xbox360 also had support for DXT compressions which were likely more commonly used than this one.
        https://forum.xen-tax.com/viewtopic.php@p=83846.html 
        https://github.com/Xenomega/Alteration/blob/master/Alteration/Halo%203/Map%20File/Raw/BitmapRaw/DXTDecoder.cs 
        https://fileadmin.cs.lth.se/cs/Personal/Michael_Doggett/talks/unc-xenos-doggett.pdf)
        */
        eSurface_CTX1 = 69, //45h
        eSurface_BC6 = 70, //46h
        eSurface_BC7 = 71, //47h

        /*
        The following formats are used for iOS/Android platforms only:
        PVRTC2
        PVRTC4
        PVRTC2a
        PVRTC4a
        ATC_RGB
        ATC_RGB1A (Presumably 1 bit for the alpha)
        ATC_RGBA
        ETC1_RGB
        ETC2_RGB
        ETC2_RGB1A
        ETC2_RGBA
        ETC2_R
        ETC2_RG
        ATSC_RGBA_4x4 
        */
        eSurface_PVRTC2 = 80, //50h
        eSurface_PVRTC4 = 81, //51h
        eSurface_PVRTC2a = 82, //52h
        eSurface_PVRTC4a = 83, //53h
        eSurface_ATC_RGB = 96, //60h
        eSurface_ATC_RGB1A = 97, //61h
        eSurface_ATC_RGBA = 98, //62h
        eSurface_ETC1_RGB = 112, //70h
        eSurface_ETC2_RGB = 113, //71h
        eSurface_ETC2_RGB1A = 114, //72h
        eSurface_ETC2_RGBA = 115, //73h
        eSurface_ETC2_R = 116, //74h
        eSurface_ETC2_RG = 117, //75h
        eSurface_ATSC_RGBA_4x4 = 128, //80h
        eSurface_FrontBuffer = 202, //90h

        /*
        Probably related to RLE encoding, which is rarely used in texture compression.

        Example: WWWWWWWWWWWWBWWWWWWWWWWWWBBBWWWWWWWWWWWWWWWWWWWWWWWWBWWWWWWWWWWWWWW 
        With a run-length encoding (RLE) data compression algorithm applied to the above hypothetical scan line, it can be rendered as follows:
        12W1B12W3B24W1B14W 
        */
        eSurface_Count = 203, //91h
        eSurface_Unknown = -1, //0FFFFFFFFh
    }
}
