using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DexterityHud.Services
{
    internal static class Win32Native
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        public const uint MAPVK_VK_TO_VSC = 0;
        private const int VK_SHIFT = 0x10;
        private const int VK_CONTROL = 0x11;
        private const int VK_MENU = 0x12;
        private const int VK_LSHIFT = 0xA0;
        private const int VK_RSHIFT = 0xA1;
        private const int VK_LCONTROL = 0xA2;
        private const int VK_RCONTROL = 0xA3;
        private const int VK_LMENU = 0xA4;
        private const int VK_RMENU = 0xA5;

        public static IntPtr GetForegroundKeyboardLayout()
        {
            var hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return IntPtr.Zero;
            uint threadId = GetWindowThreadProcessId(hWnd, out _);
            return GetKeyboardLayout(threadId);
        }

        public static string VirtualKeyToString(uint virtualKey, IntPtr hkl)
        {
            var keyboardState = GetKeyboardStateSnapshot();

            uint scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC, hkl);
            var sb = new StringBuilder(10);
            int result = ToUnicodeEx(virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0, hkl);
            if (result > 0)
                return sb.ToString().Substring(0, result);
            return string.Empty;
        }

        private static byte[] GetKeyboardStateSnapshot()
        {
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            ApplyAsyncKeyState(keyboardState, VK_SHIFT, VK_LSHIFT, VK_RSHIFT);
            ApplyAsyncKeyState(keyboardState, VK_CONTROL, VK_LCONTROL, VK_RCONTROL);
            ApplyAsyncKeyState(keyboardState, VK_MENU, VK_LMENU, VK_RMENU);

            return keyboardState;
        }

        private static void ApplyAsyncKeyState(byte[] keyboardState, int aggregateKey, params int[] keys)
        {
            bool isPressed = false;

            foreach (var key in keys)
            {
                if ((GetAsyncKeyState(key) & short.MinValue) != 0)
                {
                    isPressed = true;
                    break;
                }
            }

            keyboardState[aggregateKey] = isPressed ? (byte)0x80 : (byte)0x00;
        }
    }
}
