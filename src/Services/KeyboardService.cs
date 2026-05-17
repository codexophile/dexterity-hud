using System;
using System.Text;
using System.Windows.Threading;

namespace DexterityHud.Services
{
    internal sealed class KeyboardService : IDisposable
    {
        private readonly DispatcherTimer _pollTimer;
        private IntPtr _currentKeyboardLayout;

        public KeyboardService(TimeSpan? pollInterval = null)
        {
            _pollTimer = new DispatcherTimer
            {
                Interval = pollInterval ?? TimeSpan.FromMilliseconds(200)
            };
            _pollTimer.Tick += OnPollTimerTick;
        }

        public event EventHandler<IntPtr>? KeyboardLayoutChanged;

        public IntPtr CurrentKeyboardLayout => _currentKeyboardLayout;

        public void Start()
        {
            UpdateCurrentLayout(forceRaiseEvent: false);
            _pollTimer.Start();
        }

        public void Stop()
        {
            _pollTimer.Stop();
        }

        public string TranslateVirtualKey(uint virtualKey)
        {
            var keyboardLayout = _currentKeyboardLayout == IntPtr.Zero
                ? Win32Native.GetForegroundKeyboardLayout()
                : _currentKeyboardLayout;

            if (keyboardLayout == IntPtr.Zero)
            {
                return string.Empty;
            }

            return Win32Native.VirtualKeyToString(virtualKey, keyboardLayout);
        }

        private void OnPollTimerTick(object? sender, EventArgs e)
        {
            UpdateCurrentLayout(forceRaiseEvent: true);
        }

        private void UpdateCurrentLayout(bool forceRaiseEvent)
        {
            var foregroundLayout = Win32Native.GetForegroundKeyboardLayout();
            if (foregroundLayout == IntPtr.Zero)
            {
                return;
            }

            if (!forceRaiseEvent && foregroundLayout == _currentKeyboardLayout)
            {
                return;
            }

            _currentKeyboardLayout = foregroundLayout;
            KeyboardLayoutChanged?.Invoke(this, _currentKeyboardLayout);
        }

        public void Dispose()
        {
            _pollTimer.Stop();
            _pollTimer.Tick -= OnPollTimerTick;
        }
    }
}
