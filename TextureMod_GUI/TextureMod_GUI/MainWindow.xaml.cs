using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using D3DTX_TextureConverter.Telltale;
using D3DTX_TextureConverter.Main;
using TextureMod_GUI.UI;

namespace TextureMod_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //main app objects
        private MainManager mainManager;
        private ConsoleWriter consoleWriter;

        //XAML INITALIZATION
        public MainWindow()
        {
            InitializeComponent(); //initalize xaml
            InitalizeApplication(); //initalize our app

            //update our UI
            UpdateUI();
        }

        /// <summary>
        /// Initalizes our objects for the application
        /// </summary>
        public void InitalizeApplication()
        {
            mainManager = new MainManager(this, consoleWriter);
            consoleWriter = new ConsoleWriter(ui_console_textbox);
            Console.SetOut(consoleWriter);

            Console.WriteLine("[Welcome to the D3DTX Texture converter!]");
            Console.WriteLine("To begin, open a directory that contains d3dtx files.");
        }

        /// <summary>
        /// updates our application UI
        /// </summary>
        public void UpdateUI()
        {
            //update the app version label
            ui_window_appversion_label.Content = mainManager.appVersion;

            //update our texture directory UI
            ui_textureDirectory_directorypath_textbox.Text = mainManager.Get_WorkingDirectory_Path();
            ui_textureDirectory_files_listview.ItemsSource = mainManager.Get_WorkingDirectory_Files();
            ui_textureDirectory_convertD3DTX_button.IsEnabled = mainManager.CanConvertTo_D3DTX();
            ui_textureDirectory_convertDDS_button.IsEnabled = mainManager.CanConvertTo_DDS();
            ui_textureDirectory_refreshdirectory_button.IsEnabled = mainManager.WorkingDirectory_Path_Exists();

            List<UI_D3DTX_6VSM> test = new List<UI_D3DTX_6VSM>();
            test.Add(new UI_D3DTX_6VSM());

            ui_imageproperties_propertyGrid.CurrentObject = new UI_D3DTX_6VSM();
        }

        /// <summary>
        /// Previews a DDS image and displays it along with it's properties
        /// </summary>
        public void PreviewImage()
        {
            //if there is no valid item selected, don't continue
            if (ui_textureDirectory_files_listview.SelectedItem == null)
                return;

            //get our selected file object from the working directory
            WorkingDirectory_File workingDirectory_file = (WorkingDirectory_File)ui_textureDirectory_files_listview.SelectedItem;

            //if the selected file is not a .dds file then don't continue (we can only display .dds files)
            if(System.IO.Path.GetExtension(workingDirectory_file.FilePath).Equals(".dds") == false)
                return;

            //create our bitmap object
            BitmapImage bitmap;

            //we are going to try to display the bitmap (if our converter fails and the image can't be decoded as a DDS properly, then we will just catch the exception and ignore)
            try
            {
                //initalize our bitmap object
                bitmap = new BitmapImage();

                //initalize the bitmap image initalizatiom method
                bitmap.BeginInit();

                //get the file path of the dds image on the disk
                bitmap.UriSource = new Uri(workingDirectory_file.FilePath, UriKind.Absolute);

                //end the initalization
                bitmap.EndInit();

                //assign the source to our bitmap object
                ui_imagepreview_image.Source = bitmap;

                //display the name of the image
                ui_imagepreview_imageName_label.Content = workingDirectory_file.FileName;

                //display the image properties
                //ui_imageproperties_infobox_textblock.Text = GetImageProperties_Text(bitmap, workingDirectory_file.FileName, workingDirectory_file.FilePath);
            }
            catch (Exception e)
            {
                //if it fails to decode the dds image and display it, notify the user and write a message.
                //ui_imageproperties_infobox_textblock.Text = "Can't Decode DDS Image.";

                //don't continue
                return;
            }
        }

        /// <summary>
        /// Displays basic image properties and info.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetImageProperties_Text(BitmapImage image, string name, string path)
        {
            string final = "";

            /*
            final += string.Format("Image Name : {0}", name);
            final += Environment.NewLine;

            final += string.Format("Pixel Width : {0}", image.PixelWidth.ToString());
            final += Environment.NewLine;

            final += string.Format("Pixel Height : {0}", image.PixelHeight.ToString());
            final += Environment.NewLine;

            final += string.Format("DDS Format : {0}", read_DDS.DDSFormat(path));
            final += Environment.NewLine;

            final += string.Format("Has Mip Maps : {0}", read_DDS.HasMipMaps(path).ToString());
            final += Environment.NewLine;

            final += string.Format("Mip Map Count : {0}", read_DDS.MipMapCount(path).ToString());
            final += Environment.NewLine;
            */

            return final;
        }

        //------------------------------------- MAIN XAML FUNCTIONS -------------------------------------

        private void ui_textureDirectory_opendirectory_button_Click(object sender, RoutedEventArgs e)
        {
            mainManager.Set_WorkingDirectory_Path();
        }

        private void ui_textureDirectory_files_listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PreviewImage();
        }

        private void ui_textureDirectory_refreshdirectory_button_Click(object sender, RoutedEventArgs e)
        {
            mainManager.Refresh_WorkingDirectory();
            UpdateUI();
        }

        private void ui_textureDirectory_files_listview_contextmenu_refreshlist_Click(object sender, RoutedEventArgs e)
        {
            mainManager.Refresh_WorkingDirectory();
            UpdateUI();
        }

        private void ui_textureDirectory_files_listview_contextmenu_openfolder_Click(object sender, RoutedEventArgs e)
        {
            mainManager.WorkingDirectory_OpenFileExplorer();
        }

        private void ui_textureDirectory_convertDDS_button_Click(object sender, RoutedEventArgs e)
        {
            mainManager.ConvertToDDS();
        }

        private void ui_textureDirectory_convertD3DTX_button_Click(object sender, RoutedEventArgs e)
        {
            mainManager.ConvertToD3DTX();
        }

        private void ui_window_help_button_Click(object sender, RoutedEventArgs e)
        {
            mainManager.Open_AppHelp();
        }
    }
}
