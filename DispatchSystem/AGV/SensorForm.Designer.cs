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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbLed1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.rfid1 = new DispatchSystem.UserControls.RFID();
            this.magneticGuide16Bit4 = new DispatchSystem.UserControls.MagneticGuide16Bit();
            this.magneticGuide16Bit3 = new DispatchSystem.UserControls.MagneticGuide16Bit();
            this.magneticGuide16Bit2 = new DispatchSystem.UserControls.MagneticGuide16Bit();
            this.magneticGuide16Bit1 = new DispatchSystem.UserControls.MagneticGuide16Bit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbLed1);
            this.splitContainer1.Panel1.Controls.Add(this.rfid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide16Bit4);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide16Bit3);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide16Bit2);
            this.splitContainer1.Panel2.Controls.Add(this.magneticGuide16Bit1);
            this.splitContainer1.Size = new System.Drawing.Size(842, 547);
            this.splitContainer1.SplitterDistance = 284;
            this.splitContainer1.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbLed1
            // 
            this.lbLed1.BackColor = System.Drawing.Color.Transparent;
            this.lbLed1.BlinkInterval = 100;
            this.lbLed1.Label = "";
            this.lbLed1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLed1.LedColor = System.Drawing.Color.Green;
            this.lbLed1.LedOffColor = System.Drawing.Color.Gray;
            this.lbLed1.LedSize = new System.Drawing.SizeF(50F, 50F);
            this.lbLed1.LedState = LBSoft.IndustrialCtrls.Leds.LBLed.ledState.Blink;
            this.lbLed1.Location = new System.Drawing.Point(0, 0);
            this.lbLed1.Name = "lbLed1";
            this.lbLed1.Renderer = null;
            this.lbLed1.Size = new System.Drawing.Size(74, 69);
            this.lbLed1.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed1.TabIndex = 17;
            this.lbLed1.Trigger = false;
            // 
            // rfid1
            // 
            this.rfid1.Location = new System.Drawing.Point(12, 256);
            this.rfid1.Name = "rfid1";
            this.rfid1.PowerLed = true;
            this.rfid1.PowerOffColor = System.Drawing.Color.Red;
            this.rfid1.PowerOnColor = System.Drawing.Color.Green;
            this.rfid1.Size = new System.Drawing.Size(259, 276);
            this.rfid1.TabIndex = 16;
            this.rfid1.Value = 0;
            this.rfid1.ValueColor = System.Drawing.Color.Magenta;
            this.rfid1.ValueLed = false;
            this.rfid1.ValueOffColor = System.Drawing.Color.Red;
            this.rfid1.ValueOnColor = System.Drawing.Color.Yellow;
            // 
            // magneticGuide16Bit4
            // 
            this.magneticGuide16Bit4.BorderColor = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit4.ColorOff = System.Drawing.Color.Lime;
            this.magneticGuide16Bit4.ColorOn = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit4.Location = new System.Drawing.Point(20, 407);
            this.magneticGuide16Bit4.Name = "magneticGuide16Bit4";
            this.magneticGuide16Bit4.Size = new System.Drawing.Size(515, 125);
            this.magneticGuide16Bit4.TabIndex = 24;
            this.magneticGuide16Bit4.Value = 21845;
            // 
            // magneticGuide16Bit3
            // 
            this.magneticGuide16Bit3.BorderColor = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit3.ColorOff = System.Drawing.Color.Lime;
            this.magneticGuide16Bit3.ColorOn = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit3.Location = new System.Drawing.Point(20, 276);
            this.magneticGuide16Bit3.Name = "magneticGuide16Bit3";
            this.magneticGuide16Bit3.Size = new System.Drawing.Size(515, 125);
            this.magneticGuide16Bit3.TabIndex = 23;
            this.magneticGuide16Bit3.Value = 21845;
            // 
            // magneticGuide16Bit2
            // 
            this.magneticGuide16Bit2.BorderColor = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit2.ColorOff = System.Drawing.Color.Lime;
            this.magneticGuide16Bit2.ColorOn = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit2.Location = new System.Drawing.Point(20, 145);
            this.magneticGuide16Bit2.Name = "magneticGuide16Bit2";
            this.magneticGuide16Bit2.Size = new System.Drawing.Size(515, 125);
            this.magneticGuide16Bit2.TabIndex = 22;
            this.magneticGuide16Bit2.Value = 21845;
            // 
            // magneticGuide16Bit1
            // 
            this.magneticGuide16Bit1.BorderColor = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit1.ColorOff = System.Drawing.Color.Lime;
            this.magneticGuide16Bit1.ColorOn = System.Drawing.Color.DimGray;
            this.magneticGuide16Bit1.Location = new System.Drawing.Point(20, 14);
            this.magneticGuide16Bit1.Name = "magneticGuide16Bit1";
            this.magneticGuide16Bit1.Size = new System.Drawing.Size(515, 125);
            this.magneticGuide16Bit1.TabIndex = 21;
            this.magneticGuide16Bit1.Value = 21845;
            // 
            // SensorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 547);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SensorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "传感器界面";
            this.Load += new System.EventHandler(this.SensorForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UserControls.RFID rfid1;
        private System.Windows.Forms.Timer timer1;
        private UserControls.MagneticGuide16Bit magneticGuide16Bit4;
        private UserControls.MagneticGuide16Bit magneticGuide16Bit3;
        private UserControls.MagneticGuide16Bit magneticGuide16Bit2;
        private UserControls.MagneticGuide16Bit magneticGuide16Bit1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed1;
    }
}