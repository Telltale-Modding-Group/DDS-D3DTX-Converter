## [Home](/Docs/home.md)

This page contains information regarding Telltale textures only.

`D3DTX` is a proprietary file container format used by the Telltale Tool game engine, storing texture data for their games. Initial beliefs were that Telltale used `DDS` in all of their games, but after some careful searching that would appear to be false.

`D3DTX` is the result of importing texture data from various other file formats, like `DDS`, `KTX`, `PNG`, `PVR` and more. The older (`pre-Poker Night 2`) `D3DTX` versions hold all original data from their containers, while later versions extract only the pixel data. The latter would be a useful timesaving feature for artists and developers, who do not want to convert back and forth between files.

Since `D3DTX` can have texture data from multiple file formats, this technically means it is cross-platform, as long as the original pixel data format is supported on the **hardware**.

Binary representation (in order of the fields):

After `Poker Night 2` D3DTX 
- Usually MSV5 or MSV6  metaheaders (with the exception of Poker Night 2 being MTRE).
- Stores some filtering options used by meshes.
- Stores the platform which is made for (unreliable field, does not affect the texture in-game).
- Stores the name of the file, which is referenced through Lua.
- Can store the original file name which was used for importing.
- Stores width, height, depth and array size.
- Stores the mip count.
- Stores the image format.
- Stores the texture layout.
- Stores some color swizzling data (does not affect the texture in-game, only when it was being imported).
- Can disable some mips. (Lazy hack used in console editions.)
- Stores some Engine related data.
- Can store the original texture data. (Used only twice in Poker Night 2.)

Pre-Poker Night 2 D3DTX 
- Usually MBIN or MTRE metaheaders.
- Stores some filtering options used by meshes.
- Stores the name of the file, which is referenced through Lua.
- Can store the original file name which was used for importing.
- Stores width, height.
- Stores the mip count.
- Stores the ORIGINAL image format value. (For example, `DXT1` is shown as `DXT1` instead of a number.)
- Can store multiple texture types.
- Stores some Engine related data.
- Stores whole containers inside the D3DTX.
