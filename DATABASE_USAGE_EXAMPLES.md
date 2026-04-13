# Usage Examples - SmurfGame Database & StartForm

## Example 1: Accessing Game Scores

### Get Top 10 Survivors

```csharp
using SmurfGame.DAL.Repositories;
using SmurfGame.DAL.Entities;

var repository = new ScoreRepository();
List<Score> topTen = repository.GetTopScores(10);

foreach (var score in topTen)
{
    Console.WriteLine($"{score.PlayerName} - {score.Points}s as {score.SmurfType}");
}
```

**Output Example:**
```
Alice - 120s as PapaSmurf
Bob - 95s as StrongSmurf
Charlie - 85s as LadySmurf
Diana - 78s as PapaSmurf
```

---

## Example 2: Get All Games Played (Most Recent First)

```csharp
var repository = new ScoreRepository();
List<Score> allGames = repository.GetAllScores();

Console.WriteLine($"Total games played: {allGames.Count}");

foreach (var game in allGames)
{
    Console.WriteLine($"{game.PlayedAt:yyyy-MM-dd HH:mm} - {game.PlayerName} survived {game.Points}s");
}
```

**Output Example:**
```
Total games played: 5
2026-04-03 23:45 - Alice survived 120s
2026-04-03 23:30 - Bob survived 95s
2026-04-03 23:15 - Alice survived 78s
```

---

## Example 3: Statistics for a Specific Player

```csharp
using System.Linq;

var repository = new ScoreRepository();
var allGames = repository.GetAllScores();

// Get all games by "Alice"
var aliceGames = allGames.Where(s => s.PlayerName == "Alice").ToList();

Console.WriteLine($"Alice's stats:");
Console.WriteLine($"  Games played: {aliceGames.Count}");
Console.WriteLine($"  Best score: {aliceGames.Max(s => s.Points)}s");
Console.WriteLine($"  Average survival: {aliceGames.Average(s => s.Points):F1}s");

// Most played character
var favoriteChar = aliceGames
    .GroupBy(s => s.SmurfType)
    .OrderByDescending(g => g.Count())
    .First()
    .Key;

Console.WriteLine($"  Favorite character: {favoriteChar}");
```

**Output Example:**
```
Alice's stats:
  Games played: 5
  Best score: 120s
  Average survival: 89.2s
  Favorite character: PapaSmurf
```

---

## Example 4: Character Statistics (Who's the Best Smurf?)

```csharp
var repository = new ScoreRepository();
var allGames = repository.GetAllScores();

var characterStats = allGames
    .GroupBy(s => s.SmurfType)
    .Select(g => new
    {
        Character = g.Key,
        GamesPlayed = g.Count(),
        AverageSurvival = g.Average(s => s.Points),
        BestScore = g.Max(s => s.Points)
    })
    .OrderByDescending(x => x.AverageSurvival);

foreach (var stat in characterStats)
{
    Console.WriteLine($"{stat.Character}:");
    Console.WriteLine($"  Games: {stat.GamesPlayed}");
    Console.WriteLine($"  Average: {stat.AverageSurvival:F1}s");
    Console.WriteLine($"  Best: {stat.BestScore}s");
}
```

**Output Example:**
```
Papa Smurf:
  Games: 3
  Average: 100.3s
  Best: 120s
Strong Smurf:
  Games: 1
  Average: 95.0s
  Best: 95s
Lady Smurf:
  Games: 1
  Average: 85.0s
  Best: 85s
```

---

## Example 5: Today's High Scores

```csharp
using System;

var repository = new ScoreRepository();
var allGames = repository.GetAllScores();

var today = DateTime.Now.Date;
var todayGames = allGames
    .Where(s => s.PlayedAt.Date == today)
    .OrderByDescending(s => s.Points);

Console.WriteLine($"Today's Top Scores ({today:yyyy-MM-dd}):");
foreach (var game in todayGames)
{
    Console.WriteLine($"  {game.PlayerName}: {game.Points}s ({game.SmurfType})");
}
```

---

## Example 6: Save a Test Score

```csharp
using SmurfGame.DAL.Repositories;
using SmurfGame.DAL.Entities;

var repository = new ScoreRepository();

var testScore = new Score
{
    PlayerName = "TestPlayer",
    SmurfType = "PapaSmurf",
    Points = 60,
    PlayedAt = DateTime.Now
};

repository.SaveScore(testScore);
Console.WriteLine("Score saved successfully!");
```

---

## Example 7: Building a Leaderboard Display (WinForms)

```csharp
using System;
using System.Windows.Forms;
using SmurfGame.DAL.Repositories;

public class LeaderboardForm : Form
{
    private DataGridView dgvLeaderboard;
    
    public LeaderboardForm()
    {
        InitializeComponent();
        LoadLeaderboard();
    }
    
    private void LoadLeaderboard()
    {
        var repository = new ScoreRepository();
        var topScores = repository.GetTopScores(20);
        
        dgvLeaderboard.DataSource = topScores.Select((s, index) => new
        {
            Rank = index + 1,
            PlayerName = s.PlayerName,
            Character = s.SmurfType,
            Survival = $"{s.Points}s",
            Date = s.PlayedAt.ToString("yyyy-MM-dd HH:mm")
        }).ToList();
    }
    
    private void InitializeComponent()
    {
        dgvLeaderboard = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false
        };
        
        Controls.Add(dgvLeaderboard);
        Text = "Smurf Village Escape - Leaderboard";
        Size = new Size(600, 400);
    }
}
```

---

## Example 8: When Game Ends (Form1.cs Integration)

```csharp
// In GameLoopTimer_Tick, when currentSmurf.Health <= 0:

private void OnGameOver()
{
    gameLoopTimer.Stop();
    
    // Calculate survival time
    int survivalSeconds = score / 20;  // 50ms per tick
    
    // Save the score
    try
    {
        var repository = new ScoreRepository();
        var score = new SmurfGame.DAL.Entities.Score
        {
            PlayerName = playerName,        // From StartForm
            SmurfType = charChoice,          // "Papa Smurf" etc.
            Points = survivalSeconds,
            PlayedAt = DateTime.Now
        };
        
        repository.SaveScore(score);
        
        // Show results
        MessageBox.Show(
            $"Congratulations, {playerName}!\n\n" +
            $"You survived {survivalSeconds} seconds as {charChoice}.\n" +
            $"Score: {survivalSeconds} points",
            "Game Over!",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error saving score: {ex.Message}");
    }
}
```

---

## Example 9: Filtering by Date Range

```csharp
using System;
using System.Linq;

var repository = new ScoreRepository();
var allGames = repository.GetAllScores();

// Games from last 7 days
var sevenDaysAgo = DateTime.Now.AddDays(-7);
var recentGames = allGames
    .Where(s => s.PlayedAt >= sevenDaysAgo)
    .OrderByDescending(s => s.Points);

Console.WriteLine($"Top scores from last 7 days:");
foreach (var game in recentGames.Take(10))
{
    Console.WriteLine($"  {game.PlayerName}: {game.Points}s");
}
```

---

## Example 10: Database Health Check

```csharp
using System;
using SmurfGame.DAL.Repositories;

public void CheckDatabaseHealth()
{
    try
    {
        var repository = new ScoreRepository();
        var allScores = repository.GetAllScores();
        
        if (allScores.Count == 0)
        {
            Console.WriteLine("Database is empty - no games played yet");
        }
        else
        {
            var avgScore = allScores.Average(s => s.Points);
            var maxScore = allScores.Max(s => s.Points);
            var minScore = allScores.Min(s => s.Points);
            
            Console.WriteLine("Database Health Check:");
            Console.WriteLine($"  Total games: {allScores.Count}");
            Console.WriteLine($"  Highest score: {maxScore}s");
            Console.WriteLine($"  Lowest score: {minScore}s");
            Console.WriteLine($"  Average score: {avgScore:F1}s");
            Console.WriteLine("  Status: ✅ OK");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database Error: {ex.Message}");
        Console.WriteLine("  Status: ❌ FAILED");
    }
}
```

---

## Example 11: Export Scores to CSV

```csharp
using System;
using System.IO;
using System.Text;
using SmurfGame.DAL.Repositories;

public void ExportScoresToCsv(string filePath)
{
    var repository = new ScoreRepository();
    var allScores = repository.GetAllScores();
    
    var csv = new StringBuilder();
    csv.AppendLine("PlayerName,SmurfType,Points,PlayedAt");
    
    foreach (var score in allScores)
    {
        csv.AppendLine($"{score.PlayerName},{score.SmurfType},{score.Points},{score.PlayedAt:yyyy-MM-dd HH:mm:ss}");
    }
    
    File.WriteAllText(filePath, csv.ToString());
    Console.WriteLine($"Exported {allScores.Count} scores to {filePath}");
}

// Usage:
ExportScoresToCsv("smurf_scores.csv");
```

---

## Example 12: Reset All Scores (Admin Function)

```csharp
// ⚠️ WARNING: This deletes all game records!

public void ResetAllScores()
{
    var context = new SmurfGame.DAL.SmurfDbContext();
    context.Scores.RemoveRange(context.Scores);
    context.SaveChanges();
    Console.WriteLine("All scores have been deleted.");
}

// Safer alternative: Backup first
public void BackupAndReset()
{
    // Export to CSV first
    ExportScoresToCsv($"backup_{DateTime.Now:yyyy-MM-dd_HHmmss}.csv");
    
    // Then reset
    ResetAllScores();
}
```

---

## Testing Checklist

- [ ] Run `dotnet run --project SmurfGame.WinForms`
- [ ] Enter name and select character in StartForm
- [ ] Play a game to completion (let health reach 0)
- [ ] Verify database file created at `bin/Debug/net10.0/smurfgame.db`
- [ ] Run Example 1 to see your score in top 10
- [ ] Run Example 2 to see all games ordered by date
- [ ] Play multiple games with different characters
- [ ] Run Example 4 to see character statistics
- [ ] Verify no errors in exception handling

---

**All examples are ready to use!** Copy/paste any example into your code and adapt as needed.
