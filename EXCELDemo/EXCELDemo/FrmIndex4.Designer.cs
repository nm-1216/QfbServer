namespace ExcelUp
{
    partial class FrmIndex4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIndex4));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboxUser = new System.Windows.Forms.ComboBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cbbLJ = new System.Windows.Forms.ComboBox();
            this.cbbXM = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GvList = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckBox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ProjectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboxAll = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.cboxUser);
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.cbbLJ);
            this.panel1.Controls.Add(this.cbbXM);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(5, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1004, 43);
            this.panel1.TabIndex = 2;
            // 
            // cboxUser
            // 
            this.cboxUser.FormattingEnabled = true;
            this.cboxUser.Location = new System.Drawing.Point(816, 12);
            this.cboxUser.Name = "cboxUser";
            this.cboxUser.Size = new System.Drawing.Size(102, 20);
            this.cboxUser.TabIndex = 3;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(927, 10);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "下 发";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(575, 10);
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
            this.label2.Location = new System.Drawing.Point(743, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "下发给用户：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "零件号：";
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
            this.GvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.CheckBox,
            this.ProjectNo,
            this.PartNo,
            this.PartName,
            this.CreateTime});
            this.GvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GvList.Location = new System.Drawing.Point(5, 103);
            this.GvList.Name = "GvList";
            this.GvList.RowTemplate.Height = 23;
            this.GvList.Size = new System.Drawing.Size(1004, 338);
            this.GvList.TabIndex = 3;
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
            // CheckBox
            // 
            this.CheckBox.HeaderText = "";
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.Width = 30;
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
            // cboxAll
            // 
            this.cboxAll.AutoSize = true;
            this.cboxAll.Location = new System.Drawing.Point(55, 108);
            this.cboxAll.Name = "cboxAll";
            this.cboxAll.Size = new System.Drawing.Size(15, 14);
            this.cboxAll.TabIndex = 4;
            this.cboxAll.UseVisualStyleBackColor = true;
            this.cboxAll.CheckedChanged += new System.EventHandler(this.cboxAll_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1004, 60);
            this.panel2.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(10, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 42);
            this.label4.TabIndex = 2;
            this.label4.Text = "标准下发";
            // 
            // FrmIndex4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 441);
            this.Controls.Add(this.cboxAll);
            this.Controls.Add(this.GvList);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmIndex4";
            this.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上海赛科利质量测量数据管理平台系统 - 标准下发";
            this.Load += new System.EventHandler(this.FrmIndex4_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GvList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cbbLJ;
        private System.Windows.Forms.ComboBox cbbXM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView GvList;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateTime;
        private System.Windows.Forms.CheckBox cboxAll;
        private System.Windows.Forms.ComboBox cboxUser;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
    }
}