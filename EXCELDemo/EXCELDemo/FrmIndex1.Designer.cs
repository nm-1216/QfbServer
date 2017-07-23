namespace ExcelUp
{
    partial class FrmIndex1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIndex1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GvList = new System.Windows.Forms.DataGridView();
            this.Pid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Iid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1004, 60);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(131, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "项目号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "检测项";
            // 
            // GvList
            // 
            this.GvList.AllowUserToAddRows = false;
            this.GvList.AllowUserToDeleteRows = false;
            this.GvList.AllowUserToResizeRows = false;
            this.GvList.BackgroundColor = System.Drawing.Color.White;
            this.GvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Pid,
            this.Iid,
            this.name});
            this.GvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GvList.Location = new System.Drawing.Point(5, 60);
            this.GvList.Name = "GvList";
            this.GvList.RowTemplate.Height = 23;
            this.GvList.Size = new System.Drawing.Size(1004, 381);
            this.GvList.TabIndex = 1;
            this.GvList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvList_CellDoubleClick);
            this.GvList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.GvList_RowPostPaint);
            // 
            // Pid
            // 
            this.Pid.DataPropertyName = "MeasReportID";
            this.Pid.HeaderText = "Pid";
            this.Pid.Name = "Pid";
            this.Pid.ReadOnly = true;
            this.Pid.Visible = false;
            // 
            // Iid
            // 
            this.Iid.DataPropertyName = "MeasItemID";
            this.Iid.HeaderText = "Iid";
            this.Iid.Name = "Iid";
            this.Iid.ReadOnly = true;
            this.Iid.Visible = false;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "MeasItemNO";
            this.name.HeaderText = "检测项";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // FrmIndex1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 441);
            this.Controls.Add(this.GvList);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmIndex1";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上海赛科利质量测量数据管理平台系统 - 检测项";
            this.Load += new System.EventHandler(this.FrmIndex1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView GvList;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Iid;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.Label label2;
    }
}