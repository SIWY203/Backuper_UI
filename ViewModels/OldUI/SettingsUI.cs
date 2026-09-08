using static InputManager;
using Backuper_UI.Services;

class SettingsUI
{

    public static void Settings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Instance["HeaderSettings"]);
            Console.WriteLine(Loc.Instance["Language"]);
            Console.WriteLine(Loc.Instance["BackupLimit"]);
            Console.WriteLine();
            Console.WriteLine(Loc.Instance["OptBack"]);
            Console.Write(Loc.Instance["Select"]);

            string input = Console.ReadLine() ?? "";
            if (input.ToUpper() == "Q") return;
            if (input == "1") LanguageSettings();
            if (input == "2") LimitSettings();
        }
    }

    public static void LanguageSettings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Instance["English"]);
            Console.WriteLine(Loc.Instance["Polish"]);
            Console.WriteLine();
            Console.WriteLine(Loc.Instance["OptBack"]);
            Console.Write(Loc.Instance["Select"]);

            string input = Console.ReadLine() ?? "";
            if (input.ToUpper() == "Q") return;

            else if (input == "1") Loc.Instance.CurrentLang = Lang.EN;
            else if (input == "2") Loc.Instance.CurrentLang = Lang.PL;
            else continue;

            Console.Clear();
            Console.WriteLine(Loc.Instance["LanguageSet"]);
            Console.ReadLine();
        }
    }

    public static void LimitSettings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Instance["InfoBackupVsSnapshot"]);
            Console.WriteLine();
            Console.WriteLine(Loc.Instance.Format("ShowCurrentLimits", Cleaner.CurrentLimit.MaxBackupCount, Cleaner.CurrentLimit.MaxSnapshotCount));
            Console.WriteLine();
            Console.WriteLine(Loc.Instance["SetBackupLimit"]);
            Console.WriteLine(Loc.Instance["SetSnapshotLimit"]);
            Console.WriteLine();
            Console.WriteLine(Loc.Instance["OptBack"]);
            Console.Write(Loc.Instance["Select"]);
        
            string input = Console.ReadLine() ?? "";
            if (input.ToUpper() == "Q") return;
            if (!IsWithinScope(input, (1, 2), out int num)) continue;

            Cleaner.Mode mode = num == 1 ? Cleaner.Mode.Backup : Cleaner.Mode.Snapshot;

            Console.Clear();
            Console.Write(Loc.Instance["EnterLimitValue"]);
            string limitInput = Console.ReadLine() ?? "";
            if (!int.TryParse(limitInput, out int limit))
            {
                Console.Clear();
                Console.WriteLine(Loc.Instance["NotNumber"]);
                Console.ReadLine();
                continue;
            }

            Result r = Cleaner.SetLimit(limit, mode);

            Console.Clear();
            if (!r.IsSuccess)
            {
                Console.WriteLine(Loc.Instance[r.ErrorKey ?? "Failure"]);
                Console.ReadLine();
                continue;
            }
            else
            {
                Console.WriteLine(Loc.Instance["NewLimitSet"]);
                Console.ReadLine();
            }
                
        }
        
    }


}

