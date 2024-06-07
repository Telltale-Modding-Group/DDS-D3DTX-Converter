namespace D3DTX_Converter.DirectX;

// DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
// DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
// DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
// DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
// Texture Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
// DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

/// <summary>
/// Identifies the type of resource being used.
/// </summary>
public enum D3D11_RESOURCE_DIMENSION
{
    UNKNOWN = 0,
    BUFFER = 1,

    /// <summary>
    /// Resource is a 1D texture. The dwWidth member of DDS_HEADER specifies the size of the texture. 
    /// <para>Typically, you set the dwHeight member of DDS_HEADER to 1; 
    /// you also must set the DDSD_HEIGHT flag in the dwFlags member of DDS_HEADER.</para>
    /// </summary>
    TEXTURE1D = 2,

    /// <summary>
    /// Resource is a 2D texture with an area specified by the dwWidth and dwHeight members of DDS_HEADER. 
    /// <para>You can also use this type to identify a cube-map texture. 
    /// For more information about how to identify a cube-map texture, see miscFlag and arraySize members.</para>
    /// </summary>
    TEXTURE2D = 3,

    /// <summary>
    /// Resource is a 3D texture with a volume specified by the dwWidth, dwHeight, and dwDepth members of DDS_HEADER. 
    /// <para>You also must set the DDSD_DEPTH flag in the dwFlags member of DDS_HEADER.</para>
    /// </summary>
    TEXTURE3D = 4
};
