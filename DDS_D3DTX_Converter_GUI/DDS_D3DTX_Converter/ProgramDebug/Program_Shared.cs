using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.DirectX;
using D3DTX_Converter.Main;
using Newtonsoft.Json;

namespace D3DTX_Converter.ProgramDebug
{
    public static class Program_Shared
    {
        public static string GetFolderPathFromUser()
        {
            string folderPath = "";

            //run a loop until the path is valid
            while (true)
            {
                //get path from user
                folderPath = Console.ReadLine();

                //check if the path is valid
                if (Directory.Exists(folderPath) == false)
                {
                    //notify the user and this loop will run again
                    Console.WriteLine("Incorrect Texture Path, try again.");
                }
                else
                {
                    break; //if it's sucessful, then break out of the loop
                }
            }

            return folderPath;
        }
    }
}
