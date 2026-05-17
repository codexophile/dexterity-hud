using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DexterityHud.Services;

namespace DexterityHud.ViewModels
{
    internal sealed class MainViewModel : IDisposable
    {
        private readonly KeyboardService _keyboardService;
        private bool _isAlwaysOnTop;

        public MainViewModel()
            : this(new KeyboardService())
        {
        }

        public MainViewModel(KeyboardService keyboardService)
        {
            _keyboardService = keyboardService;
            Keys = new ObservableCollection<KeyViewModel>(BuildKeys());
            Rows = new ObservableCollection<KeyboardRowViewModel>(BuildRows(Keys));
            _keyboardService.KeyboardLayoutChanged += OnKeyboardLayoutChanged;
            _keyboardService.Start();
            RefreshSecondaryTexts();
        }

        public ObservableCollection<KeyViewModel> Keys { get; }

        public ObservableCollection<KeyboardRowViewModel> Rows { get; }

        public bool IsAlwaysOnTop
        {
            get => _isAlwaysOnTop;
            set
            {
                if (_isAlwaysOnTop == value)
                {
                    return;
                }

                _isAlwaysOnTop = value;
                ApplyTopmost(value);
            }
        }

        public void Dispose()
        {
            _keyboardService.KeyboardLayoutChanged -= OnKeyboardLayoutChanged;
            _keyboardService.Dispose();
        }

        private void OnKeyboardLayoutChanged(object? sender, IntPtr keyboardLayout)
        {
            RefreshSecondaryTexts();
        }

        private void RefreshSecondaryTexts()
        {
            foreach (var key in Keys)
            {
                key.UpdateSecondaryText(_keyboardService.TranslateVirtualKey(key.VirtualKey));
            }
        }

        private static IEnumerable<KeyViewModel> BuildKeys()
        {
            return new[]
            {
                new KeyViewModel(0x31, "1", Brushes.SaddleBrown),
                new KeyViewModel(0x32, "2", Brushes.DarkGoldenrod),
                new KeyViewModel(0x33, "3", Brushes.OliveDrab),
                new KeyViewModel(0x34, "4", Brushes.RoyalBlue),
                new KeyViewModel(0x35, "5", Brushes.RoyalBlue),
                new KeyViewModel(0x36, "6", Brushes.MediumSeaGreen),
                new KeyViewModel(0x37, "7", Brushes.MediumSeaGreen),
                new KeyViewModel(0x38, "8", Brushes.DarkCyan),
                new KeyViewModel(0x39, "9", Brushes.DarkCyan),
                new KeyViewModel(0x30, "0", Brushes.IndianRed),
                new KeyViewModel(0x51, "Q", Brushes.SaddleBrown),
                new KeyViewModel(0x57, "W", Brushes.DarkGoldenrod),
                new KeyViewModel(0x45, "E", Brushes.OliveDrab),
                new KeyViewModel(0x52, "R", Brushes.RoyalBlue),
                new KeyViewModel(0x54, "T", Brushes.RoyalBlue),
                new KeyViewModel(0x59, "Y", Brushes.MediumSeaGreen),
                new KeyViewModel(0x55, "U", Brushes.MediumSeaGreen),
                new KeyViewModel(0x49, "I", Brushes.DarkCyan),
                new KeyViewModel(0x4F, "O", Brushes.DarkCyan),
                new KeyViewModel(0x50, "P", Brushes.IndianRed),
                new KeyViewModel(0x41, "A", Brushes.SaddleBrown),
                new KeyViewModel(0x53, "S", Brushes.DarkGoldenrod),
                new KeyViewModel(0x44, "D", Brushes.OliveDrab),
                new KeyViewModel(0x46, "F", Brushes.RoyalBlue),
                new KeyViewModel(0x47, "G", Brushes.RoyalBlue),
                new KeyViewModel(0x48, "H", Brushes.MediumSeaGreen),
                new KeyViewModel(0x4A, "J", Brushes.MediumSeaGreen),
                new KeyViewModel(0x4B, "K", Brushes.DarkCyan),
                new KeyViewModel(0x4C, "L", Brushes.DarkCyan),
                new KeyViewModel(0xBA, ";", Brushes.IndianRed),
                new KeyViewModel(0x5A, "Z", Brushes.SaddleBrown),
                new KeyViewModel(0x58, "X", Brushes.DarkGoldenrod),
                new KeyViewModel(0x43, "C", Brushes.OliveDrab),
                new KeyViewModel(0x56, "V", Brushes.RoyalBlue),
                new KeyViewModel(0x42, "B", Brushes.RoyalBlue),
                new KeyViewModel(0x4E, "N", Brushes.MediumSeaGreen),
                new KeyViewModel(0x4D, "M", Brushes.MediumSeaGreen),
                new KeyViewModel(0xBC, ",", Brushes.DarkCyan),
                new KeyViewModel(0xBE, ".", Brushes.DarkCyan),
                new KeyViewModel(0xBF, "/", Brushes.IndianRed),
                new KeyViewModel(0xBD, "-", Brushes.IndianRed),
                new KeyViewModel(0xBB, "=", Brushes.IndianRed),
                new KeyViewModel(0xDB, "[", Brushes.IndianRed),
                new KeyViewModel(0xDD, "]", Brushes.IndianRed),
                new KeyViewModel(0xDE, "'", Brushes.IndianRed),
            };
        }

        private static IEnumerable<KeyboardRowViewModel> BuildRows(IReadOnlyCollection<KeyViewModel> keys)
        {
            var lookup = keys.ToDictionary(key => key.VirtualKey, key => key);

            return new[]
            {
                new KeyboardRowViewModel(new[]
                {
                    lookup[0x31], lookup[0x32], lookup[0x33], lookup[0x34], lookup[0x35],
                    lookup[0x36], lookup[0x37], lookup[0x38], lookup[0x39], lookup[0x30]
                }),
                new KeyboardRowViewModel(new[]
                {
                    lookup[0x51], lookup[0x57], lookup[0x45], lookup[0x52], lookup[0x54],
                    lookup[0x59], lookup[0x55], lookup[0x49], lookup[0x4F], lookup[0x50]
                }),
                new KeyboardRowViewModel(new[]
                {
                    lookup[0x41], lookup[0x53], lookup[0x44], lookup[0x46], lookup[0x47],
                    lookup[0x48], lookup[0x4A], lookup[0x4B], lookup[0x4C], lookup[0xBA], lookup[0xDE]
                }),
                new KeyboardRowViewModel(new[]
                {
                    lookup[0x5A], lookup[0x58], lookup[0x43], lookup[0x56], lookup[0x42],
                    lookup[0x4E], lookup[0x4D], lookup[0xBC], lookup[0xBE], lookup[0xBF], lookup[0xBD],
                    lookup[0xBB], lookup[0xDB], lookup[0xDD]
                })
            };
        }

        private static void ApplyTopmost(bool isTopmost)
        {
            var application = Application.Current;
            if (application?.MainWindow != null)
            {
                application.MainWindow.Topmost = isTopmost;
            }
        }
    }
}
