using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;

namespace Dreamland.WPF.DynamicSkin.Demo;

/// <summary>
///     主题管理器 - 提供高级主题管理功能
/// </summary>
public class ThemeManager
{
    private static ThemeManager? _instance;
    private static readonly object _lock = new();

    private ThemeManager()
    {
        // 初始化
    }

    /// <summary>
    ///     获取主题管理器单例
    /// </summary>
    public static ThemeManager Instance
    {
        get
        {
            if (_instance == null)
                lock (_lock)
                {
                    _instance ??= new ThemeManager();
                }

            return _instance;
        }
    }

    /// <summary>
    ///     当前主题名称
    /// </summary>
    public string? CurrentTheme
    {
        get => DynamicSkinService.CurrentSkinName;
        set => DynamicSkinService.CurrentSkinName = value;
    }

    /// <summary>
    ///     获取所有可用主题
    /// </summary>
    public string[] AvailableThemes => new[] { "Default", "Dark", "Light", "Blue" };

    /// <summary>
    ///     主题变更事件
    /// </summary>
    public event EventHandler<string?>? ThemeChanged;

    /// <summary>
    ///     应用主题
    /// </summary>
    public void ApplyTheme(string? themeName)
    {
        if (CurrentTheme != themeName)
        {
            CurrentTheme = themeName;
            ThemeChanged?.Invoke(this, themeName);
        }
    }

    /// <summary>
    ///     循环切换下一个主题
    /// </summary>
    public void NextTheme()
    {
        var themes = new[] { null, "Dark", "Light", "Blue" };
        var currentIndex = Array.IndexOf(themes, CurrentTheme);
        var nextIndex = (currentIndex + 1) % themes.Length;
        ApplyTheme(themes[nextIndex]);
    }

    /// <summary>
    ///     获取当前主题的主色调
    /// </summary>
    public Color GetPrimaryColor()
    {
        var resourceKey = CurrentTheme == null ? "PrimaryColor" : $"PrimaryColor.{CurrentTheme}";

        if (Application.Current?.TryFindResource(resourceKey) is Color color) return color;

        return Colors.Blue; // 默认颜色
    }

    /// <summary>
    ///     检测系统是否使用深色模式（Windows 10/11）
    /// </summary>
    public bool IsSystemDarkMode()
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int intValue && intValue == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    ///     根据系统主题自动切换
    /// </summary>
    public void ApplySystemTheme()
    {
        var isDarkMode = IsSystemDarkMode();
        ApplyTheme(isDarkMode ? "Dark" : "Light");
    }

    /// <summary>
    ///     清除所有缓存的资源
    /// </summary>
    public void ClearCache()
    {
        DynamicSkinService.ClearResourceCache();
    }

    /// <summary>
    ///     预加载主题（提高首次切换速度）
    /// </summary>
    public void PreloadThemes()
    {
        var currentTheme = CurrentTheme;

        foreach (var theme in AvailableThemes.Where(t => t != "Default"))
            // 临时切换到该主题以触发资源加载
            CurrentTheme = theme;

        // 恢复原主题
        CurrentTheme = currentTheme;
    }
}