using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterDetailsViewModel(Cluster cluster)
{
    public Cluster Cluster { get; } = cluster;

}

