using Eventless;

namespace NoteTakerWpf
{
    public class Note
    {
        public IMutable<string> Text { get; } = new Mutable<string>();

        public IMutable<bool> IsSelected { get; } = new Mutable<bool>();        
    }
}