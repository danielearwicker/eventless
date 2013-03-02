using System;
using System.Windows.Forms;
using Eventless;

namespace NoteTaker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var initialNotes = new[]
                {
                    "Hello ",
                    "Goodbye",
                    "Lots and lots",
                    "Of exciting notes",
                    "You can select them",
                    "And then delete them",
                    "Or select all of them",
                    "Then un-select particular ones",
                    "It may not look like much",
                    "But it keeps me off the streets",
                    "And out of prison",
                    "Which has to be good news for taxpayers",
                    "This is probably enough example notes",
                    "Unless you want to hear about my difficult upbringing?",
                    "Wait... come back...",
                    "Typical."
                };


            var notes = new Notes(initialNotes);
            
            var notesForm = new NotesForm();
            notesForm.Bind(notes);

            Application.Run(notesForm);
        }
    }
}
