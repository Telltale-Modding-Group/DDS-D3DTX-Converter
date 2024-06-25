using D3DTX_Converter.DirectX.Enums;
using D3DTX_Converter.TelltaleEnums;
using Hexa.NET.DirectXTex;
using Pfim;
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
    private static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(DXGIFormat dxgiFormat)
    {
        return dxgiFormat switch
        {
            DXGIFormat.B8G8R8A8_UNORM_SRGB => T3SurfaceFormat.eSurface_ARGB8,
            DXGIFormat.B8G8R8A8_UNORM => T3SurfaceFormat.eSurface_ARGB8,
            DXGIFormat.R16G16B16A16_UNORM => T3SurfaceFormat.eSurface_ARGB16,
            DXGIFormat.B5G6R5_UNORM => T3SurfaceFormat.eSurface_RGB565,
            DXGIFormat.B5G5R5A1_UNORM => T3SurfaceFormat.eSurface_ARGB1555,
            DXGIFormat.B4G4R4A4_UNORM => T3SurfaceFormat.eSurface_ARGB4,
            DXGIFormat.A4B4G4R4_UNORM => T3SurfaceFormat.eSurface_ARGB4,
            DXGIFormat.R10G10B10A2_UNORM => T3SurfaceFormat.eSurface_ARGB2101010,
            DXGIFormat.R16G16_UNORM => T3SurfaceFormat.eSurface_RG16,
            DXGIFormat.R8G8B8A8_UNORM_SRGB => T3SurfaceFormat.eSurface_RGBA8,
            DXGIFormat.R8G8B8A8_UNORM => T3SurfaceFormat.eSurface_RGBA8,
            DXGIFormat.R32_UINT => T3SurfaceFormat.eSurface_R32,
            DXGIFormat.R32G32_UINT => T3SurfaceFormat.eSurface_RG32,
            DXGIFormat.R32G32B32A32_FLOAT => T3SurfaceFormat.eSurface_RGBA32F,
            DXGIFormat.R8G8B8A8_SNORM => T3SurfaceFormat.eSurface_RGBA8S,
            DXGIFormat.A8_UNORM => T3SurfaceFormat.eSurface_A8,
            DXGIFormat.R8_UNORM => T3SurfaceFormat.eSurface_L8,
            DXGIFormat.R8G8_UNORM => T3SurfaceFormat.eSurface_AL8,
            DXGIFormat.R16_UNORM => T3SurfaceFormat.eSurface_L16,
            DXGIFormat.R16G16_SNORM => T3SurfaceFormat.eSurface_RG16S,
            DXGIFormat.R16G16B16A16_SNORM => T3SurfaceFormat.eSurface_RGBA16S,
            DXGIFormat.R16G16B16A16_UINT => T3SurfaceFormat.eSurface_R16UI,
            DXGIFormat.R16_FLOAT => T3SurfaceFormat.eSurface_R16F,
            DXGIFormat.R16G16B16A16_FLOAT => T3SurfaceFormat.eSurface_RGBA16F,
            DXGIFormat.R32_FLOAT => T3SurfaceFormat.eSurface_R32F,
            DXGIFormat.R32G32_FLOAT => T3SurfaceFormat.eSurface_RG32F,
            DXGIFormat.R32G32B32A32_UINT => T3SurfaceFormat.eSurface_RGBA32,
            DXGIFormat.R11G11B10_FLOAT => T3SurfaceFormat.eSurface_RGB111110F,
            DXGIFormat.R9G9B9E5_SHAREDEXP => T3SurfaceFormat.eSurface_RGB9E5F,
            DXGIFormat.D16_UNORM => T3SurfaceFormat.eSurface_DepthPCF16,
            DXGIFormat.D24_UNORM_S8_UINT => T3SurfaceFormat.eSurface_DepthPCF24,
            DXGIFormat.D32_FLOAT_S8X24_UINT => T3SurfaceFormat.eSurface_DepthStencil32,
            DXGIFormat.D32_FLOAT => T3SurfaceFormat.eSurface_Depth32F,
            DXGIFormat.BC1_UNORM => T3SurfaceFormat.eSurface_BC1,
            DXGIFormat.BC2_UNORM => T3SurfaceFormat.eSurface_BC2,
            DXGIFormat.BC3_UNORM => T3SurfaceFormat.eSurface_BC3,
            DXGIFormat.BC4_UNORM => T3SurfaceFormat.eSurface_BC4,
            DXGIFormat.BC5_UNORM => T3SurfaceFormat.eSurface_BC5,
            DXGIFormat.BC6H_UF16 => T3SurfaceFormat.eSurface_BC6,
            DXGIFormat.BC7_UNORM => T3SurfaceFormat.eSurface_BC7,
            DXGIFormat.BC1_UNORM_SRGB => T3SurfaceFormat.eSurface_BC1,
            DXGIFormat.BC2_UNORM_SRGB => T3SurfaceFormat.eSurface_BC2,
            DXGIFormat.BC3_UNORM_SRGB => T3SurfaceFormat.eSurface_BC3,
            DXGIFormat.BC7_UNORM_SRGB => T3SurfaceFormat.eSurface_BC7,
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
    public static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(DXGIFormat dxgiFormat, T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_Unknown)
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
    public static DXGIFormat GetDXGIFromTelltaleSurfaceFormat(T3SurfaceFormat format, T3SurfaceGamma gamma = T3SurfaceGamma.eSurfaceGamma_Linear)
    {
        switch (format)
        {
            default:
                return DXGIFormat.R8G8B8A8_UNORM; // Choose R8G8B8A8 if the format is not specified. (Raw data)

            // In order of T3SurfaceFormat enum
            //--------------------ARGB8--------------------
            case T3SurfaceFormat.eSurface_ARGB8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.B8G8R8A8_UNORM_SRGB;
                else
                    return DXGIFormat.B8G8R8A8_UNORM;
            //--------------------ARGB16--------------------
            case T3SurfaceFormat.eSurface_ARGB16:
                return DXGIFormat.R16G16B16A16_UNORM;

            //--------------------RGB565--------------------
            case T3SurfaceFormat.eSurface_RGB565:
                return DXGIFormat.B5G6R5_UNORM;

            //--------------------ARGB1555--------------------
            case T3SurfaceFormat.eSurface_ARGB1555:
                return DXGIFormat.B5G5R5A1_UNORM;

            //--------------------ARGB4--------------------
            //ACTUALLY IT'S RGBA4 - IT NEEDS TO BE UPDATED
            case T3SurfaceFormat.eSurface_ARGB4:
                return DXGIFormat.A4B4G4R4_UNORM;

            //--------------------ARGB2101010--------------------
            case T3SurfaceFormat.eSurface_ARGB2101010:
                return DXGIFormat.R10G10B10A2_UNORM;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_R16:
                return DXGIFormat.R16_UNORM;

            //--------------------RG16--------------------
            case T3SurfaceFormat.eSurface_RG16:
                return DXGIFormat.R16G16_UNORM;

            //--------------------RGBA16--------------------
            case T3SurfaceFormat.eSurface_RGBA16:
                return DXGIFormat.R16G16B16A16_UNORM;

            //--------------------RG8--------------------
            case T3SurfaceFormat.eSurface_RG8:
                return DXGIFormat.R8G8_UNORM;

            //--------------------RGBA8--------------------
            case T3SurfaceFormat.eSurface_RGBA8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.R8G8B8A8_UNORM_SRGB;
                else
                    return DXGIFormat.R8G8B8A8_UNORM;

            //--------------------R32--------------------
            case T3SurfaceFormat.eSurface_R32:
                return DXGIFormat.R32_FLOAT;

            //--------------------RG32--------------------
            case T3SurfaceFormat.eSurface_RG32:
                return DXGIFormat.R32G32_FLOAT;

            //--------------------RGBA32--------------------
            case T3SurfaceFormat.eSurface_RGBA32:
                return DXGIFormat.R32G32B32A32_FLOAT; // It could be UINT

            //--------------------R8--------------------
            case T3SurfaceFormat.eSurface_R8:
                return DXGIFormat.R8_UNORM;

            //--------------------RGBA8S--------------------
            case T3SurfaceFormat.eSurface_RGBA8S:
                return DXGIFormat.R8G8B8A8_SNORM;

            //--------------------A8--------------------
            case T3SurfaceFormat.eSurface_A8:
                return DXGIFormat.A8_UNORM;

            //--------------------L8--------------------
            case T3SurfaceFormat.eSurface_L8:
                return DXGIFormat.R8_UNORM;

            //--------------------AL8--------------------
            case T3SurfaceFormat.eSurface_AL8:
                return DXGIFormat.R8G8_UNORM;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_L16:
                return DXGIFormat.R16_UNORM;

            //--------------------RG16S--------------------
            case T3SurfaceFormat.eSurface_RG16S:
                return DXGIFormat.R16G16_SNORM;

            //--------------------RGBA16S--------------------
            case T3SurfaceFormat.eSurface_RGBA16S:
                return DXGIFormat.R16G16B16A16_SNORM;

            //--------------------RGBA16UI--------------------
            case T3SurfaceFormat.eSurface_R16UI:
                return DXGIFormat.R16G16B16A16_UINT;

            //--------------------RG16F--------------------
            case T3SurfaceFormat.eSurface_R16F:
                return DXGIFormat.R16_FLOAT;

            //--------------------RGBA16F--------------------
            case T3SurfaceFormat.eSurface_RGBA16F:
                return DXGIFormat.R16G16B16A16_FLOAT;

            //--------------------R32F--------------------
            case T3SurfaceFormat.eSurface_R32F:
                return DXGIFormat.R32_FLOAT;

            //--------------------RG32F--------------------
            case T3SurfaceFormat.eSurface_RG32F:
                return DXGIFormat.R32G32_FLOAT; // It could be UINT

            //--------------------RGBA32F--------------------
            case T3SurfaceFormat.eSurface_RGBA32F:
                return DXGIFormat.R32G32B32A32_FLOAT;

            //--------------------RGBA1010102F--------------------
            case T3SurfaceFormat.eSurface_RGBA1010102F:
                return DXGIFormat.R10G10B10A2_UNORM;

            //--------------------RGB111110F--------------------
            case T3SurfaceFormat.eSurface_RGB111110F:
                return DXGIFormat.R11G11B10_FLOAT;

            //--------------------RGB9E5F--------------------
            case T3SurfaceFormat.eSurface_RGB9E5F:
                return DXGIFormat.R9G9B9E5_SHAREDEXP;

            //--------------------DepthPCF16--------------------
            case T3SurfaceFormat.eSurface_DepthPCF16:
                return DXGIFormat.D16_UNORM;

            //--------------------DepthPCF24--------------------
            case T3SurfaceFormat.eSurface_DepthPCF24:
                return DXGIFormat.D24_UNORM_S8_UINT;

            //--------------------Depth16--------------------
            case T3SurfaceFormat.eSurface_Depth16:
                return DXGIFormat.D16_UNORM;

            //--------------------Depth24--------------------
            case T3SurfaceFormat.eSurface_Depth24:
                return DXGIFormat.D24_UNORM_S8_UINT;

            //--------------------DepthStencil32--------------------
            case T3SurfaceFormat.eSurface_DepthStencil32:
                return DXGIFormat.D32_FLOAT_S8X24_UINT;

            //--------------------Depth32F--------------------
            case T3SurfaceFormat.eSurface_Depth32F:
                return DXGIFormat.D32_FLOAT;

            //--------------------Depth32F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
                return DXGIFormat.D32_FLOAT_S8X24_UINT;

            //--------------------Depth24F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
                return DXGIFormat.D24_UNORM_S8_UINT;

            //--------------------DXT1 / BC1--------------------
            case T3SurfaceFormat.eSurface_BC1:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.BC1_UNORM_SRGB;
                else
                    return DXGIFormat.BC1_UNORM;

            //--------------------DXT2 and DXT3 / BC2--------------------
            case T3SurfaceFormat.eSurface_BC2:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.BC2_UNORM_SRGB;
                else
                    return DXGIFormat.BC2_UNORM;

            //--------------------DXT4 and DXT5 / BC3--------------------
            case T3SurfaceFormat.eSurface_BC3:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.BC3_UNORM_SRGB;
                else
                    return DXGIFormat.BC3_UNORM;

            //--------------------ATI1 / BC4--------------------
            case T3SurfaceFormat.eSurface_BC4:
                return DXGIFormat.BC4_UNORM;

            //--------------------ATI2 / BC5--------------------
            case T3SurfaceFormat.eSurface_BC5:
                return DXGIFormat.BC5_UNORM;

            //--------------------BC6H--------------------
            case T3SurfaceFormat.eSurface_BC6:
                return DXGIFormat.BC6H_UF16;

            //--------------------BC7--------------------
            case T3SurfaceFormat.eSurface_BC7:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return DXGIFormat.BC7_UNORM_SRGB;
                else
                    return DXGIFormat.BC7_UNORM;

            //--------------------ATC--------------------
            case T3SurfaceFormat.eSurface_ATC_RGB:
                return DXGIFormat.UNKNOWN;

            //--------------------ATCA--------------------
            case T3SurfaceFormat.eSurface_ATC_RGB1A:
                return DXGIFormat.UNKNOWN;

            //--------------------ATCI--------------------
            case T3SurfaceFormat.eSurface_ATC_RGBA:
                return DXGIFormat.UNKNOWN;

            //-------------------PVRTC2--------------------
            case T3SurfaceFormat.eSurface_PVRTC2:
                return DXGIFormat.B8G8R8A8_UNORM;

                //-------------------PVRTC2a--------------------
            case T3SurfaceFormat.eSurface_PVRTC2a:
                return DXGIFormat.B8G8R8A8_UNORM;
            
            //-------------------PVRTC4--------------------
            case T3SurfaceFormat.eSurface_PVRTC4:
                return DXGIFormat.B8G8R8A8_UNORM;

                //-------------------PVRTC4a--------------------
            case T3SurfaceFormat.eSurface_PVRTC4a:
                return DXGIFormat.B8G8R8A8_UNORM;

            //-------------------CTX1--------------------
            case T3SurfaceFormat.eSurface_CTX1:
                return DXGIFormat.BC1_UNORM;

            //--------------------UNKNOWN--------------------
            case T3SurfaceFormat.eSurface_Unknown:
                return DXGIFormat.UNKNOWN;
        }
    }

    /// <summary>
    /// Returns the corresponding Direct3D9 surface format from a Direct3D10/DXGI format.
    /// This is used for the conversion process from .d3dtx to .dds. (Legacy .d3dtx)
    /// </summary>
    /// <param name="dxgiFormat">The DXGI format.</param>
    /// <param name="metadata">The metadata of our .dds file. It is used in determining if the alpha is premultiplied.</param>
    /// <returns>The corresponding Direct3D9 format.</returns>
    public static D3DFormat GetD3DFORMATFromDXGIFormat(DXGIFormat dxgiFormat, TexMetadata metadata)
    {
        return dxgiFormat switch
        {
            DXGIFormat.B8G8R8A8_UNORM => D3DFormat.A8R8G8B8,
            DXGIFormat.B8G8R8X8_UNORM => D3DFormat.X8R8G8B8,
            DXGIFormat.B5G6R5_UNORM => D3DFormat.R5G6B5,
            DXGIFormat.B5G5R5A1_UNORM => D3DFormat.A1R5G5B5,
            DXGIFormat.B4G4R4A4_UNORM => D3DFormat.A4R4G4B4,
            DXGIFormat.A4B4G4R4_UNORM => D3DFormat.A4R4G4B4,
            DXGIFormat.A8_UNORM => D3DFormat.A8,
            DXGIFormat.R10G10B10A2_UNORM => D3DFormat.A2B10G10R10,
            DXGIFormat.R8G8B8A8_UNORM => D3DFormat.A8B8G8R8,
            DXGIFormat.R8G8B8A8_UNORM_SRGB => D3DFormat.A8B8G8R8,
            DXGIFormat.R16G16_UNORM => D3DFormat.G16R16,
            DXGIFormat.R16G16B16A16_UNORM => D3DFormat.A16B16G16R16,
            DXGIFormat.R8_UNORM => D3DFormat.L8,
            DXGIFormat.R8G8_UNORM => D3DFormat.A8L8,
            DXGIFormat.R8G8_SNORM => D3DFormat.V8U8,
            DXGIFormat.R8G8B8A8_SNORM => D3DFormat.Q8W8V8U8,
            DXGIFormat.R16G16_SNORM => D3DFormat.V16U16,
            DXGIFormat.G8R8_G8B8_UNORM => D3DFormat.R8G8_B8G8,
            DXGIFormat.YUY2 => D3DFormat.YUY2,
            DXGIFormat.R8G8_B8G8_UNORM => D3DFormat.G8R8_G8B8,
            DXGIFormat.BC1_UNORM => D3DFormat.DXT1,
            DXGIFormat.BC2_UNORM => metadata.IsPMAlpha() ? D3DFormat.DXT2 : D3DFormat.DXT3,
            DXGIFormat.BC3_UNORM => metadata.IsPMAlpha() ? D3DFormat.DXT4 : D3DFormat.DXT5,
            DXGIFormat.BC4_UNORM => D3DFormat.ATI1,
            DXGIFormat.BC4_SNORM => D3DFormat.BC4S,
            DXGIFormat.BC5_UNORM => D3DFormat.ATI2,
            DXGIFormat.BC5_SNORM => D3DFormat.BC5S,
            DXGIFormat.D16_UNORM => D3DFormat.D16,
            DXGIFormat.D32_FLOAT => D3DFormat.D32F_LOCKABLE,
            DXGIFormat.D24_UNORM_S8_UINT => D3DFormat.D24S8,
            DXGIFormat.R16_UNORM => D3DFormat.L16,
            DXGIFormat.R16G16B16A16_SNORM => D3DFormat.Q16W16V16U16,
            DXGIFormat.R16_FLOAT => D3DFormat.R16F,
            DXGIFormat.R16G16_FLOAT => D3DFormat.G16R16F,
            DXGIFormat.R16G16B16A16_FLOAT => D3DFormat.A16B16G16R16F,
            DXGIFormat.R32_FLOAT => D3DFormat.R32F,
            DXGIFormat.R32G32_FLOAT => D3DFormat.G32R32F,
            DXGIFormat.R32G32B32A32_FLOAT => D3DFormat.A32B32G32R32F,

            _ => D3DFormat.UNKNOWN
        };
    }

    /// <summary>
    /// Returns the corresponding Direct3D9 surface format from a Direct3D10/DXGI format.
    /// This is used for the conversion process from .d3dtx to .dds. (Legacy .d3dtx)
    /// </summary>
    /// <param name="dxgiFormat">The DXGI format.</param>
    /// <param name="metadata">The metadata of our .dds file. It is used in determining if the alpha is premultiplied.</param>
    /// <returns>The corresponding Direct3D9 format.</returns>
    public static DxgiFormat GetDXGIFormatFromD3DFormat(D3DFormat format)
    {
        return format switch
        {
            D3DFormat.A8R8G8B8 => DxgiFormat.B8G8R8A8_UNORM,
            D3DFormat.X8R8G8B8 => DxgiFormat.B8G8R8X8_UNORM,
            D3DFormat.X8L8V8U8 => DxgiFormat.B8G8R8A8_UNORM,
            D3DFormat.R5G6B5 => DxgiFormat.B5G6R5_UNORM,
            D3DFormat.X1R5G5B5 => DxgiFormat.B5G5R5A1_UNORM,
            D3DFormat.A1R5G5B5 => DxgiFormat.B5G5R5A1_UNORM,
            D3DFormat.A4R4G4B4 => DxgiFormat.B4G4R4A4_UNORM,
            D3DFormat.A8 => DxgiFormat.A8_UNORM,
            D3DFormat.A2B10G10R10 => DxgiFormat.R10G10B10A2_UNORM,
            D3DFormat.A8B8G8R8 => DxgiFormat.R8G8B8A8_UNORM,
            D3DFormat.X8B8G8R8 => DxgiFormat.R8G8B8A8_UNORM,
            D3DFormat.G16R16 => DxgiFormat.R16G16_UNORM,
            D3DFormat.A16B16G16R16 => DxgiFormat.R16G16B16A16_UNORM,
            D3DFormat.L8 => DxgiFormat.R8_UNORM,
            D3DFormat.A8L8 => DxgiFormat.R8G8_UNORM,
            D3DFormat.V8U8 => DxgiFormat.R8G8_SNORM,
            D3DFormat.Q8W8V8U8 => DxgiFormat.R8G8B8A8_SNORM,
            D3DFormat.V16U16 => DxgiFormat.R16G16_SNORM,
            D3DFormat.R8G8_B8G8 => DxgiFormat.G8R8_G8B8_UNORM,
            D3DFormat.YUY2 => DxgiFormat.YUY2,
            D3DFormat.G8R8_G8B8 => DxgiFormat.R8G8_B8G8_UNORM,
            D3DFormat.DXT1 => DxgiFormat.BC1_UNORM,
            D3DFormat.DXT2 => DxgiFormat.BC2_UNORM,
            D3DFormat.DXT3 => DxgiFormat.BC2_UNORM,
            D3DFormat.DXT4 => DxgiFormat.BC3_UNORM,
            D3DFormat.DXT5 => DxgiFormat.BC3_UNORM,
            D3DFormat.ATI1 => DxgiFormat.BC4_UNORM,
            D3DFormat.BC4S => DxgiFormat.BC4_SNORM,
            D3DFormat.ATI2 => DxgiFormat.BC5_UNORM,
            D3DFormat.BC5S => DxgiFormat.BC5_SNORM,
            D3DFormat.D16 => DxgiFormat.D16_UNORM,
            D3DFormat.D32F_LOCKABLE => DxgiFormat.D32_FLOAT,
            D3DFormat.D24S8 => DxgiFormat.D24_UNORM_S8_UINT,
            D3DFormat.L16 => DxgiFormat.R16_UNORM,
            D3DFormat.Q16W16V16U16 => DxgiFormat.R16G16B16A16_SNORM,
            D3DFormat.R16F => DxgiFormat.R16_FLOAT,
            D3DFormat.G16R16F => DxgiFormat.R16G16_FLOAT,
            D3DFormat.A16B16G16R16F => DxgiFormat.R16G16B16A16_FLOAT,
            D3DFormat.R32F => DxgiFormat.R32_FLOAT,
            D3DFormat.G32R32F => DxgiFormat.R32G32_FLOAT,
            D3DFormat.A32B32G32R32F => DxgiFormat.R32G32B32A32_FLOAT,

            _ => DxgiFormat.UNKNOWN
        };
    }

    /// <summary>
    /// Returns the corresponding Direct3D9 surface format from a Direct3D10/DXGI format.
    /// This is used for the conversion process from .d3dtx to .dds. (Legacy .d3dtx)
    /// </summary>
    /// <param name="dxgiFormat">The DXGI format.</param>
    /// <param name="metadata">The metadata of our .dds file. It is used in determining if the alpha is premultiplied.</param>
    /// <returns>The corresponding Direct3D9 format.</returns>
    public static bool IsPremultipliedAlpha(D3DFormat format)
    {
        return format switch
        {
            D3DFormat.DXT2 => true,
            D3DFormat.DXT4 => true,

            _ => false
        };
    }
}
