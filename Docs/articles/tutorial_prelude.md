## [Home](/Docs/home.md)

# How to make a Texture Mod (Prelude)

## Welcome to the tutorial on how to make a texture mod for ***any*** Telltale Game!

**This is the prelude to the tutorial before we begin. This includes detailed information regarding what software you will use and answers the most commonly asked questions.**

### PLEASE READ THE Q/A FULLY BEFORE YOU BEGIN

#### Tutorial contents:
1. **[The first part](/Docs/tutorial_part1/tutorial_part_1.md)**  is about locating and extracting the textures using Telltale Explorer. 
2. **[The second part](/Docs/tutorial_part2/tutorial_part_2.md)** is about using the Telltale Texture Tool. 
3. **[The third part](/Docs/tutorial_part3/tutorial_part_3.md)** is about editing these textures and putting them in-game.

---

### Q/A

#### What are textures?
Textures are images which are applied to models. These models include, but are not limited to characters, objects, maps, items, menus, etc.

#### What are `D3DTX` files?
These are the textures files for the Telltale Tool Engine. More details can be found [here](/Docs/articles/d3dtx.md)
I will often refer to them as either "Telltale textures" or "`D3DTX` files/textures".

#### Where are the Telltale textures files? I only find `TTARCH` files or `LENC` files in the game's directory.
They are located inside those `TTARCH` files. However, they cannot be opened with normal archive software like `7zip` or `WinRAR`. For that case we use `Telltale Explorer`.

#### What are `DDS (Direct Draw Surface)` files?
`[Direct Draw Surface](https://en.wikipedia.org/wiki/DirectDraw_Surface)` is a Microsoft file format like `PNG` or `JPEG`. However, unlike them, this format supports multiple texture layouts, surface formats and mipmaps (or mips). Additionally, it supports compressions, which reduce the size of the image by 4-6 times and are read efficiently by the GPU.

#### What is the difference between `D3DTX` and `DDS`? 
Telltale's texture files can only be read by their engine, while `DDS` is a more common format. `D3DTX` stores a lot more information than a `DDS` file.

#### How does this converter work?
The tutorial will cover most of this. The idea is:
1. You extract the pixel data from `D3DTX` to another format like `PNG` or `DDS`.
2. Edit the files.
3. Convert the files back to `D3DTX`.

`JSON` files will be generated when you convert the `D3DTX` file for the first time. They are **very** important, because they store additional useful data that is lost during the conversion. In any case, **DO NOT DELETE THEM!** 

#### What software do I need to edit `D3DTX`?
- For extracting use `[Telltale Explorer](https://quickandeasysoftware.net/software/telltale-explorer)`. It has a GUI with a built-in image viewer and filtering options. There is also `[ttarchext](http://aluigi.altervista.org/papers.htm#ttarchext)`, but it is outdated.
- For converting use the `Telltale Texture Tool`.
- For editing use `[Paint.NET](https://www.getpaint.net/)`, `[GIMP](https://www.gimp.org/downloads/)` or `Photoshop` with `[NVIDIA's Texture Tools Exporter](https://developer.nvidia.com/texture-tools-exporter)`. 
- For advanced editing (super rare cases) use the `[NVIDIA Texture Tools Exporter](https://developer.nvidia.com/texture-tools-exporter)` or the legacy `[DirectX Texture Tool](https://www.microsoft.com/en-us/download/details.aspx?id=8109)`.
- For debugging purposes use Lucas's `[Telltale Inspector](https://github.com/LucasSaragosa/TelltaleInspector)`.

#### What is the difference between `Telltale Explorer` and `Telltale Texture Tool` regarding textures?
`Telltale Explorer` can open `TTARCH` and can only export `DDS` or `PNG` images.
However, `Telltale Explorer` **does not** support all textures and they will appear broken.

Telltale Texture Tool **cannot** open `TTARCH` files, but it can preview the `D3DTX` files more accurately and it exports their `DDS` counterparts more precise.
Most importantly, importing the edited textures would be impossible without this tool.
TL:DR `Telltale Explorer` - use it to export files. `Telltale Texture Mod Tool` - use it to edit `D3DTX` files.

#### Which image editor should I choose?
Usually the answer to that question is - the one you are most comfortable with. If you do not have experience with any of them, I recommend `Paint.NET` on Windows or GIMP on Linux. Both will cover 99.9% of the cases. 
> [!NOTE]
> `Photoshop`, while it is commercial, has access to the `NVIDIA Texture Tools`.
> 
> `Paint.NET` has a lot of surface formats support, but it lacks some editing tools and some legacy surface formats.
> 
> `GIMP` has support for some uncommon surface formats which cannot be found in `Paint.NET` and it is a lot more powerful.

#### Which software will we use for the tutorials?
- `Telltale Explorer` for extracting textures.
- `Paint.NET` for editing.
- `Telltale Inspector` for debugging.

#### Which textures do I need to edit? Where do I find them?
[This page](/Docs/articles/textures.md) should answer anything regarding the textures themselves.

---
# [Click here to begin the tutorial!](/Docs/tutorial_part1/tutorial_part_1.md)