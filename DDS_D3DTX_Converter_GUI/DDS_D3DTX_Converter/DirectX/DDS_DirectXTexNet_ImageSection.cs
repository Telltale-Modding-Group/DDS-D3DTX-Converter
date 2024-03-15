using DirectXTexNet;

namespace D3DTX_Converter.DirectX
{
    public struct DDS_DirectXTexNet_ImageSection
    {
        public long Width;
        public long Height;
        public DXGI_FORMAT Format;
        public long SlicePitch;
        public long RowPitch;
    }
}
