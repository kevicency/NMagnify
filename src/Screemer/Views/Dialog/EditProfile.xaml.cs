using System.Windows;

namespace Screemer.Views.Dialog
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        public EditProfile()
        {
            InitializeComponent();
            Loaded += (sender, args) => Name.Focus();
        }
    }
}
