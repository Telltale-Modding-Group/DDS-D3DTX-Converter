using System;
using D3DTX_Converter.TexconvEnums;

namespace D3DTX_Converter.TexconvOptions;

/// <summary>
/// Use sRGB if both the input and output data are in the sRGB color format (ie. gamma ~2.2).
/// <para>Use sRGBi if only the input is in sRGB; use sRGBo if only the output is in sRGB.</para>
/// </summary>
public class OutputSRGB
{
    public TexconvEnumSrgb srgbMode;

    public string GetArgumentOutput() => string.Format("-{0}", Enum.GetName(typeof(TexconvEnumSrgb), srgbMode));
}
