using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Logging;
using Avalonia.Markup.Xaml;
using DDS_D3DTX_Converter.ViewModels;
using DDS_D3DTX_Converter.Views;


namespace DDS_D3DTX_Converter;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        try
        {
            BindingPlugins.DataValidators.RemoveAt(0);
         
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
                };
            }
            base.OnFrameworkInitializationCompleted();
        }
    
        catch (Exception e)
        {
            try
            {
                // Create the "Crashes" directory if it doesn't exist
                string crashesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Crashes");
                Directory.CreateDirectory(crashesDirectory);

                // Create a new text file with the current date as the file name
                string logFileName = DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                string logFilePath = Path.Combine(crashesDirectory, logFileName);

                using (StreamWriter writer = new StreamWriter(logFilePath))
                {
                    writer.WriteLine("Timestamp: " + DateTime.Now);
                    writer.WriteLine("Error Details: " + e.Message);
                    writer.WriteLine("Stack Trace:\n" + e.StackTrace);
                    writer.WriteLine("------------------------------------------");
                    writer.Close();
                }
                
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine("Error writing technical details to file: " + ex.Message);
                throw new Exception(e.Message, e);
            }
        }
       
    }
}
