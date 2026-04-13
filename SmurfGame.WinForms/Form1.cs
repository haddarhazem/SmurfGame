using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using SmurfGame.BL;
using SmurfGame.BL.Entities;
using SmurfGame.DAL;
using Microsoft.EntityFrameworkCore;

namespace SmurfGame.WinForms
{
    public partial class Form1 : Form
    {
        int speed = 3;
        private Smurf? currentSmurf;
        private SmurfGameContext? db;
        private string charChoice = "Papa Smurf";
        private string playerName = "Player";
        private string smurfType = "PapaSmurf";

        private Bitmap? imgHaut, imgBas, imgGauche, imgDroite;
        private Bitmap? gargamelImg;
        private Bitmap? redMushroom, yellowMushroom;

        private MazeGenerator? maze;
        private Player? playerRef;

        private PictureBox? pbGargamel;
        private PictureBox? pbBuff;

        private int pDx = 0, pDy = 0;
        private int score = 0;
        private bool isYellowBuffActive = false;
        private int buffTimer = 0;

        // 1. Declare the Timers
        private System.Windows.Forms.Timer? autoSaveTimer;
        private System.Windows.Forms.Timer? gameLoopTimer;

        public Form1(string playerName = "Player", string smurfType = "PapaSmurf")
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.playerName = playerName;
            this.smurfType = smurfType;

            // Generate HTML maze as background image
            this.BackgroundImage = GenerateMazeBackground();
            this.BackgroundImageLayout = ImageLayout.None; // Match exactly to 680x520
            this.ClientSize = new System.Drawing.Size(680, 520);

            // Integrate information into the bottom info strip
            lblHealth.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            lblHealth.ForeColor = System.Drawing.Color.FromArgb(168, 200, 154);
            lblHealth.BackColor = System.Drawing.Color.FromArgb(22, 46, 20);
            lblHealth.Location = new System.Drawing.Point(20, 485);
            lblHealth.AutoSize = true;

            lblCoordinates.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            lblCoordinates.ForeColor = System.Drawing.Color.FromArgb(168, 200, 154);
            lblCoordinates.BackColor = System.Drawing.Color.FromArgb(22, 46, 20);
            lblCoordinates.Location = new System.Drawing.Point(300, 485);
            lblCoordinates.AutoSize = true;

            this.KeyUp += Form1_KeyUp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Use the smurf type passed from StartForm
            if (smurfType == "PapaSmurf")
                charChoice = "Papa Smurf";
            else if (smurfType == "StrongSmurf")
                charChoice = "Strong Smurf";
            else if (smurfType == "LadySmurf")
                charChoice = "Lady Smurf";

            // Try to load images from multiple possible locations
            bool imagesLoaded = TryLoadImages(charChoice);

            if (!imagesLoaded)
            {
                MessageBox.Show(
                    "Warning: Image files not found. The game will run without sprites.\n\n" +
                    "To fix this, ensure the 'images' folder exists in the project root with:\n" +
                    "- images\\papa smurf\\ (back.png, face.png, left.png, right.png)\n" +
                    "- images\\strong smurf\\ (back.png, face.png, left.png, right.png)\n" +
                    "- images\\lady smurf\\ (back.png, face.png, left.png, right.png)\n" +
                    "- images\\gargamel\\ (front.png)\n" +
                    "- images\\buffs\\ (red buff.png, yellow buff.png)",
                    "Missing Images",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            maze = new MazeGenerator(17, 13, (680-40)/17, (520-40-60)/13);

            // Start player in a well-centered safe position in first cell
            // TileW and TileH are the cell dimensions, start near the center
            int startX = 20 + (maze.TileW / 2);
            int startY = 40 + (maze.TileH / 2);
            playerRef = new Player(startX, startY, maze);

            System.Diagnostics.Debug.WriteLine($"Maze TileW: {maze.TileW}, TileH: {maze.TileH}");
            System.Diagnostics.Debug.WriteLine($"Player starting at X: {playerRef.X}, Y: {playerRef.Y}");

            pbPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPlayer.Size = new Size(20, 20);
            pbPlayer.Location = new Point(playerRef.X, playerRef.Y);
            pbPlayer.BackColor = Color.Transparent;
            if (imgBas != null) pbPlayer.Image = imgBas;

            // Setup Gargamel
            pbGargamel = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(24, 24), BackColor = Color.Transparent, Image = gargamelImg };
            pbGargamel.Location = new Point(680 - 64, 520 - 100);
            this.Controls.Add(pbGargamel);
            pbGargamel.BringToFront();

            // Setup Buff
            pbBuff = new PictureBox { SizeMode = PictureBoxSizeMode.StretchImage, Size = new Size(16, 16), BackColor = Color.Transparent, Image = redMushroom ?? yellowMushroom };
            pbBuff.Location = new Point(-100, -100);
            this.Controls.Add(pbBuff);
            pbBuff.BringToFront();

            // 1. Connect to the database
            db = new SmurfGameContext(new DbContextOptions<SmurfGameContext>());

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // --- NEW: DELETE OLD DATA ---
            var oldSmurfs = db.Smurfs.ToList();
            if (oldSmurfs.Count > 0)
            {
                db.Smurfs.RemoveRange(oldSmurfs);
                db.SaveChanges(); // Push the delete to SQL
            }
            // ----------------------------

            // 2. Create the fresh Smurf for this session
            if (charChoice == "Papa Smurf") 
                currentSmurf = new PapaSmurf(pbPlayer.Left, pbPlayer.Top, maze);
            else if (charChoice == "Lady Smurf") 
                currentSmurf = new LadySmurf(pbPlayer.Left, pbPlayer.Top, maze);
            else 
                currentSmurf = new StrongSmurf(pbPlayer.Left, pbPlayer.Top, maze);

            // Note: The new Smurf classes don't use Entity Framework
            // They are pure game entities without database persistence

            this.Text = $"Smurf Village Escape - {playerName} as {charChoice} | Score: 0";
            lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";

            autoSaveTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            autoSaveTimer.Start();

            // Game Loop setup
            gameLoopTimer = new System.Windows.Forms.Timer { Interval = 50 };
            gameLoopTimer.Tick += GameLoopTimer_Tick;
            gameLoopTimer.Start();

            UpdateHealthBar();
        }

        // 3. The method that runs every 1 second
        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (db != null)
            {
                // This pushes whatever the current X and Y are to SQL Server
                db.SaveChanges();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z: pDy = -speed; if (imgHaut != null) pbPlayer.Image = imgHaut; break;
                case Keys.S: pDy = speed; if (imgBas != null) pbPlayer.Image = imgBas; break;
                case Keys.Q: pDx = -speed; if (imgGauche != null) pbPlayer.Image = imgGauche; break;
                case Keys.D: pDx = speed; if (imgDroite != null) pbPlayer.Image = imgDroite; break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z: if (pDy < 0) pDy = 0; break;
                case Keys.S: if (pDy > 0) pDy = 0; break;
                case Keys.Q: if (pDx < 0) pDx = 0; break;
                case Keys.D: if (pDx > 0) pDx = 0; break;
            }
        }

        private void GameLoopTimer_Tick(object sender, EventArgs e)
        {
            // Move Player with collision detection
            if (pDx != 0 || pDy != 0)
            {
                int currentSpeed = isYellowBuffActive ? speed + 2 : speed;
                int moveX = Math.Sign(pDx) * currentSpeed;
                int moveY = Math.Sign(pDy) * currentSpeed;

                if (playerRef != null)
                {
                    int oldX = playerRef.X;
                    int oldY = playerRef.Y;

                    playerRef.Move(moveX, moveY);

                    if (playerRef.X != oldX || playerRef.Y != oldY)
                    {
                        pbPlayer.Left = playerRef.X;
                        pbPlayer.Top = playerRef.Y;
                    }
                }

                if (currentSmurf != null)
                {
                    currentSmurf.X = pbPlayer.Left;
                    currentSmurf.Y = pbPlayer.Top;
                    lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";
                }
            }

            // Move Gargamel (AI Chase WITH COLLISION DETECTION)
            int gSpeed = 2;
            if (pbGargamel != null && maze != null)
            {
                int newX = pbGargamel.Left;
                int newY = pbGargamel.Top;

                // Try X movement
                if (pbPlayer.Left > pbGargamel.Left)
                    newX += gSpeed;
                else if (pbPlayer.Left < pbGargamel.Left)
                    newX -= gSpeed;

                // Check X collision (24px size for Gargamel)
                if (!maze.IsWall(newX, newY) && !maze.IsWall(newX + 24, newY) &&
                    !maze.IsWall(newX, newY + 24) && !maze.IsWall(newX + 24, newY + 24))
                {
                    pbGargamel.Left = newX;
                }

                // Try Y movement
                newY = pbGargamel.Top;
                if (pbPlayer.Top > pbGargamel.Top)
                    newY += gSpeed;
                else if (pbPlayer.Top < pbGargamel.Top)
                    newY -= gSpeed;

                // Check Y collision (24px size for Gargamel)
                if (!maze.IsWall(pbGargamel.Left, newY) && !maze.IsWall(pbGargamel.Left + 24, newY) &&
                    !maze.IsWall(pbGargamel.Left, newY + 24) && !maze.IsWall(pbGargamel.Left + 24, newY + 24))
                {
                    pbGargamel.Top = newY;
                }
            }

            // Collision with Gargamel
            if (pbGargamel != null && pbPlayer.Bounds.IntersectsWith(pbGargamel.Bounds))
            {
                if (currentSmurf != null && currentSmurf.Health > 0)
                {
                    currentSmurf.Health -= 1;
                    UpdateHealthBar();
                }
                else if (currentSmurf != null)
                {
                    gameLoopTimer?.Stop();

                    // Calculate survival time in seconds (50ms per tick)
                    int survivalSeconds = score / 20;

                    // Save the score to database
                    try
                    {
                        var scoreRepo = new SmurfGame.DAL.Repositories.ScoreRepository();
                        var newScore = new SmurfGame.DAL.Entities.Score
                        {
                            PlayerName = playerName,
                            SmurfType = charChoice,
                            Points = survivalSeconds,
                            PlayedAt = DateTime.Now
                        };
                        scoreRepo.SaveScore(newScore);
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't crash the game over screen
                        System.Diagnostics.Debug.WriteLine($"Error saving score: {ex.Message}");
                    }

                    MessageBox.Show($"Game Over! {playerName}\n\nYou survived {survivalSeconds} seconds as {charChoice}.\nFinal Score: {score} ticks", "Game Over");
                }
            }

            // Spawn Buffs randomly
            if (new Random().Next(0, 100) < 2 && pbBuff.Left < 0) // small chance to spawn if offscreen
            {
                int bx = new Random().Next(40, 600);
                int by = new Random().Next(60, 460);
                pbBuff.Location = new Point(bx, by);
                bool isYellow = new Random().Next(0, 2) == 0;
                pbBuff.Tag = isYellow ? "yellow" : "red";

                if (isYellow && yellowMushroom != null)
                    pbBuff.Image = yellowMushroom;
                else if (!isYellow && redMushroom != null)
                    pbBuff.Image = redMushroom;
            }

            // Collision with Buff
            if (pbBuff != null && pbPlayer.Bounds.IntersectsWith(pbBuff.Bounds) && pbBuff.Left > 0)
            {
                if (currentSmurf != null)
                {
                    if ((string?)pbBuff.Tag == "red")
                    {
                        currentSmurf.Health = Math.Min(currentSmurf.Health + 20, currentSmurf.MaxHealth);
                        UpdateHealthBar();
                    }
                    else if ((string?)pbBuff.Tag == "yellow")
                    {
                        isYellowBuffActive = true;
                        buffTimer = 100; // ~5 seconds of boost
                    }
                }
                pbBuff.Location = new Point(-100, -100); // hide buff
            }

            // Handle buff timer
            if (isYellowBuffActive)
            {
                buffTimer--;
                if (buffTimer <= 0) isYellowBuffActive = false;
            }

            score++;
            this.Text = $"Smurf Village Escape - {playerName} as {charChoice} | Score: {score}";
        }
        private void UpdateHealthBar()
        {
            if (currentSmurf != null)
            {
                // 1. Find the percentage (Current / Max * 100)
                double percent = ((double)currentSmurf.Health / currentSmurf.MaxHealth) * 100;

                // 2. Figure out how many '#' to draw (divide by 10)
                int hashCount = (int)(percent / 10);

                // Prevent errors if health drops below 0
                if (hashCount < 0) hashCount = 0;

                // 3. Build the text strings
                string healthBars = new string('#', hashCount);
                string emptyBars = new string('-', 10 - hashCount); // Fills the missing health with dashes

                // 4. Put it all together in the label
                lblHealth.Text = $"HP: [{healthBars}{emptyBars}] {Math.Round(percent)}%";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop(); // Stop the timer so it doesn't crash while closing
            }

            if (db != null)
            {
                db.SaveChanges(); // One final save
                db.Dispose();
            }
        }

        /// <summary>
        /// Tries to load images from multiple possible locations.
        /// Returns true if all images were loaded successfully.
        /// </summary>
        private bool TryLoadImages(string smurfType)
        {
            try
            {
                // Try multiple paths relative to different possible execution locations
                string[] possibleImageBasePaths = new[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images"), // App domain base
                    Path.Combine(AppContext.BaseDirectory, "images"),  // App directory
                    Path.Combine(Directory.GetCurrentDirectory(), "images"), // Working directory
                    "images",                                            // Current directory
                    // Look in parent directories (for when running from bin\Debug)
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "images"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "images")
                };

                string imagePath = null;
                foreach (var basePath in possibleImageBasePaths)
                {
                    string fullPath = Path.GetFullPath(basePath); // Resolve .. and . in the path
                    System.Diagnostics.Debug.WriteLine($"Checking for images at: {fullPath}");
                    if (Directory.Exists(fullPath))
                    {
                        imagePath = fullPath;
                        System.Diagnostics.Debug.WriteLine($"✓ Found images folder at: {imagePath}");
                        break;
                    }
                }

                if (imagePath == null)
                {
                    System.Diagnostics.Debug.WriteLine("❌ Images folder not found in any expected location");
                    System.Diagnostics.Debug.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
                    System.Diagnostics.Debug.WriteLine($"AppDomain BaseDirectory: {AppDomain.CurrentDomain.BaseDirectory}");
                    return false;
                }

                string pathWithSpace = smurfType.ToLower();
                string pathWithUnderscore = smurfType.ToLower().Replace(" ", "_");

                // Try both folder naming conventions: with spaces and with underscores
                string smurfPath = null;
                string[] possiblePaths = new[]
                {
                    Path.Combine(imagePath, pathWithSpace),      // e.g., "papa smurf"
                    Path.Combine(imagePath, pathWithUnderscore)  // e.g., "papa_smurf"
                };

                foreach (var possiblePath in possiblePaths)
                {
                    if (Directory.Exists(possiblePath))
                    {
                        smurfPath = possiblePath;
                        System.Diagnostics.Debug.WriteLine($"Found smurf folder at: {smurfPath}");
                        break;
                    }
                }

                if (smurfPath == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Looking for smurf images in: {possiblePaths[0]} or {possiblePaths[1]}");
                    return false;
                }

                // Load smurf direction images with proper naming convention
                // Files are named like: "papa smurf back.png", "papa smurf face.png", etc.
                string smurfPrefix = smurfType;
                string haut = Path.Combine(smurfPath, $"{smurfPrefix} back.png");
                string bas = Path.Combine(smurfPath, $"{smurfPrefix} face.png");
                string gauche = Path.Combine(smurfPath, $"{smurfPrefix} left.png");
                string droite = Path.Combine(smurfPath, $"{smurfPrefix} right.png");

                // Check if all files exist
                if (!File.Exists(haut))
                    System.Diagnostics.Debug.WriteLine($"Missing: {haut}");
                if (!File.Exists(bas))
                    System.Diagnostics.Debug.WriteLine($"Missing: {bas}");
                if (!File.Exists(gauche))
                    System.Diagnostics.Debug.WriteLine($"Missing: {gauche}");
                if (!File.Exists(droite))
                    System.Diagnostics.Debug.WriteLine($"Missing: {droite}");

                if (!File.Exists(haut) || !File.Exists(bas) || 
                    !File.Exists(gauche) || !File.Exists(droite))
                {
                    System.Diagnostics.Debug.WriteLine($"Missing smurf images in: {smurfPath}");
                    return false;
                }

                // Load smurf images
                imgHaut = (Bitmap)Image.FromFile(haut);
                imgBas = (Bitmap)Image.FromFile(bas);
                imgGauche = (Bitmap)Image.FromFile(gauche);
                imgDroite = (Bitmap)Image.FromFile(droite);

                // Load Gargamel image
                string gargamelPath = Path.Combine(imagePath, "gargamel", "gargamel front.png");
                if (!File.Exists(gargamelPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Missing Gargamel image: {gargamelPath}");
                    return false;
                }
                gargamelImg = (Bitmap)Image.FromFile(gargamelPath);

                // Load buff images
                string redBuffPath = Path.Combine(imagePath, "buffs", "red buff.png");
                string yellowBuffPath = Path.Combine(imagePath, "buffs", "yellow buff.png");

                if (!File.Exists(redBuffPath))
                    System.Diagnostics.Debug.WriteLine($"Missing: {redBuffPath}");
                if (!File.Exists(yellowBuffPath))
                    System.Diagnostics.Debug.WriteLine($"Missing: {yellowBuffPath}");

                if (!File.Exists(redBuffPath) || !File.Exists(yellowBuffPath))
                {
                    System.Diagnostics.Debug.WriteLine($"Missing buff images");
                    return false;
                }

                redMushroom = (Bitmap)Image.FromFile(redBuffPath);
                yellowMushroom = (Bitmap)Image.FromFile(yellowBuffPath);

                // Remove white background from all images
                if (imgHaut != null) imgHaut.MakeTransparent(Color.White);
                if (imgBas != null) imgBas.MakeTransparent(Color.White);
                if (imgGauche != null) imgGauche.MakeTransparent(Color.White);
                if (imgDroite != null) imgDroite.MakeTransparent(Color.White);
                if (gargamelImg != null) gargamelImg.MakeTransparent(Color.White);
                if (redMushroom != null) redMushroom.MakeTransparent(Color.White);
                if (yellowMushroom != null) yellowMushroom.MakeTransparent(Color.White);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading images: {ex.Message}");
                return false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblHealth_Click(object sender, EventArgs e)
        {

        }

        private class MazeCell
        {
            public bool N = true, S = true, E = true, W = true, Visited = false;
        }

        private Bitmap GenerateMazeBackground()
        {
            int W = 680, H = 520;
            Bitmap bmp = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int COLS = 17, ROWS = 13;
                int CELL_W = (W - 40) / COLS;
                int CELL_H = (H - 40 - 60) / ROWS;
                int OX = 20, OY = 40;

                MazeCell[,] cells = new MazeCell[ROWS, COLS];
                for (int r = 0; r < ROWS; r++)
                    for (int c = 0; c < COLS; c++)
                        cells[r, c] = new MazeCell();

                Random rnd = new Random(42);

                void Carve(int r, int c)
                {
                    cells[r, c].Visited = true;
                    int[] dirs = { 0, 1, 2, 3 }; // N=0, S=1, E=2, W=3
                    for (int i = 0; i < 4; i++) {
                        int swap = rnd.Next(i, 4);
                        int temp = dirs[i]; dirs[i] = dirs[swap]; dirs[swap] = temp;
                    }

                    foreach (int dir in dirs)
                    {
                        int dr = 0, dc = 0;
                        if (dir == 0) dr = -1; else if (dir == 1) dr = 1; else if (dir == 2) dc = 1; else if (dir == 3) dc = -1;
                        int nr = r + dr, nc = c + dc;

                        if (nr >= 0 && nr < ROWS && nc >= 0 && nc < COLS && !cells[nr, nc].Visited)
                        {
                            if (dir == 0) { cells[r, c].N = false; cells[nr, nc].S = false; }
                            else if (dir == 1) { cells[r, c].S = false; cells[nr, nc].N = false; }
                            else if (dir == 2) { cells[r, c].E = false; cells[nr, nc].W = false; }
                            else if (dir == 3) { cells[r, c].W = false; cells[nr, nc].E = false; }
                            Carve(nr, nc);
                        }
                    }
                }
                Carve(0, 0);

                // Sky - Match HTML: #0f1c2e (15, 28, 46)
                g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(15, 28, 46)), 0, 0, W, OY + CELL_H / 2);

                // Stars - Match HTML positions and color
                int[][] stars = { new[]{45,8}, new[]{110,22}, new[]{180,6}, new[]{250,28}, new[]{340,14}, new[]{410,5}, new[]{470,26}, new[]{535,10}, new[]{595,30}, new[]{648,18}, new[]{80,38}, new[]{290,42}, new[]{510,36} };
                using (var starBrush = new SolidBrush(System.Drawing.Color.FromArgb(230, 240, 255, 217)))
                {
                    foreach (var s in stars) 
                        g.FillEllipse(starBrush, s[0] - 1.1f, s[1] - 1.1f, 2.2f, 2.2f);
                }

                // Moon - Match HTML: outer #fff8dc (255, 248, 220), inner #0f1c2e
                g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(255, 248, 220)), 580 - 20, 28 - 20, 40, 40);
                g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(15, 28, 46)), 592 - 15, 21 - 15, 30, 30);

                // Ground - Match HTML: #2a5224 (42, 82, 36)
                g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(42, 82, 36)), 0, OY - 10, W, H - OY + 10);

                // Distant tree silhouettes - Match HTML: #162e14 (22, 46, 20)
                using (var distantTreeBrush = new SolidBrush(System.Drawing.Color.FromArgb(22, 46, 20)))
                {
                    // Tree silhouettes at: [60,38],[180,44],[330,36],[490,42],[630,38]
                    var distantTrees = new (int cx, int r)[] { (60, 38), (180, 44), (330, 36), (490, 42), (630, 38) };
                    foreach (var (cx, r) in distantTrees)
                    {
                        g.FillEllipse(distantTreeBrush, cx - r * 1.6f, OY - r, r * 3.2f, r * 2);
                    }
                }

                // Maze floor (path cells) - Match HTML: #2d5a27 (45, 90, 39)
                using (var pathBrush = new SolidBrush(System.Drawing.Color.FromArgb(45, 90, 39)))
                {
                    for (int r = 0; r < ROWS; r++)
                        for (int c = 0; c < COLS; c++)
                            g.FillRectangle(pathBrush, OX + c * CELL_W + 1, OY + r * CELL_H + 1, CELL_W - 1, CELL_H - 1);
                }

                // Maze walls - Match HTML: color #1a3d18 (26, 61, 24), width 6
                using (var wallPen = new Pen(System.Drawing.Color.FromArgb(26, 61, 24), 6))
                {
                    g.DrawRectangle(wallPen, OX, OY, COLS * CELL_W, ROWS * CELL_H);

                    for (int r = 0; r < ROWS; r++)
                    {
                        for (int c = 0; c < COLS; c++)
                        {
                            var cell = cells[r, c];
                            int x = OX + c * CELL_W, y = OY + r * CELL_H;

                            if (cell.S && r < ROWS - 1)
                                g.DrawLine(wallPen, x, y + CELL_H, x + CELL_W, y + CELL_H);

                            if (cell.E && c < COLS - 1)
                                g.DrawLine(wallPen, x + CELL_W, y, x + CELL_W, y + CELL_H);
                        }
                    }
                }

                // Wall texture - Match HTML: color #245c1e (36, 92, 30), width 2
                using (var texPen = new Pen(System.Drawing.Color.FromArgb(36, 92, 30), 2))
                {
                    for (int r = 0; r < ROWS; r++)
                    {
                        for (int c = 0; c < COLS; c++)
                        {
                            var cell = cells[r, c];
                            int x = OX + c * CELL_W, y = OY + r * CELL_H;

                            if (cell.S && r < ROWS - 1)
                                g.DrawLine(texPen, x + 2, y + CELL_H - 2, x + CELL_W - 2, y + CELL_H - 2);

                            if (cell.E && c < COLS - 1)
                                g.DrawLine(texPen, x + CELL_W - 2, y + 2, x + CELL_W - 2, y + CELL_H - 2);
                        }
                    }
                }

                // Grass tufts - Match HTML colors: #3a7232, #44843b
                void DrawTuft(float tx, float ty)
                {
                    g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(58, 114, 50)), tx - 3, ty - 6, 6, 12);
                    g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(68, 132, 59)), tx + 2.5f, ty - 5, 5, 10);
                }
                int[][] tufts = { new[]{170,140}, new[]{430,220}, new[]{60,310}, new[]{560,340}, new[]{250,400}, new[]{480,440}, new[]{100,460}, new[]{370,480} };
                foreach (var t in tufts) DrawTuft(t[0], t[1]);

                // Border trees - Match HTML layered approach
                void DrawTree(float tx, float ty, float size)
                {
                    g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(74, 48, 24)), tx - 3, ty, 6, size * 1.5f);
                    g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(27, 94, 40)), tx - size, ty - size * 0.6f, size * 2, size * 2.6f);
                    g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(33, 104, 48)), tx - size * 0.5f, ty, size * 1.3f, size * 1.8f);
                    g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(23, 78, 32)), tx + size * 0.55f, ty - size * 0.2f, size * 1.2f, size * 1.7f);
                }
                DrawTree(8, OY - 10, 22);
                DrawTree(672, OY - 10, 22);
                DrawTree(340, OY - 14, 20);
                DrawTree(180, OY - 8, 17);
                DrawTree(500, OY - 8, 17);

                // Bottom info strip - Match HTML: #162e14 (22, 46, 20)
                int BY = OY + ROWS * CELL_H + 10;
                g.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(22, 46, 20)), 0, BY - 4, W, H - BY + 4);

                // Entry / Exit markers - Match HTML: green #7ecf5a (126, 207, 90), red #e84040 (232, 64, 64)
                g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(126, 207, 90)), OX + CELL_W / 2 - 6, OY + CELL_H / 2 - 6, 12, 12);
                g.FillEllipse(new SolidBrush(System.Drawing.Color.FromArgb(232, 64, 64)), OX + (COLS - 0.5f) * CELL_W - 6, OY + (ROWS - 0.5f) * CELL_H - 6, 12, 12);
            }
            return bmp;
        }
    }
}