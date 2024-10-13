
## [Home](/Docs/home.md)

# How to make a put a D3DTX inside a TTARCH

> Warning: This guide is outdated as of 2024. Telltale Script Editor has received support for newer titles. Additionally this guide uses the original wiki for example.
#### Welcome to the alternative guide on how to put your new textures inside the archives. **Yes, a lot to read and seemingly a lot of steps but it's not hard, trust me. I just detailed each step as much as I could.**

#### Before we begin, you need to make sure you install **[Telltale Script Editor](https://github.com/Telltale-Modding-Group/Telltale-Script-Editor)** as we will use this to build our mod file. As a plus you can also keep adding to this mod file we create if you want to modify more textures.


#### **In addition, we also recommend you install [Load Any Level](https://github.com/droyti/LoadAnyLevel)**. This mod makes it easy to iterate and test things in the game much faster if you want to see your changes much quicker. Look under the **Usage** section on the main page to learn how to boot directly to a level.

## Step 1/6 - Create a Telltale Script Editor project

Open the Telltale Script Editor.

![tse1](/Docs/tutorial-screenshots/tse1_old.png)

Navigate to **File** and click **New Project**. It will ask for you to define where you want your **project folder** to be. After defining a project path it will then ask you for a **project name** and who the **author** is (obviously you!).

![tse2](/Docs/tutorial-screenshots/tse2_old.png)

## Step 2/6 - Opening a .ttarch2 game archive

Once we have created our project you should see something similar to what is shown below.

![tse3](/Docs/tutorial-screenshots/tse3_old.png)

The next step now is to open a **.ttarch2** archive. Go under **File > Open > .TTARCH2 Archive**. This will open a file browser, you need to navigate to your games archives folder.

![tse4](/Docs/tutorial-screenshots/tse4_old.png)

## Step 3/6 - Selecting a _txmesh .ttarch2 archive

Since we are working with **The Walking Dead Definitive Edition** we will navigate to the main game directory, opening the **Archives** folder. In this folder, you will select a **.ttarch2** archive with the suffix **_txmesh**.

A **_txmesh** archive means that this archive contains textures and meshes. Worth noting that each episode has a different **_txmesh** archive. If you wish to open multiple **_txmesh** archives and do a big texture mod your free to do so, but since this is a tutorial we will keep it simple. 

**In this case, I am selecting the _txmesh archive for TWD Season 2 Episode 1 because it's where I originally extracted this texture from.** We do this so that Telltale Script Editor can build the archive and set it to where when the episode is loaded in, the mod archive and the modified files in the archive override the original ones (not replace them, override. Since new archives are treated as patch files).

![tse5](/Docs/tutorial-screenshots/tse5_old.png)

## Step 4/6 - Adding in the modified .d3dtx texture

Once you have opened a **_txmesh** archive you should see the folder appear in the **project hierarchy view**.

![tse6](/Docs/tutorial-screenshots/tse6_old.png)

Open the project directory in File Explorer and navigate to the extracted folder as illustrated below.

![tse7](/Docs/tutorial-screenshots/tse7_old.png)

Open the folder and then move or copy your modified **.d3dtx** and paste it into the folder.

![tse8](/Docs/tutorial-screenshots/tse8_old.png)

Once you have done this, return to the application and you should see the texture appear in the **project hierarchy view** in that folder.

## Step 5/6 - Building the Mod

Once you have done step 5, return to the application and you should see the texture appear in the **project hierarchy view** in that folder. The next step now is to build the project into a mod by navigating to **Project > Build**.

**(OPTIONAL READING)** Worth noting there is also an option to **Build and Run**. This isn't required but it's a useful feature of the editor if you want to be able to iterate on this mod fast. It will ask for you to specify where your game executable is and then it will build the project, automatically integrate the mod into the game files, and launch the game for you. It will remember the game executable location every time you do this process in the editor, however closing or restarting the application will require you to select the game executable again.

![tse9](/Docs/tutorial-screenshots/tse9_old.png)

Assuming you've followed the steps, the editor will build the Mod into a Builds folder with the zip file in it and this is shown in the **project hierarchy view**.

![tse10](/Docs/tutorial-screenshots/tse10_old.png)

## Step 6/6 - Installing

Once again, this can vary depending on how you wish to go. You can either install the mod manually by opening the zip file and dumping ALL of its contents into the game's Archives directory as illustrated below.

![install1](/Docs/tutorial-screenshots/install1_old.png)

Or, since it's built through the [Telltale Script Editor](https://github.com/Telltale-Modding-Group/Telltale-Script-Editor). It's automatically compatible with the **[Telltale Mod Launcher](https://github.com/Telltale-Modding-Group/TelltaleModLauncher)** and you can install the .zip file through the launcher easily.

![install2](/Docs/tutorial-screenshots/install2_old.png)

## Finished!

![result](/Docs/tutorial-screenshots/result_old.png)