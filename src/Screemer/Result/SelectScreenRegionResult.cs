using System;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Views;

namespace Screemer.Result
{
    public class SelectScreenRegionResult : IResult
    {
        public ISelectScreenRegion SelectScreenRegion { get; set; }

        public ScreenRegion ScreenRegion { get; set; }

        public void Execute(ActionExecutionContext context)
        {
            SelectScreenRegion.ScreenRegionSelected += (sender, args) =>
            {
                ScreenRegion = args.SelectedRegion;
                Completed(this, new ResultCompletionEventArgs());
            };

            SelectScreenRegion.SelectionCancelled += (sender, args) => 
                Completed(this, new ResultCompletionEventArgs() {WasCancelled = true});

            SelectScreenRegion.Show();
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };
    }
}