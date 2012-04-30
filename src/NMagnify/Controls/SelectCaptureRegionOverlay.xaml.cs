using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using NMagnify.Model;

namespace NMagnify.Controls
{
    public interface ISelectCaptureRegion
    {
        event EventHandler SelectionCancelled;
        event EventHandler<ScreenRegionSelectedEventArgs> ScreenRegionSelected;

        void Show();
    }

    /// <summary>
    ///   Interaction logic for SelectCaptureRegionOverlay.xaml
    /// </summary>
    public partial class SelectCaptureRegionOverlay : Window, ISelectCaptureRegion
    {
        Point? _firstSelection;
        Point? _secondSelection;

        public SelectCaptureRegionOverlay()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Color.FromArgb(120, 120, 120, 120));

            Loaded += (sender, args) =>
            {
                Left = SystemInformation.VirtualScreen.Left;
                Top = SystemInformation.VirtualScreen.Top;
                Width = SystemInformation.VirtualScreen.Width;
                Height = SystemInformation.VirtualScreen.Height;

                verticalLine.Y1 = 0;
                verticalLine.Y2 = Height;

                horizontalLine.X1 = 0;
                horizontalLine.X2 = Width;
            };

            MouseMove += (sender, args) =>
            {
                var pos = args.GetPosition(this);

                CrossLinesOn(pos);

                if (_firstSelection.HasValue)
                {
                    DrawSelection(_firstSelection.Value, pos);
                }
            };

            KeyDown += (sender, args) =>
            {
                if (args.Key == Key.Escape)
                {
                    Close();
                    OnSelectionCancelled(EventArgs.Empty);
                }
            };

            MouseLeftButtonDown += (sender, args) =>
            {
                var pos = Mouse.GetPosition(this);

                if (!_firstSelection.HasValue)
                {
                    _firstSelection = pos;
                }
                else if ((int) _firstSelection.Value.X != (int) pos.X
                         && (int) _firstSelection.Value.Y != (int) pos.Y)
                {
                    _secondSelection = pos;

                    var region = new ScreenRegion(_firstSelection.Value, _secondSelection.Value);
                    region.Left += (int) Left;
                    region.Right += (int) Left;
                    region.Top += (int) Top;
                    region.Bottom += (int) Top;
                    OnScreenRegionSelected(new ScreenRegionSelectedEventArgs(region));
                    Close();
                }
            };

            MouseRightButtonDown += (sender, args) => ResetSelection();
        }

        #region ISelectCaptureRegion Members

        public event EventHandler SelectionCancelled;

        public event EventHandler<ScreenRegionSelectedEventArgs> ScreenRegionSelected;

        #endregion

        public void OnSelectionCancelled(EventArgs e)
        {
            EventHandler handler = SelectionCancelled;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void OnScreenRegionSelected(ScreenRegionSelectedEventArgs e)
        {
            EventHandler<ScreenRegionSelectedEventArgs> handler = ScreenRegionSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        void DrawSelection(Point p1, Point p2)
        {
            var region = new ScreenRegion(p1, p2);

            Canvas.SetLeft(selection, region.Left);
            Canvas.SetTop(selection, region.Top);
            selection.Width = Math.Abs(region.Width);
            selection.Height = Math.Abs(region.Height);
            selection.Visibility = Visibility.Visible;
        }

        void ResetSelection()
        {
            _firstSelection = null;
            _secondSelection = null;
            selection.Visibility = Visibility.Collapsed;
        }

        void CrossLinesOn(Point point)
        {
            verticalLine.X1 = point.X;
            verticalLine.X2 = point.X;
            horizontalLine.Y1 = point.Y;
            horizontalLine.Y2 = point.Y;
        }
    }
}