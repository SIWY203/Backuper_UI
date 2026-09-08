using static ClusterManager;
using static InputManager;
using Backuper_UI.Services;

namespace Backuper_UI.ViewModels;

class MenuUI
{
    public static void Menu()
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["HeaderMenu"]);
        if (Clusters.Count > 0)
        {
            Console.WriteLine(Loc.Instance["ClusterList"]);
            for (int i = 0; i < Clusters.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {Clusters[i].Name}");
            }
        }

        Console.WriteLine();
        Console.WriteLine(Loc.Instance["OptAddCluster"]);
        Console.WriteLine(Loc.Instance["OptRemoveCluster"]);
        Console.WriteLine(Loc.Instance["OptSettings"]);
        Console.WriteLine(Loc.Instance["Quit"]);
        Console.Write(Loc.Instance["Select"]);

        string input = Console.ReadLine() ?? "";
        if (IsWithinScope(input, Clusters, out int num))
        {
            ClusterUI.Details(Clusters[num-1]);
        }
        if (input.ToLower() == "a") ClusterUI.RunCreator();
        if (input.ToLower() == "r") ClusterUI.RunRemover();
        if (input.ToLower() == "s") SettingsUI.Settings();
        if (input.ToLower() == "q") Environment.Exit(0);

    }

}

