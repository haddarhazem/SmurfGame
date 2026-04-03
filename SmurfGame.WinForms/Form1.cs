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
        private bool isMario = true;

        private Bitmap imgHaut, imgBas, imgGauche, imgDroite;

        // 1. Declare the Timer
        private System.Windows.Forms.Timer autoSaveTimer;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Improve label visibility with a bold monospaced font and solid background
            lblHealth.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            lblHealth.ForeColor = System.Drawing.Color.DarkGreen;
            lblHealth.BackColor = System.Drawing.Color.White;

            lblCoordinates.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            lblCoordinates.ForeColor = System.Drawing.Color.DarkBlue;
            lblCoordinates.BackColor = System.Drawing.Color.White;

            // 1. Ajoutez votre image en arrière-plan (remplacez 'maze' par le nom exact de votre image)
            this.BackgroundImage = Properties.Resources.maze;
            this.BackgroundImageLayout = ImageLayout.Stretch; // Stretch allows the image to scale

            // 2. Ajustez la taille de la zone jouable pour qu'elle corresponde à l'image doublée
            if (this.BackgroundImage != null)
            {
                this.ClientSize = new System.Drawing.Size(this.BackgroundImage.Width * 4, this.BackgroundImage.Height * 4);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ask user for character choice
            DialogResult result = MessageBox.Show("Do you want to play as Mario? (Click 'No' to play as Kratos)", "Character Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            isMario = (result == DialogResult.Yes);

            // Load and cache images, swapping left/right for Kratos
            if (isMario)
            {
                imgHaut = Properties.Resources.mario_haut;
                imgBas = Properties.Resources.mario_bas;
                imgGauche = Properties.Resources.mario_gauche;
                imgDroite = Properties.Resources.mario_droite;
            }
            else
            {
                imgHaut = Properties.Resources.kratos_haut;
                imgBas = Properties.Resources.kratos_bas;
                imgGauche = Properties.Resources.kratos_droite; // Fix: swapped for Kratos
                imgDroite = Properties.Resources.kratos_gauche; // Fix: swapped for Kratos
            }

            // Remove white background permanently from cached images
            imgHaut.MakeTransparent(System.Drawing.Color.White);
            imgBas.MakeTransparent(System.Drawing.Color.White);
            imgGauche.MakeTransparent(System.Drawing.Color.White);
            imgDroite.MakeTransparent(System.Drawing.Color.White);

            // Force same size as Mario
            pbPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            pbPlayer.Size = new System.Drawing.Size(34, 34);
            pbPlayer.BackColor = System.Drawing.Color.Transparent; // Make PictureBox background transparent
            pbPlayer.Image = imgBas;

            // 1. Connect to the database
            db = new SmurfGameContext(new DbContextOptions<SmurfGameContext>());
            db.Database.EnsureCreated();

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
                Name = isMario ? "Mario" : "Kratos",
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
                    if (pbPlayer.Top > 0) { pbPlayer.Top -= speed; pbPlayer.Image = imgHaut; }
                    break;
                case Keys.S:
                    if (pbPlayer.Bottom < this.ClientSize.Height) { pbPlayer.Top += speed; pbPlayer.Image = imgBas; }
                    break;
                case Keys.Q:
                    if (pbPlayer.Left > 0) { pbPlayer.Left -= speed; pbPlayer.Image = imgGauche; }
                    break;
                case Keys.D:
                    if (pbPlayer.Right < this.ClientSize.Width) { pbPlayer.Left += speed; pbPlayer.Image = imgDroite; }
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

        private void lblHealth_Click(object sender, EventArgs e)
        {

        }
    }
}