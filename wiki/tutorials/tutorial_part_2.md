## Step 1/9 - Selecting your Texture Folder

Open the application, simple and straightforward.

![tut1](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut1.png)

Go to the folder icon and click it to browse for your folder containing your **.d3dtx** textures.

![tut2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut2.png)

It will open a folder browser, in here navigate and find the folder that contains your **.d3dtx** textures. Worth mentioning you can just have a single **.d3dtx** file in there or multiple. Once your folder is selected in the browser, just click **Select Folder**.

![tut3](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut3.png)

## Step 2/9 - Converting textures to .dds

Once you select your folder, the **Textures Directory section** will be populated with your **.d3dtx** files. The next thing to do here is to now convert these **.d3dtx** files into a **.dds** by clicking **Convert To DDS**. 

It's worth mentioning that as of v1.0.0 the application will freeze for a few moments, but it will not crash. Just let it run and the conversion will be complete.

![tut4](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut4.png)

## Step 3/9 - Browsing through the .dds textures

Once the application finishes converting the **.d3dtx** textures, it will remove the original **.d3dtx** textures from the directory and replace them with the extracted **.dds** texture file along with a **.header** file that contains the original d3dtx header **(DO NOT REMOVE THIS FILE)**. 

**With this process complete you can now browse through the textures and find which one you want to modify.**

![tut5](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut5.png)

**Once you have picked your .dds file to edit**, be sure to select it in the application like so, and before we move forward the next step is to look in the **Image Properties** section to know what to keep in mind when saving the texture after modification.

![tut6](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut6.png)

## Step 4/9 - Noting the Image Properties (IMPORTANT)

Look under the **Image Properties** section of the selected image you want to modify and note the information displayed. The **image resolution** needs to remain the same *(this will change in a later update)*. Not only that you need to pay attention most importantly to the **DDS Format**, and **Mip Map** info. 

If the texture has mipmap information. **Has Mip Maps** will be displaying True along with **Mip Map Count** which will display the number of mipmaps. Occasionally when saving a dds texture the mipmap count will change, **this is fine** and won't ruin the texture. We just want to make sure that when we modify and save the image, that our image editing software generates mipmaps and is also saving in the same compression type as the original.

![tut7](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut7.png)

## Step 5/9 - Opening the Image in your editing software

Once we have the information noted. To get the texture you can **right-click** on the File Browser in the **Textures Directory** section and click **Open Folder** to open the folder in File Explorer.

![tut8](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut8.png)

Once you have it open in file explorer you can right click and edit this image in the following programs. 

### **[Photoshop](https://www.adobe.com/products/photoshop.html)** with **[this DDS plugin](https://software.intel.com/content/www/us/en/develop/articles/intel-texture-works-plugin.html)** or **[Paint.NET](https://www.getpaint.net/)**. 

In the case of this tutorial we are using **[Photoshop](https://www.adobe.com/products/photoshop.html)** with **[this DDS plugin](https://software.intel.com/content/www/us/en/develop/articles/intel-texture-works-plugin.html)** but the same steps will apply to **[Paint.NET](https://www.getpaint.net/)**. **[Photoshop](https://www.adobe.com/products/photoshop.html)** will ask for a prompt like this. It's self-explanatory but I'll describe it in a little more detail.

![tut9](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut9.png)

Checking the field here will load the different mipmap levels into **separate layers** in the image editor. You can do this however you need to keep in mind that when you are making changes to one layer you will need to do the same to each different mipmap layer which can get tedious. **For convenience and ease, I recommend leaving this off as we can generate mipmaps from the single texture later when saving, which is the next step.**

**Note:** if you are editing an alpha texture it will ask if you want to load it in the alpha channel. **Leave this off** as the alpha channel will just be read as transparency by default in **[Photoshop](https://www.adobe.com/products/photoshop.html)**.

## Step 6/9 - Saving the modified Image in your editing software (IMPORTANT)

**In this case, we are still using [Photoshop](https://www.adobe.com/products/photoshop.html) with [this DDS plugin](https://software.intel.com/content/www/us/en/develop/articles/intel-texture-works-plugin.html) but the information I will describe here can be applied to a different image editing software with DDS support.**

Worth noting here that the modified image here is the same resolution as the original.

@@ -94,40 +84,43 @@ Minor note: If your application is giving an error indicating that it can't save

![tut10](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut10.png)

## Step 7/9 - Opening the Texture Mod Tool application and viewing/verifying the texture

Once we have our final modified texture, we will return to the application to view and verify the texture. We can see that the image dimensions are the same, the format is also the same and along with that the texture has mipmaps as well. The mipmap count did grow compared to the original however this is okay, the important thing here is that the image has mipmaps.

![tut11](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut11.png)

## Step 8/9 - Converting the textures back into a D3DTX format

After we finished verifying our texture data and information we can now start the conversion process of putting these back into their native **.d3dtx** format. Simply click the **Convert To D3DTX** button in the **Textures Directory** section.

![tut12](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut12.png)

 **Once we click the Convert To D3DTX button in the Textures Directory section it will ask you what folder to save the modified textures into.** Simply select an empty folder and click **Select Folder**. Once again as of v1.0.0 the application will freeze for a moment as it begins to convert the textures into a **.d3dtx** format.

![tut13](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut13.png)

## Step 9/9 - Conversion Complete

Once the conversion is complete, the application will automatically open file explorer to the folder in which your modified textures were saved to. In this case, we can see all of our converted textures and I have selected the main texture that I modified.

![tut14](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut14.png)

# [Part 2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))

**This next section is where we will sort of split off because how one would go about this can vary. You can still follow along just by clicking **[Part 2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))**, but there are two ways we can now integrate this modified texture back into the game.**

Main Route 1
- **[Part 2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))** describes how to do it where we use **[Telltale Script Editor](https://github.com/Telltale-Modding-Group/Telltale-Script-Editor)** to build the textures into a custom .ttarch2 archive, and this can either added in manually or can be added in using the **[Telltale Mod Launcher](https://github.com/Telltale-Modding-Group/TelltaleModLauncher)**. **We recommend it this way** as it's non-destructive to the game files, and distribution of this mod is made much easier as the **[Telltale Script Editor](https://github.com/Telltale-Modding-Group/Telltale-Script-Editor)** builds the final mod into a .zip file that is compatible with the **[Telltale Mod Launcher](https://github.com/Telltale-Modding-Group/TelltaleModLauncher)**.

Alternative Route 1
- With the D3DTX files you can actually drop them in the Game Directory Archives/Packs folder that contain the .ttarch2 archives and the game will actually see it and load it up.

Alternative Route 2
- This is the old way of doing it, it's more complicated and destructive but it's another way of doing it, it involves using **[ttarchext](http://aluigi.altervista.org/papers.htm#ttarchext)** which is a console application and will require you to manually extract the archive, replace the texture file, repack the archive, and replace the original archive with the modified one. This is an acceptable option, However, as stated, it is destructive to the game files and distribution would require you to send the whole archive which can get difficult fast as txmesh archives happen to be the biggest ones in size in the game files.

**So on to **[Part 2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))** where we build this texture into a mod ready file!**

For the record, the final result after integrating it into The Walking Dead Definitive Edition (and **[Part 2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D--How-to-make-a-Texture-Mod-(Part-2))** will describe how to get the mod built/tested/ready).

![result](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/result.png)