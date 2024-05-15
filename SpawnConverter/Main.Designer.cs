
namespace SpawnConverter
{
    partial class Main
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
            this.LevelLabel = new System.Windows.Forms.Label();
            this.LevelComboBox = new System.Windows.Forms.ComboBox();
            this.GoButton = new System.Windows.Forms.Button();
            this.LevelAllCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // LevelLabel
            // 
            this.LevelLabel.Location = new System.Drawing.Point(15, 15);
            this.LevelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(52, 27);
            this.LevelLabel.TabIndex = 0;
            this.LevelLabel.Text = "Level";
            this.LevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LevelComboBox
            // 
            this.LevelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LevelComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.LevelComboBox.FormattingEnabled = true;
            this.LevelComboBox.Location = new System.Drawing.Point(75, 17);
            this.LevelComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LevelComboBox.Name = "LevelComboBox";
            this.LevelComboBox.Size = new System.Drawing.Size(241, 23);
            this.LevelComboBox.Sorted = true;
            this.LevelComboBox.TabIndex = 1;
            this.LevelComboBox.TabStop = false;
            // 
            // GoButton
            // 
            this.GoButton.Enabled = false;
            this.GoButton.Location = new System.Drawing.Point(13, 63);
            this.GoButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.GoButton.Name = "GoButton";
            this.GoButton.Size = new System.Drawing.Size(371, 27);
            this.GoButton.TabIndex = 4;
            this.GoButton.Text = "Go!";
            this.GoButton.UseVisualStyleBackColor = true;
            this.GoButton.Click += new System.EventHandler(this.GoButton_Click);
            // 
            // LevelAllCheckBox
            // 
            this.LevelAllCheckBox.Enabled = false;
            this.LevelAllCheckBox.Location = new System.Drawing.Point(335, 15);
            this.LevelAllCheckBox.Name = "LevelAllCheckBox";
            this.LevelAllCheckBox.Size = new System.Drawing.Size(48, 27);
            this.LevelAllCheckBox.TabIndex = 5;
            this.LevelAllCheckBox.TabStop = false;
            this.LevelAllCheckBox.Text = "All";
            this.LevelAllCheckBox.UseVisualStyleBackColor = true;
            this.LevelAllCheckBox.CheckedChanged += new System.EventHandler(this.LevelAllCheckBox_CheckedChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 109);
            this.Controls.Add(this.LevelAllCheckBox);
            this.Controls.Add(this.GoButton);
            this.Controls.Add(this.LevelComboBox);
            this.Controls.Add(this.LevelLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SpawnConverter";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.ComboBox LevelComboBox;
        private System.Windows.Forms.Button GoButton;
        private System.Windows.Forms.CheckBox LevelAllCheckBox;
    }
}

