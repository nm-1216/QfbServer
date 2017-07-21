namespace ExcelUp
{
    partial class FrmIndex
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIndex));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.测量标准管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测量标准下发ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.历史查询与分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.测量标准导入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbbLJ = new System.Windows.Forms.ComboBox();
            this.cbbXM = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GvList = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Show = new System.Windows.Forms.DataGridViewButtonColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.测量标准管理ToolStripMenuItem,
            this.测量标准下发ToolStripMenuItem,
            this.历史查询与分析ToolStripMenuItem,
            this.用户管理ToolStripMenuItem,
            this.测量标准导入ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1014, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 测量标准管理ToolStripMenuItem
            // 
            this.测量标准管理ToolStripMenuItem.Name = "测量标准管理ToolStripMenuItem";
            this.测量标准管理ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.测量标准管理ToolStripMenuItem.Text = "测量标准管理";
            // 
            // 测量标准下发ToolStripMenuItem
            // 
            this.测量标准下发ToolStripMenuItem.Name = "测量标准下发ToolStripMenuItem";
            this.测量标准下发ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.测量标准下发ToolStripMenuItem.Text = "测量标准下发";
            this.测量标准下发ToolStripMenuItem.Click += new System.EventHandler(this.测量标准下发ToolStripMenuItem_Click);
            // 
            // 历史查询与分析ToolStripMenuItem
            // 
            this.历史查询与分析ToolStripMenuItem.Name = "历史查询与分析ToolStripMenuItem";
            this.历史查询与分析ToolStripMenuItem.Size = new System.Drawing.Size(104, 21);
            this.历史查询与分析ToolStripMenuItem.Text = "历史查询与分析";
            this.历史查询与分析ToolStripMenuItem.Click += new System.EventHandler(this.历史查询与分析ToolStripMenuItem_Click);
            // 
            // 用户管理ToolStripMenuItem
            // 
            this.用户管理ToolStripMenuItem.Name = "用户管理ToolStripMenuItem";
            this.用户管理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.用户管理ToolStripMenuItem.Text = "用户管理";
            this.用户管理ToolStripMenuItem.Click += new System.EventHandler(this.用户管理ToolStripMenuItem_Click);
            // 
            // 测量标准导入ToolStripMenuItem
            // 
            this.测量标准导入ToolStripMenuItem.Name = "测量标准导入ToolStripMenuItem";
            this.测量标准导入ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.测量标准导入ToolStripMenuItem.Text = "测量标准导入";
            this.测量标准导入ToolStripMenuItem.Click += new System.EventHandler(this.测量标准导入ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.cbbLJ);
            this.panel1.Controls.Add(this.cbbXM);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 43);
            this.panel1.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(607, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查 询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cbbLJ
            // 
            this.cbbLJ.FormattingEnabled = true;
            this.cbbLJ.Location = new System.Drawing.Point(355, 11);
            this.cbbLJ.Name = "cbbLJ";
            this.cbbLJ.Size = new System.Drawing.Size(200, 20);
            this.cbbLJ.TabIndex = 1;
            // 
            // cbbXM
            // 
            this.cbbXM.FormattingEnabled = true;
            this.cbbXM.Location = new System.Drawing.Point(69, 11);
            this.cbbXM.Name = "cbbXM";
            this.cbbXM.Size = new System.Drawing.Size(200, 20);
            this.cbbXM.TabIndex = 0;
            this.cbbXM.SelectedIndexChanged += new System.EventHandler(this.cbbXM_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "零件号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "项目号：";
            // 
            // GvList
            // 
            this.GvList.AllowUserToAddRows = false;
            this.GvList.AllowUserToDeleteRows = false;
            this.GvList.AllowUserToResizeRows = false;
            this.GvList.BackgroundColor = System.Drawing.Color.White;
            this.GvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.ProjectNo,
            this.PartNo,
            this.PartName,
            this.CreateTime,
            this.Delete,
            this.Show});
            this.GvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GvList.Location = new System.Drawing.Point(0, 68);
            this.GvList.Name = "GvList";
            this.GvList.RowTemplate.Height = 23;
            this.GvList.Size = new System.Drawing.Size(1014, 302);
            this.GvList.TabIndex = 2;
            this.GvList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvList_CellContentClick);
            this.GvList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.GvList_RowPostPaint);
            // 
            // id
            // 
            this.id.DataPropertyName = "MeasReportID";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // ProjectNo
            // 
            this.ProjectNo.DataPropertyName = "ProjectNo";
            this.ProjectNo.HeaderText = "项目号";
            this.ProjectNo.Name = "ProjectNo";
            this.ProjectNo.ReadOnly = true;
            // 
            // PartNo
            // 
            this.PartNo.DataPropertyName = "PartNo";
            this.PartNo.HeaderText = "零件号";
            this.PartNo.Name = "PartNo";
            this.PartNo.ReadOnly = true;
            this.PartNo.Width = 200;
            // 
            // PartName
            // 
            this.PartName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PartName.DataPropertyName = "PartName";
            this.PartName.HeaderText = "零件名称";
            this.PartName.Name = "PartName";
            this.PartName.ReadOnly = true;
            // 
            // CreateTime
            // 
            this.CreateTime.DataPropertyName = "CreateTime";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd";
            this.CreateTime.DefaultCellStyle = dataGridViewCellStyle1;
            this.CreateTime.HeaderText = "导入日期";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.ReadOnly = true;
            // 
            // Delete
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "删除";
            this.Delete.DefaultCellStyle = dataGridViewCellStyle2;
            this.Delete.HeaderText = "删除";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Delete.Text = "";
            // 
            // Show
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.NullValue = "查询";
            this.Show.DefaultCellStyle = dataGridViewCellStyle3;
            this.Show.HeaderText = "查询";
            this.Show.Name = "Show";
            this.Show.ReadOnly = true;
            this.Show.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Show.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(172, 164);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(656, 23);
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Visible = false;
            // 
            // FrmIndex
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 370);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.GvList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FrmIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上海赛科利质量测量数据管理平台系统";
            this.Load += new System.EventHandler(this.FrmIndex_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 测量标准管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测量标准下发ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史查询与分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测量标准导入ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbLJ;
        private System.Windows.Forms.ComboBox cbbXM;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView GvList;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewButtonColumn Show;
    }
}