using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
   public static class FastReportHelper
    {
        public static void LoadFastReport(Control ctr, string fileName,  DataTable dt)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("Report file not found：" + fileName, "Report prompts");
                    return;
                }

                ctr.Controls.Clear();
                FastReport.Report report = new FastReport.Report();
                FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();

                previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
                ctr.Controls.Add(previewControl);
                report.Preview = previewControl;

                report.Load(fileName);

                // Ensure DataTable has rows and required columns
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count >= 11)
                {
                    string Bmodel = dt.Rows[0][0].ToString();
                    string AModel = dt.Rows[0][1].ToString();
                    string Line = dt.Rows[0][2].ToString();
                    string COD = dt.Rows[0][3].ToString();                  
                    string Date = dt.Rows[0][4].ToString();                 
                    string article = dt.Rows[0][5].ToString();
                    string sop = dt.Rows[0][6].ToString();
                    string bct = dt.Rows[0][7].ToString();
                    string reason = dt.Rows[0][8].ToString();
                    string joint = dt.Rows[0][9].ToString();
                    string sopsig = dt.Rows[0][10].ToString();
                    string sup = dt.Rows[0][11].ToString();


                    var textObject = report.FindObject("Bmodel1") as FastReport.TextObject;
                    var textObject1 = report.FindObject("Amodel1") as FastReport.TextObject;
                    var textObject2 = report.FindObject("Line1") as FastReport.TextObject;
                    var textObject3 = report.FindObject("COdate1") as FastReport.TextObject;                  
                    var textObject4 = report.FindObject("frdate") as FastReport.TextObject;                
                    var textObject5 = report.FindObject("frarticle") as FastReport.TextObject;
                    var textObject6 = report.FindObject("frsop1") as FastReport.TextObject;
                    var textObject7 = report.FindObject("frbc") as FastReport.TextObject;
                    var textObject8 = report.FindObject("frreason") as FastReport.TextObject;
                    var textObject9 = report.FindObject("frjoint") as FastReport.TextObject;
                    var textObject10 = report.FindObject("frsopsign") as FastReport.TextObject;
                    var textObject11 = report.FindObject("frsup") as FastReport.TextObject;


                    if (textObject != null) textObject.Text = Bmodel;
                    if (textObject1 != null) textObject1.Text = AModel;
                    if (textObject2 != null) textObject2.Text = Line;
                    if (textObject3 != null) textObject3.Text = COD;
                    if (textObject4 != null) textObject4.Text = Date;                 
                    if (textObject5 != null) textObject5.Text = article;
                    if (textObject6 != null) textObject6.Text = sop;
                    if (textObject7 != null) textObject7.Text = bct;
                    if (textObject8 != null) textObject8.Text = reason;
                    if (textObject9 != null) textObject9.Text = joint;
                    if (textObject10 != null) textObject10.Text = sopsig;
                    if (textObject11 != null) textObject11.Text = sup;
     
                }

                // Prepare and show the report
                report.Prepare();
                report.ShowPrepared(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading FastReport: " + ex.Message);
            }
        }
        public static void LoadFastReport1(Control ctr, string fileName, Dictionary<string, string> dicParameter, DataTable dt, string tablename)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("Report file not found：" + fileName, "Report prompts");
                    return;
                }

                ctr.Controls.Clear();
                FastReport.Report report = new FastReport.Report();
                FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();

                previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
                ctr.Controls.Add(previewControl);
                report.Preview = previewControl;

                report.Load(fileName);

                // Ensure DataTable has rows and required columns
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count >= 11)
                {
                    string Bmodel = dt.Rows[0][0].ToString();
                    string Amodel = dt.Rows[0][1].ToString();
                    string line = dt.Rows[0][2].ToString();
                    string Codate = dt.Rows[0][3].ToString();
                    string Date = dt.Rows[0][4].ToString();
                    string Art = dt.Rows[0][5].ToString();              
                    string mcs = dt.Rows[0][6].ToString();
                    string supsig = dt.Rows[0][7].ToString();
                    string planaut = dt.Rows[0][8].ToString();
                    string reason = dt.Rows[0][9].ToString();
                    string plantsig = dt.Rows[0][10].ToString();

                    var textObject = report.FindObject("Bmodel") as FastReport.TextObject;
                    var textObject1 = report.FindObject("afmodel") as FastReport.TextObject;
                    var textObject2 = report.FindObject("prdline") as FastReport.TextObject;
                    var textObject3 = report.FindObject("Codate1") as FastReport.TextObject;
                    var textObject4 = report.FindObject("frdate") as FastReport.TextObject;                   
                    var textObject5 = report.FindObject("frart") as FastReport.TextObject;                  
                    var textObject6 = report.FindObject("frmcs") as FastReport.TextObject;
                    var textObject7 = report.FindObject("frsupsig") as FastReport.TextObject;
                    var textObject8 = report.FindObject("frplanaut") as FastReport.TextObject;
                    var textObject9 = report.FindObject("frplantsig") as FastReport.TextObject;
                    var textObject10 = report.FindObject("frreason") as FastReport.TextObject;

                    if (textObject != null) textObject.Text = Bmodel;
                    if (textObject1 != null) textObject1.Text = Amodel;
                    if (textObject2 != null) textObject2.Text = line;
                    if (textObject3 != null) textObject3.Text = Codate;
                    if (textObject4 != null) textObject4.Text = Date;
                    if (textObject5 != null) textObject5.Text = Art;
                    if (textObject6 != null) textObject6.Text = mcs;
                    if (textObject7 != null) textObject7.Text = supsig;
                    if (textObject8 != null) textObject8.Text = planaut;
                    if (textObject9 != null) textObject9.Text = reason;
                    if (textObject10 != null) textObject10.Text = plantsig;
                }

                // Prepare and show the report
                report.Prepare();
                report.ShowPrepared(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading FastReport: " + ex.Message);
            }
        }
        public static void LoadFastReport3(Control ctr, string fileName, DataTable dt)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("Report file not found：" + fileName, "Report prompts");
                    return;
                }

                ctr.Controls.Clear();
                FastReport.Report report = new FastReport.Report();
                FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl();

                previewControl.Dock = System.Windows.Forms.DockStyle.Fill;
                ctr.Controls.Add(previewControl);
                report.Preview = previewControl;

                report.Load(fileName);

                // Ensure DataTable has rows and required columns
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count >= 20)
                {
                    string Bmodel     = dt.Rows[0][0].ToString();
                    string AModel     = dt.Rows[0][1].ToString();
                    string Line       = dt.Rows[0][2].ToString();
                    string COD        = dt.Rows[0][3].ToString();
                    string Applicant  = dt.Rows[0][4].ToString();
                    string Prodept    = dt.Rows[0][5].ToString();
                    string Equipment  = dt.Rows[0][6].ToString();
                    string Times_of_Demand = dt.Rows[0][7].ToString();
                    string Exixt_Qty       = dt.Rows[0][8].ToString();
                    string Demand_Qty      = dt.Rows[0][9].ToString();
                    string Output_Target   = dt.Rows[0][10].ToString();
                    string IE_standard     = dt.Rows[0][11].ToString();
                    string Tran_dept       = dt.Rows[0][12].ToString();
                    string Remarks         = dt.Rows[0][13].ToString();
                    string Receive_Date    = dt.Rows[0][14].ToString();
                    string Supervisor      = dt.Rows[0][15].ToString();
                    string Section_Head    = dt.Rows[0][16].ToString();
                    string Plantin_chanrge = dt.Rows[0][17].ToString();
                    string Procontrol_Dept = dt.Rows[0][18].ToString();
                    string Controller      = dt.Rows[0][19].ToString();
                  
                    var textObject = report.FindObject("Bmodeltxt") as FastReport.TextObject;
                    var textObject1 = report.FindObject("AModeltxt") as FastReport.TextObject;
                    var textObject2 = report.FindObject("Depttxt") as FastReport.TextObject;
                    var textObject3 = report.FindObject("COdate") as FastReport.TextObject;
                    var textObject4 = report.FindObject("applicanttxt") as FastReport.TextObject;
                    var textObject5 = report.FindObject("PCtxt") as FastReport.TextObject;
                    var textObject6 = report.FindObject("Equipmenttxt") as FastReport.TextObject;
                    var textObject7 = report.FindObject("TimeofDemandtxt") as FastReport.TextObject;
                    var textObject8 = report.FindObject("ExistingQtytxt") as FastReport.TextObject;
                    var textObject9 = report.FindObject("DemandQtytxt") as FastReport.TextObject;
                    var textObject10 = report.FindObject("OutputTargettxt") as FastReport.TextObject;
                    var textObject11 = report.FindObject("IEStandardtxt") as FastReport.TextObject;
                    var textObject12 = report.FindObject("TransferDepttxt") as FastReport.TextObject;
                    var textObject13 = report.FindObject("Remarkstxt") as FastReport.TextObject;
                    var textObject14 = report.FindObject("ReceiveDatetxt") as FastReport.TextObject;
                    var textObject15 = report.FindObject("Supervisortxt") as FastReport.TextObject;
                    var textObject16 = report.FindObject("SectionHeadtxt") as FastReport.TextObject;
                    var textObject17 = report.FindObject("Plantinchanrgetxt") as FastReport.TextObject;
                    var textObject18 = report.FindObject("Controldepttext") as FastReport.TextObject;
                    var textObject19 = report.FindObject("Controllertxt") as FastReport.TextObject;
      

                    if (textObject != null) textObject.Text   = Bmodel;
                    if (textObject1 != null) textObject1.Text = AModel;
                    if (textObject2 != null) textObject2.Text = Line;
                    if (textObject3 != null) textObject3.Text = COD;
                    if (textObject4 != null) textObject4.Text = Applicant;
                    if (textObject5 != null) textObject5.Text = Prodept;
                    if (textObject6 != null) textObject6.Text = Equipment;
                    if (textObject7 != null) textObject7.Text = Times_of_Demand;
                    if (textObject8 != null) textObject8.Text = Exixt_Qty;
                    if (textObject9 != null) textObject9.Text = Demand_Qty;
                    if (textObject10 != null) textObject10.Text = Output_Target;
                    if (textObject11 != null) textObject11.Text = IE_standard;
                    if (textObject12 != null) textObject12.Text = Tran_dept;
                    if (textObject13 != null) textObject13.Text = Remarks;
                    if (textObject14 != null) textObject14.Text = Receive_Date;
                    if (textObject15 != null) textObject15.Text = Supervisor;
                    if (textObject16 != null) textObject16.Text = Section_Head;
                    if (textObject17 != null) textObject17.Text = Plantin_chanrge;
                    if (textObject18 != null) textObject18.Text = Procontrol_Dept;
                    if (textObject19 != null) textObject19.Text = Controller;

                }

                // Prepare and show the report
                report.Prepare();
                report.ShowPrepared(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading FastReport: " + ex.Message);
            }
        }

       
        public static void Assembly_checklistreport(Control ctr, string fileName, DataTable dt, Dictionary<string, bool> radioButtonStates, Dictionary<string, string> radioButtonMappings, Dictionary<string, Image> images)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("Report file not found: " + fileName, "Report prompts");
                    return;
                }

                ctr.Controls.Clear();
                FastReport.Report report = new FastReport.Report();
                FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl
                {
                    Dock = System.Windows.Forms.DockStyle.Fill
                };

                ctr.Controls.Add(previewControl);
                report.Preview = previewControl;
                report.Load(fileName);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow firstRow = dt.Rows[0];

                    string[] columnNames = { "Seq", "Dept", "COD", "BModel", "AModel", "ActualCOD", "DelayReasons", "Aarticle", "COType", "Select_Plan",
                        "Problem", "COT", "COPT", "RUT", "Line_Adate", "Value_Adate", "MN_Adate", "Machine_Adate", "CTPT_Adate", "Laust_Adate", "Gauge_Adate", "SOP_Adate", "Cement_Adate"};
                    var mappings = new Dictionary<string, string>
            {
                { "Text20", columnNames[0] },
                { "Text7", columnNames[1] },
                { "Text11", columnNames[2] },
                { "Text3", columnNames[3] },
                { "Text8", columnNames[4] },
                { "Text12", columnNames[5] },
                { "Text16", columnNames[6] },
                { "Text4", columnNames[7] },
                { "Text18", columnNames[8] },
                { "Text15", columnNames[9] },

                 { "Text99", columnNames[10] },
                { "Text101", columnNames[11] },
                { "Text103", columnNames[12] },
                { "Text105", columnNames[13] },

                { "Text54", columnNames[14] },
                { "Text56", columnNames[15] },
                { "Text58", columnNames[16] },
                 { "Text60", columnNames[17] },
                { "Text62", columnNames[18] },
                { "Text64", columnNames[19] },
                { "Text66", columnNames[20] },
                { "Text68", columnNames[21] },
                { "Text70", columnNames[22] }


            };

                    foreach (var mapping in mappings)
                    {
                        var reportObject = report.FindObject(mapping.Key);
                        int columnIndex = dt.Columns.IndexOf(mapping.Value);

                        if (columnIndex >= 0 && reportObject is FastReport.TextObject textObject)
                        {
                            textObject.Text = !DBNull.Value.Equals(firstRow[columnIndex])
                                ? firstRow[columnIndex].ToString()
                                : string.Empty;
                        }
                    }

                    if (radioButtonStates == null)
                    {
                        throw new ArgumentNullException(nameof(radioButtonStates));
                    }

                    foreach (var mapping in radioButtonMappings)
                    {
                        var reportObject = report.FindObject(mapping.Key);
                        if (reportObject != null && reportObject is FastReport.CheckBoxObject checkBoxObject)
                        {
                            bool isChecked = radioButtonStates.ContainsKey(mapping.Value) && radioButtonStates[mapping.Value];
                            checkBoxObject.Visible = isChecked;
                            checkBoxObject.Checked = isChecked;
                        }
                        else
                        {
                            // Log or handle the case where the object is not found
                            Console.WriteLine($"Report object with key {mapping.Key} not found.");
                        }
                    }
                }
              //  Bind images to FastReport objects
                if (images != null)
                {
                    foreach (var kvp in images)
                    {
                        string key = kvp.Key;
                        Image image = kvp.Value;


                        var imageObject = report.FindObject(key) as FastReport.PictureObject;
                        if (imageObject != null)
                        {
                            imageObject.Image = image; // Correct property to assign the image
                            imageObject.Visible = true; // Ensure the object is visible in the report
                        }
                        else
                        {
                            // Debug information if the object is not found
                            Console.WriteLine($"PictureObject with name {key} not found in the report.");
                        }
                    }
                }
                report.Prepare();
                report.ShowPrepared(true);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Report file not found: " + ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading FastReport: " + ex.Message, "Error");
            }
        }
        public static void Stitching_Checklistreport(Control ctr, string fileName, DataTable dt, Dictionary<string, bool> SradioButtonStates, Dictionary<string, string> radioButtonMappings, Dictionary<string, Image> images)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("Report file not found: " + fileName, "Report prompts");
                    return;
                }

                ctr.Controls.Clear();
                FastReport.Report report = new FastReport.Report();
                FastReport.Preview.PreviewControl previewControl = new FastReport.Preview.PreviewControl
                {
                    Dock = System.Windows.Forms.DockStyle.Fill
                };

                ctr.Controls.Add(previewControl);
                report.Preview = previewControl;
                report.Load(fileName);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow firstRow = dt.Rows[0];

                    string[] columnNames = { "Seq", "Dept", "COD", "BModel", "AModel", "ActualCOD", "DelayReasons", "Aarticle", "COType", "Select_Plan",
                        "Problem", "COT", "COPT", "RUT", "Line_Adate", "Value_Adate", "MN_Adate", "Machine_Adate", "CTPT_Adate","SOP__Adate", "Gauge_Adate","Laust_Adate","Cutt_Adate","Eligible_Adate","Compo_Adate", "CS_Adate"};
                    var mappings = new Dictionary<string, string>
            {
                { "SText20", columnNames[0] },
                { "SText7", columnNames[1] },
                { "SText11", columnNames[2] },
                { "SText3", columnNames[3] },
                { "SText8", columnNames[4] },
                { "SText12", columnNames[5] },
                { "SText16", columnNames[6] },
                { "SText4", columnNames[7] },
                { "SText18", columnNames[8] },
                { "SText15", columnNames[9] },
                { "SText99", columnNames[10] },
                { "SText101", columnNames[11] },
                { "SText103", columnNames[12] },
                {"Stxtrut",columnNames[13] },
                { "SText54", columnNames[14] },
                { "SText56", columnNames[15] },
                { "SText58", columnNames[16] },
                { "SText60", columnNames[17] },
                { "SText62", columnNames[18] },
                { "SText64", columnNames[19] },
                { "SText66", columnNames[20] },
                { "SText68", columnNames[21] },
                { "SText70", columnNames[22] },
                { "SText112", columnNames[23] },
                { "SText115", columnNames[24] },
                { "SText118", columnNames[25] }
            


            };

                    foreach (var mapping in mappings)
                    {
                        var reportObject = report.FindObject(mapping.Key);
                        int columnIndex = dt.Columns.IndexOf(mapping.Value);

                        if (columnIndex >= 0 && reportObject is FastReport.TextObject textObject)
                        {
                            textObject.Text = !DBNull.Value.Equals(firstRow[columnIndex])
                                ? firstRow[columnIndex].ToString()
                                : string.Empty;
                        }
                    }

                    if (SradioButtonStates == null)
                    {
                        throw new ArgumentNullException(nameof(SradioButtonStates));
                    }

                    foreach (var mapping in radioButtonMappings)
                    {
                        var reportObject = report.FindObject(mapping.Key);
                        if (reportObject != null && reportObject is FastReport.CheckBoxObject checkBoxObject)
                        {
                            bool isChecked = SradioButtonStates.ContainsKey(mapping.Value) && SradioButtonStates[mapping.Value];
                            checkBoxObject.Visible = isChecked;
                            checkBoxObject.Checked = isChecked;
                        }
                        else
                        {
                            // Log or handle the case where the object is not found
                            Console.WriteLine($"Report object with key {mapping.Key} not found.");
                        }
                    }
                }
                // Bind images to FastReport objects
                if (images != null)
                {
                    foreach (var kvp in images)
                    {
                        string key = kvp.Key;
                        Image image = kvp.Value;

                        var imageObject = report.FindObject(key) as FastReport.PictureObject;
                        if (imageObject != null)
                        {
                            imageObject.Image = image; // Correct property to assign the image
                            imageObject.Visible = true; // Ensure the object is visible in the report
                        }
                        else
                        {
                            // Debug information if the object is not found
                            Console.WriteLine($"PictureObject with name {key} not found in the report.");
                        }
                    }
                }


                report.Prepare();
                report.ShowPrepared(true);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Report file not found: " + ex.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading FastReport: " + ex.Message, "Error");
            }
        }

    }
}
