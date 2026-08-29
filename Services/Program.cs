using static ClusterManager;

class Program
{
    public static void Main(string[] args)
    {
        Loc.LoadLangConfig();
        Cleaner.LoadConfig();
        LoadClusters();
        if (Clusters.Count == 0)
        {
            ClusterUI.RunCreator();
            SaveClusters();
        }

        while (true)
        {
            MenuUI.Menu();
        }
        


    }

}
