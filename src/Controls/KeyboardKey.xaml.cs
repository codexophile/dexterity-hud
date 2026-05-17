using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DexterityHud.Controls
{
    public partial class KeyboardKey : UserControl
    {
        public static readonly DependencyProperty PrimaryTextProperty = DependencyProperty.Register(
            nameof(PrimaryText), typeof(string), typeof(KeyboardKey), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SecondaryTextProperty = DependencyProperty.Register(
            nameof(SecondaryText), typeof(string), typeof(KeyboardKey), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty FingerColorProperty = DependencyProperty.Register(
            nameof(FingerColor), typeof(Brush), typeof(KeyboardKey), new PropertyMetadata(Brushes.SlateGray));

        public KeyboardKey()
        {
            InitializeComponent();
        }

        public string PrimaryText
        {
            get => (string)GetValue(PrimaryTextProperty);
            set => SetValue(PrimaryTextProperty, value);
        }

        public string SecondaryText
        {
            get => (string)GetValue(SecondaryTextProperty);
            set => SetValue(SecondaryTextProperty, value);
        }

        public Brush FingerColor
        {
            get => (Brush)GetValue(FingerColorProperty);
            set => SetValue(FingerColorProperty, value);
        }
    }
}