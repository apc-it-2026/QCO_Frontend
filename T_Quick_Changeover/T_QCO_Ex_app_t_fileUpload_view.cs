using SJeMES_Control_Library;
using SJeMES_Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class T_QCO_Ex_app_t_fileUpload_view : Form
    {
        public static string sequenceID;
        string[] files;
        private string folderPath = @"E:\AEQS_Project_Related_Files\AEQS_Source_Code\deliveryFile\Client\T_Quick_Changeover\FileUploads";
        public T_QCO_Ex_app_t_fileUpload_view()
        {
            InitializeComponent();
            
        }
        private void T_QCO_Ex_app_t_fileUpload_view_Load_1(object sender, EventArgs e)
        {
            LoadFilesToDataGridView(sequenceID);
            CopyColumnNameToRows();         
        }
        public void Addeseq2(string seq)
        {
            sequenceID = seq;
        }       
        private void CopyColumnNameToRows()
        {
            if (dataGridView1.Columns.Count > 0)
            {
                string viewColumnName = "View";
                string deleteColumnName = "Delete";

                if (dataGridView1.Columns.Contains(viewColumnName))
                {
                    int viewColumnIndex = dataGridView1.Columns[viewColumnName].Index;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {                       
                        row.Cells[viewColumnName].Value = dataGridView1.Columns[viewColumnIndex].HeaderText;
                    }
                }
                else
                {
                    MessageBox.Show($"{viewColumnName} column not found in the Attachment.");
                }

                if (dataGridView1.Columns.Contains(deleteColumnName))
                {
                    int deleteColumnIndex = dataGridView1.Columns[deleteColumnName].Index;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Cells[deleteColumnName].Value = dataGridView1.Columns[deleteColumnIndex].HeaderText;
                    }
                }
                else
                {
                    MessageBox.Show($"{deleteColumnName} column not found in the DataGridView.");
                }

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageBox.Show("No Files found.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("No columns in the DataGridView.");
            }
        }


        private void LoadFilesToDataGridView(string sequenceID)
        {
            if (Directory.Exists(folderPath))
            {
                files = Directory.GetFiles(folderPath);
                DataTable dt = new DataTable();
                dt.Columns.Add("FileName");

                foreach (string file in files)
                {
                    if (CheckSeq(file, sequenceID))
                    {
                        DataRow row = dt.NewRow();
                        row["FileName"] = Path.GetFileName(file);
                        dt.Rows.Add(row);
                    }
                }

                dataGridView1.DataSource = dt;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.AutoResizeColumns();
                CopyColumnNameToRows();
                RemoveRowsWithEmptyFileName();
            }
            else
            {
                MessageHelper.ShowErr(this, "Folder not found!");
            }
        }

        public bool CheckSeq(string filePath, string sequenceID)
        {
            // Implement your logic to check if the file name contains the sequenceID
            return Path.GetFileName(filePath).Contains(sequenceID);
        }
        private void RemoveRowsWithEmptyFileName()
        {
            // Finish editing before attempting to remove rows
            dataGridView1.EndEdit();

            for (int i = dataGridView1.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dataGridView1.Rows[i];
                if (string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["FileName"].Value)))
                {
                    if (!row.IsNewRow)
                    {
                        dataGridView1.Rows.Remove(row);
                    }
                }
            }
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if a valid row is clicked
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string fileName = selectedRow.Cells["FileName"].Value.ToString();
                if (e.ColumnIndex == 1) 
                {
                    string filePath = Path.Combine(folderPath, fileName);
                    if (File.Exists(filePath))
                    {
                         Process.Start(filePath); 
                    }
                    else
                    {
                        MessageHelper.ShowErr(this, "File not found!");
                    }
                }
                else if (e.ColumnIndex == 0) 
                {
                    // Perform the action to delete the file
                    string filePath = Path.Combine(folderPath, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        LoadFilesToDataGridView(sequenceID); 
                    }
                    else
                    {
                        MessageHelper.ShowErr(this, "File not found!");
                    }
                }
            }
        }

    }

}
