using D3DTX_Converter.TelltaleEnums;

namespace D3DTX_Converter.TelltaleFunctions
{
    public static class T3TextureBase
    {

        public static long D3D9Format_FromSurfaceFormat(T3SurfaceFormat format)
        {
            return format switch
            {
                (T3SurfaceFormat)0xBu => 42,
                (T3SurfaceFormat)0xCu => 17,
                (T3SurfaceFormat)0xDu => 3,
                (T3SurfaceFormat)0x31u or (T3SurfaceFormat)0x33u => 77,
                (T3SurfaceFormat)0x34u => 75,
                (T3SurfaceFormat)0x90u => 22,
                _ => (long)0,
            };

        }
    }
}
