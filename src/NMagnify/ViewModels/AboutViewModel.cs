using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;

namespace NMagnify.ViewModels
{
    public class AboutViewModel : Screen
    {
        public DateTime ReleaseDate { get; set; }
        public Version Version { get; set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ReleaseDate = ApplicationInformation.CompileDate;
            Version = ApplicationInformation.ExecutingAssemblyVersion;
        }
    }
}
