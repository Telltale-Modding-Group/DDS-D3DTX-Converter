using DirectXTexNet;

namespace D3DTX_Converter.DirectX
{

    /// <summary>
    /// This class will be used for legacy D3D9 formats. TWDS1 and previous games use D3D9 formats directly, instead of having enums.
    /// </summary>
    public class D3D9
    {
        public static DXGI_FORMAT GetDXGIFormat(int value)
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

    // Define DXGI_FORMAT enumeration

}