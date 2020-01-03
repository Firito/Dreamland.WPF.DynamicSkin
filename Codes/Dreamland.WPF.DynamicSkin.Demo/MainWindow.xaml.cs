using System.Collections.Generic;
using System.Windows;

namespace Dreamland.WPF.DynamicSkin.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Dictionary<string, string> _skinsDictionary = new Dictionary<string, string>()
        {
            {"默认", "" },
            {"暗色", "DarkSkin" },
            {"绿色", "GreenSkin" }
        };


        public MainWindow()
        {
            InitializeComponent();

            ComboBox.ItemsSource = _skinsDictionary;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            DynamicSkinService.CurrentSkinName = ComboBox.SelectedValue.ToString();
        }
    }
}
