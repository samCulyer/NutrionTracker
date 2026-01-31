using Avalonia.Controls;
using NutritionTracker.Views.Settings;

namespace NutritionTracker;

public partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}