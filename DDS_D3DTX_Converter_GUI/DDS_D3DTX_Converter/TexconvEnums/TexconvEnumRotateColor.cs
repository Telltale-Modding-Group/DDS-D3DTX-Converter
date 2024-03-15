namespace D3DTX_Converter.TexconvEnums
{
    public enum TexconvEnumRotateColor
    {
        _709to2020, //Converts from Rec.709 color primaries to Rec.2020 color primaries.
        _2020to709, //Converts from Rec.2020 color primaries to Rec.709 color primaries.
        _709toHDR10, //Converts from Rec.709 color primaries to Rec.2020, normalizing nits, and applying the ST.2084 curve to create an HDR10 signal.
        _HDR10to709, //Converts an HDR10 signal back to Rec.709 color primaries with linear values.
        _P3to2020, //Converts from DCI-P3 color primaries to Rec.2020 color primaries.
        _P3toHDR10, //Converts from DCI-P3 color primaries to Rec.2020, normalizing nits, and applying the ST.2084 curve to create an HDR10 signal.
        _709toDisplayP3, //Converts from Rec.709 color primaries to Display-P3 (D65 white point)
        _DisplayP3to709 //Converts from Display-P3 (D65 white point) to Rec.709 color primaries.
    }
}
