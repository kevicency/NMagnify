using System;
using Caliburn.Micro;
using NMagnify.Controls;
using NMagnify.Model;

namespace NMagnify.Results
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