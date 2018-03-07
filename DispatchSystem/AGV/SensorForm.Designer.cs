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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SensorForm));
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.rfid1 = new DispatchSystem.UserControls.RFID();
            this.magneticGuide8Bit4 = new DispatchSystem.UserControls.MagneticGuide8Bit();
            this.magneticGuide8Bit3 = new DispatchSystem.UserControls.MagneticGuide8Bit();
            this.magneticGuide8Bit2 = new DispatchSystem.UserControls.MagneticGuide8Bit();
            this.magneticGuide8Bit1 = new DispatchSystem.UserControls.MagneticGuide8Bit();
            this.lbButton2 = new LBSoft.IndustrialCtrls.Buttons.LBButton();
            this.lbKnob1 = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.lbLed2 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbButton1 = new LBSoft.IndustrialCtrls.Buttons.LBButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 65);
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
            this.splitContainer1.Panel1.Controls.Add(this.lbKnob1);
            this.splitContainer1.Panel1.Controls.Add(this.lbButton1);
            this.splitContainer1.Panel1.Controls.Add(this.lbLed1);
            this.splitContainer1.Panel1.Controls.Add(this.lbButton2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide8Bit4);
            this.splitContainer1.Panel2.Controls.Add(this.rfid1);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide8Bit3);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide8Bit2);
            this.splitContainer1.Panel2.Controls.Add(this.lbLed2);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide8Bit1);
            this.splitContainer1.Size = new System.Drawing.Size(718, 505);
            this.splitContainer1.SplitterDistance = 194;
            this.splitContainer1.TabIndex = 2;
            // 
            // rfid1
            // 
            this.rfid1.Location = new System.Drawing.Point(343, 309);
            this.rfid1.Name = "rfid1";
            this.rfid1.PowerLed = true;
            this.rfid1.PowerOffColor = System.Drawing.Color.Red;
            this.rfid1.PowerOnColor = System.Drawing.Color.Green;
            this.rfid1.Size = new System.Drawing.Size(165, 184);
            this.rfid1.TabIndex = 16;
            this.rfid1.Value = 0;
            this.rfid1.ValueColor = System.Drawing.Color.Magenta;
            this.rfid1.ValueLed = false;
            this.rfid1.ValueOffColor = System.Drawing.Color.Red;
            this.rfid1.ValueOnColor = System.Drawing.Color.Yellow;
            // 
            // magneticGuide8Bit4
            // 
            this.magneticGuide8Bit4.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide8Bit4.ColorOff = System.Drawing.Color.Red;
            this.magneticGuide8Bit4.ColorOn = System.Drawing.Color.Yellow;
            this.magneticGuide8Bit4.Location = new System.Drawing.Point(13, 378);
            this.magneticGuide8Bit4.Name = "magneticGuide8Bit4";
            this.magneticGuide8Bit4.Size = new System.Drawing.Size(324, 116);
            this.magneticGuide8Bit4.TabIndex = 19;
            this.magneticGuide8Bit4.Value = 85;
            // 
            // magneticGuide8Bit3
            // 
            this.magneticGuide8Bit3.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide8Bit3.ColorOff = System.Drawing.Color.Red;
            this.magneticGuide8Bit3.ColorOn = System.Drawing.Color.Yellow;
            this.magneticGuide8Bit3.Location = new System.Drawing.Point(13, 256);
            this.magneticGuide8Bit3.Name = "magneticGuide8Bit3";
            this.magneticGuide8Bit3.Size = new System.Drawing.Size(324, 116);
            this.magneticGuide8Bit3.TabIndex = 18;
            this.magneticGuide8Bit3.Value = 85;
            // 
            // magneticGuide8Bit2
            // 
            this.magneticGuide8Bit2.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide8Bit2.ColorOff = System.Drawing.Color.Red;
            this.magneticGuide8Bit2.ColorOn = System.Drawing.Color.Yellow;
            this.magneticGuide8Bit2.Location = new System.Drawing.Point(13, 134);
            this.magneticGuide8Bit2.Name = "magneticGuide8Bit2";
            this.magneticGuide8Bit2.Size = new System.Drawing.Size(324, 116);
            this.magneticGuide8Bit2.TabIndex = 17;
            this.magneticGuide8Bit2.Value = 85;
            // 
            // magneticGuide8Bit1
            // 
            this.magneticGuide8Bit1.BorderColor = System.Drawing.Color.DarkGray;
            this.magneticGuide8Bit1.ColorOff = System.Drawing.Color.Red;
            this.magneticGuide8Bit1.ColorOn = System.Drawing.Color.Yellow;
            this.magneticGuide8Bit1.Location = new System.Drawing.Point(13, 12);
            this.magneticGuide8Bit1.Name = "magneticGuide8Bit1";
            this.magneticGuide8Bit1.Size = new System.Drawing.Size(324, 116);
            this.magneticGuide8Bit1.TabIndex = 14;
            this.magneticGuide8Bit1.Value = 85;
            // 
            // lbButton2
            // 
            this.lbButton2.BackColor = System.Drawing.Color.Transparent;
            this.lbButton2.ButtonColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lbButton2.Label = "";
            this.lbButton2.Location = new System.Drawing.Point(21, 409);
            this.lbButton2.Name = "lbButton2";
            this.lbButton2.Renderer = null;
            this.lbButton2.RepeatInterval = 100;
            this.lbButton2.RepeatState = false;
            this.lbButton2.Size = new System.Drawing.Size(153, 75);
            this.lbButton2.StartRepeatInterval = 500;
            this.lbButton2.State = LBSoft.IndustrialCtrls.Buttons.LBButton.ButtonState.Normal;
            this.lbButton2.Style = LBSoft.IndustrialCtrls.Buttons.LBButton.ButtonStyle.Rectangular;
            this.lbButton2.TabIndex = 13;
            // 
            // lbKnob1
            // 
            this.lbKnob1.BackColor = System.Drawing.Color.Transparent;
            this.lbKnob1.DrawRatio = 0.205F;
            this.lbKnob1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lbKnob1.IndicatorColor = System.Drawing.Color.MistyRose;
            this.lbKnob1.IndicatorOffset = 10F;
            this.lbKnob1.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("lbKnob1.KnobCenter")));
            this.lbKnob1.KnobColor = System.Drawing.Color.DarkOrange;
            this.lbKnob1.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("lbKnob1.KnobRect")));
            this.lbKnob1.Location = new System.Drawing.Point(50, 284);
            this.lbKnob1.MaxValue = 100F;
            this.lbKnob1.MinValue = 0F;
            this.lbKnob1.Name = "lbKnob1";
            this.lbKnob1.Renderer = null;
            this.lbKnob1.ScaleColor = System.Drawing.Color.Gray;
            this.lbKnob1.Size = new System.Drawing.Size(41, 56);
            this.lbKnob1.StepValue = 0.1F;
            this.lbKnob1.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.lbKnob1.TabIndex = 12;
            this.lbKnob1.Value = 100F;
            // 
            // lbLed2
            // 
            this.lbLed2.BackColor = System.Drawing.Color.Transparent;
            this.lbLed2.BlinkInterval = 100;
            this.lbLed2.Font = new System.Drawing.Font("宋体", 18F);
            this.lbLed2.Label = "";
            this.lbLed2.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLed2.LedColor = System.Drawing.Color.Peru;
            this.lbLed2.LedSize = new System.Drawing.SizeF(50F, 50F);
            this.lbLed2.Location = new System.Drawing.Point(449, 12);
            this.lbLed2.Name = "lbLed2";
            this.lbLed2.Renderer = null;
            this.lbLed2.Size = new System.Drawing.Size(68, 56);
            this.lbLed2.LedState = LBSoft.IndustrialCtrls.Leds.LBLed.ledState.Blink;
            this.lbLed2.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Rectangular;
            this.lbLed2.TabIndex = 10;
            // 
            // lbLed1
            // 
            this.lbLed1.BackColor = System.Drawing.Color.Transparent;
            this.lbLed1.BlinkInterval = 500;
            this.lbLed1.Font = new System.Drawing.Font("宋体", 18F);
            this.lbLed1.Label = "";
            this.lbLed1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLed1.LedColor = System.Drawing.Color.Red;
            this.lbLed1.LedSize = new System.Drawing.SizeF(50F, 50F);
            this.lbLed1.Location = new System.Drawing.Point(35, 337);
            this.lbLed1.Name = "lbLed1";
            this.lbLed1.Renderer = null;
            this.lbLed1.Size = new System.Drawing.Size(68, 56);
            this.lbLed1.LedState = LBSoft.IndustrialCtrls.Leds.LBLed.ledState.Off;
            this.lbLed1.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed1.TabIndex = 9;
            // 
            // lbButton1
            // 
            this.lbButton1.BackColor = System.Drawing.Color.Transparent;
            this.lbButton1.ButtonColor = System.Drawing.Color.Red;
            this.lbButton1.Label = "";
            this.lbButton1.Location = new System.Drawing.Point(109, 343);
            this.lbButton1.Name = "lbButton1";
            this.lbButton1.Renderer = null;
            this.lbButton1.RepeatInterval = 100;
            this.lbButton1.RepeatState = false;
            this.lbButton1.Size = new System.Drawing.Size(50, 50);
            this.lbButton1.StartRepeatInterval = 500;
            this.lbButton1.State = LBSoft.IndustrialCtrls.Buttons.LBButton.ButtonState.Normal;
            this.lbButton1.Style = LBSoft.IndustrialCtrls.Buttons.LBButton.ButtonStyle.Circular;
            this.lbButton1.TabIndex = 8;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SensorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 505);
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
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed1;
        private LBSoft.IndustrialCtrls.Buttons.LBButton lbButton1;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob lbKnob1;
        private LBSoft.IndustrialCtrls.Buttons.LBButton lbButton2;
        private UserControls.MagneticGuide8Bit magneticGuide8Bit1;
        private UserControls.RFID rfid1;
        private System.Windows.Forms.Timer timer1;
        private UserControls.MagneticGuide8Bit magneticGuide8Bit4;
        private UserControls.MagneticGuide8Bit magneticGuide8Bit3;
        private UserControls.MagneticGuide8Bit magneticGuide8Bit2;
    }
}