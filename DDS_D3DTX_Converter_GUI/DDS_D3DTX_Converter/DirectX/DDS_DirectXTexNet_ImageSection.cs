namespace D3DTX_Converter.DirectX;

/// <summary>
/// Image section of a DDS file. Contains width, height, format, slice pitch and row pitch.
/// </summary>
public struct DDS_DirectXTexNet_ImageSection
{
    public nuint Width;
    public nuint Height;
    public DXGIFormat Format;
    public nuint SlicePitch;
    public nuint RowPitch;
    public byte[] Pixels;
};
