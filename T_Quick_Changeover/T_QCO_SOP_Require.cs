using AutocompleteMenuNS;
using FastReport;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using SJeMES_Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using System.Drawing.Drawing2D;


namespace T_Quick_Changeover
{
    public partial class T_QCO_SOP_Require : Form
    {

        AutoCompleteStringCollection Autodata;
        DataTable dt1;
        public T_QCO_SOP_Require()
        {
            InitializeComponent();
        }
        public void GetInstance(string Article,string Modelfrom, string Modelto, string line, string COD)
        {
            arttxt.Text = Article;
            Bmodeltxt.Text = Modelfrom;
            Amodeltxt.Text = Modelto;
            linetxt.Text = line;
            Codatetxt.Text = COD;
        }
        private void soptxt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string sop = soptxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", sop.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        MessageBox.Show("No Data Found");
                        // MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        soptxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void jointtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string joint = jointtxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", joint.ToString());
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
                        jointtxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }
       // 2014.4.8.0
        private void sopsigntxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string sopsign = sopsigntxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", sopsign.ToString());
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
                        sopsigntxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void suptxt_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {

                string sup = suptxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", sup.ToString());
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
                        suptxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void T_QCO_SOP_Require_Load(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            datetxt.Text = formattedDateTime;
            LoadModel();
           // autocompleteMenu1.SetAutocompleteMenu(modeltxt, autocompleteMenu1);
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        public void LoadModel()
        {

            //modeltxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
           // modeltxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Autodata = new AutoCompleteStringCollection();
            dt1 = new DataTable();
            Dictionary<string, string> kk = new Dictionary<string, string>();
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "LoadModel",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt1 == null || dt1.Rows.Count == 0)
                {
                    MessageBox.Show("No Data Found");
                    // MessageHelper.ShowErr(this,"No Data Found");
                    return;
                }
                else
                {
                    autocompleteMenu1.MaximumSize = new Size(250, 350);
                    var columnWidth = new[] { 50, 200 };
                    int n = 1;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt1.Rows[i]["A_MODEL"].ToString() }, dt1.Rows[i]["A_MODEL"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                        n++;
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

        }

        private void Printbtn_Click(object sender, EventArgs e)
        {
            
            if (jointtxt.Text == "")
            {
                MessageBox.Show("Please Enter Joint Signature");
                return;
            }
            if (jointtxt.Text == "")
            {
                MessageBox.Show("Please Enter Joint Signature");
                return;
            }
            if (sopsigntxt.Text == "")
            {
                MessageBox.Show("Please Enter SOP Barrower Sig");
                return;
            }
            if (suptxt.Text == "")
            {
                MessageBox.Show("Please Enter Supervisor Signature");
                return;

            }
           //string path = @"E:\AEQS_Project_Related_Files\AEQS_Source_Code\deliveryFile\Client\T_Quick_Changeover\QCO_SOP_Fast_report.frx";
            string path = Path.Combine(Application.StartupPath, "T_Quick_Changeover", "QCO_SOP_Fast_report.frx");
            try
            {
                // Create and populate the DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("BModel");
                dt.Columns.Add("AModel");
                dt.Columns.Add("Line");
                dt.Columns.Add("COD");           
                dt.Columns.Add("Date");                           
                dt.Columns.Add("article");
                dt.Columns.Add("sop");
                dt.Columns.Add("bct");
                dt.Columns.Add("reson");
                dt.Columns.Add("joint");
                dt.Columns.Add("sopsig");
                dt.Columns.Add("sup");

                DataRow newRow = dt.NewRow();
                newRow["BModel"] = Bmodeltxt.Text;
                newRow["AModel"] = Amodeltxt.Text;
                newRow["Line"] = linetxt.Text;
                newRow["COD"] = Codatetxt.Text;               
                newRow["Date"] = datetxt.Text;                           
                newRow["article"] = arttxt.Text;
                newRow["sop"] = soptxt.Text;
                newRow["bct"] = bctxt.Text;
                newRow["reson"] = resontxt.Text;
                newRow["joint"] = jointtxt.Text;
                newRow["sopsig"] = sopsigntxt.Text;
                newRow["sup"] = suptxt.Text;
             

                dt.Rows.Add(newRow);
                T_QCO_FR file = new T_QCO_FR(dt, path);
                file.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Object not found: {ex.TargetSite?.Name}");
            }


        }

        private void bctxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string joint = bctxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", joint.ToString());
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
                        bctxt.Text = dt1.Rows[0][0].ToString();
                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

      
        private void panel1_Paint(object sender, PaintEventArgs e)
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
