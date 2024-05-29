## [Home](/wiki/home.md)

This page contains information regarding textures.

#### How do I find the textures?
Use Telltale Explorer. [This tutorial explains how to use it.](/wiki/tutorials/tutorial_part_1.md)

#### Where are the textures stored?
I mentioned they are stored in TTARCH files, but you may have seen there are *a lot*. Let's see how to recognize an archive's purpose.
The first part of the TTARCH's name describes where it's used. 
- **Boot** (Used for booting the game)
- **Menu** (Used for game's ui)
- **Project** (Used for game's ui)
- **[GameName][3-digit number]** (Used for episodes)

Usually the ones you are looking for end in the following ways:
- **_txmesh** (~90% of the textures are stored there)
- **_compressed** (~5% of the textures are stored there)
- **_all** (~5% of the textures are stored there)
> Example: Most of the common textures for TWDS1 in TWD:DE are stored in ProjectSeason1_txmesh.ttarch2 archive.

#### Which textures do I need to edit?
This question is a complicated one and it depends on what the user wants. Let's answer some additional questions first.

##### What types of textures usually exist?
There are a lot of types of textures. [This article](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm) explains the main ones. Typically *(unless you are an experienced texture artist)* you would only really need to be modifying the diffuse textures. 

###### What are diffuse textures?
**[Diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)** are just the raw material/model colors and these textures typically have no suffix after them in their file names like _spec, _ink, _detail, etc. These suffixes indicate and deal with other material properties like the **[specularity](https://www.modding-forum.com/guide/17-diffuse-specular-and-normal-maps/)** of a material, the **[normal/bump map](https://en.wikipedia.org/wiki/Normal_mapping)** of the material, or the ink/black lines on a material.
> Sometimes you may not find the texture you are looking for. It may be stored in a different archive.

#### How to recognize the type of the texture and where it is used? 
#### Navigation
Telltale uses the following naming convention.
| First Prefix |  Description |
| --- | --- |
| adv |
| color | 
| env (environment)|
| fx (effect)|
| gobo |
| lookup |
| normalxy |
| obj | Textures used for objects.
| sk[N] (skeletons) | Textures used on characters. The number represents the height of the skeleton.
| tile | Used for repetitive texturing for floors, walls, roofs, etc.
| ui (user interface) | Textures used for the UI (buttons, dialogs, etc.)

| Suffix | Description |
| --- | --- |
| NONE | Diffuse texture |
| detail | Bump/SDF map |
| nm | Normal map |
| glow | Glow map |
| spec | Specular texture |
| ink | Sac

> Example: sk54_lee_head.d3dtx is a diffuse texture, however sk54_lee_head_spec.d3dtx is a specular texture.

#### What are mipmaps?
A lot of textures have mipmaps. They are basically lower resolution textures that are embedded in the file and are used at a distance in the game engine to prevent **[aliasing and artifacts](https://gdbooks.gitbooks.io/legacyopengl/content/Chapter7/mip1.png)**. 
![mipmaps](http://archive.gamedev.net/archive/reference/programming/features/dxmipmap/mipmaps.gif)



#### What is surface format/compression type?
Pixels aren't always stored in RGB or RGBA format. There can be many different ways for optimization purposes (saving space).

A lot of textures have mipmaps. They are basically lower resolution textures that are embedded in the file and are used at a distance in the game engine to prevent **[aliasing and artifacts](https://gdbooks.gitbooks.io/legacyopengl/content/Chapter7/mip1.png)**. 
![mipmaps](http://archive.gamedev.net/archive/reference/programming/features/dxmipmap/mipmaps.gif)

### Critical Note

> The Texture Mod Tool in the Image Properties section reveals both information about the Compression Type format, and the mipmap count to the user when extracting/converting the textures using this tool so you'll know when a specific texture needs to be in a specific format or requires mipmaps.
#### 
The **Compression Type** format is also something that you have to keep in mind when modifying/saving these textures. 
See this table
The formats must be the same and cannot change. 