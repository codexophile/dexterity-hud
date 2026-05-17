---
name: Getting started
description: Use this prompt to start implementing the app from nothing.
---

---

# Project Role

Your task is to help me build an alternative On-Screen Keyboard (OSK) visualizer.

# Project Overview

The application is a purely visual On-Screen Keyboard overlay for Windows. At this stage, it DOES NOT need to send keystrokes or perform actual typing. Its primary purpose is to act as a dual-layout visual reference and a touch-typing guide.

# Tech Stack

- Language: C#
- Framework: WPF (Windows Presentation Foundation)
- Target OS: Windows (.NET 8 or latest LTS)
- Architecture: MVVM (Model-View-ViewModel) preferred, or structured code-behind.

# Core Requirements

1. **Always on Top:** The main window must have an option to stay on top of all other windows (Topmost = true).
2. **Dual-Layout Display:** Every key on the UI must be capable of displaying two characters:
   - Primary Layout: US English QWERTY (Fixed).
   - Secondary Layout: The user's current global OS input language.
3. **Real-time Global Language Tracking:** The app must detect the OS input language of the _currently active/focused window_ in Windows, not just the WPF app itself. When the user switches languages in another app, the secondary layout on our WPF app must update in real-time.
4. **Touch-Typing Color Coding:** Keys must be color-coded based on standard touch-typing rules (indicating which hand and finger should press the key).

# Technical Implementation Guidelines (Crucial for AI)

## 1. Global Input Language Detection

WPF's built-in `InputLanguageManager` only tracks the input language of the WPF application thread itself. To track global OS changes in real-time, you MUST use Win32 APIs.

- Use a `DispatcherTimer` (e.g., 200ms interval) to poll the active window.
- **Required Win32 APIs (P/Invoke):**
  - `GetForegroundWindow()`
  - `GetWindowThreadProcessId()`
  - `GetKeyboardLayout()`
- When the foreground keyboard layout (HKL) changes, trigger an update to the ViewModel.

## 2. Character Mapping (Virtual Key to Unicode)

To display the secondary character for a given key, you must translate standard Virtual Keys (VK) into characters based on the currently detected system keyboard layout.

- **Required Win32 APIs (P/Invoke):**
  - `MapVirtualKeyEx()` or `ToUnicodeEx()`
- Map standard QWERTY Virtual Keys (e.g., VK_Q, VK_W, OemSemicolon) to their respective string representations in the current HKL.

## 3. UI and Key Custom Control

Create a custom `UserControl` or define a distinct DataTemplate for a `KeyboardKey`.

- It should have Dependency Properties for:
  - `PrimaryText` (string)
  - `SecondaryText` (string)
  - `FingerColor` (Brush)
- **Visual Structure:** A Border containing a Grid. Place `PrimaryText` in the top-left (or dominant position) and `SecondaryText` in the bottom-right (or secondary position) of the key.

## 4. Color Coding Dictionary

Implement a mapping system that assigns specific colors to specific Virtual Keys based on touch typing:

- **Left Pinky:** 1, Q, A, Z (Color A)
- **Left Ring:** 2, W, S, X (Color B)
- **Left Middle:** 3, E, D, C (Color C)
- **Left Index:** 4, 5, R, T, F, G, V, B (Color D)
- **Right Index:** 6, 7, Y, U, H, J, N, M (Color E)
- **Right Middle:** 8, I, K, , (Color F)
- **Right Ring:** 9, O, L, . (Color G)
- **Right Pinky:** 0, -, =, P, [, ], ;, ', /, etc. (Color H)
- **Thumbs:** Spacebar (Color I)

# Step-by-Step Execution Plan

Please generate the code in the following order:

1. **Win32 Native Methods:** Create a static helper class with the required P/Invoke signatures (`GetForegroundWindow`, `GetKeyboardLayout`, `ToUnicodeEx`, etc.).
2. **Keyboard Service:** Create a class/service that uses a timer to track the global active layout and provides a method to translate a Virtual Key to a string using `ToUnicodeEx`.
3. **ViewModel & Data Models:** Create a `KeyViewModel` representing a single key, and a `MainViewModel` containing an `ObservableCollection<KeyViewModel>`. Initialize it with the standard QWERTY layout and bind the color codes.
4. **XAML Layout:** Generate the `MainWindow.xaml` using a `Viewbox` and `UniformGrid` or `Grid` to lay out the standard keyboard rows. Add a CheckBox for "Stay on Top".
5. **Connecting it up:** Ensure the timer updates the `SecondaryText` property of all `KeyViewModel` instances whenever a layout change is detected.

Begin by confirming you understand these requirements, and then start with Step 1 (Win32 Native Methods).
