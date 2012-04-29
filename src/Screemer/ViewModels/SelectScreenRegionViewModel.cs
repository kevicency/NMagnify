using System.Drawing;
using Caliburn.Micro;
using Screemer.Views;

namespace Screemer.ViewModels
{
    public class SelectScreenRegionViewModel : Screen
    {
        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            (view as ISelectScreenRegion).ScreenRegionSelected += (sender, args) =>
            {
                SelectedRegion = args.SelectedRegion;
            };
        }

        protected Rectangle SelectedRegion { get; set; }
    }
}