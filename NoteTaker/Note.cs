using Eventless;

namespace NoteTaker
{
    public class Note
    {
        public readonly ISetable<string> Text = new Setable<string>();
        public readonly ISetable<bool> IsSelected = new Setable<bool>();
        public readonly ISetable<NotePriority> Priority = new Setable<NotePriority>();
        public readonly ISetable<bool> IsActive;
        
        public Note(Notes notes)
        {
            IsActive = Computed.From(
                get: () => notes.ActiveNote.Value == this,
                set: value =>
                    {
                        if (value)
                            notes.ActiveNote.Value = this;
                        else if (notes.ActiveNote.Value == this)
                            notes.ActiveNote.Value = null;
                    }
            );
        }
    }
}