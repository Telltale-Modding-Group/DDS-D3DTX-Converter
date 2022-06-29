using System;

namespace D3DTX_TextureConverter.TelltaleEnums
{
    [Flags]
    public enum T3SamplerStateValue
    {
        eSamplerState_WrapU_Value,
        eSamplerState_WrapV_Value,
        eSamplerState_Filtered_Value,
        eSamplerState_BorderColor_Value,
        eSamplerState_GammaCorrect_Value,
        eSamplerState_MipBias_Value,
        eSamplerState_Count
    }
}
