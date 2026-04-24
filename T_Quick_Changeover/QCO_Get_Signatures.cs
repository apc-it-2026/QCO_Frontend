using Newtonsoft.Json;
using SJeMES_Control_Library;
using SJeMES_Framework.WebAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T_Quick_Changeover
{
    public partial class QCO_Get_Signatures : Form
    {
        public List<Dictionary<string, object>> filediclist = new List<Dictionary<string, object>>();
        public Dictionary<string, object> filedic = new Dictionary<string, object>();
        byte[] fileContent;
        DataTable dt;
        public int receivedIndex;
        public int recieved;

        private Point? previousPoint = null;
        private Pen pen = new Pen(Color.Red, 2);
        byte[] signatureBytes;


        private Dictionary<int, List<Point>> signatureObject = new Dictionary<int, List<Point>>();
        private Pen signaturePen = new Pen(Color.Black, 2);
        private List<Point> currentCurvePoints;
        private int currentCurve = -1;
        public QCO_Get_Signatures(int index,int f)
        {
            InitializeComponent();
            receivedIndex = index;
            recieved = f;
            signametxtx.KeyPress += signametxtx_KeyPress;
            sigpasstxt.KeyPress += sigpasstxt_KeyPress;
            sigpasstxt.MaxLength = 6;
            signametxtx.MaxLength = 6;
            Bartxt.KeyPress += Bartxt_KeyPress;
            passtxt.KeyPress += passtxt_KeyPress;
            Bartxt.MaxLength = 6;
            passtxt.MaxLength = 6;
            textBox1.KeyPress += textBox1_KeyPress;
            textBox1.MaxLength = 6;
            Barcodetxt.KeyPress += Barcodetxt_KeyPress;
            Barcodetxt.MaxLength = 6;

        }

        private void uploadbtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string text = string.Empty;
                filediclist = new List<Dictionary<string, object>>();

                foreach (string file in openFileDialog.FileNames)
                {
                    fileContent = File.ReadAllBytes(file);
                    filedic = new Dictionary<string, object>();
                    filedic.Add("file_content", fileContent);
                    filediclist.Add(filedic);
                }
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;             
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
            }
            
        }

        private void savebtn_Click(object sender, EventArgs e)
        {

            string username = Barcodetxt.Text;
            string password = Pwdtxt.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please provide both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Please upload an image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
          

            SaveToDatabase(username, password, fileContent);
        }
      
        private void SaveToDatabase(string username, string password, byte[] image)
        {
            try
            {
                string base64Image = Convert.ToBase64String(image);

                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("username", username);
                data.Add("password", password);
                data.Add("image", fileContent);

                string jsonData = JsonConvert.SerializeObject(data);

                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getpermission",
                    Program.Client.UserToken, jsonData);

                if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
                {
                    string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                    if (json == "1")
                    {                       
                        MessageBox.Show("Inserted successfully!");
                    }
                    else
                    {
                        MessageHelper.ShowErr(this, "No Data Found");
                    }                    
                }
                else
                {
                    MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Barcodetxt.Text = "";
            Pwdtxt.Text = "";
            pictureBox1.Image = null;

        }

        private void Apprbtn_Click(object sender, EventArgs e)
        {
            string username = Bartxt.Text;
            string password =  passtxt.Text;
            if (ValidateCredentials(username, password))
            { 
               
                DisplayImageFromDatabase(username, password);
               
                DisplayImage1(username, password);
            }
            else
            {
                MessageHelper.ShowErr(this, "Invalid Barcode or password.");
            }
           
        }
        private bool ValidateCredentials(string username, string password)
        {
            try
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("username", username);
                data.Add("password", password);

                string jsonData = JsonConvert.SerializeObject(data);

                string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Check_Cred",
                    Program.Client.UserToken, jsonData);

                return Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating credentials: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void DisplayImage1(string username, string password)
        {
            try
            {
                byte[] imageData = GetImageFromDatabase(username, password);

                if (imageData != null && imageData.Length > 0)
                {
                    Image img = ByteArrayToImage(imageData);
                    T_QCO_Checklist2 existingForm = Application.OpenForms.OfType<T_QCO_Checklist2>().FirstOrDefault();
                    if (existingForm != null)
                    {
                        if (receivedIndex == 1 && existingForm != null && recieved == 5)
                        {
                            existingForm.Shows1(img);
                            existingForm.Focus();
                        }
                        else if (receivedIndex == 2 && existingForm != null && recieved == 6)
                        {
                            existingForm.Shows2(img);
                            existingForm.Focus();
                        }
                        else if (receivedIndex == 3 && existingForm != null && recieved == 7)
                        {
                            existingForm.Shows3(img);
                            existingForm.Focus();
                        }
                        else if (receivedIndex == 4 && existingForm != null && recieved == 8)
                        {
                            existingForm.Shows4(img);
                            existingForm.Focus();
                        }
                        //else
                        //{
                        //    T_QCO_Checklist2 newForm = new T_QCO_Checklist2();
                        //    newForm.Showimage(img);
                        //    newForm.Show();
                        //}
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = img;
                        QCO_Prop.Param = "Checklist";
                        QCO_Prop.image = img;
                        this.Hide();

                    }
                }
                else
                {
                    MessageBox.Show("No image found for the user.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DisplayImageFromDatabase(string username, string password)
        {
            try
            {
                byte[] imageData = GetImageFromDatabase(username, password);

                if (imageData != null && imageData.Length > 0)
                {
                    Image image1 = ByteArrayToImage(imageData);
                    T_QCO_Checklist2 existingForm = Application.OpenForms.OfType<T_QCO_Checklist2>().FirstOrDefault();                   
                   
                    if (receivedIndex == 1 && existingForm != null && recieved == 1)
                    {
                        existingForm.Showimage(image1);
                        existingForm.Focus();
                    }
                    else if (receivedIndex == 2 && existingForm != null && recieved == 2)
                    {
                        existingForm.Showimage1(image1);
                        existingForm.Focus();
                    }
                    else if (receivedIndex == 3 && existingForm != null && recieved == 3)
                    {
                        existingForm.Showimage2(image1);
                        existingForm.Focus();
                    }
                    else if (receivedIndex == 4 && existingForm != null && recieved == 4)
                    {
                        existingForm.Showimage3(image1);
                        existingForm.Focus();
                    }
                   
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    pictureBox1.Image = image1;
                    QCO_Prop.Param = "Checklist";
                    QCO_Prop.image = image1;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("No image found for the user.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private byte[] GetImageFromDatabase(string username, string password)
        {
            byte[] imageData = null;

            try
            {
                if (ValidateCredentials(username, password))
                {
                    Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "username", username },
                { "password", password }
            };

                    string jsonData = JsonConvert.SerializeObject(data);

                    string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Check_Cred",
                        Program.Client.UserToken, jsonData);

                    var response = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret);

                    if (Convert.ToBoolean(response["IsSuccess"]))
                    {
                        if (response.ContainsKey("RetData"))
                        {
                            string base64Image = response["RetData"].ToString();
                            base64Image = base64Image.Replace("\"", ""); 

                            // Convert the Base64-encoded string to a byte array
                            imageData = Convert.FromBase64String(base64Image);
                        }
                        else
                        {
                            MessageBox.Show("RetData does not contain the expected property.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageHelper.ShowErr(this, response["ErrMsg"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return imageData;
        }



        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string userInput = textBox1.Text;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", userInput);
            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Get_PWD",
                 Program.Client.UserToken, JsonConvert.SerializeObject(data));
            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                dt = JsonConvert.DeserializeObject<DataTable>(json);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    string password = dt.Rows[0][0].ToString();
                    pdtxt.Text = password;
                    textBox1.ReadOnly = true;
                }
            }
            else
            {
                MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Updatebtn_Click(object sender, EventArgs e)
        {
            string user = textBox1.Text;
            string pass = pdtxt.Text;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", user);
            data.Add("password", pass);          
            string jsonData = JsonConvert.SerializeObject(data);

            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Update_password",
                Program.Client.UserToken, jsonData);

            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                if (json == "1")
                {
                    MessageBox.Show("Updated successfully!");
                }
                else
                {
                    MessageHelper.ShowErr(this, "Not Updated");
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
            textBox1.Text = "";
            pdtxt.Text = "";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
        private void DrawSignature(Graphics g)
        {
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (var curve in signatureObject)
            {
                if (curve.Value.Count < 2)
                    continue;
                using (GraphicsPath gPath = new GraphicsPath())
                {
                    gPath.AddCurve(curve.Value.ToArray(), 0.5F);
                    g.DrawPath(signaturePen, gPath);
                }
            }
        }

        private void Signaturepad_MouseDown(object sender, MouseEventArgs e)
        {
            currentCurvePoints = new List<Point>();
            currentCurve += 1;
            signatureObject.Add(currentCurve, currentCurvePoints);
        }

        private void Signaturepad_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || currentCurve < 0)
                return;
            signatureObject[currentCurve].Add(e.Location);
            Signaturepad.Invalidate();
        }

        private void Signaturepad_Paint(object sender, PaintEventArgs e)
        {
            if (currentCurve < 0 || signatureObject[currentCurve].Count == 0)
                return;
            DrawSignature(e.Graphics);
        }

        private void Signaturepad_MouseUp(object sender, MouseEventArgs e)
        {
            previousPoint = null;
        }

        private void Clear_sigbtn_Click(object sender, EventArgs e)
        {
            currentCurve = -1;
            signatureObject.Clear();
            Signaturepad.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (signametxtx.Text == "")
            {
                MessageHelper.ShowErr(this, "Barcode Should not empty");
                return;
            }
            if (sigpasstxt.Text == "")
            {
                MessageHelper.ShowErr(this, "Password Should not empty");
                return;
            }
            using (Bitmap imgSignature = new Bitmap(Signaturepad.Width, Signaturepad.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(imgSignature))
                {
                    DrawSignature(g);
                    // imgSignature.Save(signaturePath, ImageFormat.Png);
                    sigpictureBox2.Image = new Bitmap(imgSignature);
                }
            }
            byte[] byteArray1 = null;


            // Check if pictureBox1 has an image
            if (sigpictureBox2.Image != null)
            {
                byteArray1 = PictureBoxToByteArray(sigpictureBox2);
                // Add the byte array to filediclist
                Dictionary<string, object> filedic = new Dictionary<string, object>();
                filedic.Add("file_content", byteArray1);
                filediclist.Add(filedic);
            }
            else
            {
                // Handle the case where pictureBox1 does not have an image
                Console.WriteLine("Error: pictureBox1 does not have an image.");
            }

            string base64Image = Convert.ToBase64String(byteArray1);
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("username", signametxtx.Text.ToString());
            data.Add("password", sigpasstxt.Text.ToString());
            data.Add("image", base64Image);

            string jsonData = JsonConvert.SerializeObject(data);

            string ret = WebAPIHelper.Post(Program.Client.APIURL, "KZ_QCO", "KZ_QCO.Controllers.GeneralServer", "Getpermission",
                Program.Client.UserToken, jsonData);

            if (Convert.ToBoolean(JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["IsSuccess"]))
            {
                string json = JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["RetData"].ToString();
                if (json == "1")
                {
                    MessageHelper.ShowSuccess(this, "Dear:" + " Submitted Succesfully");
                }
                else if (json == "2")
                {
                    MessageHelper.ShowSuccess(this, "Dear:" + " Updated Succesfully");
                }
                else
                {
                    MessageHelper.ShowErr(this, "Failed Submission");
                    return;
                }
            }
            else
            {
                MessageHelper.ShowErr(this, JsonConvert.DeserializeObject<Dictionary<string, object>>(ret)["ErrMsg"].ToString());
            }
 
            signametxtx.Text = "";
            sigpasstxt.Text = "";
            currentCurve = -1;
            signatureObject.Clear();
            Signaturepad.Invalidate();

        }
        public byte[] PictureBoxToByteArray(PictureBox pictureBox)
        {
            if (pictureBox.Image != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    pictureBox.Image.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
            else
            {
                // Handle the case where pictureBox does not have an image
                Console.WriteLine("Error: PictureBox does not have an image.");
                return null;
            }
        }

        private void QCO_Get_Signatures_Load(object sender, EventArgs e)
        {
            sigpictureBox2.Visible = false;
        }

        private void sigpasstxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the pressed key is not a digit or a control key, set Handled to true
                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {
                // If the text length is already 6 and the pressed key is not a control key, set Handled to true
                e.Handled = true;
            }
        }

        private void signametxtx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
              
                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {               
                e.Handled = true;
            }
        }

        private void Bartxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {

                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void passtxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {

                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {

                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void Barcodetxt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {

                e.Handled = true;
            }
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length >= 6 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
