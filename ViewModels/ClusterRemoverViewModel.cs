using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterRemoverViewModel
{
    public ICommand RemoveClusterCommand { get; }
    public ClusterListViewModel ClusterList { get; } = new();

    public ClusterRemoverViewModel()
    {
        RemoveClusterCommand = new RelayCommand(RemoveCluster);
        ClusterList.Refresh();
    }

    private void RemoveCluster()
    {
        if (ClusterList.SelectedCluster is null) return;
        ClusterManager.RemoveCluster(ClusterList.SelectedCluster);
        ClusterList.Refresh();
    }

}
