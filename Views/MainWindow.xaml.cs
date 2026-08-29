using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static ClusterManager;

namespace Backuper_UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Loc.LoadLangConfig();
            //Cleaner.LoadConfig();
            //LoadClusters();
            //if (Clusters.Count == 0)
            //{
            //    ClusterUI.RunCreator();
            //    SaveClusters();
            //}

            //while (true)
            //{
            //    MenuUI.Menu();
            //}
        }
    }
}