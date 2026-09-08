using static ClusterManager;
using static InputManager;
using Backuper_UI.Services;
namespace Backuper_UI.ViewModels;

class ClusterUI
{
    public static void RunCreator()
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["InfoCanDropDir"]);
        Console.WriteLine();
        Console.Write(Loc.Instance["Ok"]);
        Console.ReadLine();
        Console.Clear();
        Console.WriteLine(Loc.Instance["HeaderCreator"]);
        Console.Write(Loc.Instance["EnterClusterName"]);
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Instance["EnterClusterSource"]);
        string source = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Instance["EnterClusterTarget"]);
        string target = Console.ReadLine() ?? string.Empty;

        Console.Write(Loc.Instance["EnterClusterTarget"]);
        string description = Console.ReadLine() ?? string.Empty;  

        Result result = AddCluster(name, source, target, description);
        if (result.IsSuccess) Console.WriteLine(Loc.Instance.Format("ClusterAdded", name));
        else Console.WriteLine(Loc.Instance[result.ErrorKey ?? "Failure"]);

        Console.ReadLine();
    }


    public static void RunRemover()
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["HeaderCreator"]);
        Console.WriteLine(Loc.Instance["SelectToRemove"]);
        for (int i = 0; i < Clusters.Count; i++)
        {
            Console.WriteLine($"[{i+1}] {Clusters[i].Name}");
        }

        Console.Write(Loc.Instance["Select"]);
        string input = Console.ReadLine() ?? "";
        if (IsWithinScope(input, Clusters, out int num))
        {
            Cluster c = Clusters[num-1];

            if (!ConfirmRemove())
            {
                Console.WriteLine(Loc.Instance["Cancelled"]);
                Console.ReadLine();
                return;
            }

            RemoveCluster(c);
        }

    }


    public static bool ConfirmRemove()
    {
        Console.Clear();
        Console.WriteLine(Loc.Instance["AskToRemoveCluster"]);
        Console.WriteLine(Loc.Instance["ConfirmRemoveCluster"]);
        Console.Write(Loc.Instance["Select"]);
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
            Console.WriteLine(Loc.Instance.Format("ClusterDetails", c.Name, c.Source, c.Target));
            Console.WriteLine(Loc.Instance["OptEditCluster"]);
            Console.WriteLine(Loc.Instance["OptCreateBackup"]);
            Console.WriteLine(Loc.Instance["OptRestoreBackup"]);
            Console.WriteLine(Loc.Instance["OptUndoRestore"]);
            Console.WriteLine(Loc.Instance["OptShowBackups"]);
            Console.WriteLine(Loc.Instance["OptBack"]);

            Console.Write(Loc.Instance["Select"]);
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
            Console.WriteLine(Loc.Instance["HeaderClusterEditor"]);
            Console.WriteLine(Loc.Instance.Format("ClusterDetails", c.Name, c.Source, c.Target));
            Console.WriteLine(Loc.Instance["OptUpdateClusterName"]);
            Console.WriteLine(Loc.Instance["OptUpdateClusterSource"]);
            Console.WriteLine(Loc.Instance["OptUpdateClusterTarget"]);
            Console.WriteLine(Loc.Instance["OptBack"]);

            Console.Write(Loc.Instance["Select"]);
            string input = Console.ReadLine() ?? "";
            if (input.ToLower() == "q") return c;
            if (!IsWithinScope(input, (1, 3), out int num)) continue;

            Console.Clear();
            Console.WriteLine(Loc.Instance["HeaderClusterEditor"]);
            switch (num)
            {
                case 1:
                    Console.Write(Loc.Instance["EnterClusterName"]);
                    string newName = Console.ReadLine() ?? "";
                    var (result, updatedName) = UpdateClusterName(c, newName);
                    if (updatedName != null)
                    {
                        c = updatedName;
                        Console.WriteLine(Loc.Instance["UpdateNameSuccess"]);
                    }
                    else Console.WriteLine(Loc.Instance[result.ErrorKey ?? "Failure"]);
                    Console.ReadLine();
                    break;

                case 2:
                    Console.Write(Loc.Instance["EnterClusterSource"]);
                    string newSource = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(newSource))
                    {
                        Console.WriteLine(Loc.Instance["ErrEmptyField"]);
                        Console.ReadLine();
                        break;
                    }
                    if (c.Target.StartsWith(newSource, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Loc.Instance["ErrSubfolder"]);
                        Console.ReadLine();
                        break;
                    }
                    Cluster? updatedSource = UpdateClusterSource(c, newSource);
                    if (updatedSource != null)
                    {
                        c = updatedSource;
                        Console.WriteLine(Loc.Instance["UpdatePathSuccess"]);
                    }
                    else Console.WriteLine(Loc.Instance["Failure"]);
                    Console.ReadLine();
                    break;

                case 3:
                    Console.Write(Loc.Instance["EnterClusterTarget"]);
                    string newTarget = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(newTarget))
                    {
                        Console.WriteLine(Loc.Instance["ErrEmptyField"]);
                        Console.ReadLine();
                        break;
                    }
                    if (newTarget.StartsWith(c.Source, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine(Loc.Instance["ErrSubfolder"]);
                        Console.ReadLine();
                        break;
                    }
                    Cluster? updatedTarget = UpdateClusterTarget(c, newTarget);
                    if (updatedTarget != null)
                    {
                        c = updatedTarget;
                        Console.WriteLine(Loc.Instance["UpdatePathSuccess"]);
                    }
                    else Console.WriteLine(Loc.Instance["Failure"]);
                    Console.ReadLine();
                    break;

                default:
                    break;
            }
        }
        

    }


}

