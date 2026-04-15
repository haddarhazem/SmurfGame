# 🏗️ Architecture & Technical Documentation

Comprehensive technical overview of the Smurf Game architecture, design patterns, and system interactions.

## Table of Contents
1. [System Architecture](#system-architecture)
2. [Layer Details](#layer-details)
3. [Game Loop & Timers](#game-loop--timers)
4. [Collision System](#collision-system)
5. [Enemy AI & Spawning](#enemy-ai--spawning)
6. [Database Design](#database-design)
7. [Code Patterns & Practices](#code-patterns--practices)

---

## System Architecture

### Three-Tier Layered Architecture

```
┌─────────────────────────────────────────┐
│        PRESENTATION LAYER               │
│     (SmurfGame.WinForms)                │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ Form1.cs (Main Game Loop)         │ │
│  │ - Event coordination              │ │
│  │ - Render management               │ │
│  │ - User input handling             │ │
│  └───────────────────────────────────┘ │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ SmurfControl (Custom Control)     │ │
│  │ - Player sprite rendering         │ │
│  │ - Animation playback              │ │
│  │ - Obstacle collision detection    │ │
│  └───────────────────────────────────┘ │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ NameEntryForm, DashboardForm      │ │
│  │ - Dialog windows                  │ │
│  │ - Score display                   │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
            ↓ References ↓
┌─────────────────────────────────────────┐
│      BUSINESS LOGIC LAYER               │
│      (SmurfGame.BL)                     │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ Entities (Domain Models)          │ │
│  │ - Creature (Base class)           │ │
│  │ - Smurf (Player)                  │ │
│  │ - Azrael, HorizontalCat,          │ │
│  │   VerticalCat (Enemies)           │ │
│  │ - Coin, BluePotion, SpeedBuff     │ │
│  └───────────────────────────────────┘ │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ Controllers                       │ │
│  │ - PlayerController                │ │
│  │ - GameLogic coordination          │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
            ↓ Persists Via ↓
┌─────────────────────────────────────────┐
│      DATA ACCESS LAYER                  │
│      (SmurfGame.DAL)                    │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ SmurfGameContext (EF DbContext)   │ │
│  │ - Entity mappings                 │ │
│  │ - Database migrations             │ │
│  │ - LINQ query execution            │ │
│  │ - Transaction management          │ │
│  └───────────────────────────────────┘ │
│                                         │
│  ↓ Connected To ↓                      │
│                                         │
│  ┌───────────────────────────────────┐ │
│  │ SQL Server LocalDB                │ │
│  │ - SmurfGameDB database            │ │
│  │ - Relational tables               │ │
│  │ - Persistent player records       │ │
│  └───────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

---

## Layer Details

### Presentation Layer (SmurfGame.WinForms)

#### Form1.cs (Main Game Window)
**Responsibilities**:
- Game state management (running, paused, over)
- Event coordination between timers
- Render-loop orchestration
- Input handling and keyboard mapping
- Collision detection management
- Score and health tracking

**Key Components**:
```csharp
// Timer-based game loop
private System.Windows.Forms.Timer autoSaveTimer;      // 1000ms
private System.Windows.Forms.Timer scoreTimer;         // 1000ms
private System.Windows.Forms.Timer entityAnimationTimer; // 150ms
private System.Windows.Forms.Timer enemyMovementTimer;  // 200ms
private System.Windows.Forms.Timer buffTimer;          // 1000ms
```

#### SmurfControl.cs (Custom Control)
**Responsibilities**:
- Render player sprite with transparency
- Manage animation frames
- Handle keyboard input for player movement
- Perform pre-movement obstacle collision
- Track current player position

**Key Features**:
- Inherits from `UserControl`
- Custom `OnPaint()` override for rendering
- Implements obstacle collision detection
- Maintains animation state (idle, moving)

#### Dialog Forms
- `NameEntryForm.cs`: Player name input
- `DashboardForm.cs`: Victory screen and leaderboard display

### Business Logic Layer (SmurfGame.BL)

#### Entity Hierarchy
```csharp
public abstract class Creature
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

// Player Character
public class Smurf : Creature
{
    public int Level { get; set; }
    public bool IsInForest { get; set; }
    public int? BestTime { get; set; }  // Leaderboard record
}

// Enemies
public class Azrael : Creature
{
    public int Damage { get; set; }
    // Stationary threat
}

public class HorizontalCat : Creature
{
    public int Speed { get; set; }      // 3 units/frame
    public int Direction { get; set; }  // 1 or -1
    public int Damage { get; set; }
}

public class VerticalCat : Creature
{
    public int Speed { get; set; }      // 3 units/frame
    public int Direction { get; set; }  // 1 or -1
    public int Damage { get; set; }
}

// Items
public class Coin
{
    public int Points { get; set; }
    public bool IsConsumed { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class BluePotion
{
    public int HealthBoost { get; set; }    // 25% recovery
    public bool IsConsumed { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class SpeedBuff
{
    public int SpeedBoostAmount { get; set; }  // +5 speed
    public bool IsConsumed { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}
```

#### PlayerController.cs
Coordinates game logic and entity interactions.

### Data Access Layer (SmurfGame.DAL)

#### SmurfGameContext.cs
```csharp
public class SmurfGameContext : DbContext
{
    // Player data
    public DbSet<Smurf> Smurfs { get; set; }
    
    // Items
    public DbSet<Coin> Coins { get; set; }
    public DbSet<BluePotion> BluePotions { get; set; }
    public DbSet<SpeedBuff> SpeedBuffs { get; set; }
    
    // Enemies (TPT inheritance pattern)
    public DbSet<Azrael> Azraels { get; set; }
    public DbSet<HorizontalCat> HorizontalCats { get; set; }
    public DbSet<VerticalCat> VerticalCats { get; set; }
}
```

**Key Features**:
- **Table Per Type (TPT)**: Each entity type has its own table
- **Automatic Migrations**: Database schema auto-created on first run
- **Connection String**: `(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB`
- **Code-First Approach**: Entity classes drive database schema

---

## Game Loop & Timers

### Timer Coordination Strategy

The game uses **five coordinated timers** to manage different aspects:

```
┌─────────────────────────────────┐
│   Master Tick (Windows Pump)    │
└──────────────┬──────────────────┘
               │
         ┌─────┴─────────────────────────────────────────┐
         │                                               │
    ┌────▼────────┐                          ┌──────────▼──────┐
    │ Animation    │                          │ Enemy Movement  │
    │ Timer: 150ms │                          │ Timer: 200ms    │
    │              │                          │                 │
    │ ✓ Coins      │                          │ ✓ Cat positions │
    │ ✓ Cats idle  │                          │ ✓ Lane enforce  │
    │              │                          │ ✓ Collision     │
    └──────────────┘                          └─────────────────┘
         │                                          │
         │   ┌─────────────────────────────────────┘
         │   │
    ┌────▼───▼─────────┐
    │ Render/Paint     │
    │ Update controls  │
    │ Refresh display  │
    └──────────────────┘
         │
    ┌────┴─────────────────────────────────────────────────┐
    │                                                      │
┌──▼──────────────┐                              ┌────────▼─────┐
│ Score Timer     │                              │ Auto-Save    │
│ 1000ms          │                              │ Timer: 1000ms│
│                 │                              │              │
│ ✓ Time counter  │                              │ ✓ DB persist │
│ ✓ UI update     │                              │              │
└─────────────────┘                              └──────────────┘
     │
┌────▼────────────────────┐
│ Buff Timer (if active)  │
│ 1000ms                  │
│                         │
│ ✓ Countdown display    │
│ ✓ Speed buff duration  │
└─────────────────────────┘
```

### Timer Intervals & Purposes

| Timer | Interval | Priority | Purpose | Updates |
|-------|----------|----------|---------|---------|
| **Animation** | 150ms | HIGH | Frame rotation for sprites | EntityAnimationTimer_Tick |
| **Enemy Movement** | 200ms | HIGH | Cat position & collision | EnemyMovementTimer_Tick |
| **Score** | 1000ms | MEDIUM | Time trial clock | ScoreTimer_Tick |
| **Auto-Save** | 1000ms | MEDIUM | Database persistence | AutoSaveTimer_Tick |
| **Buff** | 1000ms | LOW | Speed buff countdown | BuffTimer_Tick |

### Game State Flags

```csharp
private bool isGameOver = false;     // Disables all input
private bool isGamePaused = false;   // Freezes movement, enemies
private int activeSpeedBoost = 0;    // Current speed boost amount
private int buffTimeRemaining = 0;   // Speed buff countdown
```

---

## Collision System

### Pre-Movement Validation Algorithm

```csharp
// Enemy Movement Pattern
private void EnemyMovementTimer_Tick(object sender, EventArgs e)
{
    // 1. Calculate potential new position
    int futureX = hCat.X + hCat.Direction * hCat.Speed;
    
    // 2. Create ghost rectangle at new position
    Rectangle ghostCat = new Rectangle(futureX, hCat.Y, 50, 50);
    bool canMove = true;
    
    // 3. Check boundaries
    if (futureX <= 10 || futureX >= this.ClientSize.Width - 80)
    {
        hCat.Direction *= -1;  // Bounce
        canMove = false;
    }
    
    // 4. Check obstacles
    if (canMove)
    {
        foreach (PictureBox wall in obstacles)
        {
            if (ghostCat.IntersectsWith(wall.Bounds))
            {
                hCat.Direction *= -1;  // Bounce
                canMove = false;
                break;
            }
        }
    }
    
    // 5. Only move if no collision
    if (canMove)
    {
        hCat.X += hCat.Direction * hCat.Speed;
    }
    
    // 6. Update UI position
    pbHCat.Left = hCat.X;
    pbHCat.Top = hCat.Y;
}
```

### Spawn Location Validation

```csharp
private Point GetSafeSpawnLocation()
{
    // Safety net: 100 attempts max
    int maxAttempts = 100;
    int currentAttempt = 0;
    
    do
    {
        currentAttempt++;
        isSafeSpot = true;
        
        randomX = rand.Next(50, this.ClientSize.Width - 50);
        randomY = rand.Next(50, this.ClientSize.Height - 50);
        
        spawnZone = new Rectangle(
            randomX - 15,    // Padding
            randomY - 15,
            25 + 30,         // Item size + padding
            25 + 30
        );
        
        // Check overlaps with:
        // - Player position
        // - Blue potion
        // - Speed buff
        // - All enemies (3 lists)
        // - All coins (list)
        // - All obstacles
        
        if (currentAttempt >= maxAttempts)
            break;
            
    } while (isSafeSpot == false);
    
    return new Point(randomX, randomY);
}
```

### Azrael Lane Avoidance

```csharp
private void SpawnAllEnemies()
{
    // Define buffer zones
    int horizontalCatLaneY = 454;
    int verticalCatLaneX = 752;
    int laneBuffer = 100;  // 100px radius
    
    Point azraelSpot = GetSafeSpawnLocation();
    
    // Try up to 50 times to find outside buffer zone
    int maxAttempts = 50;
    int attempts = 0;
    
    while ((Math.Abs(azraelSpot.X - verticalCatLaneX) < laneBuffer ||
            Math.Abs(azraelSpot.Y - horizontalCatLaneY) < laneBuffer) 
            && attempts < maxAttempts)
    {
        azraelSpot = GetSafeSpawnLocation();
        attempts++;
    }
    
    // Spawn Azrael at validated location
    Azrael azrael = new Azrael { X = azraelSpot.X, Y = azraelSpot.Y };
}
```

---

## Enemy AI & Spawning

### Enemy Behaviors

#### Azrael (Stationary Cat)
- **Spawn**: Random location with 100px buffer from cat lanes
- **Movement**: None (stationary threat)
- **Animation**: Idle frame cycling every 150ms
- **Threat**: 30 HP damage on collision
- **AI**: None (passive)

#### HorizontalCat (Bottom Lane)
- **Spawn**: Random X position, fixed Y: 454
- **Movement**: Left-right (Direction: ±1) at Speed: 3 units/frame
- **Lane Enforcement**: Y position reset to 454 every frame
- **Bouncing**: Reverses Direction on wall/boundary collision
- **Animation**: Frame cycle offset from Azrael
- **Threat**: 30 HP damage on collision

#### VerticalCat (Right Lane)
- **Spawn**: Fixed X: 752, random Y position
- **Movement**: Up-down (Direction: ±1) at Speed: 3 units/frame
- **Lane Enforcement**: X position reset to 752 every frame
- **Bouncing**: Reverses Direction on wall/boundary collision
- **Animation**: Different frame offset for visual distinction
- **Threat**: 30 HP damage on collision

### Spawning Algorithm

```csharp
private void SpawnAllEnemies()
{
    // 1. AZRAEL (Stationary)
    // - Gets safe spawn location
    // - Tries 50 times to avoid lane buffers
    // - Falls back to best attempt if crowded
    
    // 2. HORIZONTAL CAT (Bottom Lane)
    // - X: Random(50, Width - 100)
    // - Y: Fixed 454 (bottom lane)
    // - Direction: 1 (start moving right)
    
    // 3. VERTICAL CAT (Right Lane)
    // - X: Fixed 752 (right lane)
    // - Y: Random(50, Height - 100)
    // - Direction: 1 (start moving down)
    
    // Add to database and create PictureBoxes
}
```

---

## Database Design

### Entity-Relationship Diagram

```
┌──────────────────────────────────┐
│         Smurfs (Players)         │
├──────────────────────────────────┤
│ PK: Id (int)                     │
│ Name (nvarchar)                  │
│ Health (int)                     │
│ MaxHealth (int)                  │
│ Level (int)                      │
│ IsInForest (bit)                 │
│ X, Y (int coordinates)           │
│ BestTime (int, nullable)         │
└──────────────────────────────────┘

┌──────────────────────────────────┐  ┌───────────────────────┐
│         Coins (Items)            │  │   BluePotions (Items) │
├──────────────────────────────────┤  ├───────────────────────┤
│ PK: Id (int)                     │  │ PK: Id (int)          │
│ Points (int)                     │  │ HealthBoost (int: 25% │
│ X, Y (int coordinates)           │  │ X, Y (int coord)      │
│ IsConsumed (bit)                 │  │ IsConsumed (bit)      │
└──────────────────────────────────┘  └───────────────────────┘

┌──────────────────────────────────┐
│      SpeedBuffs (Items)          │
├──────────────────────────────────┤
│ PK: Id (int)                     │
│ SpeedBoostAmount (int: +5)       │
│ HealthBoost (int: 0)             │
│ X, Y (int coordinates)           │
│ IsConsumed (bit)                 │
└──────────────────────────────────┘

┌──────────────────────────────────┐
│      Creature (Base - TPT)       │
├──────────────────────────────────┤
│ PK: Id (int)                     │
│ Discriminator (nvarchar)         │
│ Name (nvarchar)                  │
│ Health (int)                     │
│ MaxHealth (int)                  │
│ X, Y (int coordinates)           │
└──────────────────────────────────┘
         ▲         ▲         ▲
         │         │         │
    ┌────┴──┐  ┌───┴────┐  ┌─┴───────┐
    │Azrael │  │HCat    │  │VCat     │
    │+ Dmg  │  │+Speed  │  │+Speed   │
    │       │  │+Dir    │  │+Dir     │
    └───────┘  └────────┘  └─────────┘
```

### Database Cleanup Strategy

On each new game, app cleans up leftover items while preserving scores:

```csharp
// DELETE ALL TEMPORARY ITEMS (NOT PLAYERS)
db.Coins.RemoveRange(db.Coins.ToList());
db.BluePotions.RemoveRange(db.BluePotions.ToList());
db.SpeedBuffs.RemoveRange(db.SpeedBuffs.ToList());
db.Azraels.RemoveRange(db.Azraels.ToList());
db.HorizontalCats.RemoveRange(db.HorizontalCats.ToList());
db.VerticalCats.RemoveRange(db.VerticalCats.ToList());

// PRESERVE: db.Smurfs (leaderboard records)
db.SaveChanges();
```

---

## Code Patterns & Practices

### Design Patterns Used

#### 1. **Entity Inheritance Pattern (TPT)**
Base `Creature` class with specific enemy types inheriting.
```csharp
public abstract class Creature { }
public class Azrael : Creature { }
public class HorizontalCat : Creature { }
public class VerticalCat : Creature { }
```

#### 2. **Repository Pattern (Implicit)**
EF Core `DbContext` acts as repository for CRUD operations.
```csharp
db.Smurfs.Add(currentSmurf);
db.SaveChanges();
```

#### 3. **Observer Pattern (Events)**
Windows Forms events coordinate game loop.
```csharp
enemyMovementTimer.Tick += EnemyMovementTimer_Tick;
autoSaveTimer.Tick += AutoSaveTimer_Tick;
```

#### 4. **State Pattern**
Game states (running, paused, over) controlled via flags.
```csharp
if (isGameOver) return;
if (isGamePaused) { /* Handle pause */ }
```

#### 5. **Strategy Pattern (Collision)**
Different collision types handled by type checking.
```csharp
if (enemy is Azrael azrael)
    damageAmount = azrael.Damage;
else if (enemy is HorizontalCat hCat)
    damageAmount = hCat.Damage;
```

### Performance Considerations

1. **Timer Efficiency**: Staggered intervals (150ms, 200ms, 1000ms)
2. **Spawn Attempt Limits**: 100 attempts max for spawn validation
3. **Collision Detection**: Pre-validation prevents unnecessary moves
4. **Database Batching**: SaveChanges() called strategically
5. **Rendering Optimization**: DoubleBuffered flag prevents flicker

### Code Quality Standards

- **Naming**: PascalCase for classes, camelCase for variables
- **Comments**: Focus on "why" not "what"
- **Null Checks**: Protection against null reference exceptions
- **Error Handling**: Try-catch in critical sections
- **Documentation**: XML comments on public methods

---

## Future Architectural Enhancements

### Planned Improvements

1. **Sound Manager**: Separate audio layer
2. **AI System**: Advanced enemy behaviors
3. **Input Manager**: Centralized input handling
4. **Event Bus**: Global event coordination
5. **Configuration Layer**: Adjustable game parameters
6. **Resource Manager**: Unified sprite/asset loading

### Potential Refactorings

1. Extract business logic from Form1 into controllers
2. Create specialized collision detector class
3. Implement object pooling for coins/potions
4. Add dependency injection for entity creation

---

**Version**: 1.0.0  
**Last Updated**: December 2024  
**Architecture Style**: Layered (3-tier) with Timers-based game loop
