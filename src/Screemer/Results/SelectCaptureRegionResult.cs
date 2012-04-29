using System;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Views;

namespace Screemer.Results
{
    public class SelectCaptureRegionResult : IResult
    {
        public ISelectCaptureRegion SelectCaptureRegion { get; set; }

        public ScreenRegion ScreenRegion { get; set; }

        public void Execute(ActionExecutionContext context)
        {
            SelectCaptureRegion.ScreenRegionSelected += (sender, args) =>
            {
                ScreenRegion = args.SelectedRegion;
                Completed(this, new ResultCompletionEventArgs());
            };

            SelectCaptureRegion.SelectionCancelled += (sender, args) => 
                Completed(this, new ResultCompletionEventArgs() {WasCancelled = true});

            SelectCaptureRegion.Show();
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}