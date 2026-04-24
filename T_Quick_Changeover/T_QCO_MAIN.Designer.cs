
namespace T_Quick_Changeover
{
    partial class T_QCO_MAIN
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
            this.PRODUCTIONSOP = new System.Windows.Forms.TabPage();
            this.SAMPLESHOE = new System.Windows.Forms.TabPage();
            this.Equipment_Request = new System.Windows.Forms.TabPage();
            this.Checklist = new System.Windows.Forms.TabPage();
            this.pages = new System.Windows.Forms.TabControl();
            this.SchedulePlan_Upload = new System.Windows.Forms.TabPage();
            this.Schedulereport = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Sch_plan_import = new System.Windows.Forms.Button();
            this.Plan_report = new System.Windows.Forms.Button();
            this.UNPLANNEDBTN = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pages.SuspendLayout();
            this.sidebar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PRODUCTIONSOP
            // 
            this.PRODUCTIONSOP.Location = new System.Drawing.Point(4, 4);
            this.PRODUCTIONSOP.Name = "PRODUCTIONSOP";
            this.PRODUCTIONSOP.Padding = new System.Windows.Forms.Padding(3);
            this.PRODUCTIONSOP.Size = new System.Drawing.Size(1028, 620);
            this.PRODUCTIONSOP.TabIndex = 4;
            this.PRODUCTIONSOP.Text = "PRODUCTION SOP";
            this.PRODUCTIONSOP.UseVisualStyleBackColor = true;
            // 
            // SAMPLESHOE
            // 
            this.SAMPLESHOE.Location = new System.Drawing.Point(4, 4);
            this.SAMPLESHOE.Name = "SAMPLESHOE";
            this.SAMPLESHOE.Padding = new System.Windows.Forms.Padding(3);
            this.SAMPLESHOE.Size = new System.Drawing.Size(1028, 620);
            this.SAMPLESHOE.TabIndex = 3;
            this.SAMPLESHOE.Text = "SAMPLE SHOE";
            this.SAMPLESHOE.UseVisualStyleBackColor = true;
            // 
            // Equipment_Request
            // 
            this.Equipment_Request.Location = new System.Drawing.Point(4, 4);
            this.Equipment_Request.Name = "Equipment_Request";
            this.Equipment_Request.Padding = new System.Windows.Forms.Padding(3);
            this.Equipment_Request.Size = new System.Drawing.Size(1028, 620);
            this.Equipment_Request.TabIndex = 2;
            this.Equipment_Request.Text = "Equipment_Request";
            this.Equipment_Request.UseVisualStyleBackColor = true;
            // 
            // Checklist
            // 
            this.Checklist.Location = new System.Drawing.Point(4, 4);
            this.Checklist.Name = "Checklist";
            this.Checklist.Padding = new System.Windows.Forms.Padding(3);
            this.Checklist.Size = new System.Drawing.Size(1028, 620);
            this.Checklist.TabIndex = 1;
            this.Checklist.Text = "Checklist";
            this.Checklist.UseVisualStyleBackColor = true;
            // 
            // pages
            // 
            this.pages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.pages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pages.Controls.Add(this.SchedulePlan_Upload);
            this.pages.Controls.Add(this.Schedulereport);
            this.pages.Controls.Add(this.Checklist);
            this.pages.Controls.Add(this.Equipment_Request);
            this.pages.Controls.Add(this.SAMPLESHOE);
            this.pages.Controls.Add(this.PRODUCTIONSOP);
            this.pages.Location = new System.Drawing.Point(278, 72);
            this.pages.Multiline = true;
            this.pages.Name = "pages";
            this.pages.SelectedIndex = 0;
            this.pages.Size = new System.Drawing.Size(1036, 691);
            this.pages.TabIndex = 8;
            // 
            // SchedulePlan_Upload
            // 
            this.SchedulePlan_Upload.Location = new System.Drawing.Point(4, 4);
            this.SchedulePlan_Upload.Name = "SchedulePlan_Upload";
            this.SchedulePlan_Upload.Size = new System.Drawing.Size(1028, 665);
            this.SchedulePlan_Upload.TabIndex = 5;
            this.SchedulePlan_Upload.Text = "SchedulePlan";
            this.SchedulePlan_Upload.UseVisualStyleBackColor = true;
            // 
            // Schedulereport
            // 
            this.Schedulereport.Location = new System.Drawing.Point(4, 4);
            this.Schedulereport.Name = "Schedulereport";
            this.Schedulereport.Size = new System.Drawing.Size(1028, 620);
            this.Schedulereport.TabIndex = 6;
            this.Schedulereport.Text = "Scheduleplan";
            this.Schedulereport.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::T_Quick_Changeover.Properties.Resources.qco_titl;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(276, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1038, 70);
            this.panel1.TabIndex = 9;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // sidebar
            // 
            this.sidebar.BackColor = System.Drawing.Color.Transparent;
            this.sidebar.BackgroundImage = global::T_Quick_Changeover.Properties.Resources.side_bav_bg;
            this.sidebar.Controls.Add(this.pictureBox1);
            this.sidebar.Controls.Add(this.Sch_plan_import);
            this.sidebar.Controls.Add(this.Plan_report);
            this.sidebar.Controls.Add(this.UNPLANNEDBTN);
            this.sidebar.Controls.Add(this.button1);
            this.sidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidebar.ForeColor = System.Drawing.SystemColors.Control;
            this.sidebar.Location = new System.Drawing.Point(0, 0);
            this.sidebar.Name = "sidebar";
            this.sidebar.Size = new System.Drawing.Size(276, 763);
            this.sidebar.TabIndex = 7;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.Image = global::T_Quick_Changeover.Properties.Resources.apc_logoo_removebg_preview;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(274, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Sch_plan_import
            // 
            this.Sch_plan_import.BackColor = System.Drawing.Color.Transparent;
            this.Sch_plan_import.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Sch_plan_import.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Sch_plan_import.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sch_plan_import.ForeColor = System.Drawing.Color.Black;
            this.Sch_plan_import.Image = global::T_Quick_Changeover.Properties.Resources.schedule;
            this.Sch_plan_import.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Sch_plan_import.Location = new System.Drawing.Point(3, 82);
            this.Sch_plan_import.Name = "Sch_plan_import";
            this.Sch_plan_import.Size = new System.Drawing.Size(274, 44);
            this.Sch_plan_import.TabIndex = 1;
            this.Sch_plan_import.Text = "SCHEDULE PLAN IMPORT";
            this.Sch_plan_import.UseVisualStyleBackColor = false;
            this.Sch_plan_import.Click += new System.EventHandler(this.Sch_plan_import_Click);
            // 
            // Plan_report
            // 
            this.Plan_report.BackColor = System.Drawing.Color.Transparent;
            this.Plan_report.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Plan_report.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Plan_report.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Plan_report.ForeColor = System.Drawing.Color.Black;
            this.Plan_report.Image = global::T_Quick_Changeover.Properties.Resources.schedule;
            this.Plan_report.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Plan_report.Location = new System.Drawing.Point(3, 132);
            this.Plan_report.Name = "Plan_report";
            this.Plan_report.Size = new System.Drawing.Size(275, 46);
            this.Plan_report.TabIndex = 5;
            this.Plan_report.Text = "SCHEDULE PLAN REPORT";
            this.Plan_report.UseVisualStyleBackColor = false;
            this.Plan_report.Click += new System.EventHandler(this.Plan_report_Click);
            // 
            // UNPLANNEDBTN
            // 
            this.UNPLANNEDBTN.BackColor = System.Drawing.Color.Transparent;
            this.UNPLANNEDBTN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UNPLANNEDBTN.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UNPLANNEDBTN.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UNPLANNEDBTN.ForeColor = System.Drawing.Color.Black;
            this.UNPLANNEDBTN.Image = global::T_Quick_Changeover.Properties.Resources.regulation;
            this.UNPLANNEDBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.UNPLANNEDBTN.Location = new System.Drawing.Point(3, 184);
            this.UNPLANNEDBTN.Name = "UNPLANNEDBTN";
            this.UNPLANNEDBTN.Size = new System.Drawing.Size(275, 40);
            this.UNPLANNEDBTN.TabIndex = 6;
            this.UNPLANNEDBTN.Text = "UNPLANEED CHANGEOVER ";
            this.UNPLANNEDBTN.UseVisualStyleBackColor = false;
            this.UNPLANNEDBTN.Click += new System.EventHandler(this.UNPLANNEDBTN_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::T_Quick_Changeover.Properties.Resources.ic_select_24;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(3, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(275, 40);
            this.button1.TabIndex = 7;
            this.button1.Text = "QCO DASH BOARDS";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // T_QCO_MAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 763);
            this.Controls.Add(this.pages);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sidebar);
            this.Name = "T_QCO_MAIN";
            this.pages.ResumeLayout(false);
            this.sidebar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button Sch_plan_import;
        private System.Windows.Forms.FlowLayoutPanel sidebar;
        private System.Windows.Forms.TabPage PRODUCTIONSOP;
        private System.Windows.Forms.TabPage SAMPLESHOE;
        private System.Windows.Forms.TabPage Equipment_Request;
        private System.Windows.Forms.TabPage Checklist;
        private System.Windows.Forms.TabControl pages;
        private System.Windows.Forms.TabPage SchedulePlan_Upload;

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem UpdateToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button butquery;
        private System.Windows.Forms.TextBox bArttxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        // private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button butImport;
        private System.Windows.Forms.Button butFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox BModel;
        // private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn File;
        private System.Windows.Forms.TextBox afAtrtxt;
        // private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        // private System.Windows.Forms.Label label5;
        private AutocompleteMenuNS.AutocompleteMenu autocompleteMenu1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Button Searchbtn;
        private System.Windows.Forms.DateTimePicker Todate;
        private System.Windows.Forms.DateTimePicker fromdate;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.TabPage Schedulereport;
        private System.Windows.Forms.Button Plan_report;
        private System.Windows.Forms.Button UNPLANNEDBTN;
        private System.Windows.Forms.Button button1;
    }
}

