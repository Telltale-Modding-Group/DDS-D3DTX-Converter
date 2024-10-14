## [Home](/Docs/home.md)

# How to edit using `Paint.NET` and convert back to `D3DTX`

#### Welcome to the third and final part of the tutorial. In this section we focus on how to use `Paint.NET` and the `Telltale Texture Tool`! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series.

> [!IMPORTANT]
> Make sure you have read the [application guide](/Docs/application_guide.md)!
> 
> Make sure you have read the [prelude](/Docs/articles/tutorial_prelude.md)!
> 
> We use `Paint.NET` and `Telltale Texture Tool v2.5.0` for this part of the tutorial, so make sure you have them downloaded and installed.
---

## Step 1/4 - Editing the outputted texture

Open your converted file in `Paint.NET` or another editor by your choice.

Edit it however you like, the freedom is yours. In my case, I replaced the 'D' with an oddly suspicious astronaut.

If you try to open `DDS` or `TGA` with mips, `GIMP` and `Photoshop` will ask you if you want to load them or not. I usually recommend **not** doing that, because you will generate new ones.

![p3_1](/Docs/tutorial_part3/p3_1.png)

---

## Step 2/4 - Saving the new texture

On `Paint.NET` go to `File` -> `Save As` and choose your preferred file type. 

For `PNG`, the options do not matter, however you will have to make sure you have checked some checkboxes in `Advanced Options`.

For `DDS`:

A window with advanced options will appear. The most important options are the surface format and the mip generation. I would personally stick to the following formats:
- `BC1 sRGB` or `BC1 Linear` (depending on the original texture's color space) for textures without any transparency.
- `BC3 sRGB` or `BC3 Linear` (depending on the original texture's color space) for textures with transparency. 

`BC1` and `BC3` are compression formats which reduce the size of your image. However, the texture quality will drop. In that case use:
- `R8G8B8A8 Linear` or `R8G8B8A8 sRGB` (depending on the original texture's color space). This format works for every single Telltale game and platform.

Enable mip generation if the original texture had them.

Save the file the same directory where you store the `JSON` files.

> [!TIP]
> This [article](/Docs/articles/surfaces.md) contains additional information regarding surface formats.

> [!NOTE]
> Upscaling/downscaling textures **is possible**.

In this case I select `BC1 (Linear, DXT1)` and generate mips.

![p3_2](/Docs/tutorial_part3/p3_2.png)

---
## Step 3/4 - Converting the modified texture into D3DTX
> [!WARNING]
> After you saved the texture, make sure it is in the same place with its `JSON` file. Otherwise, an error would appear.

Let's go back to the Telltale Texture Tool and reselect our new textures.

#### DDS
![p3_3](/Docs/tutorial_part3/p3_3.png)
#### PNG
![p3_3](/Docs/tutorial_part3/p3_4.png)

Cool, the textures look OK. 

For `PNG`, Select `Advanced Options` -> `Generate Mips` -> `Automatic`. It should look like this:
###
![p3_4](/Docs/tutorial_part3/p3_5.png)


Alright, let's convert!
###
![p3_4](/Docs/tutorial_part3/p3_6.png)

###
Perfect! It looks like it was saved correctly! Let's put it in the game!

---
## Step 4/4 Putting the texture in-game 

Copy the **modified D3DTX** file and put it inside the game's directory. 
> [!NOTE]
> The game will prioritize loading the new files rather than the ones in the game archives. 

> [!TIP] 
> If you want to put the texture inside a TTARCH, check out this somewhat-outdated [wiki](/Docs/articles/embed_ttarch.md).

Here's how mine looks:

![p3_5](/Docs/tutorial_part3/p3_7.png)

# Let's test it!

[<img src="/wiki/tutorial_part3/ingame_sh1.png" width=800>](/Docs/tutorial_part3/ingame_sh1.png) [<img src="/wiki/tutorial_part3/ingame_sh2.png" width=800>](/Docs/tutorial_part3/ingame_sh2.png)
[<img src="/wiki/tutorial_part3/ingame_sh3.png" width=800>](/Docs/tutorial_part3/ingame_sh3.png)

It works! You may notice that the 'D' outline is still present on the hat. That is because there is another texture called **sk56_clementine200_hair_detail.d3dtx** containing that outline. If you want to get rid of the 'D', you have to edit that texture as well.
> ##### Read [here](/Docs/articles/textures.md#navigation) about all texture types.

### That's it! If you encounter any issues either report them by or read this page about troubleshooting, which utilizes Telltale Inspector. 
# Happy Modding!
----

## If you want to practice, find the detail texture that I mentioned above. Try to apply all the used steps from the whole tutorial. Here are my results:

| Original Detail Texture             |  Edited Detail Texture
:-------------------------:|:-------------------------:
![fr1](/Docs/tutorial_part3/final_result1.png) | ![fr2](/Docs/tutorial_part3/final_result2.png)

### Final result:
[<img src="/Docs/tutorial_part3/final_result3.png" width=800>](/Docs/tutorial_part3/final_result3.png) [<img src="/Docs/tutorial_part3/final_result4.png" width=800>](/Docs/tutorial_part3/final_result4.png)
