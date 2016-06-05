using System.Drawing;
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

            Computed.Do(() => BackColor = note.IsActive.Value
                                    ? SystemColors.Highlight
                                    : SystemColors.Window);

            Computed.Do(() => pictureBox.Image = 
                note.Priority.Value == NotePriority.High ? Properties.Resources.high :
                note.Priority.Value == NotePriority.Low ? Properties.Resources.low :
                null);

            textBoxContent.Click += (s, ev) => note.IsActive.Value = true;
        }
    }
}
