using Caliburn.Micro.Contrib.Dialogs;

namespace Screemer.Views.Dialog
{
    public class EditProfileDialog : Dialog<Answer>
    {
        public string Name { get; set; }
        public int CPS { get; set; }

        public EditProfileDialog()
            : base(DialogType.None, "Edit Profile", "", Answer.Ok, Answer.Cancel)
        {
            
        }
    }
}