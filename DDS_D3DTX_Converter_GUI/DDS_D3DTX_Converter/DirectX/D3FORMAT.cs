using D3DTX_Converter.Utilities;
using DirectXTexNet;

namespace D3DTX_Converter.DirectX
{

    /// <summary>
    /// This class will be used for legacy D3D9 formats. TWDS1 and previous games use D3D9 formats directly, instead of having enums.
    /// </summary>

    public enum D3DFORMAT : uint
    {
        UNKNOWN = 0,

        R8G8B8 = 20,
        A8R8G8B8 = 21,
        X8R8G8B8 = 22,
        R5G6B5 = 23,
        X1R5G5B5 = 24,
        A1R5G5B5 = 25,
        A4R4G4B4 = 26,
        R3G3B2 = 27,
        A8 = 28,
        A8R3G3B2 = 29,
        X4R4G4B4 = 30,
        A2B10G10R10 = 31,
        A8B8G8R8 = 32,
        X8B8G8R8 = 33,
        G16R16 = 34,
        A2R10G10B10 = 35,
        A16B16G16R16 = 36,

        A8P8 = 40,
        P8 = 41,

        L8 = 50,
        A8L8 = 51,
        A4L4 = 52,

        V8U8 = 60,
        L6V5U5 = 61,
        X8L8V8U8 = 62,
        Q8W8V8U8 = 63,
        V16U16 = 64,
        A2W10V10U10 = 67,

        UYVY = 0x59565955, // 'UYVY'
        R8G8_B8G8 = 0x47424752, // 'RGBG'
        YUY2 = 0x32595559, // 'YUY2'
        G8R8_G8B8 = 0x42475247, // 'GRGB'
        DXT1 = 0x31545844, // 'DXT1'
        DXT2 = 0x32545844, // 'DXT2'
        DXT3 = 0x33545844, // 'DXT3'
        DXT4 = 0x34545844, // 'DXT4'
        DXT5 = 0x35545844, // 'DXT5'

        D16_LOCKABLE = 70,
        D32 = 71,
        D15S1 = 73,
        D24S8 = 75,
        D24X8 = 77,
        D24X4S4 = 79,
        D16 = 80,

        D32F_LOCKABLE = 82,
        D24FS8 = 83,


        D32_LOCKABLE = 84,
        S8_LOCKABLE = 85,


        L16 = 81,

        VERTEXDATA = 100,
        INDEX16 = 101,
        INDEX32 = 102,

        Q16W16V16U16 = 110,

        MULTI2_ARGB8 = 0x3145544D, // 'MET1'

        R16F = 111,
        G16R16F = 112,
        A16B16G16R16F = 113,

        R32F = 114,
        G32R32F = 115,
        A32B32G32R32F = 116,

        CxV8U8 = 117,

        A1 = 118,
        A2B10G10R10_XR_BIAS = 119,
        BINARYBUFFER = 199,

        AI44 = 0x34344941, // 'AI44'
        IA44 = 0x34344149, // 'IA44'
        YV12 = 0x32315659, // 'YV12'

        FORCE_DWORD = 0x7fffffff
    }

    public class D3D9FormatConverter
    {
        public static DXGI_FORMAT GetFromD3D9ToDXGIFormat(uint value)
        {
            switch (value)
            {
                case 83: return DXGI_FORMAT.D24_UNORM_S8_UINT;
                case 82: return DXGI_FORMAT.D32_FLOAT;
                case 79: return DXGI_FORMAT.D24_UNORM_S8_UINT;
                case 77: return DXGI_FORMAT.D24_UNORM_S8_UINT;
                case 81: return DXGI_FORMAT.R16_UNORM;
                case 80: return DXGI_FORMAT.D16_UNORM;
                case 75: return DXGI_FORMAT.D24_UNORM_S8_UINT;
                case 73: return DXGI_FORMAT.D16_UNORM;
                case 71: return DXGI_FORMAT.D32_FLOAT;
                case 70: return DXGI_FORMAT.D16_UNORM;
                case 1111970375: return DXGI_FORMAT.R8G8_B8G8_UNORM;
                case 1195525970: return DXGI_FORMAT.G8R8_G8B8_UNORM;
                //  case 117: return DXGI_FORMAT.CxV8U8_UNORM;
                case 116: return DXGI_FORMAT.R32G32B32A32_FLOAT;
                case 115: return DXGI_FORMAT.R32G32_FLOAT;
                case 114: return DXGI_FORMAT.R32_FLOAT;
                case 113: return DXGI_FORMAT.R16G16B16A16_FLOAT;
                case 112: return DXGI_FORMAT.R16G16_FLOAT;
                case 111: return DXGI_FORMAT.R16_FLOAT;
                case 827606349: return DXGI_FORMAT.R8G8B8A8_UNORM; // Assuming this is what was meant by "Multi2Argb8"
                case 110: return DXGI_FORMAT.R16G16B16A16_SNORM;
                case 100: return DXGI_FORMAT.UNKNOWN; // Assuming this is what was meant by "VertexData", as it's not a DXGI format
                case 894720068: return DXGI_FORMAT.BC3_UNORM; // Assuming Dxt5
                case 877942852: return DXGI_FORMAT.BC2_UNORM; // Assuming Dxt4
                case 861165636: return DXGI_FORMAT.BC2_UNORM; // Assuming Dxt3
                case 844388420: return DXGI_FORMAT.BC2_UNORM; // Assuming Dxt2
                case 827611204: return DXGI_FORMAT.BC1_UNORM; // Assuming Dxt1
                case 844715353: return DXGI_FORMAT.YUY2;
                //   case 1498831189: return DXGI_FORMAT.UYVY;
                case 67: return DXGI_FORMAT.R10G10B10A2_UNORM;
                case 64: return DXGI_FORMAT.R16G16_UNORM;
                case 63: return DXGI_FORMAT.R8G8B8A8_UNORM;
                case 62: return DXGI_FORMAT.R8G8B8A8_UNORM;
                case 61: return DXGI_FORMAT.R8G8B8A8_UNORM;
                case 60: return DXGI_FORMAT.R8G8_UNORM;
                //   case 52: return DXGI_FORMAT.R4G4_UNORM_PACK8;
                case 51: return DXGI_FORMAT.R8G8_UNORM;
                case 50: return DXGI_FORMAT.R8_UNORM;
                case 41: return DXGI_FORMAT.R8_UINT;
                case 40: return DXGI_FORMAT.R8G8_UINT;
                case 36: return DXGI_FORMAT.R16G16B16A16_UNORM;
                //   case 35: return DXGI_FORMAT.A2R10G10B10_UNORM_PACK32;
                case 34: return DXGI_FORMAT.R16G16_UNORM;
                case 33: return DXGI_FORMAT.B8G8R8X8_UNORM; // Assuming X8B8G8R8
                case 32: return DXGI_FORMAT.R8G8B8A8_UNORM;
                case 31: return DXGI_FORMAT.R10G10B10A2_UNORM;
                //    case 30: return DXGI_FORMAT.B4G4R4A4_UNORM_PACK16;
                //   case 29: return DXGI_FORMAT.A8R3G3B2_UNORM_PACK8;
                case 28: return DXGI_FORMAT.A8_UNORM;
                //   case 27: return DXGI_FORMAT.R3G3B2_UNORM;
                //   case 26: return DXGI_FORMAT.B4G4R4A4_UNORM_PACK16;
                //   case 25: return DXGI_FORMAT.B5G5R5A1_UNORM_PACK16;
                //    case 24: return DXGI_FORMAT.B5G5R5A1_UNORM_PACK16;
                case 23: return DXGI_FORMAT.B5G6R5_UNORM;
                case 22: return DXGI_FORMAT.B8G8R8X8_UNORM;
                case 21: return DXGI_FORMAT.B8G8R8A8_UNORM;
                case 20: return DXGI_FORMAT.B8G8R8X8_UNORM; // Assuming R8G8B8
                default: return DXGI_FORMAT.UNKNOWN;
            }
        }
    }



}