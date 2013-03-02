using System;
using System.Linq;
using System.Windows.Forms;
using Eventless;
using Eventless.WinForms;

namespace NoteTaker
{
    public partial class NotesForm : Form
    {
        public NotesForm()
        {
            InitializeComponent();

            var notes = new SetableList<Note>
                {
                    new Note {Text = {Value = "Hello "}},
                    new Note {Text = {Value = "Goodbye"}},
                    new Note {Text = {Value = "Lots and lots"}},
                    new Note {Text = {Value = "Of exciting notes"}},
                    new Note {Text = {Value = "You can select them"}},
                    new Note {Text = {Value = "And then delete them"}},
                    new Note {Text = {Value = "Or select all of them"}},
                    new Note {Text = {Value = "Then un-select particular ones"}},
                    new Note {Text = {Value = "It may not look like much"}},
                    new Note {Text = {Value = "But it keeps me off the streets"}},
                    new Note {Text = {Value = "And out of prison"}},
                    new Note {Text = {Value = "Which has to be good news for taxpayers"}},
                    new Note {Text = {Value = "This is probably enough example notes"}},
                    new Note {Text = {Value = "Unless you want to hear about my difficult upbringing?"}},
                    new Note {Text = {Value = "Wait... come back..."}},
                    new Note {Text = {Value = "Typical."}},
                };
            
            // For each each note, make a CheckedListBoxItem
            panelSelectionList.BindForEach(notes).As<NoteListItemForm>();

            var newNoteText = Setable.From(string.Empty);
            textBoxNewNote.BindText(newNoteText);

            // Add button only enabled if text box is non-empty
            buttonAdd.BindEnabled(Computed.From(() => newNoteText.Value.Length != 0));
            buttonAdd.Click += (s, ev) =>
            {
                notes.Add(new Note { Text = { Value = newNoteText.Value } });
                textBoxNewNote.Text = string.Empty;
                textBoxNewNote.Focus();
            };

            // list of currently selected notes (important: ToList to avoid lazy evaluation)
            var selectedNotes = Computed.From(() => notes.Where(n => n.IsSelected.Value).ToList());

            // Two-way binding
            checkBoxAllNotes.BindCheckState(Computed.From(

                get: () => selectedNotes.Value.Count == 0
                               ? CheckState.Unchecked
                               : selectedNotes.Value.Count == notes.Count
                                     ? CheckState.Checked
                                     : CheckState.Indeterminate,

                set: state =>
                    {
                        // only pay attention when setting to a definite state
                        if (state == CheckState.Indeterminate)
                            return;

                        foreach (var note in notes)
                            note.IsSelected.Value = state == CheckState.Checked;
                    }));

            checkBoxAllNotes.BindEnabled(Computed.From(() => notes.Count != 0));

            // Delete button only enabled if selection is non-empty
            buttonDelete.BindEnabled(Computed.From(() => selectedNotes.Value.Count != 0));
            buttonDelete.Click += (s, ev) => notes.RemoveAll(selectedNotes.Value);
        }
    }
}