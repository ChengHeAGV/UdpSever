namespace DispatchSystem
{
    partial class RegisterForm
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
            this.doubleBufferListView1 = new DispatchSystem.DoubleBufferListView();
            this.SuspendLayout();
            // 
            // doubleBufferListView1
            // 
            this.doubleBufferListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.doubleBufferListView1.Font = new System.Drawing.Font("宋体", 16F);
            this.doubleBufferListView1.GridLines = true;
            this.doubleBufferListView1.Location = new System.Drawing.Point(0, 0);
            this.doubleBufferListView1.Name = "doubleBufferListView1";
            this.doubleBufferListView1.Size = new System.Drawing.Size(850, 488);
            this.doubleBufferListView1.TabIndex = 0;
            this.doubleBufferListView1.UseCompatibleStateImageBehavior = false;
            this.doubleBufferListView1.View = System.Windows.Forms.View.Details;
            this.doubleBufferListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.doubleBufferListView1_MouseDoubleClick);
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 488);
            this.Controls.Add(this.doubleBufferListView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegisterForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataForm_FormClosing);
            this.Load += new System.EventHandler(this.DataForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferListView doubleBufferListView1;
    }
}