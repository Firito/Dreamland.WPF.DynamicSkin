using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dreamland.WPF.DynamicSkin;

/// <summary>
///     动态皮肤模块 / Dynamic skin model
/// </summary>
internal class DynamicSkinModel : INotifyPropertyChanged
{
    private readonly object _lockObject = new();
    private string? _currentSkinName;

    /// <summary>
    ///     当前皮肤名称 / Current skin name
    /// </summary>
    public string? CurrentSkinName
    {
        get
        {
            lock (_lockObject)
            {
                return _currentSkinName;
            }
        }
        set
        {
            lock (_lockObject)
            {
                if (_currentSkinName == value)
                    return;

                _currentSkinName = value;
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     属性更改事件 / Property changed event
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     触发属性更改事件 / Raise property changed event
    /// </summary>
    /// <param name="propertyName">属性名称 / Property name</param>
    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}