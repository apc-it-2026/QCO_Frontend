using MiniExcelLibs;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using AutoSendEmail;
using System.Drawing.Drawing2D;
using System.Windows.Controls;
using System.Data;
using System.Net.Mail;
using System.Net;
using NewExportExcels;
using ClosedXML.Excel;
using MaterialSkin.Controls;
using SJeMES_Framework.WebAPI;
using System.Net.Http;

namespace T_Quick_Changeover
{
    public partial class T_QCO_Schedule_PlanImport : MaterialForm
    {
        public List<object[]> data = new List<object[]>();
        DataTable excelData = new DataTable();
        DataTable dt;
        static List<string> logList = new List<string>();
        DataTable dt1;


        public T_QCO_Schedule_PlanImport()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            tabControl1.Height = Screen.GetBounds(this).Height - 70;
            string k = Program.Client.UserCode;
        }
        private void butFile_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            string errs = "";

            dataGridView2.AutoGenerateColumns = false;

            if (dataGridView2 != null)
            {
                dataGridView2.Columns.Clear();
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "EXCEL|*.xls*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                data.Clear(); // Clear data before loading new files

                foreach (string filename in ofd.FileNames)
                {
                    try
                    {
                        this.dataGridView1.Rows.Add(filename);
                        dataGridView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                        GetExcelData(Path.GetFullPath(filename));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message);
                    }
                }

                dataGridView2.AllowUserToAddRows = false;

                if (data != null && data.Count > 0)
                {
                    int colNum = data[0].Length;

                    for (int i = 0; i < colNum; i++)
                    {
                        DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();

                        if (i > 0 && i < colNum)
                        {
                            try
                            {
                                // data[0][i] = Convert.ToDateTime(data[0][i].ToString()).ToShortDateString();
                                acCode.Width = 100;
                            }
                            catch (Exception ex)
                            {
                                errs += "column:" + (i + 1) + "," + ex.Message + "\n";
                            }
                        }

                        acCode.Name = data[0][i].ToString();
                        acCode.HeaderText = data[0][i].ToString();
                        dataGridView2.Columns.Add(acCode);
                    }

                    dataGridView2.Rows.Add(data[1]);

                    for (int i = 2; i < data.Count; i++)
                    {

                        dataGridView2.Rows.Add(data[i]);
                        dataGridView2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridView_RowPostPaint);
                    }
                }
            }

            if (errs != "")
            {
                MessageBox.Show(errs);
            }



        }

        private void GetExcelData(string fileName)
        {
            try
            {
                var rows = MiniExcel.Query(fileName).ToList();

                if (rows != null && rows.Count > 0)
                {
                    if (data == null)
                    {
                        data = new List<object[]>();
                    }

                    foreach (var row in rows)
                    {
                        if (row is System.Dynamic.ExpandoObject expandoRow)
                        {
                            object[] rowData = expandoRow.Select(kv => kv.Value).ToArray();
                            data.Add(rowData);
                        }
                        else
                        {
                            MessageBox.Show("Unexpected Format issue");
                        }
                    }
                }
                else
                {
                    data = new List<object[]>(); // Initialize data as an empty list
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }


        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            StringFormat centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
           
        }


        private void butImport_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            butFile.Enabled = false;
            butImport.Enabled = false;
            if (this.dataGridView2.Rows.Count >= 1)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    update_db();
                    butFile.Enabled = true;
                    butImport.Enabled = true;
                }
                catch (Exception ex)
                {
                    butFile.Enabled = true;
                    butImport.Enabled = true;
                    //MessageBox.Show(this, ex.Message);
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, ex.Message);
                }
            }
            else
            {
                butFile.Enabled = true;
                butImport.Enabled = true;
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("err-00001", Program.Client, "", Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
        }


        private void update_db()
        {
            string errs = "";
            DataTable tab = new DataTable();
            object[] cols = data[0];
            for (int i = 0; i < cols.Length; i++)
            {
                tab.Columns.Add(cols[i].ToString());
            }

            // Pass progressBar1 control to the UpdateProgressBar method
            UpdateProgressBar(progressBar1, cols.Length, data.Count);

            for (int i = 1; i < data.Count; i++)
            {
                DataRow dr = tab.NewRow();

                for (int j = 0; j < cols.Length; j++)
                {
                    if (data[i][j] != null && data[i][j].ToString().Trim() != "")
                    {
                        try
                        {
                            // Pass the row number 'i' to GenerateUniqueSequence method
                            string sequence = GenerateUniqueSequence(i);
                            if (j == 0)
                            {
                                dr[j] = sequence; // Set the unique sequence in the first column
                            }
                            else
                            {
                                dr[j] = data[i][j].ToString().Trim();
                            }
                        }
                        catch (Exception ex)
                        {
                            errs += $"row:{i},column:{j + 1},{ex.Message}\n";
                        }
                    }
                    else
                    {
                        dr[j] = "";
                    }
                }
                tab.Rows.Add(dr);
            }

            if (errs == "")
            {
                try
                {
                    UpLoad(tab); // Assuming 'UpLoad' is the method to upload data to your database
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowErr(this, ex.Message);
                   
                }
            }
            else
            {
                MessageBox.Show(errs);
            }
        }
        private void UpdateProgressBar(System.Windows.Forms.ProgressBar progressBar, int colsLength, int dataCount)
        {
            progressBar.Maximum = dataCount - 1;
            progressBar.Step = progressBar.Maximum / colsLength;
        }
        private int counterForCurrentMonth = -1;
        private bool isCounterInitialized = false;
        private string GenerateUniqueSequence(int rowNumber)
        {
            //string timestamp = DateTime.Now.ToString("yyyyMM");
            //string formattedCounter = rowNumber.ToString().PadLeft(4, '0');
            //string sequence = $"QCO-{timestamp}-{formattedCounter}";
            //return sequence;

            string timestamp = DateTime.Now.ToString("yyyyMM");
            if (!isCounterInitialized)
            {
                // Call GetFormattedCounter only once for the current month
                counterForCurrentMonth = GetFormattedCounter(timestamp);
                isCounterInitialized = true;
            }
            string formattedCounterStr = counterForCurrentMonth.ToString().PadLeft(4, '0');
            string sequence = $"QCO-{timestamp}-{formattedCounterStr}";         
            counterForCurrentMonth++;
            return sequence;
        }
       

        private int GetFormattedCounter(string timestamp)
        {
            Dictionary<object, string> parameters = new Dictionary<object, string>();
            parameters.Add("given_month", timestamp);

            try
            {
                string response = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                    Program.Client.APIURL,
                    "KZ_QCO",
                    "KZ_QCO.Controllers.GeneralServer",
                    "GetSeq",
                    Program.Client.UserToken,
                    Newtonsoft.Json.JsonConvert.SerializeObject(parameters)
                );

                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["IsSuccess"]))
                {
                    DataTable dt = new DataTable();
                    string json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return 1;
                    }
                   
                    string maxSequence = dt.Rows[0][0].ToString();

                    if (maxSequence.Length >= 4)
                    {
                        string numericPart = maxSequence.Substring(maxSequence.Length - 4);

                        if (int.TryParse(numericPart, out int maxCounter))
                        {
                            return maxCounter + 1;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response)["ErrMsg"].ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 1;
        }


        public void Getimportdata()
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            //dt = new DataTable();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getimportdata",
                Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {             
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt1 = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                if (dt1 == null || dt1.Rows.Count == 0)
                {                  
                    return;
                }                      
            }
            else
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("Import failed!", Program.Client, "", Program.Client.Language);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
        }
       // List<string> listSend, listCopy, listError;

        public void Getmaillist(out List<string> listSend, out List<string> listCopy, out List<string> listError)
        {
             listSend = new List<string>();
             listCopy = new List<string>();
             listError = new List<string>();
            Dictionary<string, object> d = new Dictionary<string, object>();
            string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getmaillist",
                    Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));

            if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));

                if (dt.Rows.Count > 0)
                {
                    string RECEIVER_LIST = dt.Columns.Count > 0 ? dt.Columns[0].ColumnName : "";
                    string COPY_LIST = dt.Columns.Count > 1 ? dt.Columns[1].ColumnName : "";
                    string ERROR_LIST = dt.Columns.Count > 2 ? dt.Columns[2].ColumnName : "";

                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(RECEIVER_LIST))
                        {
                            listSend.Add(row[RECEIVER_LIST].ToString());
                        }

                        if (!string.IsNullOrEmpty(COPY_LIST))
                        {
                            listCopy.Add(row[COPY_LIST].ToString());
                        }

                        if (!string.IsNullOrEmpty(ERROR_LIST))
                        {
                            listError.Add(row[ERROR_LIST].ToString());
                        }
                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, "No data Found!");
                }
            }
            else
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg("Import failed!", Program.Client, "", Program.Client.Language);
                string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
            }
        }


        public void UpLoad(DataTable tab)
        {
            
            List<string> listSend, listCopy, listError;
            Getmaillist(out listSend, out listCopy, out listError);
            mailEnty mail_enty = null;
            try
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                d.Add("data", tab);

                string ret = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "UpLoad_File",
                    Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(d));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    Getimportdata();


                     listError = new List<string> { "it-software08@in.apachefootwear.com" };
                    string body = " Dear All, "
                        +
                        " Change over Schedule plan " 
                        + DateTime.Now.ToString("yyyy / MM / dd") +
                        " is attached For Your Reference";
                                       string fomatList = "";
                    string[] listFomat = new string[] { fomatList };
                    mail_enty = new mailEnty("it-software08@in.apachefootwear.com", listSend, listCopy, listError, "Change Over Schedule Plan", body, listFomat);

                  
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();

                    if (json == "Inserted successfully!")
                    {
                        // SendEmail1(dt, mail_enty);
                        HandleSuccessfulUpload(dt1, mail_enty, listSend, listCopy);
                        Suceesmsg();

                        string currentmonth = DateTime.Now.ToString("yyyy/MM");
                        Sendnotification("ME", $"",
                             $"'{currentmonth}' Monthly Schedule Plan Uploaded ");
                             
                    }
                   else if (json == "Updated successfully!")
                    {
                        MessageBox.Show("Updated successfully!");
                    }
                    else if (json == "Uploaded Excel is not in the correct format.")
                    {
                        MessageBox.Show("Uploaded Excel is not in the correct format.");
                        
                    }

                    else
                    {
                        MessageBox.Show("Excel Import failed!");
                 
                    }  
                }
                else
                {
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("Import failed!", Program.Client, "", Program.Client.Language);
                    string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString();
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in UpLoad method: " + ex.Message);
               


            }
        }
        public void Suceesmsg()
        {          
            MessageHelper.ShowSuccess(this, "Inserted successfully!");
        }
        private void HandleSuccessfulUpload(DataTable tab, mailEnty mail_enty, List<string> listSend, List<string> listCopy)
        {
            SendEmail1(tab, mail_enty, listSend, listCopy);
            
        }

        public void SendEmail1(DataTable jsondata, mailEnty _enty, List<string> listSend, List<string> listCopy)
        {
            string _mail_body = "";
            try
            
            {
                if (_enty != null && jsondata.Rows.Count > 0)
                {
                    using (MailMessage mm = new MailMessage())
                    {
                        mm.From = new MailAddress("IT-announcement@in.apachefootwear.com", "MES QCO Monthly Schedule Plan");

                        foreach (string send in listSend)
                        {
                            mm.To.Add(send);
                        }

                        foreach (string cc in listCopy)
                        {
                            mm.CC.Add(cc);
                        }

                        mm.Subject = "Change Over Schedule Plan";

                        // Check if sending an attachment
                        if (_enty.sendExcel == null)
                        {
                            DataSet ds = new DataSet();
                            ds.Tables.Add(jsondata);

                            XLWorkbook wbie = new XLWorkbook();

                            foreach (DataTable dt in ds.Tables)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    dt.Columns.Add("SlNo", typeof(int)).SetOrdinal(0);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        dt.Rows[i]["SlNo"] = i + 1;
                                    }
                                }
                                wbie.Worksheets.Add(dt);
                            }

                            using (MemoryStream ieMemoryStream = new MemoryStream())
                            {
                                wbie.SaveAs(ieMemoryStream);
                                byte[] iebytes = ieMemoryStream.ToArray();

                                string ieattachname = "ChangeOver_Schedule_Plan(" + DateTime.Now.ToString("yyyy/MM/dd") + ").xlsx";
                                mm.Attachments.Add(new Attachment(new MemoryStream(iebytes), ieattachname));
                            }
                            string[] paras = new string[jsondata.Columns.Count];

                            for (int j = 0; j < jsondata.Columns.Count; j++)
                            {
                                paras[j] = jsondata.Rows[0][j].ToString();
                            }

                            _mail_body = string.Format(_enty.mailBody, paras);

                            StringBuilder strHTMLBuilder = new StringBuilder();
                            strHTMLBuilder.Append("<html>");
                            strHTMLBuilder.Append("<head>");
                            strHTMLBuilder.Append("</head>");
                            strHTMLBuilder.Append("<body>");
                            strHTMLBuilder.Append($"<div>{_mail_body}</div>");
                            strHTMLBuilder.Append("<table border='1' cellpadding='1' cellspacing='0' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>");
                            strHTMLBuilder.Append("<tr bgcolor='lightblue'>");

                            foreach (DataColumn myColumn in jsondata.Columns)
                            {
                                strHTMLBuilder.Append("<th><h4>");
                                strHTMLBuilder.Append(myColumn.ColumnName);
                                strHTMLBuilder.Append("</h4></th>");
                            }

                            strHTMLBuilder.Append("</tr>");

                            foreach (DataRow myRow in jsondata.Rows)
                            {
                                strHTMLBuilder.Append("<tr>");
                                foreach (DataColumn myColumn in jsondata.Columns)
                                {
                                    strHTMLBuilder.Append("<td>");
                                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                                    strHTMLBuilder.Append("</td>");
                                }
                                strHTMLBuilder.Append("</tr>");
                            }

                            strHTMLBuilder.Append("</table>");
                            strHTMLBuilder.Append("</body>");
                            strHTMLBuilder.Append("</html>");
                            string mssage= string.Format("Thanks!!<br /><b>APC IT</b> <br /><br /> <u> Important Points From IT: </u> <br /> 1.This Mail is Auto Scheduled by IT-Team Please Do not Reply to this Email. <br/> 2.If you would like to unsubscribe, please let IT-Department Know.");
                            mm.Body = strHTMLBuilder.ToString() + mssage;


                            mm.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient())
                            {
                                smtp.Host = "apcmailserver.apachefootwear.com";
                                smtp.Port = 25;
                                smtp.Credentials = new NetworkCredential("IT-announcement@in.apachefootwear.com", "it-123456");
                                smtp.Send(mm);
                            }
                        }
                       
                           
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SendEmail1 method: " + ex.Message);
            }
        }
        public class mailEnty
        {
            public string mailId { get; set; }
            public List<string> listSend { get; set; }
            public List<string> listCopy { get; set; }
            public List<string> listError { get; set; }

            public string mailSubject { get; set; }
            public string mailBody { get; set; }

            public string sendExcel { get; set; }
            public string pathName { get; set; }
            public string fileName { get; set; }
            public string[] listFomat { get; set; }
            public string isDispaly { get; set; }

            public mailEnty(string mail_id, List<string> list_send, List<string> list_copy, List<string> list_error, string mail_subject, string mail_body, string[] list_fomat)
            {
                this.mailId = mail_id;
                this.listSend = list_send;
                this.listCopy = list_copy;
                this.listError = list_error;
                this.mailSubject = mail_subject;
                this.mailBody = mail_body;
                listFomat = list_fomat;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            QCO_FileUpload f9 = new QCO_FileUpload();
            f9.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            QCO_Upload f10 = new QCO_Upload();
            f10.Show();
        }
        public async void Sendnotification(string role, string dept, string txtmsg)
        {
            List<string> numbers = Getalertslist(role, dept);
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
        public List<string> Getalertslist(string role, string dept)
        {
            var numbers = new List<string>();
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("role", role);
            data.Add("plant", dept);
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getalertslist",
                 Program.Client.UserToken, JsonConvert.SerializeObject(data));
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response["RetData"] == null || string.IsNullOrEmpty(response["RetData"].ToString()))
                {
                    // Alert user and return
                    MessageBox.Show("No data found for the provided role and department or Plant", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return numbers;
                }
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
        //private void panel1_Paint(object sender, PaintEventArgs e)
        //{
        //    Rectangle rect = panel1.ClientRectangle;

        //    using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Empty, Color.Empty, LinearGradientMode.Horizontal))
        //    {
        //        // Define light/multiple colors
        //        ColorBlend colorBlend = new ColorBlend();
        //        colorBlend.Colors = new Color[]
        //        {
        //    Color.FromArgb(255, 255, 200, 200), // Light Red
        //    Color.FromArgb(255, 200, 200, 255), // Light Blue
        //    Color.FromArgb(255, 200, 255, 200), // Light Green
        //    Color.FromArgb(255, 255, 255, 200), // Light Yellow (optional extra)
        //    Color.FromArgb(255, 200, 255, 255)  // Light Cyan (optional extra)
        //        };

        //        colorBlend.Positions = new float[]
        //        {
        //    0.0f, 0.25f, 0.5f, 0.75f, 1.0f
        //        };

        //        brush.InterpolationColors = colorBlend;

        //        e.Graphics.FillRectangle(brush, rect);
        //    }
        //}
    }
}
