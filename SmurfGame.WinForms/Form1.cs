using Microsoft.EntityFrameworkCore;
using SmurfGame.BL.Entities;
using SmurfGame.DAL;
using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace SmurfGame.WinForms
{
    public partial class Form1 : Form
    {
        private Smurf currentSmurf;
        private SmurfGameContext db;
        private Random rand = new Random();
        // 1. Declare the Timer
        private System.Windows.Forms.Timer autoSaveTimer;

        // invisible walls
        private List<PictureBox> obstacles = new List<PictureBox>();
        private List<PictureBox> pbCoinsList = new List<PictureBox>();
        private List<Coin> currentCoinsList = new List<Coin>();

        private System.Windows.Forms.Timer scoreTimer;
        private int secondsElapsed = 0;
        private Label lblScoreTimer;

        // --- COIN & AZRAEL ANIMATION TIMER ---
        private System.Windows.Forms.Timer entityAnimationTimer;
        private Image[] azraelFrames;
        private int currentAzraelFrame = 0;
        private Image[] coinFrames;
        private int currentCoinFrame = 0;

        // --- SMURF CONTROL ---
        private SmurfControl smurfControl;

        private PictureBox pbBluePotion;
        private BluePotion currentBluePotion;

        private PictureBox pbSpeedBuff;
        private SpeedBuff currentSpeedBuff;
        private System.Windows.Forms.Timer buffTimer;
        private int activeSpeedBoost = 0;

        // speed buff COUNTDOWN variables
        private int buffTimeRemaining = 0;
        private Label lblBuffTime;

        // --- AZRAEL VARIABLES ---
        private PictureBox pbAzrael;
        private Azrael currentAzrael;

        // --- CHAT BOX VARIABLES ---
        private PictureBox pbChatBox;
        private Label lblChatText;
        private bool hasSeenBuffChat = false; // Remembers if he's seen it before
        private bool isGamePaused = false;    // Stops movement/enemies while reading

        // --Health Bar Variables--
        private PictureBox pbHealthBar;
        private PictureBox pbPlayerIcon;
        private Label lblHPDisplay;  // NEW: Display current HP/Max HP
        private Label lblDamage;  // NEW: Display damage taken

        // --- SCORE & COIN VARIABLES ---
        private int score = 0;
        private Label lblScore;

        private PictureBox pbCoin;
        private Coin currentCoin;

        // --- GAME STATE VARIABLES ---
        private bool isGameOver = false;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private Point GetSafeSpawnLocation()
        {
            int randomX, randomY;
            Rectangle spawnZone;
            bool isSafeSpot;

            // 1. Reduced padding so items can spawn much closer together!
            int padding = 15;
            int itemSize = 25; // Your PictureBox width/height

            // 2. SAFETY NET: Prevent the game from freezing in an infinite loop
            int maxAttempts = 100;
            int currentAttempt = 0;

            do
            {
                currentAttempt++;
                isSafeSpot = true;

                // Roll the dice for coordinates
                randomX = rand.Next(50, this.ClientSize.Width - 50);
                randomY = rand.Next(50, this.ClientSize.Height - 50);

                // Create an invisible boundary around these coordinates
                spawnZone = new Rectangle(
                    randomX - padding,
                    randomY - padding,
                    itemSize + (padding * 2),
                    itemSize + (padding * 2)
                );

                // Check the Player
                if (pbPlayer != null && pbPlayer.Visible && spawnZone.IntersectsWith(pbPlayer.Bounds))
                    isSafeSpot = false;

                // Check the Blue Potion
                if (pbBluePotion != null && pbBluePotion.Visible && spawnZone.IntersectsWith(pbBluePotion.Bounds))
                    isSafeSpot = false;

                // Check the Speed Buff
                if (pbSpeedBuff != null && pbSpeedBuff.Visible && spawnZone.IntersectsWith(pbSpeedBuff.Bounds))
                    isSafeSpot = false;

                // Check Azrael
                if (pbAzrael != null && pbAzrael.Visible && spawnZone.IntersectsWith(pbAzrael.Bounds))
                    isSafeSpot = false;

                // --- FIX: Check the LIST of coins, not just the old single variable! ---
                foreach (PictureBox existingCoin in pbCoinsList)
                {
                    // If the spot touches a coin we already placed on the map, reroll!
                    if (existingCoin.Visible && spawnZone.IntersectsWith(existingCoin.Bounds))
                    {
                        isSafeSpot = false;
                        break;
                    }
                }

                // Check all map obstacles (Trees/Cliffs)
                foreach (PictureBox wall in obstacles)
                {
                    if (spawnZone.IntersectsWith(wall.Bounds))
                    {
                        isSafeSpot = false;
                        break; // Stop checking this spot, we already know it's bad
                    }
                }

                // --- SAFETY NET ACTIVATION ---
                // If the map is so crowded that we failed 100 times, just break out 
                // and place it here anyway so the game doesn't crash/freeze.
                if (currentAttempt >= maxAttempts)
                {
                    break;
                }

            } while (isSafeSpot == false); // Keep looping until it finds an empty spot!

            // Once it escapes the loop, return the winning coordinates
            return new Point(randomX, randomY);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // IMMEDIATELY hide all designer picture boxes except the ones we need
                // This prevents visual clutter and duplicate rendering
                foreach (Control control in this.Controls)
                {
                    if (control is PictureBox pb)
                    {
                        // Keep only the picture boxes with the "obstacle" tag visible
                        if (pb.Tag == null || pb.Tag.ToString() != "obstacle")
                        {
                            pb.Visible = false;
                        }
                    }
                }

                string chosenName = "Papa Smurf"; // Fallback name

                using (NameEntryForm nameForm = new NameEntryForm())
                {
                    // Pause the game and show the popup. Wait for them to click OK!
                    if (nameForm.ShowDialog() == DialogResult.OK)
                    {
                        chosenName = nameForm.PlayerName; // Grab what they typed
                    }
                }

                System.Diagnostics.Debug.WriteLine("? Name entry complete: " + chosenName);

                /**/

                //  Connect to the database
                try
                {
                    var optionsBuilder = new DbContextOptionsBuilder<SmurfGameContext>();
                    optionsBuilder.UseSqlServer(
                        @"server=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB;Integrated Security=true"
                    );

                    db = new SmurfGameContext(optionsBuilder.Options);

                    System.Diagnostics.Debug.WriteLine("? Database context created");

                    // IMPORTANT: Only create the database if it doesn't exist - DO NOT delete it!
                    // This preserves all player scores from previous games
                    db.Database.EnsureCreated();
                    System.Diagnostics.Debug.WriteLine("? Database created (if needed)");

                    // --- MAP CLEANUP (PROTECTS SCORES) ---
                    // Only delete game items, NOT player data!
                    // This grabs all leftover items from the last game and deletes them.
                    // Notice we do NOT touch db.Smurfs, so your leaderboard is perfectly safe!
                    db.Coins.RemoveRange(db.Coins.ToList());
                    db.BluePotions.RemoveRange(db.BluePotions.ToList());
                    db.SpeedBuffs.RemoveRange(db.SpeedBuffs.ToList());
                    db.Azraels.RemoveRange(db.Azraels.ToList());

                    // Push the cleanup to SQL Server before we spawn the new items
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("? Database cleanup complete");
                    // -------------------------------------
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("? Database Error: " + ex.Message);
                    MessageBox.Show(
                        $"Database Connection Error:\n\n{ex.Message}\n\n" +
                        $"Inner Exception: {ex.InnerException?.Message}\n\n" +
                        $"Stack Trace: {ex.StackTrace}",
                        "Database Error"
                    );
                    this.Close();
                    return;
                }

                // 2. Create the fresh Smurf for this session
                currentSmurf = new Smurf
                {
                    Name = chosenName,
                    Health = 100,
                    MaxHealth = 100,
                    Level = 1,
                    IsInForest = true,
                    X = 62,
                    Y = 154
                };

                // 3. Save the new Smurf so it gets an ID
                db.Smurfs.Add(currentSmurf);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("? Smurf created with ID: " + currentSmurf.Id);

                this.Text = $"Smurf ID: {currentSmurf.Id} - Ready to play!";

                // --- CREATE SMURF CONTROL ---
                smurfControl = new SmurfControl();
                smurfControl.CurrentSmurf = currentSmurf;
                smurfControl.Left = currentSmurf.X;
                smurfControl.Top = currentSmurf.Y;
                smurfControl.Width = 30;  // Reduced from 40 to fit through paths
                smurfControl.Height = 30; // Reduced from 40 to fit through paths
                smurfControl.BackColor = Color.Transparent;
                this.Controls.Add(smurfControl);
                smurfControl.BringToFront();

                System.Diagnostics.Debug.WriteLine("? SmurfControl added to form");

                lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";

                // 4. Start the Auto-Save Timer
                autoSaveTimer = new System.Windows.Forms.Timer();
                autoSaveTimer.Interval = 1000;
                autoSaveTimer.Tick += AutoSaveTimer_Tick;
                autoSaveTimer.Start();

                // 1. Create the visual countdown label
                lblBuffTime = new Label();
                lblBuffTime.AutoSize = true;
                lblBuffTime.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                lblBuffTime.ForeColor = Color.Orange;
                lblBuffTime.BackColor = Color.Transparent;
                lblBuffTime.Left = 600;
                lblBuffTime.Top = 40; // Place it right under your Health/Coordinate labels
                lblBuffTime.Visible = false; // Keep it hidden until he eats a buff
                this.Controls.Add(lblBuffTime);

                // 1. Create the Chat Box Background
                pbChatBox = new PictureBox();
                pbChatBox.Image = Properties.Resources.chat_box;
                pbChatBox.Width = 400; // Adjust based on your image size
                pbChatBox.Height = 150;
                pbChatBox.SizeMode = PictureBoxSizeMode.StretchImage;

                // Center it near the bottom of the screen
                pbChatBox.Left = (this.ClientSize.Width - pbChatBox.Width) / 2;
                pbChatBox.Top = this.ClientSize.Height - pbChatBox.Height - 50;
                pbChatBox.Visible = false; // Hide it to start
                this.Controls.Add(pbChatBox);

                // 2. Create the Text inside the Chat Box
                lblChatText = new Label();
                lblChatText.Text = "Whoa! I feel so fast!\nI can definitely outrun Azrael now!\n\n(Press SPACE to continue)";
                lblChatText.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblChatText.ForeColor = Color.White;
                lblChatText.AutoSize = true;
                lblChatText.Left = 20;
                lblChatText.Top = 20;

                // WINFORMS MAGIC TRICK: Set the Parent to the PictureBox so the background is truly transparent!
                lblChatText.Parent = pbChatBox;
                lblChatText.BackColor = Color.Transparent;
                lblChatText.BringToFront();

                // healthbar picture box (RIGHT SIDE)
                pbHealthBar = new PictureBox();
                pbHealthBar.Width = 150;
                pbHealthBar.Height = 30;
                pbHealthBar.SizeMode = PictureBoxSizeMode.StretchImage;
                pbHealthBar.BackColor = Color.Transparent;
                pbHealthBar.Left = this.ClientSize.Width - 170;  // Right side with padding
                pbHealthBar.Top = 10;
                this.Controls.Add(pbHealthBar);
                pbHealthBar.BringToFront();

                // NEW: Health text display (shows HP/MaxHP)
                lblHPDisplay = new Label();
                lblHPDisplay.AutoSize = true;
                lblHPDisplay.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblHPDisplay.ForeColor = Color.White;
                lblHPDisplay.BackColor = Color.Transparent;
                lblHPDisplay.Text = "HP: 100/100";
                lblHPDisplay.Left = pbHealthBar.Left;
                lblHPDisplay.Top = pbHealthBar.Bottom + 2;
                this.Controls.Add(lblHPDisplay);
                lblHPDisplay.BringToFront();

                // NEW: Damage indicator label (appears when hit)
                lblDamage = new Label();
                lblDamage.AutoSize = true;
                lblDamage.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                lblDamage.ForeColor = Color.Red;
                lblDamage.BackColor = Color.Transparent;
                lblDamage.Text = "";
                lblDamage.Left = pbHealthBar.Left + 20;
                lblDamage.Top = pbHealthBar.Top + 50;
                lblDamage.Visible = false;
                this.Controls.Add(lblDamage);
                lblDamage.BringToFront();

                // --- ADD THE PLAYER PORTRAIT ---
                pbPlayerIcon = new PictureBox();
                pbPlayerIcon.Width = 60;
                pbPlayerIcon.Height = 60;
                pbPlayerIcon.SizeMode = PictureBoxSizeMode.Zoom;
                pbPlayerIcon.BackColor = Color.Transparent;

                pbPlayerIcon.Image = Properties.Resources.playericon;

                // Position it to the left of the health bar
                pbPlayerIcon.Left = pbHealthBar.Left - 70;
                pbPlayerIcon.Top = pbHealthBar.Top - 5;

                this.Controls.Add(pbPlayerIcon);
                pbPlayerIcon.BringToFront();

                // 1. Create the Time Trial UI (RIGHT SIDE)
                lblScoreTimer = new Label();
                lblScoreTimer.AutoSize = true;
                lblScoreTimer.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblScoreTimer.ForeColor = Color.Gold;
                lblScoreTimer.BackColor = Color.Transparent;
                lblScoreTimer.Text = "Time: 0s";

                lblScoreTimer.Left = this.ClientSize.Width - 170;
                lblScoreTimer.Top = 50;

                this.Controls.Add(lblScoreTimer);
                lblScoreTimer.BringToFront();

                // Create the Score Label (for coins collected) (RIGHT SIDE)
                lblScore = new Label();
                lblScore.AutoSize = true;
                lblScore.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                lblScore.ForeColor = Color.Yellow;
                lblScore.BackColor = Color.Transparent;
                lblScore.Text = "Coins: 0/6";

                lblScore.Left = this.ClientSize.Width - 170;
                lblScore.Top = lblScoreTimer.Bottom + 2;

                this.Controls.Add(lblScore);
                lblScore.BringToFront();

                System.Diagnostics.Debug.WriteLine("? UI elements created");

                // 2. Setup the Stopwatch Timer
                scoreTimer = new System.Windows.Forms.Timer();
                scoreTimer.Interval = 1000; // Ticks every 1 second
                scoreTimer.Tick += ScoreTimer_Tick;
                scoreTimer.Start();

                // -----------------------------

                // 2. Setup the Timer to tick EVERY 1 SECOND
                buffTimer = new System.Windows.Forms.Timer();
                buffTimer.Interval = 1000;
                buffTimer.Tick += BuffTimer_Tick;

                UpdateHealthBar();

                // Load the animated coin frames!
                coinFrames = new Image[]
                {
                    Properties.Resources.coin0,
                    Properties.Resources.coin1,
                    Properties.Resources.coin2,
                    Properties.Resources.coin3,
                    Properties.Resources.coin4
                };

                // Load Azrael animation frames!
                azraelFrames = new Image[] { 
                    Properties.Resources.azrael0, Properties.Resources.azrael1,
                    Properties.Resources.azrael2, Properties.Resources.azrael3,
                    Properties.Resources.azrael4
                };

                // Setup entity animation timer for coins and Azrael
                entityAnimationTimer = new System.Windows.Forms.Timer();
                entityAnimationTimer.Interval = 150;
                entityAnimationTimer.Tick += EntityAnimationTimer_Tick;
                entityAnimationTimer.Start();

                System.Diagnostics.Debug.WriteLine("? Animation frames and timers loaded");

                // Scan the form for our red hitboxes
                foreach (Control x in this.Controls)
                {
                    if (x is PictureBox && x.Tag != null && x.Tag.ToString() == "obstacle")
                    {
                        obstacles.Add((PictureBox)x);

                        // --- ADD THIS LINE ---
                        x.Visible = false; // Hides the red box, but its collision math still works perfectly!
                    }
                }

                System.Diagnostics.Debug.WriteLine("? Obstacles found: " + obstacles.Count);

                // Pass obstacles to the Smurf Control
                smurfControl.SetObstacles(obstacles);

                SpawnBluePotion();
                System.Diagnostics.Debug.WriteLine("? Blue potion spawned");

                SpawnSpeedBuff();
                System.Diagnostics.Debug.WriteLine("? Speed buff spawned");

                SpawnAzrael();
                System.Diagnostics.Debug.WriteLine("? Azrael spawned");

                SpawnAllCoins();
                System.Diagnostics.Debug.WriteLine("? Coins spawned");

                System.Diagnostics.Debug.WriteLine("??? FORM LOAD COMPLETE - GAME READY ???");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("??? CRITICAL ERROR IN Form1_Load: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);

                // Get the real inner exception for database errors
                string innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception";
                if (ex.InnerException?.InnerException != null)
                {
                    innerExceptionMessage += "\n\nDeep Inner: " + ex.InnerException.InnerException.Message;
                }

                System.Diagnostics.Debug.WriteLine("Inner Exception: " + innerExceptionMessage);

                MessageBox.Show(
                    $"Critical Error in Form1_Load:\n\n{ex.Message}\n\n" +
                    $"Inner Exception:\n{innerExceptionMessage}\n\n" +
                    $"Stack Trace: {ex.StackTrace}",
                    "Critical Error"
                );
                this.Close();
            }
        }

        // Animates coins and Azrael
        private void EntityAnimationTimer_Tick(object sender, EventArgs e)
        {
            // --- AZRAEL IDLE ANIMATION ---
            if (pbAzrael != null && pbAzrael.Visible)
            {
                pbAzrael.Image = azraelFrames[currentAzraelFrame];
                currentAzraelFrame++;

                // Loop back to the first frame if we reach the end
                if (currentAzraelFrame >= azraelFrames.Length)
                {
                    currentAzraelFrame = 0;
                }
            }

            // --- COIN ANIMATION ---
            // 1. Advance to the next frame
            currentCoinFrame++;
            if (currentCoinFrame >= coinFrames.Length)
            {
                currentCoinFrame = 0; // Loop back to the start
            }

            // 2. Apply the new frame to every coin currently sitting on the map
            foreach (PictureBox coin in pbCoinsList)
            {
                if (coin.Visible)
                {
                    coin.Image = coinFrames[currentCoinFrame];
                }
            }
        }

        private void BuffTimer_Tick(object sender, EventArgs e)
        {
            // 1. Subtract a second
            buffTimeRemaining--;

            if (buffTimeRemaining > 0)
            {
                // 2. Still have time left! Update the screen.
                lblBuffTime.Text = $"Speed Buff: {buffTimeRemaining}s";
            }
            else
            {
                // 3. Time's up! (0 seconds left)
                buffTimer.Stop();

                smurfControl.Speed -= activeSpeedBoost; // Take away the speed
                activeSpeedBoost = 0;      // Reset the tracker

                lblBuffTime.Visible = false; // Hide the label from the screen
            }
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
            if (isGameOver)
            {
                return; // The game is over! Ignore all key presses!
            }

            // --- CHAT BOX LOCK ---
            if (isGamePaused)
            {
                // If the game is paused and they press Space, dismiss the chat!
                if (e.KeyCode == Keys.Space)
                {
                    pbChatBox.Visible = false; // Hide chat
                    isGamePaused = false;      // Unfreeze game

                    // Resume the Smurf's animation!
                    smurfControl.ResumeAnimation();
                    if (activeSpeedBoost > 0) buffTimer.Start();
                }

                // Return immediately so the rest of the movement code below doesn't run!
                return;
            }

            // Movement controls
            switch (e.KeyCode)
            {
                case Keys.Z:
                    smurfControl.MoveUp();
                    break;
                case Keys.S:
                    smurfControl.MoveDown();
                    break;
                case Keys.Q:
                    smurfControl.MoveLeft();
                    break;
                case Keys.D:
                    smurfControl.MoveRight();
                    break;
            }

            // Update the object's coordinates in memory (The timer will save them to the DB)
            if (currentSmurf != null)
            {
                Point smurfPos = smurfControl.GetPosition();
                currentSmurf.X = smurfPos.X;
                currentSmurf.Y = smurfPos.Y;

                lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";
            }

            CheckCollisions();
        }

        private void SpawnBluePotion()
        {
            Point spawnPoint = GetSafeSpawnLocation();

            currentBluePotion = new BluePotion
            {
                HealthBoost = 25,
                IsConsumed = false,
                X = spawnPoint.X,
                Y = spawnPoint.Y
            };

            if (db != null)
            {
                db.BluePotions.Add(currentBluePotion);
                db.SaveChanges();
            }

            pbBluePotion = new PictureBox();
            pbBluePotion.Width = 30;
            pbBluePotion.Height = 30;
            pbBluePotion.Left = currentBluePotion.X;
            pbBluePotion.Top = currentBluePotion.Y;
            pbBluePotion.SizeMode = PictureBoxSizeMode.Zoom;
            pbBluePotion.BackColor = Color.Transparent;

            pbBluePotion.Image = Properties.Resources.bluePotion; // Make sure to add a blue potion image to your resources
            this.Controls.Add(pbBluePotion);
        }

        private void SpawnAzrael()
        {
            Point safeSpot = GetSafeSpawnLocation();

            currentAzrael = new Azrael
            {
                Name = "Azrael",
                Health = 100,
                MaxHealth = 100,
                Damage = 60,
                X = safeSpot.X,
                Y = safeSpot.Y
            };

            if (db != null)
            {
                db.Azraels.Add(currentAzrael);
                db.SaveChanges();
            }

            if (pbAzrael == null) // Only create the PictureBox if it doesn't exist yet
            {
                pbAzrael = new PictureBox();
                pbAzrael.Width = 60; // Azrael might need to be slightly bigger than an item
                pbAzrael.Height = 60;
                pbAzrael.SizeMode = PictureBoxSizeMode.Zoom;
                pbAzrael.BackColor = Color.Transparent;
                this.Controls.Add(pbAzrael);
            }

            pbAzrael.Left = currentAzrael.X;
            pbAzrael.Top = currentAzrael.Y;
            pbAzrael.Visible = true;
            pbAzrael.BringToFront();
        }
        private void SpawnSpeedBuff()
        {
            Point spawnPoint = GetSafeSpawnLocation();

            currentSpeedBuff = new SpeedBuff
            {
                SpeedBoostAmount = 5, // Gives +5 speed
                HealthBoost = 0,      // Doesn't heal
                IsConsumed = false,
                X = spawnPoint.X,
                Y = spawnPoint.Y
            };

            if (db != null)
            {
                db.SpeedBuffs.Add(currentSpeedBuff);
                db.SaveChanges();
            }

            pbSpeedBuff = new PictureBox();
            pbSpeedBuff.Width = 30;
            pbSpeedBuff.Height = 30;
            pbSpeedBuff.Left = currentSpeedBuff.X;
            pbSpeedBuff.Top = currentSpeedBuff.Y;
            pbSpeedBuff.SizeMode = PictureBoxSizeMode.Zoom;

            pbSpeedBuff.Image = Properties.Resources.speedPotion; 

            this.Controls.Add(pbSpeedBuff);
            pbSpeedBuff.BringToFront();
        }

        private void CheckCollisions()
        {
            if (pbBluePotion != null && pbBluePotion.Visible && smurfControl.GetBounds().IntersectsWith(pbBluePotion.Bounds))
            {
                pbBluePotion.Visible = false; // Hide the potion
                currentBluePotion.IsConsumed = true; // Mark it as consumed in the database

                double healPercentage = currentBluePotion.HealthBoost / 100.0;
                int healAmount = (int)(currentSmurf.MaxHealth * healPercentage);
                currentSmurf.Health += healAmount;

                if (currentSmurf.Health > currentSmurf.MaxHealth)
                {
                    currentSmurf.Health = currentSmurf.MaxHealth; // Cap health at max
                }

                UpdateHealthBar();

                SpawnBluePotion();
            }

            // --- SPEED BUFF COLLISION ---
            if (pbSpeedBuff != null && pbSpeedBuff.Visible && smurfControl.GetBounds().IntersectsWith(pbSpeedBuff.Bounds))
            {
                // 1. Hide the item and update database
                pbSpeedBuff.Visible = false;
                currentSpeedBuff.IsConsumed = true;

                // 2. Apply Speed Boost (if he doesn't already have it)
                if (activeSpeedBoost == 0)
                {
                    activeSpeedBoost = currentSpeedBuff.SpeedBoostAmount;
                    smurfControl.Speed += activeSpeedBoost;
                }

                // 3. Reset the 10-second timer and UI label
                buffTimeRemaining = 10;
                lblBuffTime.Text = $"Speed Buff: {buffTimeRemaining}s";
                lblBuffTime.Visible = true;
                buffTimer.Start();

                // 4. --- FIRST TIME CHAT LOGIC ---
                if (hasSeenBuffChat == false)
                {
                    hasSeenBuffChat = true; // Make sure it only happens once
                    isGamePaused = true;    // Freeze the player's movement

                    // Show the Dialog Box
                    pbChatBox.Visible = true;
                    pbChatBox.BringToFront();

                    // Stop timers so Azrael and the buff clock pause while reading!
                    buffTimer.Stop();
                }

                // 5. Spawn a new buff somewhere else on the map!
                SpawnSpeedBuff();
            }

            // --- AZRAEL COLLISION (ENEMY) ---
            if (pbAzrael != null && pbAzrael.Visible && smurfControl.GetBounds().IntersectsWith(pbAzrael.Bounds))
            {
                // 1. Take damage!
                int damageAmount = currentAzrael.Damage;
                currentSmurf.Health -= damageAmount;

                // 2. Show damage indicator
                if (lblDamage != null)
                {
                    lblDamage.Text = $"- {damageAmount} HP!";
                    lblDamage.Visible = true;

                    // Hide it after 2 seconds
                    var damageTimer = new System.Windows.Forms.Timer();
                    damageTimer.Interval = 2000;
                    damageTimer.Tick += (s, e) =>
                    {
                        lblDamage.Visible = false;
                        damageTimer.Stop();
                        damageTimer.Dispose();
                    };
                    damageTimer.Start();
                }

                // 3. Check if Papa Smurf died
                if (currentSmurf.Health <= 0)
                {
                    currentSmurf.Health = 0;
                    UpdateHealthBar();

                    // Stop everything
                    MessageBox.Show("Oh no! Azrael caught you! Game Over.", "Defeat");

                    // Close the form to end the application
                    this.Close(); 
                }
                else
                {
                    // He survived! Update the health bar
                    UpdateHealthBar();

                    // Teleport Azrael to a new random safe spot so he doesn't keep hitting the player
                    SpawnAzrael();
                }
            }

            // --- COIN TIME TRIAL COLLISION ---
            // We loop BACKWARDS so we can safely delete coins from the list while checking them
            for (int i = pbCoinsList.Count - 1; i >= 0; i--)
            {
                PictureBox currentPbCoin = pbCoinsList[i];
                Coin currentDbCoin = currentCoinsList[i];

                if (currentPbCoin.Visible && smurfControl.GetBounds().IntersectsWith(currentPbCoin.Bounds))
                {
                    // 1. Hide it from screen and mark in DB
                    currentPbCoin.Visible = false;
                    currentDbCoin.IsConsumed = true;

                    // 2. Remove it from our active lists
                    pbCoinsList.RemoveAt(i);
                    currentCoinsList.RemoveAt(i);

                    // 3. Update the score display
                    if (lblScore != null)
                    {
                        int coinsRemaining = pbCoinsList.Count;
                        lblScore.Text = $"Coins: {6 - coinsRemaining}/6";
                    }

                    // 4. Check if that was the last coin!
                    if (pbCoinsList.Count == 0)
                    {
                        scoreTimer.Stop(); // Stop the clock!

                        // Freeze the game and show the victory score
                        isGamePaused = true;

                        // Save the fastest time to the database
                        if (currentSmurf.BestTime == null || secondsElapsed < currentSmurf.BestTime)
                        {
                            currentSmurf.BestTime = secondsElapsed;
                            db.SaveChanges();
                        }

                        MessageBox.Show($"You collected all 6 coins in {secondsElapsed} seconds!", "Victory!");

                        DashboardForm  dashboard = new DashboardForm();
                        dashboard.ShowDialog();

                        // Optional: Reset the game here if you want them to play again!
                    }
                }
            }
        }
        private void UpdateHealthBar()
        {
            if (currentSmurf != null && pbHealthBar != null)
            {
                // 1. Calculate the exact percentage (e.g., 68.5%)
                double percent = ((double)currentSmurf.Health / currentSmurf.MaxHealth) * 100;

                // Prevent math errors if health drops below 0 somehow
                if (percent < 0) percent = 0;

                // 2. Ask our helper method for the correct image and apply it!
                pbHealthBar.Image = GetClosestHealthImage(percent);

                // 3. Update the HP text display
                if (lblHPDisplay != null)
                {
                    lblHPDisplay.Text = $"HP: {currentSmurf.Health}/{currentSmurf.MaxHealth}";
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (autoSaveTimer != null)
            {
                autoSaveTimer.Stop(); // Stop the timer so it doesn't crash while closing
            }

            if (entityAnimationTimer != null)
            {
                entityAnimationTimer.Stop();
            }

            if (smurfControl != null)
            {
                smurfControl.Dispose();
            }

            if (db != null)
            {
                db.SaveChanges(); // One final save
                db.Dispose();
            }
        }

        private Image GetClosestHealthImage(double percentage)
        {
            // Find the closest image using mathematical midpoints
            if (percentage >= 92.5) return Properties.Resources.percent100;
            if (percentage >= 77.5) return Properties.Resources.percent85;
            if (percentage >= 60.0) return Properties.Resources.percent70;
            if (percentage >= 40.0) return Properties.Resources.percent50;
            if (percentage >= 25.0) return Properties.Resources.percent30;
            if (percentage >= 15.0) return Properties.Resources.percent20;

            // If it's below 15%, show the 10% bar so they know they are almost dead!
            return Properties.Resources.percent10;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            // SmurfControl handles animation internally
        }

        private void SpawnAllCoins()
        {
            for (int i = 0; i < 6; i++)
            {
                // Get a location (keeps them out of the solid trees)
                Point safeSpot = GetSafeSpawnLocation();

                // Create database entity
                Coin newCoin = new Coin
                {
                    Points = 10,
                    X = safeSpot.X,
                    Y = safeSpot.Y
                };

                if (db != null)
                {
                    db.Coins.Add(newCoin);
                    db.SaveChanges();
                }

                // Create PictureBox
                PictureBox pbNewCoin = new PictureBox();
                pbNewCoin.Width = 30;  // Increased to 30 to give the bubble room
                pbNewCoin.Height = 30; // Increased to 30
                pbNewCoin.SizeMode = PictureBoxSizeMode.Zoom;
                pbNewCoin.BackColor = Color.Transparent;

                // --- NEW: Set the image to the first frame of your bubble animation! ---
                pbNewCoin.Image = coinFrames[0];

                pbNewCoin.Left = newCoin.X;
                pbNewCoin.Top = newCoin.Y;
                pbNewCoin.Visible = true;

                this.Controls.Add(pbNewCoin);
                pbNewCoin.BringToFront();

                // Add them to our tracking lists!
                pbCoinsList.Add(pbNewCoin);
                currentCoinsList.Add(newCoin);
            }
        }

        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            lblScoreTimer.Text = $"Time: {secondsElapsed}s";
        }
    }
}