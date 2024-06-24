using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Svg.Skia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using D3DTX_Converter.Main;
using D3DTX_Converter.Utilities;
using D3DTX_Converter.Views;
using DDS_D3DTX_Converter_GUI.Utilities;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IImage = Avalonia.Media.IImage;

namespace DDS_D3DTX_Converter.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    #region MEMBERS

    private readonly ObservableCollection<FormatItemViewModel> _d3dtxTypes =
    [
        new FormatItemViewModel { Name = "DDS", ItemStatus = true },
        new FormatItemViewModel { Name = "PNG", ItemStatus = false },
        new FormatItemViewModel { Name = "KTX", ItemStatus = false },
        new FormatItemViewModel { Name = "KTX2", ItemStatus = false }
    ];

    private readonly ObservableCollection<FormatItemViewModel> _ddsTypes =
    [
        new FormatItemViewModel { Name = "D3DTX", ItemStatus = true},
        new FormatItemViewModel { Name = "PNG", ItemStatus = true },
        new FormatItemViewModel { Name = "JPEG", ItemStatus = true },
        new FormatItemViewModel { Name = "BMP", ItemStatus = true },
        new FormatItemViewModel { Name = "TIFF", ItemStatus = true },
        new FormatItemViewModel { Name = "TGA", ItemStatus = false }
    ];

    private readonly ObservableCollection<FormatItemViewModel> _ktxTypes =
    [
        new FormatItemViewModel { Name = "D3DTX", ItemStatus = true}
    ];

    private readonly ObservableCollection<FormatItemViewModel> _otherTypes =
        [new FormatItemViewModel { Name = "DDS", ItemStatus = true }];

    private readonly ObservableCollection<FormatItemViewModel> _folderTypes =
    [
        new FormatItemViewModel { Name = "D3DTX -> DDS", ItemStatus = true},
        new FormatItemViewModel { Name = "D3DTX -> PNG", ItemStatus = false},
        new FormatItemViewModel { Name = "D3DTX -> KTX", ItemStatus = false},
        new FormatItemViewModel { Name = "D3DTX -> KTX2", ItemStatus = false},
        new FormatItemViewModel { Name = "DDS -> D3DTX", ItemStatus = true},
        new FormatItemViewModel { Name = "DDS -> PNG", ItemStatus = true},
        new FormatItemViewModel { Name = "DDS -> JPEG", ItemStatus = true},
        new FormatItemViewModel { Name = "DDS -> BMP", ItemStatus = true},
        new FormatItemViewModel { Name = "DDS -> TIFF", ItemStatus = true},
        new FormatItemViewModel { Name = "DDS -> TGA", ItemStatus = false},
        new FormatItemViewModel { Name = "PNG -> DDS", ItemStatus = true},
        new FormatItemViewModel { Name = "JPEG -> DDS", ItemStatus = true},
        new FormatItemViewModel { Name = "BMP -> DDS", ItemStatus = true},
        new FormatItemViewModel { Name = "TIFF -> DDS", ItemStatus = true},
        new FormatItemViewModel { Name = "TGA -> DDS", ItemStatus = false}
    ];

    private readonly ObservableCollection<FormatItemViewModel> _versionConvertOptions =
    [
        new FormatItemViewModel { Name = "Default", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 1", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 2", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 3", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 4", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 5", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 6", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 7", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 8", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 9", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 10", ItemStatus = true},
        new FormatItemViewModel { Name = "Legacy Version 11", ItemStatus = true},
    ];

    private readonly List<string> _allTypes = [".png", ".jpg", ".jpeg", ".bmp", ".tif", ".tiff", ".d3dtx", ".dds", ".ktx", ".ktx2", ".tga"];

    // No idea if this is correct
    public static FilePickerFileType AllowedTypes { get; } = new("All Supported Types")
    {
        Patterns = ["*.png", "*.jpg", "*.jpeg", "*.bmp", "*.tif", "*.tiff", "*.d3dtx", "*.dds", "*.ktx", "*.ktx2", "*.tga", "*.json"],
        AppleUniformTypeIdentifiers = ["public.image"],
        MimeTypes = ["image/png", "image/jpeg", "image/bmp", "image/tiff", "image/tga", "image/vnd.ms-dds", "image/vnd.ms-d3dtx", "image/vnd.ms-ktx", "image/vnd.ms-ktx2"]
    };

    private readonly MainManager mainManager = MainManager.GetInstance();
    private readonly Uri _assetsUri = new("avares://DDS_D3DTX_Converter/Assets/");
    private static readonly string ErrorSvgFilename = "error.svg";

    #endregion

    #region UI PROPERTIES

    [ObservableProperty] private WorkingDirectoryFile? _dataGridSelectedItem;

    [ObservableProperty] private ImageProperties _imageProperties;
    [ObservableProperty] private FormatItemViewModel _selectedFormat;

    [ObservableProperty] private FormatItemViewModel _selectedVersionConvertOption;
    [ObservableProperty] private ObservableCollection<FormatItemViewModel> _formatsList;
    [ObservableProperty] private ObservableCollection<FormatItemViewModel> _versionConvertOptionsList;
    [ObservableProperty] private bool _comboBoxStatus;

    [ObservableProperty] private bool _versionConvertComboBoxStatus;
    [ObservableProperty] private bool _saveButtonStatus;
    [ObservableProperty] private bool _deleteButtonStatus;
    [ObservableProperty] private bool _convertButtonStatus;
    [ObservableProperty] private bool _debugButtonStatus;
    [ObservableProperty] private bool _contextOpenFolderStatus;
    [ObservableProperty] private bool _chooseOutputDirectoryCheckBoxEnabledStatus;

    [ObservableProperty] private int _selectedComboboxIndex;
    [ObservableProperty] private int _selectedLegacyTitleIndex;
    [ObservableProperty] private string? _imageNamePreview;
    [ObservableProperty] private IImage? _imagePreview;
    [ObservableProperty] private string? _fileText;
    [ObservableProperty] private string? _directoryPath;
    [ObservableProperty] private bool _returnDirectoryButtonStatus;
    [ObservableProperty] private bool _refreshDirectoryButtonStatus;

    [ObservableProperty] private bool _chooseOutputDirectoryCheckboxStatus;
    [ObservableProperty] private ObservableCollection<WorkingDirectoryFile>? _workingDirectoryFiles;

    public class FormatItemViewModel
    {
        public string? Name { get; set; }
        public bool ItemStatus { get; set; }
    }

    public WorkingDirectoryFile? DataGrid_SelectedItem
    {
        get => _dataGridSelectedItem;
        set
        {
            if (_dataGridSelectedItem != value)
            {
                _dataGridSelectedItem = value;
                PreviewImage();
                ResetPanAndZoomCommand.Execute(null);
            }
        }
    }

    public RelayCommand ResetPanAndZoomCommand { get; internal set; }

    private void ResetPanAndZoom()
    {
        // Logic to reset pan and zoom
        // This method will be linked with code-behind to reset the ZoomBorder.
    }

    #endregion

    public MainViewModel()
    {
        ImagePreview = new SvgImage()
        {
            Source = SvgSource.Load(ErrorSvgFilename, _assetsUri)
        };
        VersionConvertOptionsList = _versionConvertOptions;
        WorkingDirectoryFiles = new ObservableCollection<WorkingDirectoryFile>();
        SelectedVersionConvertOption = VersionConvertOptionsList[0];
    }

    #region MAIN MENU BUTTONS ACTIONS

    // Open Directory Command
    public async Task OpenDirectoryButton_Click()
    {
        try
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            await mainManager.SetWorkingDirectoryPath(provider);

            if (mainManager.GetWorkingDirectoryPath() != string.Empty)
            {
                ReturnDirectoryButtonStatus = true;
                RefreshDirectoryButtonStatus = true;
                DataGrid_SelectedItem = null;

                await UpdateUiAsync();
            }
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(e.Message);
        }
    }

    public async void SaveFileButton_Click()
    {
        try
        {
            if (DataGrid_SelectedItem is not null)
            {
                var topLevel = GetMainWindow();

                if (Directory.Exists(DataGrid_SelectedItem.FilePath))
                {
                    throw new Exception("Cannot save a directory.");
                }

                // Start async operation to open the dialog.
                var storageFile = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save File",
                    SuggestedFileName = DataGrid_SelectedItem.FileName,
                    ShowOverwritePrompt = true,
                    DefaultExtension = DataGrid_SelectedItem.FileType is null ? "bin" : DataGrid_SelectedItem.FileType.Substring(1)
                });

                if (storageFile is not null)
                {
                    var destinationFilePath = storageFile.Path.AbsolutePath;

                    if (File.Exists(DataGrid_SelectedItem.FilePath))
                        File.Copy(DataGrid_SelectedItem.FilePath, destinationFilePath, true);
                }
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync("Error during saving the file. " + ex.Message);
        }
        finally
        {
            await SafeRefreshDirectoryAsync();
            await UpdateUiAsync();
        }
    }

    public async void AddFilesButton_Click()
    {
        try
        {
            if (string.IsNullOrEmpty(DirectoryPath) || !Directory.Exists(DirectoryPath)) return;

            var topLevel = GetMainWindow();

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Files",
                AllowMultiple = true,
                SuggestedStartLocation = await topLevel.StorageProvider.TryGetFolderFromPathAsync(DirectoryPath),
                FileTypeFilter = [AllowedTypes]
            });

            foreach (var file in files)
            {
                var destinationFilePath = Path.Combine(DirectoryPath, file.Name);

                var i = 1;
                while (File.Exists(destinationFilePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                    var extension = Path.GetExtension(file.Name);
                    destinationFilePath = Path.Combine(DirectoryPath,
                        $"{fileNameWithoutExtension}({i++}){extension}");
                }

                File.Copy(new Uri(file.Path.ToString()).LocalPath, destinationFilePath);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync("Error during adding files. Some files were not copied. " + ex.Message);
        }

        await SafeRefreshDirectoryAsync();
        await UpdateUiAsync();
    }

    // Delete Command
    public async Task DeleteFileButton_Click()
    {
        var workingDirectoryFile =
            DataGrid_SelectedItem;

        var textureFilePath = workingDirectoryFile.FilePath;

        try
        {
            if (File.Exists(textureFilePath))
            {
                File.Delete(textureFilePath);
            }
            else if (Directory.Exists(textureFilePath))
            {
                var mainWindow = GetMainWindow();
                var messageBox =
                    MessageBoxes.GetConfirmationBox("Are you sure you want to delete this directory?");

                var result = await MessageBoxManager.GetMessageBoxStandard(messageBox)
                    .ShowWindowDialogAsync(mainWindow);

                if (result != ButtonResult.Yes) return;

                Directory.Delete(textureFilePath);
            }
            else
            {
                throw new Exception("Invalid file or directory path.");
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
        finally
        {
            DataGrid_SelectedItem = null;
            await SafeRefreshDirectoryAsync();
            await UpdateUiAsync();
        }
    }

    public void HelpButton_Click()
    {
        mainManager.OpenAppHelp();
    }


    [RelayCommand]
    public void AboutButton_Click()
    {
        var mainWindow = GetMainWindow();
        var aboutWindow = new AboutWindow
        {
            DataContext = new AboutViewModel()
        };

        aboutWindow.ShowDialog(mainWindow);
    }

    #endregion

    #region CONTEXT MENU ACTIONS

    public async void ContextMenuAddFilesCommand()
    {
        try
        {
            if (string.IsNullOrEmpty(DirectoryPath) || !Directory.Exists(DirectoryPath)) return;

            var topLevel = GetMainWindow();

            // Start async operation to open the dialog.
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open Files",
                AllowMultiple = true
            });

            foreach (var file in files)
            {
                var destinationFilePath = Path.Combine(DirectoryPath, file.Name);

                var i = 1;
                while (File.Exists(destinationFilePath))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                    var extension = Path.GetExtension(file.Name);
                    destinationFilePath = Path.Combine(DirectoryPath,
                        $"{fileNameWithoutExtension}({i++}){extension}");
                }

                File.Copy(file.Path.AbsolutePath, destinationFilePath);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync("Error during adding files. Some files were not copied." + ex.Message);
        }
        finally
        {

        }

        await SafeRefreshDirectoryAsync();
        await UpdateUiAsync();
    }

    public async void ContextMenuOpenFileCommand()
    {
        try
        {
            if (DataGrid_SelectedItem == null)
                return;

            var workingDirectoryFile =
                DataGrid_SelectedItem;

            var filePath = workingDirectoryFile.FilePath;

            if (!File.Exists(filePath) && !Directory.Exists(filePath))
                throw new DirectoryNotFoundException("Directory was not found");

            mainManager.OpenFile(filePath);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
    }

    public async void ContextMenuOpenFolderCommand()
    {
        try
        {
            // if there is no valid item selected, don't continue
            if (DataGrid_SelectedItem == null)
                return;

            // get our selected file object from the working directory
            var workingDirectoryFile = DataGrid_SelectedItem;
            if (!Directory.Exists(workingDirectoryFile.FilePath))
                throw new DirectoryNotFoundException("Directory not found.");

            await mainManager.SetWorkingDirectoryPath(workingDirectoryFile.FilePath);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
        finally
        {
            ContextOpenFolderStatus = false;
            await UpdateUiAsync();
        }
    }

    [RelayCommand]
    public async Task ContextMenuOpenFileExplorerCommand()
    {
        try
        {
            if (DirectoryPath == null) return;

            if (DataGrid_SelectedItem == null)
            {
                if (Directory.Exists(DirectoryPath))
                    await OpenFileExplorer(DirectoryPath);
            }
            else
            {
                if (File.Exists(DataGrid_SelectedItem.FilePath))
                    await OpenFileExplorer(DataGrid_SelectedItem.FilePath);
                else if (Directory.Exists(DataGrid_SelectedItem.FilePath))
                    await OpenFileExplorer(DataGrid_SelectedItem.FilePath);
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
    }

    [RelayCommand]
    public async Task RefreshDirectoryButton_Click()
    {
        if (DirectoryPath != null && DirectoryPath != string.Empty)
        {
            await RefreshUiAsync();
        }
    }

    [RelayCommand]
    public async Task ContextDeleteFileCommand()
    {
        await DeleteFileButton_Click();
    }

    public async Task SafeRefreshDirectoryAsync()
    {
        try
        {
            mainManager.RefreshWorkingDirectory();
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
    }

    #endregion

    #region CONVERTER PANEL ACTIONS

    /// <summary>
    /// Convert command of the "Convert to" button. It initiates the conversion process.
    /// Error dialogs appear when something goes wrong with the conversion process.
    /// </summary>
    [RelayCommand]
    public async Task ConvertButton_Click()
    {
        try
        {
            if (DataGrid_SelectedItem == null) return;

            var workingDirectoryFile =
                DataGrid_SelectedItem;

            string? textureFilePath = workingDirectoryFile.FilePath;

            if (!File.Exists(textureFilePath) && !Directory.Exists(textureFilePath))
                throw new DirectoryNotFoundException("File/Directory was not found.");

            string outputDirectoryPath = mainManager.GetWorkingDirectoryPath();

            if (ChooseOutputDirectoryCheckboxStatus)
            {
                var topLevel = GetMainWindow();

                // Start async operation to open the dialog.
                var folderPath = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Choose your output folder location.",
                    AllowMultiple = false,
                });

                if (folderPath is null || folderPath.Count == 0)
                {
                    return;
                }

                outputDirectoryPath = folderPath.First().Path.AbsolutePath;
            }

            D3DTXConversionType conversionType = GetD3DTXConversionType();

            string[] types = SelectedFormat.Name.Split(" -> ");

            TextureType oldTextureType;
            TextureType newTextureType;

            if (types.Length == 2)
            {
                oldTextureType = GetTextureTypeFromExtension(types[0]);
                newTextureType = GetTextureTypeFromExtension(types[1]);

                if (!ChooseOutputDirectoryCheckboxStatus)
                {
                    outputDirectoryPath = textureFilePath;
                }

                Converter.ConvertBulk(textureFilePath, outputDirectoryPath, oldTextureType, newTextureType, conversionType);
            }
            else
            {
                oldTextureType = GetTextureTypeFromExtension(DataGrid_SelectedItem.FileType);
                newTextureType = GetTextureTypeFromItem(SelectedFormat.Name);
                Console.WriteLine("Old Texture Type: " + oldTextureType);
                Console.WriteLine("New Texture Type: " + newTextureType);
                await Converter.ConvertTexture(textureFilePath, outputDirectoryPath, oldTextureType, newTextureType, conversionType);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            var mainWindow = GetMainWindow();
            var messageBox =
                MessageBoxes.GetErrorBox(ex.Message);
            await MessageBoxManager.GetMessageBoxStandard(messageBox).ShowWindowDialogAsync(mainWindow);
            Logger.Instance().Log(ex);
        }
        finally
        {
            mainManager.RefreshWorkingDirectory();
            await UpdateUiAsync();
        }
    }

    private static TextureType GetTextureTypeFromItem(string newTextureType)
    {
        return newTextureType switch
        {
            "D3DTX" => TextureType.D3dtx,
            "DDS" => TextureType.Dds,
            "PNG" => TextureType.Png,
            "JPG" => TextureType.Jpeg,
            "JPEG" => TextureType.Jpeg,
            "BMP" => TextureType.Bmp,
            "TIF" => TextureType.Tiff,
            "TIFF" => TextureType.Tiff,
            "KTX" => TextureType.Ktx,
            "KTX2" => TextureType.Ktx2,
            "TGA" => TextureType.Tga,
            _ => TextureType.Unknown
        };
    }

    private D3DTXConversionType GetD3DTXConversionType()
    {
        return SelectedVersionConvertOption.Name switch
        {
            "Legacy Version 1" => D3DTXConversionType.LV1,
            "Legacy Version 2" => D3DTXConversionType.LV2,
            "Legacy Version 3" => D3DTXConversionType.LV3,
            "Legacy Version 4" => D3DTXConversionType.LV4,
            "Legacy Version 5" => D3DTXConversionType.LV5,
            "Legacy Version 6" => D3DTXConversionType.LV6,
            "Legacy Version 7" => D3DTXConversionType.LV7,
            "Legacy Version 8" => D3DTXConversionType.LV8,
            "Legacy Version 9" => D3DTXConversionType.LV9,
            "Legacy Version 10" => D3DTXConversionType.LV10,
            "Legacy Version 11" => D3DTXConversionType.LV11,
            _ => D3DTXConversionType.DEFAULT
        };
    }

    private static TextureType GetTextureTypeFromExtension(string newTextureType)
    {
        return GetTextureTypeFromItem(newTextureType.ToUpper().Remove(0, 1));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task DebugButton_Click()
    {
        try
        {
            if (DataGrid_SelectedItem == null) return;

            var workingDirectoryFile =
                DataGrid_SelectedItem;

            string? textureFilePath = workingDirectoryFile.FilePath;

            if (!File.Exists(textureFilePath))
                throw new DirectoryNotFoundException("File was not found.");

            D3DTXConversionType conversionType = GetD3DTXConversionType();

            var d3dtx = new D3DTX_Master();

            d3dtx.Read_D3DTX_File(textureFilePath, conversionType);

            var mainWindow = GetMainWindow();
            var messageBox = MessageBoxes.GetDebugInformationBox(d3dtx.GetD3DTXDebugInfo());

            var result = await MessageBoxManager.GetMessageBoxStandard(messageBox)
                    .ShowWindowDialogAsync(mainWindow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            await HandleExceptionAsync(ex.Message);
        }
    }

    #endregion

    ///<summary>
    /// Updates our application UI, mainly the data grid.
    ///</summary>
    private async Task UpdateUiAsync()
    {
        // update our texture directory UI
        try
        {
            DirectoryPath = mainManager.GetWorkingDirectoryPath();
            mainManager.RefreshWorkingDirectory();
            var workingDirectoryFiles = mainManager.GetWorkingDirectoryFiles();

            for (int i = WorkingDirectoryFiles.Count - 1; i >= 0; i--)
            {
                if (!workingDirectoryFiles.Contains(WorkingDirectoryFiles[i]))
                {
                    WorkingDirectoryFiles.RemoveAt(i);
                }
            }

            // Add items from the list to the observable collection if they are not already present
            foreach (var item in workingDirectoryFiles)
            {
                if (!WorkingDirectoryFiles.Contains(item))
                {
                    WorkingDirectoryFiles.Add(item);
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            await HandleExceptionAsync("Error during updating ui. " + ex.Message);
        }
    }

    #region SMALL MENU BUTTON ACTIONS

    public async Task ReturnDirectory_Click()
    {
        try
        {
            if (Directory.GetParent(DirectoryPath) == null) return;
            WorkingDirectoryFiles.Clear();
            await mainManager.SetWorkingDirectoryPath(Directory.GetParent(DirectoryPath).ToString());
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex.Message);
        }
        finally
        {
            PreviewImage();
            await UpdateUiAsync();
        }
    }

    public async Task ContextMenuRefreshDirectoryCommand()
    {
        await RefreshDirectoryButton_Click();
    }

    #endregion

    #region HELPERS

    private Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
            return lifetime.MainWindow;

        throw new Exception("Main Parent Window Not Found");
    }

    private void ChangeComboBoxItemsByItemExtension(string itemExtension)
    {
        var extensionMappings = new Dictionary<string, ObservableCollection<FormatItemViewModel>>
        {
            { ".dds", _ddsTypes },
            { ".d3dtx", _d3dtxTypes },
            {".ktx", _otherTypes},
            {".ktx2", _otherTypes},
            { ".png", _otherTypes },
            { ".jpg", _otherTypes },
            { ".jpeg", _otherTypes },
            { ".bmp", _otherTypes },
            { ".tga", _otherTypes },
            { ".tif", _otherTypes },
            { ".tiff", _otherTypes },
            {"", _folderTypes}
        };

        if (itemExtension == null)
        {
            FormatsList = null;
            ConvertButtonStatus = false;
            ComboBoxStatus = false;
        }
        else if (extensionMappings.TryGetValue(itemExtension, out var selectedItems))
        {
            if (itemExtension.Equals(".d3dtx"))
                VersionConvertComboBoxStatus = true;
            else
                VersionConvertComboBoxStatus = false;

            FormatsList = selectedItems;
            ConvertButtonStatus = true;
            SelectedComboboxIndex = 0;
            ComboBoxStatus = true;
            // There is an issue in Avalonia relating to dynamic sources and binding indexes.
            // Github issue: https://github.com/AvaloniaUI/Avalonia/issues/13736
            // When fixed, the line below can be removed.
            SelectedFormat = selectedItems[0];
        }
        else
        {
            FormatsList = null;
            ConvertButtonStatus = false;
            VersionConvertComboBoxStatus = false;
            ComboBoxStatus = false;
        }
    }

    #endregion

    public async void RowDoubleTappedCommand(object? sender, TappedEventArgs args)
    {
        try
        {
            var source = args.Source;
            if (source is null) return;
            if (source is Border)
            {
                if (DataGrid_SelectedItem == null)
                    return;

                var workingDirectoryFile =
                    DataGrid_SelectedItem;

                var filePath = workingDirectoryFile.FilePath;

                if (!File.Exists(filePath) && !Directory.Exists(filePath))
                    throw new DirectoryNotFoundException("Directory was not found");

                if (File.Exists(workingDirectoryFile.FilePath))
                {
                    mainManager.OpenFile(filePath);
                }
                else
                {
                    await mainManager.SetWorkingDirectoryPath(workingDirectoryFile.FilePath);
                    WorkingDirectoryFiles.Clear();
                    await UpdateUiAsync();
                }
            }
        }
        catch (Exception ex)
        {
            var mainWindow = GetMainWindow();
            var messageBox = MessageBoxes.GetErrorBox(ex.Message);

            await MessageBoxManager.GetMessageBoxStandard(messageBox).ShowWindowDialogAsync(mainWindow);
        }
        finally
        {
            ContextOpenFolderStatus = false;
        }
    }

    private void UpdateUIElementsAsync()
    {
        if (DataGrid_SelectedItem != null)
        {
            var workingDirectoryFile = DataGrid_SelectedItem;
            var path = workingDirectoryFile.FilePath;
            var extension = Path.GetExtension(path).ToLowerInvariant();

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                ResetUIElements();
                mainManager.RefreshWorkingDirectory();
                UpdateUiAsync().Wait();
                throw new Exception("File or directory do not exist anymore! Refreshing the directory.");
            }

            DebugButtonStatus = extension == ".d3dtx" || extension == ".dds";
            SaveButtonStatus = File.Exists(path);
            DeleteButtonStatus = true;
            ContextOpenFolderStatus = Directory.Exists(path);
            ChooseOutputDirectoryCheckBoxEnabledStatus = true;
            if (extension == string.Empty && !Directory.Exists(path))
            {
                ChangeComboBoxItemsByItemExtension(null);
            }
            else
            {
                ChangeComboBoxItemsByItemExtension(extension);
            }
        }
        else
        {
            ResetUIElements();
        }
    }

    private void ResetUIElements()
    {
        SaveButtonStatus = false;
        DeleteButtonStatus = false;
        DebugButtonStatus = false;
        ConvertButtonStatus = false;
        ComboBoxStatus = false;
        VersionConvertComboBoxStatus = false;
        ChooseOutputDirectoryCheckBoxEnabledStatus = false;
        DebugButtonStatus = false;
        ChooseOutputDirectoryCheckboxStatus = false;
        ImageProperties = ImageProperties.GetImagePropertiesFromInvalid();
        ImagePreview = new SvgImage()
        {
            Source = SvgSource.Load(ErrorSvgFilename, _assetsUri)
        };
        ImageNamePreview = string.Empty;
    }

    private void PreviewImage()
    {
        try
        {
            UpdateUIElementsAsync();

            if (DataGrid_SelectedItem == null)
                return;

            var workingDirectoryFile = DataGrid_SelectedItem;
            var filePath = workingDirectoryFile.FilePath;
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            ImageNamePreview = workingDirectoryFile.FileName + workingDirectoryFile.FileType;
            ImageProperties = GetImageProperties(filePath, extension);
            ImagePreview = GetImagePreview(filePath, extension);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            HandleImagePreviewErrorAsync(ex);
        }
    }

    private Task OpenFileExplorer(string path)
    {
        MainManager.OpenFileExplorer(path);
        return Task.CompletedTask;
    }

    private async Task RefreshUiAsync()
    {
        await SafeRefreshDirectoryAsync();
        await UpdateUiAsync();
    }

    private IImage GetImagePreview(string filePath, string extension)
    {
        return extension.ToLower() switch
        {
            ".d3dtx" => ImageUtilities.ConvertD3dtxToBitmap(filePath, GetD3DTXConversionType()),
            ".dds" => ImageUtilities.ConvertFileFromDdsToBitmap(filePath),
            ".tga" => ImageUtilities.ConvertFileFromTgaToBitmap(filePath),
            ".tiff" => ImageUtilities.ConvertTiffToBitmap(filePath),
            ".tif" => ImageUtilities.ConvertTiffToBitmap(filePath),
            ".png" => new Bitmap(filePath),
            ".jpg" => new Bitmap(filePath),
            ".jpeg" => new Bitmap(filePath),
            ".bmp" => new Bitmap(filePath),
            _ => new SvgImage { Source = SvgSource.Load(ErrorSvgFilename, _assetsUri) }
        };
    }


    private ImageProperties GetImageProperties(string filePath, string extension)
    {
        var supportedExtensions = _allTypes;

        if (supportedExtensions.Contains(extension.ToLower()))
        {
            return extension.ToLower() switch
            {
                ".d3dtx" => ImageProperties.GetImagePropertiesFromD3DTX(filePath, GetD3DTXConversionType()),
                ".dds" => ImageProperties.GetDdsProperties(filePath),
                ".ktx2" => ImageProperties.GetKtx2Properties(filePath),
                _ => ImageProperties.GetImagePropertiesFromOthers(filePath),
            };
        }

        // Return empty properties for unsupported formats
        return ImageProperties.GetImagePropertiesFromInvalid();
    }


    private async Task HandleImagePreviewErrorAsync(Exception ex)
    {
        await HandleExceptionAsync("Error during previewing image.\nError message: " + ex.Message);
        ImagePreview = new SvgImage { Source = SvgSource.Load(ErrorSvgFilename, _assetsUri) };
    }

    private async Task HandleExceptionAsync(string message)
    {
        var mainWindow = GetMainWindow();
        var messageBox = MessageBoxes.GetErrorBox(message);
        await MessageBoxManager.GetMessageBoxStandard(messageBox).ShowWindowDialogAsync(mainWindow);
    }
}