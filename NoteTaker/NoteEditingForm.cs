using System.Windows.Forms;
using Eventless;
using Eventless.WinForms;

namespace NoteTaker
{
    public partial class NoteEditingForm : Form, IBindsTo<Note>
    {
        public NoteEditingForm()
        {
            InitializeComponent();
            TopLevel = false;
            Visible = true;
        }

        public void Bind(Note note)
        {
            textBoxContent.BindText(note.Text);
            checkBoxIsSelected.BindChecked(note.IsSelected);

            radioButtonHigh.BindChecked(note.Priority, NotePriority.High);
            radioButtonNormal.BindChecked(note.Priority, NotePriority.Normal);
            radioButtonLow.BindChecked(note.Priority, NotePriority.Low);
        }
    }
}
