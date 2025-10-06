using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Dreamland.WPF.DynamicSkin.Demo;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private string _currentTheme = "Default";

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        // Monitor theme changes
        UpdateCurrentTheme();
    }

    /// <summary>
    ///     Current theme name
    /// </summary>
    public string CurrentTheme
    {
        get => _currentTheme;
        set
        {
            if (_currentTheme != value)
            {
                _currentTheme = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    ///     Update current theme display
    /// </summary>
    private void UpdateCurrentTheme()
    {
        CurrentTheme = DynamicSkinService.CurrentSkinName ?? "Default";
    }

    /// <summary>
    ///     Switch to default theme
    /// </summary>
    private void OnDefaultThemeClick(object sender, RoutedEventArgs e)
    {
        DynamicSkinService.CurrentSkinName = null;
        UpdateCurrentTheme();
        ShowNotification("Switched to Default theme");
    }

    /// <summary>
    ///     Switch to Dark theme
    /// </summary>
    private void OnDarkThemeClick(object sender, RoutedEventArgs e)
    {
        DynamicSkinService.CurrentSkinName = "Dark";
        UpdateCurrentTheme();
        ShowNotification("Switched to Dark theme");
    }

    /// <summary>
    ///     Switch to Light theme
    /// </summary>
    private void OnLightThemeClick(object sender, RoutedEventArgs e)
    {
        DynamicSkinService.CurrentSkinName = "Light";
        UpdateCurrentTheme();
        ShowNotification("Switched to Light theme");
    }

    /// <summary>
    ///     Switch to Blue theme
    /// </summary>
    private void OnBlueThemeClick(object sender, RoutedEventArgs e)
    {
        DynamicSkinService.CurrentSkinName = "Blue";
        UpdateCurrentTheme();
        ShowNotification("Switched to Blue theme");
    }

    /// <summary>
    ///     Open advanced features window
    /// </summary>
    private void OnAdvancedClick(object sender, RoutedEventArgs e)
    {
        var advancedWindow = new AdvancedWindow
        {
            Owner = this
        };
        advancedWindow.ShowDialog();

        // Update theme display after closing advanced window
        UpdateCurrentTheme();
    }

    /// <summary>
    ///     Show notification message
    /// </summary>
    private void ShowNotification(string message)
    {
        Debug.WriteLine($"[Theme Switch] {message}");
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}