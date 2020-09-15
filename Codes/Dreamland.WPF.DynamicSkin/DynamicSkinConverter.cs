using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Dreamland.WPF.DynamicSkin
{
    internal class DynamicSkinConverter : IValueConverter
    {
        private readonly FrameworkElement _element;

        public DynamicSkinConverter(FrameworkElement element)
        {
            _element = element;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resourceKey = parameter?.ToString();
            var skinName = value?.ToString();

            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                DynamicSkinService.RaiseErrorOutput(
                    $"[DynamicSkinError] DynamicSkinMultiConverter: Parameter error. Resource is {resourceKey} ");
                return null;
            }

            try
            {
                object resource = null;

                if (string.IsNullOrWhiteSpace(skinName))
                    return DynamicSkinService.ResourceDictionary?.TryGetValue(resourceKey, out resource) == true
                        ? resource
                        : _element.TryFindResource(resourceKey);

                if (DynamicSkinService.ResourceDictionary?.TryGetValue(resourceKey + "." + skinName, out resource) ==
                    true) return resource;

                if ((resource = _element.TryFindResource(resourceKey + "." + skinName)) != null) return resource;

                DynamicSkinService.RaiseErrorOutput(
                    $"[DynamicSkinError] DynamicSkinMultiConverter: Can't find {resourceKey + "." + skinName}");
                return DynamicSkinService.ResourceDictionary?.TryGetValue(resourceKey, out resource) == true
                    ? resource
                    : _element.TryFindResource(resourceKey);
            }
            catch (Exception ex)
            {
                DynamicSkinService.RaiseErrorOutput($"[DynamicSkinError] DynamicSkinMultiConverter: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object resourceKey, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}