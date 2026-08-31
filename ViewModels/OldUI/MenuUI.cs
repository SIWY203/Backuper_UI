using static ClusterManager;
using static InputManager;

namespace Backuper_UI.ViewModels;

class MenuUI
{
    public static void Menu()
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("HeaderMenu"));
        if (Clusters.Count > 0)
        {
            Console.WriteLine(Loc.Get("ClusterList"));
            for (int i = 0; i < Clusters.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {Clusters[i].Name}");
            }
        }

        Console.WriteLine();
        Console.WriteLine(Loc.Get("OptAddCluster"));
        Console.WriteLine(Loc.Get("OptRemoveCluster"));
        Console.WriteLine(Loc.Get("OptSettings"));
        Console.WriteLine(Loc.Get("Quit"));
        Console.Write(Loc.Get("Select"));

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

