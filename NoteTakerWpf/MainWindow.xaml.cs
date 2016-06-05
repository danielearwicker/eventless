using System.Windows;
using Eventless;
using NoteTaker.Models;

namespace NoteTakerWpf
{    
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel.NewNoteText.Value = "Buy milk";
            ViewModel.AddNote.Execute(null);
        }

        public Notes ViewModel => (Notes) DataContext;
    }
}
