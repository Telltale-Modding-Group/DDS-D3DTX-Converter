//using TelltaleTextureTool.TelltaleEnums;
//using static Ktx.Ktx2;

//namespace TelltaleTextureTool.DirectX;

//// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
//// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
//// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
//// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
//// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
//// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide
//// D3DFORMAT - https://learn.microsoft.com/en-us/windows/win32/direct3d9/d3dformat
//// Map Direct3D 9 Formats to Direct3D 10 - https://learn.microsoft.com/en-gb/windows/win32/direct3d10/d3d10-graphics-programming-guide-resources-legacy-formats?redirectedfrom=MSDN

///// <summary>
///// The class is used for decoding and encoding .dds headers. 
///// </summary>
//public static partial class KTX2_HELPER
//{
//    /// <summary>
//    /// Get the Telltale surface format from a DXGI format.
//    /// This is used for the conversion process from .dds to .d3dtx.
//    /// </summary>
//    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
//    /// <returns>The corresponding T3SurfaceFormat enum from the DXGI format.</returns>
//    private static T3SurfaceFormat GetTelltaleSurfaceFormatFromDXGI(VkFormat vkFormat)
//    {
//        return vkFormat switch
//        {
//            VkFormat.B8G8R8A8Snorm => T3SurfaceFormat.ARGB8,
//            VkFormat.B8G8R8A8Srgb => T3SurfaceFormat.ARGB8,
//            VkFormat.R16G16B16A16Unorm => T3SurfaceFormat.ARGB16,
//            VkFormat.B5G6R5UnormPack16 => T3SurfaceFormat.RGB565,
//            VkFormat.B5G5R5A1UnormPack16 => T3SurfaceFormat.ARGB1555,
//            VkFormat.B4G4R4A4UnormPack16 => T3SurfaceFormat.ARGB4,
//            VkFormat.A2R10G10B10UnormPack32 => T3SurfaceFormat.ARGB2101010,
//            VkFormat.R16G16Unorm => T3SurfaceFormat.RG16,
//            VkFormat.R8G8B8A8Unorm => T3SurfaceFormat.RGBA8,
//            VkFormat.R8G8B8A8Srgb => T3SurfaceFormat.RGBA8,
//            VkFormat.R32Uint => T3SurfaceFormat.R32,
//            VkFormat.R32G32Uint => T3SurfaceFormat.RG32,
//            VkFormat.R32G32B32A32Sfloat => T3SurfaceFormat.RGBA32F,
//            VkFormat.R8Unorm => T3SurfaceFormat.L8,
//            VkFormat.R8Srgb => T3SurfaceFormat.L8,
//            VkFormat.R8G8Unorm => T3SurfaceFormat.AL8,
//            VkFormat.R8G8Srgb => T3SurfaceFormat.AL8,
//            VkFormat.R16Unorm => T3SurfaceFormat.L16,
//            VkFormat.R16G16Snorm => T3SurfaceFormat.RG16S,
//            VkFormat.R8G8B8A8Snorm => T3SurfaceFormat.RGBA8S,
//            VkFormat.R16G16B16A16Snorm => T3SurfaceFormat.RGBA16S,
//            VkFormat.R16G16B16A16Uint => T3SurfaceFormat.R16UI,
//            VkFormat.R16Sfloat => T3SurfaceFormat.R16F,
//            VkFormat.R16G16B16A16Sfloat => T3SurfaceFormat.RGBA16F,
//            VkFormat.R32Sfloat => T3SurfaceFormat.R32F,
//            VkFormat.R32G32Sfloat => T3SurfaceFormat.RG32F,
//            VkFormat.R32G32B32A32Uint => T3SurfaceFormat.RGBA32,
//            // VkFormat.BC1RgbaUnormBlock => T3SurfaceFormat.eSurface_BC1,
//            // VkFormat.BC2UnormBlock => T3SurfaceFormat.eSurface_BC2,
//            // VkFormat.BC3UnormBlock => T3SurfaceFormat.eSurface_BC3,
//            // VkFormat.BC4UnormBlock => T3SurfaceFormat.eSurface_BC4,
//            // VkFormat.BC5UnormBlock => T3SurfaceFormat.eSurface_BC5,
//            // VkFormat.BC6HUfloatBlock => T3SurfaceFormat.eSurface_BC6,
//            // VkFormat.BC7UnormBlock => T3SurfaceFormat.eSurface_BC7,
//            // VkFormat.BC1RgbSrgbBlock => T3SurfaceFormat.eSurface_BC1,
//            // VkFormat.BC1RgbaSrgbBlock => T3SurfaceFormat.eSurface_BC1,
//            // VkFormat.BC2SrgbBlock => T3SurfaceFormat.eSurface_BC2,
//            // VkFormat.BC3SrgbBlock => T3SurfaceFormat.eSurface_BC3,
//            // VkFormat.BC7SrgbBlock => T3SurfaceFormat.eSurface_BC7,

//            VkFormat.Astc4X4UnormBlock => T3SurfaceFormat.ATSC_RGBA_4x4,
//            VkFormat.Astc4X4SrgbBlock => T3SurfaceFormat.ATSC_RGBA_4x4,

//            VkFormat.Etc2R8G8B8A1SrgbBlock => T3SurfaceFormat.ETC2_RGB1A,
//            VkFormat.Etc2R8G8B8A1UnormBlock => T3SurfaceFormat.ETC2_RGB1A,

//            VkFormat.Etc2R8G8B8A8SrgbBlock => T3SurfaceFormat.ETC2_RGBA,
//            VkFormat.Etc2R8G8B8A8UnormBlock => T3SurfaceFormat.ETC2_RGBA,

//            VkFormat.Etc2R8G8B8SrgbBlock => T3SurfaceFormat.ETC2_RGB,
//            VkFormat.Etc2R8G8B8UnormBlock => T3SurfaceFormat.ETC2_RGB,

//            VkFormat.EacR11UnormBlock => T3SurfaceFormat.ETC2_R,
//            VkFormat.EacR11SnormBlock => T3SurfaceFormat.ETC2_R,

//            VkFormat.EacR11G11UnormBlock => T3SurfaceFormat.ETC2_RG,
//            VkFormat.EacR11G11SnormBlock => T3SurfaceFormat.ETC2_RG,

//            VkFormat.Pvrtc12BppSrgbBlockImg => T3SurfaceFormat.PVRTC2,
//            VkFormat.Pvrtc12BppUnormBlockImg => T3SurfaceFormat.PVRTC2,
//            VkFormat.Pvrtc14BppSrgbBlockImg => T3SurfaceFormat.PVRTC2,
//            VkFormat.Pvrtc14BppUnormBlockImg => T3SurfaceFormat.PVRTC2a,

//            VkFormat.Pvrtc22BppSrgbBlockImg => T3SurfaceFormat.PVRTC4,
//            VkFormat.Pvrtc22BppUnormBlockImg => T3SurfaceFormat.PVRTC4,
//            VkFormat.Pvrtc24BppSrgbBlockImg => T3SurfaceFormat.PVRTC4,
//            VkFormat.Pvrtc24BppUnormBlockImg => T3SurfaceFormat.PVRTC4,

//            _ => T3SurfaceFormat.Unknown,
//        };
//    }

//    public static bool HasAlpha(VkFormat format)
//    {
//        switch (format)
//        {
//            case VkFormat.R4G4B4A4UnormPack16:
//            case VkFormat.B4G4R4A4UnormPack16:
//            case VkFormat.R5G5B5A1UnormPack16:
//            case VkFormat.B5G5R5A1UnormPack16:
//            case VkFormat.A1R5G5B5UnormPack16:
//            case VkFormat.R8G8B8A8Unorm:
//            case VkFormat.R8G8B8A8Snorm:
//            case VkFormat.R8G8B8A8Uscaled:
//            case VkFormat.R8G8B8A8Sscaled:
//            case VkFormat.R8G8B8A8Uint:
//            case VkFormat.R8G8B8A8Sint:
//            case VkFormat.R8G8B8A8Srgb:
//            case VkFormat.B8G8R8A8Unorm:
//            case VkFormat.B8G8R8A8Snorm:
//            case VkFormat.B8G8R8A8Uscaled:
//            case VkFormat.B8G8R8A8Sscaled:
//            case VkFormat.B8G8R8A8Uint:
//            case VkFormat.B8G8R8A8Sint:
//            case VkFormat.B8G8R8A8Srgb:
//            case VkFormat.A8B8G8R8UnormPack32:
//            case VkFormat.A8B8G8R8SnormPack32:
//            case VkFormat.A8B8G8R8UscaledPack32:
//            case VkFormat.A8B8G8R8SscaledPack32:
//            case VkFormat.A8B8G8R8UintPack32:
//            case VkFormat.A8B8G8R8SintPack32:
//            case VkFormat.A8B8G8R8SrgbPack32:
//            case VkFormat.A2R10G10B10UnormPack32:
//            case VkFormat.A2R10G10B10SnormPack32:
//            case VkFormat.A2R10G10B10UscaledPack32:
//            case VkFormat.A2R10G10B10SscaledPack32:
//            case VkFormat.A2R10G10B10UintPack32:
//            case VkFormat.A2R10G10B10SintPack32:
//            case VkFormat.A2B10G10R10UnormPack32:
//            case VkFormat.A2B10G10R10SnormPack32:
//            case VkFormat.A2B10G10R10UscaledPack32:
//            case VkFormat.A2B10G10R10SscaledPack32:
//            case VkFormat.A2B10G10R10UintPack32:
//            case VkFormat.A2B10G10R10SintPack32:
//            case VkFormat.R16G16B16A16Unorm:
//            case VkFormat.R16G16B16A16Snorm:
//            case VkFormat.R16G16B16A16Uscaled:
//            case VkFormat.R16G16B16A16Sscaled:
//            case VkFormat.R16G16B16A16Uint:
//            case VkFormat.R16G16B16A16Sint:
//            case VkFormat.R16G16B16A16Sfloat:
//            case VkFormat.R32G32B32A32Uint:
//            case VkFormat.R32G32B32A32Sint:
//            case VkFormat.R32G32B32A32Sfloat:
//            case VkFormat.R64G64B64A64Uint:
//            case VkFormat.R64G64B64A64Sint:
//            case VkFormat.R64G64B64A64Sfloat:
//            // case VkFormat.BC1RgbaUnormBlock:
//            // case VkFormat.BC1RgbaSrgbBlock:
//            // case VkFormat.BC2UnormBlock:
//            // case VkFormat.BC2SrgbBlock:
//            // case VkFormat.BC3UnormBlock:
//            // case VkFormat.BC3SrgbBlock:
//            // case VkFormat.BC7UnormBlock:
//            // case VkFormat.BC7SrgbBlock:
//            case VkFormat.Etc2R8G8B8A1UnormBlock:
//            case VkFormat.Etc2R8G8B8A1SrgbBlock:
//            case VkFormat.Etc2R8G8B8A8UnormBlock:
//            case VkFormat.Etc2R8G8B8A8SrgbBlock:
//            case VkFormat.Astc4X4UnormBlock:
//            case VkFormat.Astc4X4SrgbBlock:
//            case VkFormat.Astc5X4UnormBlock:
//            case VkFormat.Astc5X4SrgbBlock:
//            case VkFormat.Astc5X5UnormBlock:
//            case VkFormat.Astc5X5SrgbBlock:
//            case VkFormat.Astc6X5UnormBlock:
//            case VkFormat.Astc6X5SrgbBlock:
//            case VkFormat.Astc6X6UnormBlock:
//            case VkFormat.Astc6X6SrgbBlock:
//            case VkFormat.Astc8X5UnormBlock:
//            case VkFormat.Astc8X5SrgbBlock:
//            case VkFormat.Astc8X6UnormBlock:
//            case VkFormat.Astc8X6SrgbBlock:
//            case VkFormat.Astc8X8UnormBlock:
//            case VkFormat.Astc8X8SrgbBlock:
//            case VkFormat.Astc10X5UnormBlock:
//            case VkFormat.Astc10X5SrgbBlock:
//            case VkFormat.Astc10X6UnormBlock:
//            case VkFormat.Astc10X6SrgbBlock:
//            case VkFormat.Astc10X8UnormBlock:
//            case VkFormat.Astc10X8SrgbBlock:
//            case VkFormat.Astc10X10UnormBlock:
//            case VkFormat.Astc10X10SrgbBlock:
//            case VkFormat.Astc12X10UnormBlock:
//            case VkFormat.Astc12X10SrgbBlock:
//            case VkFormat.Astc12X12UnormBlock:
//            case VkFormat.Astc12X12SrgbBlock:
//            case VkFormat.G8B8G8R8422Unorm:
//            case VkFormat.B8G8R8G8422Unorm:
//            case VkFormat.G8B8R83Plane420Unorm:
//            case VkFormat.G8B8R82Plane420Unorm:
//            case VkFormat.G8B8R83Plane422Unorm:
//            case VkFormat.G8B8R82Plane422Unorm:
//            case VkFormat.G8B8R83Plane444Unorm:
//            case VkFormat.R10X6G10X6B10X6A10X6Unorm4Pack16:
//            case VkFormat.G10X6B10X6G10X6R10X6422Unorm4Pack16:
//            case VkFormat.B10X6G10X6R10X6G10X6422Unorm4Pack16:
//            case VkFormat.G10X6B10X6R10X63Plane420Unorm3Pack16:
//            case VkFormat.G10X6B10X6R10X62Plane420Unorm3Pack16:
//            case VkFormat.G10X6B10X6R10X63Plane422Unorm3Pack16:
//            case VkFormat.G10X6B10X6R10X62Plane422Unorm3Pack16:
//            case VkFormat.G10X6B10X6R10X63Plane444Unorm3Pack16:
//            case VkFormat.R12X4G12X4B12X4A12X4Unorm4Pack16:
//            case VkFormat.G12X4B12X4G12X4R12X4422Unorm4Pack16:
//            case VkFormat.B12X4G12X4R12X4G12X4422Unorm4Pack16:
//            case VkFormat.G12X4B12X4R12X43Plane420Unorm3Pack16:
//            case VkFormat.G12X4B12X4R12X42Plane420Unorm3Pack16:
//            case VkFormat.G12X4B12X4R12X43Plane422Unorm3Pack16:
//            case VkFormat.G12X4B12X4R12X42Plane422Unorm3Pack16:
//            case VkFormat.G12X4B12X4R12X43Plane444Unorm3Pack16:
//            case VkFormat.G16B16R163Plane420Unorm:
//            case VkFormat.G16B16R162Plane420Unorm:
//            case VkFormat.G16B16R163Plane422Unorm:
//            case VkFormat.G16B16R162Plane422Unorm:
//            case VkFormat.G16B16R163Plane444Unorm:
//            case VkFormat.Pvrtc12BppUnormBlockImg:
//            case VkFormat.Pvrtc12BppSrgbBlockImg:
//            case VkFormat.Pvrtc14BppUnormBlockImg:
//            case VkFormat.Pvrtc14BppSrgbBlockImg:
//            case VkFormat.Pvrtc22BppUnormBlockImg:
//            case VkFormat.Pvrtc22BppSrgbBlockImg:
//            case VkFormat.Pvrtc24BppUnormBlockImg:
//            case VkFormat.Pvrtc24BppSrgbBlockImg:
//                return true;
//            default:
//                return false;
//        }
//    }

//    /// <summary>
//    /// Get the Telltale surface format from a DXGI format and an already existing surface format.
//    /// This is used for the conversion process from .dds to .d3dtx.
//    /// This is used when equivalent formats are found. 
//    /// Some Telltale games have different values for the same formats, but they do not ship with all of them.
//    /// This can create issues if the Telltale surface format is not found in the game. 
//    /// In any case, use Lucas's Telltale Inspector to change the value if any issues arise.
//    /// </summary>
//    /// <param name="dxgiFormat">The Direct3D10/DXGI format.</param>
//    /// <param name="surfaceFormat">(Optional) The existing Telltale surface format. Default value is UNKNOWN.</param>
//    /// <returns>The corresponding T3SurfaceFormat enum from the DXGI format and Telltale surface format.</returns>
//    public static T3SurfaceFormat GetTelltaleSurfaceFormatFromVulkan(VkFormat vkFormat, T3SurfaceFormat surfaceFormat = T3SurfaceFormat.Unknown)
//    {
//        T3SurfaceFormat surfaceFormatFromDXGI = GetTelltaleSurfaceFormatFromVulkan(vkFormat);

//        if (surfaceFormatFromDXGI == T3SurfaceFormat.Unknown)
//        {
//            return surfaceFormat;
//        }

//        if (surfaceFormatFromDXGI == T3SurfaceFormat.L16 && surfaceFormat == T3SurfaceFormat.R16)
//        {
//            return T3SurfaceFormat.L16;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.AL8 && surfaceFormat == T3SurfaceFormat.RG8)
//        {
//            return T3SurfaceFormat.RG8;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.L8 && surfaceFormat == T3SurfaceFormat.R8)
//        {
//            return T3SurfaceFormat.R8;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.ARGB16 && surfaceFormat == T3SurfaceFormat.RGBA16)
//        {
//            return T3SurfaceFormat.RGBA16;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.ARGB2101010 && surfaceFormat == T3SurfaceFormat.RGBA1010102F)
//        {
//            return T3SurfaceFormat.RGBA1010102F;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.R32F && surfaceFormat == T3SurfaceFormat.R32)
//        {
//            return T3SurfaceFormat.R32;
//        }
//        else if (surfaceFormatFromDXGI == T3SurfaceFormat.RG32F && surfaceFormat == T3SurfaceFormat.RG32)
//        {
//            return T3SurfaceFormat.R32;
//        }

//        return surfaceFormatFromDXGI;
//    }

//    /// <summary>
//    /// Returns the corresponding VkFormat from a Telltale surface format. 
//    /// This is used for the conversion process from .d3dtx to .dds.
//    /// </summary>
//    /// <param name="format"></param>
//    /// <param name="gamma"></param>
//    /// <returns>The corresponding VkFormat.</returns>
//    public static VkFormat GetVkFormatFromTelltaleSurfaceFormat(T3SurfaceFormat format, T3SurfaceGamma gamma = T3SurfaceGamma.Linear, T3PlatformType platformType = T3PlatformType.ePlatform_PC, T3TextureAlphaMode alphaMode = T3TextureAlphaMode.Unknown)
//    {
//        VkFormat vkFormat = format switch
//        {
//            // In order of T3SurfaceFormat enum
//            //--------------------ARGB8--------------------
//            T3SurfaceFormat.ARGB8 => gamma == T3SurfaceGamma.sRGB ? VkFormat.B8G8R8A8Srgb : VkFormat.B8G8R8A8Unorm,
//            //--------------------ARGB16--------------------
//            T3SurfaceFormat.ARGB16 => VkFormat.R16G16B16A16Unorm,
//            //--------------------RGB565--------------------
//            T3SurfaceFormat.RGB565 => VkFormat.B5G6R5UnormPack16,
//            //--------------------ARGB1555--------------------
//            T3SurfaceFormat.ARGB1555 => VkFormat.B5G5R5A1UnormPack16,
//            //--------------------ARGB4--------------------
//            T3SurfaceFormat.ARGB4 => VkFormat.B4G4R4A4UnormPack16,
//            //--------------------ARGB2101010--------------------
//            T3SurfaceFormat.ARGB2101010 => VkFormat.A2B10G10R10UnormPack32,
//            //--------------------R16--------------------
//            T3SurfaceFormat.R16 => VkFormat.R16Unorm,
//            //--------------------RG16--------------------
//            T3SurfaceFormat.RG16 => VkFormat.R16G16Unorm,
//            //--------------------RGBA16--------------------
//            T3SurfaceFormat.RGBA16 => VkFormat.R16G16B16A16Unorm,
//            //--------------------RG8--------------------
//            T3SurfaceFormat.RG8 => VkFormat.R8G8Unorm,
//            //--------------------RGBA8--------------------
//            T3SurfaceFormat.RGBA8 => gamma == T3SurfaceGamma.sRGB ? VkFormat.R8G8B8A8Srgb : VkFormat.R8G8B8A8Unorm,
//            //--------------------R32--------------------
//            T3SurfaceFormat.R32 => VkFormat.R32Sfloat,
//            //--------------------RG32--------------------
//            T3SurfaceFormat.RG32 => VkFormat.R32G32Sfloat,
//            //--------------------RGBA32--------------------
//            T3SurfaceFormat.RGBA32 => VkFormat.R32G32B32A32Sfloat, // It could be UINT
//            //--------------------R8--------------------
//            T3SurfaceFormat.R8 => VkFormat.R8Unorm,
//            //--------------------RGBA8S--------------------
//            T3SurfaceFormat.RGBA8S => VkFormat.R8G8B8A8Snorm,
//            //--------------------L8--------------------
//            T3SurfaceFormat.L8 => gamma == T3SurfaceGamma.sRGB ? VkFormat.R8Srgb : VkFormat.R8Unorm,
//            //--------------------AL8--------------------
//            T3SurfaceFormat.AL8 => gamma == T3SurfaceGamma.sRGB ? VkFormat.R8G8Srgb : VkFormat.R8G8Unorm,
//            //--------------------R16--------------------
//            T3SurfaceFormat.L16 => VkFormat.R16Unorm,
//            //--------------------RG16S--------------------
//            T3SurfaceFormat.RG16S => VkFormat.R16G16Snorm,
//            //--------------------RGBA16S--------------------
//            T3SurfaceFormat.RGBA16S => VkFormat.R16G16B16A16Snorm,
//            //--------------------RGBA16UI--------------------
//            T3SurfaceFormat.R16UI => VkFormat.R16G16B16A16Uint,
//            //--------------------RG16F--------------------
//            T3SurfaceFormat.R16F => VkFormat.R16Sfloat,
//            //--------------------RGBA16F--------------------
//            T3SurfaceFormat.RGBA16F => VkFormat.R16G16B16A16Sfloat,
//            //--------------------R32F--------------------
//            T3SurfaceFormat.R32F => VkFormat.R32Sfloat,
//            //--------------------RG32F--------------------
//            T3SurfaceFormat.RG32F => VkFormat.R32G32Sfloat,
//            //--------------------RGBA32F--------------------
//            T3SurfaceFormat.RGBA32F => VkFormat.R32G32B32A32Sfloat,
//            //--------------------RGBA1010102F--------------------
//            T3SurfaceFormat.RGBA1010102F => VkFormat.A2B10G10R10UnormPack32,
//            //--------------------RGB111110F--------------------
//            T3SurfaceFormat.RGB111110F => VkFormat.B10G11R11UfloatPack32,
//            //--------------------RGB9E5F--------------------
//            T3SurfaceFormat.RGB9E5F => VkFormat.E5B9G9R9UfloatPack32,
//            //--------------------DepthPCF16--------------------
//            T3SurfaceFormat.DepthPCF16 => VkFormat.D16Unorm,
//            //--------------------DepthPCF24--------------------
//            T3SurfaceFormat.DepthPCF24 => VkFormat.D24UnormS8Uint,
//            //--------------------Depth16--------------------
//            T3SurfaceFormat.Depth16 => VkFormat.D16Unorm,
//            //--------------------Depth24--------------------
//            T3SurfaceFormat.Depth24 => VkFormat.D24UnormS8Uint,
//            //--------------------DepthStencil32--------------------
//            T3SurfaceFormat.DepthStencil32 => VkFormat.D32SfloatS8Uint,
//            //--------------------Depth32F--------------------
//            T3SurfaceFormat.Depth32F => VkFormat.D32Sfloat,
//            //--------------------Depth32F_Stencil8--------------------
//            T3SurfaceFormat.Depth32F_Stencil8 => VkFormat.D32SfloatS8Uint,
//            //--------------------Depth24F_Stencil8--------------------
//            T3SurfaceFormat.Depth24F_Stencil8 => VkFormat.D24UnormS8Uint,
//            // //--------------------DXT1 / BC1--------------------
//            // T3SurfaceFormat.eSurface_BC1 => alphaMode > 0 ? (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB ? VkFormat.BC1RgbaSrgbBlock : VkFormat.BC1RgbaUnormBlock) : (gamma == T3SurfaceGamma.eSurfaceGamma_sRGB ? VkFormat.BC1RgbSrgbBlock : VkFormat.BC1RgbUnormBlock),
//            // //--------------------DXT2 and DXT3 / BC2--------------------
//            // T3SurfaceFormat.eSurface_BC2 => gamma == T3SurfaceGamma.eSurfaceGamma_sRGB ? VkFormat.BC2SrgbBlock : VkFormat.BC2UnormBlock,
//            // //--------------------DXT4 and DXT5 / BC3--------------------
//            // T3SurfaceFormat.eSurface_BC3 => gamma == T3SurfaceGamma.eSurfaceGamma_sRGB ? VkFormat.BC3SrgbBlock : VkFormat.BC3UnormBlock,
//            // //--------------------ATI1 / BC4--------------------
//            // T3SurfaceFormat.eSurface_BC4 => VkFormat.BC4UnormBlock,
//            // //--------------------ATI2 / BC5--------------------
//            // T3SurfaceFormat.eSurface_BC5 => VkFormat.BC5UnormBlock,
//            // //--------------------BC6H--------------------
//            // T3SurfaceFormat.eSurface_BC6 => VkFormat.BC6HUfloatBlock,
//            // //--------------------BC7--------------------
//            // T3SurfaceFormat.eSurface_BC7 => gamma == T3SurfaceGamma.eSurfaceGamma_sRGB ? VkFormat.BC7SrgbBlock : VkFormat.BC7UnormBlock,
//            // //--------------------ATSC_RGBA_4x4--------------------
//            // T3SurfaceFormat.eSurface_ATSC_RGBA_4x4 => VkFormat.Astc4X4UnormBlock,
//            //--------------------ETC2_RGB1A--------------------
//            T3SurfaceFormat.ETC2_RGB1A => gamma == T3SurfaceGamma.sRGB ? VkFormat.Etc2R8G8B8A1SrgbBlock : VkFormat.Etc2R8G8B8A1UnormBlock,
//            //--------------------ETC2_RGBA--------------------
//            T3SurfaceFormat.ETC2_RGBA => gamma == T3SurfaceGamma.sRGB ? VkFormat.Etc2R8G8B8A8SrgbBlock : VkFormat.Etc2R8G8B8A8UnormBlock,
//            //--------------------ETC2_RGB--------------------
//            T3SurfaceFormat.ETC2_RGB => gamma == T3SurfaceGamma.sRGB ? VkFormat.Etc2R8G8B8SrgbBlock : VkFormat.Etc2R8G8B8UnormBlock,
//            //--------------------ETC2_R--------------------
//            T3SurfaceFormat.ETC2_R => VkFormat.EacR11UnormBlock,
//            //--------------------ETC2_RG--------------------
//            T3SurfaceFormat.ETC2_RG => VkFormat.EacR11G11UnormBlock,
//            //--------------------PVRTC2--------------------
//            T3SurfaceFormat.PVRTC2 => gamma == T3SurfaceGamma.sRGB ? VkFormat.Pvrtc12BppSrgbBlockImg : VkFormat.Pvrtc12BppUnormBlockImg,
//            //--------------------PVRTC2a--------------------
//            T3SurfaceFormat.PVRTC2a => gamma == T3SurfaceGamma.sRGB ? VkFormat.Pvrtc22BppSrgbBlockImg : VkFormat.Pvrtc22BppUnormBlockImg,
//            //--------------------PVRTC4--------------------
//            T3SurfaceFormat.PVRTC4 => gamma == T3SurfaceGamma.sRGB ? VkFormat.Pvrtc14BppSrgbBlockImg : VkFormat.Pvrtc14BppUnormBlockImg,
//            //--------------------PVRTC4a--------------------
//            T3SurfaceFormat.PVRTC4a => gamma == T3SurfaceGamma.sRGB ? VkFormat.Pvrtc24BppSrgbBlockImg : VkFormat.Pvrtc24BppUnormBlockImg,
//            //--------------------UNKNOWN--------------------
//            T3SurfaceFormat.Unknown => VkFormat.Undefined,

//            _ => VkFormat.R8G8B8A8Unorm, // Choose R8G8B8A8 if the format is not specified. (Raw data)
//        };

//        if (platformType == T3PlatformType.ePlatform_Android)
//        {
//            vkFormat = SwitchChannels(vkFormat);
//        }

//        return vkFormat;
//    }

//    private static bool IsSRGB(VkFormat format) => format switch
//    {
//        VkFormat.R8Srgb => true,
//        VkFormat.R8G8Srgb => true,
//        VkFormat.R8G8B8Srgb => true,
//        VkFormat.R8G8B8A8Srgb => true,
//        VkFormat.B8G8R8A8Srgb => true,
//        // VkFormat.BC1RgbaSrgbBlock => true,
//        // VkFormat.BC2SrgbBlock => true,
//        // VkFormat.BC3SrgbBlock => true,
//        // VkFormat.BC7SrgbBlock => true,
//        VkFormat.Etc2R8G8B8SrgbBlock => true,
//        VkFormat.Etc2R8G8B8A1SrgbBlock => true,
//        VkFormat.Etc2R8G8B8A8SrgbBlock => true,
//        VkFormat.Astc4X4SrgbBlock => true,
//        VkFormat.Astc5X5SrgbBlock => true,
//        VkFormat.Pvrtc12BppSrgbBlockImg => true,
//        VkFormat.Pvrtc14BppSrgbBlockImg => true,
//        VkFormat.Pvrtc22BppSrgbBlockImg => true,
//        VkFormat.Pvrtc24BppSrgbBlockImg => true,
//        _ => false
//    };

//    private static VkFormat SwitchChannels(VkFormat format)
//    {
//        return format switch
//        {
//            VkFormat.R8G8B8A8Unorm => VkFormat.B8G8R8A8Unorm,
//            VkFormat.B8G8R8A8Unorm => VkFormat.R8G8B8A8Unorm,
//            VkFormat.R8G8B8A8Srgb => VkFormat.B8G8R8A8Srgb,
//            VkFormat.B8G8R8A8Srgb => VkFormat.R8G8B8A8Srgb,
//            VkFormat.R8G8B8Unorm => VkFormat.B8G8R8Unorm,
//            VkFormat.B8G8R8Unorm => VkFormat.R8G8B8Unorm,
//            VkFormat.R8G8B8Srgb => VkFormat.B8G8R8Srgb,
//            VkFormat.B8G8R8Srgb => VkFormat.R8G8B8Srgb,
//            VkFormat.B4G4R4A4UnormPack16 => VkFormat.R4G4B4A4UnormPack16,
//            VkFormat.R4G4B4A4UnormPack16 => VkFormat.B4G4R4A4UnormPack16,
//            _ => format
//        };
//    }
//}
