using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using static ConfigManager;
namespace Backuper_UI.Services;

public enum Lang { PL, EN }

public class Loc : INotifyPropertyChanged
{
    public Lang CurrentLang
    {
        get => field;
        set
        {
            if (field == value) return;
            field = value;
            File.WriteAllText(LanguageConfigFile, value.ToString()); // auto-save
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Binding.IndexerName)); // Refresh XAML binds on the indexer
        }
    } = Lang.EN;

    public event PropertyChangedEventHandler? PropertyChanged;


    // Loc.Instance["msgKey"]
    public static Loc Instance { get; } = new();
    public string this[string key] =>
        Dictionary.TryGetValue(key, out var translations) && translations.TryGetValue(CurrentLang, out var text)
            ? text
            : key;


    // Loc.Instance.Format("msgKey", arg0, arg1, ...)
    public string Format(string key, params object[] args)
    {
        return string.Format(Instance[key], args);
    }


    // Lang Configs
    public void LoadLangConfig()
    {
        if (File.Exists(LanguageConfigFile))
        {
            string lang = File.ReadAllText(LanguageConfigFile).Trim();
            if (Enum.TryParse(lang, true, out Lang parsedLang))
            {
                CurrentLang = parsedLang;
                return;
            }
        }
        CurrentLang = Lang.EN; // default
    }


    private static readonly Dictionary<string, Dictionary<Lang, string>> Dictionary = new()
    {
        // ================================
        //  main menu
        // ================================
        ["HeaderMenu"] = new() {
            [Lang.PL] = "============ Backuper ============",
            [Lang.EN] = "============ Backuper ============"
        },
        ["ClusterList"] = new() {
            [Lang.PL] = "Lista klastrów: ",
            [Lang.EN] = "Cluster list:"
        },
        ["OptAddCluster"] = new() {
            [Lang.PL] = "[A] Dodaj klaster",
            [Lang.EN] = "[A] Add cluster"
        },
        ["OptRemoveCluster"] = new() {
            [Lang.PL] = "[R] Usuń klaster",
            [Lang.EN] = "[R] Remove cluster"
        },
        ["OptSettings"] = new() {
            [Lang.PL] = "[S] Ustawienia",
            [Lang.EN] = "[S] Settings"
        },
        ["Quit"] = new() {
            [Lang.PL] = "[Q] Wyjdź",
            [Lang.EN] = "[Q] Quit"
        },

        // ================================
        //  settings
        // ================================
        ["HeaderSettings"] = new() {
            [Lang.PL] = "=========== Ustawienia ===========",
            [Lang.EN] = "============ Settings ============"
        },
        ["Language"] = new() {
            [Lang.PL] = "[1] Język",
            [Lang.EN] = "[1] Language"
        },
        ["LanguageSet"] = new() {
            [Lang.PL] = "Ustawiono język polski",
            [Lang.EN] = "English language set"
        },
        ["English"] = new() {
            [Lang.PL] = "[1] English",
            [Lang.EN] = "[1] English"
        },
        ["Polish"] = new() {
            [Lang.PL] = "[2] Polski",
            [Lang.EN] = "[2] Polski"
        },
        ["BackupLimit"] = new()
        {
            [Lang.PL] = "[2] Limit kopii",
            [Lang.EN] = "[2] Backup limit"
        },
        ["InfoBackupVsSnapshot"] = new()
        {
            [Lang.PL] = "Limit backupów dotyczy kopii zapasowych danego folderu, \n" +
                        "limit snapshotów dotyczy kopii bieżącej zawartości źródłowej, \n" +
                        "utworzonej po przywróceniu kopii zapasowej.",
            [Lang.EN] = "The backup limit applies to backups of a given folder, \n" +
                        "the snapshot limit applies to a copy of the current source content, \n" +
                        "created after restoring a backup."
        },
        ["ShowCurrentLimits"] = new()
        {
            [Lang.PL] = "Aktualny limit backupów: {0} \nAktualny limit snapshotów: {1}",
            [Lang.EN] = "Current backup limit: {0} \nCurrent snapshot limit: {1}"
        },
        ["SetBackupLimit"] = new()
        {
            [Lang.PL] = "[1] Ustaw limit backupów",
            [Lang.EN] = "[1] Set backup count limit"
        },
        ["SetSnapshotLimit"] = new()
        {
            [Lang.PL] = "[2] Ustaw limit snapshotów",
            [Lang.EN] = "[2] Set snapshot count limit"
        },
        ["EnterLimitValue"] = new()
        {
            [Lang.PL] = "Podaj nowy limit: ",
            [Lang.EN] = "Ender a new limit: "
        },
        ["NewLimitSet"] = new()
        {
            [Lang.PL] = "Nowy limit został ustawiony.",
            [Lang.EN] = "A new limit has been set."
        },
        ["ErrNumberOutOfRange"] = new()
        {
            [Lang.PL] = "Liczba przekracza dozwolony zakres.",
            [Lang.EN] = "The number exceeds the allowed range."
        },


        // ================================
        //  backups
        // ================================
        ["BackupCreating"] = new() {
            [Lang.PL] = "Tworzenie kopii...",
            [Lang.EN] = "Creating backup..."
        },
        ["BackupCreated"] = new() {
            [Lang.PL] = "Utworzono backup!",
            [Lang.EN] = "Backup created!"
        },
        ["BackupRestored"] = new() {
            [Lang.PL] = "Przywrócono backup!",
            [Lang.EN] = "Backup restored!"
        },
        ["InfoSnapshotCreation"] = new() {
            [Lang.PL] = "Zostanie utworzona migawka zawierająca bieżącą zawartość źródłową.",
            [Lang.EN] = "There will be created a snapshot with the current source content."
        },
        ["NoBackupToRestore"] = new() {
            [Lang.PL] = "Brak backupów do przywrocenia!",
            [Lang.EN] = "There is no backups to restore!"
        },
        ["AskForRestoreBackup"] = new() {
            [Lang.PL] = "Czy na pewno chcesz przywrócić backup?",
            [Lang.EN] = "Are you sure you want to restore the backup?"
        },
        ["ConfirmRestoreBackup"] = new()
        {
            [Lang.PL] = "[Y] Tak, przywróć\n[N] Nie, anuluj",
            [Lang.EN] = "[Y] Yes, restore\n[N] No, cancel"
        },
        ["NoBackupToDisplay"] = new()
        {
            [Lang.PL] = "Brak backupów do wyświetlenia!",
            [Lang.EN] = "There is no backups to display!"
        },
        ["BackupsOfCluster"] = new()
        {
            [Lang.PL] = "Backupy klastra {0}: ",
            [Lang.EN] = "Backups of cluster {0}: "
        },
        ["ErrPathNotExist"] = new()
        {
            [Lang.PL] = "Ścieżka źródłowa, lub do backupów nie istnieje!",
            [Lang.EN] = "Source path or backup path does not exist!"
        },
        ["ErrCloneFailed"] = new()
        {
            [Lang.PL] = "Kopiowanie nie powiodło się!",
            [Lang.EN] = "Copying failed!"
        },
        ["ErrReplaceFailed"] = new()
        {
            [Lang.PL] = "Kopiowanie nie powiodło się!",
            [Lang.EN] = "Copying failed!"
        },


        // ================================
        //  clusters
        // ================================
        ["HeaderCreator"] = new()
        {
            [Lang.PL] = "=== KREATOR KLASTRÓW ===",
            [Lang.EN] = "=== CLUSTER CREATOR ==="
        },
        ["InfoCanDropDir"] = new()
        {
            [Lang.PL] = "Możesz wpisać, lub upuścić folder do okna konsoli :)",
            [Lang.EN] = "You can type or drop the folder into the console window :)"
        },
        ["EnterClusterName"] = new()  { 
            [Lang.PL] = "Podaj nazwę klastra: ", 
            [Lang.EN] = "Enter cluster name: " 
        },
        ["EnterClusterSource"] = new()  { 
            [Lang.PL] = "Podaj ścieżkę źródłową: ",
            [Lang.EN] = "Enter source path: " },
        ["EnterClusterTarget"] = new()  { 
            [Lang.PL] = "Podaj ścieżkę backupu: ",
            [Lang.EN] = "Enter backup path: " 
        },
        ["ErrEmptyFields"] = new() { 
            [Lang.PL] = "Błąd: Wszystkie pola muszą być wypełnione!",
            [Lang.EN] = "Error: All fields must be filled!" 
        },
        ["ErrEmptyField"] = new() { 
            [Lang.PL] = "Błąd: Nic nie wpisano!",
            [Lang.EN] = "Error: Nothing was entered!"
        },
        ["ErrSubfolder"] = new() {
            [Lang.PL] = "Błąd: Ścieżka docelowa nie może być podfolderem źródła!",
            [Lang.EN] = "Error: Destination cannot be a subfolder of source!" 
        },
        ["ClusterAdded"] = new() {
            [Lang.PL] = "Klaster {0} został dodany!",
            [Lang.EN] = "Cluster {0} added!" 
        },
        ["ClusterExists"] = new() { 
            [Lang.PL] = "Klaster {0} już istnieje!",
            [Lang.EN] = "Cluster {0} already exists!" 
        },
        ["SelectToRemove"] = new() {
            [Lang.PL] = "Wybierz do usunięcia:",
            [Lang.EN] = "Select to remove:"
        },
        ["AskToRemoveCluster"] = new()
        {
            [Lang.PL] = "Czy na pewno chcesz usunąć cluster?",
            [Lang.EN] = "Are you sure you want to remove cluster?"
        },
        ["ConfirmRemoveCluster"] = new()
        {
            [Lang.PL] = "[Y] Tak, usuń\n[N] Nie, anuluj",
            [Lang.EN] = "[Y] Yes, remove\n[N] No, cancel"
        },
        ["ClusterDetails"] = new()
        {
            [Lang.PL] = "Klaster {0} \nŚcieżka źródłowa: {1} \nŚcieżka docelowa: {2} \n",
            [Lang.EN] = "Cluster {0} \nSource path: {1} \nTarget path: {2} \n"
        },
        ["OptEditCluster"] = new()
        {
            [Lang.PL] = "[E] Edytuj klaster\n",
            [Lang.EN] = "[E] Edit cluster\n"
        },
        ["OptCreateBackup"] = new() {
            [Lang.PL] = "[1] Stwórz backup", 
            [Lang.EN] = "[1] Create Backup" 
        },
        ["OptRestoreBackup"] = new() {
            [Lang.PL] = "[2] Przywróć backup",
            [Lang.EN] = "[2] Restore Backup" 
        },
        ["OptUndoRestore"] = new() {
            [Lang.PL] = "[3] Cofnij przywracanie",
            [Lang.EN] = "[3] Undo restore" 
        },
        ["OptShowBackups"] = new() { 
            [Lang.PL] = "[4] Pokaż wszystkie backupy",
            [Lang.EN] = "[4] Show All Backups" 
        },
        ["ErrClusterAlreadyExist"] = new() { 
            [Lang.PL] = "Błąd! Ten klaster już istnieje!",
            [Lang.EN] = "Error! Cluster already exist!"
        },
        ["ErrSameDirectory"] = new() { 
            [Lang.PL] = "Błąd! Nie można przypisać tego samego folderu do obu ścieżek!",
            [Lang.EN] = "Error! Cannot assign the same folder to both paths!"
        },

        // ================================
        //  cluster editor
        // ================================
        ["HeaderClusterEditor"] = new() { 
            [Lang.PL] = "============ EDYTOR KLASTRÓW ============",
            [Lang.EN] = "============ CLUSTER EDITOR ============="
        },
        ["OptUpdateClusterName"] = new() { 
            [Lang.PL] = "[1] Zmień nazwę",
            [Lang.EN] = "[1] Change name"
        },
        ["OptUpdateClusterSource"] = new() { 
            [Lang.PL] = "[2] Zmień ścieżkę źródłową",
            [Lang.EN] = "[2] Change source path"
        },
        ["OptUpdateClusterTarget"] = new() { 
            [Lang.PL] = "[3] Zmień ścieżkę docelową",
            [Lang.EN] = "[3] Change backup path"
        },
        ["UpdateClusterName"] = new() { 
            [Lang.PL] = "Podaj nową nazwę: ",
            [Lang.EN] = "Enter new name: "
        },
        ["UpdateClusterSource"] = new() { 
            [Lang.PL] = "Podaj nowe źródło: ",
            [Lang.EN] = "Enter new source: "
        },
        ["UpdateClusterTarget"] = new() { 
            [Lang.PL] = "Podaj nową ścieżkę backupów: ",
            [Lang.EN] = "Enter new backup path: "
        },
        ["UpdateNameSuccess"] = new() { 
            [Lang.PL] = "Nazwa została zmieniona!",
            [Lang.EN] = "The name has been changed!"
        },
        ["UpdatePathSuccess"] = new() { 
            [Lang.PL] = "Ścieżka została zmieniona!",
            [Lang.EN] = "The path has been changed!"
        },
        ["NameIsTaken"] = new() { 
            [Lang.PL] = "Nazwa jest już zajęta!",
            [Lang.EN] = "Name is already taken!"
        },

        // ================================
        //  snapshot
        // ================================
        ["ErrSnapshotCreatingFailed"] = new()
        {
            [Lang.PL] = "Nie udało się zrobić snapshota! Anulowano.",
            [Lang.EN] = "Snapshot failed! Operation canceled."
        },
        ["ErrSnapshotPathNotExist"] = new()
        {
            [Lang.PL] = "Ścieżka do snapshota nie istnieje!",
            [Lang.EN] = "Snapshot failed! Operation canceled."
        },
        ["SnapshotRestored"] = new()
        {
            [Lang.PL] = "Cofnięto przywracanie!",
            [Lang.EN] = "The restore was undone!"
        },
        ["NoSnapshotToRestore"] = new()
        {
            [Lang.PL] = "Brak snapshotów do przywrocenia!",
            [Lang.EN] = "There is no spanshots to restore!"
        },
        ["AskForUndoRestore"] = new()
        {
            [Lang.PL] = "Czy na pewno chcesz cofnąć przywracanie?",
            [Lang.EN] = "Are you sure you want to undo restore?"
        },
        ["ConfirmUndoRestore"] = new()
        {
            [Lang.PL] = "[Y] Tak, cofnij\n[N] Nie, anuluj",
            [Lang.EN] = "[Y] Yes, undo\n[N] No, cancel"
        },

        // ================================
        //  standard
        // ================================
        ["OptBack"] = new()
        {
            [Lang.PL] = "[Q] Powrót",
            [Lang.EN] = "[Q] Back"
        },
        ["Select"] = new() { 
            [Lang.PL] = "\nWybierz: ",
            [Lang.EN] = "\nSelect: " 
        },
        ["Cancelled"] = new() {
            [Lang.PL] = "Anulowano...",
            [Lang.EN] = "Cancelled..."
        },
        ["Failure"] = new() {
            [Lang.PL] = "Niepowodzenie!",
            [Lang.EN] = "Failure!"
        },        
        ["NotNumber"] = new() {
            [Lang.PL] = "To nie jest liczba!",
            [Lang.EN] = "This is not a number!"
        },        
        ["Ok"] = new() {
            [Lang.PL] = "[Enter] Ok, dzięki ",
            [Lang.EN] = "[Enter] Ok, thanks "
        },        
        
    
    };
    

}

