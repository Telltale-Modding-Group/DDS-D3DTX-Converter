## How to find and extract textures 

**Welcome to the first part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to find and extract the textures needed for your mods!**

## [Make sure you read the prelude before beginning. It contains important information.](/wiki/tutorials/tutorial_prelude.md)

#### We use **Telltale Explorer** for this part of the tutorial. Make sure to download it.

## Step 1/4 - Locating the textures
Telltale store their files in **TTARCH/TTARCH2** files also know as **Telltale archives**. We open these archives using Telltale Explorer. Now the question remains - which? Luckily, Telltale has good naming conventions. 
Check [this](/wiki/tutorials/textures.md#where-are-the-textures-stored) article. 

Here is an example of how TWD:DE directory looks like and where .d3dtx files are stored.

## Step 2/4 Opening the archives
After you have an idea where to look, run TelltaleExplorer.exe.
It should look like this [image of explorer]

Click on Open and find the game's directory. 

Select your desired archive and open it. 

Click on Open and find the game's directory where the archives are stored.

## Step 3/4 - Finding the right textures
Check [this](/wiki/tutorials/textures.md#how-to-recognize-the-type-of-the-texture-and-where-its-used) article. For this example let's take clemblaghah

![tut1](./wiki/tutorial-screenshots/tut1_old.png)

## Step 4/4 - Extracting the textures

When you select the texture you want to edit, click on **Save** -> **Save as Raw**. 
Choose a folder on your preference (it's a good idea to be empty) and save it.

# Congratulations, you can now locate and extract the right textures!

### Tips and tricks

Telltale Explorer has some good QoL features. 

#### Don't want to see .d3dmesh files? The view button acts like a filter. 
Click "View" and choose .d3dtx.

#### Want to save all files? 
Clicking on Save All Files will give you some options.
- **Save all (raw dump)** - save all files from the archive
- **Save all visible files (raw dump)** - like Save all (raw dump), but with an applied filter

#### Want to find a specific texture? 
Use the search option in the top right corner. 
> Note: it doesn't work with filters.
