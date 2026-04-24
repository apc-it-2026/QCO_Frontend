using DataGrid.DataGridViewCustomColumn;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using SJeMES_Control_Library;
using SJeMES_Framework.Common;
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
//using DataGrid.DataGridViewCustomColumn;

namespace T_Quick_Changeover
{
    public partial class QCO_Upload : MaterialForm
    {
        public static string SEQ;
        public QCO_Upload()
        {
            InitializeComponent();
            ///SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
        }
        DataTable dt;

        private void btn_search_Click(object sender, EventArgs e)
        {
             FormLoad();
           // Loadallfiles(pageControl2.PageSize, pageControl2.PageIndex,);
        }
       
        public void Addseq2(string seq)
        {
            SEQ = seq;
        }
        public void FormLoad()
        {
            pageControl2.PageSize = int.Parse(enum_page.enum_PageSize);
            pageControl2.PageIndex = int.Parse(enum_page.PageIndex);
            pageControl2.SetPage();
            //start_date.Format = DateTimePickerFormat.Custom;
            //start_date.CustomFormat = " ";
        }
      
        public void GetDataList(int pageSize, int pageIndex, out int totalCount)
        {
            totalCount = 0;
            try
            {

                //请求api的数据展示
                Dictionary<string, object> p = new Dictionary<string, object>();
                p.Add("FILE_NAME", txt_FILE_NAME.Text);
                p.Add("pageSize", pageSize);
                p.Add("pageIndex", pageIndex);
                p.Add("SEQ", SEQ);


                string retdata = WebAPIHelper.Post(
                                            Program.Client.APIURL,
                                            "KZ_QCO",//类库名
                                            "KZ_QCO.Controllers.GeneralServer",//类名
                                            "GetMainList",//方法名
                                            Program.Client.UserToken,//token
                                            Newtonsoft.Json.JsonConvert.SerializeObject(p));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retdata)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(retdata)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                    dataGridView1.Rows.Clear();
                    if(dt == null)
                    {
                        string msg = "No Data Found";
                        SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                    }
                    else 
                    {
                        int i = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            dataGridView1.Rows.Add();
                            DataGridViewRow dgvr = dataGridView1.Rows[i];
                            var currOpa = (DataGrid.DataGridViewCustomColumn.DataGridViewOperationItems)dgvr.Cells["operation"].Value;                          
                            dgvr.Cells["ID"].Value = dr["ID"].ToString();
                            dgvr.Cells["file_name"].Value = dr["file_name"].ToString();
                            dgvr.Cells["curr_upload_time"].Value = dr["curr_upload_time"].ToString();
                            i++;
                        }
                    }
                   
                    this.dataGridView1.ClearSelection();
                }
                else
                {
                    string msg = SJeMES_Framework.Common.UIHelper.UImsg("Import failed!", Program.Client, "", Program.Client.Language);
                    string exceptionMsg = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retdata)["ErrMsg"].ToString();
                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg + exceptionMsg);
                }               
            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            DataGridViewCheckBoxColumn checkbox = new DataGridViewCheckBoxColumn();
            checkbox.HeaderText = "选择";
            checkbox.Name = "IsChecked";
            checkbox.TrueValue = true;
            checkbox.FalseValue = false;
            checkbox.DataPropertyName = "IsChecked";

            GenClass.AutoSizeColumnStyle(dataGridView1);
            pageControl2.BindPageEvent += GetDataList;
            FormLoad();
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    string name = this.dataGridView1.Columns[e.ColumnIndex].Name;
                    if (name == "operation")
                    {
                        DataGridViewOperationCell cell = this.dataGridView1.Rows[this.dataGridView1.CurrentRow.Index].Cells["operation"] as DataGridViewOperationCell;

                        if (cell.CurrentItem == null)
                        {
                            return;
                        }
                        if (cell.CurrentItem.Equals("select"))//查看
                        {
                            string file_name = dataGridView1.CurrentRow.Cells["file_name"].Value.ToString();
                            DataTable dt = File_list(file_name);
                            QCO_View add = new QCO_View(dt);
                            add.ShowDialog();
                            FormLoad();
                        }
                        else if (cell.CurrentItem.Equals("delete"))//删除
                        {

                            if (MessageBox.Show("confirm deletion? ", "This delete cannot be undone", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    string file_name = dataGridView1.CurrentRow.Cells["file_name"].Value.ToString();
                                    Dictionary<string, object> p = new Dictionary<string, object>();
                                    p.Add("file_name", file_name);
                                    string retdata = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL,
                                         "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Main_Delete", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                                    ResultObject ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultObject>(retdata);
                                    if (!ret.IsSuccess)
                                    {
                                        throw new Exception(ret.ErrMsg);
                                    }
                                    else
                                    {
                                        // MessageBox.Show("Operation deleted successfully");
                                        MessageHelper.ShowSuccess(this, "Operation deleted successfully");
                                        FormLoad();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                                    SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
                                }
                            }

                        }
                        else if (cell.CurrentItem.Equals("update"))//生效
                        {

                            if (MessageBox.Show("Confirmed to take effect? ", "This effect cannot be undone", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    string currId = dataGridView1.CurrentRow.Cells["ID"].Value.ToString();
                                    Dictionary<string, object> p = new Dictionary<string, object>();
                                    p.Add("id", currId);
                                    string retdata = SJeMES_Framework.WebAPI.WebAPIHelper.Post(Program.Client.APIURL,
                                         "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Main_Update_EFFECT", Program.Client.UserToken, Newtonsoft.Json.JsonConvert.SerializeObject(p));
                                    ResultObject ret = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultObject>(retdata);
                                    if (!ret.IsSuccess)
                                    {
                                        throw new Exception(ret.ErrMsg);
                                    }
                                    else
                                    {
                                       
                                        MessageHelper.ShowSuccess(this, "The operation took effect successfully");
                                        // FormLoad();
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
                }
            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
        }
        public DataTable File_list(string file_name)
        {
            DataTable dt = new DataTable();
            try
            {
                //请求api的数据展示
                Dictionary<string, object> data = new Dictionary<string, object>();
                //键值对传值
                data.Add("file_name", file_name);
                string retdata = SJeMES_Framework.WebAPI.WebAPIHelper.Post(
                                            Program.Client.APIURL,
                                            "KZ_QCO",//类库名
                                            "KZ_QCO.Controllers.GeneralServer",//类名
                                            "Main_ListFile",//方法名
                                            Program.Client.UserToken,//token
                                            Newtonsoft.Json.JsonConvert.SerializeObject(data));
                if (Convert.ToBoolean(Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(retdata)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(retdata)["RetData"].ToString();
                    dt = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
                }
            }
            catch (Exception ex)
            {
                string msg = SJeMES_Framework.Common.UIHelper.UImsg(ex.Message, Program.Client, Program.Client.WebServiceUrl, Program.Client.Language);
                SJeMES_Control_Library.MessageHelper.ShowErr(this, msg);
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    item["FILE_URL"] = Program.Client.PicUrl + item["FILE_URL"];
                }
            }
            return dt;
        }
    }
}
