using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
namespace Backuper_UI.ViewModels;

public class ConfirmViewModel
{
    public ICommand ConfirmCommand { get; }
    public ConfirmViewModel()
    {
        ConfirmCommand = new RelayCommand(ConfirmAction);
    }

    public void ConfirmAction()
    {
        
    }
}