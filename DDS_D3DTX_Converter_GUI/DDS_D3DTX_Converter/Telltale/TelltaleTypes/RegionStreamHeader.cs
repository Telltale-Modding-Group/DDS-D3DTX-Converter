namespace D3DTX_Converter.TelltaleTypes;

public class RegionStreamHeader
{
    // Exists from mVersion 5
    public int mFaceIndex { get; set; }
    // Exists from mVersion 3
    public int mMipIndex { get; set; }
    // Exists from mVersion 4
    public int mMipCount { get; set; }
    // Exists from mVersion 3
    public uint mDataSize { get; set; }
    // Exists from mVersion 3
    public int mPitch { get; set; }
    // Exists from mVersion 9
    public int mSlicePitch { get; set; }
}
