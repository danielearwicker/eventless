using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Eventless;

namespace NoteTaker.Models
{
    public class Notes
    {
        public IGetable<ObservableCollection<Note>> AllNotes { get; } = new GetableList<Note>();
        
        public IGetable<IEnumerable<Note>> SelectedNotes { get; }

        public ISetable<bool?> SelectAllIsChecked { get; }

        public IGetable<bool> SelectAllIsEnabled { get; }
        
        public ISetable<string> NewNoteText { get; } = new Setable<string>(string.Empty);

        public ICommand AddNote { get; }

        public Notes()
        {            
            SelectedNotes = Computed.From(
                () => AllNotes.Value.Where(n => n.IsSelected.Value).ToList()
            ).Throttle(TimeSpan.FromMilliseconds(10));

            SelectAllIsChecked = Computed.From(
                get: () => !SelectedNotes.Value.Any()
                               ? false
                               : SelectedNotes.Value.Count() == AllNotes.Value.Count
                                     ? true
                                     : default(bool?),
                set: state =>
                {
                    // only pay attention when setting to a definite state
                    if (state == null)
                        return;

                    foreach (var note in AllNotes.Value)
                        note.IsSelected.Value = state == true;
                });

            SelectAllIsEnabled = Computed.From(() => AllNotes.Value.Count != 0);

            AddNote = new GetableCommand(
                execute: () =>
                {
                    var text = NewNoteText.Value;
                    NewNoteText.Value = string.Empty;
                    AllNotes.Value.Add(new Note {Text = {Value = text}});
                }, 
                canExecute: Computed.From(() => NewNoteText.Value.Length != 0));
        }        
    }
}