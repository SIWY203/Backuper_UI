using System.Text.Json;

class Cleaner
{
    public static CopyLimit CurrentLimit { get; private set; } = LoadConfig() ?? new CopyLimit(3, 2);


    public enum Mode { Backup, Snapshot }


    public static CopyLimit? LoadConfig()
    {
        if (!File.Exists(ConfigManager.LimitsConfigFile)) return null;

        try
        {
            string json = File.ReadAllText(ConfigManager.LimitsConfigFile);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CopyLimit>(json, options);
        }
        catch
        {
            return null;
        }
    }


    public static void SaveConfig()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(CurrentLimit, options);
        File.WriteAllText(ConfigManager.LimitsConfigFile, json);
    }


    public static Result SetLimit(int limit, Mode mode)
    {
        if (limit < 1 || limit > 999)
        {
            return Result.Fail("ErrNumberOutOfRange");
        }
        if (mode == Mode.Backup) CurrentLimit = CurrentLimit with { MaxBackupCount = limit };
        if (mode == Mode.Snapshot) CurrentLimit = CurrentLimit with { MaxSnapshotCount = limit };
        SaveConfig();

        return Result.Ok();
    }


    public static void Check(Cluster cluster, Mode mode)
    {
        int limit = mode == Mode.Backup ? CurrentLimit.MaxBackupCount : CurrentLimit.MaxSnapshotCount;
        string path = mode == Mode.Backup ? cluster.Target : Path.Combine(cluster.Target, "#snapshots");

        if (!Directory.Exists(path)) return;
        int count = mode switch
        {
            Mode.Backup => Directory.EnumerateDirectories(path)
                                    .Count(dir => !Path.GetFileName(dir)
                                    .StartsWith("#snapshots", StringComparison.OrdinalIgnoreCase)),

            Mode.Snapshot => Directory.EnumerateDirectories(path, "snapshot_*").Count(),

            _ => 0
        };

        if (count > limit)
        {
            int toRemoveCount = count - limit;
            for (int i = 0; i < toRemoveCount; i++)
            {
                CleanUp(cluster, mode);
            }
        }
    }


    public static void CleanUp(Cluster cluster, Mode mode)
    {
        string path = mode == Mode.Backup ? cluster.Target : Path.Combine(cluster.Target, "#snapshots");

        if (!Directory.Exists(path)) return;
        IEnumerable<string> directories = mode switch
        {
            Mode.Backup => Directory.EnumerateDirectories(path)
                                    .Where(dir => !Path.GetFileName(dir)
                                    .StartsWith("#snapshots", StringComparison.OrdinalIgnoreCase)),

            Mode.Snapshot => Directory.EnumerateDirectories(path, "snapshot_*"),

            _ => []
        };

        string? oldest = directories.OrderBy(Directory.GetLastWriteTime).FirstOrDefault();

        if (oldest is not null)
        {
            try
            {
                Directory.Delete(oldest, recursive: true);
            }
            catch
            {
                // Ignorowanie ewentualnych błędów dostępu/zablokowanych plików
            }
        }
    }

}
