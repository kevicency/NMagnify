using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace NMagnify.Views
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    public partial class AboutView : MetroWindow
    {
        public AboutView()
        {
            InitializeComponent();

            MouseDown += (sender, args) => Close();
            KeyDown += (sender, args) => Close();
        }

        void BrowseSource(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/kmees/NMagnify");
            e.Handled = true;
        }

        void BrowseIssues(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/kmees/NMagnify/issues");
            e.Handled = true;
        }

        void BrowseBlog(object sender, RoutedEventArgs e)
        {
            Process.Start("http://kmees.github.com/");
            e.Handled = true;
        }
    }
}
