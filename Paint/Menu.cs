using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Paint
{
    public partial class Menu : Form
    {
        private string userName;
        private string displayName;
        private LoginAndRegister authForm;

        public Menu(string username, string displayName, LoginAndRegister authForm)
        {
            InitializeComponent();
            this.userName = username;
            this.displayName = displayName;
            this.authForm = authForm;

            Uname.Text = $"Name: {displayName}\n";

            var bitmap = (Bitmap)Image.FromFile("trowel.png");
            this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
        }

        public Cursor CreateCursor(Bitmap bitmap, Size size)
        {
            bitmap = new Bitmap(bitmap, size);
            return new Cursor(bitmap.GetHicon());
        }

        //Drag form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panel_welcome_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnMaximize_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/UserData?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("UserData");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("UserData");

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("userName", userName);
            var update = Builders<BsonDocument>.Update.Set("isActive", false);
            collection.UpdateOne(filter, update);

            authForm.Close();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public void showNote()
        {
            string note = "\t\tMỘT SỐ LƯU Ý KHI SỬ DỤNG ỨNG DỤNG\n\n\n"
                             + "1. Vẽ với tốc độ vừa phải.\n\n"
                             + "2. Vẽ một nét xong thì thả ra để tối ưu hơn khi vẽ với nhiều người.\n\n"
                             + "3. Khi dùng thùng sơn tô màu phải giới hạn trong một vùng nhỏ, không tô full màn trắng.\n\n"
                             + "4. Có thể sử dụng các loại phần mềm VPN để tạo không gian riêng với bạn bè của mình(Vd: Hamachi)\n\n";

            MessageBox.Show(note, "LƯU Ý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void CreateRoom ()
        {
            string hostIP = hostIP_textBox.Text.Trim();
            IPAddress temp;
            if (IPAddress.TryParse(hostIP, out temp))
            {
                DrawTogether_host host = new DrawTogether_host(displayName, hostIP, this);
                host.Show();
                showNote();
                codeRoom_textBox.Text = "Code...";
                hostIP_textBox.Text = "IP address...";

                this.Hide();
            }
            else
            {
                MessageBox.Show("IP address is not correct!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void JoinRoom ()
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/Room?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("Room");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Room");

            string roomCode = codeRoom_textBox.Text.Trim();

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("code", roomCode);
            var docsCount = collection.Find(filter).CountDocuments();

            if (docsCount == 1)
            {
                var docs = collection.Find(filter).ToList();
                string hostIP = "";

                foreach (var doc in docs)
                {
                    hostIP = doc.GetValue("ip").AsString;
                }

                DrawTogether_client client = new DrawTogether_client(displayName, hostIP, roomCode, this);
                client.Show();
                showNote();

                codeRoom_textBox.Text = "Code...";
                hostIP_textBox.Text = "IP address...";
                this.Hide();
            }
            else
            {
                MessageBox.Show("This room is not exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void room_btn_Click(object sender, EventArgs e)
        {
            CreateRoom();
        }

        private void join_btn_Click_1(object sender, EventArgs e)
        {
            JoinRoom();
        }

        private void Menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/UserData?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("UserData");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("UserData");

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("userName", userName);
            var update = Builders<BsonDocument>.Update.Set("isActive", false);
            collection.UpdateOne(filter, update);

            authForm.Close();
        }

        private void codeRom_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (codeRoom_textBox.Text.Trim() == "Code..." ||
                codeRoom_textBox.Text.Trim() == "")
            {
                join_btn.Enabled = false;
            }
            else
            {
                join_btn.Enabled = true;
                if (e.KeyValue == 13)
                {
                    JoinRoom();
                }
            }
        }

        private void hostIP_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (hostIP_textBox.Text.Trim() == "IP address..." ||
                hostIP_textBox.Text.Trim() == "")
            {
                room_btn.Enabled = false;
            }
            else
            {
                room_btn.Enabled = true;
                if (e.KeyValue == 13)
                {
                    CreateRoom();
                }
            }
        }

        private void hostIP_textBox_Click(object sender, EventArgs e)
        {
            if (hostIP_textBox.Text.Trim() == "IP address...")
            {
                hostIP_textBox.Clear();
                room_btn.Enabled = false;
            }
        }

        private void codeRoom_textBox_Click(object sender, EventArgs e)
        {
            if (codeRoom_textBox.Text.Trim() == "Code...")
            {
                codeRoom_textBox.Clear();
                join_btn.Enabled = false;
            }
        }

        public static bool checkIP(string str)
        {
            string[] arr = str.Split('.');
            if (arr.Length != 4)
                return false;
            foreach (string sub in arr)
            {
                try
                {
                    int test = int.Parse(sub);
                    if (test < 0 || test > 255)
                        return false;

                    if (test.ToString().Length != sub.Length)
                        return false;
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static bool checkCode(string code)
        {
            if (code.Length != 8) return false;
            for (int i = 0; i < code.Length; i++)
                if (code[i] < 97 || code[i] > 122)
                    return false;
            return true;
        }

        private void hostIP_textBox_Leave(object sender, EventArgs e)
        {
            string ip = hostIP_textBox.Text.Trim();
            if (!checkIP(ip))
            {
                hostIP_textBox.Text = "IP address...";

            }
        }

        private void codeRoom_textBox_Leave(object sender, EventArgs e)
        {
            string code = codeRoom_textBox.Text.Trim();
            if (!checkCode(code))
            {
                codeRoom_textBox.Text = "Code...";
            }
        }
    }
}
