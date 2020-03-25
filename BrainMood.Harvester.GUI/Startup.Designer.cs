namespace BrainMood.Harvester.GUI
{
    partial class Startup
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
            this.label1 = new System.Windows.Forms.Label();
            this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.samplesPerEmotionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timePerSampleTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.breakTimeTextBox = new System.Windows.Forms.TextBox();
            this.startButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dataDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output directory:";
            // 
            // outputDirectoryTextBox
            // 
            this.outputDirectoryTextBox.Location = new System.Drawing.Point(126, 19);
            this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
            this.outputDirectoryTextBox.Size = new System.Drawing.Size(220, 20);
            this.outputDirectoryTextBox.TabIndex = 1;
            this.outputDirectoryTextBox.Text = "TODO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Samples per emotion:";
            // 
            // samplesPerEmotionTextBox
            // 
            this.samplesPerEmotionTextBox.Location = new System.Drawing.Point(126, 55);
            this.samplesPerEmotionTextBox.Name = "samplesPerEmotionTextBox";
            this.samplesPerEmotionTextBox.Size = new System.Drawing.Size(220, 20);
            this.samplesPerEmotionTextBox.TabIndex = 3;
            this.samplesPerEmotionTextBox.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Time per sample:";
            // 
            // timePerSampleTextBox
            // 
            this.timePerSampleTextBox.Location = new System.Drawing.Point(126, 92);
            this.timePerSampleTextBox.Name = "timePerSampleTextBox";
            this.timePerSampleTextBox.Size = new System.Drawing.Size(220, 20);
            this.timePerSampleTextBox.TabIndex = 5;
            this.timePerSampleTextBox.Text = "15";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Break time:";
            // 
            // breakTimeTextBox
            // 
            this.breakTimeTextBox.Location = new System.Drawing.Point(126, 129);
            this.breakTimeTextBox.Name = "breakTimeTextBox";
            this.breakTimeTextBox.Size = new System.Drawing.Size(220, 20);
            this.breakTimeTextBox.TabIndex = 7;
            this.breakTimeTextBox.Text = "15";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(15, 199);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(331, 23);
            this.startButton.TabIndex = 8;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Data directory:";
            // 
            // dataDirectoryTextBox
            // 
            this.dataDirectoryTextBox.Location = new System.Drawing.Point(126, 164);
            this.dataDirectoryTextBox.Name = "dataDirectoryTextBox";
            this.dataDirectoryTextBox.Size = new System.Drawing.Size(220, 20);
            this.dataDirectoryTextBox.TabIndex = 10;
            this.dataDirectoryTextBox.Text = "TODO";
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 232);
            this.Controls.Add(this.dataDirectoryTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.breakTimeTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.timePerSampleTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.samplesPerEmotionTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.outputDirectoryTextBox);
            this.Controls.Add(this.label1);
            this.Name = "Startup";
            this.Text = "Startup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox outputDirectoryTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox samplesPerEmotionTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox timePerSampleTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox breakTimeTextBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dataDirectoryTextBox;
    }
}

