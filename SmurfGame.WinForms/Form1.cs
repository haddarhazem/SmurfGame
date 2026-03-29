using System;
using System.Windows.Forms;
using SmurfGame.BL.Entities;
using SmurfGame.DAL;
using Microsoft.EntityFrameworkCore;

namespace SmurfGame.WinForms
{
    public partial class Form1 : Form
    {
        int speed = 10;
        private Smurf currentSmurf;
        private SmurfGameContext db;

        // 1. Declare the Timer
        private System.Windows.Forms.Timer autoSaveTimer;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Connect to the database
            db = new SmurfGameContext(new DbContextOptions<SmurfGameContext>());

            // --- NEW: DELETE OLD DATA ---
            // Grab all old Smurfs from previous games
            var oldSmurfs = db.Smurfs.ToList();

            // If there are any, delete them from the database
            if (oldSmurfs.Count > 0)
            {
                db.Smurfs.RemoveRange(oldSmurfs);
                db.SaveChanges(); // Push the delete to SQL
            }
            // ----------------------------

            // 2. Create the fresh Smurf for this session
            currentSmurf = new Smurf
            {
                Name = "Papa Smurf",
                Health = 100,
                MaxHealth = 100,
                Level = 1,
                IsInForest = true,
                X = pbPlayer.Left,
                Y = pbPlayer.Top
            };

            // 3. Save the new Smurf so it gets an ID
            db.Smurfs.Add(currentSmurf);
            db.SaveChanges();

            this.Text = $"Smurf ID: {currentSmurf.Id} - Ready to play!";

            lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";

            // 4. Start the Auto-Save Timer
            autoSaveTimer = new System.Windows.Forms.Timer();
            autoSaveTimer.Interval = 1000;
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
            autoSaveTimer.Start();

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
            // (Keep your exact movement switch statement here)
            switch (e.KeyCode)
            {
                case Keys.Z:
                    if (pbPlayer.Top > 0) { pbPlayer.Top -= speed; pbPlayer.Image = Properties.Resources.mario_haut; }
                    break;
                case Keys.S:
                    if (pbPlayer.Bottom < this.ClientSize.Height) { pbPlayer.Top += speed; pbPlayer.Image = Properties.Resources.mario_bas; }
                    break;
                case Keys.Q:
                    if (pbPlayer.Left > 0) { pbPlayer.Left -= speed; pbPlayer.Image = Properties.Resources.mario_gauche; }
                    break;
                case Keys.D:
                    if (pbPlayer.Right < this.ClientSize.Width) { pbPlayer.Left += speed; pbPlayer.Image = Properties.Resources.mario_droite; }
                    break;
            }

            // Update the object's coordinates in memory (The timer will save them to the DB)
            if (currentSmurf != null)
            {
                currentSmurf.X = pbPlayer.Left;
                currentSmurf.Y = pbPlayer.Top;

                lblCoordinates.Text = $"X: {currentSmurf.X} | Y: {currentSmurf.Y}";
            }
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}