# Application Guide

This page is meant to introduce users to the software's UI. The application window is resizable, and each column section can be scaled as well to suit your viewing needs.

![Main Thumb](/Docs/tutorial-screenshots/mainThumb.png)

---
## Top Menu Bar

![ui-guide1](/Docs/application_guide/top_menu_bar.png)

- `Open Folder` - It opens a folder explorer dialog window where you select the folder that contains your extracted textures.
- `Save` - Save the selected file to a location.
- `Add` - Add a file to the directory that is opened.
- `Delete` - Delete a file from the directory that is opened.
- `Help -> Help` - Opens the GitHub documentation.
- `Help -> About` - Opens a dialog window with information about the software.

---
## Left Column Section

#### File Browser
![ui-guide2](/Docs/application_guide/file_browser.png)

This section contains a simple file browser for selecting and viewing all of your texture files, JSON files and folders.
- `Current Directory` - Displays the currently selected folder path.
- `Go One Directory Up Button` - When clicked, the current directory level goes up by one level.
- `Refresh Directory Button` - Refreshes the `file browser`.
- `File Browser`  - A file explorer which displays `D3DTX`, `DDS`, `TGA`, `PNG`, `JPEG`, `BMP`, `TIFF`, and `JSON` files, as well folders (they do not have a file type). 

Inside the `File Browser`:
- `Double clicking on a folder` - Opens the folder in Telltale Texture Tool.
- `Double clicking on a file` - Opens the file with its preferred software.

#### Context Menu (Right-click Menu)
![ui-guide4](/Docs/application_guide/context_menu.png)

- `Open File` - Opens the file with its preferred software.
- `Open Folder` - Opens the selected folder inside the application.
- `Open in Explorer` - Opens the current directory in a `File Explorer.
- `Add File` - Same functionality as `Add Button`.
- `Refresh Directory` - Same functionality as `Refresh Directory Button`.
- `Delete File` - Same functionality as `Delete Button`.


---
## Middle Column Selection

#### Image Preview

This section contains an image viewer. It displays all supported image formats. Additionally, there are `mip` and `face` sliders, which allow you to view them.

There is also `Pan and Zoom` feature, which means you can zoom in/out using your scroll wheel, and move around by dragging. The camera resets when you change your selected file.

![ui-guide5](/Docs/application_guide/image_preview.png)

---
## Right Column Selection

#### Image Information
![ui-guide7](/Docs/application_guide/image_information.png)

This section displays the properties of the currently selected texture.

- `File Name` - The file name of the image.
- `Image Name` - The name of the texture. If the file is `D3DTX`, its embedded name will be displayed.
- `Pixel Width` - The width of the image.
- `Pixel Height`- The height of the image.
- `Surface Format` - The surface format of the texture.
- `Color Space` - The color space of the surface format. If it displays `Unknown`, assume it is `Linear`.
- `Mip Count` - The number of texture mip levels. The minimum number is 1.
- `Array Size` - The number of textures inside the file. The minimum number is 1.
- `Has Transparency` - Indicates if the surface format supports transparency.
- `Alpha Mode` - Indicates the alpha mode of the texture.
- `Texture Layout` - Displays the layout of the texture.

More information can be found [here](/Docs/articles/textures.md) and [here](/Docs/articles/surfaces.md).

#### Advanced Options
![ui-guide7](/Docs/application_guide/advanced_options.png)

This section contains advanced settings.

- `Select Game` combobox - If you choose a title, the converter will automatically try to use the `D3DTX` version for that title. `Default Mode` automatically detects the version, but it can be slow.
- `Legacy Console` checkbox - If checked, that means the texture is from an older console game.
- `Auto Compression` - Automatically applies `BC1` or `BC3` compression to the textures.
- `Auto Normal Maps` - Applies some specific normal map effects to make them compatible for Telltale games.
- `Generate Mips -> Automatic` - Automatically generate the maximum amount of mips.
- `Generate Mips -> Manual` - Choose the number of generated mips. Use for console and mobile platforms.
- `Enable Swizzling` - Allows you to swizzle/deswizzle console textures from the selected platform.
- `Enable Effects` - Allows you to apply image effects to the image. The effects are mainly used for normal maps.
- `Create Normal Map` - If checked, it creates a normal map from the image. Currently, it is not reliable - I recommend using other `Normal Map` tools like `NVIDIA Texture Tools`.

#### Conversion Section
![ui-guide6](/Docs/application_guide/conversion_panel.png)
- `From` and `To` - convert `From` one format `To` another.
- `Choose Output Directory Checkbox` - A checkbox if you want to specify the path when converting.
- `Convert Button` - Converts the texture into the chosen format.

---
## Debug Information
When you click on `Debug Information`, you will see additional information about the texture. Useful for low-level debugging.

Examples:

The D3DTX variant of the texture:
![ui-guide8](/Docs/application_guide/debug_information_d3dtx.png)

Example: The DDS variant of the texture:
![ui-guide9](/Docs/application_guide/debug_information_dds.png)
