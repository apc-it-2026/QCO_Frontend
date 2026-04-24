using Newtonsoft.Json;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialSkin.Controls;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace T_Quick_Changeover
{
    public partial class T_QCO_Schedule_Report : Form
    {
        DataTable dt;
        DataTable selectedRowData;
        public T_QCO_MAIN Main;
        DataTable dt1;
        public T_QCO_Schedule_Report(T_QCO_MAIN F)
        {
            InitializeComponent();
            monthCalendar1.Visible = false;
            Main = F;
        }
        private void UpdateDateTimePickers(int year, int month)
        {
            // Calculate the start and end dates for the selected month
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            // Update the DateTimePicker controls
            startDateTimePicker.Value = startDate;
            endDateTimePicker.Value = endDate;
        }
        private void T_QCO_Schedule_Report_Load(object sender, EventArgs e)
        {

            int selectedYear = monthCalendar1.SelectionStart.Year;
            int selectedMonth = monthCalendar1.SelectionStart.Month;
            UpdateDateTimePickers(selectedYear, selectedMonth);
            Load_Schedule_Data();
            dataGridView1.DataBindingComplete += DataGridView1_DataBindingComplete;

        }
        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DatagridHighlightRows();
        }

        public void Load_Schedule_Data()
        {
            string plant = string.IsNullOrEmpty(plantscombo.SelectedItem?.ToString()) ? "All" : plantscombo.SelectedItem.ToString();
            object COStatus = Statuscombo.SelectedItem != null ? Statuscombo.SelectedItem.ToString() : string.Empty;
            object Changestatus = changecombo.SelectedItem != null ? changecombo.SelectedItem.ToString() : string.Empty;
            object Status = Cotypecombo.SelectedItem != null ? Cotypecombo.SelectedItem.ToString() : string.Empty;

            object StatusValue = "All";
            
            if (COStatus != null)
            {
                switch (COStatus.ToString())
                {
                    case "Not Started":
                        StatusValue = 0;
                        break;
                    case "On going":
                        StatusValue = 2;
                        break;
                    case "Cancel":
                        StatusValue = 1;
                        break;
                    case "Delay":
                        StatusValue = 3;
                        break;
                    case "Completed":
                        StatusValue = 4;
                        break;
                    case "All": // Special case for "All"
                        StatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        StatusValue = "All";
                        break;
                }
            }
            object ChangestatusValue = "All";
            if (Changestatus != null)
            {
                switch (Changestatus.ToString())
                {
                    case "Planned Changeover":
                        ChangestatusValue = "PS";
                        break;
                    case "Un-planned Changeover":
                        ChangestatusValue = "UP";
                        break;
                    case "All": // Special case for "All"
                        ChangestatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        ChangestatusValue = "All";
                        break;
                }
            }
            object statusValue = "All";
            if (Status != null)
            {
                switch (Status.ToString())
                {
                    case "Model":
                        statusValue = "Model";
                        break;
                    case "Version":
                        statusValue = "Version";
                        break;
                    case "All": // Special case for "All"
                        statusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        statusValue = "All";
                        break;
                }
            }
            Dictionary<string, object> kk = new Dictionary<string, object>();


            kk.Add("Fromdate", startDateTimePicker.Value.ToString());
            kk.Add("Todate", endDateTimePicker.Value.ToString());
            kk.Add("Status", statusValue);
            kk.Add("Plant", plant);
            kk.Add("COstatus", StatusValue);
            kk.Add("Changestatus", ChangestatusValue);
            try
            {
         
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "GetSPlan",
                    Program.Client.UserToken,
                    Newtonsoft.Json.JsonConvert.SerializeObject(kk)
                );

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

                if (!Convert.ToBoolean(response["IsSuccess"]))
                {
                    MessageBox.Show(response["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Deserialize the data into a DataTable
                dt = JsonConvert.DeserializeObject<DataTable>(response["RetData"].ToString());

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No Data Available");
                    return;
                }
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                Getmodel_current_status();
                DatagridHighlightRows();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Getmodel_current_status()
        {
            
            Dictionary<object, string> kk = new Dictionary<object, string>
    {
        { "Fromdate", startDateTimePicker.Value.ToString() },
        { "Todate", endDateTimePicker.Value.ToString() },
        { "Plants", "" }
    };

            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "Getmodel_current_status",
                    Program.Client.UserToken,
                    Newtonsoft.Json.JsonConvert.SerializeObject(kk)
                );

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

                if (!Convert.ToBoolean(response["IsSuccess"]))
                {
                    MessageBox.Show(response["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dt1 = JsonConvert.DeserializeObject<DataTable>(response["RetData"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DatagridHighlightRows()
        {
            if (dt1 == null || dt1.Rows.Count == 0) return;

            // Ensure the STATUS column exists
            if (!dataGridView1.Columns.Contains("STATUS"))
            {
                MessageBox.Show("STATUS column not found in the DataGridView.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                // Skip new rows
                if (row.IsNewRow) continue;
                if (row.Cells["STATUS"].Value != null && int.TryParse(row.Cells["STATUS"].Value.ToString(), out int status))
                {
                    switch (status)
                    {
                        case 0: // Not Started
                            row.DefaultCellStyle.BackColor = Color.White;
                            break;
                        case 1: // Cancel
                            row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                            break;
                        case 2: // On Going
                            row.DefaultCellStyle.BackColor = Color.Orange;
                            break;
                        case 3: // Delay
                            row.DefaultCellStyle.BackColor = Color.Red;
                            break;
                        case 4: // Completed
                            row.DefaultCellStyle.BackColor = Color.Green;
                            break;
                        default: // Unknown or other cases
                            row.DefaultCellStyle.BackColor = Color.White;
                            break;
                    }
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.Gray; // Example: Unknown status
                }
            }
           
        }


        private void Searchbtn_Click(object sender, EventArgs e)
        {
            Load_Schedule_Data();
        }

       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string plant = string.IsNullOrEmpty(plantscombo.SelectedItem?.ToString()) ? "All" : plantscombo.SelectedItem.ToString();
            object COStatus = Statuscombo.SelectedItem != null ? Statuscombo.SelectedItem.ToString() : string.Empty;
            object Changestatus = changecombo.SelectedItem != null ? changecombo.SelectedItem.ToString() : string.Empty;
            object Status = Cotypecombo.SelectedItem != null ? Cotypecombo.SelectedItem.ToString() : string.Empty;

            object StatusValue = "All";

            if (COStatus != null)
            {
                switch (COStatus.ToString())
                {
                    case "Not Started":
                        StatusValue = 0;
                        break;
                    case "On going":
                        StatusValue = 2;
                        break;
                    case "Cancel":
                        StatusValue = 1;
                        break;
                    case "Delay":
                        StatusValue = 3;
                        break;
                    case "Completed":
                        StatusValue = 4;
                        break;
                    case "All": // Special case for "All"
                        StatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        StatusValue = "All";
                        break;
                }
            }
            object ChangestatusValue = "All";
            if (Changestatus != null)
            {
                switch (Changestatus.ToString())
                {
                    case "Planned Changeover":
                        ChangestatusValue = "PS";
                        break;
                    case "Un-planned Changeover":
                        ChangestatusValue = "UP";
                        break;
                    case "All": // Special case for "All"
                        ChangestatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        ChangestatusValue = "All";
                        break;
                }
            }
            object statusValue = "All";
            if (Status != null)
            {
                switch (Status.ToString())
                {
                    case "Model":
                        statusValue = "Model";
                        break;
                    case "Version":
                        statusValue = "Version";
                        break;
                    case "All": // Special case for "All"
                        statusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        statusValue = "All";
                        break;
                }
            }
            Dictionary<string, object> kk = new Dictionary<string, object>();


            kk.Add("Fromdate", startDateTimePicker.Value.ToString());
            kk.Add("Todate", endDateTimePicker.Value.ToString());
            kk.Add("Status", statusValue);
            kk.Add("Plant", plant);
            kk.Add("COstatus", StatusValue);
            kk.Add("Changestatus", ChangestatusValue);
            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetSPlan", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    dt = new DataTable();
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Data Available");
                        return;
                    }
                    dataGridView1.DataSource = dt;                 
                    Getmodel_current_status();
                    DatagridHighlightRows();
                    dt = dataGridView1.DataSource as DataTable;
                    if (dt.Rows.Count == 0)
                    {
                       
                    }
                    else
                    {
                        int numRows = dataGridView1.Rows.Count;
                        //  MessageBox.Show("Total Record count: " + numRows, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                DataTable selectedRowData = new DataTable();
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    selectedRowData.Columns.Add(dataGridView1.Columns[i].HeaderText);
                }

                DataRow newRow = selectedRowData.NewRow();
                for (int i = 0; i < selectedRow.Cells.Count; i++)
                {
                    if (selectedRow.Cells[i].Value != null)
                    {
                        newRow[i] = selectedRow.Cells[i].Value.ToString();
                    }
                }
                selectedRowData.Rows.Add(newRow);

                string departmentCode = null;

                if (dataGridView1.Columns.Contains("DEPARTMENT_CODE"))
                {
                    int departmentCodeIndex = dataGridView1.Columns["DEPARTMENT_CODE"].Index;
                    object departmentCodeValue = selectedRow.Cells[departmentCodeIndex].Value;

                    if (departmentCodeValue != null)
                    {
                        departmentCode = departmentCodeValue.ToString();
                    }
                }

                T_QCO_Checklist2 f = new T_QCO_Checklist2();
                if (departmentCode != null)
                {
                    if (departmentCode.Contains("L"))
                    {
                        f.SetData(selectedRowData);
                        f.SetOpenTabPage1(true);
                    }
                    else
                    {
                        f.SetData2(selectedRowData);
                        f.SetOpenTabPage2(true);
                    }
                }

                f.Show();
                T_QCO_Equipment_Request ER = new T_QCO_Equipment_Request();
                ER.Fitdata(selectedRowData);
            }
        }
        private void Statuscombo_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  Dictionary<object, string> kk = new Dictionary<object, string>();
            Dictionary<string, object> kk = new Dictionary<string, object>();
            kk.Add("Fromdate", startDateTimePicker.Value.ToString());
            kk.Add("Todate", endDateTimePicker.Value.ToString());
            object Status = Statuscombo.SelectedItem;
            object statusValue = "All"; // Default value for "All" case

            if (Status != null)
            {
                switch (Status.ToString())
                {
                    case "Not Started":
                        statusValue = 0;
                        break;
                    case "On going":
                        statusValue = 2;
                        break;
                    case "Cancel":
                        statusValue = 1;
                        break;
                    case "Delay":
                        statusValue = 3;
                        break;
                    case "Completed":
                        statusValue = 4;
                        break;
                    case "All": // Special case for "All"
                        statusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        statusValue = "All";
                        break;
                }
            }
            kk.Add("Status", statusValue);
            kk.Add("plant", plantscombo.SelectedItem);
            string CO = changecombo.SelectedItem != null ? changecombo.SelectedItem.ToString() : string.Empty;
            string Cotype = Cotypecombo.SelectedItem != null ? Cotypecombo.SelectedItem.ToString() : string.Empty;
            kk.Add("CO", CO);
            kk.Add("Cotype", Cotype);

            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetSPlanstatus", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    dt = new DataTable();
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Data Available");
                        return;
                    }
                    dataGridView1.DataSource = dt;              
                    Getmodel_current_status();
                    DatagridHighlightRows();
                    dt = dataGridView1.DataSource as DataTable;
                    if (dt.Rows.Count == 0)
                    {
                        
                    }
                    else
                    {
                        int numRows = dataGridView1.Rows.Count;
                        
                    }
                }
                else
                {
                    MessageBox.Show(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }
        }
      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Cancellbtn"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                if (dataGridView1.Columns.Contains("QCO_SEQUENCE") && dataGridView1.Columns.Contains("STATUS"))
                {
                    var sequenceValue = selectedRow.Cells["QCO_SEQUENCE"].Value;
                    var statusValue = selectedRow.Cells["STATUS"].Value;

                    if (sequenceValue != null && !string.IsNullOrEmpty(sequenceValue.ToString()) && statusValue != null)
                    {
                        // Check if STATUS value is 0 (only proceed with cancellation if STATUS == 0)
                        if (Convert.ToInt32(statusValue) == 0)
                        {
                            // Show confirmation dialog
                            DialogResult result = MessageBox.Show(
                                "Are you sure you want to cancel this changeover?",
                                "Confirm Cancellation",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                            string val = Cancel_check();
                                if(val== "Unauthorized")
                                {                                  
                                    return;
                                }
                                else
                                {
                                    try
                                    {
                                        Dictionary<string, object> kk = new Dictionary<string, object>
                            {
                                { "QCO_SEQUENCE", sequenceValue.ToString() }
                            };

                                        // Call the WebAPI to update the status
                                        string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                                            Program.Client.APIURL,
                                            "KZ_QCO",
                                            "KZ_QCO.Controllers.GeneralServer",
                                            "UpdateStatusToZero",
                                            Program.Client.UserToken,
                                            Newtonsoft.Json.JsonConvert.SerializeObject(kk));

                                        Dictionary<string, object> response = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);
                                        if (Convert.ToBoolean(response["IsSuccess"]))
                                        {
                                            selectedRow.Cells["STATUS"].Value = 1;
                                            MessageBox.Show("Successfully Cancelled The Changeover", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Failed to update the status via the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                            }
                            else
                            {
                                // Do nothing if the user selects "No"
                                MessageBox.Show("Cancellation aborted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cancellation is only allowed if the status is 'Not started'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid QCO_SEQUENCE or STATUS value.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    Load_Schedule_Data();
                }
            }
           
        }

        public string Cancel_check()
        {
            string data = "";
            // Create a dictionary to hold the parameters
            Dictionary<object, string> kk = new Dictionary<object, string>();
  
            try
            {
                // Make the API call to get the schedule data
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "Cancel_check",
                    Program.Client.UserToken,
                    Newtonsoft.Json.JsonConvert.SerializeObject(kk)
                );

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

                if (!Convert.ToBoolean(response["IsSuccess"]))
                {
                    MessageBox.Show(response["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    string User = "Unauthorized";
                    return User;
                }
                Getmodel_current_status();
                DatagridHighlightRows();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}\n{ex.StackTrace}", "Exception", (System.Windows.MessageBoxButton)MessageBoxButtons.OK, (System.Windows.MessageBoxImage)MessageBoxIcon.Error);
            }
            return data;
        }

        private void changecombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, object> kk = new Dictionary<string, object>();
            kk.Add("Fromdate", startDateTimePicker.Value.ToString());
            kk.Add("Todate", endDateTimePicker.Value.ToString());
            string plant = plantscombo.SelectedItem != null ? plantscombo.SelectedItem.ToString() : string.Empty;
            object Status = changecombo.SelectedItem;
            object statusValue = "All"; // Default value for "All" case
            object status = Statuscombo.SelectedItem;
            object StatusValue = "All"; // Default value for "All" case

            if (status != null)
            {
                switch (status.ToString())
                {
                    case "Not Started":
                        StatusValue = 0;
                        break;
                    case "On going":
                        StatusValue = 2;
                        break;
                    case "Cancel":
                        StatusValue = 1;
                        break;
                    case "Delay":
                        StatusValue = 3;
                        break;
                    case "Completed":
                        StatusValue = 4;
                        break;
                    case "All": // Special case for "All"
                        StatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        StatusValue = "All";
                        break;
                }
            }
  
            if (Status != null)
            {
                switch (Status.ToString())
                {
                    case "Planned Changeover":
                        statusValue = "PS";
                        break;
                    case "Un-planned Changeover":
                        statusValue = "UP";
                        break;         
                    case "All": // Special case for "All"
                        statusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        statusValue = "All";
                        break;
                }
            }
            kk.Add("Status", statusValue);
            kk.Add("plant", plant);
            kk.Add("COStatus", StatusValue);


            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getchangeovers", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    dt = new DataTable();
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Data Available");
                        dataGridView1.DataSource = null;
                        return;
                    }
                    dataGridView1.DataSource = dt;
                    Getmodel_current_status();
                    DatagridHighlightRows();
                    dt = dataGridView1.DataSource as DataTable;
                    if (dt.Rows.Count == 0)
                    {

                    }
                    else
                    {
                        int numRows = dataGridView1.Rows.Count;

                    }
                }
                else
                {
                    MessageBox.Show(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }

        }

        private void Cotypecombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<string, object> kk = new Dictionary<string, object>();
            kk.Add("Fromdate", startDateTimePicker.Value.ToString());
            kk.Add("Todate", endDateTimePicker.Value.ToString());
            object Status = Cotypecombo.SelectedItem;
            string plant = plantscombo.SelectedItem != null ? plantscombo.SelectedItem.ToString() : string.Empty;
            object Changestatus = changecombo.SelectedItem;
            object statusValue = "All"; // Default value for "All" case

            if (Status != null)
            {
                switch (Status.ToString())
                {
                    case "Model":
                        statusValue = "Model";
                        break;
                    case "Version":
                        statusValue = "Version";
                        break;
                    case "All": // Special case for "All"
                        statusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        statusValue = "All";
                        break;
                }
            }
            object COStatus = Statuscombo.SelectedItem;
            object StatusValue = "All"; // Default value for "All" case

            if (COStatus != null)
            {
                switch (COStatus.ToString())
                {
                    case "Not Started":
                        StatusValue = 0;
                        break;
                    case "On going":
                        StatusValue = 2;
                        break;
                    case "Cancel":
                        StatusValue = 1;
                        break;
                    case "Delay":
                        StatusValue = 3;
                        break;
                    case "Completed":
                        StatusValue = 4;
                        break;
                    case "All": // Special case for "All"
                        StatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        StatusValue = "All";
                        break;
                }
            }
            object ChangestatusValue = "All";
            if (Changestatus != null)
            {
                switch (Changestatus.ToString())
                {
                    case "Planned Changeover":
                        ChangestatusValue = "PS";
                        break;
                    case "Un-planned Changeover":
                        ChangestatusValue = "UP";
                        break;
                    case "All": // Special case for "All"
                        ChangestatusValue = "All"; // Pass "All" as a string
                        break;
                    default:
                        // Handle unexpected values, if necessary
                        ChangestatusValue = "All";
                        break;
                }
            }
         
            kk.Add("Status", statusValue);        
            kk.Add("Plant", plant);
            kk.Add("COstatus", StatusValue);
            kk.Add("Changestatus", ChangestatusValue);

            try
            {
                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getmodel_wise_co", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    dt = new DataTable();
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No Data Available");
                        dataGridView1.DataSource = null;
                        return;
                    }
                    dataGridView1.DataSource = dt;
                    Getmodel_current_status();
                    DatagridHighlightRows();
                    dt = dataGridView1.DataSource as DataTable;
                    if (dt.Rows.Count == 0)
                    {

                    }
                    else
                    {
                        int numRows = dataGridView1.Rows.Count;

                    }
                }
                else
                {
                    MessageBox.Show(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
            }

        }

       
    

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = panel1.ClientRectangle;

            using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Empty, Color.Empty, LinearGradientMode.Horizontal))
            {
                // Define light/multiple colors
                ColorBlend colorBlend = new ColorBlend();
                colorBlend.Colors = new Color[]
                {
            Color.FromArgb(255, 255, 200, 200), // Light Red
            Color.FromArgb(255, 200, 200, 255), // Light Blue
            Color.FromArgb(255, 200, 255, 200), // Light Green
            Color.FromArgb(255, 255, 255, 200), // Light Yellow (optional extra)
            Color.FromArgb(255, 200, 255, 255)  // Light Cyan (optional extra)
                };

                colorBlend.Positions = new float[]
                {
            0.0f, 0.25f, 0.5f, 0.75f, 1.0f
                };

                brush.InterpolationColors = colorBlend;

                e.Graphics.FillRectangle(brush, rect);
            }
        }
    }
}

