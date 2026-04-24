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
    public partial class T_Preventive_Measures : Form
    {
        public T_Preventive_Measures()
        {
            InitializeComponent();
           
        }
        public static RichTextBox GetPreventiveContent()
        {
            RichTextBox tempBox = new RichTextBox();
            tempBox.Font = new Font("Segoe UI", 10);
            string[] lines = new string[]
            {
        "✅ Preventive Measures (Proactive actions to avoid problems)",
        "Line supervisors fill checklist before one week advance → Preventive: Ensures preparation is not left to the last minute.",
        "Please focus on quality issues in production → Preventive: Avoids defects that cause delays during or after changeover.",
        "Check with MNT people about machine condition → Preventive: Prevents machine breakdowns during changeover.",
        "Arrange CS programs, molds, threads before changeover → Preventive: Ensures all essentials are ready in advance.",
        "PC to ensure material is in place before changeover → Preventive: Avoids material-related delays.",
        "Trained operators(multiskilling) → Preventive: Ensures manpower is flexible and efficient during changeover.",
        "Focus on material delivery from cutting and outsourcing → Preventive: Ensures continuous supply flow without delay.",
        "All departments to play roles effectively → Preventive: Promotes collaboration and clarity of responsibility."
            };

            for (int i = 0; i < lines.Length; i++)
            {
                switch (i)
                {
                    case 0: tempBox.SelectionColor = Color.Red; break;
                    case 1: tempBox.SelectionColor = Color.DarkGreen; break;
                    case 2: tempBox.SelectionColor = Color.Blue; break;
                    default: tempBox.SelectionColor = Color.Black; break;
                }

                tempBox.AppendText(lines[i] + Environment.NewLine);
            }

            return tempBox;
        }




    }
}
