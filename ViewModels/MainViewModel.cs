using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ObservableCollection<Cluster> Clusters { get; }

    public MainViewModel()
    {
        AddClusterCommand = new RelayCommand(ShowCreator);
        RemoveClusterCommand = new RelayCommand(ShowRemover);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        Clusters = new ObservableCollection<Cluster>(ClusterManager.Clusters);

        ClusterManager.LoadClusters();
        RefreshClusters();
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

        RefreshClusters();
    }

    private void ShowRemover()
    {
        var removerWindow = new ClusterRemoverWindow();
        removerWindow.ShowDialog();
    }

    private void RefreshClusters()
    {
        Clusters.Clear();
        foreach (var cluster in ClusterManager.Clusters) Clusters.Add(cluster);
    }
}