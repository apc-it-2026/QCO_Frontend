
namespace T_Quick_Changeover
{
    partial class T_QCO_RAMP_UP
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Submitbtn = new System.Windows.Forms.Button();
            this.txttargetreach = new System.Windows.Forms.TextBox();
            this.txtNewModelFirstPair = new System.Windows.Forms.TextBox();
            this.Calculate_Ramp_Upbtn = new System.Windows.Forms.Button();
            this.targettimepicker = new System.Windows.Forms.DateTimePicker();
            this.lblResult = new System.Windows.Forms.Label();
            this.Secondproductlbl = new System.Windows.Forms.Label();
            this.FirstProductlbl = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Headinglbl = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Submitbtn);
            this.panel1.Controls.Add(this.txttargetreach);
            this.panel1.Controls.Add(this.txtNewModelFirstPair);
            this.panel1.Controls.Add(this.Calculate_Ramp_Upbtn);
            this.panel1.Controls.Add(this.targettimepicker);
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Controls.Add(this.Secondproductlbl);
            this.panel1.Controls.Add(this.FirstProductlbl);
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 414);
            this.panel1.TabIndex = 0;
            // 
            // Submitbtn
            // 
            this.Submitbtn.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Submitbtn.ForeColor = System.Drawing.Color.DarkRed;
            this.Submitbtn.Location = new System.Drawing.Point(262, 300);
            this.Submitbtn.Name = "Submitbtn";
            this.Submitbtn.Size = new System.Drawing.Size(68, 33);
            this.Submitbtn.TabIndex = 9;
            this.Submitbtn.Text = "Submit";
            this.Submitbtn.UseVisualStyleBackColor = true;
            this.Submitbtn.Click += new System.EventHandler(this.Submitbtn_Click);
            // 
            // txttargetreach
            // 
            this.txttargetreach.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txttargetreach.Location = new System.Drawing.Point(336, 131);
            this.txttargetreach.Name = "txttargetreach";
            this.txttargetreach.Size = new System.Drawing.Size(198, 22);
            this.txttargetreach.TabIndex = 8;
            // 
            // txtNewModelFirstPair
            // 
            this.txtNewModelFirstPair.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewModelFirstPair.Location = new System.Drawing.Point(246, 81);
            this.txtNewModelFirstPair.Name = "txtNewModelFirstPair";
            this.txtNewModelFirstPair.Size = new System.Drawing.Size(198, 22);
            this.txtNewModelFirstPair.TabIndex = 7;
            // 
            // Calculate_Ramp_Upbtn
            // 
            this.Calculate_Ramp_Upbtn.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Calculate_Ramp_Upbtn.ForeColor = System.Drawing.Color.DarkRed;
            this.Calculate_Ramp_Upbtn.Location = new System.Drawing.Point(262, 202);
            this.Calculate_Ramp_Upbtn.Name = "Calculate_Ramp_Upbtn";
            this.Calculate_Ramp_Upbtn.Size = new System.Drawing.Size(167, 33);
            this.Calculate_Ramp_Upbtn.TabIndex = 5;
            this.Calculate_Ramp_Upbtn.Text = "Calculate Ramp-Up Time";
            this.Calculate_Ramp_Upbtn.UseVisualStyleBackColor = true;
            this.Calculate_Ramp_Upbtn.Click += new System.EventHandler(this.Calculate_Ramp_Upbtn_Click);
            // 
            // targettimepicker
            // 
            this.targettimepicker.Location = new System.Drawing.Point(227, 131);
            this.targettimepicker.MinDate = new System.DateTime(2024, 3, 1, 0, 0, 0, 0);
            this.targettimepicker.Name = "targettimepicker";
            this.targettimepicker.Size = new System.Drawing.Size(103, 20);
            this.targettimepicker.TabIndex = 4;
            this.targettimepicker.KeyDown += new System.Windows.Forms.KeyEventHandler(this.startTimeProduct2_KeyDown);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResult.Location = new System.Drawing.Point(258, 260);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(14, 19);
            this.lblResult.TabIndex = 2;
            this.lblResult.Text = ":";
            // 
            // Secondproductlbl
            // 
            this.Secondproductlbl.AutoSize = true;
            this.Secondproductlbl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Secondproductlbl.Location = new System.Drawing.Point(38, 131);
            this.Secondproductlbl.Name = "Secondproductlbl";
            this.Secondproductlbl.Size = new System.Drawing.Size(159, 19);
            this.Secondproductlbl.TabIndex = 1;
            this.Secondproductlbl.Text = "Target Reached Time:";
            // 
            // FirstProductlbl
            // 
            this.FirstProductlbl.AutoSize = true;
            this.FirstProductlbl.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstProductlbl.Location = new System.Drawing.Point(38, 82);
            this.FirstProductlbl.Name = "FirstProductlbl";
            this.FirstProductlbl.Size = new System.Drawing.Size(162, 19);
            this.FirstProductlbl.TabIndex = 0;
            this.FirstProductlbl.Text = "New Model First Pair :";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SeaShell;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.Headinglbl);
            this.panel2.ForeColor = System.Drawing.Color.Crimson;
            this.panel2.Location = new System.Drawing.Point(0, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 58);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // Headinglbl
            // 
            this.Headinglbl.AutoSize = true;
            this.Headinglbl.Font = new System.Drawing.Font("Trajan Pro", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Headinglbl.ForeColor = System.Drawing.Color.Crimson;
            this.Headinglbl.Location = new System.Drawing.Point(175, 20);
            this.Headinglbl.Name = "Headinglbl";
            this.Headinglbl.Size = new System.Drawing.Size(234, 20);
            this.Headinglbl.TabIndex = 3;
            this.Headinglbl.Text = "Calculate Ramp-Up Time";
            // 
            // T_QCO_RAMP_UP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 480);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "T_QCO_RAMP_UP";
            this.Text = "QCO_Ramp_Up_Time";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Calculate_Ramp_Upbtn;
        private System.Windows.Forms.DateTimePicker targettimepicker;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label Secondproductlbl;
        private System.Windows.Forms.Label FirstProductlbl;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Headinglbl;
        private System.Windows.Forms.TextBox txttargetreach;
        private System.Windows.Forms.TextBox txtNewModelFirstPair;
        private System.Windows.Forms.Button Submitbtn;
    }
}