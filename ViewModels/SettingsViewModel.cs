using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Backuper_UI.Services;
namespace Backuper_UI.ViewModels;

public class SettingsViewModel
{
    public Dictionary<Lang, string> Languages { get; } = new()
    {
        [Lang.PL] = "Polski",
        [Lang.EN] = "English"
    };

    public Lang SelectedLanguage { get; set; } = Loc.Instance.CurrentLang;

    public ICommand SaveCommand { get; }

    public int BackupLimit { get; set; } = Cleaner.CurrentLimit.MaxBackupCount;
    public int SnapshotLimit { get; set; } = Cleaner.CurrentLimit.MaxSnapshotCount;

    public SettingsViewModel()
    {
        SaveCommand = new RelayCommand<Window>(SaveOptions);
    }

    public async void SaveOptions(Window? window)
    {
        Loc.Instance.CurrentLang = SelectedLanguage;
        Cleaner.SetLimit(BackupLimit, Cleaner.Mode.Backup);
        Cleaner.SetLimit(SnapshotLimit, Cleaner.Mode.Snapshot);
        Cleaner.SaveConfig();

        if (window is not null) window.DialogResult = true;
    }
}