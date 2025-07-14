# CSharpTetris

CSharpTetris is a simple but functional Tetris clone built with C# and WinForms.  
It is designed to demonstrate basic game mechanics such as shape movement, rotation, collision detection, and line clearing.

---

## Features

- Tetromino generation with random shapes and colors
- Basic movement and collision detection
- Line clearing and block dropping
- Keyboard input for player controls
- Simple WinForms-based GUI
- Sound support via NAudio

---

## Controls

| Key       | Action             |
|-----------|--------------------|
| ← / →     | Move left/right    |
| ↓         | Soft drop          |
| ↑    | rotate |

---

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Optional: [NAudio NuGet package](https://www.nuget.org/packages/NAudio) for MP3 playback

To install NAudio:

```bash
dotnet add package NAudio
