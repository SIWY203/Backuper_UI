using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
namespace Backuper_UI.ViewModels;

public class SettingsViewModel
{
    public ObservableCollection<string> Languages { get; } = ["English", "Polish"];

    public string? SelectedLanguage { get; set; }

    public ICommand SaveCommand { get; }

    public int BackupLimit { get; set; } = Cleaner.CurrentLimit.MaxBackupCount;
    public int SnapshotLimit { get; set; } = Cleaner.CurrentLimit.MaxSnapshotCount;

    public SettingsViewModel()
    {
        SaveCommand = new RelayCommand<Window>(SaveOptions);
    }

    public async void SaveOptions(Window? window)
    {
        // language
        Cleaner.SetLimit(BackupLimit, Cleaner.Mode.Backup);
        Cleaner.SetLimit(SnapshotLimit, Cleaner.Mode.Snapshot);
        Cleaner.SaveConfig();

        if (window is not null) window.DialogResult = true;
    }
}