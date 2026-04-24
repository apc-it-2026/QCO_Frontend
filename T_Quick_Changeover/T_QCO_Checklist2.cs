using Newtonsoft.Json;
using SJeMES_Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutocompleteMenuNS;
using SJeMES_Control_Library;
using System.IO;
using System.Drawing.Imaging;
using FastReport;
using System.Net.Http;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace T_Quick_Changeover
{

    public partial class T_QCO_Checklist2 : Form
    {
        private string _selectedRadioButton;
        private string _SselectedRadioButton;
        private bool NotificationsSent = false;

        AutoCompleteStringCollection Autodata;
        public Boolean isTitle = false;
        IList<object[]> data = null;
        DataTable dt;
        DataTable radiodt;
        DataTable BRD;
        DataTable SBRD;
        public static string COTValue, Cottime;
        public static string COPTValue, Copttime;
        public static int RUTValue;

        public static Image image2;
        public static Image image3;
        public static Image image4;
        public static Image image5;

        public static Image images1;
        public static Image images2;
        public static Image images3;
        public static Image images4;
        DataTable data1;
        Dictionary<string, bool> radioButtonStates = new Dictionary<string, bool>();
        Dictionary<string, bool> SradioButtonStates = new Dictionary<string, bool>();
        string S_NO, Modelfrom, Modelto, COD, line, Article;
        string LINE_UPDATE_TIME, VALUE_UPDATE_TIME, MAN_UPDATE_TIME, MACHINE_UPDATE_TIME, CTPQ_UPDATE_TIME, LAU_UPDATE_TIME, GAUGE_UPDATE_TIME, SAMPLE_SH_UPDATE_TIME, CEMENT_PR_UPDATE_TIME;

        string S_NO1, Modelfrom1, Modelto1, COD1, line1, Article1;
        string S_LINEUPDATE_DATE, S_VALUEUPDATE_DATE, S_REQUIREDUPDATE_DATE, SMACHINUPDATE_DATE, S_CTPQ_UPDATE_DATE, S_SAMPLEUPDATE_DATE, S_GAUGE_UPDATE_DATE, S_MATERIAL_UPDATE_DATE, S_COMPONENT_UPDATE_DATE, S_ELIGIBLE_UPDATE_DATE, S_STITCHINGUPDATE_DATE, S_CS_MC_UPDATE_DATE;
       
        private bool openTabPage1;
        private bool openTabPage2;
        string dateString, dateString1;

        string Sstatus, Astatus;
        public void SetOpenTabPage1(bool value)
        {
            openTabPage1 = value;
            BindTabPage();
        }
        public void SetOpenTabPage2(bool value)
        {
            openTabPage2 = value;
            BindTabPage();
        }

        public void BindTabPage()
        {
            if (openTabPage1)
            {
                if (tabPage1 != null)
                {
                    SelectedTab = tabPage1;
                }
            }
            else
            {
                if (tabPage1 != null)
                {
                    SelectedTab = tabPage2;
                }
            }
        }
        public void GetInstance(string COT, string COPT,string cottime,string copttime)
        {
            COTValue = COT;
            COPTValue = COPT;
            Cottime = cottime;
            Copttime = copttime;
            BindCot(COTValue, COPTValue, Cottime, Copttime);
        }
        public void BindCot(string COT, string COPT,string Cottime,string Copttime)
        {
            if (COT == null && COPT == null)
            {

            }
            else
            {
                COTtxt.Text = COT;
                COPTtxt.Text = COPT;
                cottimelbl.Text = Cottime;
                Copttimelbl.Text = Copttime;
            }
        }      
        public void GetInstance1(string COT, string COPT, string Cottime, string Copttime)
        {
            COTValue = COT;
            COPTValue = COPT;
            Scottimelbl.Text = Cottime;
            Scopttimelbl.Text = Copttime;
            BindCot1(COTValue, COPTValue);
        }

        public void BindCot1(string COT, string COPT)
        {
            if (COT == null && COPT == null)
            {

            }
            else
            {
                cottxt2.Text = COT;
                copttxt2.Text = COPT;
            }
        }
        public void GetAssemblyRUT(int rut)
        {
            RUTValue = rut;
            BindAssemrut(RUTValue);
        }
       
        public void BindAssemrut(int rut)
        {
            if (rut == null )
            {

            }
            else
            {
                textramup.Text = rut.ToString();
                
            }
        }
        
        public void GetstitchRUT(int rut)
        {
            RUTValue = rut;
            Bindstitchrut(RUTValue);
        }
        public void Bindstitchrut(int rut)
        {
            if (rut == null)
            {

            }
            else
            {
                txtSRut.Text = rut.ToString();

            }
        }
        private void cotbtn_Click(object sender, EventArgs e)
        {
            int Aline = 1;
            string dept = linetxt.Text;
            T_QCO_COT_COPT cot = new T_QCO_COT_COPT(Aline,dept,this);
            //cot.Show();
            cot.Owner = this; 
            cot.ShowDialog();
            Calculate_RUTbtn.Visible = true;

        }

        private void signaturebtn_Click(object sender, EventArgs e)
        {
            int A = 1;
            int indexValue = 1; 

            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue,A);
            pr.Show();
        }

        private void pcsigbtn_Click(object sender, EventArgs e)
        {
            int A = 2;
            int indexValue = 2;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue,A);
            pr.Show();
        }

        private void techsigbtn_Click(object sender, EventArgs e)
        {
            int A = 3;
            int indexValue = 3;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue,A);
            pr.Show();
        }

        private void mainsigbtn_Click(object sender, EventArgs e)
        {
            int A = 4;
            int indexValue = 4;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue,A);
            pr.Show();
        }
        public void ShowImageAndBind(Image image, PictureBox pictureBox)
        {
            if (image != null && pictureBox != null)
            {
                BindImage(image, pictureBox);
            }
        }

        private void BindImage(Image image, PictureBox pictureBox)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Image = image;
        }
        public void Showimage(Image image)
        {
            image2 = image;
            ShowImageAndBind(image2, pictureBox1);
        }
        public void Shows1(Image img)
        {
            images1 = img;
            ShowImageAndBind(images1, pictureBox9);
        }
        public void Shows2(Image img)
        {
            images2 = img;
            ShowImageAndBind(images2, pictureBox7);
        }
        public void Shows3(Image img)
        {
            images3 = img;
            ShowImageAndBind(images3, pictureBox6);
        }
        public void Shows4(Image img)
        {
            images4 = img;
            ShowImageAndBind(images4, pictureBox5);
        }
        public void Showimage1(Image image)
        {
            image3 = image;
            ShowImageAndBind(image3, pictureBox2);
        }
        public void Showimage2(Image image)
        {
            image4 = image;
            ShowImageAndBind(image4, pictureBox3);
        }
        public void Showimage3(Image image)
        {
            image5 = image;
            ShowImageAndBind(image5, pictureBox4);
        }
        public Button Calculate_RUT
        {
            get { return Calculate_RUTbtn; }
        }
        public Button RUT
        {
            get { return btncalrutS; }
        }

        public T_QCO_Checklist2()
        {
            InitializeComponent();
            Articletext.Visible = false;
            Aarticlenolbl.Visible = false;
            this.WindowState = FormWindowState.Maximized;


            aplancombo.Items.AddRange(new string[] { "Scheduled", "Unplanned_Changeover" });
            aplancombo.SelectedIndexChanged += aplancombo_SelectedIndexChanged;
            selectplancombo.Items.AddRange(new string[] { "Scheduled", "Unplanned_Changeover" });//aplancombo
            selectplancombo.SelectedIndexChanged += selectplancombo_SelectedIndexChanged;

            Calculate_RUTbtn.Visible = false;//20241128
            btncalrutS.Visible = false;


        }

        private void Submitbtn_Click(object sender, EventArgs e)
        {
            Savedata();          
        }
        private double checkedCount = 0;
        private double totalCheckedCount = 0;
        private const double IncrementValue = 11.11111111;

        private double checkedCount1 = 0;
        private int checkedRadioButtons = 0; // Number of checked radio buttons
        private int totalRadioButtons = 12;

        public TabPage SelectedTab { get; private set; }

        private void UpdateProgressBar()
        {
  
            int maxValue = progressBar1.Maximum;
            int minValue = progressBar1.Minimum;
            // int initialValue = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToInt32(Astatus);
            double initialValue = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double scaledValue = (checkedCount / 100.0) * maxValue;
            int updatedValue = Clamp((int)Math.Round(scaledValue + initialValue), minValue, maxValue);
            progressBar1.Value = updatedValue;
            Progressvalue.Text = $"{progressBar1.Value}%";
           // Progressvalue.Refresh();

        }

        private void UpdateProgressBar1()
        {
            int maxValue = progressBar2.Maximum;
            int minValue = progressBar2.Minimum;

            //int initialValue = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToInt32(Sstatus);
            int initialValue = string.IsNullOrEmpty(Sstatus) ? 0 : (int)Math.Round(Convert.ToDouble(Sstatus));

            double scaledValue = Math.Max(0, (checkedCount1 / 100.0) * maxValue);
            int updatedValue = Clamp((int)Math.Round(scaledValue + initialValue), minValue, maxValue);

            // **Round to 100% if progress is close (>= 95%)**
            if (updatedValue >= (maxValue * 0.95))
            {
                updatedValue = maxValue; // Round to 100%
            }

            // Update the progress bar
            progressBar2.Value = updatedValue;
            StitchProgress.Text = $"{progressBar2.Value}%";
            StitchProgress.Refresh();
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private bool LinelayoutProcessed = false;
        private bool isvalueProcessed = false;
        private bool ismanpProcessed = false;
        private bool isMachinerProcessed = false;
        private bool isCTPQrProcessed = false;
        private bool ismaterialrProcessed = false;
        private bool isGaugerbProcessed = false;
        private bool isSampleshoerProcessed = false;
        private bool iscementrbProcessed = false;

        private bool SlinerbProcessed = false;
        private bool SvaluesrbProcessed = false;
        private bool SManprbProcessed = false;
        private bool SmachinerbProcessed = false;
        private bool SctpqrbProcessed = false;
        private bool SsamplerbProcessed = false;
        private bool SgaugerbProcessed = false;
        private bool SmoldrbProcessed = false;
        private bool SCcomprbProcessed = false;
        private bool SeligibrbProcessed = false;
        private bool SStitchingrbProcessed = false;
        private bool ScsrbProcessed = false;


        private bool LinelayoutAlertSent = false;
        private bool isvalueAlertSent = false;
        private bool ismanpAlertSent = false;
        private bool isMachinerAlertSent = false;
        private bool isCTPQrAlertSent = false;
        private bool ismaterialrAlertSent = false;
        private bool isGaugerbAlertSent = false;
        private bool isSampleshoerAlertSent = false;
        private bool iscementrbAlertSent = false;



        private bool SlinerbAlertSent = false;
        private bool SvaluesrbAlertSent = false;
        private bool SManprbAlertSent = false;
        private bool SmachineAlertSent = false;
        private bool SctpqrbAlertSent = false;
        private bool SsamplerbAlertSent = false;
        private bool SgaugerbAlertSent = false;
        private bool SmoldrbAlertSent = false;
        private bool SCcomprbAlertSent = false;
        private bool SeligibrbAlertSent = false;
        private bool SStitchingrbAlertSent = false;
        private bool ScsrbAlertSent = false;

      
       
        private double UpdateCheckedCount(bool isChecked)
        {
            // Determine increment or decrement value
            double increment = isChecked ? IncrementValue : -IncrementValue;

            // Ensure checkedCount never goes negative
            checkedCount = Math.Max(0, checkedCount + increment);

            if (isChecked)
            {
                totalCheckedCount += increment;
            }
            else
            {
        
                double possibleNewTotal = totalCheckedCount + increment; 
                if (possibleNewTotal > checkedCount)
                {
                    totalCheckedCount += increment; 
                }
                else
                {
                    totalCheckedCount = checkedCount; 
                }
            }

            // Clamp to zero to avoid negative values
            totalCheckedCount = Math.Max(0, totalCheckedCount);

            UpdateProgressBar();

            return totalCheckedCount;
        }
        private double UpdateCheckedCount1(bool isChecked)
        {
            double increment = isChecked ? IncrementValue : -IncrementValue;
            checkedCount1 = Math.Max(0, checkedCount1 + increment);
            if (isChecked)
            {
                checkedRadioButtons++;
            }
            else
            {
                double possibleNewTotal = checkedRadioButtons + increment; 
                if (possibleNewTotal > checkedCount1)
                {
                    checkedRadioButtons++; 
                }
                else
                {
                    checkedRadioButtons =Convert.ToInt32(checkedCount1); 
                }
            }
            // Clamp to zero to avoid negative values
            checkedRadioButtons = Math.Max(0, checkedRadioButtons);

            UpdateProgressBar1();
            return checkedCount1;
        }


        public async void Sendnotification(string role,string dept, string txtmsg)
        {
            List<string> numbers = Getalertslist(role,dept);
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

        public void Getradiodata_alerts()
        {
            _selectedRadioButton = "khaleel";

            Dictionary<string, string> kk = new Dictionary<string, string>
    {
        { "QCO_Sequtxt", S_NO.ToString() }
    };

            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetRadiodata",
                                           Program.Client.UserToken, JsonConvert.SerializeObject(kk));

            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                radiodt = JsonConvert.DeserializeObject<DataTable>(json);

                if (radiodt == null || radiodt.Rows.Count == 0)
                {
                    return; 
                }
                RadioButton[] radioButtons = { Linelayoutrbt1, valuerdb1, Manprdb1, Machinerdb1, CTPQrdb1, materialrdb1, Gaugerbn1, Sampleshoerbn1, cementrbd1 };

                if (radioButtons.Length > 0 && radiodt.Rows.Count > 0)
                {
                    radioButtonStates = new Dictionary<string, bool>();

                    for (int i = 0; i < radioButtons.Length; i++)
                    {
                        if (radioButtons[i] == null)
                        {
                            Console.WriteLine($"RadioButton at index {i} is null");
                            continue; 
                        }

                        string radioButtonState = radiodt.Rows[0][i].ToString();
                        bool isChecked = radioButtonState == "Normal";

                        Console.WriteLine($"RadioButton {radioButtons[i].Name} State: {radioButtonState}"); 

                        radioButtons[i].Checked = isChecked;
                        radioButtonStates[radioButtons[i].Name] = isChecked; 
                    }
                }
            }
        }
        #region This is For Assembly First Radio Buttons Block
        private void Linelayoutrbt1_Click(object sender, EventArgs e)
        {
            if (LinelayoutProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Linelayoutrbt1.Checked = false;
                return;

            }
            bool isCurrentlyChecked = Linelayoutrbt1.Checked;
            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(Linelayoutrbt1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                line_update_txt.Text = currentDate;        
                LinelayoutProcessed = true;
              
            }
        }

        private void valuerdb1_Click(object sender, EventArgs e)
        {
            if (isvalueProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                valuerdb1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = valuerdb1.Checked;
            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(valuerdb1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";
                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Value_Update_txt.Text = currentDate;
          
                isvalueProcessed = true;
            }
        }
        private void Manprdb1_Click(object sender, EventArgs e)
        {
            if (ismanpProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Manprdb1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Manprdb1.Checked;
            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(isCurrentlyChecked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";
                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Man_Update_txt.Text = currentDate;
                ismanpProcessed = true;
            }
        }
        private void Machinerdb1_Click(object sender, EventArgs e)
        {
            if (isMachinerProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Machinerdb1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Machinerdb1.Checked;

            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(Machinerdb1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Machine_update_txt.Text = currentDate;
               

                isMachinerProcessed = true;
            }
        }
        private void CTPQrdb1_Click(object sender, EventArgs e)
        {
            if (isCTPQrProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CTPQrdb1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = CTPQrdb1.Checked;
            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(CTPQrdb1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Ctpq_Update_txt.Text = currentDate;
              
                isCTPQrProcessed = true;
            }
        }
        private void materialrdb1_Click(object sender, EventArgs e)
        {
            if (ismaterialrProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                materialrdb1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = materialrdb1.Checked;

            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(materialrdb1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Mold_Update_txt.Text = currentDate;
               
                ismaterialrProcessed = true;
            }
        }
        private void Gaugerbn1_Click(object sender, EventArgs e)
        {
            if (isGaugerbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Gaugerbn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Gaugerbn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(Gaugerbn1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Gaue_Update_txt.Text = currentDate;
               
                isGaugerbProcessed = true;
            }
        }
        private void Sampleshoerbn1_Click(object sender, EventArgs e)
        {
            if (isSampleshoerProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Sampleshoerbn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Sampleshoerbn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(Sampleshoerbn1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Sop_update_txt.Text = currentDate;
              
                isSampleshoerProcessed = true;
            }
        }
        private void cementrbd1_Click(object sender, EventArgs e)
        {
            if (iscementrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cementrbd1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = cementrbd1.Checked;
            if (isCurrentlyChecked)
            {
                double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);
                double increment = UpdateCheckedCount(cementrbd1.Checked);
                double newProgress = currentAstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                Progressvalue.Text = $"{roundedProgress:F2} %";

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                Cement_update_txt.Text = currentDate;
                iscementrbProcessed = true;
          
            }
        }
        #endregion This is For Assembly First Radio Buttons Block

        #region This is For Assembly Second Radio Buttons Block


        private void Linelayoutrbt2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("Linelayoutrbt1") && Program.RadioButtonStates["Linelayoutrbt1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                Linelayoutrbt1.Checked = true;
                return;
            }
            double status = 0;
            if (!string.IsNullOrEmpty(Astatus))
            {
                status = Convert.ToDouble(Astatus);
            }

            // Use the status value for any required logic
            double currentAstatus = status;
            double decrement = UpdateCheckedCount(false);
            //  double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            line_update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");

        }



        private void valuerdb2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("valuerdb1") && Program.RadioButtonStates["valuerdb1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                valuerdb1.Checked = true;
                return;
            }
           
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            //double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Value_Update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }
   

        private void Manprdb2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("Manprdb1") && Program.RadioButtonStates["Manprdb1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                Manprdb1.Checked = true;
                return;
            }

           
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            //  double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Man_Update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        private void Sampleshoerbn2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("Sampleshoerbn1") && Program.RadioButtonStates["Sampleshoerbn1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                Sampleshoerbn1.Checked = true;
                return;
            }
            
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            //double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Sop_update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        private void cementrbd2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("cementrbd1") && Program.RadioButtonStates["cementrbd1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                cementrbd1.Checked = true;
                return;
            }
           
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            // double newProgress = currentAstatus + decrement;
            //double newProgress = currentAstatus - decrement;
            double newProgress = Math.Max(currentAstatus - decrement, checkedCount);

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Cement_update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }
        private void Machinerdb2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("Machinerdb1") && Program.RadioButtonStates["Machinerdb1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                Machinerdb1.Checked = true;
                return;
            }
            
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            //double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus- decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Machine_update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        private void CTPQrdb2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("CTPQrdb1") && Program.RadioButtonStates["CTPQrdb1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                CTPQrdb1.Checked = true;
                return;
            }
            
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            // double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus- decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Ctpq_Update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        private void materialrdb2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("materialrdb1") && Program.RadioButtonStates["materialrdb1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                materialrdb1.Checked = true;
                return;
            }
           
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            // double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Mold_Update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        private void Gaugerbn2_Click(object sender, EventArgs e)
        {
            if (Program.RadioButtonStates.ContainsKey("Gaugerbn1") && Program.RadioButtonStates["Gaugerbn1"])
            {
                MessageBox.Show("Cannot Decrement — Already Saved Data");
                Gaugerbn1.Checked = true;
                return;
            }
           
            double currentAstatus = string.IsNullOrEmpty(Astatus) ? 0 : Convert.ToDouble(Astatus);

            double decrement = UpdateCheckedCount(false);
            //  double newProgress = currentAstatus + decrement;
            double newProgress = currentAstatus - decrement;

            newProgress = Math.Max(newProgress, 0);
            int roundedProgress = (int)Math.Round(newProgress);  //
            Progressvalue.Text = $"{roundedProgress:F2} %";
            Gaue_Update_txt.Text = "";
            Astatus = roundedProgress.ToString("F2");
        }

        #endregion This is For Assembly Second Radio Buttons Block
        private void T_QCO_Checklist2_Load(object sender, EventArgs e)
        {
            string Assembly = line;
            string Stitch = line1;

            if (!string.IsNullOrWhiteSpace(Assembly) && Assembly.Contains("L"))
            {
                #region This Block is For Applaying Smooth Suggestion For QCO
                string[] preventiveItems = new string[]
      {
    "Line supervisors fill checklist before 4 Days advance → Preventive: Ensures preparation is not left to the last minute.",
    "Please focus on quality issues in production → Preventive: Avoids defects that cause delays during or after changeover.",
    "Check with MNT people about machine condition → Preventive: Prevents machine breakdowns during changeover.",
    "Arrange CS programs, molds, threads before changeover → Preventive: Ensures all essentials are ready in advance.",
    "PC to ensure material is in place before changeover → Preventive: Avoids material-related delays.",
    "Trained operators (multiskilling) → Preventive: Ensures manpower is flexible and efficient during changeover.",
    "Focus on material delivery from cutting and outsourcing → Preventive: Ensures continuous supply flow without delay.",
    "All departments to play roles effectively → Preventive: Promotes collaboration and clarity of responsibility."
      };

                int lastColumn = tableLayoutPanel8.ColumnCount - 1;
                int totalRows = tableLayoutPanel8.RowCount;

                // Clear existing controls in last column
                for (int i = tableLayoutPanel8.Controls.Count - 1; i >= 0; i--)
                {
                    var ctrl = tableLayoutPanel8.Controls[i];
                    if (tableLayoutPanel8.GetColumn(ctrl) == lastColumn)
                    {
                        tableLayoutPanel8.Controls.RemoveAt(i);
                    }
                }

                // Create RichTextBox
                RichTextBox summaryBox = new RichTextBox();
                summaryBox.ReadOnly = true;
                summaryBox.BorderStyle = BorderStyle.None;
                summaryBox.BackColor = Color.WhiteSmoke;
                summaryBox.Dock = DockStyle.Fill;
                summaryBox.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
                summaryBox.ForeColor = Color.Black;
                summaryBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                summaryBox.Margin = new Padding(5);

                // Add Title
                summaryBox.SelectionColor = Color.DarkGreen;
                summaryBox.SelectionFont = new Font("Times New Roman", 9.5F, FontStyle.Bold);
                summaryBox.AppendText("✅ Preventive Measures (Proactive actions to avoid problems)\n\n");

                // Add each entry with color split
                foreach (var item in preventiveItems)
                {
                    string[] parts = item.Split(new[] { "→" }, StringSplitOptions.None);

                    // ✔️ + Green part (first part)
                    summaryBox.SelectionColor = Color.Green;
                    summaryBox.SelectionFont = new Font("Times New Roman", 9F, FontStyle.Bold);
                    summaryBox.AppendText("✔️ " + parts[0].Trim() + " ");

                    // Black part (second part)
                    if (parts.Length > 1)
                    {
                        summaryBox.SelectionColor = Color.Black;
                        summaryBox.SelectionFont = new Font("Times New Roman", 9F, FontStyle.Bold);
                        summaryBox.AppendText("→" + parts[1].Trim());
                    }

                    summaryBox.AppendText("\n\n");
                }

                // Add to table
                tableLayoutPanel8.Controls.Add(summaryBox, lastColumn, 0);
                tableLayoutPanel8.SetRowSpan(summaryBox, totalRows);

                #endregion This Block is For Applaying Smooth Suggestion For QCO
            }
            if(!string.IsNullOrWhiteSpace(Stitch) && Stitch.Contains("S"))
            {
                #region This Block is For Applaying Smooth Suggestion For QCO
                string[] preventiveItems = new string[]
      {
    "Line supervisors fill checklist before 4 days advance → Preventive: Ensures preparation is not left to the last minute.",
    "Please focus on quality issues in production → Preventive: Avoids defects that cause delays during or after changeover.",
    "Check with MNT people about machine condition → Preventive: Prevents machine breakdowns during changeover.",
    "Arrange CS programs, molds, threads before changeover → Preventive: Ensures all essentials are ready in advance.",
    "PC to ensure material is in place before changeover → Preventive: Avoids material-related delays.",
    "Trained operators (multiskilling) → Preventive: Ensures manpower is flexible and efficient during changeover.",
    "Focus on material delivery from cutting and outsourcing → Preventive: Ensures continuous supply flow without delay.",
    "All departments to play roles effectively → Preventive: Promotes collaboration and clarity of responsibility."
      };

                int lastColumn = tableLayoutPanel19.ColumnCount - 1;
                int totalRows = tableLayoutPanel19.RowCount;

                // Clear existing controls in last column
                for (int i = tableLayoutPanel19.Controls.Count - 1; i >= 0; i--)
                {
                    var ctrl = tableLayoutPanel19.Controls[i];
                    if (tableLayoutPanel19.GetColumn(ctrl) == lastColumn)
                    {
                        tableLayoutPanel19.Controls.RemoveAt(i);
                    }
                }

                // Create RichTextBox
                RichTextBox summaryBox = new RichTextBox();
                summaryBox.ReadOnly = true;
                summaryBox.BorderStyle = BorderStyle.None;
                summaryBox.BackColor = Color.WhiteSmoke;
                summaryBox.Dock = DockStyle.Fill;
                summaryBox.Font = new Font("Times New Roman", 9F, FontStyle.Bold);
                summaryBox.ForeColor = Color.Black;
                summaryBox.ScrollBars = RichTextBoxScrollBars.Vertical;
                summaryBox.Margin = new Padding(5);

                // Add Title
                summaryBox.SelectionColor = Color.DarkGreen;
                summaryBox.SelectionFont = new Font("Times New Roman", 9.5F, FontStyle.Bold);
                summaryBox.AppendText("✅ Preventive Measures (Proactive actions to avoid problems)\n\n");

                // Add each entry with color split
                foreach (var item in preventiveItems)
                {
                    string[] parts = item.Split(new[] { "→" }, StringSplitOptions.None);

                    // ✔️ + Green part (first part)
                    summaryBox.SelectionColor = Color.Green;
                    summaryBox.SelectionFont = new Font("Times New Roman", 9F, FontStyle.Bold);
                    summaryBox.AppendText("✔️ " + parts[0].Trim() + " ");

                    // Black part (second part)
                    if (parts.Length > 1)
                    {
                        summaryBox.SelectionColor = Color.Black;
                        summaryBox.SelectionFont = new Font("Times New Roman", 9F, FontStyle.Bold);
                        summaryBox.AppendText("→" + parts[1].Trim());
                    }

                    summaryBox.AppendText("\n\n");
                }

                // Add to table
                tableLayoutPanel19.Controls.Add(summaryBox, lastColumn, 0);
                tableLayoutPanel19.SetRowSpan(summaryBox, totalRows);

                #endregion This Block is For Applaying Smooth Suggestion For QCO

            }






            LoadModel();
           
            autocompleteMenu1.SetAutocompleteMenu(BMCOtxt, autocompleteMenu1);
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.MaximumSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            if (DateTime.TryParse(COD, out DateTime dateTime))
            {
                dateString1 = dateTime.ToString("yyyy/MM/dd");
            }
            QCO_Sequtxt.Text = S_NO;
            BMCOtxt.Text = Modelfrom;
            A_MCOtxt.Text = Modelto;
            linetxt.Text = line;
            COdatetxt.Text = dateString1;
            Articletext.Text = Article;

            line_update_txt.Text = LINE_UPDATE_TIME;
            Value_Update_txt.Text = VALUE_UPDATE_TIME;
            Man_Update_txt.Text = MAN_UPDATE_TIME;
            Machine_update_txt.Text = MACHINE_UPDATE_TIME;
            Ctpq_Update_txt.Text = CTPQ_UPDATE_TIME;
            Mold_Update_txt.Text = LAU_UPDATE_TIME;
            Gaue_Update_txt.Text = GAUGE_UPDATE_TIME;
            Sop_update_txt.Text = SAMPLE_SH_UPDATE_TIME;
            Cement_update_txt.Text = CEMENT_PR_UPDATE_TIME;

            if (DateTime.TryParse(COD1, out DateTime dateTime1))
            { 
                 dateString = dateTime1.ToString("yyyy/MM/dd");
            }
            sseqtxt.Text = S_NO1;
            sdpttxt.Text = line1;
            scotxt.Text  = dateString;
            sbmodeltxt.Text= Modelfrom1;
            samodeltxt.Text = Modelto1;
            Aarttxt.Text= Article1;

            S_line_time_txt.Text = S_LINEUPDATE_DATE;
            S_vs_time_txt.Text   = S_VALUEUPDATE_DATE;
            S_mp_time_txt.Text   = S_REQUIREDUPDATE_DATE;
            S_man_time_txt.Text  = SMACHINUPDATE_DATE;
            S_ctpt_time_txt.Text = S_CTPQ_UPDATE_DATE;
            S_sop_time_txt.Text  = S_SAMPLEUPDATE_DATE;
            S_gau_time_txt.Text  = S_GAUGE_UPDATE_DATE;
            S_mold_time_txt.Text = S_MATERIAL_UPDATE_DATE;
            S_com_time_txt.Text  = S_COMPONENT_UPDATE_DATE;
            S_eq_time_txt.Text   = S_ELIGIBLE_UPDATE_DATE;
            S_cm_time_txt.Text   = S_STITCHINGUPDATE_DATE;
            S_cs_time_txt.Text   = S_CS_MC_UPDATE_DATE;

            // Get the date from the TextBox
            if (DateTime.TryParse(COdatetxt.Text, out DateTime enteredDate))
            {                
                string enteredDat = enteredDate.ToString("yyyy/MM/dd");
                DateTime givenCOD = DateTime.Parse(enteredDat);
                DateTime currentDate = DateTime.Now;
                string cur = currentDate.ToString("yyyy/MM/dd");
                DateTime curdate = DateTime.Parse(cur);
                TimeSpan difference = givenCOD - curdate;
                int daysDifference = difference.Days;
                DateTime fourDaysBefore = enteredDate.AddDays(-4);
                if (progressBar1.Value == 100)
                {
                    fourlbl.Text = "COD Completed";
                    threelbl.Text = "COD Completed";
                    panel7.BackColor = Color.Green;
                    panel8.BackColor = Color.Green;
                    
                }
                else
                {
                    if (daysDifference < 0)
                    {
                        panel7.BackColor = Color.Red;
                        panel8.BackColor = Color.Red;
                        fourlbl.Text = "COD date already Crossed";
                        threelbl.Text= "COD date already Crossed";
                    }
                    else
                    {
                        switch (daysDifference)
                        {
                            case 4:
                                panel7.BackColor = Color.Green;
                                fourlbl.Text = "Four days to go";
                                break;
                            case 3:
                                panel7.BackColor = Color.Yellow;
                                fourlbl.Text = "Three days to go";
                                break;
                            case 2:
                                panel7.BackColor = Color.Orange;
                                fourlbl.Text = "Two days to go";
                                break;
                            case 1:
                                panel7.BackColor = Color.Red;
                                fourlbl.Text = "One day to go";
                                break;
                            case 0:
                                panel7.BackColor = Color.DeepPink;
                                fourlbl.Text = "Need to Change Over today";
                                break;
                            default:
                                panel7.BackColor = Color.Gray;
                                fourlbl.Text = "More than 4 Days";
                                break;
                        }
                        // Panel 2
                        switch (daysDifference)
                        {
                            case 3:
                                panel8.BackColor = Color.Yellow;
                                threelbl.Text = "Three days to go";
                                break;
                            case 2:
                                panel8.BackColor = Color.Orange;
                                threelbl.Text = "Two days to go";
                                break;
                            case 1:
                                panel8.BackColor = Color.Red;
                                threelbl.Text = "One day to go";
                                break;
                            case 0:
                                panel8.BackColor = Color.DeepPink;
                                threelbl.Text = "Need to Change Over today";
                                break;
                            default:
                                panel8.BackColor = Color.Gray;
                                threelbl.Text = "More than 3 Days";
                                break;
                        }
                    }
                }
            }
            else if (DateTime.TryParse(scotxt.Text, out DateTime enteredDate1))
            {
                string enteredDat = enteredDate1.ToString("yyyy/MM/dd");
                DateTime givenCOD = DateTime.Parse(enteredDat);
                DateTime currentDate = DateTime.Now;
                string cur = currentDate.ToString("yyyy/MM/dd");
                DateTime curdate = DateTime.Parse(cur);
                TimeSpan difference = givenCOD - curdate;
                int daysDifference = difference.Days;
                if (progressBar2.Value == 100)
                {
                    sfourlbl.Text = "COD Completed";
                    Sthreelbl.Text = "COD Completed";
                    panel14.BackColor = Color.Green;
                    panel13.BackColor = Color.Green;
                }
                else
                {
                    if (daysDifference < 0)
                    {
                        panel14.BackColor = Color.Red;
                        panel13.BackColor = Color.Red;
                        sfourlbl.Text = "COD date already Crossed";
                        Sthreelbl.Text= "COD date already Crossed";
                    }
                    else
                    {
                        switch (daysDifference)
                        {
                            case 4:
                                panel14.BackColor = Color.Green;
                                string text = "Three days to go";
                                sfourlbl.Text = text;
                                break;
                            case 3:
                                panel14.BackColor = Color.Yellow;
                                sfourlbl.Text = "Three days to go";
                                break;
                            case 2:
                                panel14.BackColor = Color.Orange;
                                sfourlbl.Text = "Two days to go";
                                break;
                            case 1:
                                panel14.BackColor = Color.Red;
                                sfourlbl.Text = "One day to go";
                                break;
                            case 0:
                                panel14.BackColor = Color.DeepPink;
                                sfourlbl.Text = "Need to Change Over today";
                                break;
                            default:
                                panel14.BackColor = Color.Gray;
                                sfourlbl.Text = "More than 4 Days";
                                break;
                        }
                        // Panel 2
                        switch (daysDifference)
                        {
                            case 3:
                                panel13.BackColor = Color.Yellow;
                                Sthreelbl.Text = "Three days to go";
                                break;
                            case 2:
                                panel13.BackColor = Color.Orange;
                                Sthreelbl.Text = "Two days to go";
                                break;
                            case 1:
                                panel13.BackColor = Color.Red;
                                Sthreelbl.Text = "One day to go";
                                break;
                            case 0:
                                panel13.BackColor = Color.DeepPink;
                                Sthreelbl.Text = "Need to Change Over today";
                                break;
                            default:
                                panel13.BackColor = Color.Gray;
                                Sthreelbl.Text = "More than 3 Days";
                                break;
                        }
                    }    
                }  
            }
          
            else
            {

                MessageBox.Show("Invalid date format. Please enter a valid date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        public void LoadModel()
        {
            BMCOtxt.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            BMCOtxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Autodata = new AutoCompleteStringCollection();
            dt = new DataTable();
            Dictionary<string, string> kk = new Dictionary<string, string>();
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "AutoMChange",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    autocompleteMenu1.MaximumSize = new Size(250, 350);
                    var columnWidth = new[] { 50, 200 };
                    int n = 1;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        autocompleteMenu1.AddItem(new MulticolumnAutocompleteItem(new[] { n + "", dt.Rows[i]["B_MODEL"].ToString() }, dt.Rows[i]["B_MODEL"].ToString()) { ColumnWidth = columnWidth, ImageIndex = n });
                        n++;
                    }
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        internal void SetData(DataTable selectedRowData)
        {
            data1 = selectedRowData;
            for (int i = 0; i < data1.Rows.Count; i++)
            {
                S_NO = data1.Rows[i]["QCO_SEQUENCE"].ToString();
                Article = data1.Rows[i]["A_ART_NO"].ToString();
                Modelfrom = data1.Rows[i]["B_MODEL"].ToString();
                Modelto = data1.Rows[i]["A_MODEL"].ToString();
                COD = data1.Rows[i]["CO_DATE"].ToString();
                line = data1.Rows[i]["DEPARTMENT_CODE"].ToString();
            }          
            this.Refresh();
            GetRadiodata();
            Getprogressstatus();
            tabPage1.Visible = true;
            tabPage2.Visible = false;
            tabControl1.TabPages.Remove(tabPage2);
        }
        internal void SetData2(DataTable selectedRowData)
        {
            data1 = selectedRowData;
            for (int i = 0; i < data1.Rows.Count; i++)
            {

                S_NO1 = data1.Rows[i]["QCO_SEQUENCE"].ToString();
                Article1 = data1.Rows[i]["A_ART_NO"].ToString();
                Modelfrom1 = data1.Rows[i]["B_MODEL"].ToString();
                Modelto1 = data1.Rows[i]["A_MODEL"].ToString();   
                COD1 = data1.Rows[i]["CO_DATE"].ToString();
                line1 = data1.Rows[i]["DEPARTMENT_CODE"].ToString();
            }
            this.Refresh();
            GetRadiodata1();
            getprogressstatus();
            tabPage2.Visible = true;
            tabPage1.Visible = false;
            tabControl1.TabPages.Remove(tabPage1);
           // tabControl1.TabPages.Add(tabPage2);
        }
        public void GetAsemblyPictureImages()
        {
            List<Image> images = new List<Image>();
            byte[] img1;
            byte[] img2;
            byte[] img3;
            byte[] img4;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("QCO_Sequtxt", S_NO.ToString());

            string apiResponse = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getassemblypicture",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));
            string retDataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)?["RetData"]?.ToString();
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response.ContainsKey("RetData"))
                {
 
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)["RetData"].ToString();
                    Dictionary<string, object> dic2 = new Dictionary<string, object>();
                    dic2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);  
                    SetImageFromDictionaryKey(dic2, "SECHEAD_NAME", pictureBox1);
                    SetImageFromDictionaryKey(dic2, "PC_NAME", pictureBox2);
                    SetImageFromDictionaryKey(dic2, "TECHNICAL_NAME", pictureBox3);
                    SetImageFromDictionaryKey(dic2, "MAINTENANCE_NAME", pictureBox4);
                }
                else
                {
                    MessageBox.Show("RetData does not contain the expected property.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }
      
        }
        public void GetstitchPictureImages()
        {
            List<Image> images = new List<Image>();
            byte[] img1;
            byte[] img2;
            byte[] img3;
            byte[] img4;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("QCO_Sequtxt", S_NO1.ToString());//sseqtxt

            string apiResponse = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getstitchpictures",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));
            string retDataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)?["RetData"]?.ToString();
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response.ContainsKey("RetData"))
                {

                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)["RetData"].ToString();
                    Dictionary<string, object> dic2 = new Dictionary<string, object>();
                    dic2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    SetImageFromDictionaryKey(dic2, "SECHEAD_NAME", pictureBox9);
                    SetImageFromDictionaryKey(dic2, "PC_NAME", pictureBox7);
                    SetImageFromDictionaryKey(dic2, "TECHNICAL_NAME", pictureBox6);
                    SetImageFromDictionaryKey(dic2, "MAINTENANCE_NAME", pictureBox5);
                }
                else
                {
                    MessageBox.Show("RetData does not contain the expected property.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }

        }
        void SetImageFromDictionaryKey(Dictionary<string, object> dic, string key, PictureBox pictureBox)
        {
            if (dic.ContainsKey(key))
            {
                string base64String = dic[key]?.ToString()?.Replace("\"", "");

                if (!string.IsNullOrEmpty(base64String))
                {
                    byte[] imgBytes = Convert.FromBase64String(base64String);
                    Image image = ByteArrayToImage(imgBytes);

                    pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox.Image = image;
                }
            }
        }

        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        public void GetRadiodata()
         {
            _selectedRadioButton = "khaleel";
            Dictionary<string, string> kk = new Dictionary<string, string>();
            kk.Add("QCO_Sequtxt", S_NO.ToString());
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetRadiodata",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                radiodt = JsonConvert.DeserializeObject<DataTable>(json);
                if (radiodt == null || radiodt.Rows.Count == 0)
                {
                    return;
                }
           
                else
                {             
                    RadioButton[] radioButtons = { Linelayoutrbt1, valuerdb1, Manprdb1, Machinerdb1, CTPQrdb1, materialrdb1, Gaugerbn1, Sampleshoerbn1, cementrbd1};

                    if (radiodt != null && radiodt.Rows.Count > 0 && radiodt.Columns.Contains("RECORD_PROBLEMS") && radiodt.Columns.Contains("COT") && radiodt.Columns.Contains("COPT"))
                    {
                        probtxt.Text = radiodt.Rows[0]["RECORD_PROBLEMS"].ToString();
                        COTtxt.Text = radiodt.Rows[0]["COT"].ToString();
                        COPTtxt.Text = radiodt.Rows[0]["COPT"].ToString();
                        textramup.Text = radiodt.Rows[0]["RUT"].ToString();
                        cottimelbl.Text = radiodt.Rows[0]["COTTIME"].ToString();
                        Copttimelbl.Text = radiodt.Rows[0]["COPTTIME"].ToString();

                        var planTypeValue = radiodt.Rows[0]["S_PLAN_TYPE"];
                        if (radiodt.Rows.Count > 0 && !string.IsNullOrEmpty(radiodt.Rows[0]["ACTUAL_CODATE"].ToString()))
                        {
                            // Try to parse the date
                            if (DateTime.TryParse(radiodt.Rows[0]["ACTUAL_CODATE"].ToString(), out DateTime actualCODate))
                            {
            
                                AcCODdateTimePicker.Value = actualCODate;
                            }
                            else
                            {
            
                                MessageBox.Show("Invalid date value.");
                            }
                        }
                        else
                        {
  
                            MessageBox.Show("Missing date value.");
                        }


                        if (planTypeValue != DBNull.Value && planTypeValue != null)
                        {
                            string planType = planTypeValue.ToString();
                            if (aplancombo.Items.Contains(planType))
                            {

                                aplancombo.SelectedItem = planType;
                            }
                            else
                            {
                                MessageBox.Show($"The plan type '{planType}' is not available in the ComboBox.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {

                            MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            aplancombo.SelectedItem = null;
                        }

                        var Co_type = radiodt.Rows[0]["CO_TYPE"];
                        if (Co_type != DBNull.Value && Co_type != null)
                        {
                            string COType = Co_type.ToString();
                            if (acotypecombo.Items.Contains(COType))
                            {

                                acotypecombo.SelectedItem = COType;
                            }
                            else
                            {
                                MessageBox.Show($"The plan type '{COType}' is not available in the ComboBox.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {

                            MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            acotypecombo.SelectedItem = null;
                        }
                        var DELAY_REASONS = radiodt.Rows[0]["DELAY_REASONS"];
                        if (DELAY_REASONS != DBNull.Value && DELAY_REASONS != null)
                        {
                            string Delayrea = DELAY_REASONS.ToString();                       
                            Adelayrecombo.Text = Delayrea;

                        }
                        else
                        {

                            MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            acotypecombo.SelectedItem = null;
                        }

                    }

                    for (int i = 0; i < radioButtons.Length; i++)
                    {
                    
                        if (radiodt.Rows[0][i].ToString() == "Normal")
                        {
                            radioButtons[i].Checked = true;
                        }
                  
                    }
                    GetAsemblyPictureImages();
                    BindRaddiodates();

                 radioButtonStates = new Dictionary<string, bool>();

                    for (int i = 0; i < radioButtons.Length; i++)
                    {
                        string radioButtonState = radiodt.Rows[0][i].ToString();
                        bool isChecked = radioButtonState == "Normal";

                        Console.WriteLine($"RadioButton {radioButtons[i].Name} State: {radioButtonState}"); // Debugging

                        radioButtons[i].Checked = isChecked;
                        radioButtonStates[radioButtons[i].Name] = isChecked; // Mark radio button as checked or unchecked
                    }
                    Program.RadioButtonStates = radioButtonStates;
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        public void BindRaddiodates()
        {

            Dictionary<string, string> kk = new Dictionary<string, string>();
            kk.Add("QCO_Sequtxt", S_NO.ToString());
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "BindRaddiodates",
                Program.Client.UserToken, JsonConvert.SerializeObject(kk));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
               BRD = JsonConvert.DeserializeObject<DataTable>(json);
                if (BRD == null || BRD.Rows.Count == 0)
                {
                    return;
                }
             
                else
                {
                    for (int i = 0; i < BRD.Rows.Count; i++)
                    {
                        LINE_UPDATE_TIME = BRD.Rows[i]["LINE_LAY_UPDATE_DATE"].ToString();
                        VALUE_UPDATE_TIME = BRD.Rows[i]["VALUE_STR_UPDATE_DATE"].ToString();
                        MAN_UPDATE_TIME = BRD.Rows[i]["REQUIRED_MAN_UPDATE_DATE"].ToString();
                        MACHINE_UPDATE_TIME = BRD.Rows[i]["MACHINE_IN_UPDATE_DATE"].ToString();
                        CTPQ_UPDATE_TIME = BRD.Rows[i]["CTPQ_UPDATE_DATE"].ToString();
                        LAU_UPDATE_TIME = BRD.Rows[i]["MATERIAL_LAU_UPDATE_DATE"].ToString();
                        GAUGE_UPDATE_TIME = BRD.Rows[i]["GAUGE_MAR_UPDATE_DATE"].ToString();
                        SAMPLE_SH_UPDATE_TIME = BRD.Rows[i]["SAMPLE_SH_UPDATE_DATE"].ToString();
                        CEMENT_PR_UPDATE_TIME = BRD.Rows[i]["CEMENT_PR_UPDATE_DATE"].ToString();
                    }
                }
                
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }
        private void SSubmit_Click(object sender, EventArgs e)
        {
            Savedata1();
        }

        public void Savedata1()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor; // Set wait cursor at start

                if (selectplancombo.SelectedIndex == -1)
                {
                    MessageHelper.ShowErr(this, "Please Select Plan type");
                    return;
                }
                if (cotypecombo.SelectedIndex == -1)
                {
                    MessageHelper.ShowErr(this, "Please Select Model type");
                    return;
                }
                if (selectplancombo.SelectedIndex == 1)
                {
                    if (problemin_stit_txt.Text == "")
                    {
                        MessageHelper.ShowErr(this, "You need to Mantion reason for Un_Schedule Plan");
                        return;
                    }
                }

                byte[] byteArray1 = PictureBoxToByteArray(pictureBox9);
                byte[] byteArray2 = PictureBoxToByteArray(pictureBox7);
                byte[] byteArray3 = PictureBoxToByteArray(pictureBox6);
                byte[] byteArray4 = PictureBoxToByteArray(pictureBox6);
                if (string.IsNullOrEmpty(sbmodeltxt.Text) && string.IsNullOrEmpty(scotxt.Text))
                {
                    ShowErrorMessage("Before submit, Please Enter Model name");
                    return;
                }
                if (!ValidateSection(Slinerbtn2, Slinertxt))
                    return;
                if (!ValidateSection(Svaluesrbtn2, Svaluesrtxt))
                    return;
                if (!ValidateSection(SManprbtn2, SManprtxt))
                    return;
                if (!ValidateSection(Smachinerbtn2, Smachinertxt))
                    return;
                if (!ValidateSection(Sctpqrbtn2, Sctpqrtxt))
                    return;
                if (!ValidateSection(Ssamplerbtn2, Ssamplertxt))
                    return;
                if (!ValidateSection(Sgaugerbtn2, Sgaugertxt))
                    return;
                if (!ValidateSection(Smoldrbtn2, Smoldrtxt))
                    return;
                if (!ValidateSection(SCcomprbtn2, SCcomprtxt))
                    return;
                if (!ValidateSection(Seligibrbtn2, Seligibtxt))
                    return;
                if (!ValidateSection(SStitchingrbtn2, SStitchingrtxt))
                    return;
                if (!ValidateSection(Scsrbtn2, Scsrtxt))
                    return;

                Dictionary<string, object> kk = new Dictionary<string, object>();
                kk.Add("BModel", sbmodeltxt.Text.ToString());
                kk.Add("AfModel", samodeltxt.Text.ToString());
                kk.Add("COdate", scotxt.Text.ToString());
                kk.Add("Prodline", sdpttxt.Text.ToString());
                if (Slinerbtn1.Checked)
                {
                    kk.Add("Linelay", Slinerbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Linelay", Slinerbtn2.Text.ToString());
                }
                if (Svaluesrbtn1.Checked)
                {
                    kk.Add("valuer", Svaluesrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("valuer", Svaluesrbtn2.Text.ToString());
                }
                if (SManprbtn1.Checked)
                {
                    kk.Add("Manpr", SManprbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Manpr", SManprbtn2.Text.ToString());
                }
                if (Smachinerbtn1.Checked)
                {
                    kk.Add("Machine", Smachinerbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Machine", Smachinerbtn2.Text.ToString());
                }
                if (Sctpqrbtn1.Checked)
                {
                    kk.Add("CTPQr", Sctpqrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("CTPQr", Sctpqrbtn2.Text.ToString());
                }
                if (Ssamplerbtn1.Checked)
                {
                    kk.Add("material", Ssamplerbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("material", Ssamplerbtn2.Text.ToString());
                }
                if (Sgaugerbtn1.Checked)
                {
                    kk.Add("Gauge", Sgaugerbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Gauge", Sgaugerbtn2.Text.ToString());
                }
                if (Smoldrbtn1.Checked)
                {
                    kk.Add("Sampleshoe", Smoldrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Sampleshoe", Smoldrbtn2.Text.ToString());
                }
                if (SCcomprbtn1.Checked)
                {
                    kk.Add("Component", SCcomprbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Component", SCcomprbtn2.Text.ToString());
                }
                if (Seligibrbtn1.Checked)
                {
                    kk.Add("Eligible", Seligibrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Eligible", Seligibrbtn2.Text.ToString());
                }
                if (SStitchingrbtn1.Checked)
                {
                    kk.Add("Stitch", SStitchingrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("Stitch", SStitchingrbtn2.Text.ToString());
                }
                if (Scsrbtn1.Checked)
                {
                    kk.Add("CS", Scsrbtn1.Text.ToString());
                }
                else
                {
                    kk.Add("CS", Scsrbtn2.Text.ToString());
                }
                kk.Add("Remarks", Stitchremarks());

                kk.Add("SectionHd", byteArray1 != null ? byteArray1 : (object)DBNull.Value);
                kk.Add("Plantin", byteArray2 != null ? byteArray2 : (object)DBNull.Value);
                kk.Add("Technic", byteArray3 != null ? byteArray3 : (object)DBNull.Value);
                kk.Add("Maintenance", byteArray4 != null ? byteArray4 : (object)DBNull.Value);

                kk.Add("COT", cottxt2.Text.ToString());
                kk.Add("COPT", copttxt2.Text.ToString());
                kk.Add("QCO_Seq", sseqtxt.Text.ToString());
                kk.Add("Co_status", progressBar2.Value);
                kk.Add("problemin_stit", problemin_stit_txt.Text);

                kk.Add("S_line", S_line_time_txt.Text.ToString());
                kk.Add("S_vs", S_vs_time_txt.Text.ToString());
                kk.Add("S_mp", S_mp_time_txt.Text.ToString());
                kk.Add("S_man", S_man_time_txt.Text);
                kk.Add("S_ctpt", S_ctpt_time_txt.Text);
                kk.Add("S_sop", S_sop_time_txt.Text);
                kk.Add("S_gau", S_gau_time_txt.Text);
                kk.Add("S_mold", S_mold_time_txt.Text);
                kk.Add("S_com", S_com_time_txt.Text);
                kk.Add("S_eq", S_eq_time_txt.Text);
                kk.Add("S_cm", S_cm_time_txt.Text);
                kk.Add("S_cs", S_cs_time_txt.Text);

                kk.Add("S_plan", selectplancombo.SelectedItem.ToString());

                kk.Add("SCCOD", dateTimePicker1.Value.ToString("yyyy/MM/dd"));//2024/08/29
                                                                              // data.Add("Acotype", acotypecombo.SelectedItem.ToString()); //2024/08/29
                if (cotypecombo.SelectedItem != null)
                {
                    kk.Add("Scotype", cotypecombo.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("No item selected in Changeover Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit the method
                }
                if (selectplancombo.SelectedItem != null)
                {
                    string selectedPlan = selectplancombo.SelectedItem.ToString();

                    if (selectedPlan == "Scheduled")
                    {
                        kk.Add("Sdelayres", "Scheduled");
                    }
                    else if (selectedPlan == "Unplanned_Changeover")
                    {
                        if (comboBox2.SelectedItem != null)
                        {
                            kk.Add("Sdelayres", comboBox2.SelectedItem.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Please select a Delay reason for Unplanned Changeover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No valid item selected in Plan Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("No item selected in Plan Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                string rut = txtSRut.Text;
                kk.Add("RUT", txtSRut.Text);
                kk.Add("Scottime", Scottimelbl.Text);
                kk.Add("Scopttime", Scopttimelbl.Text);

                if (progressBar2.Value == 100)
                {
                    if (pictureBox9.Image == null || pictureBox7.Image == null || pictureBox6.Image == null || pictureBox5.Image == null)
                    {
                        MessageHelper.ShowErr(this, "Before submit, Upload all Signatures");
                        return;
                    }
                    string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                    string cot = cottxt2.Text;
                    string copt = copttxt2.Text;

 
                    Sendnotification("ME", $"{line1}",
                    $"📋 **Change Over Completion Details**\n\n" +
                    $"📍 Production Line: **{line1}**\n" +
                    $"🏭 Before Model: **{Modelfrom1}**\n" +
                    $"🆕 After Model: **{Modelto1}**\n" +
                    $"📅 CO Date: **{COD1}**\n" +
                    $"✅ Actual Completion Date: **{currentDate}**\n" +
                    $"⏱️ COT: **{cot}**\n" +
                    $"⚙️ COPT: **{copt}**\n" +
                    $"🔧 RUT: **{rut}**\n\n" +
                    $"📩 Please verify and confirm the above changeover details.\n" +
                    $"🙏 Thank you."
                );
                }


                    // **Check if RUT is being updated separately**
                    bool isSecondSubmission = !string.IsNullOrEmpty(rut) && NotificationsSent;

                if (!isSecondSubmission)  // **Send notification only on first submission**
                {
                    if (Slinerbtn1.Checked && SlinerbProcessed && !SlinerbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                            Sendnotification("CI", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"🏭 Line Layout (Before Model): **{Modelfrom1}**\n" +
                            $"🆕 Line Layout (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                            );



                        SlinerbAlertSent = true; // 🔒 Prevent repeat sending
                    }
                    if (Svaluesrbtn1.Checked && SvaluesrbProcessed && !SvaluesrbAlertSent) 
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("IE", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"🏭 Value Stream (Before Model): **{Modelfrom1}**\n" +
                            $"🆕 Value Stream (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SvaluesrbAlertSent = true;
                    }
                    if (SManprbtn1.Checked && SManprbProcessed && !SManprbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("ST", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"👷 Man Power (Before Model): **{Modelfrom1}**\n" +
                            $"🧑‍ Man Power (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SManprbAlertSent = true;
                    }
                    if (Smachinerbtn1.Checked && SmachinerbProcessed && !SmachineAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                        Sendnotification("MNT", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"🧰 Machine Inspection (Before Model): **{Modelfrom1}**\n" +
                            $"⚙️ Machine Inspection (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SmachineAlertSent = true;
                    }
                    if (Sctpqrbtn1.Checked && SctpqrbProcessed && !SctpqrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("QIP", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"🧾 CTPQ/CTPO (Before Model): **{Modelfrom1}**\n" +
                            $"🧪 CTPQ/CTPO (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );


                        SctpqrbAlertSent = true;
                    }
                    if (Ssamplerbtn1.Checked && SsamplerbProcessed && !SsamplerbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("ST", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"👞 Sample Shoe/SOP (Before Model): **{Modelfrom1}**\n" +
                            $"🧾 Sample Shoe/SOP (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SsamplerbAlertSent = true;
                    }
                    if (Sgaugerbtn1.Checked && SgaugerbProcessed && !SgaugerbAlertSent) 
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");


                        Sendnotification("ST", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"📏 Gauge Marking (Before Model): **{Modelfrom1}**\n" +
                            $"🖊 Gauge Marking (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SgaugerbAlertSent = true;
                    }
                    if (Smoldrbtn1.Checked && SmoldrbProcessed && !SmoldrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");


                        Sendnotification("PC", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"📦 Material Lausting (Before Model): **{Modelfrom1}**\n" +
                            $"🆕 Material Lausting (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SmoldrbAlertSent = true;
                    }
                    if (SCcomprbtn1.Checked && SCcomprbProcessed && !SCcomprbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("ST", $"{line1}",
                           $"👋 **Hello User!**\n" +
                           $"📢 This alert is from *QCO SYSTEM*\n\n" +
                           $"📍 Production Line:** **{line1}**\n" +
                           $"✂️  Cutting Component (Before Model): **{Modelfrom1}**\n" +
                           $"🆕 Cutting Component (After Model): **{Modelto1}**\n" +
                           $"📅 Changeover Date: **{COD1}**\n" +
                           $"✅ **Actual Completion Date:** **{currentDate}**\n\n" +
                           $"📩 Please review and confirm the above details.\n" +
                           $"🙏 Thank you."
                       );


                        SCcomprbAlertSent = true;
                    }
                    if (Seligibrbtn1.Checked && SeligibrbProcessed && !SeligibrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("ST", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"📊 Eligible Qty (Before Model): **{Modelfrom1}**\n" +
                            $"🆕 Eligible Qty (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ **Actual Completion Date:** **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SeligibrbAlertSent = true;
                    }
                    if (SStitchingrbtn1.Checked && SStitchingrbProcessed && !SStitchingrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification("ST", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"🧵 Stitching Component (Before Model): **{Modelfrom1}**\n" +
                            $"🪡  Stitching Component (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ **Actual Completion Date:** **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        SStitchingrbAlertSent = true;
                    }
                    if (Scsrbtn1.Checked && ScsrbProcessed && !ScsrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                        Sendnotification("TECH", $"{line1}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line1}**\n" +
                            $"💻 CS M/C Programme Setting (Before Model): **{Modelfrom1}**\n" +
                            $"🖥 CS M/C Programme Setting (After Model): **{Modelto1}**\n" +
                            $"📅 Changeover Date: **{COD1}**\n" +
                            $"✅ **Actual Completion Date:** **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        ScsrbAlertSent = true;
                    }
                    NotificationsSent = true;  // **Mark that notifications were sent**
                }

                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "SChecklist",
                    Program.Client.UserToken, JsonConvert.SerializeObject(kk));
                ProcessResponse(ret);
                ClearFields();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default; // Reset cursor back to normal after completion
            }
        }


        public DataTable Stitchremarks()
        {
            // Create a new DataTable
            DataTable dt = new DataTable();

            // Define the columns
            dt.Columns.Add("Slinerem", typeof(string));
            dt.Columns.Add("Svaluesrem", typeof(string));
            dt.Columns.Add("SManprrem", typeof(string));
            dt.Columns.Add("Smachinerem", typeof(string));
            dt.Columns.Add("Sctpqrrem", typeof(string));
            dt.Columns.Add("Ssamplerrem", typeof(string));
            dt.Columns.Add("Sgaugerem", typeof(string));
            dt.Columns.Add("Smoldrrem", typeof(string));
            dt.Columns.Add("SCcomprrem", typeof(string));
            dt.Columns.Add("Seligibrem", typeof(string));
            dt.Columns.Add("SStitchingrem", typeof(string));
            dt.Columns.Add("Scsrrem", typeof(string));
          

            // Create a new row
            DataRow row = dt.NewRow();

            // Get values from text boxes
            string SLine_rem = Slinertxt.Text;
            string SValue_rem = Svaluesrtxt.Text;
            string SMan_rem = SManprtxt.Text;
            string SMachine_rem = Smachinertxt.Text;
            string SCtpq_rem = Sctpqrtxt.Text;
            string Ssample_rem = Ssamplertxt.Text;
            string SGueage_rem = Sgaugertxt.Text;
            string Smoldrem = Smoldrtxt.Text;
            string SCcomprem = SCcomprtxt.Text;
            string Seligibrem = Seligibtxt.Text;
            string SStitchingrem = SStitchingrtxt.Text;
            string Scsrrem = Scsrtxt.Text;

            // Assign values to the row
            row["Slinerem"] = SLine_rem;
            row["Svaluesrem"] = SValue_rem;
            row["SManprrem"] = SMan_rem;
            row["Smachinerem"] = SMachine_rem;
            row["Sctpqrrem"] = SCtpq_rem;
            row["Ssamplerrem"] = Ssample_rem;
            row["Sgaugerem"] = SGueage_rem;
            row["Smoldrrem"] = Smoldrem;
            row["SCcomprrem"] = SCcomprem;
            row["Seligibrem"] = Seligibrem;
            row["SStitchingrem"] = SStitchingrem;
            row["Scsrrem"] = Scsrrem;
  

            // Add the row to the DataTable
            dt.Rows.Add(row);

            // Return the populated DataTable
            return dt;

        }

        private void ShowErrorMessage(string message)
        {
            MessageHelper.ShowErr(this, message);
        }

        private bool ValidateSection(RadioButton radioButton, TextBox textBox)
        {
            if (radioButton.Checked && string.IsNullOrEmpty(textBox.Text))
            {
                ShowErrorMessage("Please Mention the Remark");
                return false;
            }
            return true;
        }
       
        private void spcsigbtn_Click(object sender, EventArgs e)
        {
            int s = 6;
            int indexValue = 2;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue, s);
            pr.Show();

        }

        private void Stechsigbtn_Click(object sender, EventArgs e)
        {
            int s = 7;
            int indexValue = 3;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue, s);
            pr.Show();
        }

        private void smainsigbtn_Click(object sender, EventArgs e)
        {
            int s = 8;
            int indexValue = 4;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue, s);
            pr.Show();
        }

       
 
        private void getssigtxt_Click(object sender, EventArgs e)
        {

            int s = 5;
            int indexValue = 1;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue, s);
            pr.Show();

        }
        private void Slineuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //  T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Slineviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Svsuplodbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Svsviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Smpuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Smpviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void SMachinebtn_Click_1(object sender, EventArgs e)
        {
            //S_NO1, Modelfrom1, Modelto1, COD1, line1, Article1;
            string S_NO1 = sseqtxt.Text;
            string Modelfrom1 = sbmodeltxt.Text;
            string Modelto1 = samodeltxt.Text;
            string line1 = sdpttxt.Text;
            string COD1 = scotxt.Text;
            T_QCO_Equipment_Request newForm = new T_QCO_Equipment_Request();
            newForm.GetInstance(S_NO1, Modelfrom1, Modelto1, line1, COD1);
            newForm.Show();

        }

        private void Sctpqupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Sctpqview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Ssampbtn_Click_1(object sender, EventArgs e)
        {
            string Article1 = Aarttxt.Text;
            string Modelfrom1 = sbmodeltxt.Text;
            string Modelto1 = samodeltxt.Text;
            string line1 = sdpttxt.Text;
            string COD1 = scotxt.Text;

            T_QCO_SampleShoe shoe = new T_QCO_SampleShoe();
            shoe.GetInstance(Article1, Modelfrom1, Modelto1, line1, COD1);
            shoe.Show();

        }

        private void Ssopview_Click_1(object sender, EventArgs e)
        {
            string Article1 = Aarttxt.Text;
            string Modelfrom1 = sbmodeltxt.Text;
            string Modelto1 = samodeltxt.Text;
            string line1 = sdpttxt.Text;
            string COD1 = scotxt.Text;
            T_QCO_SOP_Require SOP = new T_QCO_SOP_Require();
            SOP.GetInstance(Article1, Modelfrom1, Modelto1, line1, COD1);
            SOP.Show();
        }

        private void Ssamupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Ssampview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Sgauuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Sgaugeview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Smoldupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Smoldview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Scaupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Scaview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Selibupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Seligview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Sstitupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Sstictview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Scsupload_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Scsview_Click_1(object sender, EventArgs e)
        {
            string seq = sseqtxt.Text;
            //T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }
        private void Slinerbtn1_Click(object sender, EventArgs e)
        {
            if (SlinerbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Slinerbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Slinerbtn1.Checked;
            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);
                double increment = UpdateCheckedCount1(Slinerbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_line_time_txt.Text = currentDate;
                // Mark the button as processed
                SlinerbProcessed = true;
               
            }
        }

        private void Svaluesrbtn1_Click(object sender, EventArgs e)
        {
            if (SvaluesrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
               Svaluesrbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Svaluesrbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);
                double increment = UpdateCheckedCount1(Svaluesrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_vs_time_txt.Text = currentDate;
                // Mark the button as processed
                SvaluesrbProcessed = true;
               
            }
        }

        private void SManprbtn1_Click(object sender, EventArgs e)
        {
            if (SManprbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SManprbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = SManprbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(SManprbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100);
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_mp_time_txt.Text = currentDate;
                // Mark the button as processed
                SManprbProcessed = true;
            }
        }

        private void Smachinerbtn1_Click(object sender, EventArgs e)
        {
            if (SmachinerbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Smachinerbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Smachinerbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Smachinerbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }

                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_man_time_txt.Text = currentDate;
                // Mark the button as processed
                SmachinerbProcessed = true;
            }
        }

        private void Sctpqrbtn1_Click(object sender, EventArgs e)
        {
            if (SctpqrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Sctpqrbtn1.Checked = false;
                return;
            }

            bool isCurrentlyChecked = Sctpqrbtn1.Checked;
            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Sctpqrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_ctpt_time_txt.Text = currentDate;
                // Mark the button as processed
                SctpqrbProcessed = true;
               
            }
        }

        private void Ssamplerbtn1_Click(object sender, EventArgs e)
        {
            if (SsamplerbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Ssamplerbtn1.Checked = false;
                return;
            }

            bool isCurrentlyChecked = Ssamplerbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Ssamplerbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100);
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_sop_time_txt.Text = currentDate;
                // Mark the button as processed
                SsamplerbProcessed = true;
            }
        }
        private void Sgaugerbtn1_Click(object sender, EventArgs e)
        {
            if (SgaugerbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Sgaugerbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Sgaugerbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Sgaugerbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_gau_time_txt.Text = currentDate;
                // Mark the button as processed
                SgaugerbProcessed = true;
                
            }
        }
        private void Smoldrbtn1_Click(object sender, EventArgs e)
        {
            if (SmoldrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Smoldrbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Smoldrbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Smoldrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); 
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_mold_time_txt.Text = currentDate;
                // Mark the button as processed
                SmoldrbProcessed = true;
            }
        }

        private void SCcomprbtn1_Click(object sender, EventArgs e)
        {
            if (SCcomprbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SCcomprbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = SCcomprbtn1.Checked;
            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(SCcomprbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100);
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_com_time_txt.Text = currentDate;
                // Mark the button as processed
               
            }
        }

        private void Seligibrbtn1_Click(object sender, EventArgs e)
        {
            if (SeligibrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Seligibrbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = Seligibrbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Seligibrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); // Ensure it remains within 0-100
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_eq_time_txt.Text = currentDate;
                // Mark the button as processed
               
            }
        }

        private void SStitchingrbtn1_Click(object sender, EventArgs e)
        {
            if (SStitchingrbProcessed)
            {
                MessageBox.Show("This action has already been processed.Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SStitchingrbtn1.Checked = false;
                return;
            }
            bool isCurrentlyChecked = SStitchingrbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(SStitchingrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); // Ensure it remains within 0-100
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_cm_time_txt.Text = currentDate;
                // Mark the button as processed
                
            }
        }

        private void Scsrbtn1_Click(object sender, EventArgs e)
        {
            if (ScsrbProcessed)
            {
                MessageBox.Show("This action has already been processed. Don't Click Several Times", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Scsrbtn1.Checked = false;
                return;
            }

            bool isCurrentlyChecked = Scsrbtn1.Checked;

            if (isCurrentlyChecked)
            {
                double currentSstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

                double increment = UpdateCheckedCount1(Scsrbtn1.Checked);
                double newProgress = currentSstatus + increment;
                int roundedProgress = (int)Math.Round(newProgress);
                StitchProgress.Text = $"{roundedProgress:F2} %";
                if (progressBar2 != null)
                {
                    int progressValue = (int)Math.Min(Math.Max(newProgress, 0), 100); // Ensure it remains within 0-100
                    progressBar2.Value = progressValue;
                }


                string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                S_cs_time_txt.Text = currentDate;
                // Mark the button as processed
               
            }
        }



        private void sshsigbtmn_Click(object sender, EventArgs e)
        {

            int s = 5;
            int indexValue = 1;
            QCO_Get_Signatures pr = new QCO_Get_Signatures(indexValue, s);
            pr.Show();
        }

        private void Uploadbtn1_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            Cursor.Current = Cursors.WaitCursor;
            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void ViewbtnL_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void VSuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void VSviewbtn_Click_1(object sender, EventArgs e)
        {

            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Mnuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

        private void Mnviewbtn_Click_1(object sender, EventArgs e)
        {

            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Clickbtn1_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            string Modelfrom = BMCOtxt.Text;
            string Modelto = A_MCOtxt.Text;
            string line = linetxt.Text;
            string COD = COdatetxt.Text;
            T_QCO_Equipment_Request newForm = new T_QCO_Equipment_Request();
            newForm.GetInstance(seq, Modelfrom, Modelto, line, COD);
            newForm.Show();
        }

        private void Qipuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Qipviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Pcuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Pcviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Wsuploadbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();
        }

      
       

       


        private readonly List<string> _unscheduledReasons = new List<string>
    {
        "Equipment Failure", "Material Shortage", "Operator Absence", "Quality Issues",
        "Unexpected Maintenance", "Power Outage", "Technical Issues", "Safety Incidents",
        "Waiting for Approval", "Change in Production Plan", "Logistics Issues", "Environmental Factors"
    };

      

        /// 2025/03/12 Change
        private void aplancombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Adelayrecombo.Items.Clear();
            if (aplancombo.SelectedItem.ToString() == "Unplanned_Changeover")
            {

                Adelayrecombo.Items.AddRange(_unscheduledReasons.ToArray());
            }
        }
        private void selectplancombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (selectplancombo.SelectedItem.ToString() == "Unplanned_Changeover")
            {

                comboBox2.Items.AddRange(_unscheduledReasons.ToArray());
            }

        }
        public Dictionary<string, Image> GetAsemblyPictureImages22()
        {
            var images = new Dictionary<string, Image>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("QCO_Sequtxt", S_NO.ToString());

            string apiResponse = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getassemblypicture",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response.ContainsKey("RetData"))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)["RetData"].ToString();
                    var imageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    foreach (var kvp in imageData)
                    {
                        string key = kvp.Key;
                        string base64String = kvp.Value?.ToString()?.Replace("\"", "");

                        if (!string.IsNullOrEmpty(base64String))
                        {
                            byte[] imgBytes = Convert.FromBase64String(base64String);
                            Image image = ByteArrayToImage(imgBytes);
                            images[key] = image;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("RetData does not contain the expected property.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }

            return images;
        }
        public Dictionary<string, Image> GetstitchPictureImages99()
        {
            var images = new Dictionary<string, Image>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("QCO_Sequtxt", S_NO1.ToString());

            string apiResponse = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getstitchpictures",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));
            var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response.ContainsKey("RetData"))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse)["RetData"].ToString();
                    var imageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    foreach (var kvp in imageData)
                    {
                        string key = kvp.Key;
                        string base64String = kvp.Value?.ToString()?.Replace("\"", "");

                        if (!string.IsNullOrEmpty(base64String))
                        {
                            byte[] imgBytes = Convert.FromBase64String(base64String);
                            Image image = ByteArrayToImage(imgBytes);
                            images[key] = image;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("RetData does not contain the expected property.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }

            return images;
        }

        private void Sprintbtn_Click(object sender, EventArgs e)
        {
            // Call GetRadiodata first to populate RadioButton states
            GetRadiodata1();

            // Ensure Program.RadioButtonStates is populated correctly
            if (Program.SRadioButtonStates == null)
            {
                MessageBox.Show("Error: Radio button states are not loaded. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path = Path.Combine(Application.StartupPath, "T_Quick_Changeover", "Stitching_Checklist_Fast_report.frx");

            try
            {
                // Create and populate the DataTable
                DataTable data = new DataTable();
                data.Columns.Add("QCO_Seq");
                data.Columns.Add("Dept");
                data.Columns.Add("COD");
                data.Columns.Add("BModel");
                data.Columns.Add("AModel");
                data.Columns.Add("ActualCOD");
                data.Columns.Add("DelayReasons");
                data.Columns.Add("Aarticle");
                data.Columns.Add("COType");
                data.Columns.Add("Select_Plan");
                data.Columns.Add("Problem");

                data.Columns.Add("COT");
                data.Columns.Add("COPT");
                data.Columns.Add("RUT");

                data.Columns.Add("Line_Adate");
                data.Columns.Add("Value_Adate");
                data.Columns.Add("MN_Adate");
                data.Columns.Add("Machine_Adate");
                data.Columns.Add("CTPT_Adate");
                data.Columns.Add("SOP_Adate");
                data.Columns.Add("Gauge_Adate");
                data.Columns.Add("Material_Adate");
                data.Columns.Add("Cutting_Adate");
                data.Columns.Add("Egbl_Adate");
                data.Columns.Add("Stitch_Adate");
                data.Columns.Add("CS_Adate");
            


                DataRow newRow = data.NewRow();
                newRow["QCO_Seq"] = sseqtxt.Text;
                newRow["Dept"] = sdpttxt.Text;
                newRow["COD"] = scotxt.Text;
                newRow["BModel"] = sbmodeltxt.Text;
                newRow["AModel"] = samodeltxt.Text;
                newRow["ActualCOD"] = dateTimePicker1.Text;
                newRow["DelayReasons"] = comboBox2.Text;
                newRow["Aarticle"] = Aarttxt.Text;
                newRow["COType"] = cotypecombo.Text;
                newRow["Select_Plan"] = selectplancombo.Text;
                newRow["Problem"] = problemin_stit_txt.Text;
                newRow["COT"] = cottxt2.Text;
                newRow["COPT"] = copttxt2.Text;
                newRow["RUT"] = txtSRut.Text;

                newRow["Line_Adate"] = S_line_time_txt.Text;
                newRow["Value_Adate"] = S_vs_time_txt.Text;
                newRow["MN_Adate"] = S_mp_time_txt.Text;
                newRow["Machine_Adate"] = S_man_time_txt.Text;
                newRow["CTPT_Adate"] = S_ctpt_time_txt.Text;
                newRow["SOP_Adate"] = S_sop_time_txt.Text;
                newRow["Gauge_Adate"] = S_gau_time_txt.Text;
                newRow["Material_Adate"] = S_mold_time_txt.Text;
                newRow["Cutting_Adate"] = S_com_time_txt.Text;
                newRow["Egbl_Adate"] = S_eq_time_txt.Text;
                newRow["Stitch_Adate"] = S_cm_time_txt.Text;
                newRow["CS_Adate"] = S_cs_time_txt.Text;

                data.Rows.Add(newRow);

                var SradioButtonMappings = new Dictionary<string, string>
        {
            { "SCheckBox1", "Slinerbtn1" },
            { "SCheckBox2", "Slinerbtn2" },
            { "SCheckBox3", "Svaluesrbtn1" },
            { "SCheckBox4", "Svaluesrbtn2" },
            { "SCheckBox5", "SManprbtn1" },
            { "SCheckBox6", "SManprbtn2" },
            { "SCheckBox7", "Smachinerbtn1" },
            { "SCheckBox8", "Smachinerbtn2" },
            { "SCheckBox9", "Sctpqrbtn1" },
            { "SCheckBox10", "Sctpqrbtn2" },
            { "SCheckBox11", "Ssamplerbtn1" },
            { "SCheckBox12", "Ssamplerbtn2" },
            { "SCheckBox13", "Sgaugerbtn1" },
            { "SCheckBox14", "Sgaugerbtn2" },
            { "SCheckBox15", "Smoldrbtn1" },
            { "SCheckBox16", "Smoldrbtn2" },
            { "SCheckBox17", "SCcomprbtn1" },
            { "SCheckBox18", "SCcomprbtn2" },
            { "SCheckBox19", "Seligibrbtn1" },
            { "SCheckBox20", "Seligibrbtn2" },
            { "SCheckBox21", "SStitchingrbtn1" },
            { "SCheckBox22", "SStitchingrbtn2" },
            { "SCheckBox23", "Scsrbtn1" },
            { "SCheckBox24", "Scsrbtn2" }
            
        };

                // Fetch images
                var images = GetstitchPictureImages99();

                // Create and show the FastReport
                Stitching_Checklist file = new Stitching_Checklist(data, path, Program.SRadioButtonStates, SradioButtonMappings, images);
                file.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Object not found: {ex.TargetSite?.Name}");
            }

        }
        string firtpaircoptval;
       

        private void Calculate_RUTbtn_Click(object sender, EventArgs e)
        {
            int Aline = 1;
            string dept = linetxt.Text;
            T_QCO_RAMP_UP Ramp_up = new T_QCO_RAMP_UP(Aline, dept,firtpaircoptval);
            Ramp_up.Show();

           
        }


        public void SetTextBoxValue(string rampvalue)
        {
            firtpaircoptval = rampvalue; // Assuming `textBoxInForm1` is your target TextBox
        }
       

        private void btncalrutS_Click(object sender, EventArgs e)
        {
            int Sline = 2;
            string dept = sdpttxt.Text;
            T_QCO_RAMP_UP Ramp_up = new T_QCO_RAMP_UP(Sline,dept, firtpaircoptval);
            Ramp_up.Show();

        }
        //private void Slinerbtn2_Click(object sender, EventArgs e)
        //{
        //    if (Program.SRadioButtonStates.ContainsKey("Slinerbtn1") && Program.SRadioButtonStates["Slinerbtn1"])
        //    {
        //        MessageBox.Show("Already Data Saved — can't decrement.");
        //        Slinerbtn1.Checked = true;
        //        return;
        //    }
        //    double status = 0;
        //    if (!string.IsNullOrEmpty(Sstatus))
        //    {
        //        status = Convert.ToDouble(Sstatus);
        //    }
        //    double currentAstatus = status;

        //    double decrement = UpdateCheckedCount1(false);
        //    double newProgress = currentAstatus - decrement;
        //    int roundedProgress = (int)Math.Round(newProgress);  //
        //    StitchProgress.Text = $"{roundedProgress:F2} %"; 
        //    S_line_time_txt.Text = "";
        //    Sstatus = roundedProgress.ToString("F2");
        //}
        private void Slinerbtn2_Click(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Slinerbtn1") && Program.SRadioButtonStates["Slinerbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Slinerbtn1.Checked = true;
                return;
            }

            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);

            // Decrease current status by decrement
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_line_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
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

        private void panel9_Paint(object sender, PaintEventArgs e)
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

        private void Svaluesrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Svaluesrbtn1") && Program.SRadioButtonStates["Svaluesrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Svaluesrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_vs_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }
        private void SManprbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("SManprbtn1") && Program.SRadioButtonStates["SManprbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                SManprbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_mp_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }
        private void Smachinerbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Smachinerbtn1") && Program.SRadioButtonStates["Smachinerbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Smachinerbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_man_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }
       

       
        private void Sctpqrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Sctpqrbtn1") && Program.SRadioButtonStates["Sctpqrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Sctpqrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_ctpt_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void Ssamplerbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Ssamplerbtn1") && Program.SRadioButtonStates["Ssamplerbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Ssamplerbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_sop_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void Sgaugerbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Sgaugerbtn1") && Program.SRadioButtonStates["Sgaugerbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Sgaugerbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_gau_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void Smoldrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Smoldrbtn1") && Program.SRadioButtonStates["Smoldrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Smoldrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_mold_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void SCcomprbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("SCcomprbtn1") && Program.SRadioButtonStates["SCcomprbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                SCcomprbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_com_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void Seligibrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Seligibrbtn1") && Program.SRadioButtonStates["Seligibrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Seligibrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_eq_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }

        private void SStitchingrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("SStitchingrbtn1") && Program.SRadioButtonStates["SStitchingrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                SStitchingrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);
            S_cm_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }
       
        private void Scsrbtn2_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.SRadioButtonStates.ContainsKey("Scsrbtn1") && Program.SRadioButtonStates["Scsrbtn1"])
            {
                MessageBox.Show("Already Data Saved — can't decrement.");
                Scsrbtn1.Checked = true;
                return;
            }
            double currentAstatus = string.IsNullOrEmpty(Sstatus) ? 0 : Convert.ToDouble(Sstatus);

            double decrement = UpdateCheckedCount1(false);
            double newProgress = Math.Max(currentAstatus - decrement, 0);

            // Round the result
            int roundedProgress = (int)Math.Round(newProgress);

            // Update UI and internal status
            StitchProgress.Text = $"{roundedProgress:F2} %";
            progressBar2.Value = Math.Min(roundedProgress, progressBar2.Maximum);        
            S_cs_time_txt.Text = "";
            Sstatus = roundedProgress.ToString("F2");
        }
        private void APrintbtn_Click(object sender, EventArgs e)
        {
            // Call GetRadiodata first to populate RadioButton states
            GetRadiodata();

            // Ensure Program.RadioButtonStates is populated correctly
            if (Program.RadioButtonStates == null)
            {
                MessageBox.Show("Error: Radio button states are not loaded. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path = Path.Combine(Application.StartupPath, "T_Quick_Changeover", "Assembly_Checklist_design.frx");

            try
            {
                // Create and populate the DataTable
                DataTable data = new DataTable();
                data.Columns.Add("QCO_Seq");
                data.Columns.Add("Dept");
                data.Columns.Add("COD");
                data.Columns.Add("BModel");
                data.Columns.Add("AModel");
                data.Columns.Add("ActualCOD");
                data.Columns.Add("DelayReasons");
                data.Columns.Add("Aarticle");
                data.Columns.Add("COType");
                data.Columns.Add("Select_Plan");
                data.Columns.Add("Problem");

                data.Columns.Add("COT");
                data.Columns.Add("COPT");
                data.Columns.Add("RUT");

                data.Columns.Add("Line_Adate");
                data.Columns.Add("Value_Adate");
                data.Columns.Add("MN_Adate");
                data.Columns.Add("Machine_Adate");
                data.Columns.Add("CTPT_Adate");
                data.Columns.Add("Laust_Adate");
                data.Columns.Add("Gauge_Adate");
                data.Columns.Add("SOP_Adate");
                data.Columns.Add("Cement_Adate");


                DataRow newRow = data.NewRow();
                newRow["QCO_Seq"] = QCO_Sequtxt.Text;
                newRow["Dept"] = linetxt.Text;
                newRow["COD"] = COdatetxt.Text;
                newRow["BModel"] = BMCOtxt.Text;
                newRow["AModel"] = A_MCOtxt.Text;
                newRow["ActualCOD"] = AcCODdateTimePicker.Text;
                newRow["DelayReasons"] = Adelayrecombo.Text;
                newRow["Aarticle"] = Articletext.Text;
                newRow["COType"] = acotypecombo.Text;
                newRow["Select_Plan"] = aplancombo.Text;
                newRow["Problem"] = probtxt.Text;
                newRow["COT"] = COTtxt.Text;
                newRow["COPT"] = COPTtxt.Text;
                newRow["RUT"] = textramup.Text;

                newRow["Line_Adate"] = line_update_txt.Text;
                newRow["Value_Adate"] = Value_Update_txt.Text;
                newRow["MN_Adate"] = Man_Update_txt.Text;
                newRow["Machine_Adate"] = Machine_update_txt.Text;
                newRow["CTPT_Adate"] = Ctpq_Update_txt.Text;
                newRow["Laust_Adate"] = Mold_Update_txt.Text;
                newRow["Gauge_Adate"] = Gaue_Update_txt.Text;
                newRow["SOP_Adate"] = Sop_update_txt.Text;
                newRow["Cement_Adate"] = Cement_update_txt.Text;

                data.Rows.Add(newRow);

                var radioButtonMappings = new Dictionary<string, string>
        {
            { "CheckBox1", "Linelayoutrbt1" },
            { "CheckBox2", "Linelayoutrbt2" },
            { "CheckBox3", "valuerdb1" },
            { "CheckBox4", "valuerdb2" },
            { "CheckBox5", "Manprdb1" },
            { "CheckBox6", "Manprdb2" },
            { "CheckBox7", "Machinerdb1" },
            { "CheckBox8", "Machinerdb2" },
            { "CheckBox9", "CTPQrdb1" },
            { "CheckBox10", "CTPQrdb2" },
            { "CheckBox11", "materialrdb1" },
            { "CheckBox12", "materialrdb2" },
            { "CheckBox13", "Gaugerbn1" },
            { "CheckBox14", "Gaugerbn2" },
            { "CheckBox15", "Sampleshoerbn1" },
            { "CheckBox16", "Sampleshoerbn2" },
            { "CheckBox17", "cementrbd1" },
            { "CheckBox18", "cementrbd2" }
        };

                // Fetch images
                var images = GetAsemblyPictureImages22();

                // Create and show the FastReport
                Assembly_checklist file = new Assembly_checklist(data, path, Program.RadioButtonStates, radioButtonMappings, images);
                file.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}. Object not found: {ex.TargetSite?.Name}");
            }
        }

        private void Wsviewbtn_Click_1(object sender, EventArgs e)
        {

            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void Shoeissuingbtn_Click_1(object sender, EventArgs e)
        {
            string Article = Articletext.Text;
            string Modelfrom = BMCOtxt.Text;
            string Modelto = A_MCOtxt.Text;
            string line = linetxt.Text;
            string COD = COdatetxt.Text;

            T_QCO_SampleShoe shoe = new T_QCO_SampleShoe();
            shoe.GetInstance(Article, Modelfrom, Modelto, line, COD);
            shoe.Show();
        }

        private void SOPbtn_Click_1(object sender, EventArgs e)
        {
            string Article = Articletext.Text;
            string Modelfrom = BMCOtxt.Text;
            string Modelto = A_MCOtxt.Text;
            string line = linetxt.Text;
            string COD = COdatetxt.Text;
            T_QCO_SOP_Require SOP = new T_QCO_SOP_Require();
            SOP.GetInstance(Article, Modelfrom, Modelto, line, COD);
            SOP.Show();
        }

        private void Sopupbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Sopviewbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();

        }

        private void Cmupbtn_Click_1(object sender, EventArgs e)
        {
            string seq = QCO_Sequtxt.Text;

            // T_QCO_Ex_app_t_fileUpload_add File = new T_QCO_Ex_app_t_fileUpload_add();
            QCO_FileUpload File = new QCO_FileUpload();
            File.Addseq(seq);
            File.Show();

        }

        private void Cmviewbtn_Click_1(object sender, EventArgs e)
        {

            string seq = QCO_Sequtxt.Text;
            // T_QCO_Ex_app_t_fileUpload_view view = new T_QCO_Ex_app_t_fileUpload_view();
            QCO_Upload view = new QCO_Upload();
            view.Addseq2(seq);
            view.Show();
        }

        private void COTcalbtn_Click_1(object sender, EventArgs e)
        {

            int Sline = 2;
            string dept = sdpttxt.Text;
            T_QCO_COT_COPT cot = new T_QCO_COT_COPT(Sline, dept,this);
            //cot.Show();
            cot.Owner = this; // Set Form1 as the owner of Form2
            cot.ShowDialog(); // Use ShowDialog for modal behavior
        }
        public void getprogressstatus()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seq", S_NO1);
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getprogressstatus",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));

            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {             
                if (response["RetData"] != null)
                {
                    string json = response["RetData"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt.Rows.Count == 0)
                    {
                        return; 
                    }
               Sstatus = dt.Rows[0]["CO_STATUS"] != DBNull.Value ? dt.Rows[0]["CO_STATUS"].ToString() : string.Empty;

                    if (!string.IsNullOrEmpty(Sstatus) && Sstatus == "100")
                    {
                        progressBar2.Value = 100;
                        StitchProgress.Text = "100";
                    }
                    else
                    {
                        progressBar2.Value =Convert.ToInt32(Sstatus);
                        StitchProgress.Text = Sstatus;
                    }
                }
                else
                {
                    MessageBox.Show("No data available in the response.");
                }
            }
        }
        public void Getprogressstatus()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("seq", S_NO);
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetAprogrestatus",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));

            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                if (response["RetData"] != null)
                {
                    string json = response["RetData"].ToString();
                    dt = JsonConvert.DeserializeObject<DataTable>(json);
                    if (dt.Rows.Count == 0)
                    {
                        return; 
                    }
                  Astatus = dt.Rows[0]["CO_STATUS"] != DBNull.Value ? dt.Rows[0]["CO_STATUS"].ToString() : string.Empty;

                    if (!string.IsNullOrEmpty(Astatus) && Astatus == "100")
                    {
                        progressBar1.Value = 100;
                        Progressvalue.Text = "100";
                    }
                    else
                    {
                        progressBar1.Value =Convert.ToInt32(Astatus);
                        Progressvalue.Text = Astatus;
                    }
                }
                else
                {                   
                    MessageBox.Show("No data available in the response.");
                }
            }
        }

        public void GetRadiodata1()
        {
            _SselectedRadioButton = "khaleel";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
        { "QCO_Sequtxt", S_NO1.ToString() 
                }
            };

            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "GetRadiodata1",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));

            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                string json = response["RetData"].ToString();
                dt = JsonConvert.DeserializeObject<DataTable>(json);

                if (dt == null || dt.Rows.Count == 0)
                    return;      
                RadioButton[] radioButtons = { Slinerbtn1, Svaluesrbtn1, SManprbtn1, Smachinerbtn1, Sctpqrbtn1, Ssamplerbtn1, Sgaugerbtn1, Smoldrbtn1, SCcomprbtn1, Seligibrbtn1, SStitchingrbtn1, Scsrbtn1 };
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("RECORD_PROBLEMS") && dt.Columns.Contains("COT") && dt.Columns.Contains("COPT"))
                {
                    problemin_stit_txt.Text = dt.Rows[0]["RECORD_PROBLEMS"].ToString();
                    cottxt2.Text = dt.Rows[0]["COT"].ToString();
                    copttxt2.Text = dt.Rows[0]["COPT"].ToString();
                    txtSRut.Text = dt.Rows[0]["RUT"].ToString();

                    Scottimelbl.Text = dt.Rows[0]["COTTIME"].ToString();
                    Scopttimelbl.Text = dt.Rows[0]["COPTTIME"].ToString();

                    var planTypeValue = dt.Rows[0]["S_PLAN_TYPE"];
                    if (planTypeValue != DBNull.Value && planTypeValue != null)
                    {
                        string planType = planTypeValue.ToString();
                        if (selectplancombo.Items.Contains(planType))
                        {

                            selectplancombo.SelectedItem = planType;
                        }
                        else
                        {                           
                            MessageBox.Show($"The plan type '{planType}' is not available in the ComboBox.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        selectplancombo.SelectedItem = null; 
                    }
                    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["SACTUAL_CODATE"].ToString()))
                    {
                        if (DateTime.TryParse(dt.Rows[0]["SACTUAL_CODATE"].ToString(), out DateTime actualCODate))
                        {
                            dateTimePicker1.Value = actualCODate;
                        }
                        else
                        {
                            MessageBox.Show("Invalid date value.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Missing date value.");
                    }

                    var SCo_type = dt.Rows[0]["SCO_TYPE"];
                    if (SCo_type != DBNull.Value && SCo_type != null)
                    {
                        string SCOType = SCo_type.ToString();
                        if (cotypecombo.Items.Contains(SCOType))
                        {

                            cotypecombo.SelectedItem = SCOType;
                        }
                        else
                        {
                            MessageBox.Show($"The plan type '{SCOType}' is not available in the ComboBox.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {

                        MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        acotypecombo.SelectedItem = null;
                    }
                    var SDELAY_REASONS = dt.Rows[0]["SDELAY_REASONS"];
                    if (SDELAY_REASONS != DBNull.Value && SDELAY_REASONS != null)
                    {
                        string SDelayrea = SDELAY_REASONS.ToString();
                        comboBox2.Text = SDelayrea;


                    }
                    else
                    {

                        MessageBox.Show("The plan type is null or invalid.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        acotypecombo.SelectedItem = null;
                    }
                }

                for (int i = 0; i < radioButtons.Length; i++)
                {
                   // radioButtons[i].Text = dt.Rows[0][i].ToString();

                    if (dt.Rows[0][i].ToString() == "Normal")
                    {
                        radioButtons[i].Checked = true;
                    }
                }
                GetstitchPictureImages();
                BindStitchdate();
              


                 SradioButtonStates = new Dictionary<string, bool>();

                for (int i = 0; i < radioButtons.Length; i++)
                {
                    string radioButtonState = dt.Rows[0][i].ToString();
                    bool isChecked = radioButtonState == "Normal";
                    radioButtons[i].Checked = isChecked;
                    SradioButtonStates[radioButtons[i].Name] = isChecked; 
                }
                Program.SRadioButtonStates = SradioButtonStates;


            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }
        }
        public void BindStitchdate()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>
    {
        { "QCO_Sequtxt", S_NO1.ToString() }
    };

            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "BindStitchdate",
                Program.Client.UserToken, JsonConvert.SerializeObject(parameters));

            Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

            if (Convert.ToBoolean(response["IsSuccess"]))
            {
                string json = response["RetData"].ToString();
                SBRD = JsonConvert.DeserializeObject<DataTable>(json);

                if (SBRD == null || SBRD.Rows.Count == 0)
                    return;
                for (int i = 0; i < SBRD.Rows.Count; i++)
                {
                    S_LINEUPDATE_DATE = SBRD.Rows[i]["LINE_LAY_UPDATE_DATE"].ToString();
                    S_VALUEUPDATE_DATE = SBRD.Rows[i]["VALUE_STR_UPDATE_DATE"].ToString();
                    S_REQUIREDUPDATE_DATE = SBRD.Rows[i]["REQUIRED_MAN_UPDATE_DATE"].ToString();
                    SMACHINUPDATE_DATE = SBRD.Rows[i]["MACHINE_IN_UPDATE_DATE"].ToString();
                    S_CTPQ_UPDATE_DATE = SBRD.Rows[i]["CTPQ_UPDATE_DATE"].ToString();
                    S_SAMPLEUPDATE_DATE = SBRD.Rows[i]["SAMPLE_SH_UPDATE_DATE"].ToString();
                    S_GAUGE_UPDATE_DATE = SBRD.Rows[i]["GAUGE_MAR_UPDATE_DATE"].ToString();
                    S_MATERIAL_UPDATE_DATE = SBRD.Rows[i]["MATERIAL_LAU_UPDATE_DATE"].ToString();
                    S_COMPONENT_UPDATE_DATE = SBRD.Rows[i]["COMPONENT_UPDATE_DATE"].ToString();
                    S_ELIGIBLE_UPDATE_DATE = SBRD.Rows[i]["ELIGIBLE_UPDATE_DATE"].ToString();
                    S_STITCHINGUPDATE_DATE = SBRD.Rows[i]["STITCHING_UPDATE_DATE"].ToString();
                    S_CS_MC_UPDATE_DATE = SBRD.Rows[i]["CS_MC_UPDATE_DATE"].ToString();
                }


            }
            else
            {
                MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
            }
        }

     
        public void Savedata()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;  // ⬅️ Set loading cursor ON

                if (aplancombo.SelectedIndex == -1)
                {
                    MessageHelper.ShowErr(this, "Please Select Plan type");
                    return;
                }
                if (aplancombo.SelectedIndex == 1 && string.IsNullOrWhiteSpace(probtxt.Text))
                {
                    MessageHelper.ShowErr(this, "You need to Mention reason for Un_Schedule Plan");
                    return;
                }
                if (acotypecombo.SelectedIndex == -1)
                {
                    MessageHelper.ShowErr(this, "Please Select Change over type");
                    return;
                }
                if (!ValidateRequiredFields())
                    return;

                string rut = textramup.Text;

                if (progressBar1.Value == 100)
                {
                    if (pictureBox1.Image == null || pictureBox2.Image == null ||
                        pictureBox3.Image == null || pictureBox4.Image == null)
                    {
                        MessageHelper.ShowErr(this, "Before submit, Upload all Signatures");
                        return;
                    }

                    string cot = COTtxt.Text;
                    string copt = COPTtxt.Text;
                    string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                    Sendnotification("ME", $"{line}",
                         $"📋 **Change Over Completion Details**\n\n" +
                         $"📍 Production Line: **{line}**\n" +
                         $"🏭 Before Model: **{Modelfrom}**\n" +
                         $"🆕 After Model: **{Modelto}**\n" +
                         $"📅 CO Date: **{COD}**\n" +
                         $"✅ Actual Completion Date: **{currentDate}**\n" +
                         $"⏱️ COT: **{cot}**\n" +
                         $"⚙️ COPT: **{copt}**\n" +
                         $"🔧 RUT: **{rut}**\n\n" +
                         $"📩 Please verify and confirm the above changeover details.\n" +
                         $"🙏 Thank you."
                     );
                }

                bool isSecondSubmission = !string.IsNullOrEmpty(rut) && NotificationsSent;

                if (!isSecondSubmission)
                {
                    if (Linelayoutrbt1.Checked && LinelayoutProcessed && !LinelayoutAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");


                        Sendnotification("CI", $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line}**\n" +
                            $"🏭 Line Layout (Before Model): **{Modelfrom}**\n" +
                            $"🆕 Line Layout (After Model): **{Modelto}**\n" +
                            $"📅 Changeover Date: **{COD}**\n" +
                            $"✅ Actual Completion Date: **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );
                        


                        LinelayoutAlertSent = true;
                    }
                    if (valuerdb1.Checked && isvalueProcessed && !isvalueAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");
                        Sendnotification("IE", $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"📍 Production Line: **{line}**\n" +
                            $"🏭 Value Stream (Before Model): **{Modelfrom}**\n" +
                            $"🆕 Value Stream (After Model): **{Modelto}**\n" +
                            $"📅 Changeover Date: **{COD}**\n" +
                            $"✅ **Actual Completion Date:** **{currentDate}**\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        isvalueAlertSent = true;
                    }
                    if (Manprdb1.Checked && ismanpProcessed && !ismanpAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "AL",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"🛠 Required Manpower Before Model: {Modelfrom}\n" +
                            $"✅ Required Manpower After Model: {Modelto}\n" +
                            $"📅 Changeover Date: {COD}\n" +
                            $"⏱️ **Actual Completion Date:** {currentDate}\n\n" +
                            $"📩 Please review and confirm the above details.\n" +
                            $"🙏 Thank you."
                        );



                        ismanpAlertSent = true;
                    }
                    if (Machinerdb1.Checked && isMachinerProcessed && !isMachinerAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "MNT",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"🛠 Machine Inspection Before Model: {Modelfrom}\n" +
                            $"✅ Machine Inspection After Model: {Modelto}\n" +
                            $"📅 Changeover Date: {COD}\n" +
                            $"⏱️ Actual Completion Date: {currentDate}\n\n" +
                            $"⚠️ Please review the inspection details and take necessary action.\n" +
                            $"🙏 Thank you."
                        );



                        isMachinerAlertSent = true;
                    }
                    if (CTPQrdb1.Checked && isCTPQrProcessed && !isCTPQrAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "QIP",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"📊 CTPQ/CTPO Before Model: {Modelfrom}\n" +
                            $"✅ CTPQ/CTPO After Model: {Modelto}\n" +
                            $"📅*Changeover Date: {COD}\n" +
                            $"⏱️ **Actual Completion Date:** {currentDate}\n\n" +
                            $"⚠️ Please review the CTPQ/CTPO data and take necessary action.\n" +
                            $"🙏 Thank you."
                        );


                        isCTPQrAlertSent = true;
                    }
                    if (materialrdb1.Checked && ismaterialrProcessed && !ismaterialrAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "AL",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"📦 Material Lausting Before Model: {Modelfrom}\n" +
                            $"✅ Material Lausting After Model: {Modelto}\n" +
                            $"📅 Changeover Date: {COD}\n" +
                            $"⏱️ Actual Completion Date: {currentDate}\n\n" +
                            $"⚠️ Please check material lausting status and take necessary action.\n" +
                            $"🙏 Thank you."
                        );



                        ismaterialrAlertSent = true;
                    }
                    if (Gaugerbn1.Checked && isGaugerbProcessed && !isGaugerbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");


                        Sendnotification(
                            "AL",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"📏 Gauge Marking Before Model: {Modelfrom}\n" +
                            $"✅ Gauge Marking After Model: {Modelto}\n" +
                            $"📅 Changeover Date:* {COD}\n" +
                            $"⏱️ **Actual Completion Date:** {currentDate}\n\n" +
                            $"⚠️ Please verify gauge marking details and take necessary action.\n" +
                            $"🙏 Thank you."
                        );


                        isGaugerbAlertSent = true;
                    }
                    if (Sampleshoerbn1.Checked && isSampleshoerProcessed && !isSampleshoerAlertSent) 
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "AL",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"👟 Sample Shoe/SOP Before Model: {Modelfrom}\n" +
                            $"✅ Sample Shoe/SOP After Model: {Modelto}\n" +
                            $"📅 Changeover Date: {COD}\n" +
                            $"⏱️ **Actual Completion Date:** {currentDate}\n\n" +
                            $"⚠️ Please check Sample Shoe/SOP details and take necessary action.\n" +
                            $"🙏 Thank you."
                        );



                        isSampleshoerAlertSent = true;
                    }
                    if (cementrbd1.Checked && iscementrbProcessed && !iscementrbAlertSent)
                    {
                        string currentDate = DateTime.Now.ToString("yyyy/MM/dd");

                        Sendnotification(
                            "AL",
                            $"{line}",
                            $"👋 **Hello User!**\n" +
                            $"📢 This alert is from *QCO SYSTEM*\n\n" +
                            $"🏭 Production Line: {line}\n" +
                            $"🧱 Cement Primer Before Model: {Modelfrom}\n" +
                            $"✅ Cement Primer After Model: {Modelto}\n" +
                            $"📅 Changeover Date: {COD}\n" +
                            $"⏱️ **Actual Completion Date:** {currentDate}\n\n" +
                            $"⚠️ Please verify Cement Primer details and take necessary action.\n" +
                            $"🙏 Thank you."
                        );



                        iscementrbAlertSent = true;
                    }

                    NotificationsSent = true;
                }

                // Save data to the database
                Dictionary<string, object> data = CollectData();
                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "AChecklist",
                     Program.Client.UserToken, JsonConvert.SerializeObject(data));

                ProcessResponse(ret);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during saving: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;  // ⬅️ Set cursor back to normal
            }

        }

        private bool ValidateRequiredFields()
        {
            if (BMCOtxt.Text == "" && COdatetxt.Text == "")
            {
                MessageHelper.ShowErr(this, "Before submit, Please Enter Model name");
                return false;
            }

            ValidateAndShowError(Linelayoutrbt2.Checked, Linelayouttxt.Text, "Please Mention the Remark");
            ValidateAndShowError(valuerdb2.Checked, valuestretxt.Text, "Please Mention the Remark");
            ValidateAndShowError(Manprdb2.Checked, manpotxt.Text, "Please Mention the Remark");
            ValidateAndShowError(Machinerdb2.Checked, machineinstxt.Text, "Please Mention the Remark");
            ValidateAndShowError(CTPQrdb2.Checked, ctpqtxt.Text, "Please Mention the Remark");
            ValidateAndShowError(materialrdb2.Checked, materiallasttxt.Text, "Please Mention the Remark");
            ValidateAndShowError(Gaugerbn2.Checked, gaugetxt.Text, "Please Mention the Remark");
            ValidateAndShowError(Sampleshoerbn2.Checked, sampleshoetxt.Text, "Please Mention the Remark");
            ValidateAndShowError(cementrbd2.Checked, cementtxt.Text, "Please Mention the Remark");

            ValidateSelection(Linelayoutrbt1.Checked || Linelayoutrbt2.Checked, "Line Layout");
            ValidateSelection(valuerdb1.Checked || valuerdb2.Checked, "Value stream");
            ValidateSelection(Manprdb1.Checked || Manprdb2.Checked, "Manpower");
            ValidateSelection(Machinerdb1.Checked || Machinerdb2.Checked, "Machine");
            ValidateSelection(CTPQrdb1.Checked || CTPQrdb2.Checked, "CTPQ");
            ValidateSelection(materialrdb1.Checked || materialrdb2.Checked, "Material");
            ValidateSelection(Gaugerbn1.Checked || Gaugerbn2.Checked, "Gauge");
            ValidateSelection(Sampleshoerbn1.Checked || Sampleshoerbn2.Checked, "Sampleshoe");
            ValidateSelection(cementrbd1.Checked || cementrbd2.Checked, "cement");

            return true;
        }

        private void ValidateAndShowError(bool isChecked, string text, string errorMsg)
        {
            if (isChecked && string.IsNullOrEmpty(text))
            {
                MessageHelper.ShowErr(this, errorMsg);              
            }
        }
        private void ValidateSelection(bool isAnySelected, string fieldName)
        {
           

            if (!isAnySelected)
            {
                //string msg = $"Please select at least one({fieldName})";
              //  MessageHelper.ShowErr(this, msg);
              
            }
        }
        private byte[] PictureBoxToByteArray(params PictureBox[] pictureBoxes)
        {
            foreach (PictureBox pictureBox in pictureBoxes)
            {
                if (pictureBox.Image == null)
                {                 
                    //if ("pictureBox1" == pictureBox.Name)
                    //{                       
                    //    MessageBox.Show($"Please Upload Section Head Signature", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else  if ("pictureBox2" == pictureBox.Name)
                    //{                     
                    //    MessageBox.Show($"Please Upload PC Signature", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else if ("pictureBox3" == pictureBox.Name)
                    //{
                    //    MessageBox.Show($"Please Upload Technical Signature", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    //else if ("pictureBox4" == pictureBox.Name)
                    //{
                    //    MessageBox.Show($"Please Upload Maintenance Signature", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}
                    return null;
                }
            }   
            using (MemoryStream memoryStream = new MemoryStream())
            {              
                Bitmap bitmap = new Bitmap(pictureBoxes[0].Image);
                // Save Bitmap to MemoryStream in a specific format (e.g., PNG)
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); 
                // Convert MemoryStream to byte array
                return memoryStream.ToArray();
            }
        }

        private Dictionary<string, object> CollectData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                byte[] byteArray1 = PictureBoxToByteArray(pictureBox1);
                byte[] byteArray2 = PictureBoxToByteArray(pictureBox2);
                byte[] byteArray3 = PictureBoxToByteArray(pictureBox3);
                byte[] byteArray4 = PictureBoxToByteArray(pictureBox4);
                data.Add("BModel", BMCOtxt.Text);
                data.Add("AfModel", A_MCOtxt.Text);
                data.Add("COdate", COdatetxt.Text);
                data.Add("Prodline", linetxt.Text);
                data.Add("Linelay", Linelayoutrbt1.Checked ? Linelayoutrbt1.Text : Linelayoutrbt2.Text);
                data.Add("valuer", valuerdb1.Checked ? valuerdb1.Text : valuerdb2.Text);
                data.Add("Manpr", Manprdb1.Checked ? Manprdb1.Text : Manprdb2.Text);
                data.Add("Machine", Machinerdb1.Checked ? Machinerdb1.Text : Machinerdb2.Text);
                data.Add("CTPQr", CTPQrdb1.Checked ? CTPQrdb1.Text : CTPQrdb2.Text);
                data.Add("material", materialrdb1.Checked ? materialrdb1.Text : materialrdb2.Text);
                data.Add("Gauge", Gaugerbn1.Checked ? Gaugerbn1.Text : Gaugerbn2.Text);
                data.Add("Sampleshoe", Sampleshoerbn1.Checked ? Sampleshoerbn1.Text : Sampleshoerbn2.Text);
                data.Add("cement", cementrbd1.Checked ? cementrbd1.Text : cementrbd2.Text);
                data.Add("Remarks", GetALLRemark());
                data.Add("COT", COTtxt.Text);
                data.Add("COPT", COPTtxt.Text);
                data.Add("Problem", probtxt.Text);
                data.Add("QCO_Seq", QCO_Sequtxt.Text);

                data.Add("Progressvalue", progressBar1.Value);

                // Convert PictureBox images to byte arrays and add to data dictionary
                data.Add("SectionHd", byteArray1 != null ? byteArray1 : (object)DBNull.Value);
                data.Add("Plantin", byteArray2 != null ? byteArray2 : (object)DBNull.Value);
                data.Add("Technic", byteArray3 != null ? byteArray3 : (object)DBNull.Value);
                data.Add("Maintenance", byteArray4 != null ? byteArray4 : (object)DBNull.Value);

                data.Add("LIne_Time", line_update_txt.Text);
                data.Add("Value_Time", Value_Update_txt.Text);
                data.Add("Man_time", Man_Update_txt.Text);
                data.Add("Machine_time", Machine_update_txt.Text);
                data.Add("Ctpq_time", Ctpq_Update_txt.Text);
                data.Add("Mold_time", Mold_Update_txt.Text);
                data.Add("Gaus_time", Gaue_Update_txt.Text);
                data.Add("Sop_time", Sop_update_txt.Text);
                data.Add("Cement_time", Cement_update_txt.Text);

                data.Add("A_S_Plan", aplancombo.SelectedItem.ToString());

                data.Add("ACCOD", AcCODdateTimePicker.Value.ToString("yyyy/MM/dd"));
                if (acotypecombo.SelectedItem != null)
                {
                    data.Add("Acotype", acotypecombo.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("No item selected in Changeover Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null; // Exit the method
                }
                if (aplancombo.SelectedItem != null)
                {
                    string selectedPlan = aplancombo.SelectedItem.ToString();

                    if (selectedPlan == "Scheduled")
                    {
                        data.Add("Sdelayres", "Scheduled"); 
                    }
                    else if (selectedPlan == "Unplanned_Changeover")
                    {
                        if (Adelayrecombo.SelectedItem != null) 
                        {
                            data.Add("Sdelayres", Adelayrecombo.SelectedItem.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Please select a Delay reason for Unplanned Changeover.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show("No valid item selected in Plan Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       
                    }
                }
                else
                {
                    MessageBox.Show("No item selected in Plan Type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                data.Add("Adelayres", Adelayrecombo.Text);

                data.Add("", Mold_Update_txt.Text);
                data.Add("RUT", textramup.Text);
                data.Add("cottime", cottimelbl.Text);
                data.Add("Copttime", Copttimelbl.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while collecting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return data;
        }
     
        private DataTable GetALLRemark()
        {
            // Create a new DataTable
            DataTable dt = new DataTable();

            // Define the columns
            dt.Columns.Add("LineRemark", typeof(string));
            dt.Columns.Add("ValueRemark", typeof(string));
            dt.Columns.Add("ManRemark", typeof(string));
            dt.Columns.Add("MachineRemark", typeof(string));
            dt.Columns.Add("CTPQRemark", typeof(string));
            dt.Columns.Add("MaterialLastRemark", typeof(string));
            dt.Columns.Add("GaugeRemark", typeof(string));
            dt.Columns.Add("SampleShoeRemark", typeof(string));
            dt.Columns.Add("CementRemark", typeof(string));

            // Create a new row
            DataRow row = dt.NewRow();

            // Get values from text boxes
            string Line_rem = Linelayouttxt.Text;
            string Value_rem = valuestretxt.Text;
            string Man_rem = manpotxt.Text;
            string Machine_rem = machineinstxt.Text;
            string Ctpq_rem = ctpqtxt.Text;
            string Material_lau_rem = materiallasttxt.Text;
            string Gueage_rem = gaugetxt.Text;
            string sample_rem = sampleshoetxt.Text;
            string Cement_rem = cementtxt.Text;

            // Assign values to the row
            row["LineRemark"] = Line_rem;
            row["ValueRemark"] = Value_rem;
            row["ManRemark"] = Man_rem;
            row["MachineRemark"] = Machine_rem;
            row["CTPQRemark"] = Ctpq_rem;
            row["MaterialLastRemark"] = Material_lau_rem;
            row["GaugeRemark"] = Gueage_rem;
            row["SampleShoeRemark"] = sample_rem;
            row["CementRemark"] = Cement_rem;

            // Add the row to the DataTable
            dt.Rows.Add(row);

            // Return the populated DataTable
            return dt;
        }


        private void ProcessResponse(string ret)
        {
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();

                if (json == "1")
                {
                    SJeMES_Control_Library.MessageHelper.ShowSuccess(this, "Data inserted Successfully");
                }
                else if(json == "2")
                {
                    MessageHelper.ShowSuccess(this, "Updated Successfully");
                }
                else
                {
                    MessageHelper.ShowErr(this, " No data Updated");
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
        }

        private void ClearFields()
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = "";
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                }
            }
        }

       



    }
}
