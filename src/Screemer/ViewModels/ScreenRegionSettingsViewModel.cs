using System.Collections.Generic;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Result;

namespace Screemer.ViewModels
{
    public class ScreenRegionSettingsViewModel : Screen
    {
        readonly IScreenCapturer _screenCapturer;

        public ScreenRegionSettingsViewModel(IScreenCapturer screenCapturer)
        {
            _screenCapturer = screenCapturer;
        }

        public ScreenRegion ScreenRegion
        {
            get { return _screenCapturer.ScreenRegion; }
        }

        public IEnumerable<IResult> SelectScreenRegion()
        {
            var selectScreenRegionResult = new SelectScreenRegionResult();

            yield return selectScreenRegionResult;

            Left = selectScreenRegionResult.ScreenRegion.Left;
            Top = selectScreenRegionResult.ScreenRegion.Top;
            Right = selectScreenRegionResult.ScreenRegion.Right;
            Bottom = selectScreenRegionResult.ScreenRegion.Bottom;
        } 

        public int Left
        {
            get { return ScreenRegion.Left; }
            set { ScreenRegion.Left = value; }
        }

        public int Top
        {
            get { return ScreenRegion.Top; }
            set { ScreenRegion.Top = value; }
        }

        public int Right
        {
            get { return ScreenRegion.Right; }
            set { ScreenRegion.Right = value; }
        }

        public int Bottom
        {
            get { return ScreenRegion.Bottom; }
            set { ScreenRegion.Bottom = value; }
        }
    }
}