using System.Diagnostics;
using System.Windows;

namespace Dreamland.WPF.DynamicSkin.Demo;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 订阅动态换肤错误事件
        DynamicSkinService.ErrorOutput += OnDynamicSkinError;

        // 可选：设置默认主题
        // DynamicSkinService.CurrentSkinName = "Dark";
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // 取消订阅事件
        DynamicSkinService.ErrorOutput -= OnDynamicSkinError;

        base.OnExit(e);
    }

    private void OnDynamicSkinError(object? sender, ErrorOutputEventArgs e)
    {
        Debug.WriteLine(e.ToString());
    }
}