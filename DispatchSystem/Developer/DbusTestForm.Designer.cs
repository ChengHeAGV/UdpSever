namespace DispatchSystem.Developer
{
    partial class DbusTestForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbLedHeart = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTimeHeart = new System.Windows.Forms.TextBox();
            this.checkBoxHeart = new System.Windows.Forms.CheckBox();
            this.textBoxTargetAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxWriteRegister = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxWriteRegisterValue = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxWriteRegisterAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWriteRegisterTime = new System.Windows.Forms.TextBox();
            this.checkBoxWriteRegister = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLocalAddress = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxTargetIp = new System.Windows.Forms.TextBox();
            this.textBoxTargetPort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbLedWriteRegister = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbLedHeart);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxTimeHeart);
            this.groupBox1.Controls.Add(this.checkBoxHeart);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 16F);
            this.groupBox1.ForeColor = System.Drawing.Color.GreenYellow;
            this.groupBox1.Location = new System.Drawing.Point(356, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(789, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "心跳";
            // 
            // lbLedHeart
            // 
            this.lbLedHeart.BackColor = System.Drawing.Color.Transparent;
            this.lbLedHeart.BlinkInterval = 500;
            this.lbLedHeart.Label = "";
            this.lbLedHeart.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedHeart.LedColor = System.Drawing.Color.GreenYellow;
            this.lbLedHeart.LedOffColor = System.Drawing.Color.Gray;
            this.lbLedHeart.LedSize = new System.Drawing.SizeF(30F, 30F);
            this.lbLedHeart.LedState = LBSoft.IndustrialCtrls.Leds.LBLed.ledState.Off;
            this.lbLedHeart.Location = new System.Drawing.Point(722, 52);
            this.lbLedHeart.Name = "lbLedHeart";
            this.lbLedHeart.Renderer = null;
            this.lbLedHeart.Size = new System.Drawing.Size(48, 33);
            this.lbLedHeart.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedHeart.TabIndex = 15;
            this.lbLedHeart.Trigger = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "周期:";
            // 
            // textBoxTimeHeart
            // 
            this.textBoxTimeHeart.Location = new System.Drawing.Point(143, 52);
            this.textBoxTimeHeart.Name = "textBoxTimeHeart";
            this.textBoxTimeHeart.Size = new System.Drawing.Size(89, 32);
            this.textBoxTimeHeart.TabIndex = 12;
            this.textBoxTimeHeart.Text = "1000";
            // 
            // checkBoxHeart
            // 
            this.checkBoxHeart.AutoSize = true;
            this.checkBoxHeart.Font = new System.Drawing.Font("宋体", 16F);
            this.checkBoxHeart.Location = new System.Drawing.Point(272, 55);
            this.checkBoxHeart.Name = "checkBoxHeart";
            this.checkBoxHeart.Size = new System.Drawing.Size(95, 26);
            this.checkBoxHeart.TabIndex = 11;
            this.checkBoxHeart.Text = "Enable";
            this.checkBoxHeart.UseVisualStyleBackColor = true;
            // 
            // textBoxTargetAddress
            // 
            this.textBoxTargetAddress.Location = new System.Drawing.Point(126, 31);
            this.textBoxTargetAddress.Name = "textBoxTargetAddress";
            this.textBoxTargetAddress.Size = new System.Drawing.Size(175, 32);
            this.textBoxTargetAddress.TabIndex = 13;
            this.textBoxTargetAddress.Text = "0";
            this.textBoxTargetAddress.TextChanged += new System.EventHandler(this.Base_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 22);
            this.label1.TabIndex = 14;
            this.label1.Text = "目  标ID:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbLedWriteRegister);
            this.groupBox2.Controls.Add(this.comboBoxWriteRegister);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.textBoxWriteRegisterValue);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxWriteRegisterAddress);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxWriteRegisterTime);
            this.groupBox2.Controls.Add(this.checkBoxWriteRegister);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 16F);
            this.groupBox2.ForeColor = System.Drawing.Color.GreenYellow;
            this.groupBox2.Location = new System.Drawing.Point(356, 155);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(789, 257);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "写单个寄存器";
            // 
            // comboBoxWriteRegister
            // 
            this.comboBoxWriteRegister.FormattingEnabled = true;
            this.comboBoxWriteRegister.Items.AddRange(new object[] {
            "随机数",
            "顺序循环",
            "固定值"});
            this.comboBoxWriteRegister.Location = new System.Drawing.Point(144, 104);
            this.comboBoxWriteRegister.Name = "comboBoxWriteRegister";
            this.comboBoxWriteRegister.Size = new System.Drawing.Size(99, 29);
            this.comboBoxWriteRegister.TabIndex = 22;
            this.comboBoxWriteRegister.Text = "固定值";
            this.comboBoxWriteRegister.SelectedIndexChanged += new System.EventHandler(this.comboBoxWriteRegister_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(72, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 22);
            this.label9.TabIndex = 21;
            this.label9.Text = "数值:";
            // 
            // textBoxWriteRegisterValue
            // 
            this.textBoxWriteRegisterValue.Location = new System.Drawing.Point(143, 152);
            this.textBoxWriteRegisterValue.Name = "textBoxWriteRegisterValue";
            this.textBoxWriteRegisterValue.Size = new System.Drawing.Size(100, 32);
            this.textBoxWriteRegisterValue.TabIndex = 20;
            this.textBoxWriteRegisterValue.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 22);
            this.label6.TabIndex = 18;
            this.label6.Text = "数值类型:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 22);
            this.label5.TabIndex = 16;
            this.label5.Text = "寄存器地址:";
            // 
            // textBoxWriteRegisterAddress
            // 
            this.textBoxWriteRegisterAddress.Location = new System.Drawing.Point(143, 48);
            this.textBoxWriteRegisterAddress.Name = "textBoxWriteRegisterAddress";
            this.textBoxWriteRegisterAddress.Size = new System.Drawing.Size(100, 32);
            this.textBoxWriteRegisterAddress.TabIndex = 15;
            this.textBoxWriteRegisterAddress.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 204);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 22);
            this.label3.TabIndex = 14;
            this.label3.Text = "周期:";
            // 
            // textBoxWriteRegisterTime
            // 
            this.textBoxWriteRegisterTime.Location = new System.Drawing.Point(143, 201);
            this.textBoxWriteRegisterTime.Name = "textBoxWriteRegisterTime";
            this.textBoxWriteRegisterTime.Size = new System.Drawing.Size(100, 32);
            this.textBoxWriteRegisterTime.TabIndex = 12;
            this.textBoxWriteRegisterTime.Text = "1000";
            // 
            // checkBoxWriteRegister
            // 
            this.checkBoxWriteRegister.AutoSize = true;
            this.checkBoxWriteRegister.Font = new System.Drawing.Font("宋体", 16F);
            this.checkBoxWriteRegister.Location = new System.Drawing.Point(272, 51);
            this.checkBoxWriteRegister.Name = "checkBoxWriteRegister";
            this.checkBoxWriteRegister.Size = new System.Drawing.Size(95, 26);
            this.checkBoxWriteRegister.TabIndex = 11;
            this.checkBoxWriteRegister.Text = "Enable";
            this.checkBoxWriteRegister.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 22);
            this.label4.TabIndex = 14;
            this.label4.Text = "本  机ID:";
            // 
            // textBoxLocalAddress
            // 
            this.textBoxLocalAddress.Location = new System.Drawing.Point(126, 196);
            this.textBoxLocalAddress.Name = "textBoxLocalAddress";
            this.textBoxLocalAddress.Size = new System.Drawing.Size(175, 32);
            this.textBoxLocalAddress.TabIndex = 13;
            this.textBoxLocalAddress.Text = "1";
            this.textBoxLocalAddress.TextChanged += new System.EventHandler(this.Base_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxTargetIp);
            this.groupBox3.Controls.Add(this.textBoxTargetPort);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.textBoxTargetAddress);
            this.groupBox3.Controls.Add(this.textBoxLocalAddress);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 16F);
            this.groupBox3.ForeColor = System.Drawing.Color.GreenYellow;
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(321, 254);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "基础配置";
            // 
            // textBoxTargetIp
            // 
            this.textBoxTargetIp.Location = new System.Drawing.Point(126, 84);
            this.textBoxTargetIp.Name = "textBoxTargetIp";
            this.textBoxTargetIp.Size = new System.Drawing.Size(175, 32);
            this.textBoxTargetIp.TabIndex = 15;
            this.textBoxTargetIp.Text = "192.168.10.106";
            this.textBoxTargetIp.TextChanged += new System.EventHandler(this.Base_TextChanged);
            // 
            // textBoxTargetPort
            // 
            this.textBoxTargetPort.Location = new System.Drawing.Point(126, 139);
            this.textBoxTargetPort.Name = "textBoxTargetPort";
            this.textBoxTargetPort.Size = new System.Drawing.Size(175, 32);
            this.textBoxTargetPort.TabIndex = 16;
            this.textBoxTargetPort.Text = "18666";
            this.textBoxTargetPort.TextChanged += new System.EventHandler(this.Base_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 144);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 22);
            this.label7.TabIndex = 17;
            this.label7.Text = "目标端口:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 22);
            this.label8.TabIndex = 18;
            this.label8.Text = "目  标IP:";
            // 
            // lbLedWriteRegister
            // 
            this.lbLedWriteRegister.BackColor = System.Drawing.Color.Transparent;
            this.lbLedWriteRegister.BlinkInterval = 500;
            this.lbLedWriteRegister.Label = "";
            this.lbLedWriteRegister.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedWriteRegister.LedColor = System.Drawing.Color.GreenYellow;
            this.lbLedWriteRegister.LedOffColor = System.Drawing.Color.Gray;
            this.lbLedWriteRegister.LedSize = new System.Drawing.SizeF(30F, 30F);
            this.lbLedWriteRegister.LedState = LBSoft.IndustrialCtrls.Leds.LBLed.ledState.Off;
            this.lbLedWriteRegister.Location = new System.Drawing.Point(722, 51);
            this.lbLedWriteRegister.Name = "lbLedWriteRegister";
            this.lbLedWriteRegister.Renderer = null;
            this.lbLedWriteRegister.Size = new System.Drawing.Size(48, 33);
            this.lbLedWriteRegister.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedWriteRegister.TabIndex = 15;
            this.lbLedWriteRegister.Trigger = false;
            // 
            // DbusTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1156, 635);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DbusTestForm";
            this.Text = "通信测试界面";
            this.Load += new System.EventHandler(this.DbusTestForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTargetAddress;
        private System.Windows.Forms.TextBox textBoxTimeHeart;
        private System.Windows.Forms.CheckBox checkBoxHeart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxWriteRegisterAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWriteRegisterTime;
        private System.Windows.Forms.CheckBox checkBoxWriteRegister;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxLocalAddress;
        private System.Windows.Forms.GroupBox groupBox3;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedHeart;
        private System.Windows.Forms.TextBox textBoxTargetIp;
        private System.Windows.Forms.TextBox textBoxTargetPort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxWriteRegister;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxWriteRegisterValue;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedWriteRegister;
    }
}