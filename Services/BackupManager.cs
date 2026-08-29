class BackupManager
{
    public static bool CreateBackup(Cluster c)
    {
        if (!Directory.Exists(c.Source) || !Directory.Exists(c.Target)) return false;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string targetDir = Path.Combine(c.Target, $"{timestamp}_{c.Name}");

        if (!CloneDirectory(c.Source, targetDir))
        {
            CleanupDirectory(targetDir);
            return false;
        }
        Cleaner.Check(c, Cleaner.Mode.Backup); // delete old backups if max count
        return true;
    }


    public static Result RestoreBackup(Cluster c)
    {
        if (!Directory.Exists(c.Source) || !Directory.Exists(c.Target)) return Result.Fail("ErrPathNotExist");

        string[] backups = Directory.GetDirectories(c.Target)
            .Where(dir => !Path.GetFileName(dir).StartsWith("#snapshots", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        if (backups.Length == 0) return Result.Fail("NoBackupToRestore");

        string latest = backups.OrderByDescending(Directory.GetLastWriteTime).First();

        var snapshotResult = CreateSnapshot(c);
        if (!snapshotResult.IsSuccess) return snapshotResult;
        Cleaner.Check(c, Cleaner.Mode.Snapshot);
        return SafeReplaceDirectory(latest, c.Source);
    }


    public static Result CreateSnapshot(Cluster c)
    {
        if (!Directory.Exists(c.Source)) return Result.Fail("ErrSourceNotExist");

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string snapshotDir = Path.Combine(c.Target, "#snapshots", $"snapshot_{timestamp}_{c.Name}");

        if (!CloneDirectory(c.Source, snapshotDir))
        {
            CleanupDirectory(snapshotDir);
            return Result.Fail("ErrSnapshotCreatingFailed");
        }
        return Result.Ok();
    }


    public static Result RestoreSnapshot(Cluster c)
    {
        if (!Directory.Exists(c.Source) || !Directory.Exists(c.Target)) return Result.Fail("ErrPathNotExist");

        string[] snapshots = Directory.GetDirectories(Path.Combine(c.Target, "#snapshots"), "snapshot_*");
        if (snapshots.Length == 0) return Result.Fail("NoSnapshotToRestore");

        string latest = snapshots.OrderByDescending(Directory.GetLastWriteTime).First();
        return SafeReplaceDirectory(latest, c.Source);
    }


    private static bool CloneDirectory(string src, string dest)
    {
        // anti copy-loop
        if (PathHelper.IsSubdirectory(src, dest)) return false;

        try
        {
            Directory.CreateDirectory(dest);

            foreach (string file in Directory.GetFiles(src))
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), true);
            }

            foreach (string dir in Directory.GetDirectories(src))
            {
                if (!CloneDirectory(dir, Path.Combine(dest, Path.GetFileName(dir))))
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }


    private static Result SafeReplaceDirectory(string source, string target)
    {
        char[] separators = [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar];
        string cleanTarget = target.TrimEnd(separators);

        string tempCloned = $"{cleanTarget}_temp_restore";
        string backupOriginal = $"{cleanTarget}_old_backup";

        if (!CloneDirectory(source, tempCloned))
        {
            CleanupDirectory(tempCloned);
            return Result.Fail("ErrReplaceFailed");
        }

        try
        {
            if (Directory.Exists(target))
            {
                CleanupDirectory(backupOriginal);
                Directory.Move(target, backupOriginal);
            }

            Directory.Move(tempCloned, target);
            CleanupDirectory(backupOriginal);
            return Result.Ok();
        }
        catch
        {
            if (!Directory.Exists(target) && Directory.Exists(backupOriginal))
            {
                try { Directory.Move(backupOriginal, target); } catch { }
            }

            CleanupDirectory(tempCloned);
            return Result.Fail("ErrReplaceFailed");
        }
    }


    public static List<string> GetBackups(Cluster c)
    {
        if (!Directory.Exists(c.Target)) return [];
        return Directory.GetDirectories(c.Target)
            .Where(dir => !Path.GetFileName(dir).StartsWith("#snapshots"))
            .OrderByDescending(Directory.GetLastWriteTime)
            .ToList();
    }


    public static bool AnyBackupExists(Cluster c)
    {
        if (!Directory.Exists(c.Target)) return false;

        return Directory.EnumerateDirectories(c.Target)
                        .Any(dir => !Path.GetFileName(dir).StartsWith("#snapshots"));
    }


    private static void CleanupDirectory(string path)
    {
        try
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
        catch
        {
            // ignore errors, no crash
        }
    }

}
