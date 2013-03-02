using System.Windows.Forms;
using Eventless;
using Eventless.WinForms;

namespace NoteTaker
{
    public partial class NoteListItemForm : Form, IBindsTo<Note>
    {
        public NoteListItemForm()
        {
            InitializeComponent();
            TopLevel = false;
            Visible = true;
        }

        public void Bind(Note note)
        {
            textBoxContent.BindText(note.Text);
            checkBoxIsSelected.BindChecked(note.IsSelected);
        }
    }
}
