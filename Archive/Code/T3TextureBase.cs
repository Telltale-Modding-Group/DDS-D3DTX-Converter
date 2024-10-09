using TelltaleTextureTool.TelltaleEnums;

namespace TelltaleTextureTool.TelltaleFunctions
{
    public static class T3TextureBase
    {

        public static long D3D9Format_FromSurfaceFormat(T3SurfaceFormat format)
        {
            return format switch
            {
                (T3SurfaceFormat)0xBu => 42, //R32 UINT
                (T3SurfaceFormat)0xCu => 17, //RG32 UINT
                (T3SurfaceFormat)0xDu => 3, //RGBA32 UINT
                (T3SurfaceFormat)0x31u or (T3SurfaceFormat)0x33u => 77, //D24X8???
                (T3SurfaceFormat)0x34u => 75, //D24S8??? wut
                (T3SurfaceFormat)0x90u => 22, // DXGI_FORMAT_X32_TYPELESS_G8X24_UINT???
                _ => (long)0,
            };

        }
    }
}
