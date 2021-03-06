# Texture Mod Tool

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Telltale-Modding-Group/DDS-D3DTX-Converter)](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/releases)
[![Github All Releases](https://img.shields.io/github/downloads/Telltale-Modding-Group/DDS-D3DTX-Converter/total.svg)](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/releases)  

This is an application designed for converting **.d3dtx textures (Telltale Tool Textures)**, to standard **.dds (Direct Draw Surface)**. **It can also do the opposite and can be used to do texture mods for a Telltale Tool Game.** 

**[DOWNLOAD HERE](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/releases)**

**[FOR GETTING STARTED MAKING TEXTURE MODS, GO HERE](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))**

**If you need help with the application then please [go here](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki)**

**If there are any issues PLEASE read [THIS](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BHelp%5D---Reporting-an-Issue-or-Bug) report them to [HERE](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/issues)**

![Main 1](tutorial-screenshots/mainThumb.png)

## DISCLAIMER (Please Read)

**NOTE:** Some **.d3dtx** textures will not work or show up properly and might even look corrupted after conversion into **.dds**. This is simply because once again, this is very new and I have yet to support more d3dtx texture variants (there are a lot). For the most part, any regular diffuse/albedo textures should work.

This is currently being developed and tested only with ***"The Walking Dead Telltale Definitive Series"*** textures. (for the time being)

### Limitations

1. Only supports **The Walking Dead Definitive Edition** textures *(for the time being)*
2. Can't upscale or downscale the texture resolution *(for the time being)*
3. Not all textures are supported, textures like **'specular maps'** *(some of them)*, **'ink maps'**, **'normal maps'** *(some of them)* may not be converted by the tool properly. *(for the time being)* **However the majority of the regular color/diffuse/albedo and even alpha textures should be supported.**

## How to use?

**[This guides you on the interface of the application. (It's easy)](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BHelp%5D-Application-Guide)**

**[This tutorial describes how to use the application](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))**

**[This is how you can compile the modified textures into a Mod File](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2)).**

### Developers

*Only for modders/developers who plan on forking/modifying/downloading the project*

There are 2 projects in this repository, **TextureMod_GUI** being the main application release with a GUI and it has the texture conversion functionality. The other project **D3DTX_TextureConverter** is a console application has texture conversion functionality as well but no GUI, however this ocassionally will be more up to date and newer than the GUI project's conversion functionality. The reason for that is that it's faster to test and iterate with the console application than the GUI application.

If you are new and don't want to deal with a console application and would rather have a user-friendly interface then use **TextureMod_GUI**, however if your comfortable with using a console application you can use the  **D3DTX_TextureConverter** BUT it will require you to change the code directly to set some parameters or switch to a different mode.

**If you want to learn about the file format** I suggest you look in the **D3DTX_TextureConverter** and look at the **[Program.cs](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/D3DTX_TextureConverter/D3DTX_TextureConverter/Program.cs)** script, it's is well documented and describes the .d3dtx format and .dds format well.
