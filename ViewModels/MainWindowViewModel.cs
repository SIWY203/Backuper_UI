using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class MainWindowViewModel
{
    public ICommand AddClusterCommand { get; }
    public ICommand RemoveClusterCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ObservableCollection<Cluster> Clusters { get; }

    public MainWindowViewModel()
    {
        AddClusterCommand = new RelayCommand(RunCreator);
        RemoveClusterCommand = new RelayCommand(RunRemover);
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

    private void RunCreator()
    {
        var creatorWindow = new ClusterCreatorWindow();
        creatorWindow.ShowDialog();

        RefreshClusters();
    }

    private void RunRemover()
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