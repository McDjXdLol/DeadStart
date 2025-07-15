# 🎮 DeadStart

An experimental 2D platformer built in C# using [Raylib-cs](https://github.com/ChrisDill/Raylib-cs).  
This is a WIP sandbox-style project made for learning Raylib and prototyping basic platformer mechanics.  
It’s raw, but playable.

---

## 🚀 Status

⛏ **In Development**  
Implemented features so far:

- [x] Player movement (left / right / jump / drop)
- [x] Basic gravity simulation
- [x] Camera that follows the player
- [x] Floor and obstacle rendering
- [x] Collision detection with obstacles
- [x] Player death + restart system
- [x] Click-based obstacle creation (with console export)
- [x] Debug overlay with FPS, mouse & player position

🔮 Planned (maybe soon, maybe "eventually"):

- [ ] Audio :)

---

## 🧠 Controls

| Key / Mouse    | Action                             |
|----------------|-------------------------------------|
| A / ←          | Move left                           |
| D / →          | Move right                          |
| W / ↑ / Space  | Jump           |
| S / ↓          | Drop through platforms              |
| Left Click     | Place obstacle (click twice)        |

---

## 🛠️ Requirements

- [.NET 9.0+](https://dotnet.microsoft.com/en-us/download)
- [Raylib-cs](https://www.nuget.org/packages/Raylib-cs)

Tested on **Visual Studio 2022**.

---

## ⚙️ How to Build / Run

```bash
git clone https://github.com/McDjXdLol/DeadStart.git
cd DeadStart
dotnet restore
dotnet run
```

---

## 💡 Notes
This project was built with learning in mind — low-level game mechanics using Raylib from scratch.
There’s no game engine magic here. No Unity. No Unreal. Just rectangles, loops, and hope.

Feel free to fork, build on top, or laugh at it. Contributions welcome (especially if you bring your own coffee).

---

## 📄 License
Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

---
