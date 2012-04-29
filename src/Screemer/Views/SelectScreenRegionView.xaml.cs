using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Screemer.Model;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Screemer.Views
{
    public interface ISelectScreenRegion
    {
        event EventHandler<ScreenRegionSelectedEventArgs> ScreenRegionSelected;
    }

    /// <summary>
    ///   Interaction logic for SelectScreenRegionView.xaml
    /// </summary>
    public partial class SelectScreenRegionView : Window, ISelectScreenRegion
    {
        Point? _firstSelection;
        Point? _secondSelection;

        public SelectScreenRegionView()
        {
            InitializeComponent();
            Background = new SolidColorBrush(Color.FromArgb(120, 120, 120, 120));

            Loaded += (sender, args) =>
            {
                Left = 0;
                Top = 0;
                Width = Screen.AllScreens.Sum(x => x.WorkingArea.Width);
                Height = Screen.AllScreens.Sum(x => x.WorkingArea.Height);

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
                }
            };

            MouseLeftButtonDown += (sender, args) =>
            {
                var pos = Mouse.GetPosition(this);

                if (!_firstSelection.HasValue)
                {
                    _firstSelection = pos;
                }
                else
                {
                    _secondSelection = pos;

                    var region = new ScreenRegion(_firstSelection.Value, _secondSelection.Value);
                    OnScreenRegionSelected(new ScreenRegionSelectedEventArgs(region));
                    Close();
                }
            };

            MouseRightButtonDown += (sender, args) => ResetSelection();
        }

        #region ISelectScreenRegion Members

        public event EventHandler<ScreenRegionSelectedEventArgs> ScreenRegionSelected;

        #endregion

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