## [Home](/Docs/home.md)

This page contains information regarding textures.

### How do I find the textures?
Use Telltale Explorer. [This part of the tutorial explains how to use it.](/Docs/tutorial_part1/tutorial_part_1.md)

### Where are the textures stored?
I mentioned that they are stored in **TTARCH files**, but you may have seen there are *a lot*. Let's see how to recognize what a particular archive stores.
First, we separate the name by underscores ("_").
The first few words describe in what game it is used and where it is being used.
Some of the game-related stuff are:
| Name | Description |
| --- | --- |
| **Boot** | These files are used during the boot process of the game. There are textures for loading screens or titles.
| **Menu** | These files are used in the game's menu. There are textures for buttons, title screens, backgrounds and more.
| **Project** | [WIP] |
| **[GameName][3-digit number]** | The files are used in a particular episode of a game. The first digit of the number represents the season, the latter 2 digits represent the episode number. |
| Other uncommon types | Usually for shaders, effects or localization-related files. |
> Example 1: **WalkingDead**\_pc\_**WalkingDead201**_txmesh stores the textures for episode 1 of The Walking Dead Season 2.
> Example 2: **WDC**\_pc\_**UISeasonM**_txmesh contains UI textures for The Walking Dead Michonne in The Walking Dead Collection.

The last word describes what files are stored there. The texture related ones are stored in the following:
| Name | Description |
| --- | --- |
| **txmesh** | <ul><li>~95% of the textures are stored there.</li><li>Each Telltale episode has its own one that contains data ONLY for its episode.</li><li>Contains meshes (d3dmesh).</li></ul>
| **compressed** | <ul><li>~5% of the textures are stored there.</li><li>Contains other varying file types.</li></ul>
| **all** |<ul><li>Small amount of textures are stored there.</li><li>Contains a lot of varying file types.</li></ul>
> Example: Most of the common textures for TWDS1 in TWD:DE are stored in ProjectSeason1_txmesh.ttarch2 archive.

There are other types or archives and Telltale types. Check out [this page](/Docs/articles/ttarch.md)

### Which textures do I need to edit?
This question is a complicated one and it depends on what the user wants. Let's answer some additional questions first.

#### What types of textures usually exist?
There are a lot of types of textures. [This article](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm) explains the main ones. Typically *(unless you are an experienced texture artist)* you would only really need to be modifying the diffuse textures. 

##### What are diffuse textures?
**[Diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)** are just the raw material/model colors and these textures typically have no suffix after them in their file names like [_spec, _ink, _detail, etc.](/Docs/articles/textures.md#navigation) 
> Sometimes you may not find the texture you are looking for. It may be stored in a different archive.

### How to recognize the type of the texture and where it is used? 
#### Navigation
Telltale uses the following naming convention.
| First Prefix | Description |
| --- | --- |
| adv | Lightmaps.
| color | Colors.
| env (environment)| Environment textures, like for e.g. trees, ground, walls, etc.
| fx (effect) | Effects.
| gobo | [Go-Betweens](https://garagefarm-net.medium.com/gobos-in-lighting-and-what-is-a-gobo-3276e17bd76b). Used for lighting.
| lookup | [Lookup textures.](https://docs.unity3d.com/550/Documentation/Manual/script-ColorCorrectionLookup.html)
| normalxy | [Normal maps, stored in BC5. They have 2 colors only.](https://en.wikipedia.org/wiki/Normal_mapping) |
| obj | Objects.
| sk[N] (skeletons) | Textures used on characters. The number represents the height of the skeleton.
| tile | Repetitive texturing like floors, walls, roofs, etc.
| ui (user interface) | UI (buttons, dialogs, etc.)

These suffixes indicate and deal with other material properties.
| Suffix | Description |
| --- | --- |
| NONE | Diffuse texture. |
| detail | Bump/SDF map. Black lines of the material. |
| nm | [Normal map.](https://en.wikipedia.org/wiki/Normal_mapping) Note: Telltale have their normal maps reversed. |
| glow | Glow map. |
| spec | [Specular texture.]((https://www.modding-forum.com/guide/17-diffuse-specular-and-normal-maps/)) |
| ink | Black lines of the material. |
| sss | Subsurface Scattering (Thanks Knollad)
| gradient | Gradients, probably used for shading.

 > [!NOTE]  
 > Example: sk54_lee_head.d3dtx is a diffuse texture, while sk54_lee_head_spec.d3dtx is a specular texture.

#### What are mipmaps?
A lot of textures have mipmaps. They are basically lower resolution textures that are embedded in the file and are used at a distance in the game engine to prevent **[aliasing and artifacts](https://gdbooks.gitbooks.io/legacyopengl/content/Chapter7/mip1.png)**. 
![mipmaps](http://archive.gamedev.net/archive/reference/programming/features/dxmipmap/mipmaps.gif)

#### What is a surface format/compression type?
Pixel data is **not** always stored in RGB or RGBA format as one would presume. There can be many different ways of storage for optimization purposes in order to save space. There are compressed and uncompressed formats, stored in different data types with different amount of channels.
> [This page](https://www.khronos.org/opengl/wiki/Image_Format) has somewhat compact information if you are more interested. 
> Check out [this page](/Docs/articles/surfaces.md) for all available surface formats.
> Check out [this file](/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/Telltale/TelltaleEnums/T3SurfaceFormat.cs) for more technical information of all available Telltale surface formats.
 
 #### What is a texture layout?
Textures can be 2D, Volumemaps (3D) or Cubemaps. They can also be 2D Array or Cubemap array.
Telltale commonly uses **2D**. In rare cases - **2D Array or Cubemaps**. Skunkcape have used volumemaps for lookups.
Here are some examples on how they should look like.
| 2D             |  Cubemap
:-------------------------:|:-------------------------:
<img src="https://cdna.artstation.com/p/assets/images/images/014/294/748/large/grace-mericer-textureprac22.jpg?1543367842" alt="drawing" width="600"/> |  ![](https://learnopengl.com/img/advanced/cubemaps_skybox.png)

 > [This page](https://www.khronos.org/opengl/wiki/Texture#Theory) has somewhat compact information if you are more interested. 


#### This information is overwhelming!
Take a small break. I suggest looking through it briefly.
In any case, Telltale mostly uses **BC1, BC2, BC3, BC4 and BC5** compressed formats, and **RGBA8**, **BGRA8** and **A8** uncompressed formats.
As for texture layouts - 99.9% are 2D textures.
> Check out [this page](/Docs/articles/surfaces.md) for all available surface formats.


### This information is still overwhelming!
The Texture Mod Tool provides Image Properties section, which reveals both information about the compression type format, the mipmap count and the texture layout (if you use the Debug Info button). This gives you information in which format to save and whether you should generate mipmaps or not. Telltale textures always have a mipmap count of at least 1. 
> Check out [this page](/Docs/articles/surfaces.md) to know in which format to save.

### What changes can we make to textures?
Technically you can change the width, height, surface format, mipmap count and potentially the layout (use this only for **creating** custom textures).

See [this page](/Docs/articles/surfaces.md) to know which format to use depending on the texture type.
> [!WARNING]  
> Warning: If the game crashes, that means the format is not supported for the game.
 >>Example: BC6H and BC7 are not supported on older Telltale titles. The surface format needs to be changed to a supported one.
 
