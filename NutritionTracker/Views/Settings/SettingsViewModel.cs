using Avalonia;
using Avalonia.Styling;
using NutritionTracker.Data;
using System.Collections.ObjectModel;

namespace NutritionTracker.Views.Settings;

public class SettingsViewModel : BaseViewModel
{
    private readonly DataStore _dataStore;
    public ObservableCollection<ThemeVariant> Themes { get; } =
        [
            ThemeVariant.Default,
            ThemeVariant.Light,
            ThemeVariant.Dark,
        ];

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
    public int DSFontSize
    {
        get => _dataStore.FontSize;
        set 
        {
            if (_dataStore.FontSize != value)
            {
                _dataStore.FontSize = value;
            }
        }

    }
    public SettingsViewModel(DataStore dataStore) 
    {
        _dataStore = dataStore;
    }
}

