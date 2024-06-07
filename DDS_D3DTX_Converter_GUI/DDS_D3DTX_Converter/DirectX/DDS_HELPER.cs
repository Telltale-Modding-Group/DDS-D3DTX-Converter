using D3DTX_Converter.TelltaleEnums;
using DirectXTexNet;
using System;
using System.Runtime.InteropServices;

namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide
// D3DFORMAT - https://learn.microsoft.com/en-us/windows/win32/direct3d9/d3dformat
// Map Direct3D 9 Formats to Direct3D 10 - https://learn.microsoft.com/en-gb/windows/win32/direct3d10/d3d10-graphics-programming-guide-resources-legacy-formats?redirectedfrom=MSDN

/// <summary>
/// The class is used for decoding and encoding .dds headers. 
/// </summary>
public static partial class DDS_HELPER
{
    /// <summary>
    /// Returns an array of bytes from an object.
    /// </summary>
    /// <param name="o">The object.</param>
    /// <returns>An array of bytes composed from the object's raw data.</returns>
    public static byte[] GetObjectBytes(object o)
    {
        int size = Marshal.SizeOf(o);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(o, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    /// <summary>
    /// Get the Telltale surface format from a DXGI format.
    /// This is used for the conversion process from .dds to .d3dtx.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <returns>The corresponding T3SurfaceFormat enum from the DXGI format.</returns>
    private static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(DXGI_FORMAT dxgiFormat)
    {
        return dxgiFormat switch
        {
            DXGI_FORMAT.B8G8R8A8_UNORM_SRGB => T3SurfaceFormat.eSurface_ARGB8,
            DXGI_FORMAT.B8G8R8A8_UNORM => T3SurfaceFormat.eSurface_ARGB8,
            DXGI_FORMAT.R16G16B16A16_UNORM => T3SurfaceFormat.eSurface_ARGB16,
            DXGI_FORMAT.B5G6R5_UNORM => T3SurfaceFormat.eSurface_RGB565,
            DXGI_FORMAT.B5G5R5A1_UNORM => T3SurfaceFormat.eSurface_ARGB1555,
            DXGI_FORMAT.B4G4R4A4_UNORM => T3SurfaceFormat.eSurface_ARGB4,
            DXGI_FORMAT.R10G10B10A2_UNORM => T3SurfaceFormat.eSurface_ARGB2101010,
            DXGI_FORMAT.R16G16_UNORM => T3SurfaceFormat.eSurface_RG16,
            DXGI_FORMAT.R8G8B8A8_UNORM_SRGB => T3SurfaceFormat.eSurface_RGBA8,
            DXGI_FORMAT.R8G8B8A8_UNORM => T3SurfaceFormat.eSurface_RGBA8,
            DXGI_FORMAT.R32_UINT => T3SurfaceFormat.eSurface_R32,
            DXGI_FORMAT.R32G32_UINT => T3SurfaceFormat.eSurface_RG32,
            DXGI_FORMAT.R32G32B32A32_FLOAT => T3SurfaceFormat.eSurface_RGBA32F,
            DXGI_FORMAT.R8G8B8A8_SNORM => T3SurfaceFormat.eSurface_RGBA8S,
            DXGI_FORMAT.A8_UNORM => T3SurfaceFormat.eSurface_A8,
            DXGI_FORMAT.R8_UNORM => T3SurfaceFormat.eSurface_L8,
            DXGI_FORMAT.R8G8_UNORM => T3SurfaceFormat.eSurface_AL8,
            DXGI_FORMAT.R16_UNORM => T3SurfaceFormat.eSurface_L16,
            DXGI_FORMAT.R16G16_SNORM => T3SurfaceFormat.eSurface_RG16S,
            DXGI_FORMAT.R16G16B16A16_SNORM => T3SurfaceFormat.eSurface_RGBA16S,
            DXGI_FORMAT.R16G16B16A16_UINT => T3SurfaceFormat.eSurface_R16UI,
            DXGI_FORMAT.R16_FLOAT => T3SurfaceFormat.eSurface_R16F,
            DXGI_FORMAT.R16G16B16A16_FLOAT => T3SurfaceFormat.eSurface_RGBA16F,
            DXGI_FORMAT.R32_FLOAT => T3SurfaceFormat.eSurface_R32F,
            DXGI_FORMAT.R32G32_FLOAT => T3SurfaceFormat.eSurface_RG32F,
            DXGI_FORMAT.R32G32B32A32_UINT => T3SurfaceFormat.eSurface_RGBA32,
            DXGI_FORMAT.R11G11B10_FLOAT => T3SurfaceFormat.eSurface_RGB111110F,
            DXGI_FORMAT.R9G9B9E5_SHAREDEXP => T3SurfaceFormat.eSurface_RGB9E5F,
            DXGI_FORMAT.D16_UNORM => T3SurfaceFormat.eSurface_DepthPCF16,
            DXGI_FORMAT.D24_UNORM_S8_UINT => T3SurfaceFormat.eSurface_DepthPCF24,
            DXGI_FORMAT.D32_FLOAT_S8X24_UINT => T3SurfaceFormat.eSurface_DepthStencil32,
            DXGI_FORMAT.D32_FLOAT => T3SurfaceFormat.eSurface_Depth32F,
            DXGI_FORMAT.BC1_UNORM => T3SurfaceFormat.eSurface_BC1,
            DXGI_FORMAT.BC2_UNORM => T3SurfaceFormat.eSurface_BC2,
            DXGI_FORMAT.BC3_UNORM => T3SurfaceFormat.eSurface_BC3,
            DXGI_FORMAT.BC4_UNORM => T3SurfaceFormat.eSurface_BC4,
            DXGI_FORMAT.BC5_UNORM => T3SurfaceFormat.eSurface_BC5,
            DXGI_FORMAT.BC6H_UF16 => T3SurfaceFormat.eSurface_BC6,
            DXGI_FORMAT.BC7_UNORM => T3SurfaceFormat.eSurface_BC7,
            DXGI_FORMAT.BC1_UNORM_SRGB => T3SurfaceFormat.eSurface_BC1,
            DXGI_FORMAT.BC2_UNORM_SRGB => T3SurfaceFormat.eSurface_BC2,
            DXGI_FORMAT.BC3_UNORM_SRGB => T3SurfaceFormat.eSurface_BC3,
            DXGI_FORMAT.BC7_UNORM_SRGB => T3SurfaceFormat.eSurface_BC7,
            _ => T3SurfaceFormat.eSurface_Unknown,
        };
    }

    /// <summary>
    /// Get the Telltale surface format from a DXGI format and an already existing surface format.
    /// This is used for the conversion process from .dds to .d3dtx.
    /// This is used when equivalent formats are found. 
    /// Some Telltale games have different values for the same formats, but they do not ship with all of them.
    /// This can create issues if the Telltale surface format is not found in the game. 
    /// In any case, use Lucas's Telltale Inspector to change the value if any issues arise.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <param name="surfaceFormat">(Optional) The existing Telltale surface format. Default value is UNKNOWN.</param>
    /// <returns>The corresponding T3SurfaceFormat enum from the DXGI format and Telltale surface format.</returns>
    public static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(DXGI_FORMAT dxgiFormat, T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_Unknown)
    {
        T3SurfaceFormat surfaceFormatFromDXGI = GetTelltaleSurfaceFormatFromDXGI(dxgiFormat);

        if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_Unknown)
        {
            return surfaceFormat;
        }

        if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_L16 && surfaceFormat == T3SurfaceFormat.eSurface_R16)
        {
            return T3SurfaceFormat.eSurface_L16;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_AL8 && surfaceFormat == T3SurfaceFormat.eSurface_RG8)
        {
            return T3SurfaceFormat.eSurface_RG8;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_L8 && surfaceFormat == T3SurfaceFormat.eSurface_R8)
        {
            return T3SurfaceFormat.eSurface_R8;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_ARGB16 && surfaceFormat == T3SurfaceFormat.eSurface_RGBA16)
        {
            return T3SurfaceFormat.eSurface_RGBA16;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_ARGB2101010 && surfaceFormat == T3SurfaceFormat.eSurface_RGBA1010102F)
        {
            return T3SurfaceFormat.eSurface_RGBA1010102F;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_R32F && surfaceFormat == T3SurfaceFormat.eSurface_R32)
        {
            return T3SurfaceFormat.eSurface_R32;
        }
        else if (surfaceFormatFromDXGI == T3SurfaceFormat.eSurface_RG32F && surfaceFormat == T3SurfaceFormat.eSurface_RG32)
        {
            return T3SurfaceFormat.eSurface_R32;
        }

        return surfaceFormatFromDXGI;
    }

    /// <summary>
    /// Returns the corresponding Direct3D10/DXGI format from a Telltale surface format. 
    /// This is used for the conversion process from .d3dtx to .dds.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="gamma"></param>
    /// <returns>The corresponding DXGI_Format.</returns>
    public static DXGI_FORMAT GetDXGIFromTelltaleSurfaceFormat(T3SurfaceFormat format, T3SurfaceGamma gamma = T3SurfaceGamma.eSurfaceGamma_Linear)
    {
        switch (format)
        {
            default:
                return DXGI_FORMAT.R8G8B8A8_UNORM; // Choose R8G8B8A8 if the format is not specified. (Raw data)

            // In order of T3SurfaceFormat enum
            //--------------------ARGB8--------------------
            case T3SurfaceFormat.eSurface_ARGB8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.B8G8R8A8_UNORM_SRGB;
                else
                    return DXGI_FORMAT.B8G8R8A8_UNORM;
            //--------------------ARGB16--------------------
            case T3SurfaceFormat.eSurface_ARGB16:
                return DXGI_FORMAT.R16G16B16A16_UNORM;

            //--------------------RGB565--------------------
            case T3SurfaceFormat.eSurface_RGB565:
                return DXGI_FORMAT.B5G6R5_UNORM;

            //--------------------ARGB1555--------------------
            case T3SurfaceFormat.eSurface_ARGB1555:
                return DXGI_FORMAT.B5G5R5A1_UNORM;

            //--------------------ARGB4--------------------
            case T3SurfaceFormat.eSurface_ARGB4:
                return DXGI_FORMAT.B4G4R4A4_UNORM;

            //--------------------ARGB2101010--------------------
            case T3SurfaceFormat.eSurface_ARGB2101010:
                return DXGI_FORMAT.R10G10B10A2_UNORM;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_R16:
                return DXGI_FORMAT.R16_UNORM;

            //--------------------RG16--------------------
            case T3SurfaceFormat.eSurface_RG16:
                return DXGI_FORMAT.R16G16_UNORM;

            //--------------------RGBA16--------------------
            case T3SurfaceFormat.eSurface_RGBA16:
                return DXGI_FORMAT.R16G16B16A16_UNORM;

            //--------------------RG8--------------------
            case T3SurfaceFormat.eSurface_RG8:
                return DXGI_FORMAT.R8G8_UNORM;

            //--------------------RGBA8--------------------
            case T3SurfaceFormat.eSurface_RGBA8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.R8G8B8A8_UNORM_SRGB;
                else
                    return DXGI_FORMAT.R8G8B8A8_UNORM;

            //--------------------R32--------------------
            case T3SurfaceFormat.eSurface_R32:
                return DXGI_FORMAT.R32_FLOAT;

            //--------------------RG32--------------------
            case T3SurfaceFormat.eSurface_RG32:
                return DXGI_FORMAT.R32G32_FLOAT;

            //--------------------RGBA32--------------------
            case T3SurfaceFormat.eSurface_RGBA32:
                return DXGI_FORMAT.R32G32B32A32_FLOAT; // It could be UINT

            //--------------------R8--------------------
            case T3SurfaceFormat.eSurface_R8:
                return DXGI_FORMAT.R8_UNORM;

            //--------------------RGBA8S--------------------
            case T3SurfaceFormat.eSurface_RGBA8S:
                return DXGI_FORMAT.R8G8B8A8_SNORM;

            //--------------------A8--------------------
            case T3SurfaceFormat.eSurface_A8:
                return DXGI_FORMAT.A8_UNORM;

            //--------------------L8--------------------
            case T3SurfaceFormat.eSurface_L8:
                return DXGI_FORMAT.R8_UNORM;

            //--------------------AL8--------------------
            case T3SurfaceFormat.eSurface_AL8:
                return DXGI_FORMAT.R8G8_UNORM;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_L16:
                return DXGI_FORMAT.R16_UNORM;

            //--------------------RG16S--------------------
            case T3SurfaceFormat.eSurface_RG16S:
                return DXGI_FORMAT.R16G16_SNORM;

            //--------------------RGBA16S--------------------
            case T3SurfaceFormat.eSurface_RGBA16S:
                return DXGI_FORMAT.R16G16B16A16_SNORM;

            //--------------------RGBA16UI--------------------
            case T3SurfaceFormat.eSurface_R16UI:
                return DXGI_FORMAT.R16G16B16A16_UINT;

            //--------------------RG16F--------------------
            case T3SurfaceFormat.eSurface_R16F:
                return DXGI_FORMAT.R16_FLOAT;

            //--------------------RGBA16F--------------------
            case T3SurfaceFormat.eSurface_RGBA16F:
                return DXGI_FORMAT.R16G16B16A16_FLOAT;

            //--------------------R32F--------------------
            case T3SurfaceFormat.eSurface_R32F:
                return DXGI_FORMAT.R32_FLOAT;

            //--------------------RG32F--------------------
            case T3SurfaceFormat.eSurface_RG32F:
                return DXGI_FORMAT.R32G32_FLOAT; // It could be UINT

            //--------------------RGBA32F--------------------
            case T3SurfaceFormat.eSurface_RGBA32F:
                return DXGI_FORMAT.R32G32B32A32_FLOAT;

            //--------------------RGBA1010102F--------------------
            case T3SurfaceFormat.eSurface_RGBA1010102F:
                return DXGI_FORMAT.R10G10B10A2_UNORM;

            //--------------------RGB111110F--------------------
            case T3SurfaceFormat.eSurface_RGB111110F:
                return DXGI_FORMAT.R11G11B10_FLOAT;

            //--------------------RGB9E5F--------------------
            case T3SurfaceFormat.eSurface_RGB9E5F:
                return DXGI_FORMAT.R9G9B9E5_SHAREDEXP;

            //--------------------DepthPCF16--------------------
            case T3SurfaceFormat.eSurface_DepthPCF16:
                return DXGI_FORMAT.D16_UNORM;

            //--------------------DepthPCF24--------------------
            case T3SurfaceFormat.eSurface_DepthPCF24:
                return DXGI_FORMAT.D24_UNORM_S8_UINT;

            //--------------------Depth16--------------------
            case T3SurfaceFormat.eSurface_Depth16:
                return DXGI_FORMAT.D16_UNORM;

            //--------------------Depth24--------------------
            case T3SurfaceFormat.eSurface_Depth24:
                return DXGI_FORMAT.D24_UNORM_S8_UINT;

            //--------------------DepthStencil32--------------------
            case T3SurfaceFormat.eSurface_DepthStencil32:
                return DXGI_FORMAT.D32_FLOAT_S8X24_UINT;

            //--------------------Depth32F--------------------
            case T3SurfaceFormat.eSurface_Depth32F:
                return DXGI_FORMAT.D32_FLOAT;

            //--------------------Depth32F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
                return DXGI_FORMAT.D32_FLOAT_S8X24_UINT;

            //--------------------Depth24F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
                return DXGI_FORMAT.D24_UNORM_S8_UINT;

            //--------------------DXT1 / BC1--------------------
            case T3SurfaceFormat.eSurface_BC1:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.BC1_UNORM_SRGB;
                else
                    return DXGI_FORMAT.BC1_UNORM;

            //--------------------DXT2 and DXT3 / BC2--------------------
            case T3SurfaceFormat.eSurface_BC2:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.BC2_UNORM_SRGB;
                else
                    return DXGI_FORMAT.BC2_UNORM;

            //--------------------DXT4 and DXT5 / BC3--------------------
            case T3SurfaceFormat.eSurface_BC3:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.BC3_UNORM_SRGB;
                else
                    return DXGI_FORMAT.BC3_UNORM;

            //--------------------ATI1 / BC4--------------------
            case T3SurfaceFormat.eSurface_BC4:
                return DXGI_FORMAT.BC4_UNORM;

            //--------------------ATI2 / BC5--------------------
            case T3SurfaceFormat.eSurface_BC5:
                return DXGI_FORMAT.BC5_UNORM;

            //--------------------BC6H--------------------
            case T3SurfaceFormat.eSurface_BC6:
                return DXGI_FORMAT.BC6H_UF16;

            //--------------------BC7--------------------
            case T3SurfaceFormat.eSurface_BC7:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGI_FORMAT.BC7_UNORM_SRGB;
                else
                    return DXGI_FORMAT.BC7_UNORM;

            //--------------------UNKNOWN--------------------
            case T3SurfaceFormat.eSurface_Unknown:
                return DXGI_FORMAT.UNKNOWN;
        }
    }

    /// <summary>
    /// Returns the corresponding Direct3D9 surface format from a Direct3D10/DXGI format.
    /// This is used for the conversion process from .d3dtx to .dds. (Legacy .d3dtx)
    /// </summary>
    /// <param name="dxgiFormat">The DXGI format.</param>
    /// <param name="metadata">The metadata of our .dds file. It is used in determining if the alpha is premultiplied.</param>
    /// <returns>The corresponding Direct3D9 format.</returns>
    public static D3DFORMAT GetD3DFORMATFromDXGIFormat(DXGI_FORMAT dxgiFormat, TexMetadata metadata)
    {
        return dxgiFormat switch
        {
            DXGI_FORMAT.B8G8R8A8_UNORM => D3DFORMAT.A8R8G8B8,
            DXGI_FORMAT.B8G8R8X8_UNORM => D3DFORMAT.X8R8G8B8,
            DXGI_FORMAT.B5G6R5_UNORM => D3DFORMAT.R5G6B5,
            DXGI_FORMAT.B5G5R5A1_UNORM => D3DFORMAT.A1R5G5B5,
            DXGI_FORMAT.B4G4R4A4_UNORM => D3DFORMAT.A4R4G4B4,
            DXGI_FORMAT.A8_UNORM => D3DFORMAT.A8,
            DXGI_FORMAT.R10G10B10A2_UNORM => D3DFORMAT.A2B10G10R10,
            DXGI_FORMAT.R8G8B8A8_UNORM => D3DFORMAT.A8B8G8R8,
            DXGI_FORMAT.R8G8B8A8_UNORM_SRGB => D3DFORMAT.A8B8G8R8,
            DXGI_FORMAT.R16G16_UNORM => D3DFORMAT.G16R16,
            DXGI_FORMAT.R16G16B16A16_UNORM => D3DFORMAT.A16B16G16R16,
            DXGI_FORMAT.R8_UNORM => D3DFORMAT.L8,
            DXGI_FORMAT.R8G8_UNORM => D3DFORMAT.A8L8,
            DXGI_FORMAT.R8G8_SNORM => D3DFORMAT.V8U8,
            DXGI_FORMAT.R8G8B8A8_SNORM => D3DFORMAT.Q8W8V8U8,
            DXGI_FORMAT.R16G16_SNORM => D3DFORMAT.V16U16,
            DXGI_FORMAT.G8R8_G8B8_UNORM => D3DFORMAT.R8G8_B8G8,
            DXGI_FORMAT.YUY2 => D3DFORMAT.YUY2,
            DXGI_FORMAT.R8G8_B8G8_UNORM => D3DFORMAT.G8R8_G8B8,
            DXGI_FORMAT.BC1_UNORM => D3DFORMAT.DXT1,
            DXGI_FORMAT.BC2_UNORM => metadata.IsPMAlpha() ? D3DFORMAT.DXT2 : D3DFORMAT.DXT3,
            DXGI_FORMAT.BC3_UNORM => metadata.IsPMAlpha() ? D3DFORMAT.DXT4 : D3DFORMAT.DXT5,
            DXGI_FORMAT.BC4_UNORM => D3DFORMAT.ATI1,
            DXGI_FORMAT.BC4_SNORM => D3DFORMAT.BC4S,
            DXGI_FORMAT.BC5_UNORM => D3DFORMAT.ATI2,
            DXGI_FORMAT.BC5_SNORM => D3DFORMAT.BC5S,
            DXGI_FORMAT.D16_UNORM => D3DFORMAT.D16,
            DXGI_FORMAT.D32_FLOAT => D3DFORMAT.D32F_LOCKABLE,
            DXGI_FORMAT.D24_UNORM_S8_UINT => D3DFORMAT.D24S8,
            DXGI_FORMAT.R16_UNORM => D3DFORMAT.L16,
            DXGI_FORMAT.R16G16B16A16_SNORM => D3DFORMAT.Q16W16V16U16,
            DXGI_FORMAT.R16_FLOAT => D3DFORMAT.R16F,
            DXGI_FORMAT.R16G16_FLOAT => D3DFORMAT.G16R16F,
            DXGI_FORMAT.R16G16B16A16_FLOAT => D3DFORMAT.A16B16G16R16F,
            DXGI_FORMAT.R32_FLOAT => D3DFORMAT.R32F,
            DXGI_FORMAT.R32G32_FLOAT => D3DFORMAT.G32R32F,
            DXGI_FORMAT.R32G32B32A32_FLOAT => D3DFORMAT.A32B32G32R32F,

            _ => D3DFORMAT.UNKNOWN
        };
    }
}
