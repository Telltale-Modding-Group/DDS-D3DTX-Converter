using D3DTX_Converter.TelltaleEnums;
using static Ktx.Ktx2;

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
public static partial class KTX2_HELPER
{
    /// <summary>
    /// Get the Telltale surface format from a DXGI format.
    /// This is used for the conversion process from .dds to .d3dtx.
    /// </summary>
    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
    /// <returns>The corresponding T3SurfaceFormat enum from the DXGI format.</returns>
    private static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(VkFormat vkFormat)
    {
        return vkFormat switch
        {
            VkFormat.B8G8R8A8Snorm => T3SurfaceFormat.eSurface_ARGB8,
            VkFormat.B8G8R8A8Srgb => T3SurfaceFormat.eSurface_ARGB8,
            VkFormat.R16G16B16A16Unorm => T3SurfaceFormat.eSurface_ARGB16,
            VkFormat.B5G6R5UnormPack16 => T3SurfaceFormat.eSurface_RGB565,
            VkFormat.B5G5R5A1UnormPack16 => T3SurfaceFormat.eSurface_ARGB1555,
            VkFormat.B4G4R4A4UnormPack16 => T3SurfaceFormat.eSurface_ARGB4,
            VkFormat.A2R10G10B10UnormPack32 => T3SurfaceFormat.eSurface_ARGB2101010,
            VkFormat.R16G16Unorm => T3SurfaceFormat.eSurface_RG16,
            VkFormat.R8G8B8A8Unorm => T3SurfaceFormat.eSurface_RGBA8,
            VkFormat.R8G8B8A8Srgb => T3SurfaceFormat.eSurface_RGBA8,
            VkFormat.R32Uint => T3SurfaceFormat.eSurface_R32,
            VkFormat.R32G32Uint => T3SurfaceFormat.eSurface_RG32,
            VkFormat.R32G32B32A32Sfloat => T3SurfaceFormat.eSurface_RGBA32F,
            VkFormat.R8Unorm => T3SurfaceFormat.eSurface_L8,
            VkFormat.R8Srgb => T3SurfaceFormat.eSurface_L8,
            VkFormat.R8G8Unorm => T3SurfaceFormat.eSurface_AL8,
            VkFormat.R8G8Srgb => T3SurfaceFormat.eSurface_AL8,
            VkFormat.R16Unorm => T3SurfaceFormat.eSurface_L16,
            VkFormat.R16G16Snorm => T3SurfaceFormat.eSurface_RG16S,
            VkFormat.R8G8B8A8Snorm => T3SurfaceFormat.eSurface_RGBA8S,
            VkFormat.R16G16B16A16Snorm => T3SurfaceFormat.eSurface_RGBA16S,
            VkFormat.R16G16B16A16Uint => T3SurfaceFormat.eSurface_R16UI,
            VkFormat.R16Sfloat => T3SurfaceFormat.eSurface_R16F,
            VkFormat.R16G16B16A16Sfloat => T3SurfaceFormat.eSurface_RGBA16F,
            VkFormat.R32Sfloat => T3SurfaceFormat.eSurface_R32F,
            VkFormat.R32G32Sfloat => T3SurfaceFormat.eSurface_RG32F,
            VkFormat.R32G32B32A32Uint => T3SurfaceFormat.eSurface_RGBA32,
            VkFormat.Bc1RgbaUnormBlock => T3SurfaceFormat.eSurface_BC1,
            VkFormat.Bc2UnormBlock => T3SurfaceFormat.eSurface_BC2,
            VkFormat.Bc3UnormBlock => T3SurfaceFormat.eSurface_BC3,
            VkFormat.Bc4UnormBlock => T3SurfaceFormat.eSurface_BC4,
            VkFormat.Bc5UnormBlock => T3SurfaceFormat.eSurface_BC5,
            VkFormat.Bc6HUfloatBlock => T3SurfaceFormat.eSurface_BC6,
            VkFormat.Bc7UnormBlock => T3SurfaceFormat.eSurface_BC7,
            VkFormat.Bc1RgbSrgbBlock => T3SurfaceFormat.eSurface_BC1,
            VkFormat.Bc1RgbaSrgbBlock => T3SurfaceFormat.eSurface_BC1,
            VkFormat.Bc2SrgbBlock => T3SurfaceFormat.eSurface_BC2,
            VkFormat.Bc3SrgbBlock => T3SurfaceFormat.eSurface_BC3,
            VkFormat.Bc7SrgbBlock => T3SurfaceFormat.eSurface_BC7,

            VkFormat.Astc4X4UnormBlock => T3SurfaceFormat.eSurface_ATSC_RGBA_4x4,
            VkFormat.Astc4X4SrgbBlock => T3SurfaceFormat.eSurface_ATSC_RGBA_4x4,

            VkFormat.Etc2R8G8B8A1SrgbBlock => T3SurfaceFormat.eSurface_ETC2_RGB1A,
            VkFormat.Etc2R8G8B8A1UnormBlock => T3SurfaceFormat.eSurface_ETC2_RGB1A,

            VkFormat.Etc2R8G8B8A8SrgbBlock => T3SurfaceFormat.eSurface_ETC2_RGBA,
            VkFormat.Etc2R8G8B8A8UnormBlock => T3SurfaceFormat.eSurface_ETC2_RGBA,

            VkFormat.Etc2R8G8B8SrgbBlock => T3SurfaceFormat.eSurface_ETC2_RGB,
            VkFormat.Etc2R8G8B8UnormBlock => T3SurfaceFormat.eSurface_ETC2_RGB,

            VkFormat.EacR11UnormBlock => T3SurfaceFormat.eSurface_ETC2_R,
            VkFormat.EacR11SnormBlock => T3SurfaceFormat.eSurface_ETC2_R,

            VkFormat.EacR11G11UnormBlock => T3SurfaceFormat.eSurface_ETC2_RG,
            VkFormat.EacR11G11SnormBlock => T3SurfaceFormat.eSurface_ETC2_RG,

            VkFormat.Pvrtc12BppSrgbBlockImg => T3SurfaceFormat.eSurface_PVRTC2,
            VkFormat.Pvrtc12BppUnormBlockImg => T3SurfaceFormat.eSurface_PVRTC2,
            VkFormat.Pvrtc14BppSrgbBlockImg => T3SurfaceFormat.eSurface_PVRTC2,
            VkFormat.Pvrtc14BppUnormBlockImg => T3SurfaceFormat.eSurface_PVRTC2a,

            VkFormat.Pvrtc22BppSrgbBlockImg => T3SurfaceFormat.eSurface_PVRTC4,
            VkFormat.Pvrtc22BppUnormBlockImg => T3SurfaceFormat.eSurface_PVRTC4,
            VkFormat.Pvrtc24BppSrgbBlockImg => T3SurfaceFormat.eSurface_PVRTC4,
            VkFormat.Pvrtc24BppUnormBlockImg => T3SurfaceFormat.eSurface_PVRTC4,

            _ => T3SurfaceFormat.eSurface_Unknown,
        };
    }

    public static bool HasAlpha(VkFormat format)
    {
        switch (format)
        {
            case VkFormat.R4G4B4A4UnormPack16:
            case VkFormat.B4G4R4A4UnormPack16:
            case VkFormat.R5G5B5A1UnormPack16:
            case VkFormat.B5G5R5A1UnormPack16:
            case VkFormat.A1R5G5B5UnormPack16:
            case VkFormat.R8G8B8A8Unorm:
            case VkFormat.R8G8B8A8Snorm:
            case VkFormat.R8G8B8A8Uscaled:
            case VkFormat.R8G8B8A8Sscaled:
            case VkFormat.R8G8B8A8Uint:
            case VkFormat.R8G8B8A8Sint:
            case VkFormat.R8G8B8A8Srgb:
            case VkFormat.B8G8R8A8Unorm:
            case VkFormat.B8G8R8A8Snorm:
            case VkFormat.B8G8R8A8Uscaled:
            case VkFormat.B8G8R8A8Sscaled:
            case VkFormat.B8G8R8A8Uint:
            case VkFormat.B8G8R8A8Sint:
            case VkFormat.B8G8R8A8Srgb:
            case VkFormat.A8B8G8R8UnormPack32:
            case VkFormat.A8B8G8R8SnormPack32:
            case VkFormat.A8B8G8R8UscaledPack32:
            case VkFormat.A8B8G8R8SscaledPack32:
            case VkFormat.A8B8G8R8UintPack32:
            case VkFormat.A8B8G8R8SintPack32:
            case VkFormat.A8B8G8R8SrgbPack32:
            case VkFormat.A2R10G10B10UnormPack32:
            case VkFormat.A2R10G10B10SnormPack32:
            case VkFormat.A2R10G10B10UscaledPack32:
            case VkFormat.A2R10G10B10SscaledPack32:
            case VkFormat.A2R10G10B10UintPack32:
            case VkFormat.A2R10G10B10SintPack32:
            case VkFormat.A2B10G10R10UnormPack32:
            case VkFormat.A2B10G10R10SnormPack32:
            case VkFormat.A2B10G10R10UscaledPack32:
            case VkFormat.A2B10G10R10SscaledPack32:
            case VkFormat.A2B10G10R10UintPack32:
            case VkFormat.A2B10G10R10SintPack32:
            case VkFormat.R16G16B16A16Unorm:
            case VkFormat.R16G16B16A16Snorm:
            case VkFormat.R16G16B16A16Uscaled:
            case VkFormat.R16G16B16A16Sscaled:
            case VkFormat.R16G16B16A16Uint:
            case VkFormat.R16G16B16A16Sint:
            case VkFormat.R16G16B16A16Sfloat:
            case VkFormat.R32G32B32A32Uint:
            case VkFormat.R32G32B32A32Sint:
            case VkFormat.R32G32B32A32Sfloat:
            case VkFormat.R64G64B64A64Uint:
            case VkFormat.R64G64B64A64Sint:
            case VkFormat.R64G64B64A64Sfloat:
            case VkFormat.Bc1RgbaUnormBlock:
            case VkFormat.Bc1RgbaSrgbBlock:
            case VkFormat.Bc2UnormBlock:
            case VkFormat.Bc2SrgbBlock:
            case VkFormat.Bc3UnormBlock:
            case VkFormat.Bc3SrgbBlock:
            case VkFormat.Bc7UnormBlock:
            case VkFormat.Bc7SrgbBlock:
            case VkFormat.Etc2R8G8B8A1UnormBlock:
            case VkFormat.Etc2R8G8B8A1SrgbBlock:
            case VkFormat.Etc2R8G8B8A8UnormBlock:
            case VkFormat.Etc2R8G8B8A8SrgbBlock:
            case VkFormat.Astc4X4UnormBlock:
            case VkFormat.Astc4X4SrgbBlock:
            case VkFormat.Astc5X4UnormBlock:
            case VkFormat.Astc5X4SrgbBlock:
            case VkFormat.Astc5X5UnormBlock:
            case VkFormat.Astc5X5SrgbBlock:
            case VkFormat.Astc6X5UnormBlock:
            case VkFormat.Astc6X5SrgbBlock:
            case VkFormat.Astc6X6UnormBlock:
            case VkFormat.Astc6X6SrgbBlock:
            case VkFormat.Astc8X5UnormBlock:
            case VkFormat.Astc8X5SrgbBlock:
            case VkFormat.Astc8X6UnormBlock:
            case VkFormat.Astc8X6SrgbBlock:
            case VkFormat.Astc8X8UnormBlock:
            case VkFormat.Astc8X8SrgbBlock:
            case VkFormat.Astc10X5UnormBlock:
            case VkFormat.Astc10X5SrgbBlock:
            case VkFormat.Astc10X6UnormBlock:
            case VkFormat.Astc10X6SrgbBlock:
            case VkFormat.Astc10X8UnormBlock:
            case VkFormat.Astc10X8SrgbBlock:
            case VkFormat.Astc10X10UnormBlock:
            case VkFormat.Astc10X10SrgbBlock:
            case VkFormat.Astc12X10UnormBlock:
            case VkFormat.Astc12X10SrgbBlock:
            case VkFormat.Astc12X12UnormBlock:
            case VkFormat.Astc12X12SrgbBlock:
            case VkFormat.G8B8G8R8422Unorm:
            case VkFormat.B8G8R8G8422Unorm:
            case VkFormat.G8B8R83Plane420Unorm:
            case VkFormat.G8B8R82Plane420Unorm:
            case VkFormat.G8B8R83Plane422Unorm:
            case VkFormat.G8B8R82Plane422Unorm:
            case VkFormat.G8B8R83Plane444Unorm:
            case VkFormat.R10X6G10X6B10X6A10X6Unorm4Pack16:
            case VkFormat.G10X6B10X6G10X6R10X6422Unorm4Pack16:
            case VkFormat.B10X6G10X6R10X6G10X6422Unorm4Pack16:
            case VkFormat.G10X6B10X6R10X63Plane420Unorm3Pack16:
            case VkFormat.G10X6B10X6R10X62Plane420Unorm3Pack16:
            case VkFormat.G10X6B10X6R10X63Plane422Unorm3Pack16:
            case VkFormat.G10X6B10X6R10X62Plane422Unorm3Pack16:
            case VkFormat.G10X6B10X6R10X63Plane444Unorm3Pack16:
            case VkFormat.R12X4G12X4B12X4A12X4Unorm4Pack16:
            case VkFormat.G12X4B12X4G12X4R12X4422Unorm4Pack16:
            case VkFormat.B12X4G12X4R12X4G12X4422Unorm4Pack16:
            case VkFormat.G12X4B12X4R12X43Plane420Unorm3Pack16:
            case VkFormat.G12X4B12X4R12X42Plane420Unorm3Pack16:
            case VkFormat.G12X4B12X4R12X43Plane422Unorm3Pack16:
            case VkFormat.G12X4B12X4R12X42Plane422Unorm3Pack16:
            case VkFormat.G12X4B12X4R12X43Plane444Unorm3Pack16:
            case VkFormat.G16B16R163Plane420Unorm:
            case VkFormat.G16B16R162Plane420Unorm:
            case VkFormat.G16B16R163Plane422Unorm:
            case VkFormat.G16B16R162Plane422Unorm:
            case VkFormat.G16B16R163Plane444Unorm:
            case VkFormat.Pvrtc12BppUnormBlockImg:
            case VkFormat.Pvrtc12BppSrgbBlockImg:
            case VkFormat.Pvrtc14BppUnormBlockImg:
            case VkFormat.Pvrtc14BppSrgbBlockImg:
            case VkFormat.Pvrtc22BppUnormBlockImg:
            case VkFormat.Pvrtc22BppSrgbBlockImg:
            case VkFormat.Pvrtc24BppUnormBlockImg:
            case VkFormat.Pvrtc24BppSrgbBlockImg:
                return true;
            default:
                return false;
        }
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
    public static T3SurfaceFormat GetTelltaleSurfaceFormatFromVulkan(VkFormat vkFormat, T3SurfaceFormat surfaceFormat = T3SurfaceFormat.eSurface_Unknown)
    {
        T3SurfaceFormat surfaceFormatFromDXGI = GetTelltaleSurfaceFormatFromVulkan(vkFormat);

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
    /// Returns the corresponding VkFormat from a Telltale surface format. 
    /// This is used for the conversion process from .d3dtx to .dds.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="gamma"></param>
    /// <returns>The corresponding VkFormat.</returns>
    public static VkFormat GetVkFormatFromTelltaleSurfaceFormat(T3SurfaceFormat format, T3SurfaceGamma gamma = T3SurfaceGamma.eSurfaceGamma_Linear, eTxAlpha alpha = eTxAlpha.eTxNoAlpha)
    {
        switch (format)
        {
            default:
                return VkFormat.R8G8B8A8Unorm; // Choose R8G8B8A8 if the format is not specified. (Raw data)

            // In order of T3SurfaceFormat enum
            //--------------------ARGB8--------------------
            case T3SurfaceFormat.eSurface_ARGB8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.B8G8R8A8Srgb;
                else
                    return VkFormat.B8G8R8A8Unorm;
            //--------------------ARGB16--------------------
            case T3SurfaceFormat.eSurface_ARGB16:
                return VkFormat.R16G16B16A16Unorm;

            //--------------------RGB565--------------------
            case T3SurfaceFormat.eSurface_RGB565:
                return VkFormat.B5G6R5UnormPack16;

            //--------------------ARGB1555--------------------
            case T3SurfaceFormat.eSurface_ARGB1555:
                return VkFormat.B5G5R5A1UnormPack16;

            //--------------------ARGB4--------------------
            case T3SurfaceFormat.eSurface_ARGB4:
                return VkFormat.B4G4R4A4UnormPack16;

            //--------------------ARGB2101010--------------------
            case T3SurfaceFormat.eSurface_ARGB2101010:
                return VkFormat.A2B10G10R10UnormPack32;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_R16:
                return VkFormat.R16Unorm;

            //--------------------RG16--------------------
            case T3SurfaceFormat.eSurface_RG16:
                return VkFormat.R16G16Unorm;

            //--------------------RGBA16--------------------
            case T3SurfaceFormat.eSurface_RGBA16:
                return VkFormat.R16G16B16A16Unorm;

            //--------------------RG8--------------------
            case T3SurfaceFormat.eSurface_RG8:
                return VkFormat.R8G8Unorm;

            //--------------------RGBA8--------------------
            case T3SurfaceFormat.eSurface_RGBA8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.R8G8B8A8Srgb;
                else
                    return VkFormat.R8G8B8A8Unorm;

            //--------------------R32--------------------
            case T3SurfaceFormat.eSurface_R32:
                return VkFormat.R32Sfloat;

            //--------------------RG32--------------------
            case T3SurfaceFormat.eSurface_RG32:
                return VkFormat.R32G32Sfloat;

            //--------------------RGBA32--------------------
            case T3SurfaceFormat.eSurface_RGBA32:
                return VkFormat.R32G32B32A32Sfloat; // It could be UINT

            //--------------------R8--------------------
            case T3SurfaceFormat.eSurface_R8:
                return VkFormat.R8Unorm;

            //--------------------RGBA8S--------------------
            case T3SurfaceFormat.eSurface_RGBA8S:
                return VkFormat.R8G8B8A8Snorm;

            //--------------------L8--------------------
            case T3SurfaceFormat.eSurface_L8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.R8Srgb;
                else
                    return VkFormat.R8Unorm;

            //--------------------AL8--------------------
            case T3SurfaceFormat.eSurface_AL8:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.R8G8Srgb;
                else
                    return VkFormat.R8G8Unorm;

            //--------------------R16--------------------
            case T3SurfaceFormat.eSurface_L16:
                return VkFormat.R16Unorm;

            //--------------------RG16S--------------------
            case T3SurfaceFormat.eSurface_RG16S:
                return VkFormat.R16G16Snorm;

            //--------------------RGBA16S--------------------
            case T3SurfaceFormat.eSurface_RGBA16S:
                return VkFormat.R16G16B16A16Snorm;

            //--------------------RGBA16UI--------------------
            case T3SurfaceFormat.eSurface_R16UI:
                return VkFormat.R16G16B16A16Uint;

            //--------------------RG16F--------------------
            case T3SurfaceFormat.eSurface_R16F:
                return VkFormat.R16Sfloat;

            //--------------------RGBA16F--------------------
            case T3SurfaceFormat.eSurface_RGBA16F:
                return VkFormat.R16G16B16A16Sfloat;

            //--------------------R32F--------------------
            case T3SurfaceFormat.eSurface_R32F:
                return VkFormat.R32Sfloat;

            //--------------------RG32F--------------------
            case T3SurfaceFormat.eSurface_RG32F:
                return VkFormat.R32G32Sfloat;

            //--------------------RGBA32F--------------------
            case T3SurfaceFormat.eSurface_RGBA32F:
                return VkFormat.R32G32B32A32Sfloat;

            //--------------------RGBA1010102F--------------------
            case T3SurfaceFormat.eSurface_RGBA1010102F:
                return VkFormat.A2B10G10R10UnormPack32;

            //--------------------RGB111110F--------------------
            case T3SurfaceFormat.eSurface_RGB111110F:
                return VkFormat.B10G11R11UfloatPack32;

            //--------------------RGB9E5F--------------------
            case T3SurfaceFormat.eSurface_RGB9E5F:
                return VkFormat.E5B9G9R9UfloatPack32;

            //--------------------DepthPCF16--------------------
            case T3SurfaceFormat.eSurface_DepthPCF16:
                return VkFormat.D16Unorm;

            //--------------------DepthPCF24--------------------
            case T3SurfaceFormat.eSurface_DepthPCF24:
                return VkFormat.D24UnormS8Uint;

            //--------------------Depth16--------------------
            case T3SurfaceFormat.eSurface_Depth16:
                return VkFormat.D16Unorm;

            //--------------------Depth24--------------------
            case T3SurfaceFormat.eSurface_Depth24:
                return VkFormat.D24UnormS8Uint;

            //--------------------DepthStencil32--------------------
            case T3SurfaceFormat.eSurface_DepthStencil32:
                return VkFormat.D32SfloatS8Uint;

            //--------------------Depth32F--------------------
            case T3SurfaceFormat.eSurface_Depth32F:
                return VkFormat.D32Sfloat;

            //--------------------Depth32F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth32F_Stencil8:
                return VkFormat.D32SfloatS8Uint;

            //--------------------Depth24F_Stencil8--------------------
            case T3SurfaceFormat.eSurface_Depth24F_Stencil8:
                return VkFormat.D24UnormS8Uint;

            //--------------------DXT1 / BC1--------------------
            case T3SurfaceFormat.eSurface_BC1:
                if (alpha > 0)
                {
                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return VkFormat.Bc1RgbaSrgbBlock;
                    else
                        return VkFormat.Bc1RgbaUnormBlock;
                }
                else
                {

                    if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                        return VkFormat.Bc1RgbSrgbBlock;
                    else
                        return VkFormat.Bc1RgbUnormBlock;
                }
            //--------------------DXT2 and DXT3 / BC2--------------------
            case T3SurfaceFormat.eSurface_BC2:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Bc2SrgbBlock;
                else
                    return VkFormat.Bc2UnormBlock;

            //--------------------DXT4 and DXT5 / BC3--------------------
            case T3SurfaceFormat.eSurface_BC3:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Bc3SrgbBlock;
                else
                    return VkFormat.Bc3UnormBlock;

            //--------------------ATI1 / BC4--------------------
            case T3SurfaceFormat.eSurface_BC4:
                return VkFormat.Bc4UnormBlock;

            //--------------------ATI2 / BC5--------------------
            case T3SurfaceFormat.eSurface_BC5:
                return VkFormat.Bc5UnormBlock;

            //--------------------BC6H--------------------
            case T3SurfaceFormat.eSurface_BC6:
                return VkFormat.Bc6HUfloatBlock;

            //--------------------BC7--------------------
            case T3SurfaceFormat.eSurface_BC7:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Bc7SrgbBlock;
                else
                    return VkFormat.Bc7UnormBlock;

            //--------------------ATSC_RGBA_4x4--------------------
            case T3SurfaceFormat.eSurface_ATSC_RGBA_4x4:
                return VkFormat.Astc4X4UnormBlock;

            //--------------------ETC2_RGB1A--------------------
            case T3SurfaceFormat.eSurface_ETC2_RGB1A:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Etc2R8G8B8A1SrgbBlock;
                else
                    return VkFormat.Etc2R8G8B8A1UnormBlock;

            //--------------------ETC2_RGBA--------------------
            case T3SurfaceFormat.eSurface_ETC2_RGBA:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Etc2R8G8B8A8SrgbBlock;
                else
                    return VkFormat.Etc2R8G8B8A8UnormBlock;

            //--------------------ETC2_RGB--------------------
            case T3SurfaceFormat.eSurface_ETC2_RGB:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Etc2R8G8B8SrgbBlock;
                else
                    return VkFormat.Etc2R8G8B8UnormBlock;

            //--------------------ETC2_R--------------------
            case T3SurfaceFormat.eSurface_ETC2_R:
                return VkFormat.EacR11UnormBlock;

            //--------------------ETC2_RG--------------------
            case T3SurfaceFormat.eSurface_ETC2_RG:
                return VkFormat.EacR11G11UnormBlock;

            //--------------------PVRTC2--------------------
            case T3SurfaceFormat.eSurface_PVRTC2:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Pvrtc12BppSrgbBlockImg;
                else
                    return VkFormat.Pvrtc12BppUnormBlockImg;

            //--------------------PVRTC2a--------------------
            case T3SurfaceFormat.eSurface_PVRTC2a:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Pvrtc22BppSrgbBlockImg;
                else
                    return VkFormat.Pvrtc22BppUnormBlockImg;

            //--------------------PVRTC4--------------------
            case T3SurfaceFormat.eSurface_PVRTC4:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Pvrtc14BppSrgbBlockImg;
                else
                    return VkFormat.Pvrtc14BppUnormBlockImg;

            //--------------------PVRTC4a--------------------
            case T3SurfaceFormat.eSurface_PVRTC4a:
                if (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB)
                    return VkFormat.Pvrtc24BppSrgbBlockImg;
                else
                    return VkFormat.Pvrtc24BppUnormBlockImg;

            //--------------------UNKNOWN--------------------
            case T3SurfaceFormat.eSurface_Unknown:
                return VkFormat.Undefined;
        }
    }

    private static bool IsSRGB(VkFormat format) => format switch
    {
        VkFormat.R8Srgb => true,
        VkFormat.R8G8Srgb => true,
        VkFormat.R8G8B8Srgb => true,
        VkFormat.R8G8B8A8Srgb => true,
        VkFormat.B8G8R8A8Srgb => true,
        VkFormat.Bc1RgbaSrgbBlock => true,
        VkFormat.Bc2SrgbBlock => true,
        VkFormat.Bc3SrgbBlock => true,
        VkFormat.Bc7SrgbBlock => true,
        VkFormat.Etc2R8G8B8SrgbBlock => true,
        VkFormat.Etc2R8G8B8A1SrgbBlock => true,
        VkFormat.Etc2R8G8B8A8SrgbBlock => true,
        VkFormat.Astc4X4SrgbBlock => true,
        VkFormat.Astc5X5SrgbBlock => true,
        VkFormat.Pvrtc12BppSrgbBlockImg => true,
        VkFormat.Pvrtc14BppSrgbBlockImg => true,
        VkFormat.Pvrtc22BppSrgbBlockImg => true,
        VkFormat.Pvrtc24BppSrgbBlockImg => true,
        _ => false
    };
}
