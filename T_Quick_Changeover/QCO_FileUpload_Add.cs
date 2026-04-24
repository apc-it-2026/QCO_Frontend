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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class QCO_FileUpload : Form
    {
        //private readonly MaterialSkinManager materialSkinManager;
        public string file_guid = string.Empty;// 文件上传 返回的guid
        public string file_name = string.Empty;// 文件上传 返回的名称
        public string _task_no = string.Empty;//
        public string _sclx = string.Empty;//
        public string _art = string.Empty;//
        public Dictionary<string, object> filedic = new Dictionary<string, object>();
        public List<Dictionary<string, object>> filediclist = new List<Dictionary<string, object>>();
        public static string SEQ;
        public QCO_FileUpload()
        {
            InitializeComponent();
        }
        public void Addseq(string seq)
        {
            SEQ = seq;
        }
        private void btn_file_upload_Click(object sender, EventArgs e)
        {
            try
            {
                if ( radioButton2.Checked == false)
                {
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("Select file method cannot be empty！", Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                    return;
                }
                if (radioButton2.Checked == false)
                {
                    FolderBrowserDialog ofd2 = new FolderBrowserDialog();
                    ofd2.Description = "Select the folder directory where the file is located";  //提示的文字
                    if (ofd2.ShowDialog() == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        string text = string.Empty;
                        string foldPath = ofd2.SelectedPath;
                        string[] files = System.IO.Directory.GetFiles(foldPath);
                        foreach (var item in files)
                        {
                            UploadFileResultDto res = SJeMES_Framework.Common.HttpHelper.UpLoadCommon(Program.Client.UploadUrl, item.ToString(), Program.Client.UserToken);
                            if (res.IsSuccess)
                            {
                                var resultDIC = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(res.ReturnObj.ToString());
                                filedic = new Dictionary<string, object>();
                                //file_guid = resultDIC["guid"].ToString();
                                //string name = System.IO.Path.GetFileNameWithoutExtension(item.ToString());
                                string name = System.IO.Path.GetFileNameWithoutExtension(item);
                                filedic.Add("file_guid", resultDIC["guid"].ToString());
                                filedic.Add("file_name", name);
                                filediclist.Add(filedic);
                            }
                        }
                        lbl_file_name.Text = Path.GetFileName(ofd2.SelectedPath);
                    }
                }
                else
                {
                    //创建文件弹出选择窗口（包括文件名）对象
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Multiselect = true;//支持多选
                    string path = string.Empty;
                    ofd.Title = "Please select a file";
                    ofd.Filter = "All files|*.*";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        string text = string.Empty;
                        filediclist = new List<Dictionary<string, object>>();
                        for (int i = 0; i < ofd.FileNames.Length; i++)
                        {
                            UploadFileResultDto res = SJeMES_Framework.Common.HttpHelper.UpLoadCommon(Program.Client.UploadUrl, ofd.FileNames[i].ToString(), Program.Client.UserToken);
                            if (res.IsSuccess)
                            {
                                var resultDIC = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(res.ReturnObj.ToString());
                                filedic = new Dictionary<string, object>();
                                //file_guid = resultDIC["guid"].ToString();
                                string name = System.IO.Path.GetFileNameWithoutExtension(ofd.FileNames[i].ToString());
                                filedic.Add("file_guid", resultDIC["guid"].ToString());
                                filedic.Add("file_name", $"{SEQ}_"+ name);
                                filediclist.Add(filedic);
                                text += name + ",";
                            }
                        }
                        lbl_file_name.Text = text.TrimEnd(',');
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
        }

        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                if (filediclist.Count == 0)
                {
                    string tipsMsg = SJeMES_Framework.Common.UIHelper.UImsg("Please upload the file！", Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                   // MessageBox.Show(tipsMsg);
                    MessageHelper.ShowSuccess(this, tipsMsg);
                    return;
                }
                if (string.IsNullOrEmpty(time.Text))
                {
                    string timeMsg = SJeMES_Framework.Common.UIHelper.UImsg("Please select a start date！", Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);                   
                    MessageHelper.ShowSuccess(this, timeMsg);
                    return;
                }
                bool issuccess = true;
             
                int i = 0;
                foreach (var item in filediclist)
                {
                    Dictionary<string, object> p = new Dictionary<string, object>();
                    //p.Add("file", );
                    p.Add("file_name", item["file_name"]);
                    p.Add("file_guid", item["file_guid"]);
                    p.Add("time", time.Text);

                    string retdata = WebAPIHelper.Post(
                                                Program.Client.APIURL,
                                                "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Commit_Main",//方法名
                                                Program.Client.UserToken,//token
                                                Newtonsoft.Json.JsonConvert.SerializeObject(p));
                    ResultObject ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultObject>(retdata);
                    if (!ret.IsSuccess)
                    {
                        issuccess = false;
                        MessageBox.Show(ret.ErrMsg);
                        return;
                    }  
                    Thread.Sleep(100);
               
                    i++;

                }

                if (issuccess)
                {
                    MessageBox.Show("Submitted successfully！");
                   // MessageHelper.ShowSuccess(this, "Submitted successfully！");
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
        }
    }
}
