using static InputManager;

class SettingsUI
{

    public static void Settings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Get("HeaderSettings"));
            Console.WriteLine(Loc.Get("Language"));
            Console.WriteLine(Loc.Get("BackupLimit"));
            Console.WriteLine();
            Console.WriteLine(Loc.Get("OptBack"));
            Console.Write(Loc.Get("Select"));

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
            Console.WriteLine(Loc.Get("English"));
            Console.WriteLine(Loc.Get("Polish"));
            Console.WriteLine();
            Console.WriteLine(Loc.Get("OptBack"));
            Console.Write(Loc.Get("Select"));

            string input = Console.ReadLine() ?? "";
            if (input.ToUpper() == "Q") return;

            else if (input == "1") Loc.Set(Lang.EN);
            else if (input == "2") Loc.Set(Lang.PL);
            else continue;

            Console.Clear();
            Console.WriteLine(Loc.Get("LanguageSet"));
            Console.ReadLine();
        }
    }

    public static void LimitSettings()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(Loc.Get("InfoBackupVsSnapshot"));
            Console.WriteLine();
            Console.WriteLine(Loc.Format("ShowCurrentLimits", Cleaner.CurrentLimit.MaxBackupCount, Cleaner.CurrentLimit.MaxSnapshotCount));
            Console.WriteLine();
            Console.WriteLine(Loc.Get("SetBackupLimit"));
            Console.WriteLine(Loc.Get("SetSnapshotLimit"));
            Console.WriteLine();
            Console.WriteLine(Loc.Get("OptBack"));
            Console.Write(Loc.Get("Select"));
        
            string input = Console.ReadLine() ?? "";
            if (input.ToUpper() == "Q") return;
            if (!IsWithinScope(input, (1, 2), out int num)) continue;

            Cleaner.Mode mode = num == 1 ? Cleaner.Mode.Backup : Cleaner.Mode.Snapshot;

            Console.Clear();
            Console.Write(Loc.Get("EnterLimitValue"));
            string limitInput = Console.ReadLine() ?? "";
            if (!int.TryParse(limitInput, out int limit))
            {
                Console.Clear();
                Console.WriteLine(Loc.Get("NotNumber"));
                Console.ReadLine();
                continue;
            }

            Result r = Cleaner.SetLimit(limit, mode);

            Console.Clear();
            if (!r.IsSuccess)
            {
                Console.WriteLine(Loc.Get(r.ErrorKey ?? "Failure"));
                Console.ReadLine();
                continue;
            }
            else
            {
                Console.WriteLine(Loc.Get("NewLimitSet"));
                Console.ReadLine();
            }
                
        }
        
    }


}

