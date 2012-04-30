using System;
using System.IO;
using System.Windows;

namespace NMagnify
{
    /// <summary>
    ///   Interaction logic for App.xaml
    /// </summary>
    public class App : Application
    {
        public static string Name
        {
            get { return "NMagnify"; }
        }

        public static string AppDataFolder
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                return Path.Combine(appDataFolder, Name);
            }
        }
    }
}