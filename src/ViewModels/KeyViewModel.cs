using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DexterityHud.ViewModels
{
    internal sealed class KeyViewModel : INotifyPropertyChanged
    {
        private string _secondaryText = string.Empty;

        public KeyViewModel(uint virtualKey, string primaryText, Brush fingerColor)
        {
            VirtualKey = virtualKey;
            PrimaryText = primaryText;
            FingerColor = fingerColor;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public uint VirtualKey { get; }

        public string PrimaryText { get; }

        public string SecondaryText
        {
            get => _secondaryText;
            private set
            {
                if (_secondaryText == value)
                {
                    return;
                }

                _secondaryText = value;
                OnPropertyChanged();
            }
        }

        public Brush FingerColor { get; }

        public void UpdateSecondaryText(string? text)
        {
            SecondaryText = string.IsNullOrWhiteSpace(text) ? string.Empty : text;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
