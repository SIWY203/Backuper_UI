using Backuper_UI.Views;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainWindowViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }

    public MainWindowViewModel()
    {
        AddClusterCommand = new RelayCommand(RunCreator);
        RemoveClusterCommand = new RelayCommand(RunRemover);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
    }

    private void OpenSettings()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.ShowDialog(); // ShowDialog() blokuje, Show() nie
    }

    private void RunCreator()
    {
        var creatorWindow = new ClusterCreatorWindow();
        creatorWindow.ShowDialog();
    }

    private void RunRemover()
    {
        var removerWindow = new ClusterRemoverWindow();
        removerWindow.ShowDialog();
    }
}