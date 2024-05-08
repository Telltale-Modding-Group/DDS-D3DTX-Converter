using D3DTX_Converter.TelltaleEnums;
using D3DTX_Converter.Utilities;
using DirectXTexNet;

namespace D3DTX_Converter.DirectX;

/// <summary>
/// Defines the various types of surface formats.
/// </summary>
public enum D3DFORMAT : uint
{
    UNKNOWN = 0,

    R8G8B8 = 20,
    A8R8G8B8 = 21,
    X8R8G8B8 = 22,
    R5G6B5 = 23,
    X1R5G5B5 = 24,
    A1R5G5B5 = 25,
    A4R4G4B4 = 26,
    R3G3B2 = 27,
    A8 = 28,
    A8R3G3B2 = 29,
    X4R4G4B4 = 30,
    A2B10G10R10 = 31,
    A8B8G8R8 = 32,
    X8B8G8R8 = 33,
    G16R16 = 34,
    A2R10G10B10 = 35,
    A16B16G16R16 = 36,

    A8P8 = 40,
    P8 = 41,

    L8 = 50,
    A8L8 = 51,
    A4L4 = 52,

    V8U8 = 60,
    L6V5U5 = 61,
    X8L8V8U8 = 62,
    Q8W8V8U8 = 63,
    V16U16 = 64,
    A2W10V10U10 = 67,

    UYVY = 0x59565955, // 'UYVY'
    R8G8_B8G8 = 0x47424752, // 'RGBG'
    YUY2 = 0x32595559, // 'YUY2'
    G8R8_G8B8 = 0x42475247, // 'GRGB'
    DXT1 = 0x31545844, // 'DXT1'
    DXT2 = 0x32545844, // 'DXT2'
    DXT3 = 0x33545844, // 'DXT3'
    DXT4 = 0x34545844, // 'DXT4'
    DXT5 = 0x35545844, // 'DXT5'

    D16_LOCKABLE = 70,
    D32 = 71,
    D15S1 = 73,
    D24S8 = 75,
    D24X8 = 77,
    D24X4S4 = 79,
    D16 = 80,

    D32F_LOCKABLE = 82,
    D24FS8 = 83,

    D32_LOCKABLE = 84,
    S8_LOCKABLE = 85,

    L16 = 81,

    VERTEXDATA = 100,
    INDEX16 = 101,
    INDEX32 = 102,

    Q16W16V16U16 = 110,

    MULTI2_ARGB8 = 0x3145544D, // 'MET1'

    R16F = 111,
    G16R16F = 112,
    A16B16G16R16F = 113,

    R32F = 114,
    G32R32F = 115,
    A32B32G32R32F = 116,

    CxV8U8 = 117,

    A1 = 118,
    A2B10G10R10_XR_BIAS = 119,
    BINARYBUFFER = 199,

    AI44 = 0x34344941, // 'AI44'
    IA44 = 0x34344149, // 'IA44'
    YV12 = 0x32315659, // 'YV12'

    FORCE_DWORD = 0x7fffffff
}

public class D3D9FormatConverter
{

    public static DXGI_FORMAT GetFromD3FORMATToDXGIFormat(D3DFORMAT value)
    {
        return value switch
        {
            D3DFORMAT.A8R8G8B8 => DXGI_FORMAT.R8G8B8A8_UNORM,
            D3DFORMAT.X8R8G8B8 => DXGI_FORMAT.B8G8R8X8_UNORM,
            D3DFORMAT.R5G6B5 => DXGI_FORMAT.B5G5R5A1_UNORM,
            D3DFORMAT.A1R5G5B5 => DXGI_FORMAT.B5G5R5A1_UNORM,
            D3DFORMAT.A4R4G4B4 => DXGI_FORMAT.B4G4R4A4_UNORM,
            D3DFORMAT.A8 => DXGI_FORMAT.A8_UNORM,
            D3DFORMAT.A2B10G10R10 => DXGI_FORMAT.R10G10B10A2_UNORM,
            D3DFORMAT.A8B8G8R8 => DXGI_FORMAT.R8G8B8A8_UNORM,
            D3DFORMAT.X8B8G8R8 => DXGI_FORMAT.G8R8_G8B8_UNORM,
            D3DFORMAT.G16R16 => DXGI_FORMAT.R16G16_UNORM,
            D3DFORMAT.A2R10G10B10 => DXGI_FORMAT.R10G10B10A2_UNORM,
            D3DFORMAT.A16B16G16R16 => DXGI_FORMAT.R16G16B16A16_UNORM,
            D3DFORMAT.A8P8 => DXGI_FORMAT.R8G8_UINT,
            D3DFORMAT.L8 => DXGI_FORMAT.R8_UNORM,
            D3DFORMAT.A8L8 => DXGI_FORMAT.R8G8_UNORM,
            D3DFORMAT.DXT1 => DXGI_FORMAT.BC1_UNORM,
            D3DFORMAT.DXT2 => DXGI_FORMAT.BC2_UNORM,
            D3DFORMAT.DXT3 => DXGI_FORMAT.BC2_UNORM,
            D3DFORMAT.DXT4 => DXGI_FORMAT.BC3_UNORM,
            D3DFORMAT.DXT5 => DXGI_FORMAT.BC3_UNORM,
            D3DFORMAT.D16_LOCKABLE => DXGI_FORMAT.D24_UNORM_S8_UINT,
            D3DFORMAT.D32 => DXGI_FORMAT.D32_FLOAT,
            D3DFORMAT.D24S8 => DXGI_FORMAT.D24_UNORM_S8_UINT,
            D3DFORMAT.D24X8 => DXGI_FORMAT.D24_UNORM_S8_UINT,
            D3DFORMAT.D16 => DXGI_FORMAT.D16_UNORM,
            D3DFORMAT.D32F_LOCKABLE => DXGI_FORMAT.D32_FLOAT,
            D3DFORMAT.D24FS8 => DXGI_FORMAT.D24_UNORM_S8_UINT,
            D3DFORMAT.D32_LOCKABLE => DXGI_FORMAT.D16_UNORM,
            D3DFORMAT.L16 => DXGI_FORMAT.R16_UNORM,
            D3DFORMAT.R16F => DXGI_FORMAT.R16_FLOAT,
            D3DFORMAT.G16R16F => DXGI_FORMAT.R16G16_FLOAT,
            D3DFORMAT.A16B16G16R16F => DXGI_FORMAT.R16G16B16A16_FLOAT,
            D3DFORMAT.R32F => DXGI_FORMAT.R32_FLOAT,
            D3DFORMAT.G32R32F => DXGI_FORMAT.R32G32_FLOAT,
            D3DFORMAT.A32B32G32R32F => DXGI_FORMAT.R32G32B32A32_FLOAT,
            D3DFORMAT.A2B10G10R10_XR_BIAS => DXGI_FORMAT.R10G10B10A2_UNORM,
            _ => DXGI_FORMAT.UNKNOWN
        };
    }

    public static D3DFORMAT GetFromDXGIFormatToD3DFORMAT(DXGI_FORMAT value)
    {
        return value switch
        {
            DXGI_FORMAT.R8G8B8A8_UNORM => D3DFORMAT.A8R8G8B8,
            DXGI_FORMAT.B8G8R8X8_UNORM => D3DFORMAT.X8R8G8B8,
            DXGI_FORMAT.B5G5R5A1_UNORM => D3DFORMAT.R5G6B5,
            DXGI_FORMAT.B4G4R4A4_UNORM => D3DFORMAT.A4R4G4B4,
            DXGI_FORMAT.A8_UNORM => D3DFORMAT.A8,
            DXGI_FORMAT.R10G10B10A2_UNORM => D3DFORMAT.A2B10G10R10,
            DXGI_FORMAT.G8R8_G8B8_UNORM => D3DFORMAT.X8B8G8R8,
            DXGI_FORMAT.R16G16_UNORM => D3DFORMAT.G16R16,
            DXGI_FORMAT.R16G16B16A16_UNORM => D3DFORMAT.A16B16G16R16,
            DXGI_FORMAT.R8G8_UINT => D3DFORMAT.A8P8,
            DXGI_FORMAT.R8_UNORM => D3DFORMAT.L8,
            DXGI_FORMAT.R8G8_UNORM => D3DFORMAT.A8L8,
            DXGI_FORMAT.BC1_UNORM => D3DFORMAT.DXT1,
            DXGI_FORMAT.BC2_UNORM => D3DFORMAT.DXT2,
            DXGI_FORMAT.BC3_UNORM => D3DFORMAT.DXT4, //check for alpha 
            DXGI_FORMAT.D24_UNORM_S8_UINT => D3DFORMAT.D24S8,
            DXGI_FORMAT.D32_FLOAT => D3DFORMAT.D32,
            DXGI_FORMAT.D16_UNORM => D3DFORMAT.D16,
            DXGI_FORMAT.R16_UNORM => D3DFORMAT.L16,
            DXGI_FORMAT.R16_FLOAT => D3DFORMAT.R16F,
            DXGI_FORMAT.R16G16_FLOAT => D3DFORMAT.G16R16F,
            DXGI_FORMAT.R16G16B16A16_FLOAT => D3DFORMAT.A16B16G16R16F,
            DXGI_FORMAT.R32_FLOAT => D3DFORMAT.R32F,
            DXGI_FORMAT.R32G32_FLOAT => D3DFORMAT.G32R32F,
            DXGI_FORMAT.R32G32B32A32_FLOAT => D3DFORMAT.A32B32G32R32F,
            //DXGI_FORMAT.R10G10B10A2_UNORM => D3DFORMAT.A2B10G10R10_XR_BIAS,
            _ => D3DFORMAT.UNKNOWN
        };
    }

    public static T3SurfaceFormat GetFromT3SurfaceFormatFromD3FORMAT(D3DFORMAT format)
    {
        return format switch
        {
            D3DFORMAT.UNKNOWN => T3SurfaceFormat.eSurface_Unknown,
            D3DFORMAT.A8R8G8B8 => T3SurfaceFormat.eSurface_ARGB8,
            D3DFORMAT.R5G6B5 => T3SurfaceFormat.eSurface_RGB565,
            D3DFORMAT.A1R5G5B5 => T3SurfaceFormat.eSurface_ARGB1555,
            D3DFORMAT.A4R4G4B4 => T3SurfaceFormat.eSurface_ARGB4,
            D3DFORMAT.A8 => T3SurfaceFormat.eSurface_A8,
            D3DFORMAT.A2B10G10R10 => T3SurfaceFormat.eSurface_ARGB2101010,
            D3DFORMAT.A8B8G8R8 => T3SurfaceFormat.eSurface_RGBA8,
            D3DFORMAT.G16R16 => T3SurfaceFormat.eSurface_RG16,
            D3DFORMAT.A2R10G10B10 => T3SurfaceFormat.eSurface_ARGB2101010,
            D3DFORMAT.A16B16G16R16 => T3SurfaceFormat.eSurface_RGBA16,
            D3DFORMAT.L8 => T3SurfaceFormat.eSurface_L8,
            D3DFORMAT.A8L8 => T3SurfaceFormat.eSurface_AL8,
            D3DFORMAT.DXT1 => T3SurfaceFormat.eSurface_DXT1,
            D3DFORMAT.DXT2 => T3SurfaceFormat.eSurface_DXT3,
            D3DFORMAT.DXT3 => T3SurfaceFormat.eSurface_DXT3,
            D3DFORMAT.DXT4 => T3SurfaceFormat.eSurface_DXT5,
            D3DFORMAT.DXT5 => T3SurfaceFormat.eSurface_DXT5,
            D3DFORMAT.D16_LOCKABLE => T3SurfaceFormat.eSurface_Depth16,
            D3DFORMAT.D32 => T3SurfaceFormat.eSurface_DepthStencil32,
            D3DFORMAT.D24S8 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
            D3DFORMAT.D24X8 => T3SurfaceFormat.eSurface_Depth24,
            D3DFORMAT.D24X4S4 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
            D3DFORMAT.D16 => T3SurfaceFormat.eSurface_Depth16,
            D3DFORMAT.D32F_LOCKABLE => T3SurfaceFormat.eSurface_Depth32F,
            D3DFORMAT.D24FS8 => T3SurfaceFormat.eSurface_Depth24F_Stencil8,
            D3DFORMAT.D32_LOCKABLE => T3SurfaceFormat.eSurface_Depth32F,
            D3DFORMAT.L16 => T3SurfaceFormat.eSurface_L16,
            D3DFORMAT.R16F => T3SurfaceFormat.eSurface_R16F,
            D3DFORMAT.G16R16F => T3SurfaceFormat.eSurface_RG16F,
            D3DFORMAT.A16B16G16R16F => T3SurfaceFormat.eSurface_RGBA16F,
            D3DFORMAT.R32F => T3SurfaceFormat.eSurface_R32F,
            D3DFORMAT.G32R32F => T3SurfaceFormat.eSurface_RG32F,
            D3DFORMAT.A32B32G32R32F => T3SurfaceFormat.eSurface_RGBA32F,
            D3DFORMAT.A2B10G10R10_XR_BIAS => T3SurfaceFormat.eSurface_RGBA1010102F,
            _ => T3SurfaceFormat.eSurface_Unknown
        };
    }

    public static D3DFORMAT GetD3D9Format(DDS_PIXELFORMAT ddpf)
    {
        if ((ddpf.dwFlags & (uint)DDPF.RGB) != 0)
        {
            switch (ddpf.dwRGBBitCount)
            {
                case 32:
                    if (ddpf.IsBitMask(0x00ff0000, 0x0000ff00, 0x000000ff, 0xff000000))
                    {
                        return D3DFORMAT.A8R8G8B8;
                    }
                    if (ddpf.IsBitMask(0x00ff0000, 0x0000ff00, 0x000000ff, 0))
                    {
                        return D3DFORMAT.X8R8G8B8;
                    }
                    if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000))
                    {
                        return D3DFORMAT.A8B8G8R8;
                    }
                    if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0))
                    {
                        return D3DFORMAT.X8B8G8R8;
                    }

                    // Note that many common DDS reader/writers (including D3DX) swap the
                    // the RED/BLUE masks for 10:10:10:2 formats. We assume
                    // below that the 'backwards' header mask is being used since it is most
                    // likely written by D3DX.

                    // For 'correct' writers this should be 0x3ff00000,0x000ffc00,0x000003ff for BGR data
                    if (ddpf.IsBitMask(0x000003ff, 0x000ffc00, 0x3ff00000, 0xc0000000))
                    {
                        return D3DFORMAT.A2R10G10B10;
                    }

                    // For 'correct' writers this should be 0x000003ff,0x000ffc00,0x3ff00000 for RGB data
                    if (ddpf.IsBitMask(0x3ff00000, 0x000ffc00, 0x000003ff, 0xc0000000))
                    {
                        return D3DFORMAT.A2B10G10R10;
                    }

                    if (ddpf.IsBitMask(0x0000ffff, 0xffff0000, 0x00000000, 0x00000000))
                    {
                        return D3DFORMAT.G16R16;
                    }
                    if (ddpf.IsBitMask(0xffffffff, 0x00000000, 0x00000000, 0x00000000))
                    {
                        return D3DFORMAT.R32F; // D3DX writes this out as a FourCC of 114
                    }
                    break;

                case 24:
                    if (ddpf.IsBitMask(0xff0000, 0x00ff00, 0x0000ff, 0))
                    {
                        return D3DFORMAT.R8G8B8;
                    }
                    break;

                case 16:
                    if (ddpf.IsBitMask(0xf800, 0x07e0, 0x001f, 0x0000))
                    {
                        return D3DFORMAT.R5G6B5;
                    }
                    if (ddpf.IsBitMask(0x7c00, 0x03e0, 0x001f, 0x8000))
                    {
                        return D3DFORMAT.A1R5G5B5;
                    }
                    if (ddpf.IsBitMask(0x7c00, 0x03e0, 0x001f, 0))
                    {
                        return D3DFORMAT.X1R5G5B5;
                    }
                    if (ddpf.IsBitMask(0x0f00, 0x00f0, 0x000f, 0xf000))
                    {
                        return D3DFORMAT.A4R4G4B4;
                    }
                    if (ddpf.IsBitMask(0x0f00, 0x00f0, 0x000f, 0))
                    {
                        return D3DFORMAT.X4R4G4B4;
                    }
                    if (ddpf.IsBitMask(0x00e0, 0x001c, 0x0003, 0xff00))
                    {
                        return D3DFORMAT.A8R3G3B2;
                    }

                    // NVTT versions 1.x wrote these as RGB instead of LUMINANCE
                    if (ddpf.IsBitMask(0xffff, 0, 0, 0))
                    {
                        return D3DFORMAT.L16;
                    }
                    if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00))
                    {
                        return D3DFORMAT.A8L8;
                    }
                    break;

                case 8:
                    if (ddpf.IsBitMask(0xe0, 0x1c, 0x03, 0))
                    {
                        return D3DFORMAT.R3G3B2;
                    }

                    // NVTT versions 1.x wrote these as RGB instead of LUMINANCE
                    if (ddpf.IsBitMask(0xff, 0, 0, 0))
                    {
                        return D3DFORMAT.L8;
                    }

                    // Paletted texture formats are typically not supported on modern video cards aka D3DFMT_P8, D3DFMT_A8P8
                    break;

                default:
                    return D3DFORMAT.UNKNOWN;
            }
        }
        else if ((ddpf.dwFlags & (uint)DDPF.LUMINANCE) != 0)
        {
            switch (ddpf.dwRGBBitCount)
            {
                case 16:
                    if (ddpf.IsBitMask(0xffff, 0, 0, 0) || ddpf.IsBitMask(0xffff, 0xffff, 0xffff, 0))
                    {
                        return D3DFORMAT.L16;
                    }
                    if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00))
                    {
                        return D3DFORMAT.A8L8;
                    }
                    break;

                case 8:
                    if (ddpf.IsBitMask(0x0f, 0, 0, 0xf0))
                    {
                        return D3DFORMAT.A4L4;
                    }
                    if (ddpf.IsBitMask(0xff, 0, 0, 0) || ddpf.IsBitMask(0xff, 0xff, 0xff, 0)) //GIMP for some reason writes this as RGB
                    {
                        return D3DFORMAT.L8;
                    }
                    if (ddpf.IsBitMask(0x00ff, 0, 0, 0xff00) || ddpf.IsBitMask(0x00ff, 0x00ff, 0x00ff, 0)) //GIMP for some reason writes this as RGBA
                    {
                        return D3DFORMAT.A8L8; //Some DDS writers assume the bitcount should be 8 instead of 16
                    }
                    break;

                default:
                    return D3DFORMAT.UNKNOWN;
            }
        }
        else if ((ddpf.dwFlags & (uint)DDPF.ALPHA) != 0)
        {
            if (8 == ddpf.dwRGBBitCount)
            {
                return D3DFORMAT.A8;
            }
            if (ddpf.IsBitMask(0xff, 0, 0, 0) || ddpf.IsBitMask(0xff, 0xff, 0xff, 0)) //GIMP for some reason writes this as RGB
            {
                return D3DFORMAT.L8;
            }
        }
        else if ((ddpf.dwFlags & (uint)DDPF.YUV) != 0)
        {
            switch (ddpf.dwRGBBitCount)
            {
                case 32:
                    if (ddpf.IsBitMask(0x000000ff, 0x0000ff00, 0x00ff0000, 0xff000000))
                    {
                        return D3DFORMAT.Q8W8V8U8;
                    }
                    if (ddpf.IsBitMask(0x0000ffff, 0xffff0000, 0x00000000, 0x00000000))
                    {
                        return D3DFORMAT.V16U16;
                    }
                    if (ddpf.IsBitMask(0x3ff00000, 0x000ffc00, 0x000003ff, 0xc0000000))
                    {
                        return D3DFORMAT.A2W10V10U10;
                    }
                    break;

                case 16:
                    if (ddpf.IsBitMask(0x00ff, 0xff00, 0, 0))
                    {
                        return D3DFORMAT.V8U8;
                    }
                    break;

                default:
                    return D3DFORMAT.UNKNOWN;
            }
        }

        else if ((ddpf.dwFlags & (uint)DDPF.FOURCC) != 0)
        {
            if (ByteFunctions.Convert_String_To_UInt32("DXT1") == ddpf.dwFourCC)
            {
                return D3DFORMAT.DXT1;
            }
            if (ByteFunctions.Convert_String_To_UInt32("DXT2") == ddpf.dwFourCC)
            {
                return D3DFORMAT.DXT2;
            }
            if (ByteFunctions.Convert_String_To_UInt32("DXT3") == ddpf.dwFourCC)
            {
                return D3DFORMAT.DXT3;
            }
            if (ByteFunctions.Convert_String_To_UInt32("DXT4") == ddpf.dwFourCC)
            {
                return D3DFORMAT.DXT4;
            }
            if (ByteFunctions.Convert_String_To_UInt32("DXT5") == ddpf.dwFourCC)
            {
                return D3DFORMAT.DXT5;
            }

            if (ByteFunctions.Convert_String_To_UInt32("RGBG") == ddpf.dwFourCC)
            {
                return D3DFORMAT.R8G8_B8G8;
            }
            if (ByteFunctions.Convert_String_To_UInt32("GRGB") == ddpf.dwFourCC)
            {
                return D3DFORMAT.G8R8_G8B8;
            }
            if (ByteFunctions.Convert_String_To_UInt32("UYVY") == ddpf.dwFourCC)
            {
                return D3DFORMAT.UYVY;
            }
            if (ByteFunctions.Convert_String_To_UInt32("YUY2") == ddpf.dwFourCC)
            {
                return D3DFORMAT.YUY2;
            }

            // Check for D3DFORMAT enums being set here
            return ddpf.dwFourCC switch
            {
                (uint)D3DFORMAT.A16B16G16R16 or 
                (uint)D3DFORMAT.Q16W16V16U16 or 
                (uint)D3DFORMAT.R16F or 
                (uint)D3DFORMAT.G16R16F or 
                (uint)D3DFORMAT.A16B16G16R16F or 
                (uint)D3DFORMAT.R32F or 
                (uint)D3DFORMAT.G32R32F or 
                (uint)D3DFORMAT.A32B32G32R32F or 
                (uint)D3DFORMAT.CxV8U8 => (D3DFORMAT)ddpf.dwFourCC,
                _ => D3DFORMAT.UNKNOWN,
            };
        }

        return D3DFORMAT.UNKNOWN;
    }
}
