using MahApps.Metro.Controls;
using NMagnify.Properties;

namespace NMagnify.Views
{
    public partial class ShellView : MetroWindow
    {
        public ShellView()
        {
            Loaded += (sender, args) =>
            {
                if (Settings.Default.Shell_Left != 0)
                {
                    Left = Settings.Default.Shell_Left;
                }
                if (Settings.Default.Shell_Top != 0)
                {
                    Top = Settings.Default.Shell_Top;
                }
            };

            Closing += (sender, args) =>
            {
                Settings.Default.Shell_Left = (int) Left;
                Settings.Default.Shell_Top = (int) Top;
                Settings.Default.Save();
            };
        }
    }
}