using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using NUnit.Framework;
using Newtonsoft.Json;
using Screemer.Model;
using Should.Fluent;

namespace Screemer.Test
{
    [TestFixture]
    public class Profile_Fixture
    {
        Profile Sut;

        [SetUp]
        public void SetUp()
        {
            Sut = new Profile();
        }

        [Test]
        public void Serialize()
        {
            Sut.Guid = Guid.NewGuid();
            Sut.CaptureRegion.Left = 1;
            Sut.CaptureRegion.Top = 2;
            Sut.CaptureRegion.Right = 3;
            Sut.CaptureRegion.Bottom = 4;

            var json = JsonConvert.SerializeObject(Sut);
            var profile = JsonConvert.DeserializeObject<Profile>(json);
            Console.WriteLine(profile);
        }
 
    }
    [TestFixture]
    public class BitmapScreenCapturer_Fixture
    {
        BitmapScreenCapturer Sut;

        [SetUp]
        public void SetUp()
        {
            Sut = new BitmapScreenCapturer();

            Sut.ScreenCaptured += (sender, args) => CapturedImage = args.CapturedImage;
        }

        protected ImageSource CapturedImage { get; set; }

        [Test]
        public void CaptureImage_CapturesCorrectSize()
        {
            var width = 20;
            var height = 30;

            Sut.CaptureRegionResolver = () => new ScreenRegion(0, 0, width, height);

            using (var bitmap = Sut.CaptureScreen())
            {
                bitmap.Width.Should().Equal(width);
                bitmap.Height.Should().Equal(height);
            }
        }

        [Test, Explicit]
        public void CaptureImage_CapturesCorrectScreenRegion()
        {
            // 20 pixel rectangle on the top left of screen
            Sut.CaptureRegionResolver = () => new ScreenRegion(0, 0, 20, 20);

            using(var bitmap = Sut.CaptureScreen())
            {
                var filename = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".png");
                bitmap.Save(filename, ImageFormat.Png);

                var process = new Process();
                process.StartInfo = new ProcessStartInfo(filename);
                process.Exited += (sender, args) => File.Delete(filename);
                process.Start();
            }
        }

        [Test]
        public void CaptureAndSleep_SleepsAsLongAsNeededToGetDesiredCapturesPerSecond()
        {
            Sut.CapturesPerSecond = 10;
            Sut.CaptureRegionResolver = () => new ScreenRegion(0, 0, 1, 1);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < Sut.CapturesPerSecond; i++)
            {
                Sut.CaptureAndSleep();
            }

            sw.Stop();
            sw.ElapsedMilliseconds.Should().Be.InRange(950L, 1050L);
        }
    }


    [TestFixture]
    public class BitmapUtility_Fixture
    {
        [Test]
        public void ConvertBitmapToImageSource_CreatesAnImageSource()
        {
            using (var image = Image.FromFile(@".\Assets\testimage.jpg"))
            {
                using (var bitmap = new Bitmap(image))
                {
                    var source = BitmapUtility.ConvertBitmapToImageSource(bitmap);
                    Array pixels = new byte[16];
                    source.CopyPixels(pixels, 8, 0);

                    // white
                    pixels.GetValue(0).Should().Equal((byte)0);
                    pixels.GetValue(1).Should().Equal((byte)0);
                    pixels.GetValue(2).Should().Equal((byte)0);
                    pixels.GetValue(3).Should().Equal((byte)255);
                    // black
                    pixels.GetValue(4).Should().Equal((byte)255);
                    pixels.GetValue(5).Should().Equal((byte)255);
                    pixels.GetValue(6).Should().Equal((byte)255);
                    pixels.GetValue(7).Should().Equal((byte)255);
                    // black
                    pixels.GetValue(8).Should().Equal((byte)255);
                    pixels.GetValue(9).Should().Equal((byte)255);
                    pixels.GetValue(10).Should().Equal((byte)255);
                    pixels.GetValue(11).Should().Equal((byte)255);
                    // white
                    pixels.GetValue(12).Should().Equal((byte)0);
                    pixels.GetValue(13).Should().Equal((byte)0);
                    pixels.GetValue(14).Should().Equal((byte)0);
                    pixels.GetValue(15).Should().Equal((byte)255);
                }
            }
        }
    }
}
