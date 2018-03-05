namespace DispatchSystem.AGV
{
    partial class ControlForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.buttonGunTongLeft = new System.Windows.Forms.Button();
            this.buttonGunTongRight = new System.Windows.Forms.Button();
            this.button_GunTongStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 69);
            this.button1.TabIndex = 0;
            this.button1.Text = "读多个寄存器";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 69);
            this.button2.TabIndex = 1;
            this.button2.Text = "写多个寄存器";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonGunTongLeft
            // 
            this.buttonGunTongLeft.Font = new System.Drawing.Font("宋体", 18F);
            this.buttonGunTongLeft.Location = new System.Drawing.Point(21, 23);
            this.buttonGunTongLeft.Name = "buttonGunTongLeft";
            this.buttonGunTongLeft.Size = new System.Drawing.Size(121, 69);
            this.buttonGunTongLeft.TabIndex = 2;
            this.buttonGunTongLeft.Text = "左转";
            this.buttonGunTongLeft.UseVisualStyleBackColor = true;
            this.buttonGunTongLeft.Click += new System.EventHandler(this.buttonGunTongLeft_Click);
            // 
            // buttonGunTongRight
            // 
            this.buttonGunTongRight.Font = new System.Drawing.Font("宋体", 18F);
            this.buttonGunTongRight.Location = new System.Drawing.Point(148, 23);
            this.buttonGunTongRight.Name = "buttonGunTongRight";
            this.buttonGunTongRight.Size = new System.Drawing.Size(121, 69);
            this.buttonGunTongRight.TabIndex = 3;
            this.buttonGunTongRight.Text = "右转";
            this.buttonGunTongRight.UseVisualStyleBackColor = true;
            this.buttonGunTongRight.Click += new System.EventHandler(this.buttonGunTongRight_Click);
            // 
            // button_GunTongStop
            // 
            this.button_GunTongStop.Font = new System.Drawing.Font("宋体", 18F);
            this.button_GunTongStop.Location = new System.Drawing.Point(148, 121);
            this.button_GunTongStop.Name = "button_GunTongStop";
            this.button_GunTongStop.Size = new System.Drawing.Size(121, 69);
            this.button_GunTongStop.TabIndex = 4;
            this.button_GunTongStop.Text = "停止";
            this.button_GunTongStop.UseVisualStyleBackColor = true;
            this.button_GunTongStop.Click += new System.EventHandler(this.button_GunTongStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonGunTongRight);
            this.groupBox1.Controls.Add(this.button_GunTongStop);
            this.groupBox1.Controls.Add(this.buttonGunTongLeft);
            this.groupBox1.Location = new System.Drawing.Point(230, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 200);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "滚筒";
            // 
            // ControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(553, 405);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ControlForm";
            this.Text = "ControlForm";
            this.Load += new System.EventHandler(this.ControlForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonGunTongLeft;
        private System.Windows.Forms.Button buttonGunTongRight;
        private System.Windows.Forms.Button button_GunTongStop;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}