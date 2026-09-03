using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterRemoverViewModel
{
    public ClusterListViewModel ClusterList { get; } = new();

    public ClusterRemoverViewModel()
    {
        ClusterList.Refresh();
    }
}