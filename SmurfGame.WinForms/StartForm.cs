namespace SmurfGame.WinForms
{
    public partial class StartForm : Form
    {
        public string PlayerName { get; private set; } = string.Empty;
        public string SelectedSmurf { get; private set; } = string.Empty;

        public StartForm()
        {
            InitializeComponent();
            this.AcceptButton = btnStart;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            // Validate player name
            string playerName = txtPlayerName.Text.Trim();
            if (string.IsNullOrEmpty(playerName))
            {
                lblError.Text = "Please enter your name.";
                lblError.ForeColor = System.Drawing.Color.FromArgb(232, 112, 112);
                return;
            }

            // Validate character selection
            string selectedSmurf = string.Empty;
            if (rbPapaSmurf.Checked)
                selectedSmurf = "PapaSmurf";
            else if (rbStrongSmurf.Checked)
                selectedSmurf = "StrongSmurf";
            else if (rbLadySmurf.Checked)
                selectedSmurf = "LadySmurf";

            if (string.IsNullOrEmpty(selectedSmurf))
            {
                lblError.Text = "Please select a character.";
                lblError.ForeColor = System.Drawing.Color.FromArgb(232, 112, 112);
                return;
            }

            // All valid - set properties and close
            PlayerName = playerName;
            SelectedSmurf = selectedSmurf;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
