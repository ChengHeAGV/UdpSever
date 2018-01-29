namespace DispatchSystem
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button3 = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.labelRX = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.labelTx = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label_Error = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.listViewUser = new DispatchSystem.DoubleBufferListView();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("宋体", 16F);
            this.button3.Location = new System.Drawing.Point(12, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(171, 48);
            this.button3.TabIndex = 55;
            this.button3.Text = "清空";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("宋体", 16F);
            this.buttonStart.ForeColor = System.Drawing.Color.Green;
            this.buttonStart.Location = new System.Drawing.Point(13, 134);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(171, 48);
            this.buttonStart.TabIndex = 54;
            this.buttonStart.Text = "启动";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 52;
            this.label3.Text = "端口号:";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("宋体", 18F);
            this.textBox2.Location = new System.Drawing.Point(13, 94);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(171, 35);
            this.textBox2.TabIndex = 53;
            this.textBox2.Text = "8080";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 50;
            this.label2.Text = "IP地址:";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 16F);
            this.textBox1.Location = new System.Drawing.Point(15, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(168, 32);
            this.textBox1.TabIndex = 51;
            this.textBox1.Text = "192.168.31.113";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("宋体", 18F);
            this.textBox3.Location = new System.Drawing.Point(190, 9);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox3.Size = new System.Drawing.Size(436, 505);
            this.textBox3.TabIndex = 56;
            // 
            // labelRX
            // 
            this.labelRX.AutoSize = true;
            this.labelRX.Font = new System.Drawing.Font("宋体", 18F);
            this.labelRX.Location = new System.Drawing.Point(855, 471);
            this.labelRX.Name = "labelRX";
            this.labelRX.Size = new System.Drawing.Size(130, 24);
            this.labelRX.TabIndex = 61;
            this.labelRX.Text = "接收数据：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(43, 263);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 62;
            this.checkBox1.Text = "显示调试信息";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // labelTx
            // 
            this.labelTx.AutoSize = true;
            this.labelTx.Font = new System.Drawing.Font("宋体", 18F);
            this.labelTx.Location = new System.Drawing.Point(639, 471);
            this.labelTx.Name = "labelTx";
            this.labelTx.Size = new System.Drawing.Size(130, 24);
            this.labelTx.TabIndex = 64;
            this.labelTx.Text = "发送数据：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(514, 601);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(82, 35);
            this.button2.TabIndex = 58;
            this.button2.Text = "发送";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox5
            // 
            this.textBox5.Font = new System.Drawing.Font("宋体", 18F);
            this.textBox5.Location = new System.Drawing.Point(23, 601);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(469, 35);
            this.textBox5.TabIndex = 57;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.White;
            this.textBox8.Font = new System.Drawing.Font("宋体", 18F);
            this.textBox8.Location = new System.Drawing.Point(136, 537);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(356, 35);
            this.textBox8.TabIndex = 60;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 18F);
            this.label4.Location = new System.Drawing.Point(19, 540);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 24);
            this.label4.TabIndex = 59;
            this.label4.Text = "目的地址：";
            // 
            // label_Error
            // 
            this.label_Error.AutoSize = true;
            this.label_Error.Font = new System.Drawing.Font("宋体", 18F);
            this.label_Error.Location = new System.Drawing.Point(1118, 471);
            this.label_Error.Name = "label_Error";
            this.label_Error.Size = new System.Drawing.Size(130, 24);
            this.label_Error.TabIndex = 65;
            this.label_Error.Text = "错误数据：";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(43, 363);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 66;
            this.checkBox2.Text = "开机启动";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // listViewUser
            // 
            this.listViewUser.Font = new System.Drawing.Font("宋体", 16F);
            this.listViewUser.GridLines = true;
            this.listViewUser.Location = new System.Drawing.Point(632, 12);
            this.listViewUser.Name = "listViewUser";
            this.listViewUser.Size = new System.Drawing.Size(693, 379);
            this.listViewUser.TabIndex = 63;
            this.listViewUser.UseCompatibleStateImageBehavior = false;
            this.listViewUser.View = System.Windows.Forms.View.Details;
            this.listViewUser.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewUser_MouseClick);
            this.listViewUser.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewUser_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1337, 668);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.label_Error);
            this.Controls.Add(this.labelTx);
            this.Controls.Add(this.listViewUser);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.labelRX);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "AGV调度系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label labelRX;
        private System.Windows.Forms.CheckBox checkBox1;
        private DoubleBufferListView listViewUser;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_Error;
        private System.Windows.Forms.Label labelTx;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

