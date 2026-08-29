using static BackupManager;
using System.Diagnostics;

class BackupUI
{
    public static void Create(Cluster c)
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("BackupCreating"));
        bool success = CreateBackup(c);
        if (success) Console.WriteLine(Loc.Get("BackupCreated"));
        else Console.WriteLine(Loc.Get("Failure"));
        Console.ReadLine();

    }


    public static void Restore(Cluster c)
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("InfoSnapshotCreation"));
        Console.WriteLine();
        if (!ConfirmAction("AskForRestoreBackup", "ConfirmRestoreBackup"))
        {
            Console.WriteLine(Loc.Get("Cancelled"));
            Console.ReadLine();
            return;
        }

        Result result = RestoreBackup(c);
        Console.WriteLine(result.IsSuccess ? Loc.Get("BackupRestored") : Loc.Get(result.ErrorKey ?? "Failure"));
        Console.ReadLine();
    }


    public static void UndoRestore(Cluster c)
    {
        Console.Clear();
        if (!ConfirmAction("AskForUndoRestore", "ConfirmUndoRestore"))
        {
            Console.WriteLine(Loc.Get("Cancelled"));
            Console.ReadLine();
            return;
        }

        Result result = RestoreSnapshot(c);
        Console.WriteLine(result.IsSuccess ? Loc.Get("SnapshotRestored") : Loc.Get(result.ErrorKey ?? "Failure"));
        Console.ReadLine();
    }


    public static bool ConfirmAction(string askKey, string confirmKey)
    {
        Console.WriteLine(Loc.Get(askKey));
        Console.WriteLine(Loc.Get(confirmKey));
        Console.Write(Loc.Get("Select"));
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
            Console.WriteLine(Loc.Get("NoBackupToDisplay"));
            Console.ReadLine();
            return;
        }

        Console.WriteLine(Loc.Format("BackupsOfCluster", c.Name));
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