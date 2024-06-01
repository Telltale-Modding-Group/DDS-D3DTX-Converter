
## [Home](/wiki/home.md)

Telltale surface formats have their equivalent **DXGI format**. If you are not sure in which format to save, this is a table containing useful information.
Softwares such as Paint.NET, GIMP, NVIDIA Texture Tool or DirectX SDK (2010) have labeled their options well enough. The download links are [here](/wiki/articles/tutorial_prelude.md#what-software-do-i-need-to-edit-d3dtx). Paint.NET or GIMP will be more than enough.
More information about different types of textures can be found [here](/wiki/articles/textures.md#what-types-of-textures-usually-exist).

| Telltale Surface Format | DXGI Format | Notes 
| ----------------------- | ----------- | -----------  
| RGBA8 | R8G8B8A8_UNORM or R8G8B8A8_UNORM_SRGB[^1] | <li>Used for **diffuse** textures.</li><li>Use this save option if you do not want to lose quality.</li> | 
| ARGB8 | B8R8G8A8_UNORM or B8R8G8A8_UNORM_SRGB[^1] | <li>Used for **diffuse** textures.</li></li><li>Use this save option if you do not want to lose quality.</li> |
| BC1[^2] | BC1_UNORM or BC1_UNORM_SRGB[^1] | <li>Used for **diffuse** textures **without alpha/transparency**.</li><li>Use this save option if you want your texture compressed.</li><li>Also known as **DXT1**.</li>
| BC2[^2] | BC2_UNORM or BC2_UNORM_SRGB[^1] |<li>Used for textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT2** (pre-multiplied alpha) or **DXT3**.</li>
| BC3[^2] | BC3_UNORM or BC3_UNORM_SRGB[^1] | <li>Used for various textures **with alpha/transparency**.</li><li>Use this save option if you want your texture **compressed**.</li><li>Also known as **DXT4** (pre-multiplied alpha) or **DXT5**.</li>
| BC4[^2] | BC4_UNORM | <li>Used for ???</li><li>Also known as **ATI1**.</li><li>Use this save option if you want your texture **compressed**.</li>
| BC5[^2] | BC5_UNORM |<li>Used for **normal maps**.</li><li>Also known as **ATI2**.<li>Use this save option if you want your texture **compressed**.</li>
| BC6[^2] | BC6H_UF16 | <li>Use this save option if you want your texture **compressed**.</li>
| BC7[^2] | BC7_UNORM or BC7_UNORM_SRGB[^1] | <li>Use this save option if you want your texture **compressed**.</li>
| ARGB16 | R16B16G16A16_UNORM |  <li>Used for big detailed textures.</li><li> Only 1 such texture has been found. **(Poker Night 2 - ui_endofdemo_items.d3dtx)**</li>
| RGB565 | R5G6B5_UNORM |
| ARGB1555 | B5G5R5A1_UNORM | 
| ARGB4 | B4G4R4A4_UNORM |
| ARGB2101010 | R10G10B10A2_UNORM | Requires swizzling the red and blue channels.
| R16 | R16_UNORM | 
| RG16 | R16G16_UNORM |
| RGBA16 | R16G16B16A16_UNORM | 
| RG8 | R8G8_UNORM |  
| R32 | R32_FLOAT | 
| RG32 | R32G32_FLOAT |
| RGBA32 | R32G32B32A32_FLOAT |
| R8 | R8_UNORM |  <li>Equivalent to L8.</li>
| RGBA8S | R8G8B8A8_SNORM |
| A8 | A8_UNORM |<li>Used for detail or 1 color textures.</li>
| L8 | R8_UNORM | <li>Equivalent to R8.</li><li>Used for old lookup textures.</li>
| AL8 | R8G8_UNORM | 
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
| RGBA32F | R32G32B32A32_FLOAT | <li>Used for light maps.</li><li>Use NVIDIA Texture Tool or DirectX SDK.</li>
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

[^1]: The output depends on the D3DTX's surface gamma value. 
[^2]: BC means Block Compression, which are basically image compression algorithms. They are **lossy**.

