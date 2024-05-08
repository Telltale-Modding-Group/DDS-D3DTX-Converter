using D3DTX_Converter.TelltaleEnums;

namespace D3DTX_Converter.TelltaleTypes
{
    public class EnumPlatformType
    {
        public static PlatformType GetPlatformType(int value)
        {
            switch (value)
            {
                default: return PlatformType.ePlatform_All;
                case 0: return PlatformType.ePlatform_None;
                case 1: return PlatformType.ePlatform_All;
                case 2: return PlatformType.ePlatform_PC;
                case 3: return PlatformType.ePlatform_Wii;
                case 4: return PlatformType.ePlatform_Xbox;
                case 5: return PlatformType.ePlatform_PS3;
                case 6: return PlatformType.ePlatform_Mac;
                case 7: return PlatformType.ePlatform_iPhone;
                case 8: return PlatformType.ePlatform_Android;
                case 9: return PlatformType.ePlatform_Vita;
                case 10: return PlatformType.ePlatform_Linux;
                case 11: return PlatformType.ePlatform_PS4;
                case 12: return PlatformType.ePlatform_XBOne;
                case 13: return PlatformType.ePlatform_WiiU;
                case 14: return PlatformType.ePlatform_Win10;
                case 15: return PlatformType.ePlatform_NX;
                case 16: return PlatformType.ePlatform_Count;
            }
        }
    }
}