namespace SyncMaester
{
    partial class MainWindow
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
            this.sourcePath = new System.Windows.Forms.TextBox();
            this.destinationPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.bSync = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            // 
            // sourcePath
            // 
            this.sourcePath.Location = new System.Drawing.Point(86, 105);
            this.sourcePath.Name = "sourcePath";
            this.sourcePath.Size = new System.Drawing.Size(354, 20);
            this.sourcePath.TabIndex = 1;
            // 
            // destinationPath
            // 
            this.destinationPath.Location = new System.Drawing.Point(86, 141);
            this.destinationPath.Name = "destinationPath";
            this.destinationPath.Size = new System.Drawing.Size(354, 20);
            this.destinationPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Destination:";
            // 
            // bSync
            // 
            this.bSync.Location = new System.Drawing.Point(446, 103);
            this.bSync.Name = "bSync";
            this.bSync.Size = new System.Drawing.Size(117, 58);
            this.bSync.TabIndex = 4;
            this.bSync.Text = "Sync";
            this.bSync.UseVisualStyleBackColor = true;
            this.bSync.Click += new System.EventHandler(this.bSync_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 318);
            this.Controls.Add(this.bSync);
            this.Controls.Add(this.destinationPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sourcePath);
            this.Controls.Add(this.label1);
            this.Name = "MainWindow";
            this.Text = "Sync Maester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sourcePath;
        private System.Windows.Forms.TextBox destinationPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bSync;
    }
}

