using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dreamland.WPF.DynamicSkin;

/// <summary>
///     动态皮肤转换器 / Dynamic skin converter
/// </summary>
internal class DynamicSkinConverter : IValueConverter
{
    private readonly WeakReference<FrameworkElement> _elementReference;

    /// <summary>
    ///     初始化动态皮肤转换器 / Initialize dynamic skin converter
    /// </summary>
    /// <param name="element">框架元素 / Framework element</param>
    public DynamicSkinConverter(FrameworkElement element)
    {
        _elementReference =
            new WeakReference<FrameworkElement>(element ?? throw new ArgumentNullException(nameof(element)));
    }

    /// <summary>
    ///     转换值 / Convert value
    /// </summary>
    /// <param name="value">源值 / Source value</param>
    /// <param name="targetType">目标类型 / Target type</param>
    /// <param name="parameter">转换参数（资源键） / Converter parameter (resource key)</param>
    /// <param name="culture">文化信息 / Culture info</param>
    /// <returns>转换后的资源对象 / Converted resource object</returns>
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var resourceKey = parameter?.ToString();
        var skinName = value?.ToString();

        if (string.IsNullOrWhiteSpace(resourceKey))
        {
            DynamicSkinService.RaiseErrorOutput(
                "[DynamicSkinError] DynamicSkinConverter: Parameter error. Resource key is null or empty.");
            return null;
        }

        if (!_elementReference.TryGetTarget(out var element))
        {
            DynamicSkinService.RaiseErrorOutput(
                "[DynamicSkinError] DynamicSkinConverter: FrameworkElement has been garbage collected.");
            return null;
        }

        try
        {
            return FindResource(element, resourceKey, skinName);
        }
        catch (Exception ex)
        {
            DynamicSkinService.RaiseErrorOutput($"[DynamicSkinError] DynamicSkinConverter: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    ///     不支持反向转换 / ConvertBack is not supported
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("DynamicSkinConverter does not support ConvertBack operation.");
    }

    /// <summary>
    ///     查找资源 / Find resource
    /// </summary>
    /// <param name="element">框架元素 / Framework element</param>
    /// <param name="resourceKey">资源键 / Resource key</param>
    /// <param name="skinName">皮肤名称 / Skin name</param>
    /// <returns>找到的资源对象 / Found resource object</returns>
    private object? FindResource(FrameworkElement element, string resourceKey, string? skinName)
    {
        // 如果没有皮肤名称，直接查找基础资源 / If no skin name, find base resource directly
        if (string.IsNullOrWhiteSpace(skinName)) return FindResourceWithFallback(element, resourceKey);

        // 构造带皮肤名称的资源键 / Construct resource key with skin name
        var skinResourceKey = $"{resourceKey}.{skinName}";

        // 首先尝试从缓存中获取 / First try to get from cache
        if (DynamicSkinService.TryGetResource(skinResourceKey, out var cachedResource) && cachedResource != null)
            return cachedResource;

        // 尝试从元素中查找带皮肤名称的资源 / Try to find resource with skin name from element
        var resource = element.TryFindResource(skinResourceKey);
        if (resource != null)
        {
            // 缓存找到的资源 / Cache the found resource
            DynamicSkinService.AddOrUpdateResource(skinResourceKey, resource);
            return resource;
        }

        // 如果找不到，输出错误并回退到基础资源 / If not found, output error and fall back to base resource
        DynamicSkinService.RaiseErrorOutput(
            $"[DynamicSkinError] DynamicSkinConverter: Can't find resource '{skinResourceKey}', falling back to '{resourceKey}'");

        return FindResourceWithFallback(element, resourceKey);
    }

    /// <summary>
    ///     带回退机制查找资源 / Find resource with fallback mechanism
    /// </summary>
    /// <param name="element">框架元素 / Framework element</param>
    /// <param name="resourceKey">资源键 / Resource key</param>
    /// <returns>找到的资源对象 / Found resource object</returns>
    private object? FindResourceWithFallback(FrameworkElement element, string resourceKey)
    {
        // 首先尝试从缓存获取 / First try to get from cache
        if (DynamicSkinService.TryGetResource(resourceKey, out var cachedResource) && cachedResource != null)
            return cachedResource;

        // 从元素中查找 / Find from element
        var resource = element.TryFindResource(resourceKey);
        if (resource != null)
            // 缓存找到的资源 / Cache the found resource
            DynamicSkinService.AddOrUpdateResource(resourceKey, resource);

        return resource;
    }
}