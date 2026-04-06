using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmurfGame.WinForms
{
    public partial class NameEntryForm : Form
    {
        // This is where we store the name they type in!
        public string PlayerName { get; private set; } = "Papa Smurf"; // Default backup

        private TextBox txtName;
        private Button btnStart;

        public NameEntryForm()
        {
            InitializeComponent();

            // 1. Setup the popup window
            this.Text = "Welcome to Smurf Village!";
            this.Size = new Size(300, 180);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false; // Hides the red X so they HAVE to click Start!

            // 2. Add the text prompt
            Label lblPrompt = new Label();
            lblPrompt.Text = "Enter your Smurf's name:";
            lblPrompt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPrompt.AutoSize = true;
            lblPrompt.Left = 20;
            lblPrompt.Top = 20;
            this.Controls.Add(lblPrompt);

            // 3. Add the typing box
            txtName = new TextBox();
            txtName.Left = 20;
            txtName.Top = 50;
            txtName.Width = 240;
            txtName.Font = new Font("Segoe UI", 12);
            this.Controls.Add(txtName);

            // 4. Add the Start Button
            btnStart = new Button();
            btnStart.Text = "Start Game";
            btnStart.Left = 160;
            btnStart.Top = 90;
            btnStart.Width = 100;
            btnStart.Height = 30;
            btnStart.BackColor = Color.LightBlue;
            btnStart.Click += BtnStart_Click; // Tells the button what to do
            this.Controls.Add(btnStart);
        }

        // 5. What happens when they click Start
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                PlayerName = txtName.Text;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // --- JUST ADD THIS EMPTY METHOD TO FIX THE ERROR ---
        private void NameEntryForm_Load(object sender, EventArgs e)
        {
            // Leave this completely empty! 
            // It just exists to keep the Visual Studio Designer happy.
        }
    }
}