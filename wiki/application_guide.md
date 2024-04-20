# Application Guide

This page is meant to help inform and guide users as to how this application works and what are the different UI elements to it. The application window is also resizable, and each section can be scaled as well to suit your viewing needs.

![Main Thumb](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/mainThumb.png)

## Top Menu Bar

![ui-guide0](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide0.png)

Not much to see in the top bar of the application. 

**On the top left of the application menu bar is**
- **Icon** - the application icon (obviously)
- **TEXTURE MOD TOOL** - The name of the application (obviously)

**On the right of the application menu bar is**
- **Version Number** - the application version number
- **Help Button** - for getting help with the application. (Clicking this button will open your default web browser and will lead you to the main help pages for this application.)

## Textures Directory

![ui-guide1](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide1.png)

This section contains a simple file browser for selecting and viewing all of your texture files.

**On the top of this section**
- **Textures Directory** - The section title
- **Directory Path** - tells you the path of the folder in which you selected
- **Folder Button** - for browsing and changing the Texture Directory Path. It opens a folder browser dialog where you can select the folder that contains your extracted D3DTX textures.

**In the middle of this section**
- **File Browser** - a listview of the .dds, .header, .d3dtx files that are in your directory

![ui-guide5](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide5.png)

**There is also a context menu for the middle section** (Right-click on this section in the application)
- **Refresh** - simply refreshes the Listview in case there are any changes and the application doesn't update the interface.
- **Open Folder** - opens the directory you have open in File Explorer

**On the bottom of this section**
- **Convert to DDS** - Converts all .d3dtx files in your directory into .dds files. It also generates a .header file for converting back to .d3dtx later (This button is greyed out only when there is no directory path given, or if there are no D3DTX textures in the directory)
- **Convert to D3DTX** - Converts all .dds and .header files in your directory back into .d3dtx files. (This button is greyed out only when there is no directory path given, or if there are no .dds and .header files in the directory)
- **Refresh Button** - Refreshes the Listview in case there are any changes and the application doesn't update the interface.

## Output Section

![ui-guide2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide2.png)

This section contains an Output in which information is written during the conversion process.

## DDS Image Preview

![ui-guide3](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide3.png)

This section contains an image viewer for viewing DDS textures. It's worth noting that this only displays .DDS images, not D3DTX images (hence the name DDS Image Preview).

**On the top of this section contains**
- **DDS Image Preview** - title of the section
- **Image Name** - where the name of the image is displayed.

**On the middle of this section contains**
- **Image Window** - When an image is selected from the Textures Directory Listview it will be displayed here.

## Image Properties

![ui-guide4](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/ui-guide4.png)

This section contains and shows the properties of an image when it's selected from the Listview in the Textures Directory section

**Properties that will be displayed**
- **Image Name** - the name of the image
- **Pixel Width** - the image resolution in width. (of the largest mipmap, or also known as the main texture)
- **Pixel Height**- the image resolution in height. (of the largest mipmap, or also known as the main texture)
- **DDS Format** - the format of the DDS texture.
- **Has Mip Maps** - displays true or false depending on if the DDS image has mipmaps.
- **Mip Map Count** - how many mipmaps are in the image.

And that is all there is to it!