using System;
using D3DTX_Converter.TexconvEnums;

namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Image filter used for resizing the images.
    /// <para>Filters with DITHER in their name indicate that the 4x4 ordered dither algorithm, while DITHER_DIFFUSION is error diffusion dithering.</para>
    /// </summary>
    public class OutputFilter
    {
        public TexconvEnumFilter filter;

        public string GetArgumentOutput() => string.Format("-if {0}", Enum.GetName(typeof(TexconvEnumFilter), filter));
    }
}
