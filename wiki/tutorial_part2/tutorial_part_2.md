## [Home](/wiki/home.md)

# How to use Telltale Texture Mod Tool

#### Welcome to the second part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to use the Telltale Texture Mod Tool! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series.
#### [Make sure you read the application guide before we begin!](/wiki/application_guide/application_guide.md)
#### [Make sure you read the prelude before we begin!](/wiki/articles/tutorial_prelude.md)

#### We use **Telltale Texture Mod Tool** for this part of the tutorial, so make sure to download and extract it.
---
## Step 1/4 - Launching Telltale Texture Mod Tool
Run the **Telltale_Texture_Mod_Tool.exe**. It should look like this:
![p2_1](/wiki/tutorial_part2/p2_1.png)

---
## Step 2/4 - Opening your Texture Folder
Click on **Open**, it will open a folder browser.
Select the folder where you extracted your **D3DTX** textures and click on **Select Folder**.

In my case when I open the folder, it will look like this. If not, make sure that you have selected the right folder.
![p2_2](/wiki/tutorial_part2/p2_2.png)

> Ignore that the "Convert Options" combobox showing "d3dtx -> dds". It is a minor GUI bug, the option only shows when you select directories only.
---
## Step 3/4 - Converting textures to DDS

Once you select your folder, the **File Browser** will be populated with your **D3DTX** files. Selecting them will show you are preview on the right side of the app. 
Now, let's convert them to **DDS**.
Stick to the **Default** option if the game is from **2013** and onwards. If not, you have to choose some of the **Legacy Versions**. More information [below](/wiki/tutorial_part2/tutorial_part_2.md#legacy-versions).

> Tip: If you want to save the **DDS** files somewhere else, click on the **Choose Output Directory** checkbox.
> Tip: If you want to convert a whole folder of textures, select the parent directory of that folder, select it and it should have more convert options (for e.g. dds -> d3dtx).

For now, we will only click on **Convert**. 
> Warning: Once the application finishes converting the **D3DTX** textures, it will generate a **DDS** file and a **JSON** file. **DO NOT DELETE THE JSON FILE!**

![p2_3](/wiki/tutorial_part2/p2_3.png)

---
## Step 4/4 - Verifying the DDS texture

Make sure you the image properties between the **D3DTX** and **DDS** file match **(Width, Height, Surface Format, Mipmaps)**.
And you are done! You can now start editing your **DDS** file!
> Tip: You can right click on the file and choose **Open File Explorer** from the context menu. It will open the folder where you have stored the file.

> Note: Remember what the image properties show you. It will be important for Part 3.

![p2_4](/wiki/tutorial_part2/p2_4.png)

## Legacy Versions
The conversion process works in similar ways, but this time you have to "**guess**" the correct Legacy Version. If you **do not**, an error will probably appear OR the texture will look corrupted.

Currently verified supported titles:
| Legacy Version | Title |
| --- | --- |
| Legacy Version 1 | The Walking Dead (2012)
| Legacy Version 2 | Jurassic Park (2011)
| Legacy Version 3 | Back to the Future: The Game (2010)

More can come in the future!

![p2_l1](/wiki/tutorial_part2/p2_l1.png)


# Congratulations, you can now convert D3DTX textures and edit their DDS variants! [You can now go to Part 3!](/wiki/tutorial_part3/tutorial_part_3.md)
