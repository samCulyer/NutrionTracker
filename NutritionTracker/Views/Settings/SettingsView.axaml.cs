using Avalonia.Controls;

namespace NutritionTracker.Views.Settings;
public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    public SettingsView(SettingsViewModel vm) : this()
    {
        DataContext = vm;
    }
}