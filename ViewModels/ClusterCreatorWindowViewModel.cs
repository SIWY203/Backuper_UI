using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterCreatorWindowViewModel
{
    public string ClusterName { get; set; } = string.Empty;
    public string SourcePath { get; set; } = string.Empty;
    public string TargetPath { get; set; } = string.Empty;
    public ICommand AddCluster { get; }

    public ClusterCreatorWindowViewModel()
    {
        AddCluster = new RelayCommand(Add);
    }

    public void Add()
    {
        Result result = ClusterManager.AddCluster(ClusterName, SourcePath, TargetPath);
        if (result.IsSuccess)
        {
            MessageBox.Show("Cluster added!", "Creator", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            MessageBox.Show(result.ErrorKey ?? "Error!", "Creator", MessageBoxButton.OK, MessageBoxImage.Error);
        }
            
    }
    
}