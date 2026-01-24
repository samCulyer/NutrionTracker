using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace NutritionTracker;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        ContentView.Content = new UserView();
    }

    private void OnTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is TabStrip tab 
            && tab.SelectedItem is TabStripItem item 
            && ContentView is not null)
        {
            ContentView.Content = item.Tag switch
            {
                "UserTab" => new UserView(),
                "RecipesTab" => new RecipesView(),
                _ => new UserView()
            };

        }
    }
}