namespace D3DTX_Converter.TelltaleEnums
{
    // note: can't find original name, so just using the prefix eTxColor
    public enum eTxColor
    {
        eTxColorUnknown = -1, // Used for compressed textures, like DXT1, DXT5, etc. 
        eTxColorFull = 0, //Presumably used for uncompressed formats which have 4 color channels (RGBA). But those with RGB - no idea. Needs further testing
        eTxColorBlack = 1, // Used with A8 surface format (TWDS2), presumably used for images with a lot of black, like shadows, hair, etc. Why is it not Grayscale? NO IDEA
        eTxColorGrayscale = 2,
        eTxColorGrayscaleAlpha = 3, // I would presume it's used for AL8 and potentially other 2 channel formats
    }
}
