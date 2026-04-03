namespace SmurfGame.WinForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pbPlayer = new PictureBox();
            lblCoordinates = new Label();
            lblHealth = new Label();
            ((System.ComponentModel.ISupportInitialize)pbPlayer).BeginInit();
            SuspendLayout();
            // 
            // pbPlayer
            // 
            pbPlayer.Image = (Image)resources.GetObject("pbPlayer.Image");
            pbPlayer.Location = new Point(119, 91);
            pbPlayer.Name = "pbPlayer";
            pbPlayer.Size = new Size(34, 34);
            pbPlayer.SizeMode = PictureBoxSizeMode.AutoSize;
            pbPlayer.TabIndex = 0;
            pbPlayer.TabStop = false;
            // 
            // lblCoordinates
            // 
            lblCoordinates.AutoSize = true;
            lblCoordinates.ForeColor = SystemColors.ControlText;
            lblCoordinates.Location = new Point(27, 41);
            lblCoordinates.Name = "lblCoordinates";
            lblCoordinates.Size = new Size(50, 20);
            lblCoordinates.TabIndex = 1;
            lblCoordinates.Text = "label1";
            lblCoordinates.Click += label1_Click;
            // 
            // lblHealth
            // 
            lblHealth.AutoSize = true;
            lblHealth.Location = new Point(27, 9);
            lblHealth.Name = "lblHealth";
            lblHealth.Size = new Size(50, 20);
            lblHealth.TabIndex = 2;
            lblHealth.Text = "label1";
            lblHealth.Click += lblHealth_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(782, 553);
            Controls.Add(lblHealth);
            Controls.Add(lblCoordinates);
            Controls.Add(pbPlayer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pbPlayer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbPlayer;
        private Label lblCoordinates;
        private Label lblHealth;
    }
}
