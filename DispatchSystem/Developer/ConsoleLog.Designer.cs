namespace DispatchSystem.Developer
{
    partial class ConsoleLog
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
            this.doubleBufferListView1 = new DispatchSystem.ExListView();
            this.SuspendLayout();
            // 
            // doubleBufferListView1
            // 
            this.doubleBufferListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleBufferListView1.Font = new System.Drawing.Font("宋体", 16F);
            this.doubleBufferListView1.GridLines = true;
            this.doubleBufferListView1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferListView1.Name = "doubleBufferListView1";
            this.doubleBufferListView1.Size = new System.Drawing.Size(1142, 569);
            this.doubleBufferListView1.TabIndex = 3;
            this.doubleBufferListView1.UseCompatibleStateImageBehavior = false;
            this.doubleBufferListView1.View = System.Windows.Forms.View.Details;
            // 
            // ConsoleLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1142, 569);
            this.Controls.Add(this.doubleBufferListView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConsoleLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ConsoleLog";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ConsoleLog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ExListView doubleBufferListView1;
    }
}