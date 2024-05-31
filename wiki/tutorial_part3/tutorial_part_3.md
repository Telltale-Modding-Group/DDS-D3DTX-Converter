## [Home](/wiki/home.md)

# How to edit using Paint.NET and how to convert back to D3DTX

#### Welcome to the third part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to use Paint.NET and the Telltale Texture Mod Tool! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series.
#### [Make sure you read the application guide before we begin!](/wiki/application_guide.md)
#### [Make sure you read the prelude before we begin!](/wiki/articles/tutorial_prelude.md)

#### We use **Paint.NET and Telltale Texture Mod Tool** for this part of the tutorial, so make sure you have them downloaded and installed.
---

## Step 1/4 - Editing the DDS 

Open the file in Paint.NET or the editor you prefer to work with.

Edit it however you like, the freedom is yours. In my case, I replaced the 'D' with an oddly suspicious astronaut.

If you try to open it with GIMP or Photoshop, it will ask you if you want to load the mipmaps or not. I usually recommend **not** doing that, since:
- You will generate new ones.
- It will add a lot of layers in your editor, which you do not need.

![p3_1](/wiki/tutorial_part3/p3_1.png)

---

## Step 2/4 - Saving the DDS

On Paint.NET go to **File** -> **Save As** -> Select **Direct Draw Surface (DDS)** option from below.
Try to save it in the same directory with its **JSON**.

Now comes the fun part - in every single DDS editor a similar window will appear. The most important options are the surface format and whether or not you should generate mipmaps.


> For surface format you have 2 options: 
> - Save it as the same one displayed on your image properties. 
> - Use this [wiki](/wiki/articles/surfaces.md) to determine which option to choose. Changing surface formats **is possible** with the latest converter.

> Warning: If the original D3DTX had more than 1 mipmap, **select** the **Generate Mip Maps** option. Do not worry if the mipmaps are less or more than the original, that's **OK**.

> Note: Upscaling/downscaling textures **is possible**.

In this case I will choose **BC1 (Linear, DXT1)**, also known as simply **BC1** or **DXT1**, for surface format and check **Generate Mip Maps** to...generate mipmaps.

![p3_2](/wiki/tutorial_part3/p3_2.png)

---
## Step 3/4 - Converting the modified DDS into D3DTX

> Warning: After you saved the texture, make sure it is in the same place with its **JSON** file. If not, an error will appear that the **JSON** was not found.

> Warning: Verify that the preview image looks like the one you saved in the Telltale Texture Mod Tool.

Cool, it looks like the **DDS** was saved correctly. 
Now select **d3dtx** from the **Convert Options** and convert.

![p3_3](/wiki/tutorial_part3/p3_3.png)

##
Let's see the **D3DTX**.
###
![p3_4](/wiki/tutorial_part3/p3_4.png)

###
Perfect! It looks like it was saved correctly! Let's put it in the game!

---
## Step 4/4 Putting the texture in-game 

Copy the **modified D3DTX** file. 
Find the game's directory and put it inside. 
> Note: The game will prioritize loading the new files rather than the ones in the game archives. 
> Optional: If you want to put the texture inside a TTARCH, check out this somewhat-outdated [wiki](/wiki/articles/embed_ttarch.md).

It should look something like this.

![p3_5](/wiki/tutorial_part3/p3_5.png)

# Let's test it!

![alt text](/wiki/tutorial_part3/ingame_sh1.png)

![alt text](/wiki/tutorial_part3/ingame_sh2.png)

![alt text](/wiki/tutorial_part3/ingame_sh3.png)

It works! You may notice that the 'D' outline is still present on the hat. That is because there is another texture called **sk56_clementine200_hair_detail.d3dtx** containing that outline. If you want to get rid of the 'D', you have to edit that texture as well.
> ##### Read [here](/wiki/articles/textures.md#navigation) about all texture files.

### That's it! If you encounter any issues either report them by or read this page about troubleshooting, which utilizes Telltale Inspector. 
# Happy Modding!
----

## If you want to practice, find the detail file that I mentioned above. Try to apply all the used steps from the whole tutorial. Here are my results:

### Original texture:
![fr1](/wiki/tutorial_part3/final_result1.png)

### Edited texture:
![fr2](/wiki/tutorial_part3/final_result2.png)

![fr3](/wiki/tutorial_part3/final_result3.png)

![fr4](/wiki/tutorial_part3/final_result4.png)
