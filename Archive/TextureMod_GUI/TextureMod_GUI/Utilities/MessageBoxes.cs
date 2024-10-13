using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D3DTX_TextureConverter.Utilities
{
    public static class MessageBoxes
    {
        /// <summary>
        /// Shows a message box with a information icon and an OK button.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        public static void Info(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Shows a message box with a error icon and an OK button.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        public static void Error(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Shows a message box with a warning icon and an OK button.
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        public static void Warning(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Shows a message box with a information icon and an YesNo button. Yes = True, No = False
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Info_Confirm(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Information);

            return result == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// Shows a message box with a error icon and an YesNo button. Yes = True, No = False
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Error_Confirm(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Error);

            return result == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// Shows a message box with a warning icon and an YesNo button. Yes = True, No = False
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool Warning_Confirm(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes ? true : false;
        }

        /// <summary>
        /// Shows a message box with a information icon and an YesNoCancel button. 0 = Yes, No = 1, Cancel = 2
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int Info_ConfirmOrCancel(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return 0;
                case MessageBoxResult.No:
                    return 1;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// Shows a message box with a warning icon and an YesNoCancel button. 0 = Yes, No = 1, Cancel = 2
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int Warning_ConfirmOrCancel(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return 0;
                case MessageBoxResult.No:
                    return 1;
                default:
                    return 2;
            }
        }

        /// <summary>
        /// Shows a message box with a error icon and an YesNoCancel button. 0 = Yes, No = 1, Cancel = 2
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int Error_ConfirmOrCancel(string message, string caption)
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return 0;
                case MessageBoxResult.No:
                    return 1;
                default:
                    return 2;
            }
        }
    }
}
