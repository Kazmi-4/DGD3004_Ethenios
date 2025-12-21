# Ethenios: Action-Adventure

**Ethenios** is a high-stakes 2D Action-Adventure platformer built with Unity. Navigate through atmospheric landscapes, master precision movement, and overcome patrolling sentinels in a world that resets its challenges the moment you fall.

---

## 🛠 Project Overview
* **Genre:** 2D Action-Adventure Platformer
* **Platform:** PC (Windows) / WebGL
* **Engine:** Unity 2022.3+ (Universal Render Pipeline)

---

## 👥 Team & Level Design
| Member | Role | Level Design |
| :--- | :--- | :--- |
| **Qasim Kazmi** | Lead Developer | **Whispering Wilds** (Level 1): A vibrant, introductory forest setting focusing on core platforming and basic enemy encounters. |
| **Mustafa Ozan Uslu** | Technical Designer | **Midnight Thicket** (Level 2): A high-difficulty nocturnal expansion featuring restricted visibility and aggressive patrol patterns. |

---

## 🎯 Game Goal
In the world of Ethenios, the player must traverse dangerous environments to collect scattered **Essence Tokens** and reach the level exit. The game features a "Perma-State" reset mechanic: if the player dies, the world instantly revives. All enemies return to their posts and tokens reappear, requiring the player to master each level in a single, perfect run.

---

## 🕹 Core Mechanics
* **Precision Platforming:** Physics-based movement including variable jump heights and air control.
* **Sentinel Patrols:** Enemies follow complex AI patrol paths using a modular `PatrolPath` system.
* **Instant Revive System:** Custom simulation events that handle the global reset of all scene objects (Enemies, Tokens, and Player) upon death.
* **Animator Override System:** Technical implementation of `AnimatorOverrideControllers` to allow diverse enemy visuals while maintaining optimized, shared logic.
* **Directional Orientation:** Sprite-flipping logic that reacts to movement velocity.

---

## ⌨ Controls
| Action | Input |
| :--- | :--- |
| **Move Left / Right** | `A` / `D` or `Left` / `Right Arrow` |
| **Jump** | `Spacebar` |
| **Stomp Attack** | `Jump on Enemy` |

---

## 🚀 Technical Implementation Details
This project utilizes a **Simulation Event System** to decouple gameplay logic from physics. Key scripts include:
* `EnemyController.cs`: Handles AI movement, orientation, and the `ResetEnemy()` state.
* `PlayerDeath.cs`: Manages the global reset sequence and object pooling/reactivation.
* `EnemyDeath.cs`: Triggers frame-perfect animation states to ensure responsive combat feedback.

---

## 🎯 Target Audience
Designed for enthusiasts of precision platformers (like *Celeste* or *Hollow Knight*) who enjoy atmospheric world-building and challenging "learn-by-dying" gameplay loops.

---
© 2025 Ethenios Development Team
