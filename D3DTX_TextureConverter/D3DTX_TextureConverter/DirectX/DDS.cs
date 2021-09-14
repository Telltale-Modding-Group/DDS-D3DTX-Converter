using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3DTX_TextureConverter.DirectX
{
    //DDS Docs - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-header
    //DDS PIXEL FORMAT - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-pixelformat
    //DDS File Layout https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dds-file-layout-for-textures
    //Texutre Block Compression in D3D11 - https://docs.microsoft.com/en-us/windows/win32/direct3d11/texture-block-compression-in-direct3d-11
    //DDS Programming Guide - https://docs.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds-pguide

    public struct DDS_HEADER
    {
        public uint dwSize; //Size of structure. This member must be set to 124. [4 bytes]
        public uint dwFlags; //Flags to indicate which members contain valid data. [4 bytes]
        public uint dwHeight;  //Surface height (in pixels). [4 bytes]
        public uint dwWidth; //Surface width (in pixels). [4 bytes]
        public uint dwPitchOrLinearSize; //The pitch or number of bytes per scan line in an uncompressed texture; the total number of bytes in the top level texture for a compressed texture. [4 bytes]
        public uint dwDepth; //Depth of a volume texture (in pixels), otherwise unused. [4 bytes]
        public uint dwMipMapCount; //Number of mipmap levels, otherwise unused. [4 bytes]
        public uint dwReserved1; //unused according to DDS docs [4 bytes]
        public uint dwReserved2; //unused according to DDS docs [4 bytes]
        public uint dwReserved3; //unused according to DDS docs [4 bytes]
        public uint dwReserved4; //unused according to DDS docs [4 bytes]
        public uint dwReserved5; //unused according to DDS docs [4 bytes]
        public uint dwReserved6; //unused according to DDS docs [4 bytes]
        public uint dwReserved7; //unused according to DDS docs [4 bytes]
        public uint dwReserved8; //unused according to DDS docs [4 bytes]
        public uint dwReserved9; //unused according to DDS docs [4 bytes]
        public uint dwReserved10; //unused according to DDS docs [4 bytes]
        public uint dwReserved11; //unused according to DDS docs [4 bytes]
        public DDS_PIXELFORMAT ddspf; //main surface pixel format
        public uint dwCaps; //Specifies the complexity of the surfaces stored. [4 bytes]
        public uint dwCaps2; //Additional detail about the surfaces stored. [4 bytes]
        public uint dwCaps3; //unused according to DDS docs [4 bytes]
        public uint dwCaps4; //unused according to DDS docs [4 bytes]
        public uint dwReserved12; //unused according to DDS docs [4 bytes]
    }

    public struct DDS_PIXELFORMAT
    {
        public uint dwSize; //Structure size; set to 32 (bytes). [4 bytes]
        public uint dwFlags; //Values which indicate what type of data is in the surface. [4 bytes]
        public uint dwFourCC; //Four-character codes for specifying compressed or custom formats. [4 bytes all together]
        public uint dwRGBBitCount; //Number of bits in an RGB (possibly including alpha) format. [4 bytes]
        public uint dwRBitMask; //Red (or lumiannce or Y) mask for reading color data. [4 bytes]
        public uint dwGBitMask; //Green (or U) mask for reading color data. [4 bytes]
        public uint dwBBitMask; //Blue (or V) mask for reading color data. [4 bytes]
        public uint dwABitMask; //Alpha mask for reading alpha data. [4 bytes]
    }
}
