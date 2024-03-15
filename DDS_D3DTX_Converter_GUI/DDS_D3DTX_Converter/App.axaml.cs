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
                Logger.Instance.Log(e);
            }
            catch (IOException ex)
            {
                Console.Error.WriteLine("Error writing technical details to file: " + ex.Message);
                throw new Exception(e.Message, e);
            }
        }
       
    }
}
