using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using NutritionTracker.Data;
using System.Collections.ObjectModel;

namespace NutritionTracker.Views.Settings;

public class SettingsViewModel : BaseViewModel
{
    public ObservableCollection<ThemeVariant> Themes { get; } =
        new()
        {
            ThemeVariant.Light,
            ThemeVariant.Dark,
        };

    public ThemeVariant? CurrentTheme
    {
        get => Application.Current!.RequestedThemeVariant;
        set
        {
            if (Application.Current!.RequestedThemeVariant != value)
            {
                Application.Current.RequestedThemeVariant = value;
                OnPropertyChanged();
            }
        }
    }
}

//public class ThemeOption
//{
//    public ThemeVariant Variant { get; }
//    public string Name { get; }

//    public ThemeOption(ThemeVariant variant, string name)
//    {
//        Variant = variant;
//        Name = name;
//    }

//    // Helper to get the brush for the swatch at runtime
//    public IBrush BackgroundBrush
//    {
//        get
//        {
//            var app = Application.Current!;
//            var dict = app.Styles[0] as IResourceProvider; // assumes first style contains ThemeDictionaries
//            if (Variant == ThemeVariant.Light)
//                return app.TryFindResource(app, ThemeVariant.Light,BackgroundBrush)
//                       ?? Brushes.White;
//            else if (Variant == ThemeVariant.Dark)
//                return app.TryFindResource("BackgroundBrush") as IBrush
//                       ?? Brushes.Black;
//            else
//                return Brushes.Gray;
//        }
//    }
//}