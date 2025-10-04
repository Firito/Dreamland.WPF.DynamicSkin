using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Dreamland.WPF.DynamicSkin;

/// <summary>
///     实现支持通用资源（Brush、Color、Thickness 等）的标记扩展。
///     适用于非 Style 类型的动态资源绑定。
///     Markup extension that supports general resources (Brush, Color, Thickness, etc.).
///     Suitable for dynamic resource binding of non-Style types.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
[Localizability(LocalizationCategory.NeverLocalize)]
public class DynamicResourceExtension : StaticResourceExtension
{
    /// <summary>
    ///     初始化 <see cref="DynamicResourceExtension" /> 类的新实例。
    ///     Initializes a new instance of the <see cref="DynamicResourceExtension" /> class.
    /// </summary>
    public DynamicResourceExtension()
    {
    }

    /// <summary>
    ///     初始化 <see cref="DynamicResourceExtension" /> 类的新实例，并提供初始资源键。
    ///     Initializes a new instance of the <see cref="DynamicResourceExtension" /> class with an initial resource key.
    /// </summary>
    /// <param name="resourceKey">此标记扩展所引用的资源键 / The resource key referenced by this markup extension</param>
    /// <exception cref="ArgumentNullException">
    ///     当 <paramref name="resourceKey" /> 参数为 <see langword="null" />。
    ///     When the <paramref name="resourceKey" /> parameter is <see langword="null" />.
    /// </exception>
    public DynamicResourceExtension(object resourceKey)
    {
        ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));
    }

    /// <summary>
    ///     返回一个应在应用了此扩展的属性上设置的对象。
    ///     Returns an object that should be set on the property where this extension is applied.
    /// </summary>
    /// <param name="serviceProvider">可为标记扩展提供服务的对象 / An object that can provide services for the markup extension</param>
    /// <returns>要在其中计算标记扩展提供值的属性上设置的对象值 / The object value to set on the property where the markup extension is evaluated</returns>
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (serviceProvider == null)
            throw new ArgumentNullException(nameof(serviceProvider));

        // 获取目标对象信息 / Get target object information
        if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service)) return null;

        // MarkupExtension 在样式模板中，返回 this 以延迟提供值 / In style templates, return this to defer value provision
        if (service.TargetObject?.GetType().Name.EndsWith("SharedDp") == true) return this;

        // 如果不是 FrameworkElement，返回 this 以延迟提供值 / If not a FrameworkElement, return this to defer value provision
        if (!(service.TargetObject is FrameworkElement element)) return this;

        // 如果是设计时，返回 ResourceKey 对应的资源 / If in design mode, return the resource corresponding to ResourceKey
        if (DesignerProperties.GetIsInDesignMode(element))
            try
            {
                return element.TryFindResource(ResourceKey);
            }
            catch
            {
                // 设计时查找失败，返回 null / Design-time lookup failed, return null
                return null;
            }

        // 运行时设置绑定 / Set binding at runtime
        return SetBinding(serviceProvider, element);
    }

    /// <summary>
    ///     为元素设置动态绑定 / Set dynamic binding for the element
    /// </summary>
    /// <param name="serviceProvider">服务提供者 / Service provider</param>
    /// <param name="element">目标框架元素 / Target framework element</param>
    /// <returns>绑定对象 / Binding object</returns>
    internal object SetBinding(IServiceProvider serviceProvider, FrameworkElement element)
    {
        var binding = new Binding
        {
            Mode = BindingMode.OneWay,
            Path = new PropertyPath(nameof(DynamicSkinModel.CurrentSkinName)),
            Source = DynamicSkinService.DynamicSkinModel,
            Converter = new DynamicSkinConverter(element),
            ConverterParameter = ResourceKey
        };

        return binding.ProvideValue(serviceProvider);
    }
}