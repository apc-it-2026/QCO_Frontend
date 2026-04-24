
namespace T_Quick_Changeover
{
    partial class QCO_FileUpload
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
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_commit = new System.Windows.Forms.Button();
            this.btn_file_upload = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.lbl_file_name = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.DateTimePicker();
            this.starttime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_cancel.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_cancel.Location = new System.Drawing.Point(96, 266);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(101, 36);
            this.btn_cancel.TabIndex = 18;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = false;
            // 
            // btn_commit
            // 
            this.btn_commit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_commit.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_commit.Location = new System.Drawing.Point(227, 266);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(101, 36);
            this.btn_commit.TabIndex = 17;
            this.btn_commit.Text = "Confirm";
            this.btn_commit.UseVisualStyleBackColor = false;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // btn_file_upload
            // 
            this.btn_file_upload.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btn_file_upload.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_file_upload.Location = new System.Drawing.Point(149, 107);
            this.btn_file_upload.Name = "btn_file_upload";
            this.btn_file_upload.Size = new System.Drawing.Size(202, 36);
            this.btn_file_upload.TabIndex = 16;
            this.btn_file_upload.Text = "+";
            this.btn_file_upload.UseVisualStyleBackColor = false;
            this.btn_file_upload.Click += new System.EventHandler(this.btn_file_upload_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 18);
            this.label2.TabIndex = 15;
            this.label2.Text = "Select a document";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.Location = new System.Drawing.Point(149, 162);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(154, 22);
            this.radioButton2.TabIndex = 21;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Select file to upload";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // lbl_file_name
            // 
            this.lbl_file_name.AutoSize = true;
            this.lbl_file_name.Location = new System.Drawing.Point(120, 180);
            this.lbl_file_name.Name = "lbl_file_name";
            this.lbl_file_name.Size = new System.Drawing.Size(0, 13);
            this.lbl_file_name.TabIndex = 22;
            // 
            // time
            // 
            this.time.Location = new System.Drawing.Point(149, 203);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(143, 20);
            this.time.TabIndex = 47;
            // 
            // starttime
            // 
            this.starttime.AutoSize = true;
            this.starttime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.starttime.Location = new System.Drawing.Point(53, 203);
            this.starttime.Name = "starttime";
            this.starttime.Size = new System.Drawing.Size(90, 18);
            this.starttime.TabIndex = 48;
            this.starttime.Text = "Starting time";
            // 
            // QCO_FileUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 346);
            this.Controls.Add(this.starttime);
            this.Controls.Add(this.time);
            this.Controls.Add(this.lbl_file_name);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.btn_file_upload);
            this.Controls.Add(this.label2);
            this.Name = "QCO_FileUpload";
            this.Text = "Uupload files";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Button btn_file_upload;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label lbl_file_name;
        private System.Windows.Forms.DateTimePicker time;
        private System.Windows.Forms.Label starttime;
    }
}