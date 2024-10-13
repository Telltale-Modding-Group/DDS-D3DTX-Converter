using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace D3DTX_Converter.Utilities
{
    public static class IOManagement
    {
        /// <summary>
        /// Filters an array of files by ".d3dtx" so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files, string filterExtension)
        {
            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                if (extension.Equals(filterExtension))
                {
                    //add the matched extension to the list
                    filteredFiles.Add(file);
                }
            }

            //return the new filtered list
            return filteredFiles;
        }

        /// <summary>
        /// Filters an array of files by ".d3dtx" so only files with said extension will be in the array.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> FilterFiles(List<string> files, string[] filterExtensions)
        {
            //the new filtered list
            List<string> filteredFiles = new List<string>();

            //run a loop through the existing 'files'
            foreach (string file in files)
            {
                //get the extension of a file
                string extension = Path.GetExtension(file);

                //if the file's extension matches our filter, add it to the list (naturally anything that doesn't have said filter will be ignored)
                for(int i = 0; i < filterExtensions.Length; i++)
                {
                    if (extension.Equals(filterExtensions[i]))
                    {
                        //add the matched extension to the list
                        filteredFiles.Add(file);
                    }
                }
            }

            //return the new filtered list
            return filteredFiles;
        }
    }
}
