using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class T_QCO_Fastreport : MaterialForm
    {

        public T_QCO_Fastreport(DataTable dt, string path)
        {
            InitializeComponent();
            SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
            Dictionary<string, string> dic = new Dictionary<string, string>();        
            FastReportHelper.LoadFastReport1(panel1, path, dic, dt, "Table");
          
        }
    }
}
