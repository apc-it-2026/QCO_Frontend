using SJeMES_Framework.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
     public class Interface
     {
        public static void RunApp(object obj)
        {
            try
            {
                Program.Client = obj as ClientClass;
                 T_QCO_MAIN frm = new T_QCO_MAIN();
                //Form1 frm = new Form1();
                  FormCollection collection = Application.OpenForms;
                frm.Owner = collection["frmMain"];
                frm.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
