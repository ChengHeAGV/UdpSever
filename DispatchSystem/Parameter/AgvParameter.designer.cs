namespace DispatchSystem
{
    partial class AgvParameter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgvParameter));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvTaskList = new AdvancedDataGridView.TreeGridView();
            this.Column1 = new AdvancedDataGridView.TreeGridColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5_LED = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox2_BaudRate = new System.Windows.Forms.ComboBox();
            this.comboBox1_PortName = new System.Windows.Forms.ComboBox();
            this.button1_openclose = new System.Windows.Forms.Button();
            this.checkBox_liucheng = new System.Windows.Forms.CheckBox();
            this.button4_savetopc = new System.Windows.Forms.Button();
            this.button3_savetoflash = new System.Windows.Forms.Button();
            this.checkBox_lujing = new System.Windows.Forms.CheckBox();
            this.checkBox_parment = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button5_readformflash = new System.Windows.Forms.Button();
            this.button2_readformpc = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.attachment = new System.Windows.Forms.DataGridViewImageColumn();
            this.参数号 = new AdvancedDataGridView.TreeGridColumn();
            this.参数名 = new AdvancedDataGridView.TreeGridColumn();
            this.当前值 = new AdvancedDataGridView.TreeGridColumn();
            this.设定值 = new AdvancedDataGridView.TreeGridColumn();
            this.描述 = new AdvancedDataGridView.TreeGridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_savemiaoshu = new System.Windows.Forms.Button();
            this.button_readmiaoshu = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvTaskList)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvTaskList);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tvTaskList
            // 
            this.tvTaskList.AllowUserToAddRows = false;
            this.tvTaskList.AllowUserToDeleteRows = false;
            this.tvTaskList.AllowUserToResizeColumns = false;
            this.tvTaskList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            this.tvTaskList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.tvTaskList, "tvTaskList");
            this.tvTaskList.BackgroundColor = System.Drawing.Color.Gray;
            this.tvTaskList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tvTaskList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tvTaskList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.tvTaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.tvTaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column6,
            this.Column2,
            this.Column7,
            this.Column8,
            this.Column9});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tvTaskList.DefaultCellStyle = dataGridViewCellStyle9;
            this.tvTaskList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.tvTaskList.ImageList = null;
            this.tvTaskList.MultiSelect = false;
            this.tvTaskList.Name = "tvTaskList";
            this.tvTaskList.RowHeadersVisible = false;
            this.tvTaskList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.tvTaskList.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.tvTaskList_CellBeginEdit);
            this.tvTaskList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tvTaskList_CellClick);
            this.tvTaskList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.tvTaskList_CellEndEdit);
            this.tvTaskList.CurrentCellChanged += new System.EventHandler(this.tvTaskList_CurrentCellChanged);
            this.tvTaskList.Scroll += new System.Windows.Forms.ScrollEventHandler(this.tvTaskList_Scroll);
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.DefaultNodeImage = ((System.Drawing.Image)(resources.GetObject("Column1.DefaultNodeImage")));
            this.Column1.DividerWidth = 1;
            this.Column1.FillWeight = 51.53443F;
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column6.DividerWidth = 1;
            resources.ApplyResources(this.Column6, "Column6");
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column2.DividerWidth = 1;
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column7.DividerWidth = 1;
            resources.ApplyResources(this.Column7, "Column7");
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column8
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column8.DividerWidth = 1;
            resources.ApplyResources(this.Column8, "Column8");
            this.Column8.Name = "Column8";
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column9
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column9.DefaultCellStyle = dataGridViewCellStyle8;
            this.Column9.DividerWidth = 1;
            resources.ApplyResources(this.Column9, "Column9");
            this.Column9.Name = "Column9";
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label5_LED
            // 
            resources.ApplyResources(this.label5_LED, "label5_LED");
            this.label5_LED.BackColor = System.Drawing.SystemColors.Control;
            this.label5_LED.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5_LED.Name = "label5_LED";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comboBox2_BaudRate
            // 
            resources.ApplyResources(this.comboBox2_BaudRate, "comboBox2_BaudRate");
            this.comboBox2_BaudRate.AutoCompleteCustomSource.AddRange(new string[] {
            resources.GetString("comboBox2_BaudRate.AutoCompleteCustomSource"),
            resources.GetString("comboBox2_BaudRate.AutoCompleteCustomSource1"),
            resources.GetString("comboBox2_BaudRate.AutoCompleteCustomSource2")});
            this.comboBox2_BaudRate.FormattingEnabled = true;
            this.comboBox2_BaudRate.Items.AddRange(new object[] {
            resources.GetString("comboBox2_BaudRate.Items"),
            resources.GetString("comboBox2_BaudRate.Items1"),
            resources.GetString("comboBox2_BaudRate.Items2"),
            resources.GetString("comboBox2_BaudRate.Items3"),
            resources.GetString("comboBox2_BaudRate.Items4")});
            this.comboBox2_BaudRate.Name = "comboBox2_BaudRate";
            this.comboBox2_BaudRate.SelectionChangeCommitted += new System.EventHandler(this.comboBox2_BaudRate_SelectionChangeCommitted);
            // 
            // comboBox1_PortName
            // 
            resources.ApplyResources(this.comboBox1_PortName, "comboBox1_PortName");
            this.comboBox1_PortName.FormattingEnabled = true;
            this.comboBox1_PortName.Name = "comboBox1_PortName";
            this.comboBox1_PortName.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_PortName_SelectionChangeCommitted);
            this.comboBox1_PortName.Click += new System.EventHandler(this.comboBox1_PortName_Click);
            // 
            // button1_openclose
            // 
            resources.ApplyResources(this.button1_openclose, "button1_openclose");
            this.button1_openclose.Name = "button1_openclose";
            this.button1_openclose.UseVisualStyleBackColor = true;
            this.button1_openclose.Click += new System.EventHandler(this.button1_openclose_Click);
            // 
            // checkBox_liucheng
            // 
            resources.ApplyResources(this.checkBox_liucheng, "checkBox_liucheng");
            this.checkBox_liucheng.Checked = true;
            this.checkBox_liucheng.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_liucheng.Name = "checkBox_liucheng";
            this.checkBox_liucheng.UseVisualStyleBackColor = true;
            // 
            // button4_savetopc
            // 
            this.button4_savetopc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button4_savetopc.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button4_savetopc, "button4_savetopc");
            this.button4_savetopc.Name = "button4_savetopc";
            this.button4_savetopc.UseVisualStyleBackColor = false;
            this.button4_savetopc.Click += new System.EventHandler(this.button4_savetopc_Click);
            // 
            // button3_savetoflash
            // 
            this.button3_savetoflash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button3_savetoflash.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button3_savetoflash, "button3_savetoflash");
            this.button3_savetoflash.Name = "button3_savetoflash";
            this.button3_savetoflash.UseVisualStyleBackColor = false;
            this.button3_savetoflash.Click += new System.EventHandler(this.button3_savetoflash_Click);
            // 
            // checkBox_lujing
            // 
            resources.ApplyResources(this.checkBox_lujing, "checkBox_lujing");
            this.checkBox_lujing.Checked = true;
            this.checkBox_lujing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_lujing.Name = "checkBox_lujing";
            this.checkBox_lujing.UseVisualStyleBackColor = true;
            // 
            // checkBox_parment
            // 
            resources.ApplyResources(this.checkBox_parment, "checkBox_parment");
            this.checkBox_parment.Checked = true;
            this.checkBox_parment.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_parment.Name = "checkBox_parment";
            this.checkBox_parment.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button1.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5_readformflash
            // 
            this.button5_readformflash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button5_readformflash.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button5_readformflash, "button5_readformflash");
            this.button5_readformflash.Name = "button5_readformflash";
            this.button5_readformflash.UseVisualStyleBackColor = false;
            this.button5_readformflash.Click += new System.EventHandler(this.button5_readformflash_Click);
            // 
            // button2_readformpc
            // 
            this.button2_readformpc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button2_readformpc.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button2_readformpc, "button2_readformpc");
            this.button2_readformpc.Name = "button2_readformpc";
            this.button2_readformpc.UseVisualStyleBackColor = false;
            this.button2_readformpc.Click += new System.EventHandler(this.button2_readformpc_Click_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.PortName = "COM3";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageStrip, "imageStrip");
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // attachment
            // 
            this.attachment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.attachment.DefaultCellStyle = dataGridViewCellStyle10;
            this.attachment.DividerWidth = 1;
            this.attachment.FillWeight = 51.53443F;
            resources.ApplyResources(this.attachment, "attachment");
            this.attachment.Name = "attachment";
            this.attachment.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // 参数号
            // 
            this.参数号.DefaultNodeImage = null;
            resources.ApplyResources(this.参数号, "参数号");
            this.参数号.Name = "参数号";
            this.参数号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 参数名
            // 
            this.参数名.DefaultNodeImage = null;
            resources.ApplyResources(this.参数名, "参数名");
            this.参数名.Name = "参数名";
            this.参数名.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 当前值
            // 
            this.当前值.DefaultNodeImage = null;
            resources.ApplyResources(this.当前值, "当前值");
            this.当前值.Name = "当前值";
            this.当前值.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 设定值
            // 
            this.设定值.DefaultNodeImage = null;
            resources.ApplyResources(this.设定值, "设定值");
            this.设定值.Name = "设定值";
            this.设定值.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 描述
            // 
            this.描述.DefaultNodeImage = null;
            resources.ApplyResources(this.描述, "描述");
            this.描述.Name = "描述";
            this.描述.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // button_savemiaoshu
            // 
            this.button_savemiaoshu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button_savemiaoshu.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button_savemiaoshu, "button_savemiaoshu");
            this.button_savemiaoshu.Name = "button_savemiaoshu";
            this.button_savemiaoshu.UseVisualStyleBackColor = false;
            this.button_savemiaoshu.Click += new System.EventHandler(this.button_savemiaoshu_Click);
            // 
            // button_readmiaoshu
            // 
            this.button_readmiaoshu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button_readmiaoshu.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button_readmiaoshu, "button_readmiaoshu");
            this.button_readmiaoshu.Name = "button_readmiaoshu";
            this.button_readmiaoshu.UseVisualStyleBackColor = false;
            this.button_readmiaoshu.Click += new System.EventHandler(this.button_readmiaoshu_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.button2.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox1);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.ForeColor = System.Drawing.Color.Blue;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.linkLabel1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);

            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            this.comboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox1_KeyPress);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button4_savetopc);
            this.Controls.Add(this.button3_savetoflash);
            this.Controls.Add(this.button_savemiaoshu);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_readmiaoshu);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button5_readformflash);
            this.Controls.Add(this.button2_readformpc);
            this.Controls.Add(this.checkBox_liucheng);
            this.Controls.Add(this.label5_LED);
            this.Controls.Add(this.checkBox_parment);
            this.Controls.Add(this.button1_openclose);
            this.Controls.Add(this.checkBox_lujing);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.comboBox2_BaudRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.comboBox1_PortName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvTaskList)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1_openclose;
        private System.Windows.Forms.Button button4_savetopc;
        private System.Windows.Forms.Button button3_savetoflash;
        private System.Windows.Forms.Button button2_readformpc;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button5_readformflash;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label5_LED;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox2_BaudRate;
        private System.Windows.Forms.ComboBox comboBox1_PortName;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.DataGridViewImageColumn attachment;
        private AdvancedDataGridView.TreeGridColumn 参数号;
        private AdvancedDataGridView.TreeGridColumn 参数名;
        private AdvancedDataGridView.TreeGridColumn 当前值;
        private AdvancedDataGridView.TreeGridColumn 设定值;
        private AdvancedDataGridView.TreeGridColumn 描述;
        private AdvancedDataGridView.TreeGridView tvTaskList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_liucheng;
        private System.Windows.Forms.CheckBox checkBox_lujing;
        private System.Windows.Forms.CheckBox checkBox_parment;
        private System.Windows.Forms.Button button_savemiaoshu;
        private System.Windows.Forms.Button button_readmiaoshu;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private AdvancedDataGridView.TreeGridColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}

