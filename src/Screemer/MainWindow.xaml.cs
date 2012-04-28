using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace Screemer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel(this.Dispatcher);
            DataContext = vm;
            
            Loaded += (sender, args) =>
            {
                vm.Start();
            };

            Closing += (sender, args) =>
            {
                vm.Stop();
            };
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        // at class level
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        readonly Dispatcher _dispatcher;
        BackgroundWorker _worker;
        BitmapSource _screenCapture;
        public BitmapSource ScreenCapture
        {
            get { return _screenCapture; }
            set
            {
                _screenCapture = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ScreenCapture"));
            }
        }
       public MainViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _worker = new BackgroundWorker();
            _worker.DoWork += (worker, args) =>
            {
                var width = 200;
                var height = 200;
                var origin = new Point(1620, 785);

                var size = new System.Drawing.Size(290, 290);
                var bitmap = new Bitmap(size.Width, size.Height);
                
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.CopyFromScreen(origin, new Point(0, 0), size);
                    }

                    IntPtr hBitmap = bitmap.GetHbitmap();
                    Action action = () =>
                    {
                        BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap,
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                        ScreenCapture = bitmapSource;
                    };

                    _dispatcher.Invoke(action, new object[0]);

                    DeleteObject(hBitmap);
                    bitmap.Dispose();
            };
            _worker.RunWorkerCompleted += (sender, args) =>
            {
                if (_isRunning)
                {
                    _worker.RunWorkerAsync();
                }
            };

        }

        bool _isRunning;

      

        public void Start()
        {
            _isRunning = true;
            _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            _isRunning = false;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
