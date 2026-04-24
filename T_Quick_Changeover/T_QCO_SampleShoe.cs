using AutocompleteMenuNS;
using FastReport;
using FastReport.Export.Pdf;
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
  
    public partial class T_QCO_SampleShoe : Form
    {
        AutoCompleteStringCollection Autodata;
        DataTable dt1;
        public T_QCO_SampleShoe()
        {
            InitializeComponent();
        }
        public void GetInstance(string Article, string Modelfrom, string Modelto, string line, string COD)
        {
            arttxt.Text = Article;
            Bmodeltxt.Text = Modelfrom;
            Amodeltxt.Text = Modelto;
            linetxt.Text = line;
            codatetxt.Text = COD;
        }



        public void print_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(arttxt.Text) || string.IsNullOrWhiteSpace(mcstxt.Text) ||
                string.IsNullOrWhiteSpace(plantxt.Text) || string.IsNullOrWhiteSpace(supervisortxt.Text) ||
                string.IsNullOrWhiteSpace(plant_in_txt.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

           // string path = @"E:\AEQS_Project_Related_Files\AEQS_Source_Code\deliveryFile\Client\T_Quick_Changeover\Qco_Sample_shoe_Fast_report.frx";
            string path = Path.Combine(Application.StartupPath, "T_Quick_Changeover", "Qco_Sample_shoe_Fast_report.frx");
            try
            {
                // Create and populate the DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("BModel");
                dt.Columns.Add("AModel");
                dt.Columns.Add("Line");
                dt.Columns.Add("COdate");
                dt.Columns.Add("Date");
                dt.Columns.Add("Art");              
                dt.Columns.Add("mcs");
                dt.Columns.Add("supsig");
                dt.Columns.Add("planaut");
                dt.Columns.Add("reason");
                dt.Columns.Add("plantsig");

                DataRow newRow = dt.NewRow();

                newRow["BModel"] = Bmodeltxt.Text;
                newRow["AModel"] = Amodeltxt.Text;
                newRow["Line"] = linetxt.Text;
                newRow["COdate"] = codatetxt.Text;
                newRow["Date"] = datetxt.Text;
                newRow["Art"] = arttxt.Text; 
                newRow["mcs"] = mcstxt.Text;
                newRow["supsig"] = supervisortxt.Text;
                newRow["planaut"] = plantxt.Text;
                newRow["reason"] = resontxt.Text;
                newRow["plantsig"] = plant_in_txt.Text;

                dt.Rows.Add(newRow);               
                T_QCO_Fastreport file = new T_QCO_Fastreport(dt, path);
                file.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Object not found: {ex.TargetSite?.Name}");
            }
        }


        private void T_QCO_SampleShoe_Load(object sender, EventArgs e)
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            datetxt.Text = formattedDateTime;
            //this.usertxt.Text = Program.client.UserCode;

            Getarticle();
            autocompleteMenu1.SetAutocompleteMenu(arttxt, autocompleteMenu1);
            // no smaller than design time size
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

            // no larger than screen size
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
           
        }
        public void Getarticle()
        {

            arttxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            arttxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Autodata = new AutoCompleteStringCollection();
            dt1 = new DataTable();
            Dictionary<string, string> kk = new Dictionary<string, string>();
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getart",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt1 == null || dt1.Rows.Count == 0)
                {
                    MessageHelper.ShowErr(this, "No Data Found");
                    return;
                }
                else
                {
                    autocompleteMenu1.MaximumSize = new Size(250, 350);
                    var columnWidth = new[] { 50, 200 };
                    int n = 1;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt1.Rows[i]["A_ART_NO"].ToString() }, dt1.Rows[i]["A_ART_NO"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                        n++;
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }

        }

        private void arttxt_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{

            //    string A_ART_NO = arttxt.Text.ToUpper();
            //    dt1 = new DataTable();
            //    Dictionary<string, string> kk = new Dictionary<string, string>();
            //    kk.Add("A_ART_NO", A_ART_NO.ToString());
            //    string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetDept",
            //        Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            //    if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            //    {
            //        string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
            //        dt1 = JsonConvert.DeserializeObject<DataTable>(json);
            //        if (dt1 == null || dt1.Rows.Count == 0)
            //        {                       
            //             MessageHelper.ShowErr(this,"No Data Found");
            //            return;
            //        }
            //        else
            //        {
            //            depttxt.Text = dt1.Rows[0][0].ToString();
            //            modeltxt.Text = dt1.Rows[0][1].ToString();

            //        }
            //    }
            //    else
            //    {
            //        MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            //    }

            //}
        }

        private void mcstxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string SecH = mcstxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", SecH.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        mcstxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void supervisortxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string SecH = supervisortxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", SecH.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                       
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        supervisortxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void plantxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string SecH = plantxt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", SecH.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                        
                         MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        plantxt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void plant_in_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                string SecH = plant_in_txt.Text.ToUpper();
                dt1 = new DataTable();
                Dictionary<string, string> kk = new Dictionary<string, string>();
                kk.Add("Signs", SecH.ToString());
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getsigns",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    dt1 = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt1 == null || dt1.Rows.Count == 0)
                    {
                       
                        MessageHelper.ShowErr(this,"No Data Found");
                        return;
                    }
                    else
                    {
                        plant_in_txt.Text = dt1.Rows[0][0].ToString();

                    }
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }

            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = panel4.ClientRectangle;

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
