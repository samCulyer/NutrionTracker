using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NutritionTracker.Views.About;

public partial class AboutView : UserControl
{
    public AboutView()
    {
        InitializeComponent();
    }

    public AboutView(AboutViewModel vm) : this ()
    {
        DataContext = vm;
    }
}