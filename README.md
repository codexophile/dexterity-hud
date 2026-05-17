# Dexterity HUD

Dexterity HUD is a WPF overlay that shows a dual-layout on-screen keyboard reference and updates the secondary layout from the active Windows input language.

It also samples Shift, Ctrl, and Alt globally so translated key labels stay accurate even when the HUD window is not focused.

The key legend uses a larger secondary label so alternate-language characters remain readable at the default HUD scale.

The HUD intentionally omits Backspace, Tab, Caps, Shift, and Space from the visible layout.

## Build

```powershell
dotnet build
```

## Run

```powershell
dotnet run
```
