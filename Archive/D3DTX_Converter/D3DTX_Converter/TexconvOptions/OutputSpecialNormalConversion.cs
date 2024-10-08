namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// Enables special *2 -1 conversion cases for converting unorm <-> float, and positive-only-floats <-> float/snorm.
    /// <para>These are typically used with normal maps.</para>
    /// </summary>
    public class OutputSpecialNormalConversion
    {
        public string GetArgumentOutput() => "-x2bias";
    }
}
