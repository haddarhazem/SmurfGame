using SmurfGame.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SmurfGame.WinForms
{
    public partial class DashboardForm : Form
    {
        public DashboardForm()
        {
            InitializeComponent();

            // 1. Setup the Window
            this.Text = "Smurf Village Leaderboard";
            this.Size = new System.Drawing.Size(400, 450);
            this.StartPosition = FormStartPosition.CenterScreen;

            // 2. Create the Data Grid
            DataGridView dgvScores = new DataGridView();
            dgvScores.Dock = DockStyle.Fill;
            dgvScores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvScores.AllowUserToAddRows = false;
            dgvScores.ReadOnly = true;
            dgvScores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // --- THE FIX: MANUALLY CREATE THE COLUMNS ---
            dgvScores.Columns.Add("ColId", "Smurf ID");
            dgvScores.Columns.Add("ColName", "Player Name");
            dgvScores.Columns.Add("ColTime", "Best Time (Seconds)");

            this.Controls.Add(dgvScores);

            // 3. Fetch the Data manually
            try
            {
                var optionsBuilder = new DbContextOptionsBuilder<SmurfGameContext>();
                optionsBuilder.UseSqlServer(
                    @"server=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB;Integrated Security=true"
                );

                using (var db = new SmurfGameContext(optionsBuilder.Options))
                {
                    // Grab the raw Smurfs from the database, ordered by best time
                    var topScores = db.Smurfs
                        .Where(s => s.BestTime != null)
                        .OrderBy(s => s.BestTime)
                        .ToList();

                    // --- THE FIX: MANUALLY PUSH THE ROWS ---
                    foreach (var smurf in topScores)
                    {
                        dgvScores.Rows.Add(smurf.Id, smurf.Name, smurf.BestTime);
                    }

                    // A little safety check so we know if the database is actually empty!
                    if (topScores.Count == 0)
                    {
                        MessageBox.Show("The leaderboard is ready! No winning scores yet. Start playing to see scores!", "Welcome!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error: " + ex.Message, "Error Loading Leaderboard");
            }
        }
    }
}