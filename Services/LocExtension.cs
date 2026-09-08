using System.Windows.Data;
using System.Windows.Markup;
namespace Backuper_UI.Services;

// OLD: Text="{Binding Path=[MessageKey], Source={x:Static services:Loc.Instance}}"
// NEW: Text="{services:Loc MessageKey}"

public class LocExtension(string key) : MarkupExtension
{
    public string Key { get; set; } = key;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding($"[{Key}]")
        {
            Source = Loc.Instance
        };
        return binding.ProvideValue(serviceProvider);
    }
}