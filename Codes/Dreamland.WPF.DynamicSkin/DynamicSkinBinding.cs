using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Dreamland.WPF.DynamicSkin
{
    /// <summary>
    ///     动态皮肤绑定
    /// </summary>
    [MarkupExtensionReturnType(typeof(object))]
    [Localizability(LocalizationCategory.NeverLocalize)]
    public class DynamicSkinBinding : StaticResourceExtension
    {
        /// <summary>
        ///   初始化 <see cref="DynamicSkinBinding" /> 类的新实例。
        /// </summary>
        public DynamicSkinBinding()
        {
        }

        /// <summary>
        ///   新实例初始化 <see cref="DynamicSkinBinding" /> 类，提供的初始密钥。
        /// </summary>
        /// <param name="resourceKey">此标记扩展所引用的资源键。</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="resourceKey" /> 参数是 <see langword="null" />, ，通过标记扩展用法或显式构造。
        /// </exception>
        public DynamicSkinBinding(object resourceKey) => ResourceKey = resourceKey ?? throw new ArgumentNullException(nameof(resourceKey));

        /// <summary>
        ///   返回一个应在应用了此扩展的属性上设置的对象。
        ///    有关 <see cref="DynamicSkinBinding" />, ，这是在资源字典，其中由标识要查找的对象中找到的对象 <see cref="DynamicSkinBinding.ResourceKey" />。
        /// </summary>
        /// <param name="serviceProvider">可为标记扩展提供服务的对象。</param>
        /// <returns>要在其中计算标记扩展提供值的属性上设置的对象值。</returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="serviceProvider" /> 已 <see langword="null" />, ，或未能实现所需的服务。
        /// </exception>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // 如果没有服务，则直接返回。
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget service)) return null;
            // MarkupExtension 在样式模板中，返回 this 以延迟提供值。
            if (service.TargetObject.GetType().Name.EndsWith("SharedDp")) return this;
            // 如果不是 FrameworkElement，那么返回 this 以延迟提供值。
            if (!(service.TargetObject is FrameworkElement element)) return this;
            // 如果是设计时，那么返回ResourceKey对应的资源
            if (DesignerProperties.GetIsInDesignMode(element)) return element.TryFindResource(ResourceKey);

            return SetBinding(serviceProvider, element);
        }

        internal object SetBinding(IServiceProvider serviceProvider, FrameworkElement element)
        {
            return new Binding
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath(nameof(DynamicSkinModel.CurrentSkinName)),
                Source = DynamicSkinService.DynamicSkinModel,
                Converter = new DynamicSkinConverter(element),
                ConverterParameter = ResourceKey
            }.ProvideValue(serviceProvider);
        }
    }
}