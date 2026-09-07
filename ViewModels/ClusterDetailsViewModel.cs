using Backuper_UI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
namespace Backuper_UI.ViewModels;

public class ClusterDetailsViewModel
{
    public ICommand CreateBackupCommand { get; }
    public ICommand RestoreBackupCommand { get; }
    public ICommand UndoRestoreCommand { get; }
    public ICommand ShowBackupsCommand { get; }
    public Cluster Cluster { get; }
    public ClusterDetailsViewModel(Cluster cluster)
    {
        Cluster = cluster;
        CreateBackupCommand = new RelayCommand(CreateBackup);
        RestoreBackupCommand = new RelayCommand(RestoreBackup);
        UndoRestoreCommand = new RelayCommand(UndoRestore);
        ShowBackupsCommand = new RelayCommand(ShowBackups);
    }

    public void CreateBackup()
    {
        if (BackupManager.CreateBackup(Cluster))
        {
            MessageBox.Show("Backup created!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else MessageBox.Show("Error!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void RestoreBackup() => ConfirmAndExecute( // msg, title, action
        $"Czy na pewno chcesz przywrócić backup klastra '{Cluster.Name}'?",
        "Przywracanie backupu",
        () =>
        {
            Result result = BackupManager.RestoreBackup(Cluster);
            if (result.IsSuccess)
            {
                MessageBox.Show("Backup restored!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else MessageBox.Show(result.ErrorKey ?? "Error!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Error);
        });

    public void UndoRestore() => ConfirmAndExecute(
        $"Czy na pewno chcesz cofnąć przywracanie klastra '{Cluster.Name}'?",
        "Cofanie przywracania",
        () =>
        {
            Result result = BackupManager.RestoreSnapshot(Cluster);
            if (result.IsSuccess)
            {
                MessageBox.Show("Backup restored!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(result.ErrorKey ?? "Error!", "Backuper", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

    public void ShowBackups()
    {
        List<string> backups = BackupManager.GetBackups(Cluster);
        try
        {
            if (OperatingSystem.IsWindows()) Process.Start("explorer.exe", Cluster.Target);
            else if (OperatingSystem.IsLinux()) Process.Start("xdg-open", Cluster.Target);
            else if (OperatingSystem.IsMacOS()) Process.Start("open", Cluster.Target);
        }
        catch { /*ignore error*/ }

    }


    private static void ConfirmAndExecute(string message, string title, Action action)
    {
        if (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        {
            action();
        }
    }



}

