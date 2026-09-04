using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterListViewModel : INotifyPropertyChanged
{
    public ObservableCollection<Cluster> Clusters { get; }
    private Cluster? _selectedCluster;
    public Cluster? SelectedCluster
    {
        get => _selectedCluster;
        set
        {
            _selectedCluster = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

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


    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}