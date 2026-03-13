using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NutritionTracker.Data;
using System.Globalization;

namespace NutritionTracker
{
    public partial class App : Application
    {
        public DataStore DataStore { get; } = new DataStore();
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            NutritionTracker.Resources.Lang.Resources.Culture = new CultureInfo("en-GB");
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}