using System;
using D3DTX_Converter.TexconvEnums;

namespace D3DTX_Converter.TexconvOptions
{
    /// <summary>
    /// A file type for the output texture. Use one of the following. The default value is dds.
    /// <para>bmp: Windows BMP</para>
    /// <para>jpg, jpeg: Joint Photographic Experts Group</para>
    /// <para>png: Portable Network Graphics</para>
    /// <para>dds: DirectDraw Surface(Direct3D texture file format)</para>
    /// <para>tga: Truevision Graphics Adapter</para>
    /// <para>hdr: Radiance RGBE</para>
    /// <para>tif, tiff: Tagged Image File Format</para>
    /// <para>wdp, hdp, jxr: Windows Media Photo</para>
    /// <para>ppm, pfm: Portable PixMap, Portable FloatMap(Netpbm)</para>
    /// </summary>
    public class OutputFileType
    {
        public TexconvEnumFileTypes fileType;

        public string GetArgumentOutput() => string.Format("-ft {0}", Enum.GetName(typeof(TexconvEnumFileTypes), fileType));
    }
}
