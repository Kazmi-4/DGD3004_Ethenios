

# 🌌 DGD | Ethenios: The Lost Rift

**Ethenios** is a high-stakes 2D Action-Adventure platformer. Navigate through atmospheric landscapes, master precision movement, and overcome the rocky sentinels of a world that resets its challenges the moment you fall.

---

## 📖 The Narrative: A World Unwelcoming
You are the **Outsider**. After a reality-bending accident, you have slipped through a fissure into *Ethenios*—a lush but hostile dimension where nature and stone possess a shared consciousness. You do not belong here.

The world itself rejects your presence. To escape, you must traverse the Whispering Wilds and the Midnight Thicket to find the dimensional rift home. But the rift requires energy: **Aether Shards**. These glowing, diamond-like rocks are the crystallized life force of this world, and taking them enrages the guardians.

---

## 🛠 Project Overview
* **Genre:** 2D Action-Adventure / Precision Platformer
* **Platform:** PC (Windows) / WebGL
* **Engine:** Unity 2022.3+ (Universal Render Pipeline)
* **Visual Style:** Atmospheric Pixel Art with Dynamic Particle Systems

## 🏗️ Game Design Architecture

The following diagram illustrates the **Systems Architecture** of Ethenios. It visualizes how the narrative context wraps around the core gameplay loop and highlights the project's unique technical foundation: a custom **Simulation Event System**.

### System Breakdown
* **The Simulation Layer:** Unlike standard Unity scenes that reload on death, Ethenios uses a deterministic `Simulation Controller` and `HeapQueue`. This allows for an **Instant Reset Event**, rewinding enemies and tokens to their exact start states without loading screens.
* **The Core Loop:** The `GameController` manages the frame-by-frame interactions between the Player (using custom `KinematicObject` physics) and the Stone Legion enemies (driven by modular `PatrolPath` logic).
* **Feedback Systems:** Player actions immediately feed into the **Economy System**, updating the HUD and UI Controllers to track the collection of Aether Shards in real-time.

<img width="507" height="395" alt="image" src="https://github.com/user-attachments/assets/ce7b01c7-79bf-45ef-8e43-302a25d35243" />

<img width="7828" height="9327" alt="Mermaid Chart - Create complex, visual diagrams with text -2026-01-01-192444" src="https://github.com/user-attachments/assets/760d94a4-6367-4d9a-9db9-a817109536df" />


---

## 🌟 Key Features

### 💎 The Economy of Light (XP & Tokens)
Survival isn't just about reaching the end; it's about gathering power.
* **Aether Shards (Tokens):** Scattered across the map are glowing, diamond-like rocks. Collecting these fuels your **Experience HUD**, directly tracking your progress toward stabilizing the exit portal.
* **Perma-State Reset:** If you fall, the world rewinds. All Shards respawn, and your collected XP for that run resets. Mastery is required.

### 🗿 The Stone Legion (Enemy Variations)
The forest is protected by ancient rock-constructs. While they share a similar rugged, rocky appearance, their behaviors differ significantly:
* **Moss-Crag Sentinels (Patrollers):** Mobile golems that tirelessly walk fixed paths along the terrain.
* **Rooted Guardians (Static):** Stationary blockers that force you to find vertical routes around them.
* **Spore-Wardens (Shooters):** Stationary turrets that fire projectiles, demanding precise timing to dodge.

### 🏃 Precision Traversal
* **Kinetic Platforms:** Moving terrain that requires players to maintain momentum while the ground shifts beneath them.
* **Variable Jump Heights:** Control your ascent with frame-perfect input.
* **Stomp Combat:** Turn gravity into a weapon by landing crushing blows on enemy heads.

---

## 🖥️ User Interface & Menus

The game features a polished, diegetic UI system handled by a robust Controller.

### 🎬 Main Menu
A visually immersive start screen featuring a **live particle simulation** of the Ethenios environment, setting the mood before you even press start.
* **Options:** Play Game / Quit Application.

### ⏸️ Pause Menu
Accessible anytime via `Esc`, giving you control over the simulation.
* **Master Volume Dial:** Fine-tune the audio levels.
* **Mute Toggle:** Instantly silence sound effects.
* **Navigation:** Resume or Quit to Desktop.


---

## 🗺️ Worlds & Levels

| Level | Environment | Design Focus |
| :--- | :--- | :--- |
| **Whispering Wilds** | **Daylight Forest** | A vibrant introductory zone. Focuses on horizontal flow, introducing the **Moss-Crag Sentinels** and basic platforming over Kinetic Platforms. |
| **Midnight Thicket** | **Nocturnal Danger** | A dark, oppressive expansion. Visibility is reduced by heavy rain. Introduces **Spore-Wardens** and complex vertical climbs. |

---

## ⌨ Controls

| Action | Input |
| :--- | :--- |
| **Move Left / Right** | `A` / `D` or `Left` / `Right Arrow` |
| **Jump** | `Spacebar` (Hold for higher jump) |
| **Stomp Attack** | Land on Enemy Head |
| **Pause Game** | `Esc` |

---

## 👥 The Team

| Member | Role | Contribution |
| :--- | :--- | :--- |
| **Qasim Kazmi** | Lead Developer | Level Design (Whispering Wilds), Core Physics, Enemy AI, Lighting/Atmosphere, UI Systems |
| **Mustafa Ozan Uslu** | Technical Designer | Level Design (Midnight Thicket), UI Design, Mechanics improvement, Assets Sprite Designs | 

---
© 2025 Ethenios Development Team
