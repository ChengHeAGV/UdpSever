namespace DispatchSystem
{
    partial class SensorForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.magneticGuide161 = new DispatchSystem.UserControls.MagneticGuide16();
            this.magneticGuide81 = new DispatchSystem.UserControls.MagneticGuide8();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "读取单个寄存器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide81);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide161);
            this.splitContainer1.Size = new System.Drawing.Size(1085, 562);
            this.splitContainer1.SplitterDistance = 361;
            this.splitContainer1.TabIndex = 2;
            // 
            // magneticGuide161
            // 
            this.magneticGuide161.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide161.Data = 21845;
            this.magneticGuide161.FalseColor = System.Drawing.Color.Red;
            this.magneticGuide161.Location = new System.Drawing.Point(3, 12);
            this.magneticGuide161.Name = "magneticGuide161";
            this.magneticGuide161.Size = new System.Drawing.Size(696, 180);
            this.magneticGuide161.TabIndex = 3;
            this.magneticGuide161.TrueColor = System.Drawing.Color.Yellow;
            // 
            // magneticGuide81
            // 
            this.magneticGuide81.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide81.Data = 21845;
            this.magneticGuide81.FalseColor = System.Drawing.Color.Red;
            this.magneticGuide81.Location = new System.Drawing.Point(3, 209);
            this.magneticGuide81.Name = "magneticGuide81";
            this.magneticGuide81.Size = new System.Drawing.Size(361, 155);
            this.magneticGuide81.TabIndex = 4;
            this.magneticGuide81.TrueColor = System.Drawing.Color.Yellow;
            // 
            // SensorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 562);
            this.Controls.Add(this.splitContainer1);
            this.Name = "SensorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SensorForm";
            this.Load += new System.EventHandler(this.SensorForm_Load);
            this.SizeChanged += new System.EventHandler(this.SensorForm_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserControls.MagneticGuide16 magneticGuide161;
        private UserControls.MagneticGuide8 magneticGuide81;
    }
}