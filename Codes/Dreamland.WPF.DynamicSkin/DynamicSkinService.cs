using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Windows;

namespace Dreamland.WPF.DynamicSkin
{
    /// <summary>
    /// 换肤服务
    /// </summary>
    public static class DynamicSkinService
    {
        static DynamicSkinService()
        {
            Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                ResourceDictionary = new ConcurrentDictionary<string, object>();
            });
        }

        /// <summary>
        /// 错误输出
        /// </summary>
        public static event EventHandler<ErrorOutputEventArgs> ErrorOutput;

        /// <summary>
        /// 皮肤Key
        /// </summary>
        public static string CurrentSkinName
        {
            get => DynamicSkinModel.CurrentSkinName;
            set => DynamicSkinModel.CurrentSkinName = value;
        }

        /// <summary>
        /// 资源字典
        /// </summary>
        internal static ConcurrentDictionary<string, object> ResourceDictionary { get; private set; }

        /// <summary>
        /// 动态皮肤模块
        /// </summary>
        internal static DynamicSkinModel DynamicSkinModel = new DynamicSkinModel();

        internal static void RaiseErrorOutput(string errorMessage)
        {
            Task.Run(() =>
            {
                ErrorOutput?.Invoke(null, new ErrorOutputEventArgs(errorMessage));
            });
        }
    }
}
