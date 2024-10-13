## [Home](/Docs/home.md)

This page has information regarding **Telltale Archives (TTARCH/TTARCH2)**.
## [WIP]

A **TTarch** file is a game data archive used by a Telltale Games video games. It contains game assets, such as scripts, animations, images, and sounds. Telltale games load assets from TTARCH files during gameplay.
> For more technical information, check out [this page](https://github.com/Telltale-Modding-Group/ttarch-docs).

Recognizing what each archive stores will make modding easier.
Let's start by separating the name by underscores ('_').
> Example: WalkingDead_pc_WalkingDead201_txmesh turns into [WalkingDead, pc, WalkingDead202, txmesh].

The first few words describe in what game it is used and where it is being used.
Some of the categories include:
| Name | Description |
| --- | --- |
| **Boot** | These files are used during the boot process of the game.
| **Menu** | These files are used in the game's menu.
| **Project** | [WIP] |
| **[GameName][3-digit number]** | The files are used in a particular episode of a game. The first digit of the number represents the season, the latter 2 digits represent the episode number. |
| Other uncommon types | Usually for shaders, effects or localization-related files. |
> Example 1: **WalkingDead**\_pc\_**WalkingDead201**_txmesh stores the textures for episode 1 of The Walking Dead Season 2.
> Example 2: **WDC**\_pc\_**UISeasonM**_txmesh contains UI textures for The Walking Dead Michonne in The Walking Dead Collection.

The last word describes what files are stored there. The texture related ones are the following:
| Name | Description |
| --- | --- |
| **txmesh** | <ul><li>~90% of the textures are stored there.</li><li>Contains most meshes. (.d3dmesh)</li><li>Stands for "texture and mesh".</li></ul>
| **compressed** | <ul><li>~5% of the textures are stored there.</li><li>Contains other varying file types.</li></ul>
| **anichore** | Contains chore files with animations.
| **ms** | Contains sound files.
| **uncompressed** | Contains sound files.
| **lipsync**  | Contains lip-syncing animations and chores.
| **chore** | Contains chore files.
| **data** | Contains various file formats - scripts, scenes, props, dialog/speech data, fonts, skeletons (models) and more.
| **all** |<ul><li>Contains varying files. A combination of chore, ms, data and txmesh.</li><li>Contains some texture files.</li></ul>

### What are the known Telltale file types?
Telltale has many different files.
| Name | Description |
| --- | --- |
| **lenc/lua** | <ul><li>Contains compiled lua scripts.</li><li></li></ul>
| **anm** | Animations.
| **wav** | <ul><li>Audio files. </li> <li>Some may cannot be opened with regular software.</li></ul>
| **d3dmesh** | Mesh/3D model files.
| **skl** |  Contains skeletal structure for meshes.
| **d3dtx** | <ul><li>Texture files.</li> <li> Used on d3dmesh.</li></ul>
| **prop** | <ul><li>Contains references and other information used internally by Telltale's engine to create objects.</li> <li>May contain all sorts of different objects, ranging from characters and weapons to UI elements to lighting.</li></ul>
| **scene** | <ul><li>A special type of **prop** file. .</li> <li>Contains all objects in a given scene..</li></ul>
| **chore** | Action sequences used in cutscenes. 
| **dlg** | [WIP]
| **landb** | Dialog/speech information.
| **font** | Fonts. Contains a D3DTX file inside it.
| **bank** | 3rd-party format used by FMOD Studio, used for music.
| **fx** | Effects.
| **probe** | Lighting.
| **enl** | Lighting.
| ** 
| **sprite** | Particles.
| **tmap** | Expressions.
| _ |  [WIP]

#### If you want to edit or use these files in-game, check out [Mawrak's Telltale Custscene Editor](https://github.com/Telltale-Modding-Group/Telltale-Script-Editor-Tweaks) and its wiki.

#### Common question: Can I use files from other games?
