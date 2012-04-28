using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Media;
using Caliburn.Micro;
using Screemer.Model;

namespace Screemer.ViewModels 
{
    public class ShellViewModel : Screen, IShell
    {

        protected override void OnActivate()
        {
            base.OnActivate();
            IoC.Get<IWindowManager>().ShowWindow(IoC.Get<CapturedScreenViewModel>());
        }
    }
}
