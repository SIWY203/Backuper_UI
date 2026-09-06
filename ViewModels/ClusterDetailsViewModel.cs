using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterDetailsViewModel
{
    public ICommand CreateBackupCommand { get; }
    public Cluster Cluster { get; }
    public ClusterDetailsViewModel(Cluster cluster)
    {
        Cluster = cluster;
        CreateBackupCommand = new RelayCommand(CreateBackup);
    }

    public void CreateBackup()
    {

    }

}

