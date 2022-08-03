using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Globalization;


namespace Paint
{
    public partial class LoginAndRegister : Form
    {
        private MongoClient dbClient;
        private IMongoDatabase database;
        private IMongoCollection<BsonDocument> collection;
        private SHA256 shHash;

        public LoginAndRegister()
        {
            InitializeComponent();
            btnLogin.PerformClick();
            dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/UserData?retryWrites=true&w=majority");
            database = dbClient.GetDatabase("UserData");
            collection = database.GetCollection<BsonDocument>("UserData");
            shHash = SHA256.Create();
            var bitmap = (Bitmap)Image.FromFile("trowel.png");
            this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
        }

        public Cursor CreateCursor(Bitmap bitmap, Size size)
        {
            bitmap = new Bitmap(bitmap, size);
            return new Cursor(bitmap.GetHicon());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpenLogin_Click(object sender, EventArgs e)
        {
            Login_panel.BringToFront();
            btnOpenLogin.BackColor = Color.FromArgb(255, 160, 122);
            btnOpenLogin.ForeColor = Color.FromArgb(0, 139, 139);
            BtnOpenReg.ForeColor = Color.FromArgb(255, 160, 122);
            BtnOpenReg.BackColor = Color.FromArgb(0, 139, 139);
            txtPass_log.Text = "Password";
            txtPass_log.ForeColor = Color.Gray;
            txtUserName_log.Text = "Username";
            txtUserName_log.ForeColor = Color.Gray;
            txtPass_log.UseSystemPasswordChar = false;
            lblNof.Text = "";
            chkBox_showPass.Checked = false;
        }

        private void BtnOpenReg_Click(object sender, EventArgs e)
        {
            Register_panel.BringToFront();
            BtnOpenReg.BackColor = Color.FromArgb(255, 160, 122);
            BtnOpenReg.ForeColor = Color.FromArgb(0, 139, 139);
            btnOpenLogin.ForeColor = Color.FromArgb(255, 160, 122);
            btnOpenLogin.BackColor = Color.FromArgb(0, 139, 139);
            txtFirstName.Text = "First Name";
            txtFirstName.ForeColor = Color.Gray;
            txtLastName.Text = "Last Name";
            txtLastName.ForeColor = Color.Gray;
            txtUserName_reg.Text = "Username";
            txtUserName_reg.ForeColor = Color.Gray;
            txtPass_reg.Text = "Password";
            txtPass_reg.ForeColor = Color.Gray;
            txtConf_reg.Text = "Confirm Password";
            txtConf_reg.ForeColor = Color.Gray;
            txtDispN.Text = "Display Name";
            txtDispN.ForeColor = Color.Gray;
            txtPass_reg.UseSystemPasswordChar = false;
            txtConf_reg.UseSystemPasswordChar = false;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            label5.Text = "";
            label6.Text = "";
            label8.Text = "";
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void LoginAndRegister_Load(object sender, EventArgs e)
        {
            Login_panel.BringToFront();
            btnOpenLogin.BackColor = Color.FromArgb(255, 160, 122);
            btnOpenLogin.ForeColor = Color.FromArgb(0, 139, 139);
        }

        //Drag form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel_Menu_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        bool checkName(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if ((s[i] >= 32 && s[i] < 65) || (s[i] > 90 && s[i] < 97) || (s[i] > 122 && s[i] <= 126))
                    return false;
            return true;
        }

        bool checkUserName(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if (s[i] == 32)
                    return false;
            return true;
        }

        public string FirstCharToUpper(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            string frstName = txtFirstName.Text.Trim();
            if (FirstCharToUpper(frstName) == "First Name")
            {
                txtFirstName.Text = "";
                txtFirstName.ForeColor = Color.Black;
            }
        }

        private void txtFirstName_Leave(object sender, EventArgs e)
        {
            string frstName = txtFirstName.Text.Trim();
            if (FirstCharToUpper(frstName) == "First Name" || frstName.Equals(""))
            {
                txtFirstName.Text = "First Name";
                txtFirstName.ForeColor = Color.Gray;
            }
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            string lastName = txtLastName.Text.Trim();
            if (FirstCharToUpper(lastName) == "Last Name")
            {
                txtLastName.Text = "";
                txtLastName.ForeColor = Color.Black;
            }
        }

        private void txtLastName_Leave(object sender, EventArgs e)
        {
            string lastName = txtLastName.Text.Trim();
            if (FirstCharToUpper(lastName) == "Last Name" || lastName.Equals(""))
            {
                txtLastName.Text = "Last Name";
                txtLastName.ForeColor = Color.Gray;
            }
        }

        private void txtUserName_reg_Enter(object sender, EventArgs e)
        {
            string userName = txtUserName_reg.Text.Trim();
            if (FirstCharToUpper(userName) == "Username")
            {
                txtUserName_reg.Text = "";
                txtUserName_reg.ForeColor = Color.Black;
            }
        }

        private void txtUserName_reg_Leave(object sender, EventArgs e)
        {
            string userName = txtUserName_reg.Text.Trim();
            if ( FirstCharToUpper(userName) == "Username" || userName.Equals(""))
            {
                txtUserName_reg.Text = "Username";
                txtUserName_reg.ForeColor = Color.Gray;
            }
        }

        private void txtPass_reg_Enter(object sender, EventArgs e)
        {
            string pass = txtPass_reg.Text.Trim();
            if (FirstCharToUpper(pass) == "Password")
            {
                txtPass_reg.Text = "";
                txtPass_reg.UseSystemPasswordChar = true;
                txtPass_reg.ForeColor = Color.Black;
            }
        }

        private void txtPass_reg_Leave(object sender, EventArgs e)
        {
            string pass = txtPass_reg.Text.Trim();
            if (FirstCharToUpper(pass) == "Password" || pass.Equals(""))
            {
                txtPass_reg.Text = "Password";
                txtPass_reg.UseSystemPasswordChar = false;
                txtPass_reg.ForeColor = Color.Gray;
            }
        }

        private void txtConf_reg_Enter(object sender, EventArgs e)
        {
            string conf = txtConf_reg.Text.Trim();
            if (FirstCharToUpper(conf) == "Confirm Password")
            {
                txtConf_reg.Text = "";
                txtConf_reg.UseSystemPasswordChar = true;
                txtConf_reg.ForeColor = Color.Black;
            }
        }

        private void txtConf_reg_Leave(object sender, EventArgs e)
        {
            string conf = txtConf_reg.Text.Trim();
            if (FirstCharToUpper(conf) == "Confirm Password" || conf.Equals(""))
            {
                txtConf_reg.Text = "Confirm Password";
                txtConf_reg.UseSystemPasswordChar = false;
                txtConf_reg.ForeColor = Color.Gray;
            }
        }

        private void userName_log_Enter(object sender, EventArgs e)
        {
            string username = txtUserName_log.Text.Trim();
            if (FirstCharToUpper(username) == "Username")
            {
                txtUserName_log.Text = "";
                txtUserName_log.ForeColor = Color.Black;
            }
        }

        private void userName_log_Leave(object sender, EventArgs e)
        {
            string username = txtUserName_log.Text.Trim();
            if (FirstCharToUpper(username) == "Username" || username.Equals(""))
            {
                txtUserName_log.Text = "Username";
                txtUserName_log.ForeColor = Color.Gray;
            }
        }

        private void txtPass_log_Enter(object sender, EventArgs e)
        {
            string pass = txtPass_log.Text.Trim();
            if (FirstCharToUpper(pass) == "Password")
            {
                txtPass_log.Text = "";
                txtPass_log.UseSystemPasswordChar = true;
                txtPass_log.ForeColor = Color.Black;
            }
        }

        private void txtPass_log_Leave(object sender, EventArgs e)
        {
            string pass = txtPass_log.Text.Trim();
            if (FirstCharToUpper(pass) == "Password" || pass.Equals(""))
            {
                txtPass_log.Text = "Password";
                txtPass_log.UseSystemPasswordChar = false;
                txtPass_log.ForeColor = Color.Gray;
            }
        }

        private void txtDispN_Leave(object sender, EventArgs e)
        {
            string displayName = txtDispN.Text.Trim();
            if (FirstCharToUpper(displayName) == "Display Name" || displayName.Equals(""))
            {
                txtDispN.Text = "Display Name";
                txtDispN.ForeColor = Color.Gray;
            }
        }

        private void txtDispN_Enter(object sender, EventArgs e)
        {
            string displayName = txtDispN.Text.Trim();
            if (FirstCharToUpper(displayName) == "Display Name")
            {
                txtDispN.Text = "";
                txtDispN.ForeColor = Color.Black;
            }
        }


        private void chkBox_showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBox_showPass.Checked == true)
                txtPass_log.UseSystemPasswordChar = false;
            else
                txtPass_log.UseSystemPasswordChar = true;
        }


        public void login()
        {
            string UserName = txtUserName_log.Text.Trim();
            string Password = txtPass_log.Text.Trim();
            bool status;

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("userName", UserName);
            var docsCount = collection.Find(filter).CountDocuments();

            //check_account
            if (docsCount == 1)
            {
                byte[] passWordBytes = Encoding.Unicode.GetBytes(Password);
                byte[] passHashValue;
                string passHexValue;
                passHashValue = shHash.ComputeHash(passWordBytes);
                passHexValue = BitConverter.ToString(passHashValue);
                passHexValue = passHexValue.Replace("-", "");

                filter = builder.And(builder.Eq("userName", UserName), builder.Eq("password", passHexValue));

                var docs = collection.Find(filter).ToList();
                foreach (var doc in docs)
                {
                    string displayName = "";
                    status = doc.GetValue("isActive").AsBoolean;

                    if (!status)
                    {
                        displayName = doc.GetValue("displayName").AsString;
                        var update = Builders<BsonDocument>.Update.Set("isActive", true);
                        collection.UpdateOne(filter, update);
                        this.Hide();
                        new Menu(UserName, displayName, this).Show();

                        //reset form
                        txtPass_log.Text = "Password";
                        txtPass_log.ForeColor = Color.Gray;
                        txtUserName_log.Text = "Username";
                        txtUserName_log.ForeColor = Color.Gray;
                        txtPass_log.UseSystemPasswordChar = false;
                        lblNof.Text = "";
                        chkBox_showPass.Checked = false;
                    }
                    else
                    {
                        lblNof.Text = "This account is currently used at other computer.";
                        txtPass_log.Clear();
                        txtConf_reg.UseSystemPasswordChar = false;
                        txtPass_log.Text = "Password";
                        txtPass_log.ForeColor = Color.Gray;
                    }
                }
            }
            else
            {
                lblNof.Text = "Incorrect password or this account doesn't exist !!!";
                txtConf_reg.UseSystemPasswordChar = false;
                txtPass_log.Text = "Password";
                txtPass_log.ForeColor = Color.Gray;
            }
        }

        public void register()
        {
            string FirstName = txtFirstName.Text.Trim();
            string LastName = txtLastName.Text.Trim();
            string UserName = txtUserName_reg.Text.Trim();
            string DisplayName = txtDispN.Text.Trim();
            string Password = txtPass_reg.Text.Trim();
            string ConfirmPass = txtConf_reg.Text.Trim();

            label9.Text = "";
            label10.Text = "";
            label3.Text = "";
            label6.Text = "";

            if (!checkName(FirstName) || !checkName(LastName) || !checkUserName(UserName) || Password != ConfirmPass
                || FirstName == "" || LastName == "" || UserName == "" || Password == "" || FirstCharToUpper(FirstName) == "First Name"
                || FirstCharToUpper(LastName) == "Last Name" || FirstCharToUpper(UserName) == "Username" || FirstCharToUpper(DisplayName) == "Display Name" 
                || FirstCharToUpper(Password) == "Password" || DisplayName == "" || ConfirmPass == "" || FirstCharToUpper(ConfirmPass) == "Confirm Password")
            {
                if (!checkName(FirstName) || FirstName == "" || FirstCharToUpper(FirstName) == "First Name")
                {
                    txtFirstName.Text = "First Name";
                    txtFirstName.ForeColor = Color.Gray;
                    label1.Text = "!";
                }
                else label1.Text = "";

                if (!checkName(LastName) || LastName == "" || FirstCharToUpper(LastName) == "Last Name")
                {
                    txtLastName.Text = "Last Name";
                    txtLastName.ForeColor = Color.Gray;
                    label2.Text = "!";
                }
                else label2.Text = "";

                if (DisplayName == "" || FirstCharToUpper(DisplayName) == "Display Name")
                {
                    label8.Text = "!";
                }
                else label8.Text = "";

                if (!checkUserName(UserName) || UserName == "" || FirstCharToUpper(UserName) == "Username")
                {
                    txtUserName_reg.Text = "Username";
                    txtUserName_reg.ForeColor = Color.Gray;
                    label3.Text = "!";
                }
                else label3.Text = "";

                if (Password.Trim() == "" || FirstCharToUpper(Password) == "Password")
                {
                    txtPass_reg.Text = "Password";
                    txtPass_reg.ForeColor = Color.Gray;
                    label4.Text = "!";
                }
                else label4.Text = "";

                if (Password != ConfirmPass || ConfirmPass == "" || FirstCharToUpper(ConfirmPass) == "Confirm Password")
                {
                    txtConf_reg.Text = "Confirm Password";
                    txtConf_reg.UseSystemPasswordChar = false;
                    txtConf_reg.ForeColor = Color.Gray;
                    label5.Text = "!";
                }
                else label5.Text = "";
            }
            else
            {
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
                label8.Text = "";

                byte[] passWordBytes = Encoding.Unicode.GetBytes(Password);
                byte[] passHashValue;
                string passHexValue;
                passHashValue = shHash.ComputeHash(passWordBytes);
                passHexValue = BitConverter.ToString(passHashValue);
                passHexValue = passHexValue.Replace("-", "");

                var builder = Builders<BsonDocument>.Filter;
                var filter1 = builder.Eq("userName", UserName);
                var filter2 = builder.Eq("displayName", DisplayName);
                var docsCount1 = collection.Find(filter1).CountDocuments();
                var docsCount2 = collection.Find(filter2).CountDocuments();

                if (docsCount2 >= 1)
                {
                    txtDispN.Text = "Display Name";
                    txtDispN.ForeColor = Color.Gray;
                    label9.Text = "This name has already existed!!!";
                    label10.Text = "!";
                    return;
                }
                else
                {

                    if (docsCount1 >= 1)
                    {
                        txtUserName_reg.Text = "Username";
                        txtUserName_reg.ForeColor = Color.Gray;
                        label6.Text = "This username has already existed!!!";
                        label3.Text = "!";
                        return;
                    }
                    else
                    {
                        //add_account
                        builder = Builders<BsonDocument>.Filter;
                        var filter = builder.Empty;
                        var docs = collection.Find(filter).ToList();

                        BsonDocument userData = new BsonDocument()
                            .Add("_id", docs.Count + 1)
                            .Add("firstName", FirstName)
                            .Add("lastName", LastName)
                            .Add("displayName", DisplayName)
                            .Add("userName", UserName)
                            .Add("password", passHexValue)
                            .Add("isActive", false);

                        try
                        {
                            collection.InsertOne(userData);
                            MessageBox.Show("Account's created successfully", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Reset form
                            txtFirstName.Text = "First Name";
                            txtFirstName.ForeColor = Color.Gray;
                            txtLastName.Text = "Last Name";
                            txtLastName.ForeColor = Color.Gray;
                            txtUserName_reg.Text = "Username";
                            txtUserName_reg.ForeColor = Color.Gray;
                            txtPass_reg.Text = "Password";
                            txtPass_reg.ForeColor = Color.Gray;
                            txtConf_reg.Text = "Confirm Password";
                            txtConf_reg.ForeColor = Color.Gray;
                            txtDispN.Text = "Display Name";
                            txtDispN.ForeColor = Color.Gray;
                            txtPass_reg.UseSystemPasswordChar = false;
                            txtConf_reg.UseSystemPasswordChar = false;
                            label1.Text = "";
                            label2.Text = "";
                            label3.Text = "";
                            label4.Text = "";
                            label5.Text = "";
                            label6.Text = "";
                            label8.Text = "";
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            register();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void txtUserName_log_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                login();
            }
        }

        private void txtPass_log_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                login();
            }
        }

        private void txtConf_reg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                register();
            }
        }
    }
}
