## How to find and extract textures 

**Welcome to the first part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to find and extract the textures needed for your mods!**

## [Make sure you read the prelude before beginning. It contains important information.](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/wiki/%5BTutorial%5D-How-to-make-a-Texture-Mod-(Prelude))

**Now with that out of the way let's begin.**
We use Telltale Explorer for this part of the tutorial. Make sure to download it.

## Step 1/9 - Locating the textures
Telltale store their files in **.ttarch/.ttarch2** files also know as **Telltale archives**. We open these archives using Telltale Explorer. Now the question remains - which? Luckily, Telltale has good naming conventions. 

Most textures can be found in the archives ending with **_txmesh**. Additionally, in front of **_txmesh** there is useful data like episode number (for e.g. 202 would mean season 2 episode 2) or Menu, which is related to ui menu content. 

Here is an example of how TWD:DE directory looks like and where .d3dtx files are stored.

##### Note, textures file can be found in other archives as well, but I am gonna leave it for you to explore them. 

## Step 2/9 Opening the archives
After you have an idea where to look, run TelltaleExplorer.exe.
It should look like this [image of explorer]


Click on Open and find the game's directory. 

Select your desired archive and open it. 

Click on Open and find the game's directory where the archives are stored.


## Step 3/9 - Finding the right textures
This looks like a mess, doesn't it? And it is, but it's an organised one.

Typically *(unless you are an experienced texture artist)* you would only really need to be modifying the **[diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)**. 

**[Diffuse textures](https://www.reallusion.com/iclone/help/iclone3/15_multiple_channel_texture_mapping/types_of_maps.htm)** are just the raw material/model colors and these textures typically have no suffix after them in their file names like _spec, _ink, _detail, _nm, etc. **For example, sk54_lee_head.d3dtx is a diffuse texture, however sk54_lee_head_spec.d3dtx is a specular texture.** These suffixes indicate and deal with other material properties like the **[specularity](https://www.modding-forum.com/guide/17-diffuse-specular-and-normal-maps/)** of a material, the **[normal/bump map](https://en.wikipedia.org/wiki/Normal_mapping)** of the material, or the ink/black lines on a material.



![tut1](./wiki/tutorial-screenshots/tut1_old.png)

![tut2](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut2.png)

![tut3](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut3.png)

## Step 4/9 - Extracting the textures

When you select the texture you want to edit, click on **Save** -> **Save as Raw**. 
Choose a folder on your preference (it's a good idea to be empty) and save it.

### Congratulations, you can now locate and extract the right textures!

### Tips and tricks

Telltale Explorer has some good QoL features. 

### The view button acts like a filter. Don't want to see .d3dmesh files? Click "View" and choose .d3dtx.

### Want to save all files? 
Clicking on Save All Files will give you some options.
- Save all (raw dump) - save all files from the archive
- Save all visible files (raw dump) - like Save all (raw dump), but with an applied filter

### Want to find a specific texture? 
Use the search option in the top right corner. 
##### Note: it doesn't work with filters.
![tut4](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut4.png)


![tut5](https://github.com/Telltale-Modding-Group/DDS-D3DTX-Converter/blob/main/tutorial-screenshots/tut5.png)

