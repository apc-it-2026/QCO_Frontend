using AutocompleteMenuNS;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class Unplanned_Changeover : Form
    {
        List<string> productionNamelist = new List<string>();
        List<string> arts ;
        List<string> Aarts ;
        List<string> BModel = new List<string>();
        List<string> AModel = new List<string>();
        DataTable dept;
        DataTable dtJson;

        private TextBox autoCompleteTextBox = new TextBox();
        private List<string> departmentNames = new List<string>(); // Store department names

        public Unplanned_Changeover()
        {
            InitializeComponent();
            InitializeDataGridView();
            DataGridViewButtonColumn saveButtonColumn = new DataGridViewButtonColumn();
            saveButtonColumn.Name = "SaveButton";
            saveButtonColumn.Text = "Save";
            saveButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(saveButtonColumn);

            dataGridView1.CellPainting += DataGridView1_CellPainting;

            
        }
        private void InitializeDataGridView()
        {
            dataGridView1.ColumnCount = 5; 

           // dataGridView1.Columns[0].Name = "ORG_ID";
            dataGridView1.Columns[0].Name = "DEPARTMENT_NAME";
            dataGridView1.Columns[1].Name = "CO_DATE";     
            dataGridView1.Columns[2].Name = "INSERT_USER";
            dataGridView1.Columns[3].Name = "B_MODEL";
            dataGridView1.Columns[4].Name = "A_MODEL";

            dataGridView1.AllowUserToAddRows = true;
            // Attach event to check row count
            autoCompleteTextBox.Visible = false;
            autoCompleteTextBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            autoCompleteTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            autoCompleteTextBox.Leave += AutoCompleteTextBox_Leave;
            autoCompleteTextBox.KeyDown += AutoCompleteTextBox_KeyDown;
            dataGridView1.Controls.Add(autoCompleteTextBox);
        }
        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "SaveButton")
            {
              
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);

                string buttonText = "Save";
                if (dataGridView1.Rows[e.RowIndex].Cells["DEPARTMENT_NAME"].Value == null ||
                    string.IsNullOrWhiteSpace(dataGridView1.Rows[e.RowIndex].Cells["DEPARTMENT_NAME"].Value.ToString()))
                {
                    buttonText = ""; 
                }
                using (Font boldFont = new Font(e.CellStyle.Font, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.Green)) 
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    e.Graphics.DrawString(buttonText, e.CellStyle.Font, brush, e.CellBounds, sf);
                }

                e.Handled = true; 
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["SaveButton"].Index)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string coDate = row.Cells["CO_DATE"].Value?.ToString()?.Trim() ?? "";
                if (string.IsNullOrEmpty(coDate))
                {
                    MessageBox.Show("CO_DATE cannot be empty. Please enter a Changeover Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                Dictionary<string, string> rowData = new Dictionary<string, string>
        {
            { "DEPARTMENT_CODE", row.Cells["DEPARTMENT_CODE"].Value?.ToString() ?? "" },
            { "DEPARTMENT_NAME", row.Cells["DEPARTMENT_NAME"].Value?.ToString() ?? "" },
            { "B_ARTNO", row.Cells["B_ARTNO"].Value?.ToString() ?? "" },
            { "A_ARTNO", row.Cells["A_ARTNO"].Value?.ToString() ?? "" },
            { "B_MODEL", row.Cells["B_MODEL"].Value?.ToString() ?? "" },
            { "A_MODEL", row.Cells["A_MODEL"].Value?.ToString() ?? "" },
            { "CO_DATE", coDate },
            { "INSERT_USER", row.Cells["INSERT_USER"].Value?.ToString() ?? "" },
            { "SEQUENCE_COLUMN", GenerateUniqueSequence() } 
        };

                SendDataToAPI(rowData);
            }
        }
        private string GenerateUniqueSequence()
        {
            string prefix = "QCO";
            string yearMonth = DateTime.Now.ToString("yyyyMM"); 
            string suffix = "UP";

            int lastSequence = GetLastSequenceForMonth(yearMonth);

            string newSequence = $"{prefix}-{yearMonth}-{suffix}{(lastSequence + 1).ToString("D2")}";

            return newSequence;
        }
        private int GetLastSequenceForMonth(string yearMonth)
        {
            try
            {
              
                var requestData = new { YearMonth = yearMonth };
                string jsonData = JsonConvert.SerializeObject(requestData);
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "GetLastSequence", 
                    Program.Client.UserToken,
                    jsonData
                );
                var responseObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);
                if (responseObj.ContainsKey("IsSuccess") && Convert.ToBoolean(responseObj["IsSuccess"]))
                {
                    string json = responseObj["RetData"].ToString();
                    DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);
                    if (dtJson.Rows.Count > 0 && dtJson.Columns.Contains("LASTSEQUENCE"))
                    {
                        if (int.TryParse(dtJson.Rows[0]["LASTSEQUENCE"].ToString(), out int lastSeq))
                        {
                            return lastSeq;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching last sequence: " + ex.Message);
            }

            return 0; 
        }
        private void SendDataToAPI(Dictionary<string, string> rowData)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(rowData);

                string responseString = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "InsertQCOData",
                    Program.Client.UserToken,
                    jsonData
                );

                HandleApiResponse(responseString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HandleApiResponse(string responseString)
        {
            if (string.IsNullOrEmpty(responseString))
                return;

            try
            {
                var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);

                if (response != null && response.ContainsKey("IsSuccess") && Convert.ToBoolean(response["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString)["RetData"].ToString();

                    if (json == "1")
                    {
                        MessageBox.Show(this, "Data submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearDataGridView();
                    }
                    else if (json == "0")
                    {                  
                        MessageBox.Show(this, "Data Updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (json == "-1")
                    {
                        MessageBox.Show("Insertion Failed");

                    }

                    else
                    {
                        MessageBox.Show("Insertion failed!");

                    }

                }
                else
                {
                    string errorMessage = response != null && response.ContainsKey("ErrMsg") ? response["ErrMsg"].ToString() : "Unknown error.";
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error parsing response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearDataGridView()
        {
            if (dataGridView1.DataSource != null)
            {
                // If DataGridView is bound to a DataTable, clear it
                DataTable dt = dataGridView1.DataSource as DataTable;
                dt?.Clear();
            }
            else
            {              
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Name != "SaveButton") 
                        {
                            cell.Value = ""; 
                        }
                    }
                }
            }
        }
        private void Unplanned_Changeover_Load(object sender, EventArgs e)
        {
            try
            {
                autocompleteMenu1.SetAutocompleteMenu(textBox1, autocompleteMenu1); // ✅ Attach autocomplete
                autocompleteMenu2.SetAutocompleteMenu(barttext, autocompleteMenu2); 
                autocompleteMenu3.SetAutocompleteMenu(aarttxt, autocompleteMenu3); 

                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer",
                    "QueryProductioncode", Program.Client.UserToken,
                    Newtonsoft.Json.JsonConvert.SerializeObject("")
                );

                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(json);

                    productionNamelist.Clear();
                    productionNamelist.Add(""); 

                    foreach (DataRow row in dtJson.Rows)
                    {
                        productionNamelist.Add(row["DEPARTMENT_CODE"].ToString());
                    }
                }
  
                if (dataGridView1.Columns.Contains("DEPARTMENT_CODE"))
                {
                    dataGridView1.Columns.Remove("DEPARTMENT_CODE");
                }
                DataGridViewTextBoxColumn departmentColumn = new DataGridViewTextBoxColumn
                {
                    Name = "DEPARTMENT_CODE",
                    HeaderText = "DEPARTMENT_CODE",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                };
                dataGridView1.Columns.Insert(0, departmentColumn);
                LoadDept();

                // ✅ Attach EditingControlShowing event to handle textBox1 positioning
                dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;

                // ✅ Populate AutoComplete
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                barttext.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                barttext.AutoCompleteSource = AutoCompleteSource.CustomSource;
                aarttxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                aarttxt.AutoCompleteSource = AutoCompleteSource.CustomSource;

                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                autoCompleteCollection.AddRange(productionNamelist.ToArray());
                textBox1.AutoCompleteCustomSource = autoCompleteCollection;
                barttext.AutoCompleteCustomSource = autoCompleteCollection;
                aarttxt.AutoCompleteCustomSource = autoCompleteCollection;


                arts = GetProductionList("Loadarticles");
                Aarts = GetProductionList("Loadarticles");
                Loadart();
                Loadarts();
                AddComboBoxToDataGridView("B_ARTNO", "B_ARTNO", 2, arts);
                AddComboBoxToDataGridView("A_ARTNO", "A_ARTNO", 3, Aarts);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // ✅ Adjust DataGridView layout
            dataGridView1.Width = this.ClientSize.Width;
            dataGridView1.Height = this.ClientSize.Height;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
        }
        private void LoadDept()
        {
            autocompleteMenu1.Items = null;
            autocompleteMenu1.MaximumSize = new Size(350, 350);
            var columnWidth = new[] { 50, 150 };
     
            int n = 1;
            for (int i = 0; i < dtJson.Rows.Count; i++)
            {
                autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(
                        new[] { n + "", dtJson.Rows[i]["DEPARTMENT_CODE"].ToString() }, dtJson.Rows[i]["DEPARTMENT_CODE"].ToString())
                { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }
        private void Loadart()
        {
            autocompleteMenu2.Items = null;
            autocompleteMenu2.MaximumSize = new Size(350, 350);
            var columnWidth = new[] { 50, 150 };

            int n = 1;          
            foreach (var art in arts)
            {
                autocompleteMenu2.AddItem(new MulticolumnAutocompleteItem(
                        new[] { n.ToString(), art }, art)
                { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }
        private void Loadarts()
        {
            autocompleteMenu3.Items = null;
            autocompleteMenu3.MaximumSize = new Size(350, 350);
            var columnWidth = new[] { 50, 150 };

            int n = 1;
            foreach (var art in Aarts)
            {
                autocompleteMenu3.AddItem(new MulticolumnAutocompleteItem(
                        new[] { n.ToString(), art }, art)
                { ColumnWidth = columnWidth, ImageIndex = n });
                n++;
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {            
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row.Cells[0].Value != null &&
                    !string.IsNullOrWhiteSpace(row.Cells[0].Value.ToString()))
                {
                    if (dataGridView1.Columns.Contains("INSERT_USER"))
                    {
                        row.Cells["INSERT_USER"].Value = Program.Client.UserName ?? "UnknownUser";
                    }
                }
            
        }
        private List<string> GetProductionList(string apiMethod)
        {
            List<string> itemList = new List<string>();

            string response = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.SuspendedWorkingHoursServer",
                apiMethod, Program.Client.UserToken,
                Newtonsoft.Json.JsonConvert.SerializeObject("")
            );

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (result != null && Convert.ToBoolean(result["IsSuccess"]))
            {
                string json = result["RetData"].ToString();
                DataTable dtJson = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));


                foreach (DataRow row in dtJson.Rows)
                {
                    if (dtJson.Columns.Contains("PROD_NO") && !string.IsNullOrWhiteSpace(row["PROD_NO"].ToString()))
                        itemList.Add(row["PROD_NO"].ToString());
                }

                if (!itemList.Any())
                {
                    foreach (DataRow row in dtJson.Rows)
                    {
                        if (dtJson.Columns.Contains("NAME_T") && !string.IsNullOrWhiteSpace(row["NAME_T"].ToString()))
                            itemList.Add(row["NAME_T"].ToString());
                    }
                }
            }
            return itemList;
        }
        private void AddComboBoxToDataGridView(string columnName, string headerText, int columnIndex, List<string> items)
        {
            if (!dataGridView1.Columns.Contains(columnName))
            {
                DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn
                {
                    Name = columnName,
                    HeaderText = headerText,
                    DataSource = items
                };
                dataGridView1.Columns.Insert(columnIndex, comboBoxColumn);
            }
            else
            {
                DataGridViewComboBoxColumn existingColumn = (DataGridViewComboBoxColumn)dataGridView1.Columns[columnName];
                existingColumn.Items.Clear();
                existingColumn.Items.AddRange(items.ToArray());
            }
            dataGridView1.EditingControlShowing += dataGridView1_EditingControlShowing;
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                comboBox.SelectionChangeCommitted -= ComboBox_SelectionChangeCommitted;
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                if (columnIndex == 0 || columnIndex == 2 || columnIndex == 3)
                {
                    comboBox.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;
                }

            }
            if (dataGridView1.CurrentCell.ColumnIndex == dataGridView1.Columns["DEPARTMENT_CODE"].Index)
            {
                Rectangle cellRectangle = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, true);

                autoCompleteTextBox.SetBounds(cellRectangle.X, cellRectangle.Y, cellRectangle.Width, cellRectangle.Height);
                autoCompleteTextBox.Text = e.Control.Text;
                autoCompleteTextBox.Visible = true;
                autoCompleteTextBox.Focus();

                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
                autoCompleteCollection.AddRange(departmentNames.ToArray());
                autoCompleteTextBox.AutoCompleteCustomSource = autoCompleteCollection;
            }
        }
       


        private void AutoCompleteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView1.CurrentCell.Value = autoCompleteTextBox.Text;
                autoCompleteTextBox.Visible = false;
                dataGridView1.Focus();
            }
        }
        private void AutoCompleteTextBox_Leave(object sender, EventArgs e)
        {
            autoCompleteTextBox.Visible = false;
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textBox))
                return;

            if (e.KeyCode != Keys.Enter) 
                return;

            string selectedValue = textBox.Text.Trim();
            if (string.IsNullOrEmpty(selectedValue))
                return;

            DataGridViewRow currentRow;
            if (dataGridView1.CurrentRow == null || dataGridView1.Rows.Count == 0)
            {
                int newRowIndex = dataGridView1.Rows.Add();
                currentRow = dataGridView1.Rows[newRowIndex];
            }
            else
            {
                currentRow = dataGridView1.CurrentRow;
            }
            if (!dataGridView1.Columns.Contains("DEPARTMENT_CODE"))
            {
                MessageBox.Show("Column 'DEPARTMENT_CODE' does not exist in the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentRow.Cells["DEPARTMENT_CODE"].Value = selectedValue;
            dataGridView1.CurrentCell = currentRow.Cells["DEPARTMENT_CODE"];
            currentRow.Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = currentRow.Index;
            if (selectedValue.Contains("5001"))
            {
                FetchDepartmentName(selectedValue);
            }
        }
        private void barttext_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textBox)) 
                return;

            if (e.KeyCode != Keys.Enter) 
                return;

            string selectedValue = textBox.Text.Trim();
            if (string.IsNullOrEmpty(selectedValue))
                return;

            DataGridViewRow currentRow;
            if (dataGridView1.CurrentRow == null || dataGridView1.Rows.Count == 0)
            {
                int newRowIndex = dataGridView1.Rows.Add();
                currentRow = dataGridView1.Rows[newRowIndex];
            }
            else
            {
                currentRow = dataGridView1.CurrentRow;
            }
            if (!dataGridView1.Columns.Contains("B_ARTNO"))
            {
                MessageBox.Show("Column 'Article' does not exist in the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentRow.Cells["B_ARTNO"].Value = selectedValue;
            dataGridView1.CurrentCell = currentRow.Cells["B_ARTNO"];
            currentRow.Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = currentRow.Index;
            string artNoColumn = dataGridView1.Columns[2]?.Name;
            if (string.IsNullOrEmpty(artNoColumn)) return;
            string modelColumn = null; 


            switch (artNoColumn)
            {
                case "B_ARTNO":
                    modelColumn = "B_MODEL";
                    break;
                case "A_ARTNO":
                    modelColumn = "A_MODEL";
                    break;
            }

            if (modelColumn != null)
            {
                FetchAndBindModel(artNoColumn, modelColumn, selectedValue);
            }

        }

        private void aarttxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox textBox)) 
                return;

            if (e.KeyCode != Keys.Enter) 
                return;

            string selectedValue = textBox.Text.Trim();
            if (string.IsNullOrEmpty(selectedValue))
                return;

            DataGridViewRow currentRow;
            if (dataGridView1.CurrentRow == null || dataGridView1.Rows.Count == 0)
            {
                int newRowIndex = dataGridView1.Rows.Add();
                currentRow = dataGridView1.Rows[newRowIndex];
            }
            else
            {
                currentRow = dataGridView1.CurrentRow;
            }
            if (!dataGridView1.Columns.Contains("A_ARTNO"))
            {
                MessageBox.Show("Column 'Article' does not exist in the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            currentRow.Cells["A_ARTNO"].Value = selectedValue;
            dataGridView1.CurrentCell = currentRow.Cells["A_ARTNO"];
            currentRow.Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = currentRow.Index;
            string artNoColumn = dataGridView1.Columns[3]?.Name;
            if (string.IsNullOrEmpty(artNoColumn)) return;
            string modelColumn = null; 
            switch (artNoColumn)
            {
                case "B_ARTNO":
                    modelColumn = "B_MODEL";
                    break;
                case "A_ARTNO":
                    modelColumn = "A_MODEL";
                    break;
            }

            if (modelColumn != null)
            {
                FetchAndBindModel(artNoColumn, modelColumn, selectedValue);
            }

        }
        private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
   
            if (!(sender is ComboBox comboBox) || dataGridView1.CurrentRow == null || comboBox.SelectedItem == null)
                return;

            string selectedValue = comboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedValue))
                return;
            if (dataGridView1.CurrentCell == null)
                return;

            int currentColumnIndex = dataGridView1.CurrentCell.ColumnIndex;
            if (currentColumnIndex < 0) return;

            string artNoColumn = dataGridView1.Columns[currentColumnIndex]?.Name;
            if (string.IsNullOrEmpty(artNoColumn)) return;

            if (selectedValue.Contains("5001"))
            {
                FetchDepartmentName(selectedValue);
            }
            else
            {
                string modelColumn = null; 


                switch (artNoColumn)
                {
                    case "B_ARTNO":
                        modelColumn = "B_MODEL";
                        break;
                    case "A_ARTNO":
                        modelColumn = "A_MODEL";
                        break;
                }

                if (modelColumn != null)
                {
                    FetchAndBindModel(artNoColumn, modelColumn, selectedValue);
                }
            }
        }

        private void FetchDepartmentName(string departmentCode)
        {
            string response = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.SuspendedWorkingHoursServer",
                "QueryDepartmentNameByCode", Program.Client.UserToken,
                Newtonsoft.Json.JsonConvert.SerializeObject(new { DEPARTMENT_CODE = departmentCode })
            );

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (result != null && Convert.ToBoolean(result["IsSuccess"]))
            {
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(result["RetData"].ToString());
                if (dtJson.Rows.Count > 0)
                    dataGridView1.CurrentRow.Cells["DEPARTMENT_NAME"].Value = dtJson.Rows[0]["DEPARTMENT_NAME"].ToString();
                else
                    MessageBox.Show("No department name found for the selected code.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Failed to retrieve Department Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FetchAndBindModel(string artNoColumn, string modelColumn, string selectedArtNo)
        {
            if (!dataGridView1.Columns.Contains(artNoColumn) || !dataGridView1.Columns.Contains(modelColumn))
                return;

            string response = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.SuspendedWorkingHoursServer",
                "Querymodel", Program.Client.UserToken,
                Newtonsoft.Json.JsonConvert.SerializeObject(new { ARTNO = selectedArtNo })
            );

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            if (result != null && Convert.ToBoolean(result["IsSuccess"]))
            {
                DataTable dtJson = SJeMES_Framework.Common.JsonHelper.GetDataTableByJson(result["RetData"].ToString());
                if (dtJson.Rows.Count > 0)
                    dataGridView1.CurrentRow.Cells[modelColumn].Value = dtJson.Rows[0]["NAME_T"].ToString();
                else
                    MessageBox.Show($"No model found for {selectedArtNo}.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Failed to retrieve model information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Get the panel's drawing area
            Rectangle rect = panel1.ClientRectangle;

            // Create a LinearGradientBrush with three color blend
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Empty, Color.Empty, LinearGradientMode.Horizontal))
            {
                // Define the colors: Red at 0.0, Blue at 0.5, Green at 1.0
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[] { Color.Red, Color.Blue, Color.Green };
                colorBlend.Positions = new float[] { 0f, 0.5f, 1f };

                // Apply the blend
                brush.InterpolationColors = colorBlend;

                // Fill the panel background
                e.Graphics.FillRectangle(brush, rect);
            }

        }
    }
}

