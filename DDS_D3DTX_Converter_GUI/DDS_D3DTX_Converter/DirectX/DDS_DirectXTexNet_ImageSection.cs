using DirectXTexNet;

namespace D3DTX_Converter.DirectX;

/// <summary>
/// Image section of a DDS file. Contains width, height, format, slice pitch and row pitch.
/// </summary>
public struct DDS_DirectXTexNet_ImageSection
{
    public long Width;
    public long Height;
    public DXGI_FORMAT Format;
    public long SlicePitch;
    public long RowPitch;
};
