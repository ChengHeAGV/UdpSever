namespace DispatchSystem.UserControls
{
    partial class ExConsole
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.exListView1 = new DispatchSystem.ExListView();
            this.SuspendLayout();
            // 
            // exListView1
            // 
            this.exListView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exListView1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.exListView1.FullRowSelect = true;
            this.exListView1.HideSelection = false;
            this.exListView1.Location = new System.Drawing.Point(0, 0);
            this.exListView1.Name = "exListView1";
            this.exListView1.Size = new System.Drawing.Size(150, 150);
            this.exListView1.TabIndex = 0;
            this.exListView1.UseCompatibleStateImageBehavior = false;
            this.exListView1.View = System.Windows.Forms.View.Details;
            // 
            // ExConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exListView1);
            this.Name = "ExConsole";
            this.Load += new System.EventHandler(this.ExConsole_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ExListView exListView1;
    }
}
