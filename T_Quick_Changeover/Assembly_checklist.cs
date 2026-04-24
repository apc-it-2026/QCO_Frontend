using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;

namespace T_Quick_Changeover
{
    public partial class Assembly_checklist : Form
    {
        private DataTable _data;
        private string _reportPath;
        private Dictionary<string, bool> _radioButtonStates;
        private Dictionary<string, string> _radioButtonMappings;
        private Dictionary<string, Image> _images;
        //public Assembly_checklist(DataTable dt, string path, Dictionary<string, bool> radioButtonStates, Dictionary<string, string> radioButtonMappings, Dictionary<string, Image> images)
        //{
        //    InitializeComponent();

        //    SJeMES_Framework.Common.UIHelper.UIUpdate(this.Name, this, Program.Client, "", Program.Client.Language);
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    FastReportHelper.Assembly_checklistreport(panel1, path, dt, radioButtonStates, radioButtonMappings, images);
        //}
        public Assembly_checklist(DataTable data, string reportPath, Dictionary<string, bool> radioButtonStates, Dictionary<string, string> radioButtonMappings, Dictionary<string, Image> images)
        {
            _data = data;
            _reportPath = reportPath;
            _radioButtonStates = radioButtonStates;
            _radioButtonMappings = radioButtonMappings;
            _images = images;
            InitializeComponent();
            GenerateReport();
        }

        private void GenerateReport()
        {
            // Call Assembly_checklistreport with images
            FastReportHelper.Assembly_checklistreport(this, _reportPath, _data, _radioButtonStates, _radioButtonMappings, _images);
        }
    }
}
