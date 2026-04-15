# 🍄 Smurf Game - Forest Adventure

A fully featured real-time action game built with **C# 14.0**, **.NET 10**, and **Windows Forms**, combined with powerful **Entity Framework Core** persistence. Navigate Papa Smurf through a dangerous forest filled with cats, collect coins for high scores, and master strategic gameplay in the ultimate time trial challenge!

---

## 📸 Game Overview

![Smurf Game Gameplay](./docs/images/gameplay_overview.png "Main Gameplay - Collecting Coins and Avoiding Cats")

Experience intense real-time action as you guide Papa Smurf through obstacle-filled terrain, evade three distinct cat threats with unique movement patterns, and race against the clock to collect all 6 coins!

---

## ✨ Key Features

### 🎮 Core Gameplay
- **Real-Time Action**: Dynamically navigate a forest environment with instant keyboard responsiveness
- **6-Coin Time Trial**: Collect all coins as quickly as possible to complete the level and record your best time
- **Multi-Enemy System**: Three distinct cat threats with unique behaviors and strategic positioning
- **Obstacle Navigation**: Navigate around trees and cliffs while evading autonomous enemies
- **Persistent Leaderboard**: All player records and best times are securely stored in SQL Server

### 🐱 Three-Threat Enemy System

#### Azrael - The Stationary Big Cat
- Fixed position spawning away from mobile cat lanes
- Idle animation for atmospheric presence
- Deals 30 HP damage on contact
- Strategic threat forcing route planning

#### Horizontal Cat - Bottom Lane Patrol
- Autonomous left-right movement along Y: 454
- Movement speed: 3 units per frame
- Bounces off obstacles and map boundaries
- Forces players to time crossings carefully

#### Vertical Cat - Right Side Lane  
- Autonomous up-down movement along X: 752
- Movement speed: 3 units per frame
- Respects all obstacle boundaries
- Creates vertical barrier on right side of map

### 🛡️ Power-ups & Items
- **Blue Potion** 💙: Recovers 25% maximum health
- **Speed Buff** ⚡: Grants +5 movement speed for 10 seconds with visual countdown
- **Coins** 🪙: 6 total collectibles required for victory

### 🏥 Health & Survival System
- Start with 100 HP maximum health
- Real-time damage display when hit (-30 HP per cat collision)
- Dynamic health bar with visual percentage indicators
- Game Over when health reaches 0
- Health recovery through blue potions

---

## 🎮 How to Play

### Controls

| Key | Action |
|-----|--------|
| **Z** | Move Up |
| **S** | Move Down |
| **Q** | Move Left |
| **D** | Move Right |
| **SPACE** | Dismiss messages/dialogs |

### Gameplay Objectives

1. **Enter Your Name** 📝
   - Custom player name for the leaderboard
   - Records persist across sessions

2. **Survive the Cats** 🐱
   - Avoid Azrael's stationary position
   - Time your crossings past Horizontal Cat's bottom lane
   - Navigate around Vertical Cat's right side patrol

3. **Collect All 6 Coins** 🪙
   - Coins spawn at randomized safe locations
   - Each coin collected advances progress counter
   - Final coin completes the level

4. **Race Against Time** ⏱️
   - Timer counts up from start
   - Fastest completion recorded to database
   - Compete for leaderboard rankings

### Victory Conditions
✅ **Success**: Collect all 6 coins  
❌ **Failure**: Health depletes to 0 HP

---

## 🏗️ Technical Architecture

### Multi-Layered Architecture

```
┌─────────────────────────────────────────────────┐
│    SmurfGame.WinForms (Presentation Layer)      │
│  ┌─────────────────────────────────────────┐   │
│  │ Form1.cs - Main Game Orchestrator       │   │
│  │ • Enemy spawning & autonomous movement  │   │
│  │ • Unified collision detection system    │   │
│  │ • Animation rendering (150ms timer)     │   │
│  │ • Game state & pause management         │   │
│  └─────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────┐   │
│  │ SmurfControl - Player Movement Engine   │   │
│  │ • Keyboard input mapping (Z/S/Q/D)      │   │
│  │ • Obstacle collision detection          │   │
│  │ • Real-time position tracking           │   │
│  └─────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
              ↓ Depends On ↓
┌─────────────────────────────────────────────────┐
│   SmurfGame.BL (Business Logic Layer)           │
│  ┌─────────────────────────────────────────┐   │
│  │ Entity Classes (Data Models)            │   │
│  │ • Creature (Base class for all units)   │   │
│  │ • Smurf (Player character)              │   │
│  │ • Azrael (Stationary cat)               │   │
│  │ • HorizontalCat (Lane-based patrol)     │   │
│  │ • VerticalCat (Autonomous movement)     │   │
│  │ • Coin, BluePotion, SpeedBuff           │   │
│  └─────────────────────────────────────────┘   │
│  ┌─────────────────────────────────────────┐   │
│  │ Controllers                             │   │
│  │ • PlayerController (Game logic hub)     │   │
│  └─────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
              ↓ Persists Via ↓
┌─────────────────────────────────────────────────┐
│   SmurfGame.DAL (Data Access Layer)             │
│  ┌─────────────────────────────────────────┐   │
│  │ SmurfGameContext (EF Core DbContext)    │   │
│  │ • TPT (Table Per Type) inheritance      │   │
│  │ • SQL Server LocalDB connection         │   │
│  │ • Automatic schema migrations           │   │
│  │ • LINQ query support                    │   │
│  └─────────────────────────────────────────┘   │
└─────────────────────────────────────────────────┘
```

### Collision Detection System

**Pre-Movement Validation Algorithm**:
1. Create Ghost Rectangle at potential new position
2. Validate against:
   - Map boundaries (edge detection)
   - Obstacle list (trees, cliffs)
   - Enemy positions (avoided during spawning)
3. Only execute movement if no collision detected
4. Bounce off walls by reversing direction vector

---

## 🛠️ Technical Stack

| Layer | Component | Version | Purpose |
|-------|-----------|---------|---------|
| **Language** | C# | 14.0 | Modern language features, pattern matching |
| **Runtime** | .NET | 10 | Latest .NET platform, performance |
| **UI Framework** | Windows Forms | Built-in | Cross-platform forms and controls |
| **ORM** | Entity Framework Core | 8.x | Database abstraction, LINQ support |
| **Database** | SQL Server LocalDB | Latest | Local persistence, relational data |
| **IDE** | Visual Studio Community | 2026 | Full-featured development environment |

---

## 📦 Installation & Quick Start

### System Requirements
- **OS**: Windows 10 or later
- **.NET SDK**: Version 10.0 or higher
- **SQL Server**: LocalDB Express installed
- **RAM**: 4GB minimum
- **Storage**: 500MB available

### Installation Steps

#### Step 1: Clone Repository
```bash
git clone https://github.com/haddarhazem/SmurfGame.git
cd SmurfGame
```

#### Step 2: Install .NET 10 SDK
```bash
# Windows - Download from official site or use Chocolatey
choco install dotnet-sdk-10.0

# Or visit: https://dotnet.microsoft.com/download/dotnet/10.0
```

#### Step 3: Install SQL Server LocalDB
```bash
# Windows - Download SQL Server Express LocalDB
# https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
```

#### Step 4: Build & Run

**Option A - Using Visual Studio**
```
1. Open SmurfGame.sln
2. Press Ctrl+Shift+B to build
3. Press F5 to run
```

**Option B - Command Line**
```bash
# Restore NuGet packages
dotnet restore

# Build solution
dotnet build

# Run WinForms app
cd SmurfGame.WinForms
dotnet run
```

#### Step 5: First Launch
- Database automatically creates on first run
- No manual migration commands needed
- Enter player name when prompted
- Start collecting coins!

---

## 🤝 Contributing Guidelines

We welcome contributions from the community! Here's how to contribute:

### Development Workflow
1. **Fork** the repository on GitHub
2. **Clone** your fork locally
3. **Create** a feature branch: `git checkout -b feature/YourFeatureName`
4. **Commit** with descriptive messages: `git commit -m 'Add enemy AI improvements'`
5. **Push** to your fork: `git push origin feature/YourFeatureName`
6. **Open** a Pull Request with detailed description

### Code Standards
- Follow **Microsoft C# Naming Conventions** (PascalCase for classes, camelCase for variables)
- Add **XML documentation comments** for all public methods
- Ensure code **compiles with 0 errors and warnings**
- Test collision detection thoroughly before submitting

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for complete details.

---

## 👤 Author & Credits

**Developer**: Hazem Haddar  
**GitHub**: [@haddarhazem](https://github.com/haddarhazem)  
**Repository**: [SmurfGame](https://github.com/haddarhazem/SmurfGame)

### Acknowledgments
- **Microsoft**: .NET Framework, Entity Framework Core, Visual Studio
- **The Smurfs**: Original characters and intellectual property inspiration

---

## 📞 Support & Contact

### Getting Help
- **Report Bugs**: [GitHub Issues](https://github.com/haddarhazem/SmurfGame/issues)
- **Feature Requests**: [GitHub Discussions](https://github.com/haddarhazem/SmurfGame/discussions)

---

**Game Version**: 1.0.0  
**Last Updated**: December 2024  
**Status**: ✅ Stable & Playable

**Ready to Play? Start Your Adventure! 🍄🎮**

