# Dreamland.WPF.DynamicSkin

[![NuGet](https://img.shields.io/nuget/v/Dreamland.WPF.DynamicSkin.svg)](https://www.nuget.org/packages/Dreamland.WPF.DynamicSkin)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

一个适用于 WPF 项目的**动态换肤控件库**，支持运行时无缝切换主题，无需重启应用程序。

**A dynamic skin-changing control library for WPF projects**, supporting seamless runtime theme switching without restarting the application.

## ✨ 特性 Features

- 🎨 **运行时主题切换** - 无需重启应用即可动态更换主题
- 🚀 **高性能** - 基于资源缓存机制，主题切换流畅快速
- 🔌 **易于集成** - 仅需简单的 XAML 标记扩展即可使用
- 📦 **轻量级** - 零外部依赖，核心库体积小
- 🎯 **完全兼容** - 支持所有标准 WPF 控件和自定义控件
- 🔧 **灵活扩展** - 支持自定义主题和资源
- 💡 **智能回退** - 主题资源未找到时自动回退到默认资源
- 🛡️ **类型安全** - 完整的 C# 代码注释和 XML 文档

## 🎯 支持的框架 Target Frameworks

- **.NET Framework 4.5**
- **.NET 6.0 (Windows)**
- **.NET 8.0 (Windows)**

## 📦 安装 Installation

### NuGet 包管理器

```bash
Install-Package Dreamland.WPF.DynamicSkin
```

### .NET CLI

```bash
dotnet add package Dreamland.WPF.DynamicSkin
```

### PackageReference

```xml
<PackageReference Include="Dreamland.WPF.DynamicSkin" Version="1.0.0" />
```

## 🚀 快速开始 Quick Start

### 1. 定义主题资源

在 `App.xaml` 中定义您的主题资源：

```xaml
<Application x:Class="YourApp.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Application.Resources>
        
        <!-- 默认主题颜色 -->
        <Color x:Key="PrimaryColor">#FF0078D7</Color>
        <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />
        
        <!-- Dark 主题颜色 -->
        <Color x:Key="PrimaryColor.Dark">#FF1E90FF</Color>
        <SolidColorBrush x:Key="PrimaryBrush.Dark" Color="{StaticResource PrimaryColor.Dark}" />
        
        <!-- 默认按钮样式 -->
        <Style x:Key="PrimaryButtonStyle" TargetType="Button">
            <Setter Property="Background" Value="{StaticResource PrimaryBrush}" />
            <Setter Property="Foreground" Value="White" />
            <Setter Property="Padding" Value="20,8" />
            <!-- 更多 Setter ... -->
        </Style>
        
        <!-- Dark 主题按钮样式 -->
        <Style x:Key="PrimaryButtonStyle.Dark" BasedOn="{StaticResource PrimaryButtonStyle}" TargetType="Button">
            <Setter Property="Background" Value="{StaticResource PrimaryBrush.Dark}" />
        </Style>
        
    </Application.Resources>
</Application>
```

### 2. 在 XAML 中使用动态主题

```xaml
<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:skin="clr-namespace:Dreamland.WPF.DynamicSkin;assembly=Dreamland.WPF.DynamicSkin"
        Title="主窗口" Height="450" Width="800">
    
    <StackPanel Margin="20">
        <!-- 使用 DynamicStyle 绑定样式 -->
        <Button Content="主要按钮" 
                Style="{skin:DynamicStyle PrimaryButtonStyle}" />
        
        <!-- 使用 DynamicResource 绑定资源 -->
        <TextBlock Text="动态前景色文本" 
                   Foreground="{skin:DynamicResource PrimaryBrush}" />
        
        <!-- Window 样式绑定 -->
        <Border Background="{skin:DynamicResource BackgroundBrush}"
                BorderBrush="{skin:DynamicResource BorderBrush}"
                BorderThickness="1" />
    </StackPanel>
</Window>
```

### 3. 在代码中切换主题

```csharp
using Dreamland.WPF.DynamicSkin;

// 切换到 Dark 主题
DynamicSkinService.CurrentSkinName = "Dark";

// 切换到 Light 主题
DynamicSkinService.CurrentSkinName = "Light";

// 恢复默认主题（无后缀）
DynamicSkinService.CurrentSkinName = null;

// 获取当前主题名称
string? currentTheme = DynamicSkinService.CurrentSkinName;
```

## 📖 核心概念 Core Concepts

### 资源命名约定

Dreamland.WPF.DynamicSkin 使用**点号命名约定**来组织主题资源：

```
基础资源名.主题名
ResourceKey.ThemeName
```

**示例：**

| 基础资源 | 默认主题 | Dark 主题 | Light 主题 |
|---------|---------|----------|-----------|
| PrimaryBrush | `PrimaryBrush` | `PrimaryBrush.Dark` | `PrimaryBrush.Light` |
| ButtonStyle | `ButtonStyle` | `ButtonStyle.Dark` | `ButtonStyle.Light` |
| BackgroundColor | `BackgroundColor` | `BackgroundColor.Dark` | `BackgroundColor.Light` |

### 工作原理

1. **标记扩展** - `DynamicStyle` 和 `DynamicResource` 创建数据绑定
2. **监听变化** - 绑定到 `DynamicSkinService.CurrentSkinName` 属性
3. **资源查找** - 根据主题名称查找对应资源（例如：`ButtonStyle.Dark`）
4. **智能回退** - 如果主题资源不存在，自动回退到基础资源
5. **缓存优化** - 使用线程安全的资源缓存提升性能

## 🎨 标记扩展 Markup Extensions

### DynamicStyle

用于绑定 `Style` 类型的资源，通常用于控件的 `Style` 属性。

```xaml
<Button Style="{skin:DynamicStyle PrimaryButtonStyle}" />

<Window Style="{skin:DynamicStyle MainWindowStyle}" />

<TextBox Style="{skin:DynamicStyle TextBoxStyle}" />
```

### DynamicResource

用于绑定非 `Style` 类型的资源，如 `Brush`、`Color`、`Thickness` 等。

```xaml
<!-- 绑定 Brush -->
<TextBlock Foreground="{skin:DynamicResource ForegroundBrush}" />

<!-- 绑定 Color -->
<Border Background="{skin:DynamicResource BackgroundColor}" />

<!-- 绑定 Thickness -->
<Border Padding="{skin:DynamicResource DefaultPadding}" />
```

## 🔧 API 参考 API Reference

### DynamicSkinService

静态服务类，提供主题切换和资源管理功能。

#### 属性 Properties

```csharp
// 获取或设置当前主题名称
public static string? CurrentSkinName { get; set; }
```

#### 方法 Methods

```csharp
// 清除所有资源缓存
public static void ClearResourceCache()

// 添加或更新资源到缓存
public static bool AddOrUpdateResource(string key, object? resource)

// 尝试获取缓存的资源
public static bool TryGetResource(string key, out object? resource)
```

#### 事件 Events

```csharp
// 错误输出事件（当资源查找失败时触发）
public static event EventHandler<ErrorOutputEventArgs>? ErrorOutput;
```

### 使用示例

```csharp
using Dreamland.WPF.DynamicSkin;

public class ThemeManager
{
    public void Initialize()
    {
        // 监听错误输出
        DynamicSkinService.ErrorOutput += (sender, e) =>
        {
            Debug.WriteLine($"主题错误: {e.ErrorMessage}");
        };
        
        // 预加载资源到缓存
        var primaryBrush = new SolidColorBrush(Colors.Blue);
        DynamicSkinService.AddOrUpdateResource("CustomBrush", primaryBrush);
        
        // 检查资源是否存在
        if (DynamicSkinService.TryGetResource("CustomBrush", out var resource))
        {
            Debug.WriteLine("资源已缓存");
        }
    }
    
    public void SwitchTheme(string themeName)
    {
        // 切换前清除缓存
        DynamicSkinService.ClearResourceCache();
        
        // 切换主题
        DynamicSkinService.CurrentSkinName = themeName;
    }
}
```

## 🔍 故障排查 Troubleshooting

### 主题切换不生效

**问题：** 切换主题后界面没有变化

**解决方案：**
1. 检查是否正确使用了 `DynamicStyle` 或 `DynamicResource` 标记扩展
2. 确认主题资源已在 `App.xaml` 中定义
3. 验证资源命名是否遵循 `ResourceKey.ThemeName` 约定
4. 检查是否在设计时模式，设计器可能不会实时更新

### 资源未找到警告

**问题：** 输出窗口显示 "Can't find resource" 错误

**解决方案：**
1. 确保为所有主题定义了相应的资源
2. 提供默认资源（无后缀版本）作为回退
3. 检查资源键的拼写是否正确
4. 监听 `DynamicSkinService.ErrorOutput` 事件获取详细信息

### 性能问题

**问题：** 主题切换时出现卡顿

**解决方案：**
1. 使用资源缓存 - 库已内置缓存机制
2. 减少资源字典的嵌套层级
3. 预加载常用主题
4. 避免在资源中使用复杂的绑定或转换器

### 设计器显示问题

**问题：** Visual Studio 设计器无法显示正确的样式

**解决方案：**
- 设计器会自动回退到静态资源查找
- 这是正常行为，运行时会正常工作
- 可以使用 `d:DataContext` 在设计时指定主题

## 📚 示例项目

完整的示例项目请查看：
- [Dreamland.WPF.DynamicSkin.Demo](../Dreamland.WPF.DynamicSkin.Demo) - 包含多种主题和控件的完整演示

## 📄 许可证 License

本项目采用 [MIT](../../LICENSE) 许可证。

## 🔗 链接 Links

- **GitHub**: [https://github.com/Firito/Dreamland.WPF.DynamicSkin](https://github.com/Firito/Dreamland.WPF.DynamicSkin)
- **NuGet**: [https://www.nuget.org/packages/Dreamland.WPF.DynamicSkin](https://www.nuget.org/packages/Dreamland.WPF.DynamicSkin)
- **Issues**: [https://github.com/Firito/Dreamland.WPF.DynamicSkin/issues](https://github.com/Firito/Dreamland.WPF.DynamicSkin/issues)
---

**感谢使用 Dreamland.WPF.DynamicSkin! 🎨✨**

If you find this library helpful, please give it a ⭐ on GitHub!
