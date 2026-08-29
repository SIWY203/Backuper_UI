class ConfigManager
{
    private static readonly string AppFolder =
#if DEBUG
        AppContext.BaseDirectory;
#else
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Backuper by Siwy203");
#endif

    public static string ClusterConfigFile { get; } = Path.Combine(AppFolder, "clusters.json");
    public static string LanguageConfigFile { get; } = Path.Combine(AppFolder, "lang_config.txt");
    public static string LimitsConfigFile { get; } = Path.Combine(AppFolder, "limits_config.json");

    static ConfigManager()
    {
        // konstruktor statyczny,
        // tworzy foldery na dowolnym OS, jeśli nie istnieją
        Directory.CreateDirectory(AppFolder);
    }
}
    

