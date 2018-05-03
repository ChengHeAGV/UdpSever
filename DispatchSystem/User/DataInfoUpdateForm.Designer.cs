namespace DispatchSystem.User
{
    partial class DataInfoUpdateForm
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
            this.comboBoxStop = new System.Windows.Forms.ComboBox();
            this.comboBoxStart = new System.Windows.Forms.ComboBox();
            this.textBoxDes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonEnter = new System.Windows.Forms.Button();
            this.labelDir = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelDir);
            this.groupBox1.Controls.Add(this.comboBoxStop);
            this.groupBox1.Controls.Add(this.comboBoxStart);
            this.groupBox1.Controls.Add(this.textBoxDes);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonCancel);
            this.groupBox1.Controls.Add(this.buttonEnter);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 259);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // comboBoxStop
            // 
            this.comboBoxStop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStop.FormattingEnabled = true;
            this.comboBoxStop.Items.AddRange(new object[] {
            "AGV",
            "PLC",
            "AGVSever",
            "MES"});
            this.comboBoxStop.Location = new System.Drawing.Point(105, 55);
            this.comboBoxStop.Name = "comboBoxStop";
            this.comboBoxStop.Size = new System.Drawing.Size(170, 24);
            this.comboBoxStop.TabIndex = 10;
            this.comboBoxStop.SelectedIndexChanged += new System.EventHandler(this.comboBoxStop_SelectedIndexChanged);
            // 
            // comboBoxStart
            // 
            this.comboBoxStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStart.FormattingEnabled = true;
            this.comboBoxStart.Items.AddRange(new object[] {
            "AGV",
            "PLC",
            "AGVSever",
            "MES"});
            this.comboBoxStart.Location = new System.Drawing.Point(105, 25);
            this.comboBoxStart.Name = "comboBoxStart";
            this.comboBoxStart.Size = new System.Drawing.Size(170, 24);
            this.comboBoxStart.TabIndex = 9;
            this.comboBoxStart.SelectedIndexChanged += new System.EventHandler(this.comboBoxStart_SelectedIndexChanged);
            // 
            // textBoxDes
            // 
            this.textBoxDes.Location = new System.Drawing.Point(105, 132);
            this.textBoxDes.Multiline = true;
            this.textBoxDes.Name = "textBoxDes";
            this.textBoxDes.Size = new System.Drawing.Size(170, 76);
            this.textBoxDes.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "描述:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "传输方向:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(190, 214);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(85, 33);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonEnter
            // 
            this.buttonEnter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEnter.Location = new System.Drawing.Point(74, 214);
            this.buttonEnter.Name = "buttonEnter";
            this.buttonEnter.Size = new System.Drawing.Size(85, 33);
            this.buttonEnter.TabIndex = 0;
            this.buttonEnter.Text = "确定";
            this.buttonEnter.UseVisualStyleBackColor = true;
            this.buttonEnter.Click += new System.EventHandler(this.buttonEnter_Click);
            // 
            // labelDir
            // 
            this.labelDir.AutoSize = true;
            this.labelDir.BackColor = System.Drawing.Color.Gold;
            this.labelDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDir.Location = new System.Drawing.Point(105, 93);
            this.labelDir.Margin = new System.Windows.Forms.Padding(1);
            this.labelDir.Name = "labelDir";
            this.labelDir.Padding = new System.Windows.Forms.Padding(3);
            this.labelDir.Size = new System.Drawing.Size(48, 24);
            this.labelDir.TabIndex = 11;
            this.labelDir.Text = "A->B";
            // 
            // UpdateDataInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 259);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateDataInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "更新数据信息";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxDes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonEnter;
        private System.Windows.Forms.ComboBox comboBoxStart;
        private System.Windows.Forms.ComboBox comboBoxStop;
        private System.Windows.Forms.Label labelDir;
    }
}