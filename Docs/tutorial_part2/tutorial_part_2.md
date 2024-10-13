## [Home](/Docs/home.md)

# How to use the Telltale Texture Tool

#### Welcome to the second part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to use the Telltale Texture Tool! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series. The same principles apply to ALL GAMES!

> [!IMPORTANT]
> Make sure you have read the [application guide](/Docs/application_guide.md)!
> 
> Make sure you have read the [prelude](/Docs/articles/tutorial_prelude.md)!
> 
> We use `Telltale Texture Tool v2.5.0` for this part of the tutorial, so make sure to download and extract it.
---
## Step 1/4 - Launching Telltale Texture Tool
Run `TelltaleTextureTool.exe`. It should look like this:
![p2_1](/Docs/tutorial_part2/p2_1.png)

---
## Step 2/4 - Opening your Texture Folder
When you click on `Open`, a folder browser will appear.
Select the folder where you extracted your `D3DTX` textures and click on `Select Folder`.

In my case when I open the folder, it will look like this. If not, make sure that you have selected the correct folder.
![p2_2](/Docs/tutorial_part2/p2_2.png)

---
## Step 3/4 - Converting textures to a regular format

Once you select your folder, the `File Browser` will be populated with your `D3DTX` files. You can preview them by selecting them.

Now, let's convert them to our desired format. Select the format from the `To` combobox. I recommend working with either `DDS`, `PNG` or `TGA` file types.

Click on `Convert`. 

> [!TIP]
> If you want to save your converted files somewhere else, click on the `Choose Output Directory` checkbox.

> [!TIP]
> If you want to bulk convert (convert all files in a folder), select the folder (their file types are empty), then select the `From` and `To` options. A message will appear if it is successful or not.

> [!WARNING]
> Once the application finishes converting the `D3DTX` textures, it will generate a `JSON` file. **DO NOT DELETE IT!** It contains important information.

![p2_3](/Docs/tutorial_part2/p2_3.png)

---
## Step 4/4 - Verifying the outputted texture

Click on the outputted image. If you used the `DDS` option, make sure the image properties with the `D3DTX`, like `Width`, `Height`, `Surface Format` and `Mips`.

You are done! You can now start editing your texture file!

> [!TIP]
> You can right click on the file and choose `Open File Explorer` from the context menu. It will open the folder where you have stored the file.

> [!NOTE]
> Remember what the image properties show you. It will be important for Part 3.

### DDS
![p2_4](/Docs/tutorial_part2/p2_4.png)

### PNG - 
![p2_5](/Docs/tutorial_part2/p2_5.png)

> [!IMPORTANT]
> For older games:
>
> The conversion process works in similar ways, but this time you have two options:
> 1. The `Default` option in the combobox means that the converter will try to recognize the format. This option is reliable, but very slow. The `Legacy Console` checkbox will be checked if the texture is from an older console game.
> 2. You have to select the right game in the `Advanced Options`, otherwise you will be greeted by a lot of errors. 

![p2_l1](/Docs/tutorial_part2/p2_l1.png)

# Congratulations, you can now convert D3DTX textures and edit them! [You can now go to Part 3!](/Docs/tutorial_part3/tutorial_part_3.md)
