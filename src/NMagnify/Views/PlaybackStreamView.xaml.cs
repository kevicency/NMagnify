using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace NMagnify.Views
{
    /// <summary>
    /// Interaction logic for PlaybackStreamView.xaml
    /// </summary>
    public partial class PlaybackStreamView : MetroWindow
    {
        public PlaybackStreamView()
        {
            InitializeComponent();
        }

        void DragMoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed && e.MiddleButton != MouseButtonState.Pressed && e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    // Calcualting correct left coordinate for multi-screen system.
                    double mouseX = PointToScreen(Mouse.GetPosition(this)).X;
                    double width = RestoreBounds.Width;
                    double left = mouseX - width / 2;

                    // Aligning window's position to fit the screen.
                    double virtualScreenWidth = SystemParameters.VirtualScreenWidth;
                    left = left < 0 ? 0 : left;
                    left = left + width > virtualScreenWidth ? virtualScreenWidth - width : left;

                    Top = 0;
                    Left = left;

                    // Restore window to normal state.
                    WindowState = WindowState.Normal;

                }

                DragMove();
            }
            if (e.ClickCount != 2)
                return;

            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}
