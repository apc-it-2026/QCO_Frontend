using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class T_QCO_COT_COPT : Form
    {
        string Newmodel;
        int Sline;
        string EndofTime;
        private T_QCO_Checklist2 RUT;
        string Timeport;
        string copttimepor;
        string DEPT;
        string Plant;
        TimeSpan cot;
        double totalMinutes;



        public T_QCO_COT_COPT(int S,string dept, T_QCO_Checklist2 rut)
        {
            InitializeComponent();
            Sline = S;
            RUT = rut;
            DEPT = dept;

         string line=ExtractAPValue(DEPT);
            Plant = line;
        }
        static string ExtractAPValue(string DEPT)
        {
            int start = DEPT.IndexOf("AP");
            if (start == -1) return "No match found!";

            // Find first occurrence of 'S' or 'L' after "AP"
            int endS = DEPT.IndexOf("S", start);
            int endL = DEPT.IndexOf("L", start);

            // Determine the correct stopping position
            int end;
            if (endS != -1 && endL != -1)
                end = Math.Min(endS, endL); 
            else if (endS != -1)
                end = endS;
            else if (endL != -1)
                end = endL;
            else
                return DEPT.Substring(start); // If no 'S' or 'L', take till end

            return DEPT.Substring(start, end - start);
        }
  
        private void Calculatebtn_Click(object sender, EventArgs e)
        {
            if (EndofdayTimePicker.Visible)
            {

                DateTime endtime1 = DateTime.Parse(endTimeP1txt.Text);
                DateTime starttime1 = DateTime.Parse(startTimeP2txt.Text);

                DateTime Endodtime = DateTime.Parse(Endofdattxt.Text);
                TimeSpan CoT1 = Endodtime - endtime1;

               
               
                DateTime specificDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 30, 0);
                TimeSpan COT2 = starttime1 - specificDateTime;
                cot = CoT1 + COT2;
            }
            else if(Plant != null)
            {
                DateTime endtime1 = DateTime.Parse(endTimeP1txt.Text);
                DateTime starttime1 = DateTime.Parse(startTimeP2txt.Text); 

                    if (starttime1 < endtime1)
                    {
                        MessageHelper.ShowErr(this, "Old Model Last pair must be Less than New Model First pair");
                        Resultcottxt.Text = "";
                        return;
                    }

                    // Define lunch break times based on plant type
                    TimeSpan lunchStart = TimeSpan.Zero;
                    TimeSpan lunchEnd = TimeSpan.Zero;

                    if (Plant == "SPC1" || Plant == "RR")
                    {
                        lunchStart = new TimeSpan(11, 45, 0); // 11:45 AM
                        lunchEnd = new TimeSpan(12, 30, 0);   // 12:30 PM
                    }
                    else if (Plant == "AP9" || Plant == "AP10" || Plant == "AP11")
                    {
                        lunchStart = new TimeSpan(12, 0, 0); // 12:00 PM
                        lunchEnd = new TimeSpan(12, 30, 0);   // 12:30 PM
                    }
                    else if (Plant == "AP3" || Plant == "AP12" || Plant == "API" || Plant == "AP5")
                    {
                        lunchStart = new TimeSpan(12, 30, 0); // 12:30 PM
                        lunchEnd = new TimeSpan(13, 15, 0);   // 1:15 PM
                    }
                    else if (Plant == "AP1" || Plant == "AP2" || Plant == "AP6" || Plant == "AP7" || Plant == "AP8")
                    {
                        lunchStart = new TimeSpan(13, 15, 0); // 1:15 PM
                        lunchEnd = new TimeSpan(14, 0, 0);    // 2:00 PM
                    }

                    TimeSpan startTimeSpan = starttime1.TimeOfDay;
                    TimeSpan endTimeSpan = endtime1.TimeOfDay;

                    totalMinutes = (starttime1 - endtime1).TotalMinutes;
                    if (startTimeSpan < lunchEnd && endTimeSpan > lunchStart)
                    {
                        totalMinutes -= 45;
                    }
                    Resultcottxt.Text = totalMinutes.ToString();
            }
            else
            {
                if (endTimeP1txt.Text == "")
                {
                    MessageHelper.ShowErr(this, " Please Enter Old Model Last Pair Time!");
                    return;
                }
                if (startTimeP2txt.Text == "")
                {
                    MessageHelper.ShowErr(this, " Please Enter New Model First Pair Time!");
                    return;
                }
                DateTime endtime = DateTime.Parse(endTimeP1txt.Text);
                DateTime starttime = DateTime.Parse(startTimeP2txt.Text);
                cot = starttime - endtime;
                if ((endtime.TimeOfDay.Hours <= 12 && starttime.TimeOfDay.Hours >= 13) || (endtime.TimeOfDay.Hours <= 12 && starttime.TimeOfDay.Hours >= 14) || (endtime.TimeOfDay.Hours <= 13 && starttime.TimeOfDay.Hours >= 14))
                {
                    // Lunch break from 12:30 to 1:15
                    if (endtime.TimeOfDay <= new TimeSpan(12, 30, 0) && starttime.TimeOfDay >= new TimeSpan(13, 15, 0))
                    {
                        cot = cot.Subtract(new TimeSpan(0, 45, 0));
                    }                 
                    else
                    {
                        cot = cot.Subtract(new TimeSpan(0, 45, 0));
                    }

                }
            }
                double cotMinutes = Math.Round(cot.TotalMinutes,2);             
                double absoluteValue = Math.Abs(cotMinutes);
            //Resultcottxt.Text = absoluteValue.ToString();
            Resultcottxt.Text = totalMinutes.ToString();
            MessageHelper.ShowSuccess(this, "Changeover Time (COT): " + Resultcottxt.Text + " minutes");
                Newmodel = startTimeP2txt.Text;
                textBox2.Text = Newmodel;

            string endTimeText = endTimeP1txt.Text.Trim();
            string startTimeText = startTimeP2txt.Text.Trim();

            // Debugging: Check what input is received
            Console.WriteLine("Raw End Time Input: " + endTimeText);
            Console.WriteLine("Raw Start Time Input: " + startTimeText);

            if (string.IsNullOrEmpty(endTimeText) || string.IsNullOrEmpty(startTimeText))
            {
                throw new Exception("Error: One or both time fields are empty.");
            }

            // Try parsing as full DateTime first
            if (!DateTime.TryParse(endTimeText, out DateTime endTime))
            {
                throw new Exception($"Invalid time format for End Time: {endTimeText}. Unable to parse.");
            }

            if (!DateTime.TryParse(startTimeText, out DateTime startTime))
            {
                throw new Exception($"Invalid time format for Start Time: {startTimeText}. Unable to parse.");
            }

            // Extract only the time and convert to 12-hour format
            string endTimeOnly = endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            string startTimeOnly = startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);

            Timeport = endTimeOnly + " -- " + startTimeOnly;

        }
        private void COPTbtn_Click(object sender, EventArgs e)
        {          
            DateTime starttime1 = DateTime.Parse(textBox1.Text);
            DateTime endtime = DateTime.Parse(textBox2.Text);        
            string endTimeText = textBox2.Text.Trim();
            string startTimeText = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(endTimeText) || string.IsNullOrEmpty(startTimeText))
            {
                throw new Exception("Error: One or both time fields are empty.");
            }
            if (!DateTime.TryParse(endTimeText, out DateTime endTime))
            {
                throw new Exception($"Invalid time format for End Time: {endTimeText}. Unable to parse.");
            }

            if (!DateTime.TryParse(startTimeText, out DateTime startTime))
            {
                throw new Exception($"Invalid time format for Start Time: {startTimeText}. Unable to parse.");
            }

            // Extract only the time and convert to 12-hour format
            string endTimeOnly = endTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            string StartTimeOnly = startTime.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            if (endTime <= startTime)
            {
                MessageHelper.ShowErr(this, "New Model Input time must be Less than New Model First pair");
                COPTtext.Text = "";
                return;
            }
            TimeSpan lunchStart = TimeSpan.Zero;
            TimeSpan lunchEnd = TimeSpan.Zero;

            if (Plant == "SPC1" || Plant == "RR")
            {
                lunchStart = new TimeSpan(11, 45, 0); // 11:45 AM
                lunchEnd = new TimeSpan(12, 30, 0);   // 12:30 PM
            }
            else if (Plant == "AP9" || Plant == "AP10" || Plant == "AP11")
            {
                lunchStart = new TimeSpan(12, 0, 0); // 12:00 PM
                lunchEnd = new TimeSpan(12, 30, 0);   // 12:30 PM
            }
            else if (Plant == "AP3" || Plant == "AP12" || Plant == "API" || Plant == "AP5")
            {
                lunchStart = new TimeSpan(12, 30, 0); // 12:30 PM
                lunchEnd = new TimeSpan(13, 15, 0);   // 1:15 PM
            }
            else if (Plant == "AP1" || Plant == "AP2" || Plant == "AP6" || Plant == "AP7" || Plant == "AP8")
            {
                lunchStart = new TimeSpan(13, 15, 0); // 1:15 PM
                lunchEnd = new TimeSpan(14, 0, 0);    // 2:00 PM
            }

            TimeSpan startTimeSpan = startTime.TimeOfDay;
            TimeSpan endTimeSpan = endTime.TimeOfDay;

            double totalMinutes = (endTime - startTime).TotalMinutes;

            // Subtract lunch break only if the work period overlaps, and only once
            if (startTimeSpan < lunchEnd && endTimeSpan > lunchStart)
            {
                totalMinutes -= 45;
            }
            COPTtext.Text = totalMinutes.ToString();
            MessageHelper.ShowSuccess(this,"Changeover Time(COT): " + COPTtext.Text + " minutes");
            copttimepor = StartTimeOnly + " -- " + endTimeOnly;
        }

        private void Clickhourbtn_Click(object sender, EventArgs e)
        {
            double min = double.Parse(Resultcottxt.Text);
            double Hour = min / 60;
            string Hr = Hour + " Hours";
            Hourstxt.Text = Hr.ToString();
        }
        string Edate;
        DateTime endTime1;
        private void endTimeProduct1_KeyDown(object sender, KeyEventArgs e)
        {
            startTimeProduct2.MinDate = endTimeProduct1.Value;
            endTimeProduct1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            string endTime = endTimeProduct1.Value.ToString();
             endTime1 = endTimeProduct1.Value;
            string edate = endTime1.ToString("yyyy/MM/dd");
            Edate = edate;
            endTimeP1txt.Text = endTime;
        }

        private void startTimeProduct2_KeyDown(object sender, KeyEventArgs e)
        {
            startTimeProduct2.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            string start = startTimeProduct2.Value.ToString();
            DateTime start1 = startTimeProduct2.Value;
            string sdate = start1.ToString("yyyy/MM/dd");
            if(start1 <= endTime1)
            {
                startTimeP2txt.Text = start;
                label4.Visible = false;
                EndofdayTimePicker.Visible = false;
                Endofdattxt.Visible = false;
            }
            else
            {
                label4.Visible = true;
                EndofdayTimePicker.Visible = true;
                Endofdattxt.Visible = true;

            }
            startTimeP2txt.Text = start;
        }

        private void Submittxt_Click(object sender, EventArgs e)
        {
            QCO_Prop.Param = "Checklist";
            QCO_Prop.COT = Resultcottxt.Text;
            QCO_Prop.COPT = COPTtext.Text;
            QCO_Prop.Cottime = Timeport;
            QCO_Prop.Copttime = copttimepor;


            string cotValue = QCO_Prop.COT;
            string coptValue = QCO_Prop.COPT;
            string cotTime = QCO_Prop.Cottime;
            string copttime = QCO_Prop.Copttime;


            this.Hide();
            T_QCO_Checklist2 existingForm = Application.OpenForms.OfType<T_QCO_Checklist2>().FirstOrDefault();

            if (existingForm != null && Sline == 1)
            {
                existingForm.GetInstance(cotValue, coptValue, cotTime, copttime);
                existingForm.Focus();
            }
            else
            {
                existingForm.GetInstance1(cotValue, coptValue, cotTime, copttime);
                existingForm.Show();
            }

            try
            {
                if (RUT?.Calculate_RUT != null && RUT?.RUT != null)
                {
                    RUT.Calculate_RUT.Visible = true;
                    RUT.RUT.Visible = true;
                }
                else
                {
                    MessageBox.Show("RUT or its controls are not initialized.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

            if (this.Owner is T_QCO_Checklist2 ownerForm)
            {
                ownerForm.SetTextBoxValue(startTimeP2txt.Text);
            }
            
            this.Close();
        }
       

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            string start = dateTimePicker1.Value.ToString();
            textBox1.Text = start;
        }

        private void T_QCO_COT_COPT_Load(object sender, EventArgs e)
        {
          
            label4.Visible = false;
            EndofdayTimePicker.Visible = false;
            Endofdattxt.Visible = false;
           
        }

        private void EndofdayTimePicker_KeyPress(object sender, KeyPressEventArgs e)
        {         
            EndofdayTimePicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            string End = EndofdayTimePicker.Value.ToString();
            Endofdattxt.Text = End;           
        }

       
    }
}
 