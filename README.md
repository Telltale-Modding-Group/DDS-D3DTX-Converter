# Texture Mod Tool

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/iMrShadow/DDS-D3DTX-Converter)](https://github.com/iMrShadow/DDS-D3DTX-Converter/releases/)
[![Github All Releases](https://img.shields.io/github/downloads/iMrShadow/DDS-D3DTX-Converter/total.svg)](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/releases)  

## DISCLAIMER 
#### **This is a more up-to-date fork of the original D3DTX-DDS Converter.** 

This is an application designed for converting **.d3dtx textures (Telltale Tool Textures)**, to standard **.dds (Direct Draw Surface)**. **It can also do the opposite and convert a .dds into a .d3dtx and this can be leveraged to do texture mods for a Telltale Tool Game. There is also support for other image formats, but they are not recommended due to data loss.for textures.** 


**[DOWNLOAD HERE](https://github.com/iMrShadow/DDS-D3DTX-Converter/releases)**

![Main 1](tutorial-screenshots/mainThumb.png)


**NOTE:** Some **.d3dtx/.dds** textures will not work or show up properly. That is a limitation to SkiaSharp rather than the application. The converted *.dds* files are converted correctly.

For The Walking Dead Season 1 (2012) and older games texture editing check out [this tutorial.](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D-The-Walking-Dead-Season-1-(and-older-games)-Texture-Editing-Tutorial)

### Notes and Limitations

1. Untested a lot of the games, but they should work. Please report any encountered issues.
2. Upscaling and downscaling dimensions should in theory work, but it may not produce the desired results. Please be cautious. I recommend using the original dimensions.
3. Almost all surface formats are supported, which cover 99% of game textures, but there could be legacy ones.
4. This is primarily aimed for PC platform games. In the future I may add Android/iOS support, but it's **not guaranteed**.
5. Support for legacy titles (before **"The Wolf Among Us"**) is planned for the future.
6. Some texture layouts are not supported, but they are not commonly found.

**TESTED**
- The Wolf Among Us
- Minecraft: Story Mode
- The Walking Dead: Season Two
- The Walking Dead: Michonne
- Tales from the Borderlands
- Batman: The Telltale Series
- Minecraft: Story Mode – Season Two
- The Walking Dead: A New Frontier
- The Walking Dead: The Final Season
- The Walking Dead: The Telltale Definitive Series

**UNTESTED**
- Game of Thrones
- Guardians of the Galaxy: The Telltale Series 
- Batman: The Enemy Within
- The Walking Dead Collection

### How to use? (The following links are outdated, but they are still informative.)

**[This guides you on the interface of the application. (It's easy)](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BHelp%5D-Application-Guide)**

**[This tutorial describes how to use the application](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))**

**[This is how you can compile the modified textures into a Mod File](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2)).**

**[FOR GETTING STARTED MAKING TEXTURE MODS, GO HERE](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-1))**

**If you need help with the application then please [go here](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki)**

**If there are any issues PLEASE read [THIS](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BHelp%5D---Reporting-an-Issue-or-Bug) report them to [HERE](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/issues)**

### Developers

*Only for modders/developers who plan on forking/modifying/downloading the project.*

There are 3 projects in this repository:
- **DDS_D3DTX_Converter_GUI** being the main application release with a GUI built with **Avalonia UI** aimed to be cross-platform. It has the latest texture conversion functionality. 
- **D3DTX_TextureConverter** is a console application which has texture conversion functionality. It's older than **DDS_D3DTX_Converter_GUI**, which is not recommended to use. It can be still used for the latest Telltale games, but not for older titles.
- **TextureMod_GUI** is the original GUI application built with **WPF (Mahapps)**. It is not recommended to use as its extremely outdated and probably broken.

If you want to use the outdated console application, you can open it in Visual Studio. It would require you to change the code to set some parameters or switch to a different mode.

**If you want to learn about the file format** I suggest you look in the **DDS_D3DTX_Converter_GUI** and look at the **[TelltaleD3DTX](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/tree/main/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/TelltaleD3DTX)**, it is well documented and describes the .d3dtx format. **Direct Surface Draw (.dds)** is a Microsoft file format, which is described in detail [here](https://learn.microsoft.com/en-us/windows/win32/direct3ddds/dx-graphics-dds). If you don't want to read, you can check out in [this folder](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/tree/main/DDS_D3DTX_Converter_GUI/DDS_D3DTX_Converter/DirectX) the following classes:
- DDS_HEADER.cs
- DDS_HEADER_DX10.cs
- DDS_PIXELFORMAT.cs

### Credits
- Thanks to [David Matos](https://github.com/frostbone25) for introducing me to the Telltale Modding Community and modding as a whole, for guiding and supporting me throughout the development, and for his original work on the old converter.
- Thanks to [Lucas Saragosa](https://github.com/LucasSaragosa) for figuring out the Telltale formats and helping me and David for parsing the .d3dtx correctly. I've also used their [TelltaleToolLib](https://github.com/LucasSaragosa/TelltaleToolLib) project as a reference, which had a lot of influence in the conversion process.
- Thanks to [Mawrak](https://github.com/Mawrak) for their work on the original GUI application, testing the software and providing critical feedback.
- Thanks to Arrizble for testing the software on **Minecraft: Story mode** series and providing critical feedback.
- Thanks to [SVG Repo](https://www.svgrepo.com/) for their amazing GUI icons.
