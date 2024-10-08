namespace TelltaleTextureTool.TelltaleEnums;

/*
    In order to correctly build your file and avoid visual errors within the game, you need to properly save the .dds file after editing it.
    Fist of all, you need to generate mip maps when saving the.dds file.
    Secondly, the file has to be saved in the correct format.
    To find out which format the .dds file has to be saved in, you need to check "mSurfaceFormat" parameter in the corresponding .json file or the Surface Format shown on the GUI.
    The parameter will contain a hexadecimal number. In order to learn, which format you need to save your file in, check the following table:
*/

// In Telltale the enum is represented using Hungarian notation (eSurface_formatName).
public enum T3SurfaceFormat
{

    // Note: Apparently, some of these formats are equivalent to each other. It's just that some are used for Direct3D9, while others are used for Direct3D11.
    #region DXGI_FORMATS / D3DFMT

    // Equivalent to DXGI_FORMAT_B8G8R8A8_UNORM or Direct3D9 D3DFMT_A8R8G8B8
    ARGB8 = 0x0,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16
    // Note: Despite the name, the channels are not reversed. Poker Knight 2 has a single texture with this format and the colors are correct.
    ARGB16 = 0x1,

    // Equivalent to DXGI_FORMAT_B5G6R5_UNORM or Direct3D9 D3DFMT_R5G6B5
    RGB565 = 0x2,

    // Equivalent to DXGI_FORMAT_B5G5R5A1_UNORM or Direct3D9 D3DFMT_A1R5G5B5
    ARGB1555 = 0x3,

    // Equivalent to DXGI_FORMAT_B4G4R4A4_UNORM or Direct3D9 D3DFMT_A4R4G4B4
    ARGB4 = 0x4,

    // Equivalent to DXGI_FORMAT_R10G10B10A2_UNORM or Direct3D9 D3DFMT_A2B10G10R10
    // It could be DXGI_FORMAT_R10G10B10A2_UINT
    ARGB2101010 = 0x5,

    // Equivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
    R16 = 0x6,

    // Equivalent to DXGI_FORMAT_R16G16_UNORM or Direct3D9 D3DFMT_G16R16
    RG16 = 0x7,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UNORM or Direct3D9 D3DFMT_A16B16G16R16
    RGBA16 = 0x8,

    // Equivalent to = DXGI_FORMAT_R8G8_UNORM or Direct3D9 D3DFMT_G8R8
    RG8 = 0x9,

    // Equivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
    RGBA8 = 0xA,

    // Equivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
    // It could be DXGI_FORMAT_R32_UINT
    R32 = 0xB,

    // Equivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
    // It could be DXGI_FORMAT_R32G32_UINT
    RG32 = 0xC,

    // Equivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
    // It could be DXGI_FORMAT_R32G32B32A32_UINT
    RGBA32 = 0xD,

    // Equivalent to DXGI_FORMAT_R8_UNORM or Direct3D9 D3DFMT_L8
    R8 = 0xE,

    // Equivalent to DXGI_FORMAT_R8G8B8A8_UNORM or Direct3D9 D3DFMT_A8B8G8R8
    RGBA8S = 0xF,

    // Equivalent to DXGI_FORMAT_A8_UNORM or Direct3D9 D3DFMT_A8
    A8 = 0x10,

    // L8, AL8 and L16 are luminance formats, which are not supported by Direct3D11. 
    // These will be mapped to DXGI_FORMAT_R8, DXGI_FORMAT_R8G8 and DXGI_FORMAT_R16_UNORM respectively.
    // For further accuracy, mapping them RGBA8 and RGBA16 would be more accurate on what they represent, but it's not necessary.

    // Equivalent to DXGI_FORMAT_R8 or Direct3D9 D3DFMT_L8
    L8 = 0x11, // Only one file found in Borderlands - mlaa_lookup

    // Equivalent to DXGI_FORMAT_R8G8 or Direct3D9 D3DFMT_A8L8
    AL8 = 0x12,

    // Equivalent to DXGI_FORMAT_R16_UNORM or Direct3D9 D3DFMT_R16F
    L16 = 0x13,

    // Equivalent to DXGI_FORMAT_R16G16_SINT or Direct3D9 D3DFMT_G16R16
    RG16S = 0x14,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_SINT or Direct3D9 D3DFMT_A16B16G16R16
    RGBA16S = 0x15,

    // Equivalent to DXGI_FORMAT_R16_UINT or Direct3D9 D3DFMT_R16F
    R16UI = 0x16,

    // Equivalent to DXGI_FORMAT_R16G16_UINT or Direct3D9 D3DFMT_G16R16
    RG16UI = 0x17,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_UINT or Direct3D9 D3DFMT_A16B16G16R16
    R16F = 0x20,

    // Equivalent to DXGI_FORMAT_R16G16_FLOAT or Direct3D9 D3DFMT_G16R16
    RG16F = 0x21,

    // Equivalent to DXGI_FORMAT_R16G16B16A16_FLOAT or Direct3D9 D3DFMT_A16B16G16R16
    RGBA16F = 0x22,

    // Equivalent to DXGI_FORMAT_R32_FLOAT or Direct3D9 D3DFMT_R32F
    R32F = 0x23,

    // Equivalent to DXGI_FORMAT_R32G32_FLOAT or Direct3D9 D3DFMT_G32R32
    RG32F = 0x24,

    // Equivalent to DXGI_FORMAT_R32G32B32A32_FLOAT or Direct3D9 D3DFMT_A32B32G32R32F
    RGBA32F = 0x25, // Used for light maps, 12 of them Borderlands

    // Equivalent to DXGI_FORMAT_R10G10B10A2_UNORM or Direct3D9 D3DFMT_A2B10G10R10
    RGBA1010102F = 0x26,

    // Equivalent to DXGI_FORMAT_R11G11B10_FLOAT or Direct3D9 D3DFMT_R11G11B10F
    RGB111110F = 0x27,

    // Equivalent to DXGI_FORMAT_R9G9B9E5_SHAREDEXP or Direct3D9 D3DFMT_R9G9B9E5
    RGB9E5F = 0x28,

    #endregion

    #region Depth/Stencil Formats
    /*
    PCF probably stands for percentage-closer filtering, which is used for shadow mapping. No idea why it's different than Depth16 and Depth24.
    https://github.com/bkaradzic/bimg/blob/master/src/image.cpp
    These are usually created run-time in the engine, so it's unlikely that you will need to use these formats.
    */
    DepthPCF16 = 0x30, // D3DFMT, probably used for PC versions D16
    DepthPCF24 = 0x31, // D3DFMT probably used for PC versions D24S8
    Depth16 = 0x32,
    Depth24 = 0x33,
    DepthStencil32 = 0x34,
    Depth32F = 0x35,
    Depth32F_Stencil8 = 0x36,
    Depth24F_Stencil8 = 0x37,

    #endregion

    #region S3TC Formats

    // Equivalent to DXGI_FORMAT_BC1_UNORM or Direct3D9 D3DFMT_DXT1
    // In Telltale Tool it is named eSurface_DXT1
    BC1 = 0x40,

    // Equivalent to DXGI_FORMAT_BC2_UNORM or Direct3D9 D3DFMT_DXT3
    // In Telltale Tool it is named eSurface_DXT3
    BC2 = 0x41,

    // Equivalent to DXGI_FORMAT_BC3_UNORM or Direct3D9 D3DFMT_DXT5
    // In Telltale Tool it is named eSurface_DXT5
    BC3 = 0x42,

    // Equivalent to DXGI_FORMAT_BC4_UNORM or Direct3D9 D3DFMT_ATI1
    // In Telltale Tool it is named eSurface_DXT5A
    BC4 = 0x43,

    // Equivalent to DXGI_FORMAT_BC5_UNORM or Direct3D9 D3DFMT_ATI2
    // In Telltale Tool it is named eSurface_DXN
    BC5 = 0x44,

    // CTX1 is a format that according to the limited information that exists online, is specific to the Xbox360 platform. 
    // CTX1 is similar to DXT1 format in that it is a two channel texture designed for tangent space normal maps, but it is lower quality. 
    // Information for this format is scarce, and so are tools regarding compressing/decompressing the format. 
    // We will ignore support for this format for the time being especially as Xbox360 also had support for DXT compressions which were likely more commonly used than this one.
    // https://forum.xen-tax.com/viewtopic.php@p=83846.html 
    // https://github.com/Xenomega/Alteration/blob/master/Alteration/Halo%203/Map%20File/Raw/BitmapRaw/DXTDecoder.cs 
    // https://fileadmin.cs.lth.se/cs/Personal/Michael_Doggett/talks/unc-xenos-doggett.pdf)

    CTX1 = 0x45,

    // Equivalent to DXGI_FORMAT_BC6H_UF16
    BC6 = 0x46,

    // Equivalent to DXGI_FORMAT_BC7_UNORM
    BC7 = 0x47,

    #endregion

    #region Mobile Formats

    // PVRTC1 2bpp RGB
    PVRTC2 = 0x50, //50h

    // PVRTC1 4bpp RGB
    PVRTC4 = 0x51, //51h

    // PVRTC 1 2bpp RGBA
    PVRTC2a = 0x52, //52h

    // PVRTC 1 4bpp RGBA
    PVRTC4a = 0x53, //53h
    ATC_RGB = 0x60, //60h ATC
    ATC_RGB1A = 0x61, //61h ATC Explicit Alpha
    ATC_RGBA = 0x62, //62h ATC Interpolated Alpha
    ETC1_RGB = 0x70, //70h

    // VK_FORMAT_ETC2_R8G8B8_SRGB_BLOCK = 148,
    ETC2_RGB = 0x71, //71h

    // VK_FORMAT_ETC2_R8G8B8A1_UNORM_BLOCK
    ETC2_RGB1A = 0x72, //72h

    // ETC2 EAC RGBA
    ETC2_RGBA = 0x73, //73h
    // ETC2 EAC R11
    ETC2_R = 0x74, //74h
    // ETC2 EAC RG11
    ETC2_RG = 0x75, //75h

    ATSC_RGBA_4x4 = 0x80, //80h

    #endregion
    FrontBuffer = 0x90, //90h

    Count = 0x91, //91h
    Unknown = unchecked((int)0xFFFFFFFF), //0FFFFFFFFh
}
