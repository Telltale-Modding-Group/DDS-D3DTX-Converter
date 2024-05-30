## [Home](/wiki/home.md)

# How to find and extract textures 

#### Welcome to the first part of the tutorial on how to make a texture mod for Telltale's games. In this section we focus on how to find and extract the textures needed for your mods! In this tutorial, we will try to mod Clementine's Season 2 hat in The Walking Dead: The Telltale Definitive Series.
#### [Make sure you read the prelude before we begin!](/wiki/articles/tutorial_prelude.md)

#### We use **Telltale Explorer** for this part of the tutorial, so make sure to download it.
----

### Step 1/4 - Locating the textures
Telltale store their files in **TTARCH/TTARCH2** files also know as **Telltale archives**, which we can open with Telltale Explorer. Now the question remains - which? Luckily, Telltale has good naming conventions. 
Check [this](/wiki/articles/textures.md#where-are-the-textures-stored) article to get to know what each archive contains. For now, let's focus on those which finish with **txmesh**.

Here is an example of how TWD:TTDS directory looks like. 
![p1_1](/wiki/tutorial_part1/p1_1.png)
----

## Step 2/4 Opening the archives
After you somewhat know where to search, run **TelltaleExplorer.exe**.
It should look like this:
![p1_2](/wiki/tutorial_part1//p1_2.png)

Click on **Open** -> **Open File**  at the top left corner and find the game's directory where the archives are stored.
Select your desired archive and open it. In this example we are opening **WDC_pc_WalkingDead201_txmesh.ttarch2**.

![p1_3](/wiki/tutorial_part1/p1_3.png)

Click on **Open** or **double click**.

## Step 3/4 - Finding the right textures
After you opened the archive, it should look like this. 
> Check [this](/wiki/articles/textures.md#how-to-recognize-the-type-of-the-texture-and-where-its-used) section to know how to recognize the files. 

![p1_4](/wiki/tutorial_part1/p1_4.png)

 For this tutorial I am extracting **sk56_clementine200_hair.d3dtx**. 
 It stands for "skeleton height (56), Clementine's model, her hair texture with a hat.
![p1_5](/wiki/tutorial_part1/p1_5.png)

---
## Step 4/4 - Extracting the textures

When you select the texture you want to edit, click on **Save** -> **Save as Raw**. 
Choose a folder on your preference (it is a good idea to be empty) and save it.
> Tip: Don't want to see **.d3dmesh** files? The **View** button acts like a filter. 
Click **View** and choose **.d3dtx**.

> Tip: Want to save all files? 
> Clicking on Save All Files will give you some options.
> - **Save all (raw dump)** - Save all files from the archive.
> - **Save all visible files (raw dump)** - Like Save all (raw dump), but with an applied filter.

> Tip: Want to find a specific texture? 
Use the search option in the top right corner. 
    >> Note: it doesn't work with filters.

![p1_6](/wiki/tutorial_part1/p1_6.png)

# Congratulations, you can now locate and extract the right textures! [You can now go to Part 2!](/wiki/tutorial_part2/tutorial_part_2.md)


