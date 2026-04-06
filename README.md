# 🍄 Smurf Village: Time Trial

A fully featured 2D Time Trial game built entirely with **C# Windows Forms** and **Entity Framework Core**. 

Navigate Papa Smurf through the village, avoid obstacles, and rescue all the trapped Smurfs as fast as possible to secure your spot on the global database-backed Leaderboard!

## 🎮 Gameplay Features
* **Time Trial Mechanics:** Collect all 6 trapped Smurf bubbles as quickly as possible.
* **Animated Sprites:** Custom frame-by-frame animations for player movement, idle breathing, and floating bubble targets.
* **Collision Detection:** Solid boundaries for trees and map edges, plus dynamic hitboxes for item collection.
* **Persistent Leaderboard:** Player names and best completion times are securely saved to a SQL database.
* **Dynamic Spawning:** Items spawn in randomized, valid map locations every time a new game starts.
* **Session Cleanup:** Automatically purges ghost items from previous crashed sessions while protecting the high-score leaderboard.

## 🛠️ Technical Stack
* **Language:** C# 
* **Framework:** .NET Windows Forms (WinForms)
* **Database:** Microsoft SQL Server
* **ORM:** Entity Framework Core (Code-First Approach)
* **Architecture:** Event-driven architecture with custom game loops (Timers) and Object-Oriented inheritance (`Creature` -> `Smurf`).

## 🚀 How to Run Locally

Because this game uses Entity Framework Core to save scores, you will need to set up the local database before playing.

### Prerequisites
* Visual Studio 2022 (or newer)
* .NET SDK
* SQL Server Express (LocalDB)

### Setup Instructions
1. **Clone the repository:**
   ```bash
   git clone [https://github.com/YourUsername/SmurfGame.git](https://github.com/YourUsername/SmurfGame.git)
