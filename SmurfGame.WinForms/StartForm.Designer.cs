namespace SmurfGame.WinForms
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblNamePrompt = new System.Windows.Forms.Label();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.lblCharacterPrompt = new System.Windows.Forms.Label();
            this.rbPapaSmurf = new System.Windows.Forms.RadioButton();
            this.rbStrongSmurf = new System.Windows.Forms.RadioButton();
            this.rbLadySmurf = new System.Windows.Forms.RadioButton();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.pnlCharacterSelection = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.lblTitle.Location = new System.Drawing.Point(120, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(260, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Smurf Village Escape";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(184)))), ((int)(((byte)(106)))));
            this.lblSubtitle.Location = new System.Drawing.Point(60, 55);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(380, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Choose your smurf and enter your name to start";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // 
            // lblNamePrompt
            // 
            this.lblNamePrompt.AutoSize = true;
            this.lblNamePrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.lblNamePrompt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNamePrompt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(200)))), ((int)(((byte)(154)))));
            this.lblNamePrompt.Location = new System.Drawing.Point(30, 90);
            this.lblNamePrompt.Name = "lblNamePrompt";
            this.lblNamePrompt.Size = new System.Drawing.Size(79, 19);
            this.lblNamePrompt.TabIndex = 2;
            this.lblNamePrompt.Text = "Your name";

            // 
            // txtPlayerName
            // 
            this.txtPlayerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(42)))), ((int)(((byte)(13)))));
            this.txtPlayerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPlayerName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPlayerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.txtPlayerName.Location = new System.Drawing.Point(120, 88);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.PlaceholderText = "Enter your name...";
            this.txtPlayerName.Size = new System.Drawing.Size(330, 25);
            this.txtPlayerName.TabIndex = 3;

            // 
            // lblCharacterPrompt
            // 
            this.lblCharacterPrompt.AutoSize = true;
            this.lblCharacterPrompt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.lblCharacterPrompt.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCharacterPrompt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.lblCharacterPrompt.Location = new System.Drawing.Point(30, 130);
            this.lblCharacterPrompt.Name = "lblCharacterPrompt";
            this.lblCharacterPrompt.Size = new System.Drawing.Size(154, 20);
            this.lblCharacterPrompt.TabIndex = 4;
            this.lblCharacterPrompt.Text = "Choose your character:";

            // 
            // rbPapaSmurf
            // 
            this.rbPapaSmurf.AutoSize = true;
            this.rbPapaSmurf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.rbPapaSmurf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbPapaSmurf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.rbPapaSmurf.Location = new System.Drawing.Point(30, 160);
            this.rbPapaSmurf.Name = "rbPapaSmurf";
            this.rbPapaSmurf.Size = new System.Drawing.Size(107, 23);
            this.rbPapaSmurf.TabIndex = 5;
            this.rbPapaSmurf.Text = "Papa Smurf";
            this.rbPapaSmurf.UseVisualStyleBackColor = false;

            // 
            // rbStrongSmurf
            // 
            this.rbStrongSmurf.AutoSize = true;
            this.rbStrongSmurf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.rbStrongSmurf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbStrongSmurf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.rbStrongSmurf.Location = new System.Drawing.Point(180, 160);
            this.rbStrongSmurf.Name = "rbStrongSmurf";
            this.rbStrongSmurf.Size = new System.Drawing.Size(117, 23);
            this.rbStrongSmurf.TabIndex = 6;
            this.rbStrongSmurf.Text = "Strong Smurf";
            this.rbStrongSmurf.UseVisualStyleBackColor = false;

            // 
            // rbLadySmurf
            // 
            this.rbLadySmurf.AutoSize = true;
            this.rbLadySmurf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.rbLadySmurf.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbLadySmurf.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.rbLadySmurf.Location = new System.Drawing.Point(340, 160);
            this.rbLadySmurf.Name = "rbLadySmurf";
            this.rbLadySmurf.Size = new System.Drawing.Size(110, 23);
            this.rbLadySmurf.TabIndex = 7;
            this.rbLadySmurf.Text = "Lady Smurf";
            this.rbLadySmurf.UseVisualStyleBackColor = false;

            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(158)))), ((int)(((byte)(63)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(244)))), ((int)(((byte)(216)))));
            this.btnStart.Location = new System.Drawing.Point(180, 210);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(240, 40);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start Game";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);

            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.lblError.Location = new System.Drawing.Point(60, 260);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 15);
            this.lblError.TabIndex = 9;

            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(61)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(500, 320);
            this.ControlBox = false;
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.rbLadySmurf);
            this.Controls.Add(this.rbStrongSmurf);
            this.Controls.Add(this.rbPapaSmurf);
            this.Controls.Add(this.lblCharacterPrompt);
            this.Controls.Add(this.txtPlayerName);
            this.Controls.Add(this.lblNamePrompt);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smurf Village Escape";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblNamePrompt;
        private System.Windows.Forms.TextBox txtPlayerName;
        private System.Windows.Forms.Label lblCharacterPrompt;
        private System.Windows.Forms.RadioButton rbPapaSmurf;
        private System.Windows.Forms.RadioButton rbStrongSmurf;
        private System.Windows.Forms.RadioButton rbLadySmurf;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Panel pnlCharacterSelection;
    }
}
