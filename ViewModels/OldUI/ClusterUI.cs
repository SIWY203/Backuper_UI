using static ClusterManager;
using static InputManager;

namespace Backuper_UI.ViewModels;

class ClusterUI
{
    public static void RunCreator()
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("InfoCanDropDir"));
        Console.WriteLine();
        Console.Write(Loc.Get("Ok"));
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine(Loc.Get("HeaderCreator"));
        Console.Write(Loc.Get("EnterClusterName"));
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Get("EnterClusterSource"));
        string source = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Get("EnterClusterTarget"));
        string target = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Get("EnterClusterTarget"));
        string description = Console.ReadLine() ?? string.Empty;  

        Result result = AddCluster(name, source, target, description);
        if (result.IsSuccess) Console.WriteLine(Loc.Format("ClusterAdded", name));
        else Console.WriteLine(Loc.Get(result.ErrorKey ?? "Failure"));

        Console.ReadLine();
    }


    public static void RunRemover()
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("HeaderCreator"));
        Console.WriteLine(Loc.Get("SelectToRemove"));
        for (int i = 0; i < Clusters.Count; i++)
        {
            Console.WriteLine($"[{i+1}] {Clusters[i].Name}");
        }

        Console.Write(Loc.Get("Select"));
        string input = Console.ReadLine() ?? "";
        if (IsWithinScope(input, Clusters, out int num))
        {
            Cluster c = Clusters[num-1];

            if (!ConfirmRemove())
            {
                Console.WriteLine(Loc.Get("Cancelled"));
                Console.ReadLine();
                return;
            }

            RemoveCluster(c);
        }

    }


    public static bool ConfirmRemove()
    {
        Console.Clear();
        Console.WriteLine(Loc.Get("AskToRemoveCluster"));
        Console.WriteLine(Loc.Get("ConfirmRemoveCluster"));
        Console.Write(Loc.Get("Select"));
        string input = Console.ReadLine() ?? "";
        Console.Clear();

        if (input.ToUpper() == "Y") return true;
        return false;
    }


    public static void Details(Cluster c)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Format("ClusterDetails", c.Name, c.Source, c.Target));
            Console.WriteLine(Loc.Get("OptEditCluster"));
            Console.WriteLine(Loc.Get("OptCreateBackup"));
            Console.WriteLine(Loc.Get("OptRestoreBackup"));
            Console.WriteLine(Loc.Get("OptUndoRestore"));
            Console.WriteLine(Loc.Get("OptShowBackups"));
            Console.WriteLine(Loc.Get("OptBack"));

            Console.Write(Loc.Get("Select"));
            string input = Console.ReadLine() ?? "";
            if (input.ToLower() == "e")
            {
                c = RunEditor(c); // get edited data to show new details
                continue;         // instantly refresh loop
            }
                
            if (input.ToLower() == "q") return;
            if (!IsWithinScope(input, (1, 4), out int num)) continue;
            switch (num)
            {
                case 1:
                    BackupUI.Create(c);
                    break;
                case 2:
                    BackupUI.Restore(c);
                    break;
                case 3:
                    BackupUI.UndoRestore(c);
                    break;
                case 4:
                    BackupUI.Show(c);
                    break;
                default:
                    break;
            }
            //return; // leave after action
        }
    }


    public static Cluster RunEditor(Cluster c)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Get("HeaderClusterEditor"));
            Console.WriteLine(Loc.Format("ClusterDetails", c.Name, c.Source, c.Target));
            Console.WriteLine(Loc.Get("OptUpdateClusterName"));
            Console.WriteLine(Loc.Get("OptUpdateClusterSource"));
            Console.WriteLine(Loc.Get("OptUpdateClusterTarget"));
            Console.WriteLine(Loc.Get("OptBack"));

            Console.Write(Loc.Get("Select"));
            string input = Console.ReadLine() ?? "";
            if (input.ToLower() == "q") return c;
            if (!IsWithinScope(input, (1, 3), out int num)) continue;

            Console.Clear();
            Console.WriteLine(Loc.Get("HeaderClusterEditor"));
            switch (num)
            {
                case 1:
                    Console.Write(Loc.Get("EnterClusterName"));
                    string newName = Console.ReadLine() ?? "";
                    var (result, updatedName) = UpdateClusterName(c, newName);
                    if (updatedName != null)
                    {
                        c = updatedName;
                        Console.WriteLine(Loc.Get("UpdateNameSuccess"));
                    }
                    else Console.WriteLine(Loc.Get(result.ErrorKey ?? "Failure"));
                    Console.ReadLine();
                    break;

                case 2:
                    Console.Write(Loc.Get("EnterClusterSource"));
                    string newSource = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(newSource))
                    {
                        Console.WriteLine(Loc.Get("ErrEmptyField"));
                        Console.ReadLine();
                        break;
                    }
                    if (c.Target.StartsWith(newSource, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Loc.Get("ErrSubfolder"));
                        Console.ReadLine();
                        break;
                    }
                    Cluster? updatedSource = UpdateClusterSource(c, newSource);
                    if (updatedSource != null)
                    {
                        c = updatedSource;
                        Console.WriteLine(Loc.Get("UpdatePathSuccess"));
                    }
                    else Console.WriteLine(Loc.Get("Failure"));
                    Console.ReadLine();
                    break;

                case 3:
                    Console.Write(Loc.Get("EnterClusterTarget"));
                    string newTarget = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(newTarget))
                    {
                        Console.WriteLine(Loc.Get("ErrEmptyField"));
                        Console.ReadLine();
                        break;
                    }
                    if (newTarget.StartsWith(c.Source, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Loc.Get("ErrSubfolder"));
                        Console.ReadLine();
                        break;
                    }
                    Cluster? updatedTarget = UpdateClusterTarget(c, newTarget);
                    if (updatedTarget != null)
                    {
                        c = updatedTarget;
                        Console.WriteLine(Loc.Get("UpdatePathSuccess"));
                    }
                    else Console.WriteLine(Loc.Get("Failure"));
                    Console.ReadLine();
                    break;

                default:
                    break;
            }
        }
        

    }


}

