using Eventless;

namespace NoteTaker
{
    public class Note
    {
        public readonly ISetable<string> Text = new Setable<string>();
        public readonly ISetable<bool> IsSelected = new Setable<bool>();
    }
}