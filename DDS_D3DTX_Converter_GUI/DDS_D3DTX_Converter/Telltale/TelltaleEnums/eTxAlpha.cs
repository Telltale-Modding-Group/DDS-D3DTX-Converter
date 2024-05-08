namespace D3DTX_Converter.TelltaleEnums
{
    //note: can't find original name, so just using the prefix eTxAlpha
    //Update: Original name could be eTxAlphaMode
    public enum eTxAlpha
    {
        eTxAlphaUnknown = -1, // Presumably used for textures, where alpha doesn't matter (UI menu, DXT1, DXT5, maybe more)
        eTxNoAlpha = 0, // Presumably used for textures, which are known to not have alpha (DXT1, uncompressed formats which don't use an alpha channel)
        eTxAlphaTest = 1,
        eTxAlphaBlend = 2,
    }
}
