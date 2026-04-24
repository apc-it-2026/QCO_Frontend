using AutocompleteMenuNS;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using SJeMES_Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SJeMES_Framework;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Net.Http;

namespace T_Quick_Changeover
{
    public partial class T_QCO_Equipment_Request : Form
    {
        AutoCompleteStringCollection Autodata;
        DataTable dt1;
        DataTable data1;
        public T_QCO_Equipment_Request()
        {
            InitializeComponent();
            Printbtn.Visible = false;
        }
        public void GetInstance(string seq, string Modelfrom, string Modelto, string line, string COD)
        {
            SLnotxt.Text = seq;
            MChangefrtxt.Text = Modelfrom;
            AfModeltxt.Text = Modelto;
            deptlinetxt.Text = line;
            PCOdatetxt.Text = COD;
        }
        internal void Fitdata(DataTable selectedRowData)
        {
            data1 = selectedRowData;
            for (int i = 0; i < data1.Rows.Count; i++)
            {

                SLnotxt.Text = data1.Rows[i][14].ToString();
                AfModeltxt.Text = data1.Rows[i][6].ToString();
                MChangefrtxt.Text = data1.Rows[i][7].ToString();
                PCOdatetxt.Text = data1.Rows[i][3].ToString();
                deptlinetxt.Text = data1.Rows[i][2].ToString();

            }
            this.Refresh();
        }
        public async void Sendnotification(string role, string txtmsg)
        {
            List<string> numbers = Getalertslist(role);
            // numbers =  Getalertslist(role);
            // Create the payload object
            var payload = new
            {
                numbers = numbers,
                groups = new List<string>(),
                textMsg = txtmsg,
                mediaurl = "",
                filename = ""
            };

            // Serialize the payload to JSON
            var jsonPayload = JsonConvert.SerializeObject(payload);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    // Set the content type to application/json
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    // Send the POST request
                    var response = await httpClient.PostAsync("http://10.3.0.70:9090/whatsapp/WhatsappApi/SendMessage", content);

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Message sent successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to send message. Status Code: {response.StatusCode}");
                        var responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Response Body: {responseBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

        }
        public List<string> Getalertslist(string role)
        {
            var numbers = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("role", role);
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getalertslist",
                 Program.Client.UserToken, JsonConvert.SerializeObject(data));
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                string json = response["RetData"].ToString();

                // Deserialize the JSON array to a list of dynamic objects
                var phoneList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);

                // Extract PHONE_NUMBER and add to the numbers list
                foreach (var item in phoneList)
                {
                    if (item.TryGetValue("PHONE_NUMBER", out string phoneNumber))
                    {
                        numbers.Add(phoneNumber);
                    }
                }
            }

            return numbers;
        }

        private void Savebtn_Click(object sender, EventArgs e)
        {
            BindDGdata();
        }
        public void BindDGdata()
        {

            if (MChangefrtxt.Text == "" && PCOdatetxt.Text == "")
            {
                MessageHelper.ShowErr(this, "Before submit, Please Enter Model name");
                return;

            }
            else
            {

                dt1 = new DataTable();
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    dt1.Columns.Add(col.Name);
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataRow dRow = dt1.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt1.Rows.Add(dRow);
                }

                Dictionary<string, object> kk = new Dictionary<string, object>();
                kk.Add("BModel", MChangefrtxt.Text.ToString());
                kk.Add("AfModel", AfModeltxt.Text.ToString());
                kk.Add("PCOdate", PCOdatetxt.Text.ToString());
                kk.Add("Prodline", deptlinetxt.Text.ToString());
                kk.Add("SLnotxt", SLnotxt.Text.ToString());
                kk.Add("request", usertxt.Text.ToString());
                kk.Add("FilledPC", FilledPCtxt.Text.ToString());
                kk.Add("recievdate", recievtxt.Text.ToString());
                kk.Add("Supervi", Supervisortxt.Text.ToString());
                kk.Add("SectionHd", Sectionheadtxt.Text.ToString());
                kk.Add("Plantin", Plantintxt.Text.ToString());
                kk.Add("PCDept", PCDepttxt.Text.ToString());
                kk.Add("Controller", Controllertxt.Text.ToString());
                kk.Add("Main", dt1);
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Equiprequest",
                     Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();

                    if (json == "1")
                    {
                        SJeMES_Control_Library.MessageHelper.ShowSuccess(this, "Data inserted Successfully");
                    }
                    else
                    {
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, "Data insertion Failed");
                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            //AfModeltxt.Text = "";
            //MChangefrtxt.Text = "";
            //PCOdatetxt.Text = "";
            //deptlinetxt.Text = "";
            //SLnotxt.Text = "";
            //usertxt.Text = "";
            //FilledPCtxt.Text = "";
            //recievtxt.Text = "";
            //Supervisortxt.Text = "";
            //Sectionheadtxt.Text = "";
            //Plantintxt.Text = "";
            //PCDepttxt.Text = "";
            //Controllertxt.Text = "";
            //dataGridView1.Rows.Clear();
            Printbtn.Visible = true;
            Sendnotification("PC",
"Hello...! User, This alert is from *QCO SYSTEM*\n" +
"Equipment Request for this Model: '" + AfModeltxt.Text + "'\n" +
"Changeover Date: '" + PCOdatetxt.Text + "'\n" +
"Request Date: '" + DateTime.Now.ToString() + "'"
);
        }

        private void T_QCO_Equipment_Request_Load(object sender, EventArgs e)
        {
            // this.SLnotxt.Text = "SL" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            this.usertxt.Text = Program.Client.UserCode;

            LoadModel();
            autocompleteMenu1.SetAutocompleteMenu(MChangefrtxt, autocompleteMenu1);
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            // Set the font size for column headers
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
        }
        public void LoadModel()
        {

            MChangefrtxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            MChangefrtxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Autodata = new AutoCompleteStringCollection();
            dt1 = new DataTable();
            Dictionary<string, string> kk = new Dictionary<string, string>();
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "AutoMChange",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt1 == null || dt1.Rows.Count == 0)
                {
                   // MessageBox.Show("No Data Found");
                     MessageHelper.ShowErr(this,"No Data Found");
                    return;
                }
                else
                {
                    autocompleteMenu1.MaximumSize = new Size(250, 350);
                    var columnWidth = new[] { 50, 200 };
                    int n = 1;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt1.Rows[i]["B_MODEL"].ToString() }, dt1.Rows[i]["B_MODEL"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                        n++;
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

        }

        private void Supervisortxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Tech = Supervisortxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                       // MessageBox.Show("No Data Found");
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        Supervisortxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void Sectionheadtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Tech = Sectionheadtxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        Sectionheadtxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void Plantintxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Tech = Plantintxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                        MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        Plantintxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void PCDepttxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Tech = PCDepttxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        PCDepttxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void Controllertxt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string Tech = Controllertxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        Controllertxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void MChangefrtxt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string MChangefr = MChangefrtxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("MChangefr", MChangefr.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetData",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        AfModeltxt.Text = dt1.Rows[0][0].ToString();
                        deptlinetxt.Text = dt1.Rows[0][1].ToString();
                        PCOdatetxt.Text = dt1.Rows[0][2].ToString();
                        SLnotxt.Text = dt1.Rows[0][3].ToString();
                    }
                }
            }
            this.usertxt.Text = Program.Client.UserCode;
        }

        private void FilledPCtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string Tech = Controllertxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", Tech.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        //MessageBox.Show("No Data Found");
                        MessageHelper.ShowErr(this, "No Data Found");
                        return;
                    }
                    else
                    {
                        Controllertxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
        }

        private void Printbtn_Click(object sender, EventArgs e)
        {
            if (MChangefrtxt.Text == "" || PCOdatetxt.Text == "")
            {
                MessageBox.Show("Please enter both Bmodel Signature and COdate");
                return;
            }
           
            string path = Path.Combine(Application.StartupPath, "T_Quick_Changeover", "QCO_Equipmentrequest.frx");
            // string path = @"E:\AEQS_Project_Related_Files\AEQS_Source_Code\deliveryFile\Client\T_Quick_Changeover\QCO_Equipmentrequest.frx";

            try
            {
                // Create and populate the DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("BModel");
                dt.Columns.Add("AModel");
                dt.Columns.Add("Line");
                dt.Columns.Add("COD");
                dt.Columns.Add("Applicant");
                dt.Columns.Add("Prodept");
                dt.Columns.Add("Equipment");
                dt.Columns.Add("Times_of_Demand");
                dt.Columns.Add("Exixt_Qty");
                dt.Columns.Add("Demand_Qty");
                dt.Columns.Add("Output_Target");
                dt.Columns.Add("IE_standard");
                dt.Columns.Add("Tran_dept");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("Receive_Date");
                dt.Columns.Add("Supervisor");
                dt.Columns.Add("Section_Head");
                dt.Columns.Add("Plantin_chanrge");
                dt.Columns.Add("Procontrol_Dept");
                dt.Columns.Add("Controller");


                bool missingData = false;
                // Check for missing data
                if (FilledPCtxt.Text == "" || recievtxt.Text == "")
                {
                    missingData = true;
                }
                // Populate dt with data from dataGridView1
               
                if (missingData)
                {
                    MessageBox.Show("Please fill in all required fields.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        bool isEmptyRow = true;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }
                        if (isEmptyRow)
                        {

                        }
                        else
                        {
                            DataRow newRow = dt.NewRow();

                            // Assign text box values to the DataRow
                            newRow["BModel"] = MChangefrtxt.Text;
                            newRow["AModel"] = AfModeltxt.Text;
                            newRow["Line"] = deptlinetxt.Text;
                            newRow["COD"] = PCOdatetxt.Text;
                            newRow["Applicant"] = usertxt.Text;
                            newRow["Prodept"] = FilledPCtxt.Text;

                            // Assign DataGridView values to the DataRow
                            newRow["Equipment"] = row.Cells["Equipment"].Value?.ToString();
                            newRow["Times_of_Demand"] = row.Cells["Timeof"].Value?.ToString();
                            newRow["Exixt_Qty"] = row.Cells["ExistingQty"].Value?.ToString();
                            newRow["Demand_Qty"] = row.Cells["DemandQty"].Value?.ToString();
                            newRow["Output_Target"] = row.Cells["PlanningOutput"].Value?.ToString();
                            newRow["IE_standard"] = row.Cells["IEStandard"].Value?.ToString();
                            newRow["Tran_dept"] = row.Cells["TransferDept"].Value?.ToString();
                            newRow["Remarks"] = row.Cells["Remark"].Value?.ToString();
                            // Assign text box values for non-grid fields to the DataRow
                            newRow["Receive_Date"] = recievtxt.Text;
                            newRow["Supervisor"] = Supervisortxt.Text;
                            newRow["Section_Head"] = Sectionheadtxt.Text;
                            newRow["Plantin_chanrge"] = Plantintxt.Text;
                            newRow["Procontrol_Dept"] = PCDepttxt.Text;
                            newRow["Controller"] = Controllertxt.Text;

                            // Add the combined values to the DataTable
                            dt.Rows.Add(newRow);
                        }

                    }
                    T_QCO_Equipment_Fastreport file = new T_QCO_Equipment_Fastreport(dt, path);
                    file.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Object not found: {ex.TargetSite?.Name}");
            }
        }

    }
}
