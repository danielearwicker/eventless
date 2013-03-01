using Eventless;

namespace NoteTaker
{
    public class Note
    {
        public readonly Writeable<string> Text = new Writeable<string>();
        public readonly Writeable<bool> IsSelected = new Writeable<bool>();
        public readonly Writeable<NotePriority> Priority = new Writeable<NotePriority>();
    }
}