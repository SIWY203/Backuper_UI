using static BackupManager;
using System.Diagnostics;
using Backuper_UI.Services;
namespace Backuper_UI.ViewModels;

class BackupUI
{
    public static void Create(Cluster c)
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["BackupCreating"]);
        bool success = CreateBackup(c);
        if (success) Console.WriteLine(Loc.Instance["BackupCreated"]);
        else Console.WriteLine(Loc.Instance["Failure"]);
        Console.ReadLine();

    }


    public static void Restore(Cluster c)
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["InfoSnapshotCreation"]);
        Console.WriteLine();
        if (!ConfirmAction("AskForRestoreBackup", "ConfirmRestoreBackup"))
        {
            Console.WriteLine(Loc.Instance["Cancelled"]);
            Console.ReadLine();
            return;
        }

        Result result = RestoreBackup(c);
        Console.WriteLine(result.IsSuccess ? Loc.Instance["BackupRestored"] : Loc.Instance[result.ErrorKey ?? "Failure"]);
        Console.ReadLine();
    }


    public static void UndoRestore(Cluster c)
    {
        Console.Clear();
        if (!ConfirmAction("AskForUndoRestore", "ConfirmUndoRestore"))
        {
            Console.WriteLine(Loc.Instance["Cancelled"]);
            Console.ReadLine();
            return;
        }

        Result result = RestoreSnapshot(c);
        Console.WriteLine(result.IsSuccess ? Loc.Instance["SnapshotRestored"] : Loc.Instance[result.ErrorKey ?? "Failure"]);
        Console.ReadLine();
    }


    public static bool ConfirmAction(string askKey, string confirmKey)
    {
        Console.WriteLine(Loc.Instance[askKey]);
        Console.WriteLine(Loc.Instance[confirmKey]);
        Console.Write(Loc.Instance["Select"]);
        string input = Console.ReadLine() ?? "";
        Console.Clear();

        return input.ToUpper() == "Y";
    }


    public static void Show(Cluster c)
    {
        Console.Clear();
        List<string> backups = GetBackups(c);

        if (!AnyBackupExists(c))
        {
            Console.WriteLine(Loc.Instance["NoBackupToDisplay"]);
            Console.ReadLine();
            return;
        }

        Console.WriteLine(Loc.Instance.Format("BackupsOfCluster", c.Name));
        foreach (var backup in backups)
        {
            Console.WriteLine($" - {backup}");
        }

        try
        {
            if (OperatingSystem.IsWindows()) Process.Start("explorer.exe", c.Target);
            else if (OperatingSystem.IsLinux()) Process.Start("xdg-open", c.Target);
            else if (OperatingSystem.IsMacOS()) Process.Start("open", c.Target);
        }
        catch { /*ignore error*/ }

        Console.ReadLine();

    }


}