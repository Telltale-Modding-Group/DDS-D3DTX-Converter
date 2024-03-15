using System;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace DDS_D3DTX_Converter_GUI.Utilities
{
    /// <summary>
    /// A helper class for the dialog messages. 
    /// </summary>
    public static class MessageBoxes
    {
        /// <summary>
        /// Creates and returns the error dialog.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBoxStandardParams GetErrorBox(string message)
        {
            var assetsUri = new Uri("avares://DDS_D3DTX_Converter/Assets/");
            var bitmap = new Bitmap(AssetLoader.Open(new Uri(assetsUri + "main_icon.ico")));

            return new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Error",
                ContentMessage = message,
                Icon = Icon.Error,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowIcon = new WindowIcon(bitmap),
                CanResize = false,
                MaxWidth = 500,
                MaxHeight = 800,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = false,
            };
        }

        /// <summary>
        /// Creates and returns the confirmation dialog.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageBoxStandardParams GetConfirmationBox(string message)
        {
            var assetsUri = new Uri("avares://DDS_D3DTX_Converter/Assets/");
            var bitmap = new Bitmap(AssetLoader.Open(new Uri(assetsUri + "main_icon.ico")));

            return new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.YesNoCancel,
                EnterDefaultButton = ClickEnum.Cancel,
                ContentTitle = "Confirm",
                ContentMessage = message,
                Icon = Icon.Question,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowIcon = new WindowIcon(bitmap),
                CanResize = false,
                MaxWidth = 500,
                MaxHeight = 800,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = false,
            };
        }
    }
}