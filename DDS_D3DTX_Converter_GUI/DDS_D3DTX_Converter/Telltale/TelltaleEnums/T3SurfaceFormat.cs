namespace D3DTX_Converter.TelltaleEnums;

/*
    In order to correctly build your file and avoid visual errors within the game, you need to properly save the .dds file after editing it.
    Fist of all, you need to generate mip maps when saving the.dds file.
    Secondly, the file has to be saved in the correct format.
    To find out which format the .dds file has to be saved in, you need to check "mSurfaceFormat" parameter in the corresponding .json file or the Surface Format shown on the GUI.
    The parameter will contain a hexadecimal number. In order to learn, which format you need to save your file in, check the following table:
*/

public enum T3SurfaceFormat
{

    // Note: Apparently, some of these formats are equivalent to each other. It's just that some are used for Direct3D9, while others are used for Direct3D11.
    #region DXGI_FORMATS

    // Equivalent to DXGI_FORMAT_B8G8R8A8_UNORM or Direct3D9 D3DFMT_A8R8G8B8
    eSurface_ARGB8 = 0x0,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16
    // Note: Despite the name, the channels are not reversed. Poker Knight 2 has a single texture with this format and the colors are correct.
    eSurface_ARGB16 = 0x1,

    // Equivalent to DXGI_FORMAT_B5G6R5_UNORM or Direct3D9 D3DFMT_R5G6B5
    eSurface_RGB565 = 0x2,

    // Equivalent to DXGI_FORMAT_B5G5R5A1_UNORM or Direct3D9 D3DFMT_A1R5G5B5
    eSurface_ARGB1555 = 0x3,

    // Equivalent to DXGI_FORMAT_B4G4R4A4_UNORM or Direct3D9 D3DFMT_A4R4G4B4
    eSurface_ARGB4 = 0x4,

    // Equivalent to DXGI_FORMAT_R10G10B10A2_UNORM or Direct3D9 D3DFMT_A2B10G10R10
    // Note: Despite the name, the channels are not reversed. Poker Knight 2 has a single texture with this format and the colors are correct.
    eSurface_ARGB2101010 = 0x5,

    // Equivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
    eSurface_R16 = 0x6,

    // Equivalent to DXGI_FORMAT_R16G16_UNORM or Direct3D9 D3DFMT_G16R16
    eSurface_RG16 = 0x7,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16
    eSurface_RGBA16 = 0x8,

    // Equivalent to = DXGI_FORMAT_R8G8_UNORM or Direct3D9 D3DFMT_G8R8
    eSurface_RG8 = 0x9,

    // Equivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
    eSurface_RGBA8 = 0xA,

    // Equivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
    // It could be DXGI_FORMAT_R32_UINT
    eSurface_R32 = 0xB,

    // Equivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
    // It could be DXGI_FORMAT_R32G32_UINT
    eSurface_RG32 = 0xC,

    // Equivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
    // It could be DXGI_FORMAT_R32G32B32A32_UINT
    eSurface_RGBA32 = 0xD,

    // Equivalent to DXGI_FORMAT_R8_UNORM or Direct3D9 D3DFMT_L8
    eSurface_R8 = 0xE,

    // Equivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
    eSurface_RGBA8S = 0xF,

    // Equivalent to DXGI_FORMAT_A8_UNORM or Direct3D9 D3DFMT_A8
    eSurface_A8 = 0x10,

    // L8, AL8 and L16 are luminance formats, which are not supported by Direct3D11. 
    // These will be mapped to DXGI_FORMAT_R8, DXGI_FORMAT_R8G8 and DXGI_FORMAT_R16_UNORM respectively.
    // For further accuracy, mapping them RGBA8 and RGBA16 would be more accurate on what they represent, but it's not necessary.

    // Equivalent to DXGI_FORMAT_R8 or Direct3D9 D3DFMT_L8
    eSurface_L8 = 0x11, // Only one file found in Borderlands - mlaa_lookup

    // Equivalent to DXGI_FORMAT_R8G8 or Direct3D9 D3DFMT_A8L8
    eSurface_AL8 = 0x12,

    // Equivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
    eSurface_L16 = 0x13,

    // Equivalent to DXGI_FORMAT_R16G16_SINT or Direct3D9 D3DFMT_G16R16
    eSurface_RG16S = 0x14,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_SINT or Direct3D9 D3DFMT_A16B16G16R16
    eSurface_RGBA16S = 0x15,

    // Equivalent to DXGI_FORMAT_R16_UINT or Direct3D9 D3DFMT_R16F
    eSurface_R16UI = 0x16,

    // Equivalent to DXGI_FORMAT_R16G16_UINT or Direct3D9 D3DFMT_G16R16
    eSurface_RG16UI = 0x17,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UINT or Direct3D9 D3DFMT_A16B16G16R16
    eSurface_R16F = 0x20,

    // Equivalent to DXGI_FORMAT_R16G16_FLOAT or Direct3D9 D3DFMT_G16R16
    eSurface_RG16F = 0x21,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_FLOAT or Direct3D9 D3DFMT_A16B16G16R16
    eSurface_RGBA16F = 0x22,

    // Equivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
    eSurface_R32F = 0x23,

    // Equivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
    eSurface_RG32F = 0x24,

    // Equivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
    eSurface_RGBA32F = 0x25, // Used for light maps, 12 of them Borderlands

    // Equivalent to DXGI_FORMAT_R10G10B10A2_UNORM or Direct3D9 D3DFMT_A2B10G10R10
    eSurface_RGBA1010102F = 0x26,

    // Equivalent to DXGI_FORMAT_R11G11B10_FLOAT or Direct3D9 D3DFMT_R11G11B10F
    eSurface_RGB111110F = 0x27,

    // Equivalent to DXGI_FORMAT_R9G9B9E5_SHAREDEXP or Direct3D9 D3DFMT_R9G9B9E5
    eSurface_RGB9E5F = 0x28,

    #endregion
    /*
    PCF probably stands for percentage-closer filtering, which is used for shadow mapping. No idea why it's different than Depth16 and Depth24.
    https://github.com/bkaradzic/bimg/blob/master/src/image.cpp
    These are usually created run-time in the engine, so it's unlikely that you will need to use these formats.
    */
    eSurface_DepthPCF16 = 0x30, //30h D3DFMT, probably used for PC versions
    eSurface_DepthPCF24 = 0x31, //31h D3DFMT probably used for PC versions
    eSurface_Depth16 = 0x32,
    eSurface_Depth24 = 0x33,
    eSurface_DepthStencil32 = 0x34,
    eSurface_Depth32F = 0x35,
    eSurface_Depth32F_Stencil8 = 0x36,
    eSurface_Depth24F_Stencil8 = 0x37,

    #region Texture Compression Formats

    // Equivalent to DXGI_FORMAT_BC1_UNORM or Direct3D9 D3DFMT_DXT1
    // One alternative Telltale name would be eSurface_DXT1
    eSurface_BC1 = 0x40,

    // Equivalent to DXGI_FORMAT_BC2_UNORM or Direct3D9 D3DFMT_DXT3
    // One alternative Telltale name would be eSurface_DXT3
    eSurface_BC2 = 0x41,

    // Equivalent to DXGI_FORMAT_BC3_UNORM or Direct3D9 D3DFMT_DXT5
    // One alternative Telltale name would be eSurface_DXT5
    eSurface_BC3 = 0x42,

    // Equivalent to DXGI_FORMAT_BC4_UNORM or Direct3D9 D3DFMT_ATI1
    // One alternative Telltale name would be eSurface_DXT5A
    eSurface_BC4 = 0x43,

    // Equivalent to DXGI_FORMAT_BC5_UNORM or Direct3D9 D3DFMT_ATI2
    // One alternative Telltale name would be eSurface_DXN
    eSurface_BC5 = 0x44,


    // CTX1 is a format that according to the limited information that exists online, is specific to the Xbox360 platform. 
    // CTX1 is similar to DXN format in that it is a two channel texture designed for tangent space normal maps, but it is lower quality. 
    // Information for this format is scarce, and so are tools regarding compressing/decompressing the format. 
    // We will ignore support for this format for the time being especially as Xbox360 also had support for DXT compressions which were likely more commonly used than this one.
    // https://forum.xen-tax.com/viewtopic.php@p=83846.html 
    // https://github.com/Xenomega/Alteration/blob/master/Alteration/Halo%203/Map%20File/Raw/BitmapRaw/DXTDecoder.cs 
    // https://fileadmin.cs.lth.se/cs/Personal/Michael_Doggett/talks/unc-xenos-doggett.pdf)

    eSurface_CTX1 = 0x45,

    // Equivalent to DXGI_FORMAT_BC6H_UF16
    eSurface_BC6 = 0x46,

    // Equivalent to DXGI_FORMAT_BC7_UNORM
    eSurface_BC7 = 0x47,


    // The following formats are used for iOS/Android platforms only, which are not supported:
    // PVRTC2
    // PVRTC4
    // PVRTC2a
    // PVRTC4a
    // ATC_RGB 
    // ATC_RGB1A (Presumably 1 bit for the alpha) 
    // ATC_RGBA 
    // ETC1_RGB
    // ETC2_RGB  
    // ETC2_RGB1A 
    // ETC2_RGBA 
    // ETC2_R
    // ETC2_RG
    // ATSC_RGBA_4x4 

    // PVRTC1 2bpp RGB
    eSurface_PVRTC2 = 0x50, //50h

    // PVRTC1 4bpp RGB
    eSurface_PVRTC4 = 0x51, //51h

    // PVRTC 1 or 2 2bpp RGBA
    eSurface_PVRTC2a = 0x52, //52h

    // PVRTC 1 or 2 4bpp RGBA
    eSurface_PVRTC4a = 0x53, //53h
    eSurface_ATC_RGB = 0x60, //60h
    eSurface_ATC_RGB1A = 0x61, //61h
    eSurface_ATC_RGBA = 0x62, //62h
    eSurface_ETC1_RGB = 0x70, //70h
    eSurface_ETC2_RGB = 0x71, //71h
    eSurface_ETC2_RGB1A = 0x72, //72h

    // ETC2 EAC RGBA
    eSurface_ETC2_RGBA = 0x73, //73h
    // ETC2 EAC R11
    eSurface_ETC2_R = 0x74, //74h
    // ETC2 EAC RG11
    eSurface_ETC2_RG = 0x75, //75h

    // Presumably eSurface_ETC2_RGBM exists, but it is not used. 

    eSurface_ATSC_RGBA_4x4 = 0x80, //80h

    #endregion
    eSurface_FrontBuffer = 0x90, //90h

    eSurface_Count = 0x91, //91h
    eSurface_Unknown = unchecked((int)0xFFFFFFFF), //0FFFFFFFFh
}
