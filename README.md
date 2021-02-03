# DDS-D3DTX-Converter

### DISCLAIMER

This is currently being developed and tested only with ***"The Walking Dead Telltale Definitive Series"*** textures.

### What is this?

**This is an experimental application** containing a WIP console script application for converting **.d3dtx textures (Telltale Tool Textures)**, to standard **.dds (Direct Draw Surface)**. 

It can also do the opposite and can be used to do texture mods for a Telltale Tool Game, which is the final goal.

### When will you release it?

**I will release a build (exe) when it's ready**, I will turn it into a full on custom application complete with a GUI for better useability and release builds of said tool. For the time being if you wish to try this yourself, look under the "How to Use" section of this page. It's worth mentioning that this is very new and bleeding edge. Expect bugs or issues as everything in the script is still very new.

It's worth noting that I don't have the script for the .d3dtx to .dds conversion finished just yet, it will be made and complete but the goal currently is to understand the .d3dtx format to the best of my ability. Once that is complete I can focus on converting said .dds to .d3dtx which should be simple and straight forward once the format is understood well enough. *(I will also write another page explaining the extraction/building process and the d3dtx format for those intrested as well. Information is power!)*

**More information will be written and explained at a later date.** Currently focused on the functionality of the script but also understanding and extracting the data from the .d3dtx format itself which is the first goal, the second being then converting a dds into a d3dtx. *For the life of me I cannot fathom how there are already a few .d3dtx converters out there, (which most don't have their source revealed) but no one has bothered to do one in reverse, hence also why I am writing this script*.

### How to use?

**This is likely to change, and I will not put out a build (exe) with a GUI until the script becomes more advanced and stable enough (The reason being that a console app is much faster for iteration and testing)**. But if you wish to try this yourself need to download the IDE (Visual Studio Community 2020) and download this project as well and open it with the IDE (the .sln project file).

1. Run the application through the editor and it will open up a console window. 
2. It will ask you to indentify a path for where your .d3dtx textures are stored. Make sure you have a folder with only the .d3dtx textures you want to convert, enter the folder path in the console.
3. It will then ask you to idenfity a path where the converted textures will be stored, make sure this is an empty folder. Enter the folder path in the console.

The script will then run through and convert all textures to be found in the source folder. 

**NOTE:** Not all .d3dtx textures will work or show up properly and might even look corrupted after conversion. This is simply because once again, this is a very new script and I have yet to support more d3dtx texture variants. For the most part, any regular diffuse/albedo textures should work.
