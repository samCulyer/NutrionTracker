using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using NutritionTracker.Views.Settings;

namespace NutritionTracker;

public partial class MainWindow : Window
{
    private SettingsViewModel SettingsViewModel { get; } = new();
    public MainWindow()
    {
        InitializeComponent();
        if (OperatingSystem.IsLinux())
        {
            this.ExtendClientAreaToDecorationsHint = false;
            TitleBarGrid.IsVisible = false;
            TitleBarGrid.Height = 0;
        }
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
                "InsightsTab" => new InsightsView(),
                "RecipesTab" => new RecipesView(),
                "SettingsTab" => new SettingsView(SettingsViewModel),
                "InfoTab" => new InfoView(),
                "AboutTab" => new AboutView(),
                _ => new UserView()
            };

        }
    }
    private void MainWindow_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Window.WindowStateProperty)
        {
            var isMaximized = WindowState == WindowState.Maximized;

            WindowBorder.CornerRadius = isMaximized ? new CornerRadius(0) : new CornerRadius(5);
            WindowBorder.BorderThickness = isMaximized ? new Thickness(8) : new Thickness(0);
        }
    }

    private void TitleBar_PointerPressed(object sender,PointerPressedEventArgs e) 
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            BeginMoveDrag(e);
        }
    }
    private void Minimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
    
    private void Maximize_Click(object sender, RoutedEventArgs e) => 
        this.WindowState = this.WindowState == WindowState.Maximized ?
            WindowState.Normal :WindowState.Maximized;
    private void Close_Click(object sender, RoutedEventArgs e) => Close();
}