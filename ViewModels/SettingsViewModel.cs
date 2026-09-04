using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
namespace Backuper_UI.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<string> Languages { get; } = ["English", "Polish"];

    public string? SelectedLanguage { get; set; }

    public ICommand SaveCommand { get; }

    public int BackupLimit { get; set; } = Cleaner.CurrentLimit.MaxBackupCount;
    public int SnapshotLimit { get; set; } = Cleaner.CurrentLimit.MaxSnapshotCount;

    private Visibility _successVisibility = Visibility.Hidden;
    public Visibility SuccessVisibility
    {
        get => _successVisibility;
        set
        {
            _successVisibility = value;
            OnPropertyChanged();
        }
    }

    public SettingsViewModel()
    {
        SaveCommand = new RelayCommand(SaveOptions);
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


    public async void SaveOptions()
    {
        // language
        Cleaner.SetLimit(BackupLimit, Cleaner.Mode.Backup);
        Cleaner.SetLimit(SnapshotLimit, Cleaner.Mode.Snapshot);
        Cleaner.SaveConfig();

        SuccessVisibility = Visibility.Visible;
        await Task.Delay(2000);
        SuccessVisibility = Visibility.Collapsed;
    }
}