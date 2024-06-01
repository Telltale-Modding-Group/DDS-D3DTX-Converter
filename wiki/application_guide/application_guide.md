# Application Guide

This page is meant to introduce users to the software's UI. The application window is resizable, and each column section can be scaled as well to suit your viewing needs.

![Main Thumb](/wiki/tutorial-screenshots/mainThumb.png)

---
## Top Menu Bar

![ui-guide1](/wiki/application_guide/ui_1.png)

On the top left of the application menu bar the following buttons are located:
- **Open** - It opens a folder browser dialog window where you can select the folder that contains your extracted textures.
- **Save** - Save the selected file to a location.
- **Add** - Add a file to the directory that is opened.
- **Delete** - Delete a file from the directory that is opened.
- **Help** - Opens the wiki page.
- **About** - Opens a window with information about the software, credits and a version number.

---
## Left Column Section

#### Textures Directory
![ui-guide2](/wiki/application_guide/ui_2.png)

This section contains a simple file browser for selecting and viewing all of your texture files.
- **Textures Directory** - The section title.
- **Directory Path** - Displays the path of the folder which you selected.
- **Return Folder Button** - Goes up one level in the directory when clicked. (Like the Previous Button in the File Explorer)
- **Refresh Directory Button** - Refreshes the **file browser** in case there are any changes to the folder content and the application does not update the interface.

#### File Browser
![ui-guide3](/wiki/application_guide/ui_3.png)

This section contains a simple file browser for selecting and viewing all of your texture files.
- **File Browser** - A viewer of the all supported image file formats, including **PNG, JPEG, NMP, TIFF, DDS, D3DTX and JSON**. It also supports folders. 

#### Context Menu
![ui-guide4](/wiki/application_guide/ui_4.png)

##### There is also a context menu for the middle section (Right-click on this section in the application)
- **Add** - Same functionality as **Add Button**.
- **Open** - Opens the file with the preferred software.
- **Open Folder** - Opens the selected folder inside the application.
- **Open in Explorer** - Opens the directory you have opened in File Explorer.
- **Refresh Directory** - Same functionality as **Refresh Directory Button**.
- **Delete File** - Same functionality as **Delete Button**.

##### There are also some double click actions 
- **Double clicking on a folder** - Opens the folder in the current software.
- **Double clicking on a file** - Opens the file in the preferred software.

---
## Right Column Selection

#### Image Preview
![ui-guide5](/wiki/application_guide/ui_5.png)

This section contains an image viewer. It displays **PNG, JPEG, BMP, TIFF, DDS and D3DTX** files and their names.

#### Conversion Section
![ui-guide6](/wiki/application_guide/ui_6.png)
- **Convert Options** - A combo box containing all possible options of the selected image format.
- **Choose Output Directory Checkbox** - A checkbox if you want to specify the path when converting.
- **Conversion Type** - A combo box containing all possible conversion options. Users should stick to the **Default** option for all games **after The Walking Dead (2012)**.
- **Convert Button** - Converts the texture into the chosen format.
- **Debug Info** - Shows all information regarding the texture in a new window. Works **only** on **D3DTX** and **DDS** files.

#### Image Properties
![ui-guide7](/wiki/application_guide/ui_7.png)

This section contains and shows the properties of an image when it is selected from the **File Browser** in the Textures Directory section.

- **Image Name** - The name of the image. In case of Telltale texture files, their embedded names are shown.
- **Pixel Width** - The width of the image. (Textures files use the width of the largest mipmap, also known as the main texture).
- **Pixel Height**- The height of the image. (Textures files use the height of the largest mipmap, also known as the main texture).
- **Surface Format** - The surface format of the texture. Additional information can be found here.
- **Transparency** - Indicates if the textures has transparency/alpha (sometimes it is unreliable).
- **Channel count** - Shows the number of color channels of the surface format (sometimes it is unreliable).
- **Mipmap Count** - The number of mipmaps in the texture. Telltale textures always have at least 1 mipmap (the main texture).

---
## Debug Window
![ui-guide8](/wiki/application_guide/ui_8.png)

When you click on **Debug Info**, a new window will appear, containing all debug texture data.

> Here is an example of the same texture, but it its DDS variant:
![ui-guide9](/wiki/application_guide/ui_9.png)
