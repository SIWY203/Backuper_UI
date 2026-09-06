using Backuper_UI.ViewModels;
using System;
using System.Collections.Generic;
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

namespace Backuper_UI.Views
{
    /// <summary>
    /// Interaction logic for ClusterListView.xaml
    /// </summary>
    public partial class ClusterListView : UserControl
    {
        public ClusterListView()
        {
            InitializeComponent();
        }

        private void OnItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Window.GetWindow(this)?.DataContext is MainViewModel mainVm)
            {
                mainVm.OpenDetailsCommand.Execute(null);
            }
        }
    }
}
