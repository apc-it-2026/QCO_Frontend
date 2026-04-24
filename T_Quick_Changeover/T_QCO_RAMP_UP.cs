using SJeMES_Control_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace T_Quick_Changeover
{
    public partial class T_QCO_RAMP_UP : Form
    {
        TimeSpan totalDuration;
        int Aline;
        int totalHours;
        string DEPT;
        string Plant;



        public T_QCO_RAMP_UP(int A,string dept, string rampvalue)
        {
            InitializeComponent();
            txtNewModelFirstPair.Text = rampvalue;
            Aline = A;
            DEPT = dept;
            string line = ExtractAPValue(DEPT);
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
     
        private void Calculate_Ramp_Upbtn_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime initialProduction = DateTime.Parse(txtNewModelFirstPair.Text);
                DateTime targetProduction = DateTime.Parse(txttargetreach.Text);

                // Working hours
                TimeSpan workStart = new TimeSpan(8, 30, 0);
                TimeSpan workEnd = new TimeSpan(19, 15, 0);
                TimeSpan dailyWorkHours = workEnd - workStart;

                // Plant-wise lunch time
                TimeSpan lunchStart = TimeSpan.Zero, lunchEnd = TimeSpan.Zero;
                if (Plant == "SPC1" || Plant == "RR")
                {
                    lunchStart = new TimeSpan(11, 45, 0);
                    lunchEnd = new TimeSpan(12, 30, 0);
                }
                else if (Plant == "AP9" || Plant == "AP10" || Plant == "AP11")
                {
                    lunchStart = new TimeSpan(12, 0, 0);
                    lunchEnd = new TimeSpan(12, 30, 0);
                }
                else if (Plant == "AP3" || Plant == "AP12" || Plant == "API" || Plant == "AP5")
                {
                    lunchStart = new TimeSpan(12, 30, 0);
                    lunchEnd = new TimeSpan(13, 15, 0);
                }
                else if (Plant == "AP1" || Plant == "AP2" || Plant == "AP6" || Plant == "AP7" || Plant == "AP8")
                {
                    lunchStart = new TimeSpan(13, 15, 0);
                    lunchEnd = new TimeSpan(14, 0, 0);
                }

                TimeSpan lunchDuration = lunchEnd - lunchStart;
                TimeSpan ruttime = TimeSpan.Zero;

                if (initialProduction.Date == targetProduction.Date)
                {
                    // Same-day case
                    TimeSpan actualDuration = targetProduction - initialProduction;
                    DateTime lunchStartTime = initialProduction.Date.Add(lunchStart);
                    DateTime lunchEndTime = initialProduction.Date.Add(lunchEnd);

                    if (initialProduction < lunchEndTime && targetProduction > lunchStartTime)
                    {
                        // Overlaps lunch
                        TimeSpan overlap =
                            (targetProduction < lunchEndTime ? targetProduction : lunchEndTime) -
                            (initialProduction > lunchStartTime ? initialProduction : lunchStartTime);
                        actualDuration -= overlap;
                    }

                    ruttime = actualDuration;
                }
                else
                {
                    // First day (partial)
                    DateTime dayEnd = initialProduction.Date.Add(workEnd);
                    TimeSpan firstDayDuration = dayEnd - initialProduction;
                    DateTime lunchStartTime = initialProduction.Date.Add(lunchStart);
                    DateTime lunchEndTime = initialProduction.Date.Add(lunchEnd);

                    if (initialProduction < lunchEndTime && dayEnd > lunchStartTime)
                    {
                        TimeSpan overlap =
                            (dayEnd < lunchEndTime ? dayEnd : lunchEndTime) -
                            (initialProduction > lunchStartTime ? initialProduction : lunchStartTime);
                        firstDayDuration -= overlap;
                    }
                    ruttime += firstDayDuration;

                    // Days in between
                    DateTime currentDay = initialProduction.Date.AddDays(1);
                    while (currentDay < targetProduction.Date)
                    {
                        ruttime += (dailyWorkHours - lunchDuration); // full day excluding lunch
                        currentDay = currentDay.AddDays(1);
                    }

                    // Last day (partial)
                    DateTime lastDayStart = targetProduction.Date.Add(workStart);
                    TimeSpan lastDayDuration = targetProduction - lastDayStart;

                    DateTime lastLunchStart = targetProduction.Date.Add(lunchStart);
                    DateTime lastLunchEnd = targetProduction.Date.Add(lunchEnd);

                    if (lastDayStart < lastLunchEnd && targetProduction > lastLunchStart)
                    {
                        TimeSpan overlap =
                            (targetProduction < lastLunchEnd ? targetProduction : lastLunchEnd) -
                            (lastDayStart > lastLunchStart ? lastDayStart : lastLunchStart);
                        lastDayDuration -= overlap;
                    }
                    ruttime += lastDayDuration;
                }

                 totalHours = (int)ruttime.TotalHours;
                int remainingMinutes = ruttime.Minutes;

                lblResult.Text = $"Ramp-Up Time: {totalHours} hours and {remainingMinutes} minutes";
                MessageHelper.ShowSuccess(this, $"Ramp-Up Time: {totalHours} hours and {remainingMinutes} minutes");
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid DateTime values (e.g., 2024/12/14 14:30:00).",
                                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void startTimeProduct2_KeyDown(object sender, KeyEventArgs e)
        {
            targettimepicker.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            string target = targettimepicker.Value.ToString();
            //DateTime start1 = targettimepicker.Value;
            //string sdate = start1.ToString("yyyy/MM/dd");

            txttargetreach.Text = target;
        }

        private void Submitbtn_Click(object sender, EventArgs e)
        {
            QCO_Prop.Param = "Checklist";
            QCO_Prop.RUT = totalHours;
            

            int RUTValue = QCO_Prop.RUT;
          

            this.Hide();
            T_QCO_Checklist2 existingForm = Application.OpenForms.OfType<T_QCO_Checklist2>().FirstOrDefault();

            if (existingForm != null && Aline == 1)
            {
                existingForm.GetAssemblyRUT(RUTValue);
                existingForm.Focus();
            }
            else
            {
                existingForm.GetstitchRUT(RUTValue);
                existingForm.Show();
            }

        }
      
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = panel2.ClientRectangle;

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
