using Backuper_UI.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Backuper_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load language config file
            Loc.Instance.LoadLangConfig();
        }
    }


}
