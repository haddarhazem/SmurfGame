# 🚀 Quick Reference Guide

Fast lookup reference for Smurf Game developers and contributors.

## Contents
- [File Locations](#file-locations)
- [Key Constants](#key-constants)
- [Important Methods](#important-methods)
- [Database Operations](#database-operations)
- [Timer Intervals](#timer-intervals)
- [Keyboard Controls](#keyboard-controls)
- [Common Tasks](#common-tasks)

---

## File Locations

### Main Game Files
| File | Purpose | Lines |
|------|---------|-------|
| `SmurfGame.WinForms/Form1.cs` | Main game logic, game loop | ~1700 |
| `SmurfGame.WinForms/SmurfControl.cs` | Player sprite & movement | ~400 |
| `SmurfGame.DAL/SmurfGameContext.cs` | Database context | ~50 |
| `SmurfGame.BL/Entities/` | Entity definitions | ~200 |

### Entity Files
- `Creature.cs` - Base class for all creatures
- `Smurf.cs` - Player character
- `Azrael.cs` - Stationary cat enemy
- `HorizontalCat.cs` - Bottom lane cat (Y:454)
- `VerticalCat.cs` - Right lane cat (X:752)
- `Coin.cs` - Collectible item
- `BluePotion.cs` - Health recovery item
- `SpeedBuff.cs` - Speed boost item

### Documentation Files
- `README.md` - Main project documentation
- `docs/ARCHITECTURE.md` - Technical deep-dive
- `docs/IMAGES_GUIDE.md` - Screenshot guidelines
- `docs/DOCUMENTATION_SUMMARY.md` - Changes summary

---

## Key Constants

### Game Dimensions
```csharp
// Form size (approximate)
ClientSize.Width = 800
ClientSize.Height = 600

// Player size
PLAYER_WIDTH = 30
PLAYER_HEIGHT = 30

// Enemy sizes
AZRAEL_WIDTH = 60
AZRAEL_HEIGHT = 60
CAT_WIDTH = 50
CAT_HEIGHT = 50

// Item size
ITEM_WIDTH = 30
ITEM_HEIGHT = 30
```

### Lane Positions
```csharp
// Fixed cat lanes
HORIZONTAL_CAT_LANE_Y = 454      // Bottom lane
VERTICAL_CAT_LANE_X = 752        // Right lane

// Lane buffer for Azrael spawning
AZRAEL_LANE_BUFFER = 100         // 100px radius
```

### Health & Damage
```csharp
PLAYER_MAX_HEALTH = 100
CAT_DAMAGE = 30                   // Each enemy type
BLUE_POTION_BOOST = 25            // 25% recovery
SPEED_BUFF_AMOUNT = 5             // +5 speed
SPEED_BUFF_DURATION = 10          // 10 seconds
```

### Speeds
```csharp
PLAYER_BASE_SPEED = 5             // units per frame
HORIZONTAL_CAT_SPEED = 3          // units per frame
VERTICAL_CAT_SPEED = 3            // units per frame
```

### Items Count
```csharp
TOTAL_COINS = 6                   // Coins needed to win
BLUE_POTIONS_SPAWNED = 1          // Active at once
SPEED_BUFFS_SPAWNED = 1           // Active at once
ENEMIES_TOTAL = 3                 // Azrael + 2 cats
```

---

## Important Methods

### In Form1.cs

#### Core Game Loop
```csharp
// Called on form load - initializes everything
Form1_Load(object sender, EventArgs e)

// Main key handler - processes movement
Form1_KeyDown(object sender, KeyEventArgs e)

// Collision detection - checks all interactions
CheckCollisions()
```

#### Spawning
```csharp
// Validates safe spawn location
Point GetSafeSpawnLocation()

// Spawns all 3 enemies
void SpawnAllEnemies()

// Spawns 6 coins
void SpawnAllCoins()

// Spawns health potion
void SpawnBluePotion()

// Spawns speed buff
void SpawnSpeedBuff()
```

#### Timers
```csharp
// Animation updates (150ms)
void EntityAnimationTimer_Tick(object sender, EventArgs e)

// Enemy movement (200ms)
void EnemyMovementTimer_Tick(object sender, EventArgs e)

// Score timer (1000ms)
void ScoreTimer_Tick(object sender, EventArgs e)

// Auto-save (1000ms)
void AutoSaveTimer_Tick(object sender, EventArgs e)

// Speed buff countdown (1000ms)
void BuffTimer_Tick(object sender, EventArgs e)
```

#### UI Updates
```csharp
// Updates health bar visual
void UpdateHealthBar()

// Gets correct health % image
Image GetClosestHealthImage(double percentage)

// Handles enemy collision damage
void HandleEnemyCollision(Creature enemy, int enemyIndex)
```

---

## Database Operations

### Connection String
```csharp
// SQL Server LocalDB connection
@"server=(LocalDB)\MSSQLLocalDB;
  Initial Catalog=SmurfGameDB;
  Integrated Security=true"
```

### Context Initialization
```csharp
var optionsBuilder = new DbContextOptionsBuilder<SmurfGameContext>();
optionsBuilder.UseSqlServer(connectionString);
db = new SmurfGameContext(optionsBuilder.Options);

// Auto-create database if needed
db.Database.EnsureCreated();
```

### Common CRUD Operations
```csharp
// Create
db.Smurfs.Add(newSmurf);
db.SaveChanges();

// Read
var player = db.Smurfs.FirstOrDefault(s => s.Id == playerId);

// Update
player.Health = 90;
db.SaveChanges();

// Delete
db.Coins.RemoveRange(db.Coins.ToList());
db.SaveChanges();
```

### Database Cleanup (New Game)
```csharp
// Clear temporary items (NOT player records)
db.Coins.RemoveRange(db.Coins.ToList());
db.BluePotions.RemoveRange(db.BluePotions.ToList());
db.SpeedBuffs.RemoveRange(db.SpeedBuffs.ToList());
db.Azraels.RemoveRange(db.Azraels.ToList());
db.HorizontalCats.RemoveRange(db.HorizontalCats.ToList());
db.VerticalCats.RemoveRange(db.VerticalCats.ToList());
db.SaveChanges();
```

---

## Timer Intervals

| Timer | Interval | Function | Purpose |
|-------|----------|----------|---------|
| Animation | 150ms | EntityAnimationTimer_Tick | Coin & cat frame rotation |
| Movement | 200ms | EnemyMovementTimer_Tick | Enemy position updates |
| Score | 1000ms | ScoreTimer_Tick | Time trial clock |
| Auto-Save | 1000ms | AutoSaveTimer_Tick | DB persistence |
| Buff | 1000ms | BuffTimer_Tick | Speed buff countdown |

### Starting Timers
```csharp
// All timers must be started explicitly
autoSaveTimer.Start();
scoreTimer.Start();
entityAnimationTimer.Start();
enemyMovementTimer.Start();
// buffTimer starts when speed buff collected
```

### Stopping Timers
```csharp
// Stop on game over
scoreTimer.Stop();

// Stop on buff expiration
buffTimer.Stop();

// Stop on form closing
autoSaveTimer.Stop();
entityAnimationTimer.Stop();
```

---

## Keyboard Controls

| Key | Action | Method |
|-----|--------|--------|
| **Z** | Move Up | smurfControl.MoveUp() |
| **S** | Move Down | smurfControl.MoveDown() |
| **Q** | Move Left | smurfControl.MoveLeft() |
| **D** | Move Right | smurfControl.MoveRight() |
| **SPACE** | Dismiss Dialog | isGamePaused = false |

### Control Processing
```csharp
// In Form1_KeyDown
switch (e.KeyCode)
{
    case Keys.Z: smurfControl.MoveUp(); break;
    case Keys.S: smurfControl.MoveDown(); break;
    case Keys.Q: smurfControl.MoveLeft(); break;
    case Keys.D: smurfControl.MoveRight(); break;
}

// Update position in memory
currentSmurf.X = smurfControl.GetPosition().X;
currentSmurf.Y = smurfControl.GetPosition().Y;

// Check for collisions
CheckCollisions();
```

---

## Common Tasks

### Add New Enemy Type

1. Create new entity class in `SmurfGame.BL/Entities/`:
   ```csharp
   public class NewEnemy : Creature
   {
       public int Speed { get; set; }
       public int Direction { get; set; }
   }
   ```

2. Add DbSet in `SmurfGameContext.cs`:
   ```csharp
   public DbSet<NewEnemy> NewEnemies { get; set; }
   ```

3. Create movement logic in `Form1.cs`
4. Add to `SpawnAllEnemies()` method
5. Add collision detection in `CheckCollisions()`
6. Add to enemy list: `pbEnemies.Add(newEnemyPictureBox)`

### Change Enemy Behavior

**HorizontalCat** (Bottom Lane Y:454):
- Edit `SpawnAllEnemies()` - Random X, fixed Y:454
- Edit `EnemyMovementTimer_Tick()` - Enforce Y:454 every frame
- Edit direction reversal logic for bouncing

**VerticalCat** (Right Lane X:752):
- Edit `SpawnAllEnemies()` - Fixed X:752, random Y
- Edit `EnemyMovementTimer_Tick()` - Enforce X:752 every frame
- Edit collision rectangle dimensions

**Azrael** (Stationary):
- Edit spawn buffer zone in `SpawnAllEnemies()`
- Change spawn attempt limit (currently 50)
- Modify lane buffer distance (currently 100px)

### Adjust Game Difficulty

```csharp
// Harder:
- Increase CAT_DAMAGE (30 → 40)
- Decrease HORIZONTAL_CAT_SPEED (3 → 4)
- Decrease VERTICAL_CAT_SPEED (3 → 4)
- Decrease spawn attempt limit (50 → 30)
- Reduce PLAYER_BASE_SPEED (5 → 3)

// Easier:
- Decrease CAT_DAMAGE (30 → 20)
- Increase PLAYER_BASE_SPEED (5 → 7)
- Increase spawn attempt limit (50 → 100)
- Increase BLUE_POTION_BOOST (25% → 50%)
- Increase SPEED_BUFF_DURATION (10 → 15)
```

### Debug Collision Issues

```csharp
// Add debug visualization in Paint event
protected override void OnPaint(PaintEventArgs e)
{
    // Draw collision zones
    e.Graphics.DrawRectangle(Pens.Red, GetBounds());
    
    // Check for intersections
    foreach (PictureBox obstacle in obstacles)
    {
        if (GetBounds().IntersectsWith(obstacle.Bounds))
        {
            System.Diagnostics.Debug.WriteLine("COLLISION!");
        }
    }
}
```

### Save Custom Game State

```csharp
// Current approach: Auto-save every 1 second
autoSaveTimer.Interval = 1000;

// To change:
autoSaveTimer.Interval = 500;  // Save every 500ms (more frequent)

// Or manual save on key events
if (e.KeyCode == Keys.Z)
{
    db.SaveChanges();  // Immediate save
    smurfControl.MoveUp();
}
```

---

## Build & Run Commands

### Visual Studio
```bash
# Build (Ctrl+Shift+B)
Build > Build Solution

# Run (F5)
Debug > Start Debugging

# Clean Build
Build > Clean Solution
Build > Build Solution
```

### Command Line
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run specific project
cd SmurfGame.WinForms
dotnet run

# Clean
dotnet clean
```

---

## Testing Checklist

- [ ] Player movement (Z/S/Q/D keys)
- [ ] Collision detection with obstacles
- [ ] Coin collection (all 6)
- [ ] Enemy spawning (3 types)
- [ ] Health bar updates
- [ ] Blue potion collection
- [ ] Speed buff activation
- [ ] Database saving (coordinates, score)
- [ ] Game over on 0 HP
- [ ] Victory on 6 coins collected

---

## Performance Tips

1. **Reduce spawn attempt limit** for crowded maps
2. **Increase timer intervals** for slower computers
3. **Use DoubleBuffered** for smoother rendering
4. **Cache obstacle list** instead of recalculating
5. **Limit active enemies** to 3 maximum
6. **Batch database saves** (1 save per second)

---

## Useful Debug Output Locations

All debug info goes to **Debug Console** (Ctrl+Alt+O in Visual Studio):

```csharp
System.Diagnostics.Debug.WriteLine("? Name entry complete: " + chosenName);
System.Diagnostics.Debug.WriteLine("? Database context created");
System.Diagnostics.Debug.WriteLine("? FORM LOAD COMPLETE - GAME READY ???");
System.Diagnostics.Debug.WriteLine("? Smurf created with ID: " + currentSmurf.Id);
```

---

## Resources

- **.NET 10 Docs**: https://learn.microsoft.com/en-us/dotnet/
- **Entity Framework Core**: https://learn.microsoft.com/en-us/ef/core/
- **Windows Forms**: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/
- **SQL Server LocalDB**: https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb
- **GitHub Repository**: https://github.com/haddarhazem/SmurfGame

---

**Last Updated**: December 2024  
**Version**: 1.0.0  
**Quick Reference Status**: ✅ Complete & Ready

