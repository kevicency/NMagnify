using System.Collections.Generic;
using Caliburn.Micro;
using NMagnify.Model;
using NMagnify.Results;

namespace NMagnify.ViewModels
{
    public class CaptureRegionSettingsViewModel : Screen
    {
        readonly IActiveProfileProvider _activeProfileProvider;

        public CaptureRegionSettingsViewModel(IActiveProfileProvider activeProfileProvider)
        {
            _activeProfileProvider = activeProfileProvider;
        }

        public int Left
        {
            get { return CaptureRegion.Left; }
            set
            {
                if (value < Right)
                {
                    CaptureRegion.Left = value;
                }
            }
        }

        public int Top
        {
            get { return CaptureRegion.Top; }
            set
            {
                if (value < Bottom)
                {
                    CaptureRegion.Top = value;
                }
            }
        }

        public int Right
        {
            get { return CaptureRegion.Right; }
            set
            {
                if (value > Left)
                {
                    CaptureRegion.Right = value;
                }
            }
        }

        public int Bottom
        {
            get { return CaptureRegion.Bottom; }
            set
            {
                if (value > Top)
                {
                    CaptureRegion.Bottom = value;
                }
            }
        }

        protected ScreenRegion CaptureRegion { get; set; }

        public IEnumerable<IResult> SelectScreenRegion()
        {
            var selectScreenRegionResult = new SelectCaptureRegionResult();

            yield return selectScreenRegionResult;

            Right = int.MaxValue;
            Left = int.MinValue;
            Top = int.MaxValue;
            Bottom = int.MaxValue;

            Left = selectScreenRegionResult.ScreenRegion.Left;
            Top = selectScreenRegionResult.ScreenRegion.Top;
            Bottom = selectScreenRegionResult.ScreenRegion.Bottom;
            Right = selectScreenRegionResult.ScreenRegion.Right;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _activeProfileProvider.ActiveProfileChanged += ActiveProfileChanged;
            if (_activeProfileProvider.ActiveProfile != null)
            {
                CaptureRegion = _activeProfileProvider.ActiveProfile.CaptureRegion;
            }
        }

        void ActiveProfileChanged(object sender, ProfileEventArgs e)
        {
            CaptureRegion = e.Profile.CaptureRegion;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
            {
                _activeProfileProvider.ActiveProfileChanged -= ActiveProfileChanged;
            }
        }
    }
}