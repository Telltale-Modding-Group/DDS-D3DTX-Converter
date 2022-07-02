using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_Converter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS DDS_HEADER_DXT10 - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header-dxt10
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    /// <summary>
    /// Surface pixel format.
    /// </summary>
    public struct DDS_PIXELFORMAT
    {
        /// <summary>
        /// [4 bytes] Structure size; set to 32 (bytes).
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwSize;

        /// <summary>
        /// [4 bytes] Values which indicate what type of data is in the surface.
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFlags;

        /// <summary>
        /// [4 bytes] Four-character codes for specifying compressed or custom formats.
        /// <para>Possible values include: DXT1, DXT2, DXT3, DXT4, or DXT5.</para>
        /// <para>A FourCC of DX10 indicates the prescense of the DDS_HEADER_DXT10 extended header, and the dxgiFormat member of that structure indicates the true format.</para>
        /// <para>When using a four-character code, dwFlags must include DDPF_FOURCC.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwFourCC;

        /// <summary>
        /// [4 bytes] Number of bits in an RGB (possibly including alpha) format.
        /// <para>Valid when dwFlags includes DDPF_RGB, DDPF_LUMINANCE, or DDPF_YUV.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwRGBBitCount;

        /// <summary>
        /// [4 bytes] Red (or lumiannce or Y) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the red mask would be 0x00ff0000.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwRBitMask;

        /// <summary>
        /// [4 bytes] Green (or U) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the green mask would be 0x0000ff00.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwGBitMask;

        /// <summary>
        /// [4 bytes] Blue (or V) mask for reading color data.
        /// <para>For instance, given the A8R8G8B8 format, the blue mask would be 0x000000ff.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwBBitMask;

        /// <summary>
        /// [4 bytes] Alpha mask for reading alpha data.
        /// <para>dwFlags must include DDPF_ALPHAPIXELS or DDPF_ALPHA. For instance, given the A8R8G8B8 format, the alpha mask would be 0xff000000.</para>
        /// <para>Is a DWORD, which is a 32-bit unsigned integer.</para>
        /// </summary>
        public uint dwABitMask;
    }
}
