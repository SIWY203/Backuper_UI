using System.Text.Json;
using static ConfigManager;

class ClusterManager
{
    public static List<Cluster> Clusters = new();

    public static Result AddCluster(string name, string source, string target)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(source) || string.IsNullOrWhiteSpace(target))
        {
            return Result.Fail("ErrEmptyFields");
        }

        Result nameCheck = IsNameAvailable(name);
        if (!nameCheck.IsSuccess) return nameCheck;

        string fullSource = Path.GetFullPath(source);
        string fullTarget = Path.GetFullPath(target);
        if (string.Equals(fullSource, fullTarget, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Fail("ErrSameDirectory");
        }

        if (PathHelper.IsSubdirectory(source, target))
        {
            return Result.Fail("ErrSubfolder");
        }

        Cluster newCluster = new Cluster(name, source, target);

        if (Clusters.Contains(newCluster)) return Result.Fail("ErrClusterAlreadyExist");
        Clusters.Add(newCluster);
        if (!SaveClusters()) return Result.Fail("Failure");
        return Result.Ok();
    }


    public static bool RemoveCluster(Cluster cluster)
    {
        if (!Clusters.Contains(cluster)) return false;
        Clusters.Remove(cluster);
        return SaveClusters();
        
    }


    public static bool SaveClusters()
    {
        try
        {
            EnsureDirectoryExists(ClusterConfigFile);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Clusters, options);
            File.WriteAllText(ClusterConfigFile, json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }


    public static void LoadClusters()
    {
        if (!File.Exists(ClusterConfigFile)) return;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var json = File.ReadAllText(ClusterConfigFile);
        Clusters = JsonSerializer.Deserialize<List<Cluster>>(json, options) ?? new List<Cluster>();
    }


    public static (Result r, Cluster? c) UpdateClusterName(Cluster current, string newName)
    {
        newName = newName.Trim();
        if (string.IsNullOrWhiteSpace(newName)) return (Result.Fail("ErrEmptyField"), null);

        Result nameCheck = IsNameAvailable(newName);
        if (!nameCheck.IsSuccess) return (nameCheck, null);

        int index = Clusters.IndexOf(current);
        if (index == -1) return (Result.Fail("Failure"), null);

        Cluster newCluster = current with { Name = newName };
        Clusters[index] = newCluster;
        return SaveClusters() ? (Result.Ok(), newCluster) : (Result.Fail("Failure"), null);
    }

    public static Cluster? UpdateClusterSource(Cluster current, string newSource)
    {
        newSource = newSource.Trim();
        if (string.IsNullOrWhiteSpace(newSource) || !Directory.Exists(newSource)) return null;

        int index = Clusters.IndexOf(current);
        if (index == -1) return null;

        Cluster newCluster = current with { Source = newSource };
        Clusters[index] = newCluster;
        return SaveClusters() ? newCluster : null;
    }

    public static Cluster? UpdateClusterTarget(Cluster current, string newTarget)
    {
        newTarget = newTarget.Trim();
        if (string.IsNullOrWhiteSpace(newTarget) || !Directory.Exists(newTarget)) return null;

        int index = Clusters.IndexOf(current);
        if (index == -1) return null;

        Cluster newCluster = current with { Target = newTarget };
        Clusters[index] = newCluster;
        return SaveClusters() ? newCluster : null;
    }


    private static void EnsureDirectoryExists(string filePath)
    {
        string? dirPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    private static Result IsNameAvailable(string name)
    {
        if (string.IsNullOrEmpty(name)) return Result.Fail("Failure");
        if (Clusters.Any(c => c.Name == name)) return Result.Fail("NameIsTaken");
        return Result.Ok();
    }

}
