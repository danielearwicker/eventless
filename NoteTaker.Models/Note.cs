using Eventless;

namespace NoteTaker.Models
{
    public class Note
    {
        public ISetable<string> Text { get; } = new Setable<string>();

        public ISetable<bool> IsSelected { get; } = new Setable<bool>();        
    }
}