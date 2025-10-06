using System.Windows;
using System.Windows.Controls;

namespace Dreamland.WPF.DynamicSkin.Demo.Controls;

/// <summary>
///     Bilingual text block control displaying Chinese and English text
/// </summary>
public partial class BilingualTextBlock : UserControl
{
    /// <summary>
    ///     Chinese text
    /// </summary>
    public static readonly DependencyProperty ChineseTextProperty =
        DependencyProperty.Register(nameof(ChineseText), typeof(string), typeof(BilingualTextBlock),
            new PropertyMetadata(string.Empty, OnTextChanged));

    /// <summary>
    ///     English text
    /// </summary>
    public static readonly DependencyProperty EnglishTextProperty =
        DependencyProperty.Register(nameof(EnglishText), typeof(string), typeof(BilingualTextBlock),
            new PropertyMetadata(string.Empty, OnTextChanged));

    /// <summary>
    ///     Chinese font size
    /// </summary>
    public static readonly DependencyProperty ChineseFontSizeProperty =
        DependencyProperty.Register(nameof(ChineseFontSize), typeof(double), typeof(BilingualTextBlock),
            new PropertyMetadata(14.0));

    /// <summary>
    ///     English font size
    /// </summary>
    public static readonly DependencyProperty EnglishFontSizeProperty =
        DependencyProperty.Register(nameof(EnglishFontSize), typeof(double), typeof(BilingualTextBlock),
            new PropertyMetadata(11.0));

    /// <summary>
    ///     Chinese font weight
    /// </summary>
    public static readonly DependencyProperty ChineseFontWeightProperty =
        DependencyProperty.Register(nameof(ChineseFontWeight), typeof(FontWeight), typeof(BilingualTextBlock),
            new PropertyMetadata(FontWeights.Normal));

    /// <summary>
    ///     Show translation indicator (blue vertical bar)
    /// </summary>
    public static readonly DependencyProperty ShowTranslationIndicatorProperty =
        DependencyProperty.Register(nameof(ShowTranslationIndicator), typeof(bool), typeof(BilingualTextBlock),
            new PropertyMetadata(true));

    public BilingualTextBlock()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string ChineseText
    {
        get => (string)GetValue(ChineseTextProperty);
        set => SetValue(ChineseTextProperty, value);
    }

    public string EnglishText
    {
        get => (string)GetValue(EnglishTextProperty);
        set => SetValue(EnglishTextProperty, value);
    }

    public double ChineseFontSize
    {
        get => (double)GetValue(ChineseFontSizeProperty);
        set => SetValue(ChineseFontSizeProperty, value);
    }

    public double EnglishFontSize
    {
        get => (double)GetValue(EnglishFontSizeProperty);
        set => SetValue(EnglishFontSizeProperty, value);
    }

    public FontWeight ChineseFontWeight
    {
        get => (FontWeight)GetValue(ChineseFontWeightProperty);
        set => SetValue(ChineseFontWeightProperty, value);
    }

    public bool ShowTranslationIndicator
    {
        get => (bool)GetValue(ShowTranslationIndicatorProperty);
        set => SetValue(ShowTranslationIndicatorProperty, value);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is BilingualTextBlock control) control.UpdateTextBlocks();
    }

    private void UpdateTextBlocks()
    {
        // The text bindings are handled by the XAML bindings
        // This method can be used for additional logic if needed
    }
}