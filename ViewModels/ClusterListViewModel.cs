using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterListViewModel
{
    public ObservableCollection<Cluster> Clusters { get; }

    public ClusterListViewModel()
    {
        Clusters = new ObservableCollection<Cluster>(ClusterManager.Clusters);

        ClusterManager.LoadClusters();
        Refresh();
    }

    public void Refresh()
    {
        Clusters.Clear();
        foreach (var cluster in ClusterManager.Clusters) Clusters.Add(cluster);
    }
}