# 🎮 CSimpleGame

An experimental 2D platformer built in C# using **Raylib-cs**.  
This is a work-in-progress sandbox project made for learning Raylib and testing basic gameplay mechanics.

---

## 📸 Screenshot  
*(To be added – currently a prototype with gray background and basic shapes)*

---

## 🚀 Status

⛏ **In Development** — currently implemented features:
- [x] Basic player movement (left/right/jump)
- [x] Gravity simulation
- [x] Camera following player
- [x] Floor and obstacle system
- [x] Debug overlay (mouse/player position, FPS)
- [x] Death and restart system
- [x] Obstacle creation with mouse clicks (with console export)

🔜 Planned features:
- [ ] Improved collision and physics
- [ ] Level system and progression
- [ ] Animations and visual polish
- [ ] UI & Menu system
- [ ] Sound effects
- [ ] Health / damage system

---

## 🧠 Controls

| Key         | Action                    |
|-------------|----------------------------|
| A / ←        | Move left                 |
| D / →        | Move right                |
| W / ↑ / Space| Jump                      |
| S / ↓        | Drop down from platforms  |
| Left Click   | Create obstacle (2 clicks)|
| Right Click  | Print obstacles to console|
| Restart      | Click button on screen after death |

---

## 🛠️ Requirements

- [.NET 6.0+](https://dotnet.microsoft.com/en-us/download)
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)

Recommended: Visual Studio 2022+ or JetBrains Rider.

---

## ⚙️ How to Build

```bash
git clone https://github.com/your_username/CSimpleGame.git
cd CSimpleGame
dotnet build
dotnet run
```

---

## 📁 Files to Track in Git

Include:
- `*.cs` – source code
- `*.csproj`, `*.sln` – project files
- `README.md`

Ignore:
- `bin/`, `obj/` – build artifacts
- `.vs/`, `.vscode/` – IDE-specific files

Use a proper `.gitignore`, for example:  
[https://github.com/github/gitignore/blob/main/VisualStudio.gitignore](https://github.com/github/gitignore/blob/main/VisualStudio.gitignore)

---

## 🤪 Easter Egg

```csharp
string test_text = "This is a test. A very important test... that tests your mental stability.";
```

See that? That’s your warning. Don’t click too hard. This game has no sanity meter. Yet.

---