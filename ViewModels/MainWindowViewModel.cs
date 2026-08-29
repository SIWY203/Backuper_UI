using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainWindowViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public MainWindowViewModel()
    {
        AddClusterCommand = new RelayCommand(ClusterUI.RunCreator);
        RemoveClusterCommand = new RelayCommand(ClusterUI.RunRemover);
        OpenSettingsCommand = new RelayCommand(SettingsUI.Settings);
    }
}