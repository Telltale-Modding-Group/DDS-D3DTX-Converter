using System;

namespace D3DTX_Converter.TelltaleEnums;

// These are used as masks.
[Flags]
public enum T3SamplerStateValue
{
    eSamplerState_WrapU_Value = 0xF, // 15 
    eSamplerState_WrapV_Value = 0xF0, // 240
    eSamplerState_Filtered_Value = 0x100, // 256
    eSamplerState_BorderColor_Value = 0x1E00, // 7680
    eSamplerState_GammaCorrect_Value = 0x2000, // 8192
    eSamplerState_MipBias_Value = 0x3FC000, // 4177920
    eSamplerState_Count = 0x6
}
