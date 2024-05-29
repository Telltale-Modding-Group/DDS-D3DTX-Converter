# Application Guide

This page is meant to help inform and guide users as to how this application works and what are the different UI elements to it. The application window is also resizable, and each section can be scaled as well to suit your viewing needs.

![Main Thumb](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/mainThumb.png)

## Top Menu Bar

![ui-guide0](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide0.png)

On the top left of the application menu bar the following buttons are located:
- **Open** - It opens a folder browser dialog window where you can select the folder that contains your extracted textures.
- **Save** - Save the selected file to a location.
- **Add** - Add a file to the directory that is opened.
- **Delete** - Delete a file from the directory that is opened.
- **Help** - Opens the wiki page.
- **About** - Opens a window with information about the software, credits and a version number.

## Textures Directory

![ui-guide1](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide1.png)

This section contains a simple file browser for selecting and viewing all of your texture files.

##### On the top left of this section
- **Textures Directory** - The section title.
- **Directory Path** - Displays the path of the folder which you selected.
- **Return Folder Button** - Goes up one level in the directory when clicked. (Like the Previous Button in the File Explorer)
- **Refresh Directory Button** - Refreshes the **file browser** in case there are any changes to the folder content and the application doesn't update the interface.

**In the middle of this section**
- **File Browser** - a view of the all supported image file formats, including **.png, .jpg, .bmp, .tiff, .dds, .d3dtx and .json**. It also supports folders. 

![ui-guide5](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide5.png)

##### There is also a context menu for the middle section (Right-click on this section in the application)
- **Add** - Same functionality as **Add Button**.
- **Open** - Opens the file with the preferred software.
- **Open Folder** - Opens the selected folder inside the application.
- **Open in Explorer** - Opens the directory you have opened in File Explorer.
- **Refresh Directory** - Same functionality as **Refresh Directory Button**.
- **Delete File** - Same functionality as **Delete Button**.

##### There are also some double click actions 
- **Double clicking on a folder** - opens the folder in the current software
- **Double clicking on a file** - opens the file in the preffered software

##### On the middle right of this section
- **Convert Options** - A combo box containing all possible options of the selected image format.
- **Choose Output Directory Checkbox** - A checkbox if you want to specify the path when converting.
- **Conversion Type** - A combo box containing all possible conversion options. Users should stick to the **Default** option for all games **after The Walking Dead**.
- **Convert Button** - Converts the texture into the chosen format.

## Output Section

![ui-guide2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide2.png)

This section contains an Output in which information is written during the conversion process.

## DDS Image Preview

![ui-guide3](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide3.png)

This section contains an image viewer. It displays **.png, .jpg, .bmp, .tiff, .dds and .d3dtx** files.

**On the top of this section contains**
- **File Name** - Displaying the name of the selected file.

**On the middle of this section contains**
- **Image Window** - The selected image from the **File Browser** will be displayed here. w

### Image Properties

![ui-guide4](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide4.png)

This section contains and shows the properties of an image when it's selected from the **File Browser** in the Textures Directory section.

**Properties that will be displayed**
- **Image Name** - The name of the image. Telltale texture files 
- **Pixel Width** - The width of the image. (Textures files use the width of the largest mipmap, also known as the main texture).
- **Pixel Height**- The height of the image. (Textures files use the height of the largest mipmap, also known as the main texture).
- **Surface Format** - The surface format of the texture. Additional information can be found here.
- **Transparency** - Indicates if the textures has transparency/alpha.
- **Channel count** - Shows the number of color channels of the surface format (at the moment it's broken).
- **Mip Map Count** - The number of mipmaps in the texture. Telltale textures always have 1 mipmap (the main texture).
- **Layout** - The type of texture layout. The possibilities are: 2D, 3D, CubeMap.

And that is all there is to it!