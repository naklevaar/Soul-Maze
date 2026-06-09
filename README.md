# 🌀 Soul Maze: The Shinigami Trials
A 3D mobile physics-based maze game developed in Unity 6 LTS as a technical evaluation assessment for the Reliance Games / Zapak Internship.

## 🕹️ Project Overview & Core Mechanics
In **Soul Maze**, the player guides a glowing Spirit Orb through an intricate maze styled after the structural corridors of the Seireitei. The mission is to navigate branching paths, dodge automated obstacles, and reach the Captain's Finish Zone. 

The game operates on a dynamic **Reiatsu (Spiritual Pressure) Score Economy**. Players start with a maximum energy pool that continuously ticks down over time. Sustaining damage from traps inflicts severe flat score deductions and forces spatial respawns, testing the player's fine motor skills and risk-reward decision-making.

### Dual-Platform Input Handling
To streamline cross-platform evaluation, the codebase implements an intelligent hybrid input processing matrix:
* **PC / Unity Editor Playtesting:** Move the ball seamlessly using standard desktop **W, A, S, D** or Arrow keys. Press **C** to hot-swap cameras, and **Escape** to access the system pause layout.
* **Android Mobile Deployment:** Hold the device flat and physically **tilt the handset**. Internal hardware sensors manipulate localized gravity vectors to roll the sphere. Tap dedicated on-screen HUD buttons for camera and menu interactions. Screen orientation is completely locked to Landscape.

---

## 🛠️ Architecture & Technical Implementations

### 1. Multi-Scene Flow State
The project architecture isolates functional dependencies into distinct scene assets to maximize optimization:
* **`MainMenu` Scene:** Handles initial boot sequences, local registry high-score rendering, and cross-scene configuration buttons.
* **`GameScene` Scene:** Houses the 3D grid layout, physics simulation matrices, real-time clocks, and interface canvas overlays.

### 2. State & Position Management (`GameManager.cs`)
Features a centralized, decoupled manager pattern that monitors active runtime state variables. It tracks spatial checkpoint data coordinates mapped to Ichigo's Substitute Shinigami Pass objects. Upon player hazard collision, the system intercepts physical force vectors, zeroes residual angular/linear velocities, and teleports the ball safely back to the last active checkpoint registry without requiring hard scene reloads.

### 3. Persistent Leaderboard Data Structure (`LeaderboardManager.cs`)
Satisfies evaluation metrics regarding advanced data collections and serialization. High scores are calculated by the `ScoreManager.cs` framework and mapped to a custom `ScoreEntry` data class object. The structural architecture:
* Employs an anonymous lambda sorting delegate expression (`(x, y) => y.score.CompareTo(x.score)`) to arrange data values in descending chronological sequence.
* Constrains the collection size dynamically to retain exactly the Top 5 ranks.
* Serializes the underlying generic list into a structured JSON text string via `JsonUtility`, archiving the string natively inside the device device registry (`PlayerPrefs`) to persist data across full hardware restarts.

### 4. Scalable UI Architecture
Interface layers (Pause, Win, Lose, Main Menu panels) are driven by a centralized `UIManager.cs` layout engine. It overrides native pixel boundaries to implement resolution-independent anchors (`Scale With Screen Size` at a target 1920x1080 matrix). Elements use nested layout groups (`VerticalLayoutGroup`) with manual bounding overrides to guarantee that alignment layers remain locked across varying mobile device screens.

---

## ⚠️ Engineering Challenges Faced & Resolved

### Bug 1: The Cross-Scene Input/Camera Deadlock
* **Issue:** Returning to the Main Menu and clicking Start a second time caused the keyboard input listeners, pause triggers, and camera tracking engines to completely freeze while the ball dropped helplessly through the world.
* **Root Cause:** Persistent manager allocations (`DontDestroyOnLoad`) were holding stale dangling heap pointers to destroyed scene objects from the previous scene instance, forcing new inputs to fire silently against null targets.
* **Resolution:** Stripped out cross-scene memory allocations and localized singleton generation directly inside `Awake()`. Explicitly forced temporal runtime unfreezing operations (`Time.timeScale = 1f`) immediately prior to executing any `SceneManager.LoadScene` instructions to ensure physical update frames reset cleanly.

### Bug 2: Rapid Multi-Frame Damage Truncation
* **Issue:** Bumping into an animated trap (the PressTrap or the Spinning Blade) would instantly wipe out thousands of score points and trigger a game-over panel within a fraction of a second.
* **Root Cause:** Unity's physics calculation matrix evaluates intersections across consecutive simulation ticks, registering a single collision impact as dozens of individual hits.
* **Resolution:** Implemented a floating timestamp delta comparison filter acting as a structural software debounce mechanism. Collisions now enforce a strict cooldown interval parameter (`penaltyCooldown`), shielding the system from multi-frame duplication data.

### Bug 3: UI Layout Collapse within Auto-Driven Arrays
* **Issue:** Activating `Control Child Size` inside the layout container shrunk active button bounding areas down to zero-vectors, while forcing expand rules resulted in severe distortion.
* **Root Cause:** Automatic drivers strip elements of explicit RectTransform parameters if content size inputs are missing.
* **Resolution:** Unchecked both layout constraint properties to re-assign sizing authority back to the child components while keeping vertical stacking behaviors locked.

---

## 🚀 Future Roadmap & Enhancements
* **Dynamic NavMesh Enemy AI:** Refactor the current node-array pathing system to integrate runtime Navigation Mesh Agents, enabling advanced hazards to actively chase the player's spatial coordinates when entering detection triggers.
* **Asynchronous Scene Shifting:** Introduce background loading routines with custom radial fill progress masks to optimize mobile memory allocations during scene transitions.
*