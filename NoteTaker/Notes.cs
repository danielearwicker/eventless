using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eventless;
using Eventless.WinForms;

namespace NoteTaker
{
    public class Notes
    {
        public readonly ISetableList<Note> AllNotes = new SetableList<Note>();
        public readonly ISetable<Note> ActiveNote = new Setable<Note>();
        public readonly IGetable<IEnumerable<Note>> SelectedNotes;

        public Notes(IEnumerable<string> initialNotes)
        {
            AllNotes.AddRange(initialNotes.Select(
                text => new Note(this) { Text = { Value = text } }));

            SelectedNotes = Computed.From(
                () => AllNotes.Where(n => n.IsSelected.Value).ToList()
            ).Throttle(100);

            Computed.Do(() => Debug.WriteLine("SelectedNotes: " + SelectedNotes.Value.Count()));
        }

        public void Add(string note)
        {
            AllNotes.Add(new Note(this) { Text = { Value = note } });
        }
    }
}