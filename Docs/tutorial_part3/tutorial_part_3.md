## [Home](/Docs/home.md)

# How to edit using Paint.NET and convert back to D3DTX

#### Welcome to the third part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to use Paint.NET and the Telltale Texture Tool! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series.

> [!IMPORTANT]
> Make sure you have read the [application guide](/Docs/application_guide.md)!
> 
> Make sure you have read the [prelude](/Docs/articles/tutorial_prelude.md)!
> We use `Paint.NET` and `Telltale Texture Tool v2.5.0` for this part of the tutorial, so make sure you have them downloaded and installed.
---

## Step 1/4 - Editing the outputted texture

Open your converted file in Paint.NET or another editor by your choice.

Edit it however you like, the freedom is yours. In my case, I replaced the 'D' with an oddly suspicious astronaut.

If you try to open `DDS` or `TGA` with mips, `GIMP` and `Photoshop` will ask you if you want to load them or not. I usually recommend **not** doing that, because, you will generate new ones.

![p3_1](/Docs/tutorial_part3/p3_1.png)

---

## Step 2/4 - Saving the new texture

On Paint.NET go to `File` -> `Save As` and choose your preferred file type. 

For `PNG`, the options do not matter, however you will have to enable one option from the Telltale Texture Tool.

For `DDS`:

A window with advanced options will appear. The most important options are the surface format and the mip generation. You can select the surface format by your choice with one caveat: or
The mip generation depends on if the original texture had them or not.

Save the file the same directory where you store the `JSON` files.

> For surface format you have 2 options: 
> - Save it as the same one displayed on your image properties. 
> - Use this [wiki](/Docs/articles/surfaces.md) to determine which option to choose. Changing surface formats **is possible** with the latest converter.

> Warning: If the original D3DTX had more than 1 mipmap, **select** the **Generate Mip Maps** option. Do not worry if the mipmaps are less or more than the original, that's **OK**.

> Note: Upscaling/downscaling textures **is possible**.

In this case I will choose **BC1 (Linear, DXT1)**, also known as simply **BC1** or **DXT1**, for surface format and check **Generate Mip Maps** to...generate mipmaps.

![p3_2](/Docs/tutorial_part3/p3_2.png)

---
## Step 3/4 - Converting the modified DDS into D3DTX

> Warning: After you saved the texture, make sure it is in the same place with its **JSON** file. If not, an error will appear that the **JSON** was not found.

> Warning: Verify that the preview image looks like the one you saved in the Telltale Texture Mod Tool.

Let's go back to the Telltale Texture Mod Tool and reselect the DDS. 

![p3_3](/Docs/tutorial_part3/p3_3.png)

##
Cool, it looks like the **DDS** was saved correctly. Now select **d3dtx** from the **Convert Options** and convert. Let's see the **D3DTX**.
###
![p3_4](/Docs/tutorial_part3/p3_4.png)

###
Perfect! It looks like it was saved correctly! Let's put it in the game!

---
## Step 4/4 Putting the texture in-game 

Copy the **modified D3DTX** file and put it inside the game's directory. 
> Note: The game will prioritize loading the new files rather than the ones in the game archives. 
> Optional: If you want to put the texture inside a TTARCH, check out this somewhat-outdated [wiki](/Docs/articles/embed_ttarch.md).

It should look something like this.

![p3_5](/Docs/tutorial_part3/p3_5.png)

# Let's test it!

[<img src="/wiki/tutorial_part3/ingame_sh1.png" width=800>](/Docs/tutorial_part3/ingame_sh1.png) [<img src="/wiki/tutorial_part3/ingame_sh2.png" width=800>](/Docs/tutorial_part3/ingame_sh2.png)
[<img src="/wiki/tutorial_part3/ingame_sh3.png" width=800>](/Docs/tutorial_part3/ingame_sh3.png)

It works! You may notice that the 'D' outline is still present on the hat. That is because there is another texture called **sk56_clementine200_hair_detail.d3dtx** containing that outline. If you want to get rid of the 'D', you have to edit that texture as well.
> ##### Read [here](/Docs/articles/textures.md#navigation) about all texture files.

### That's it! If you encounter any issues either report them by or read this page about troubleshooting, which utilizes Telltale Inspector. 
# Happy Modding!
----

## If you want to practice, find the detail texture that I mentioned above. Try to apply all the used steps from the whole tutorial. Here are my results:

| Original Detail Texture             |  Edited Detail Texture
:-------------------------:|:-------------------------:
![fr1](/Docs/tutorial_part3/final_result1.png) | ![fr2](/Docs/tutorial_part3/final_result2.png)

### Final result:
[<img src="/Docs/tutorial_part3/final_result3.png" width=800>](/Docs/tutorial_part3/final_result3.png) [<img src="/Docs/tutorial_part3/final_result4.png" width=800>](/Docs/tutorial_part3/final_result4.png)
