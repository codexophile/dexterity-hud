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
        public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        public const uint MAPVK_VK_TO_VSC = 0;

        public static IntPtr GetForegroundKeyboardLayout()
        {
            var hWnd = GetForegroundWindow();
            if (hWnd == IntPtr.Zero) return IntPtr.Zero;
            uint threadId = GetWindowThreadProcessId(hWnd, out _);
            return GetKeyboardLayout(threadId);
        }

        public static string VirtualKeyToString(uint virtualKey, IntPtr hkl)
        {
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKeyEx(virtualKey, MAPVK_VK_TO_VSC, hkl);
            var sb = new StringBuilder(10);
            int result = ToUnicodeEx(virtualKey, scanCode, keyboardState, sb, sb.Capacity, 0, hkl);
            if (result > 0)
                return sb.ToString().Substring(0, result);
            return string.Empty;
        }
    }
}
