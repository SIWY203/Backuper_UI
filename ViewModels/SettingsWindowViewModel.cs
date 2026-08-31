using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class SettingsWindowViewModel
{
    // Kolekcja automatycznie odświeżająca widok po dodaniu/usunięciu elementów
    public ObservableCollection<string> Languages { get; } = ["English", "Polish"];

    public string? SelectedLanguage { get; set; }

    public int BackupLimit { get; set; } = Cleaner.CurrentLimit.MaxBackupCount;
    public int SnapshotLimit { get; set; } = Cleaner.CurrentLimit.MaxSnapshotCount;

    public SettingsWindowViewModel()
    {
        
    }
}