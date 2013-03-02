using System;
using System.Linq;
using System.Windows.Forms;
using Eventless;
using Eventless.WinForms;

namespace NoteTaker
{
    public partial class NotesForm : Form, IBindsTo<Notes>
    {
        public NotesForm()
        {
            InitializeComponent();
        }

        public void Bind(Notes notes)
        {
            // For each each note, make a CheckedListBoxItem
            panelSelectionList.BindForEach(notes.AllNotes)
                              .As<NoteListItemForm>();

            var newNoteText = Setable.From(string.Empty);
            textBoxNewNote.BindText(newNoteText);

            // Add button only enabled if text box is non-empty
            buttonAdd.BindEnabled(Computed.From(() => newNoteText.Value.Length != 0));
            buttonAdd.Click += (s, ev) =>
            {
                notes.Add(newNoteText.Value);
                textBoxNewNote.Text = string.Empty;
                textBoxNewNote.Focus();
            };

            // Two-way binding
            checkBoxAllNotes.BindCheckState(Computed.From(
                get: () => !notes.SelectedNotes.Value.Any()
                               ? CheckState.Unchecked
                               : notes.SelectedNotes.Value.Count() == notes.AllNotes.Count
                                     ? CheckState.Checked
                                     : CheckState.Indeterminate,
                set: state =>
                {
                    // only pay attention when setting to a definite state
                    if (state == CheckState.Indeterminate)
                        return;

                    foreach (var note in notes.AllNotes)
                        note.IsSelected.Value = state == CheckState.Checked;
                }));

            checkBoxAllNotes.BindEnabled(Computed.From(() => notes.AllNotes.Count != 0));

            // Delete button only enabled if selection is non-empty
            buttonDelete.BindEnabled(Computed.From(() => notes.SelectedNotes.Value.Any()));
            buttonDelete.Click += (s, ev) => notes.AllNotes.RemoveAll(notes.SelectedNotes.Value);

            // Similar for priorities
            Action<Button, NotePriority> bindPriorityButton = (button, priority) =>
                {
                    button.BindEnabled(Computed.From(
                        () => notes.SelectedNotes.Value.Any(note => note.Priority.Value != priority)));

                    button.Click += (s, ev) =>
                    {
                        foreach (var note in notes.SelectedNotes.Value)
                            note.Priority.Value = priority;
                    };
                };

            bindPriorityButton(buttonLow, NotePriority.Low);
            bindPriorityButton(buttonNormal, NotePriority.Normal);
            bindPriorityButton(buttonHigh, NotePriority.High);

            panelActiveNote.BindContent(notes.ActiveNote).As<NoteEditingForm>();

            panelEditors.BindForEach(notes.AllNotes).As<NoteEditingForm>();
        }
    }
}