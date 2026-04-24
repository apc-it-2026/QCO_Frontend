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

namespace T_Quick_Changeover
{
    public partial class T_QCO_Ex_app_t_fileUpload_add : Form
    {
        public static string SEQ;
   
        //private readonly MaterialSkinManager materialSkinManager;
        public string file_guid = string.Empty;// 文件上传 返回的guid
        public string file_name = string.Empty;// 文件上传 返回的名称
        public string _task_no = string.Empty;//
        public string _sclx = string.Empty;//
        public string _art = string.Empty;//
        public Dictionary<string, object> filedic = new Dictionary<string, object>();
        public List<Dictionary<string, object>> filediclist = new List<Dictionary<string, object>>();
        public string filePath;

        public T_QCO_Ex_app_t_fileUpload_add()
        {
            InitializeComponent();
        }

        public void Addseq(string seq)
        {
            SEQ = seq;
        }
        private void btn_file_upload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; 
            string path = string.Empty;
            ofd.Title = "Please select a file";
            ofd.Filter = "All files|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string text = string.Empty;
                filediclist = new List<Dictionary<string, object>>();
                Cursor.Current = Cursors.WaitCursor;
                foreach (string file in ofd.FileNames)
                {
                    byte[] fileContent = File.ReadAllBytes(file); 

                    // Store file content and other necessary details in a dictionary
                    filedic = new Dictionary<string, object>();
                    filedic.Add("file_content", fileContent);
                    // filedic.Add("file_name", Path.GetFileName(file));
                    filedic.Add("file_name", $"{SEQ}_{Path.GetFileName(file)}");
                    filediclist.Add(filedic);

                    text += Path.GetFileNameWithoutExtension(file) + ",";
                }

                lbl_file_name.Text = text.TrimEnd(',');
            }
        }
        private void btn_commit_Click(object sender, EventArgs e)
        {           
            try
            {
                if (filediclist.Count == 0)
                {
                    // Check if files are uploaded
                    string tipsMsg = SJeMES_Framework.Common.UIHelper.UImsg("Please upload the file!", Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                    MessageBox.Show(tipsMsg);
                    return;
                }
                foreach (var item in filediclist)
                {
                    byte[] fileContent = (byte[])item["file_content"];
                    string fileName = item["file_name"].ToString();
                    string folderPath = @"E:\AEQS_Project_Related_Files\AEQS_Source_Code\deliveryFile\Client\T_Quick_Changeover\FileUploads";

                    // Get the current date
                    string currentDate = DateTime.Now.ToString("yyyyMMdd");
                    string newFileName = $"{currentDate}_{fileName}";
                    string filePath = Path.Combine(folderPath, newFileName);

                    File.WriteAllBytes(filePath, fileContent);
                }
                MessageHelper.ShowSuccess(this, "Files submitted successfully!");

                this.Close();
            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }




        }


    }
}
