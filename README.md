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
   git clone [https://github.com/haddarhazem/SmurfGame](https://github.com/haddarhazem/SmurfGame)

Open the solution (.sln) in Visual Studio.

Open the Package Manager Console (Tools -> NuGet Package Manager -> Package Manager Console).

Ensure the Default Project is set to SmurfGame.DAL.

Run the following command to build the database:

PowerShell
Update-Database -Project SmurfGame.DAL -StartupProject SmurfGame.WinForms
Press F5 or click Start in Visual Studio to play!

🕹️ Controls
Arrow Keys: Move Papa Smurf up, down, left, and right.

Objective: Enter your name, navigate the map, and touch all 6 floating bubbles to stop the timer.

📸 Screenshots
(Replace these placeholders with actual images of your game)

Entering a custom player name before the game begins.

Rescuing trapped Smurfs while avoiding Azrael.

The interactive database-backed Leaderboard.

💡 Lessons Learned
This project was a deep dive into bypassing the standard limitations of Windows Forms. Key achievements include creating a custom visual render loop using Timers, handling transparent images over complex backgrounds, and seamlessly integrating an Entity Framework SQL database without interrupting the real-time gameplay.


***

### Quelques conseils pour GitHub :
1. N'oublie pas de remplacer `YourUsername` dans le lien de clonage par ton vrai pseudo GitHub.
2. Pour ajouter des images, le plus simple est de prendre des captures d'écran de ton jeu en marche, de les glisser-déposer directement dans la zone de texte quand tu édites le README sur le site web de GitHub. GitHub créera les liens automatiquement !
3. **Important :** Assure-toi d'avoir un fichier `.gitignore` spécifique à Visual Studio dans ton projet (pour éviter d'envoyer les gros dossiers `/bin/` et `/obj/` sur GitHub).

Félicitations encore pour avoir mené ce projet jusqu'au bout ! C'est une excellente pièce pour ton port
