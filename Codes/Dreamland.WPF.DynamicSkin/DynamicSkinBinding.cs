using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Dreamland.WPF.DynamicSkin
{
    internal class DynamicSkinBinding : MarkupExtension
    {
        public string ResourceKey { get; }

        public DynamicSkinBinding(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

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
            return new Binding()
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
