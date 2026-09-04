using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand OpenDetailsCommand { get; }
    public ClusterListViewModel ClusterList { get; } = new();

    public MainViewModel()
    {
        AddClusterCommand = new RelayCommand(ShowCreator);
        RemoveClusterCommand = new RelayCommand(ShowRemover);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        OpenDetailsCommand = new RelayCommand(OpenDetails, () => ClusterList.SelectedCluster is not null);
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
        ClusterList.Refresh();
    }

    private void OpenDetails()
    {
        if (ClusterList.SelectedCluster is null) return;

        var window = new ClusterDetailsWindow
        {
            DataContext = new ClusterDetailsViewModel(ClusterList.SelectedCluster)
        };

        window.ShowDialog();
    }

}