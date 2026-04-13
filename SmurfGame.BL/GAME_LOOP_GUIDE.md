# SmurfGame Loop & Mechanics Guide

## Game Loop Overview

Every game tick (typically every 50ms), your code performs these steps:

1. **Move the Player** - Process keyboard input and move the selected Smurf
2. **Move Gargamel** - Simple AI chase toward player
3. **Check Creature Collision** - If touching, deal damage
4. **Manage Buffs** - Spawn buffs randomly, check for pickup
5. **Redraw Everything** - Update UI and canvas

---

## Movement System

### Player Movement

```csharp
// In your game loop input handler:
if (keyPressed == Keys.Z)
    currentSmurf.SetMovementInput(0, -speed);  // Move up
else if (keyPressed == Keys.S)
    currentSmurf.SetMovementInput(0, speed);   // Move down
else if (keyPressed == Keys.Q)
    currentSmurf.SetMovementInput(-speed, 0);  // Move left
else if (keyPressed == Keys.D)
    currentSmurf.SetMovementInput(speed, 0);   // Move right

// Then in your game tick:
currentSmurf.Move();  // Applies movement with collision checking
```

**Key Characteristics:**
- Movement checks all 4 corners of hitbox for wall collision
- Player can "slide" along walls (if moving X hits wall but Y is clear, only Y moves)
- Speed is multiplied by movement direction (e.g., Speed=4 moves 4 pixels per tick)

---

### Gargamel AI

```csharp
// In your game loop:
Gargamel gargamel = new Gargamel(startX, startY, maze);

// Each tick:
gargamel.ChasePlayer(currentSmurf.X, currentSmurf.Y);
```

**How It Works:**
1. Compare Gargamel's position to player's position
2. Move by `Speed` (3) steps toward player in both X and Y directions
3. Uses same collision system as player (respects walls)
4. Creates natural "chase" behavior without pathfinding

Example movement logic in Gargamel:
```
if (X < targetX)      moveX = +Speed
else if (X > targetX) moveX = -Speed
else                  moveX = 0

if (Y < targetY)      moveY = +Speed
else if (Y > targetY) moveY = -Speed
else                  moveY = 0
```

---

## Collision Detection

### Creature-to-Creature Collision

```csharp
// Check if player is touching Gargamel
if (currentSmurf.IsTouching(gargamel))
{
    // Player takes damage
    currentSmurf.TakeDamage(gargamel.Damage);
    
    // Optional: If player is StrongSmurf, deal counter-damage
    if (currentSmurf is StrongSmurf strong)
    {
        gargamel.TakeDamage(strong.CounterDamage);
    }
}
```

**Method:** `bool IsTouching(Creature other)`
- Returns true if hitboxes overlap
- Uses: `Math.Abs(X - other.X) < Size && Math.Abs(Y - other.Y) < Size`
- Default Size = 14 pixels

### Creature-to-Buff Collision

```csharp
// Check if player touched the buff
if (buff.IsTouching(currentSmurf))
{
    // Apply buff effect
    buff.ApplyTo(currentSmurf);
    
    // Remove buff from game (move off-screen or mark as consumed)
    buff.X = -100;
    buff.Y = -100;
}
```

**Method:** `bool IsTouching(Creature creature)` (in Buff class)
- Buff Size = 12 pixels (default)
- Creates larger hit-detection radius than creatures

---

## Buff System

### Buff Spawning

```csharp
// In your game loop, every few ticks:
if (Random.Next(0, 100) < 2)  // 2% chance per tick
{
    int x = Random.Next(40, 640);
    int y = Random.Next(60, 460);
    
    bool isYellow = Random.Next(0, 2) == 0;
    
    if (isYellow)
        currentBuff = new YellowBuff(x, y, duration: 300);  // ~6 seconds at 50ms ticks
    else
        currentBuff = new RedBuff(x, y, duration: 300);
}
```

### Red Buff (Health Restoration)

```csharp
public class RedBuff : Buff
{
    public const int HealthRestore = 30;
    
    public override void ApplyTo(Creature target)
    {
        int heal = HealthRestore;
        
        // Double effect for LadySmurf
        if (target is LadySmurf)
            heal = (int)(heal * LadySmurf.BuffMultiplier);  // = 60
        
        target.Heal(heal);
    }
}
```

**Effect:**
- Normal Smurf: +30 HP
- LadySmurf: +60 HP (2x multiplier)

### Yellow Buff (Speed Boost)

```csharp
public class YellowBuff : Buff
{
    public const int SpeedBoost = 4;
    
    public override void ApplyTo(Creature target)
    {
        int boost = SpeedBoost;
        
        // Double effect for LadySmurf
        if (target is LadySmurf)
            boost = (int)(boost * LadySmurf.BuffMultiplier);  // = 8
        
        target.Speed += boost;
    }
}
```

**Effect:**
- Normal Smurf: +4 Speed (total Speed increases)
- LadySmurf: +8 Speed (2x multiplier)

---

## Character Stats

### PapaSmurf
- **Speed:** 4 pixels/tick
- **MaxHealth:** 150 (highest HP)
- **Role:** Balanced leader, great for beginners
- **Advantage:** Extra health pool for longer survival

### StrongSmurf
- **Speed:** 6 pixels/tick (fastest)
- **MaxHealth:** 100
- **CounterDamage:** 20
- **Role:** Offensive character
- **Advantage:** Fast movement, can damage Gargamel back on collision

### LadySmurf
- **Speed:** 5 pixels/tick
- **MaxHealth:** 120
- **BuffMultiplier:** 2.0x
- **Role:** Buff specialist
- **Advantage:** Buffs are twice as effective

---

## Health System

### Taking Damage

```csharp
// Creature takes damage
creature.TakeDamage(amount);

// Health is clamped to minimum 0
public void TakeDamage(int damage)
{
    Health -= damage;
    if (Health < 0) Health = 0;
}
```

### Healing

```csharp
// Creature heals
creature.Heal(amount);

// Health is clamped to maximum
public void Heal(int amount)
{
    Health += amount;
    if (Health > MaxHealth) Health = MaxHealth;
}
```

### Death Condition

```csharp
if (currentSmurf.Health <= 0)
{
    gameLoopTimer.Stop();
    MessageBox.Show($"Game Over! You survived {score} ticks.");
    // Game ends
}
```

---

## Maze & Collision

### Wall Collision Detection

All creatures use the same maze collision system:

```csharp
// Before moving, check all 4 corners
bool canMoveX = !maze.IsWall(newX, Y) &&
                !maze.IsWall(newX + Size, Y) &&
                !maze.IsWall(newX, Y + Size) &&
                !maze.IsWall(newX + Size, Y + Size);

bool canMoveY = !maze.IsWall(X, newY) &&
                !maze.IsWall(X + Size, newY) &&
                !maze.IsWall(X, newY + Size) &&
                !maze.IsWall(X + Size, newY + Size);

if (canMoveX) X = newX;
if (canMoveY) Y = newY;
```

**Key Feature:** Movement is resolved independently for X and Y, allowing "sliding" along walls.

### Maze Properties

```csharp
var maze = new MazeGenerator(
    cols: 17,
    rows: 13,
    tileW: 36,    // pixels per tile width
    tileH: 32     // pixels per tile height
);

// Maze dimensions
int pixelWidth = 17 * 36 = 612 pixels
int pixelHeight = 13 * 32 = 416 pixels

// Wall thickness = 6 pixels
const int WALL_THICKNESS = 6;
```

---

## Scoring & Win Condition

### Score System

```csharp
private int score = 0;

// In game loop, every tick:
score++;

// Update UI
this.Text = $"Playing! Score: {score}";
```

Score = survival ticks. At 50ms per tick:
- 20 ticks = 1 second
- 1200 ticks = 60 seconds (1 minute)

### Win Condition (Example)

```csharp
const int TARGET_SURVIVAL_TICKS = 1200;  // 60 seconds

// In game loop:
if (score >= TARGET_SURVIVAL_TICKS)
{
    gameLoopTimer.Stop();
    MessageBox.Show($"You won! Final score: {score}");
}
```

---

## Step-by-Step Game Loop Implementation

```csharp
private void GameLoopTimer_Tick(object sender, EventArgs e)
{
    // ========== STEP 1: MOVE PLAYER ==========
    if (inputX != 0 || inputY != 0)
    {
        currentSmurf.SetMovementInput(inputX, inputY);
        currentSmurf.Move();
        
        // Update UI
        pbPlayer.Left = currentSmurf.X;
        pbPlayer.Top = currentSmurf.Y;
    }
    
    // ========== STEP 2: MOVE GARGAMEL ==========
    gargamel.ChasePlayer(currentSmurf.X, currentSmurf.Y);
    pbGargamel.Left = gargamel.X;
    pbGargamel.Top = gargamel.Y;
    
    // ========== STEP 3: CHECK COLLISION ==========
    if (currentSmurf.IsTouching(gargamel))
    {
        currentSmurf.TakeDamage(gargamel.Damage);
        UpdateHealthBar();
        
        if (currentSmurf.Health <= 0)
        {
            gameLoopTimer.Stop();
            MessageBox.Show($"Game Over! Score: {score}");
            return;
        }
    }
    
    // ========== STEP 4: MANAGE BUFFS ==========
    // Spawn buff if needed
    if (activeBuff == null && Random.Next(0, 100) < 2)
    {
        activeBuff = CreateRandomBuff();
    }
    
    // Check buff collision
    if (activeBuff != null && activeBuff.IsTouching(currentSmurf))
    {
        activeBuff.ApplyTo(currentSmurf);
        activeBuff = null;  // Remove buff
    }
    
    // ========== STEP 5: UPDATE SCORE & DISPLAY ==========
    score++;
    this.Text = $"Playing! Score: {score}";
    lblHealth.Text = $"HP: {currentSmurf.Health}/{currentSmurf.MaxHealth}";
    
    // Trigger redraw (automatically happens with PictureBox updates)
}
```

---

## Quick Reference: Class Methods

### Creature Methods
- `abstract void Move()` - Override to implement movement
- `protected void MoveWithCollision(int dx, int dy)` - Move with wall checking
- `void TakeDamage(int damage)` - Deal damage
- `void Heal(int amount)` - Restore health
- `bool IsTouching(Creature other)` - Check collision

### Smurf Methods (all subclasses)
- `void SetMovementInput(int dx, int dy)` - Set direction
- `void Move()` - Move in set direction

### Gargamel Methods
- `void ChasePlayer(int targetX, int targetY)` - AI movement toward player

### Buff Methods
- `abstract void ApplyTo(Creature target)` - Apply buff effect
- `bool IsTouching(Creature creature)` - Check collision
