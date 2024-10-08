
### Version 2.4.0

#### Wiki (WIP):
- Initial commit for the wiki. It is serviceable, but not yet fully completed.
- Contains a basic tutorial applicable to other games.
- Contains additional articles for textures, archives and surface formats.

#### Converter:
- Added Poker Night 2 (2013) support (mVersion 3 games).
- Added Minecraft Story Mode: Season One - Xbox One support (mVersion 6 games). (Thanks [Knollad Knolladious](https://www.youtube.com/channel/UCegvS4IJnO926qnuIEfQfJw)!)
- Added TWD: Michonne (2016) support (mVersion 7 games). 
- Added support for some legacy Telltale games (pre-Poker Knight 2). These include:
    - The Walking Dead (2012) (Thanks [Lucas Saragosa](https://github.com/LucasSaragosa)!)
    - Jurassic Park (2011)
    - Back to the Future: The game (2010)
    - Puzzle Agent 2 (Untested)
    - Law & Order: Legacies (Untested)
- Added array texture support (Thanks [Knollad Knolladious](https://www.youtube.com/channel/UCegvS4IJnO926qnuIEfQfJw)!).
- Added cubemap texture support. Previously they were split into 6 separate DDS images.
- Added cubemap array texture support (They usually do not exist. Please report if you find any).
- Added volumemap texture support (They usually do not exist. Please report if you find any).
- Added support to export a DDS image even if the version is not recognized (it will not generate a JSON file). This is a feature parity with Telltale Explorer and should work for any Telltale game.
- Reworked the JSON file - it now has an additional field class that includes the conversion type used. The app is still compatible with older versions.
- Added the Debug CLI back, which will act as the main one for now.
- Improved the bulk conversion performance, it now converts more textures per thread.

#### GUI: 
- Added a Conversion Type combobox. 
- Added support for previewing older D3DTX files. It should be almost up-to-par to that of Telltale Explorer. 
- Added a Debug info button. When clicked on, it will display in a dialog the information about DDS images and D3DTX files.
- Added a new exporting feature, when the mVersion is unknown. It is used when the "Default" option is used.
- Changed the names of some Telltale surface formats. They are now shown a little bit more consistent with their more known DXGI counterparts.
    - DXT1   now shows as BC1
    - DXT3   now shows as BC2
    - DXT5   now shows as BC3
    - DXT5a now shows as BC4
    - DXN    now shows as BC5
- Reorganized some elements. 
    - Image preview should be a little bigger than before.
    - Image properties take the whole panel space.
    - The data grid is now expanded to the bottom.
- Improved "Add Files" button.
- Renamed the app title to "Telltale Texture Mod Tool".
- Updated the Help button link.
- Updated the About window.
- Enhanced some functions.
- Improved window and column resizing.

#### Bugfixes:
The converter is now more robust than before, which fixes a lot of bugs.
- Fixed a lot of bugs regarding DDS header writing including flags, mips and pixel formats. 
    - Fixed Telltale A8 surface format conversion.
    - Fixed potentially ARGB2101010.
    - Fixed sRGB conversion.
    - Fixed other pixel formats headers.
    - Fixed extracting the data correctly.
    - Fixed an inconvenience where if the texture had no mips it still had it enabled in the header.
- Fixed incorrect Telltale surface formats enum values.
- Fixed writing non-existing compression formats.
- Fixed a lot of DDS reading bugs.
- Fixed all region indexing when writing to D3DTX.
- Fixed SurfaceGamma not changing depending on the DDS format.
- Fixed channel count displaying wrong values in image properties (partially).
- Fixed transparency issue DDS images. Previously transparent pixels appeared white. Now, a side effect is slower load time (but worth it).
- Fixed displaying rare surface formats which include but are not limited to A8, L8, A8L8, L16, R16, RGBA16, ARGB16 and more.
- Fixed SamplerState not having correct values.
- Fixed not reading images with uppercase extensions.
- Fixed bulk conversion when the output directory was not set.
- Fixed "Delete File" from context menu not working.
- Fixed error message notification when converting from DDS to other file formats, even thought it is created.
- Fixed error message notification that when converting from other file formats to DDS the JSON was not found.
- Fixed GUI resizing of some elements.
- Fixed GUI window resizing not working properly.
- Fixed "Refresh Button" not working properly.
- Fixed the context menu "Refresh" command not working properly.
- Fixed many other minor bugs.

#### Technical Improvements:
- The project now fully utilizes the DirectXTex .NET wrapper. This brings a lot of bug fixes, overall improvements and readability to the codespace.
- Reorganized the whole project for better readability.
- Refactored a lot of classes and functions.
- Removed or archived a lot of unused code and files. The project should be a lot cleaner with less code. 
- Reduced tool size (with around 15MBs).
- Deprecated D3DTX_V8.
- Bumped DirectXTexNet to v1.0.7 (it now uses DirectXTex March 2024 version).
- Bumped Avalonia.Xaml.Behaviors to v11.0.10.9.
- Bumped Avalonia.Svg.Skia to v11.0.0.18.
- Bumped ImageSharp to v3.1.4.

### Version 2.3.0

#### Changelog:
- Updated the README to be more up-to-date, which now includes an actual screenshot of the new converter
- Added bulk conversion. The simplest way to use it: select a folder, choose the desired option and convert
- Added a checkbox for optional output directory
- Many bug fixes to DDS -> D3DTX conversion process. DDS files now finally convert correctly
- D3DTX meta headers now convert properly
- Scrollbar resetting after conversion is now fixed, but there is a GUI small side effect with focusing on the selectable item
- Some small GUI QOL improvements

### Version 2.2.0

#### Changelog:
- D3DTX_V5 - D3DTX_V8 files now should save properly when converting from .dds
- Some fixes related to surface formats
- Some fixes related to D3DTX -> DDS conversion
- Added initial header file for The Walking Dead
- File items in the app explorer are now filtered - only relevant files will show (.d3dtx, .dds, .json and supported image formats)

### Version 2.1.0

#### Changelog:
- Added more surface format support.
- Fixed conversion with other compression formats.
- Fixed mToolProps - it now saves correctly in the .json files.
- Fixed EnvMap files saving destination paths and .json files.

### Version 2.0.1 Hotfix

#### Changelog:
- Fixed converting to other file formats when path contains whitespaces.
- Fixed previewing other d3dtx formats.

### Version 2.0.0 (New GUI)

#### Changelog:
- Initial commit of the new GUI converter

#### Features:
- Converts the following formats: .d3dtx <-> .dds, .dds <-> .png, .jpg, .bmp, .tiff.
- Preview the images and its properties (if supported).
- The GUI acts like a file explorer. You are able to open and delete folders and files using double clicking, the menu buttons or context menu buttons.

### Version CLI Beta 2.0.1
- Fixed missing framework issue.

### Version CLI Beta Pre-release

- Fully support D3DTX conversions without restrictions with a functioning CLI.

### Version 1.0.0 (Original GUI)

#### Changelog
- First Release



