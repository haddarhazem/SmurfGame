namespace SmurfGame.WinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Show StartForm first
            StartForm startForm = new StartForm();
            DialogResult result = startForm.ShowDialog();

            // If user clicked Start, launch the game with their selection
            if (result == DialogResult.OK)
            {
                string playerName = startForm.PlayerName;
                string smurfType = startForm.SelectedSmurf;
                Application.Run(new Form1(playerName, smurfType));
            }
            // Otherwise, application exits
        }
    }
}
