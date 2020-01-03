using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dreamland.WPF.DynamicSkin.Annotations;

namespace Dreamland.WPF.DynamicSkin
{
    /// <summary>
    /// 动态皮肤模块
    /// </summary>
    internal class DynamicSkinModel : INotifyPropertyChanged
    {
        private string _currentSkinName;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string CurrentSkinName
        {
            get => _currentSkinName;
            set
            {
                _currentSkinName = value;
                OnPropertyChanged();
            }
        }
    }
}
