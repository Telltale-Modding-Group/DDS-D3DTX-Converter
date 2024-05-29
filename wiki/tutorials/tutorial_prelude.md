# How to make a Texture Mod (Prelude)

**Welcome to the tutorial on how to make a texture mod for ***any*** Telltale Game!**

**This is the prelude to the tutorial before we begin. This includes information regarding what software you will use and answers the most common questions. It's a lot to read but it's not hard, trust me. I just detailed a lot of information in here just to answer and hopefully answer any common questions one might have.**

### PLEASE READ THE Q/A FULLY

This **[first part](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))** of the tutorial is for locating and extracting the textures using Telltale Explorer. 

This **[second part](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))** of the tutorial is for using the application converting the textures. 

The **[third part](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))** of this tutorial is editing these textures.

This **[fourth part](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))** of the tutorial is for using the application and extracting/editing/converting the textures. 

The main steps are: 
1. Extract
2. Convert
3. Edit
4. Convert
5. Archive (Optional)

### Q/A

#### What are textures?
Textures are images which are applied to models. These models include, but are not limited to characters, objects, maps, items, menus, etc.

#### What are D3DTX files?
These are file formats in which Telltale store their textures in. It's propriety, meaning it cannot be opened using any other software other than Telltale-related ones like Telltale Explorer or Telltale Texture Mod Tool. They are very customized and differ from game to game. More information can be found here.
I will often use "Telltale textures" as a synonym.

#### Where are the Telltale textures files? I only find TTARCH files or LENC files in the game's directory.
They are located inside those TTARCH archives. However, they cannot be opened with normal archive software like 7zip or WinRAR. For that case we use Telltale Explorer.

#### What are DDS (Direct Draw Surface) files?
Direct Draw Surface is a Microsoft file format like PNG or JPEG. However, unlike them, this format supports multiple texture layouts, surface formats and mipmaps (or mips).

#### What is the difference between D3DTX and DDS? 
Telltale's texture files can only be read by their engine, while DDS is a more common format among games. D3DTX stores a lot more information than a DDS file.

#### How does this converter work?
When we convert the **extracted D3DTX** files, we generate their DDS analogues, which can be opened in image editing software. 
Since D3DTX files store additional information that other files can't store including DDS, we need to find a way to preserve that data. For that case we automatically generate a **JSON** file. It's **very** important, because it's used in all conversion processes. Without it, the textures won't be converted back.

#### What software do I need to edit D3DTX?
- For extracting use **[Telltale Explorer](https://quickandeasysoftware.net/software/telltale-explorer)**. It has a GUI with an in-built image viewer and filtering options. There's also **[ttarchext](http://aluigi.altervista.org/papers.htm#ttarchext)**, but it's outdated.
- For converting use the Telltale Texture Mod Tool.
- For editing (DDS) use **[Paint.NET](https://www.getpaint.net/)**, **[GIMP](https://www.gimp.org/downloads/)** or **Photoshop** with **[NVIDIA's Texture Tools Exporter](https://developer.nvidia.com/texture-tools-exporter)**. 
- For advanced editing (super rare cases) use the **[NVIDIA Texture Tools Exporter](https://developer.nvidia.com/texture-tools-exporter)** or the legacy **[DirectX Texture Tool](https://www.microsoft.com/en-us/download/details.aspx?id=8109)**.
- For debugging purposes use Lucas's **[Telltale Inspector](https://github.com/LucasSaragosa/TelltaleInspector)**.
***TL:DR*** There are lot of tools, but the most important ones are Telltale Explorer, an image editing software which supports DDS and Telltale Texture Mod Tool.

#### I have heard that Telltale Explorer can export the texture files as DDS. What's wrong with it and why can't I use it?
Importing the edited textures would be impossible. Furthermore, some surface formats are exported incorrectly.

#### Which image editor should I choose?
- Usually the answer to that question is - the one you are most comfortable with. If you don't have experience with any of them, I recommend Paint.NET on Windows or GIMP on Linux. Both will cover 99.9% of the cases. For this tutorial we will use Paint.NET as it's very simple to use.
- Photoshop is paid, but it has access to the NVIDIA texture tool.

##### For advanced users:
- Paint.NET has a lot of surface formats support, but it lacks some editing tools and some legacy surface formats.
- GIMP has custom mipmaps options (you can edit the amount of mipmaps you need), support for some uncommon surface formats which cannot be found in Paint.NET and it's a lot more powerful.

**It's also worth mentioning that you need to be mindful of the textures you select.** Typically *(unless you are an experienced texture artist)* you would only really need to be modifying the **[diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)**. 

**[Diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)** are just the raw material/model colors and these textures typically have no suffix after them in their file names like _spec, _ink, _detail, _nm, etc. **For example, sk54_lee_head.d3dtx is a diffuse texture, however sk54_lee_head_spec.d3dtx is a specular texture.** These suffixes indicate and deal with other material properties like the **[specularity](https://www.modding-forum.com/guide/17-diffuse-specular-and-normal-maps/)** of a material, the **[normal/bump map](https://en.wikipedia.org/wiki/Normal_mapping)** of the material, or the ink/black lines on a material.

**In addition to extraction**, occasionally you may not find the texture you are looking for in the episode _txmesh archive. In the case of the definitive edition, the other place to look for these textures would be the main project archives. For season 1 for example, most of the common textures used are stored in ProjectSeason1_txmesh.ttarch2 archive.

**It's also recommended that you have an image editor ready to use.** 

If you are using **[Photoshop](https://www.adobe.com/products/photoshop.html)** *(which I use)*, you need to install **[this DDS plugin](https://software.intel.com/content/www/us/en/develop/articles/intel-texture-works-plugin.html)** to read/edit/save .dds textures. 

If you don't have Photoshop you can use  which is a **free** image editing software with DDS support. 

I don't recommend GIMP as the built-in DDS plugin doesn't support the newer DDS files that this converter spits out and seems to break very easily. **[Paint.NET](https://www.getpaint.net/)** is a great alternative.


### Critical Note
Once again it's very important to mention that a lot of **D3DTX** textures have **[Mip Maps](http://archive.gamedev.net/archive/reference/programming/features/dxmipmap/mipmaps.gif)**, mipmaps are basically lower resolution textures that are embedded in the file and are used at a distance in the game engine to prevent **[aliasing and artifacts](https://gdbooks.gitbooks.io/legacyopengl/content/Chapter7/mip1.png)**. 

The **Compression Type** format is also something that you have to keep in mind when modifying/saving these textures. 

The formats must be the same and cannot change. 

**The Texture Mod Tool in the Image Properties section reveals both information about the Compression Type format, and the Mip Maps to the user when extracting/converting the textures using this tool so you'll know when a specific texture needs to be in a specific format or requires mipmaps.**

# [Click here to Proceed to Part 1 of Making a Texture Mod](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))


### [You can skip to Part 1 here if want to ignore all of this](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1)) however there are some important things to keep in mind that this prelude goes over.