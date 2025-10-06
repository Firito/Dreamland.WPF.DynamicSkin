using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace Dreamland.WPF.DynamicSkin.Demo;

/// <summary>
///     AdvancedWindow.xaml 的交互逻辑
/// </summary>
public partial class AdvancedWindow : Window
{
    private readonly ThemeManager _themeManager;

    public AdvancedWindow()
    {
        InitializeComponent();
        _themeManager = ThemeManager.Instance;

        // 初始化主题列表
        ThemesListBox.ItemsSource = _themeManager.AvailableThemes;

        UpdateThemeInfo();
        UpdateCacheStats();
    }

    /// <summary>
    ///     更新主题信息显示
    /// </summary>
    private void UpdateThemeInfo()
    {
        CurrentThemeText.Text = DynamicSkinService.CurrentSkinName ?? "默认";

        // 获取主色调
        var primaryColor = _themeManager.GetPrimaryColor();
        PrimaryColorText.Text = primaryColor.ToString();
    }

    /// <summary>
    ///     更新缓存统计
    /// </summary>
    private void UpdateCacheStats()
    {
        CacheCountText.Text = "N/A";
    }

    /// <summary>
    ///     返回主窗口
    /// </summary>
    private void OnBackClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    /// <summary>
    ///     循环切换主题 (Switch to next theme)
    /// </summary>
    private void OnNextThemeClick(object sender, RoutedEventArgs e)
    {
        _themeManager.NextTheme();
        UpdateThemeInfo();
        MessageBox.Show($"已切换到 (Switched to): {CurrentThemeText.Text}", "主题切换 (Theme Switch)",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    ///     跟随系统主题 (Follow system theme)
    /// </summary>
    private void OnSystemThemeClick(object sender, RoutedEventArgs e)
    {
        _themeManager.ApplySystemTheme();
        UpdateThemeInfo();
        var isDark = _themeManager.IsSystemDarkMode();
        MessageBox.Show($"检测到系统使用 {(isDark ? "深色" : "浅色")} 模式 (Detected system {(isDark ? "dark" : "light")} mode)\n已应用对应主题 (Applied corresponding theme)",
            "系统主题 (System Theme)", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    ///     预加载主题 (Preload themes)
    /// </summary>
    private void OnPreloadThemesClick(object sender, RoutedEventArgs e)
    {
        var stopwatch = Stopwatch.StartNew();
        _themeManager.PreloadThemes();
        stopwatch.Stop();

        UpdateThemeInfo();
        MessageBox.Show($"主题预加载完成 (Theme preload completed)\n耗时 (Time): {stopwatch.ElapsedMilliseconds}ms",
            "预加载完成 (Preload Completed)", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    ///     清除缓存 (Clear cache)
    /// </summary>
    private void OnClearCacheClick(object sender, RoutedEventArgs e)
    {
        _themeManager.ClearCache();
        UpdateCacheStats();
        MessageBox.Show("资源缓存已清除 (Resource cache cleared)", "清除缓存 (Clear Cache)",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    ///     添加自定义资源 (Add custom resource)
    /// </summary>
    private void OnAddResourceClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var key = ResourceKeyTextBox.Text;
            var colorString = ResourceValueTextBox.Text;

            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("请输入资源键 (Please enter resource key)", "错误 (Error)",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 解析颜色 (Parse color)
            var color = (Color)ColorConverter.ConvertFromString(colorString);
            var brush = new SolidColorBrush(color);

            // 添加到资源字典 (Add to resource dictionary)
            DynamicSkinService.AddOrUpdateResource(key, brush);

            // 更新预览 (Update preview)
            ResourcePreviewBorder.Background = brush;

            MessageBox.Show($"资源 (Resource) '{key}' 添加成功 (added successfully)!", "成功 (Success)",
                MessageBoxButton.OK, MessageBoxImage.Information);

            UpdateCacheStats();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"添加资源失败 (Failed to add resource): {ex.Message}", "错误 (Error)",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    ///     刷新统计
    /// </summary>
    private void OnRefreshStatsClick(object sender, RoutedEventArgs e)
    {
        UpdateCacheStats();
    }

    /// <summary>
    ///     性能测试 (Performance test)
    /// </summary>
    private void OnPerformanceTestClick(object sender, RoutedEventArgs e)
    {
        var stopwatch = Stopwatch.StartNew();
        var themes = new[] { "Dark", "Light", "Blue", null };

        for (var i = 0; i < 1000; i++) DynamicSkinService.CurrentSkinName = themes[i % themes.Length];

        stopwatch.Stop();
        UpdateThemeInfo();

        var result = $"✅ 测试完成 (Test Completed)\n" +
                     $"切换次数 (Switch Count): 1000 次 (times)\n" +
                     $"总耗时 (Total Time): {stopwatch.ElapsedMilliseconds}ms\n" +
                     $"平均耗时 (Average Time): {stopwatch.ElapsedMilliseconds / 1000.0:F3}ms (per switch)";

        PerformanceResultText.Text = result;

        MessageBox.Show(result, "性能测试结果 (Performance Test Results)",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>
    ///     内存测试 (Memory test)
    /// </summary>
    private void OnMemoryTestClick(object sender, RoutedEventArgs e)
    {
        var beforeMemory = GC.GetTotalMemory(true);

        // 创建和销毁窗口 (Create and destroy windows)
        for (var i = 0; i < 100; i++)
        {
            var window = new Window
            {
                Width = 400,
                Height = 300
            };

            // 强制显示和关闭 (Force show and close)
            window.Show();
            window.Close();

            if (i % 20 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        // 强制GC (Force GC)
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        var afterMemory = GC.GetTotalMemory(false);
        var memoryIncrease = (afterMemory - beforeMemory) / 1024.0 / 1024.0;

        var result = $"✅ 测试完成 (Test Completed)\n" +
                     $"创建窗口数 (Windows Created): 100 个 (windows)\n" +
                     $"测试前内存 (Memory Before): {beforeMemory / 1024.0 / 1024.0:F2}MB\n" +
                     $"测试后内存 (Memory After): {afterMemory / 1024.0 / 1024.0:F2}MB\n" +
                     $"内存增加 (Memory Increase): {memoryIncrease:F2}MB\n" +
                     $"平均每窗口 (Average per Window): {memoryIncrease / 100.0:F3}MB";

        MemoryResultText.Text = result;

        MessageBox.Show(result, "内存测试结果 (Memory Test Results)",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}