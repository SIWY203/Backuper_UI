using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ClusterListViewModel ClusterList { get; } = new();

    public MainViewModel()
    {
        AddClusterCommand = new RelayCommand(ShowCreator);
        RemoveClusterCommand = new RelayCommand(ShowRemover);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        ClusterList.Refresh();
    }

    private void OpenSettings()
    {
        var settingsWindow = new SettingsWindow();
        settingsWindow.ShowDialog(); // ShowDialog() blokuje, Show() nie
    }

    private void ShowCreator()
    {
        var creatorWindow = new ClusterCreatorWindow();
        creatorWindow.ShowDialog();

        ClusterList.Refresh();
    }

    private void ShowRemover()
    {
        var removerWindow = new ClusterRemoverWindow();
        removerWindow.ShowDialog();
    }

}