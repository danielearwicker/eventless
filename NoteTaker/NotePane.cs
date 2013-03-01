using System.Windows.Forms;
using Eventless.WinForms;

namespace NoteTaker
{
    public partial class NotePane : Form
    {
        public NotePane()
        {
            InitializeComponent();
            TopLevel = false;
            Visible = true;
        }

        public void Bind(Note note)
        {
            textBoxNoteText.BindText(note.Text);

            checkBoxSelected.BindChecked(note.IsSelected);

            radioButtonPriorityHigh.BindChecked(note.Priority, NotePriority.High);
            radioButtonPriorityNormal.BindChecked(note.Priority, NotePriority.Normal);
            radioButtonPriorityLow.BindChecked(note.Priority, NotePriority.Low);
        }
    }
}
