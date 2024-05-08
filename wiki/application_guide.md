# Application Guide

This page is meant to help inform and guide users as to how this application works and what are the different UI elements to it. The application window is also resizable, and each section can be scaled as well to suit your viewing needs.

![Main Thumb](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/mainThumb.png)

## Top Menu Bar

![ui-guide0](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide0.png)

**On the top left of the application menu bar is**
- **Open** - It opens a folder browser dialog where you can select the folder that contains your extracted D3DTX textures.
- **Save** - save the selected file to a location
- **Add** - add a file to the folder
- **Delete** - delete a file 
- **Help** - opens the wiki page
- **About** - opens a window with information about the software with credits and a version number

## Textures Directory

![ui-guide1](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide1.png)

This section contains a simple file browser for selecting and viewing all of your texture files.

**On the top left of this section**
- **Textures Directory** - The section title
- **Directory Path** - tells you the path of the folder in which you selected
- **Return Folder Button** - goes up one level in the directory
- **Refresh Directory Button** - simply refreshes the Listview in case there are any changes and the application doesn't update the interface.

**In the middle of this section**
- **File Browser** - a datagridview of the all supported image file formats, including **.png, .jpg, .bmp, .tiff, .dds, .d3dtx and .json** 

![ui-guide5](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide5.png)

**There is also a context menu for the middle section** (Right-click on this section in the application)
- **Refresh** - 
- **Open Folder** - opens the directory you have open in File Explorer

**There are also some double click actions** 
- **Double clicking on a folder** - opens the folder in the current software
- **Double clicking on a file** - opens the file in the preffered software

**On the middle right of this section**
- **Convert Options:** - a combo box containing all possible options of the selected image format
- **Choose Output Directory Checkbox** - a checkbox if you want to specify the path when converting
- **Convert Button** - Refreshes the Listview in case there are any changes and the application doesn't update the interface.

## Output Section

![ui-guide2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide2.png)

This section contains an Output in which information is written during the conversion process.

## DDS Image Preview

![ui-guide3](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide3.png)

This section contains an image viewer for viewing textures. It displays **.png, .jpg, .bmp, .tiff, .dds and .d3dtx** files (if the game is supported).

**On the top of this section contains**
- **File Name** - where the name of the image is displayed
- **Image Preview** - title of the section


**On the middle of this section contains**
- **Image Window** - when an image is selected from the Textures Directory DataGrid it will be displayed here

## Image Properties

![ui-guide4](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide4.png)

This section contains and shows the properties of an image when it's selected from the Listview in the Textures Directory section

**Properties that will be displayed**
- **Image Name** - the name of the image
- **Pixel Width** - the image resolution in width. (of the largest mipmap, or also known as the main texture)
- **Pixel Height**- the image resolution in height. (of the largest mipmap, or also known as the main texture)
- **Surface Format** - the surface format of the texture. Additional info can be found here.
- **Transparency** - indicates if the texture has alpha
- **Channel count** - shows the number of color channels of the surface format (at the moment it's broken)
- **Mip Map Count** - how many mipmaps are in the texture
- **Layout** - shows what type of texture layout it is - 2d, 3d or cube texture (to be added)

And that is all there is to it!