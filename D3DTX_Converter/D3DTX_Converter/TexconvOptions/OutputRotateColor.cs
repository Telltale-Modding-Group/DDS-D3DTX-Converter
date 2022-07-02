using System;
using D3DTX_Converter.TexconvEnums;

namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Performs color rotations and/or ST.2084 curves for converting image data to/from HDR10 signals.
    /// </summary>
    public class OutputRotateColor
    {
        public TexconvEnumRotateColor colorspace;

        public string GetArgumentOutput()
        {
            string enumString = Enum.GetName(typeof(TexconvEnumFileTypes), colorspace);
            string cleanedString = enumString.Remove(0, 1); //remove _

            return string.Format("-rotatecolor {0}", cleanedString);
        }
    }
}
