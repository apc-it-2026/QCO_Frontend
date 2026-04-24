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
    public partial class Stitching_Checklist : MaterialForm
    {
        private DataTable _data;
        private string _reportPath;
        private Dictionary<string, bool> _SradioButtonStates;
        private Dictionary<string, string> _radioButtonMappings;
        private Dictionary<string, Image> _images;
        //public Stitching_Checklist()
        //{
        //    InitializeComponent();
        //}
        public Stitching_Checklist(DataTable data, string reportPath, Dictionary<string, bool> radioButtonStates, Dictionary<string, string> radioButtonMappings, Dictionary<string, Image> images)
        {
            _data = data;
            _reportPath = reportPath;
            _SradioButtonStates = radioButtonStates;
            _radioButtonMappings = radioButtonMappings;
            _images = images;
            InitializeComponent();
            GenerateReport();
        }

        private void GenerateReport()
        {
            // Call Assembly_checklistreport with images
            FastReportHelper.Stitching_Checklistreport(this, _reportPath, _data, _SradioButtonStates, _radioButtonMappings, _images);
        }
    }
}
