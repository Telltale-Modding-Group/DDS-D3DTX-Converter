// using System;
// using System.IO;

// namespace TelltaleTextureTool.ProgramDebug
// {
//     public static class ConsoleFunctions
//     {
//         public static void SetConsoleColor(ConsoleColor backgroundColor, ConsoleColor foregroundColor)
//         {
//             Console.BackgroundColor = backgroundColor;
//             Console.ForegroundColor = foregroundColor;
//         }
//     }

//     public static class Program_Shared
//     {
//         // I need a better system.
//         public const int currentMaxD3DTXModes = 4;

//         public static string GetFolderPathFromUser()
//         {
//             string folderPath;

//             //run a loop until the path is valid
//             while (true)
//             {
//                 //get path from user
//                 folderPath = Console.ReadLine();

//                 //check if the path is valid
//                 if (Directory.Exists(folderPath) == false)
//                 {
//                     //notify the user and this loop will run again
//                     Console.WriteLine("Incorrect texture path, try again.");
//                 }
//                 else
//                 {
//                     break; // If it's successful, then break out of the loop
//                 }
//             }

//             return folderPath;
//         }

//         public static string GetFilePathFromUser()
//         {
//             string filePath;

//             // Run a loop until the path is valid
//             while (true)
//             {
//                 // Get the path from user
//                 filePath = Console.ReadLine();

//                 //check if the path is valid
//                 if (Directory.Exists(filePath) == false)
//                 {
//                     //notify the user and this loop will run again
//                     Console.WriteLine("Incorrect texture path, try again.");
//                 }
//                 else
//                 {
//                     break; // If it's successful, then break out of the loop
//                 }
//             }

//             return filePath;
//         }

//         public static int GetFileConversionMode()
//         {
//             int fileConversionMode;

//             // Run a loop until the mode is valid
//             while (true)
//             {
//                 try
//                 {
//                     // Get the mode from user
//                     fileConversionMode = Convert.ToInt32(Console.ReadLine());

//                     if (fileConversionMode < 0 || fileConversionMode > 4)
//                     {
//                         Console.WriteLine("Incorrect file conversion mode, try again. Value must be between 0 and 3.");
//                     }
//                     else
//                     {
//                         break; // If it's successful, then break out of the loop
//                     }
//                 }
//                 catch (Exception)
//                 {
//                     Console.WriteLine("Incorrect file conversion mode, try again.");
//                 }
//             }

//             return fileConversionMode;
//         }

//         public static int GetOthersConversionMode()
//         {
//             int othersConversionMode;

//             // Run a loop until the mode is valid
//             while (true)
//             {
//                 try
//                 {
//                     // Get the mode from user
//                     othersConversionMode = Convert.ToInt32(Console.ReadLine());

//                     if (othersConversionMode < 0 || othersConversionMode > 3)
//                     {
//                         Console.WriteLine("Incorrect others file conversion mode, try again. Value must be between 0 and 3.");
//                     }
//                     else
//                     {
//                         break; // If it's successful, then break out of the loop
//                     }
//                 }
//                 catch (Exception)
//                 {
//                     Console.WriteLine("Incorrect others file conversion mode, try again. Value must be between 0 and 3.");
//                 }
//             }

//             return othersConversionMode;
//         }

//         public static int GetD3DTXConversionMode()
//         {
//             int d3dtxConversionMode;

//             // Run a loop until the mode is valid
//             while (true)
//             {
//                 try
//                 {
//                     // Get the mode from user
//                     d3dtxConversionMode = Convert.ToInt32(Console.ReadLine());

//                     if (d3dtxConversionMode > 0)
//                     {
//                         Console.WriteLine("Incorrect d3dtx conversion mode, try again. Value must be lower or equal to 0, the minimum being: " + (-currentMaxD3DTXModes + 1) + ".");
//                     }
//                     else
//                     {
//                         break; // If it's successful, then break out of the loop
//                     }
//                 }
//                 catch (Exception)
//                 {
//                     Console.WriteLine("Incorrect d3dtx conversion mode, try again. Value must be lower or equal to 0, the minimum being: " + (-currentMaxD3DTXModes + 1) + ".");
//                 }
//             }

//             return d3dtxConversionMode;
//         }

//         public static bool GetBulkConversionBoolean()
//         {
//             string bulkConversionEnabled;

//             // Run a loop until the mode is valid
//             while (true)
//             {
//                 try
//                 {
//                     // Get the value from user
//                     bulkConversionEnabled = Console.ReadLine().ToLower();

//                     if (bulkConversionEnabled.Equals("y") && bulkConversionEnabled.Equals("n"))
//                     {
//                         Console.WriteLine("Incorrect value, try again. Value must be either Y or N.");
//                     }
//                     else
//                     {
//                         break; // If it's successful, then break out of the loop
//                     }
//                 }
//                 catch (Exception)
//                 {
//                     Console.WriteLine("Incorrect value, try again. Value must be either Y or N.");
//                 }
//             }

//             return bulkConversionEnabled.Equals("y");
//         }
//     }
// }
