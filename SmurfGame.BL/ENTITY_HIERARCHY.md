# SmurfGame Entity Hierarchy - Complete Refactor

## Overview
The SmurfGame has been refactored with a clean, scalable inheritance hierarchy for game entities and buffs.

---

## Entity Class Hierarchy

### Creature (Abstract Base Class)
**File:** `SmurfGame.BL/Entities/Creature.cs`
- **Purpose:** Abstract base for all moving game entities (Smurfs and Villains)
- **Key Properties:**
  - `int X, Y` - Position coordinates
  - `int Speed` - Movement speed
  - `int Health, MaxHealth` - Health management
  - `int Size` - Hitbox size (default 14)
  - `protected MazeGenerator Maze` - Reference to maze for collision detection
- **Key Methods:**
  - `abstract void Move()` - Movement logic (implemented by subclasses)
  - `protected void MoveWithCollision(int dx, int dy)` - Movement with wall collision
  - `void TakeDamage(int damage)` - Reduce health
  - `void Heal(int amount)` - Restore health
  - `bool IsTouching(Creature other)` - Collision detection with other creatures

---

## Smurf Hierarchy

### Smurf (Abstract Class)
**File:** `SmurfGame.BL/Entities/Smurf.cs`
- **Inherits from:** `Creature`
- **Purpose:** Abstract base for all playable Smurf characters
- **Additional Properties:**
  - `int Id` - Database identifier (EF Core)
  - `int Level` - Smurf level (default 1)
  - `bool IsInForest` - Game state flag

---

#### PapaSmurf (Concrete Class)
**File:** `SmurfGame.BL/Entities/PapaSmurf.cs`
- **Inherits from:** `Smurf`
- **Purpose:** The wise leader with the highest health
- **Stats:**
  - Speed: 4
  - MaxHealth: 150 (highest of all smurfs)
- **Features:**
  - Player-controlled with `SetMovementInput(int dx, int dy)`
  - `Move()` implementation based on input

---

#### StrongSmurf (Concrete Class)
**File:** `SmurfGame.BL/Entities/StrongSmurf.cs`
- **Inherits from:** `Smurf`
- **Purpose:** The strongest smurf with highest speed and counter-damage ability
- **Stats:**
  - Speed: 6 (fastest of all smurfs)
  - MaxHealth: 100
  - CounterDamage: 20 (can deal damage back to Gargamel on collision)
- **Features:**
  - Player-controlled with `SetMovementInput(int dx, int dy)`
  - `Move()` implementation based on input

---

#### LadySmurf (Concrete Class)
**File:** `SmurfGame.BL/Entities/LadySmurf.cs`
- **Inherits from:** `Smurf`
- **Purpose:** Special character with doubled buff effects
- **Stats:**
  - Speed: 5
  - MaxHealth: 120
  - BuffMultiplier: 2.0 (buffs are 2x more effective for her)
- **Features:**
  - Player-controlled with `SetMovementInput(int dx, int dy)`
  - `Move()` implementation based on input
  - Buffs applied to her are automatically doubled

---

## Villain Hierarchy

### Villain (Abstract Class)
**File:** `SmurfGame.BL/Entities/Villain.cs`
- **Inherits from:** `Creature`
- **Purpose:** Abstract base for all enemy entities
- **Additional Properties:**
  - `int Damage` - Damage dealt on collision
- **Key Methods:**
  - `abstract void ChasePlayer(int targetX, int targetY)` - Chase logic

---

#### Gargamel (Concrete Class)
**File:** `SmurfGame.BL/Entities/Gargamel.cs`
- **Inherits from:** `Villain`
- **Purpose:** Main antagonist who chases the player
- **Stats:**
  - Speed: 3
  - MaxHealth: 100
  - Damage: 30
- **Features:**
  - Simple AI: moves one step closer to player's coordinates
  - `ChasePlayer(int targetX, int targetY)` - Pursues the player
  - Uses collision-aware movement via `MoveWithCollision()`

---

## Buff Hierarchy

### Buff (Abstract Base Class)
**File:** `SmurfGame.BL/Entities/Buff.cs`
- **Purpose:** Abstract base for all power-ups/buffs
- **Note:** Does NOT inherit from Creature (separate hierarchy)
- **Key Properties:**
  - `int X, Y` - Position coordinates
  - `int Duration` - Effect duration (in ticks)
  - `int Size` - Collision size (default 12)
- **Key Methods:**
  - `abstract void ApplyTo(Creature target)` - Apply buff effect
  - `bool IsTouching(Creature creature)` - Collision detection

---

#### RedBuff (Concrete Class)
**File:** `SmurfGame.BL/Entities/RedBuff.cs`
- **Inherits from:** `Buff`
- **Purpose:** Health restoration mushroom
- **Effect:**
  - Restores 30 health
  - Restores 60 health if applied to LadySmurf (2x multiplier)
- **Spawn:** Random positions, every few seconds

---

#### YellowBuff (Concrete Class)
**File:** `SmurfGame.BL/Entities/YellowBuff.cs`
- **Inherits from:** `Buff`
- **Purpose:** Speed boost mushroom
- **Effect:**
  - Increases Speed by 4
  - Increases Speed by 8 if applied to LadySmurf (2x multiplier)
- **Spawn:** Random positions, every few seconds

---

## Class Diagram

```
                    ┌─────────────────────────┐
                    │    Creature (abstract)   │
                    │  Position, Speed, Health │
                    │   + abstract Move()      │
                    └────────────┬─────────────┘
                                 │
                    ┌────────────┴──────────────┐
                    │                           │
         ┌──────────▼──────────┐    ┌──────────▼──────────┐
         │ Smurf (abstract)    │    │ Villain (abstract)  │
         │ +Id, Level, etc.    │    │ +Damage             │
         └──────────┬──────────┘    └──────────┬──────────┘
                    │                          │
         ┌──────────┼──────────┐               │
         │          │          │               │
    ┌────▼───┐ ┌────▼────┐ ┌──▼────────┐     │
    │ Papa   │ │ Strong  │ │ Lady      │     │
    │ Smurf  │ │ Smurf   │ │ Smurf     │     │
    └────────┘ └─────────┘ └───────────┘     │
                                            ┌─▼────────┐
                                            │Gargamel  │
                                            └───────────┘


                    ┌──────────────────┐
                    │ Buff (abstract)   │
                    │ Position, Duration│
                    │ abstract ApplyTo()│
                    └────────┬──────────┘
                             │
                    ┌────────┴──────────┐
                    │                   │
            ┌───────▼──────┐    ┌───────▼──────┐
            │  RedBuff     │    │ YellowBuff   │
            │ Restores HP  │    │ Boosts Speed │
            └──────────────┘    └──────────────┘
```

---

## Key Design Decisions

1. **Clean Separation:** Creatures (moving entities) and Buffs are in separate hierarchies
2. **Maze Integration:** Creatures have access to maze for collision detection
3. **Specialization:** Each Smurf has unique stats and potential special abilities
4. **Simple AI:** Gargamel uses direct approach movement (no complex pathfinding)
5. **Buff Mechanics:** 
   - Red = Health restoration
   - Yellow = Speed boost
   - LadySmurf gets 2x effect for both buff types
6. **Entity Framework Ready:** Smurf class includes EF Core properties (Id, Level, IsInForest) for database persistence

---

## Usage Example

```csharp
// Create a maze
var maze = new MazeGenerator(17, 13, 36, 32);

// Create a player character
var player = new PapaSmurf(startX: 50, startY: 50, maze);

// Create Gargamel
var gargamel = new Gargamel(startX: 600, startY: 400, maze);

// Create buffs
var healthBuff = new RedBuff(x: 200, y: 200, duration: 300);
var speedBuff = new YellowBuff(x: 300, y: 300, duration: 300);

// Game loop (simplified)
while (gameRunning)
{
    // Move player based on input
    player.SetMovementInput(inputDx, inputDy);
    player.Move();
    
    // Move Gargamel
    gargamel.ChasePlayer(player.X, player.Y);
    
    // Check collision with Gargamel
    if (player.IsTouching(gargamel))
        player.TakeDamage(gargamel.Damage);
    
    // Check collision with buffs
    if (healthBuff.IsTouching(player))
    {
        healthBuff.ApplyTo(player);
    }
    
    if (speedBuff.IsTouching(player))
    {
        speedBuff.ApplyTo(player);
    }
}
```

---

## Files Cleaned Up

The following old/obsolete files have been removed:
- `Entity.cs` (replaced by Creature)
- `Spider.cs` (old enemy)
- `Bug.cs` (old enemy)
- `BzzFly.cs` (old enemy)
- `NewCharacters.cs` (placeholder)
- `BluePotion.cs` (old buff, replaced by YellowBuff)
- `RedPotion.cs` (old buff, replaced by RedBuff)
- `Berry.cs` (old item)
- `Item.cs` (old base class)

---

## Namespace
All entity classes are in: `SmurfGame.BL.Entities`
