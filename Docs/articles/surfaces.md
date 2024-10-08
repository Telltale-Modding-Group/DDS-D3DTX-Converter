
## [Home](/Docs/home.md)

Many people know that images are usually stored in RGB or RGBA format. While this is true, the pixel data can take in a lot of different forms for a lot of reasons, the main 2 being: to reduce the amount resources (compression) or the program requiring more precise calculations (HDR).

Telltale heavily compressed their textures for a good reason. D3DTX is a container just like DDS or KTX, but it supports multiple platforms.

Telltale surface formats have their equivalent **OpenGL formats**. If you are not sure in which format to save, this is a table containing useful information.
Softwares such as Paint.NET, GIMP, NVIDIA Texture Tool or DirectX SDK (2010) have labeled their options well enough. The download links are [here](/Docs/articles/tutorial_prelude.md#what-software-do-i-need-to-edit-d3dtx). Paint.NET or GIMP will be more than enough.
More information about different types of textures can be found [here](/Docs/articles/textures.md#what-types-of-textures-usually-exist).

| Telltale Surface Format | DXGI Format | Notes 
| ----------------------- | ----------- | -----------  
| RGBA8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] | <ul><li>Used for **diffuse** textures.</li><li>Use this save option if you do not want to lose quality.</li></ul> | 
| ARGB8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] | <ul><li>Used for **diffuse** textures.</li></li><li>Use this save option if you do not want to lose quality.</li></ul> |
| BC1[^2] | BC1_UNORM or BC1_UNORM_SRGB[^1] |<ul><li>Used for **diffuse** textures **without alpha/transparency**.</li><li>Use this save option if you want your texture compressed.</li><li>Also known as **DXT1**.</li></ul>
| BC2[^2] | BC2_UNORM or BC2_UNORM_SRGB[^1] |<ul><li>Used for textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT2** (pre-multiplied alpha) or **DXT3**.</li></ul>
| BC3[^2] | BC3_UNORM or BC3_UNORM_SRGB[^1] |<ul><li>Used for various textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT4** (pre-multiplied alpha) or **DXT5**.</li></ul>
| BC4[^2] | BC4_UNORM | <ul><li>Used for ???</li><li>Also known as **ATI1**.</li><li>Use this save option if you want your texture **compressed**.</li></ul>
| BC5[^2] | BC5_UNORM |<ul><li>Used for **normal maps**.</li><li>Also known as **ATI2**.<li>Use this save option if you want your texture **compressed**.</li></ul>
| BC6[^2] | BC6H_UF16 | <ul><li>Use this save option if you want your texture **compressed**.</li></ul>
| BC7[^2] | BC7_UNORM or BC7_UNORM_SRGB[^1] | <ul><li>Use this save option if you want your texture **compressed**.</li></ul>
| ARGB16 | R16B16G16A16_UNORM |  <ul><li>Used for big detailed textures.</li><li> Only 1 such texture has been found. **(Poker Night 2 - ui_endofdemo_items.d3dtx)**</li></ul>
| RGB565 | R5G6B5_UNORM | <ul><li>Used for **diffuse** textures.</li></ul>
| ARGB1555 | B5G5R5A1_UNORM | 
| ARGB4 | R4G4B4A4_UNORM | <ul><li>Used for **diffuse** textures.</li></ul>
| ARGB2101010 | R10G10B10A2_UNORM | Requires swizzling the red and blue channels.
| R16 | R16_UNORM | 
| RG16 | R16G16_UNORM |
| RGBA16 | R16G16B16A16_UNORM | 
| RG8 | R8G8_UNORM |  
| R32 | R32_FLOAT | 
| RG32 | R32G32_FLOAT |
| RGBA32 | R32G32B32A32_FLOAT |
| R8 | R8_UNORM |  <ul><li>Equivalent to L8.</li></ul>
| RGBA8S | R8G8B8A8_SNORM |
| A8 | A8_UNORM |<ul><li>Used for detail or 1 color textures.</li></ul>
| L8 | R8_UNORM | <ul><li>Equivalent to R8.</li><li>Used for old lookup textures.</li></ul>
| AL8 | R8G8_UNORM | Used for look ups.
| L16 | R16_UNORM | Use DirectX SDK (2010 version).
| RG16S | R16G16_SNORM | 
| RGBA16S | R16G16B16A16_SNORM | 
| R16UI | R16_UINT|
| RG16UI | R16G16 _UINT |
| R16F | R16_FLOAT |
| RG16F | R16G16_FLOAT | 
| RGBA16F | R16G16B16A16_FLOAT | 
| R32F | R32_FLOAT | 
| RG32F | R32G32_FLOAT |  |
| RGBA32F | R32G32B32A32_FLOAT | <ul><li>Used for light maps.</li><li>Use NVIDIA Texture Tool or DirectX SDK.</li></ul>
| [Any Format]_UNORM | [Any Format]_UNORM | You will find these formats in legacy Telltale titles.

> Disclaimer: There are other existing Telltale formats, but they **cannot** be found on PC platforms.

> Disclaimer: DDS editors **do not** have all of the save options presented above. Use **DirectX SDK (2010)** or **NVIDIA Texture Tool** if you want a more specific format.

| Telltale Surface Gamma | DXGI Format |
| --- | --- | 
| Non-existent | Non-SRGB (Linear) |
| Unknown | Non-SRGB (Linear) |
| Linear | Non-SRGB (Linear) | 
| sRGB | SRGB |
> Note: Non-existent means that there is no such field in the relative D3DTX version.

### Reading Debug Information

#### D3DTX

A typical D3DTX debug window will look like this. It has 2 parts:
- **[Meta Header](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleMeta/)** which contains information which the engine reads which has 3 versions.
- **[D3DTX Header](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleD3DTX/)** which contains texture information.

There are many [enums](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleEnums/) and [structures](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleTypes/), which are displayed on the window.

A single game will use **only 1 meta header version** and **only 1 D3DTX version**. They can never mix.
Legacy Versions **do not** have mVersions. You have to use the combobox to select the correct version for the specific game.

I would recommend checking out the different headers and their classes in the code - they are written in detail.

For simple debugging purposes we will focus only on few things:
We will verify the following fields which are the most important ones:
- mWidth
- mHeight
- mDepth
- mArraySize 
- mSurfaceFormat
- mNumMipLevels
- mGammaSurface 
- mTextureLayout 
- mType ([the texture type](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleEnums/T3TextureType.cs))
- mRegionCount

For advanced users, verify these ones:
- Meta Default Section Chunk Size
- Meta Async Section Chunk Size
- mSamplerState
- mNormalMapFormat
- mHDRLightmapScale
- mSwizzle
- mAlphaMode
- mColorMode
- mRegionHeaders
- mAuxDataCount (if you encounter any textures with AUX data, **please open an issue**).

> mRegionHeaders are basically each section of the image - mipmaps, faces, slices. I have few examples [here](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Main/DDS_Master.cs#L348).

![debug1](/Docs/application_guide/ui_8.png)


#### DDS
The debug information of a DDS image is very simple. The most important fields are written in **bold**.
- **Width** - The width of the base image.
- **Height** - The height of the base image.
- Depth - 
- **Format** - The DXGI format of the image.
- **Mips** - The number of mipmaps of per image.
- **Dimension** - The texture layout of the image.
- Array Elements - The number of distinct images.
- Volumemap - If the Dimension is TEXTURE3D or the Depth is higher than 1, it is considered volumemap.
- Cubemap - If the texture is cubemap.
- Alpha mode - ignore.
- Premultiplied alpha - only valid for DXT2 or DXT4 compressions (BC2 and BC3 respectively).
- Misc Flags - ignore.
- Misc Flags2 - ignore. 

![debug1](/Docs/application_guide/ui_9.png)

[^1]: The output depends on the D3DTX's surface gamma value. 
[^2]: BC means Block Compression, which are basically image compression algorithms. They are **lossy**.

