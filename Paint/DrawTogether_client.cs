using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Paint
{
    public partial class DrawTogether_client : Form
    {
        int cursor = 0;
        // Menu form
        private Menu menuForm;
        private string roomCode;

        // User variables
        private User user;
        private string displayName;

        // Graphic variables
        private Point current = new Point();
        private Point old = new Point();
        private Pen p = new Pen(Color.Black, 5);
        private ColorDialog cd = new ColorDialog();
        public Graphics g;
        public Bitmap bm;

        // Shape
        private Rectangle rect = new Rectangle();
        private Point triangleAnchor = new Point();
        private Point triangleVertex = new Point();

        private int initialX;
        private int initialY;

        private toolsManagement tools = new toolsManagement();


        public DrawTogether_client(string displayName, string hostIP, string roomCode, Menu menuForm)
        {
            InitializeComponent();
            this.menuForm = menuForm;
            this.roomCode = roomCode;
            this.displayName = displayName;

            userName_label.Text = $"Name: {displayName}\n";
            user = new User(displayName, this);
            lblroomCode.Text = roomCode;
            
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/Room?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("Room");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Room");

            string code = lblroomCode.Text.Trim();

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("code", code);
            var docsCount = collection.Find(filter).CountDocuments();

            if (docsCount > 0)
            {

            }
            

            CheckForIllegalCrossThreadCalls = false;
            bm = new Bitmap(drawBoard.Width, drawBoard.Height);
            g = Graphics.FromImage(bm);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            p.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);

            tools.toolList.Add(penTool);
            tools.toolList.Add(eraserTool);
            tools.toolList.Add(fillColorTool);
            tools.toolList.Add(rectangleTool);
            tools.toolList.Add(circleTool);
            tools.toolList.Add(triangleTool);

            tools.activeTool(penTool, ref tools.isPenActive);

            if (user.createConnection(hostIP))
            {
                sendMessage(displayName + '\n');

                string message = $"{DateTime.Now}  {displayName} has joined the room\n";
                sendMessage(message);
                user.sendTextData(3, displayName + "\n");
            }
            else
            {
                MessageBox.Show("Server is offline now, please close this form", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Flood fill color Method
        private void FloodFill(Bitmap bmp, Point pt, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();
            Color targetColor = bmp.GetPixel(pt.X, pt.Y);
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point a = pixels.Pop();
                if (a.X <= bmp.Width && a.X >= 0 &&
                        a.Y <= bmp.Height && a.Y >= 0) //make sure we stay within bounds
                {

                    if (bmp.GetPixel(a.X, a.Y) == targetColor)
                    {
                        bmp.SetPixel(a.X, a.Y, replacementColor);
                        pixels.Push(new Point(a.X - 1, a.Y));
                        pixels.Push(new Point(a.X + 1, a.Y));
                        pixels.Push(new Point(a.X, a.Y - 1));
                        pixels.Push(new Point(a.X, a.Y + 1));
                    }
                }
            }
        }

        // Form event
        private void DrawTogether_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (user != null)
            {
                user.closeConnection();
            }
            menuForm.Show();
        }

        // Event
        private void mess_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                string message = $"{ DateTime.Now }  { displayName } : " + mess_textBox.Text.Trim() + '\n';

                if (message != "")
                {
                    sendMessage(message);
                    mess_textBox.Clear();
                }
            }
        }

        private void drawBoard_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                current = e.Location;

                if (tools.isPenActive)
                {
                    g.DrawLine(p, old, current);
                    old = current;
                }
                else if (tools.isRectangleActive || tools.isCircleActive)
                {
                    rect = new Rectangle(
                        Math.Min(e.X, initialX),
                        Math.Min(e.Y, initialY),
                        Math.Abs(e.X - initialX),
                        Math.Abs(e.Y - initialY)
                    );
                }
                else if (tools.isTriangleActive)
                {
                    triangleAnchor.Y = e.Y;
                    triangleVertex.X = (e.X + triangleAnchor.X) / 2;
                }

                drawBoard.Image = bm;
            }
        }

        private void drawBoard_MouseDown(object sender, MouseEventArgs e)
        {
            initialX = e.X;
            initialY = e.Y;

            if (tools.isPenActive)
            {
                old = e.Location;
            }
            else if (tools.isFillColorActive)
            {
                FloodFill(bm, e.Location, p.Color);
            }
            else if (tools.isTriangleActive)
            {
                triangleAnchor.X = initialX;
                triangleVertex.Y = initialY;
            }

            if (radioButton1.Checked)
                p.Width = 5;
            else if (radioButton2.Checked)
                p.Width = 7;
            else if (radioButton3.Checked)
                p.Width = 9;
            else if (radioButton4.Checked)
                p.Width = 11;
            else if (radioButton5.Checked)
                p.Width = 13;
        }

        private void drawBoard_MouseUp(object sender, MouseEventArgs e)
        {
            if (tools.isRectangleActive)
            {
                g.DrawRectangle(p, rect);
            }
            else if (tools.isCircleActive)
            {
                g.DrawEllipse(p, rect);
            }
            else if (tools.isTriangleActive)
            {
                g.DrawLine(p, triangleVertex, triangleAnchor);
                g.DrawLine(p, triangleVertex, current);
                g.DrawLine(p, current, triangleAnchor);
            }

            drawBoard.Image = bm;
            sendImage(drawBoard.Image);
        }

        private void colorPicker_Click(object sender, EventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
            {
                p.Color = cd.Color;
            }
        }

        private void clearAll_Click(object sender, EventArgs e)
        {
            drawBoard.Invalidate();
            g.Clear(Color.White);
            sendImage(bm);
        }

        // Chọn Tool
        private void penTool_Click(object sender, EventArgs e)
        {
            cursor = 0;
            p.Color = cd.Color;
            tools.activeTool(penTool, ref tools.isPenActive);
        }

        private void eraserTool_Click(object sender, EventArgs e)
        {
            cursor = 1;
            tools.activeTool(eraserTool, ref tools.isEraserActive);
            tools.isPenActive = true;
            p.Color = Color.White;
        }

        private void fillColorTool_Click(object sender, EventArgs e)
        {
            cursor = 2;
            tools.activeTool(fillColorTool, ref tools.isFillColorActive);
        }

        private void rectangleTool_Click(object sender, EventArgs e)
        {
            cursor = 0;
            p.Color = cd.Color;
            tools.activeTool(rectangleTool, ref tools.isRectangleActive);
        }

        private void circleTool_Click(object sender, EventArgs e)
        {
            cursor = 0;
            p.Color = cd.Color;
            tools.activeTool(circleTool, ref tools.isCircleActive);
        }

        private void triangleTool_Click(object sender, EventArgs e)
        {
            cursor = 0;
            tools.activeTool(triangleTool, ref tools.isTriangleActive);
        }

        public void sendMessage(string str)
        {
            user.sendTextData(1, str);
        }

        public void sendList(string str)
        {
            user.sendTextData(3, str);
        }

        public void sendImage(Image temp)
        {
            user.sendTextData(2);
            user.sendImageData(drawBoard.Image);
        }

        //Comment
        private void Meow_MouseMove(object sender, MouseEventArgs e)
        {
            meo.Text = "Meow~";
        }

        private void Meow_MouseLeave(object sender, EventArgs e)
        {
            meo.Text = "";
        }

        private void sayHi_MouseMove(object sender, MouseEventArgs e)
        {
            Hii.Text = "Hi^^";
        }

        private void sayHi_MouseLeave(object sender, EventArgs e)
        {
            Hii.Text = "";
        }

        private void penTool_MouseLeave(object sender, EventArgs e)
        {
            pen.Text = "";
        }

        private void penTool_MouseMove(object sender, MouseEventArgs e)
        {
            pen.Text = "Pen";
        }

        private void eraserTool_MouseMove(object sender, MouseEventArgs e)
        {
            eraser.Text = "Eraser";
        }

        private void eraserTool_MouseLeave(object sender, EventArgs e)
        {
            eraser.Text = "";
        }

        private void fillColorTool_MouseMove(object sender, MouseEventArgs e)
        {
            fillColor.Text = "Fill Color";
        }

        private void fillColorTool_MouseLeave(object sender, EventArgs e)
        {
            fillColor.Text = "";
        }

        private void colorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            Colpicker.Text = "Colors";
        }

        private void colorPicker_MouseLeave(object sender, EventArgs e)
        {
            Colpicker.Text = "";
        }

        private void rectangleTool_MouseMove(object sender, MouseEventArgs e)
        {
            rectangle.Text = "Rectangle";
        }

        private void rectangleTool_MouseLeave(object sender, EventArgs e)
        {
            rectangle.Text = "";
        }

        private void circleTool_MouseMove(object sender, MouseEventArgs e)
        {
            elipse.Text = "Elipse";
        }

        private void circleTool_MouseLeave(object sender, EventArgs e)
        {
            elipse.Text = "";
        }

        private void triangleTool_MouseMove(object sender, MouseEventArgs e)
        {
            triangle.Text = "Triangle";
        }

        private void triangleTool_MouseLeave(object sender, EventArgs e)
        {
            triangle.Text = "";
        }

        private void clearAll_MouseMove(object sender, MouseEventArgs e)
        {
            Clear.Text = "Clear All";
        }

        private void clearAll_MouseLeave(object sender, EventArgs e)
        {
            Clear.Text = "";
        }

        public Cursor CreateCursor(Bitmap bitmap, Size size)
        {
            bitmap = new Bitmap(bitmap, size);
            return new Cursor(bitmap.GetHicon());
        }

        private void drawBoard_MouseEnter(object sender, EventArgs e)
        {
            if (cursor == 0)
            {
                var bitmap = (Bitmap)Image.FromFile("location.png");
                this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
            }
            else if (cursor == 1)
            {
                var bitmap = (Bitmap)Image.FromFile("eraser.png");
                this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
            }
            else if (cursor == 2)
            {
                var bitmap = (Bitmap)Image.FromFile("paint-bucket.png");
                this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
            }
        }

        private void drawBoard_MouseLeave(object sender, EventArgs e)
        {
            var bitmap = (Bitmap)Image.FromFile("trowel.png");
            this.Cursor = CreateCursor(bitmap, new Size(bitmap.Width - 490, bitmap.Height - 490));
        }

        private void chatBox_Enter(object sender, EventArgs e)
        {
            chatBox.Enabled = false;
            chatBox.Enabled = true;
        }

        private void copyCodeRoom_btn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblroomCode.Text);
        }

        private void DrawTogether_client_Load(object sender, EventArgs e)
        {

        }
    }

    public class User
    {
        private TcpClient tcpClient;
        private NetworkStream ns;
        private BinaryWriter writer;
        private string displayName;
        private DrawTogether_client app;

        public User(string str, DrawTogether_client currentApp)
        {
            displayName = str;
            app = currentApp;
        }

        public bool createConnection(string IP)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse(IP);
            IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 8080);

            try
            {
                tcpClient.Connect(iPEndPoint);
                ns = tcpClient.GetStream();
                writer = new BinaryWriter(ns);

                Thread listenData = new Thread(new ThreadStart(listener));
                listenData.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void sendTextData(int mode, string str = "")
        {
            string sendStr = str.Trim() != "" ? (mode.ToString() + str) : mode.ToString();
            try
            {
                Byte[] data = Encoding.Unicode.GetBytes(sendStr);
                if (ns != null)
                    ns.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void sendImageData(Image temp)
        {
            Image img = temp;

            // Lưu ảnh vào MemoryStream
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // Lấy buffer ảnh
            byte[] buffer = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(buffer, 0, buffer.Length);

            // Gửi độ lớn của buffer
            writer.Write(buffer.Length);

            // Gửi buffer
            writer.Write(buffer);
        }

        public Image getImageData()
        {
            BinaryReader reader = new BinaryReader(this.ns);

            // Lấy độ lớn của buffer
            int ctBytes = reader.ReadInt32();

            // Đưa buffer vào Memorystream
            MemoryStream ms = new MemoryStream(reader.ReadBytes(ctBytes));

            // Lấy ảnh từ Memorystream
            Image img = Image.FromStream(ms);
            return img;
        }

        public void closeConnection()
        {
            if (tcpClient != null)
            {
                string message = $"{ DateTime.Now }  { displayName } has quit the room\n";
                sendTextData(1, message);
                if (ns != null)
                    ns.Close();
                tcpClient.Close();
            }
        }

        public void AddUser(string s)
        {
            app.lviOnlUser.Items.Add(new ListViewItem { Text = s });
        }

        public void AddList (string list)
        {
            string[] user = list.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < user.Length; i++)
            {
                AddUser(user[i]);
            }
        }

        public void listener()
        {
            try
            {
                while (tcpClient.Connected)
                {
                    string text = "";
                    int bytesReceived = 2;
                    Byte[] data = new Byte[2];
                    Byte[] checkRecvData = new Byte[2];

                    bytesReceived = ns.Read(checkRecvData, 0, checkRecvData.Length);
                    string check = Encoding.Unicode.GetString(checkRecvData, 0, bytesReceived);

                    if (check == "1")
                    {
                        do
                        {
                            try
                            {
                                bytesReceived = ns.Read(data, 0, data.Length);
                                text += Encoding.Unicode.GetString(data, 0, bytesReceived);
                            }
                            catch
                            {
                                break;
                            }
                        } while (bytesReceived == 2 && text[text.Length - 1] != '\n');

                        app.chatBox.Text += text;
                    }
                    else if (check == "2")
                    {
                        Image img = getImageData();
                        app.bm = (Bitmap)img;
                        app.g = Graphics.FromImage(app.bm);
                        app.drawBoard.Image = app.bm;
                    }
                    else if (check == "3")
                    {
                        app.lviOnlUser.Items.Clear();
                        do
                        {
                            try
                            {
                                bytesReceived = ns.Read(data, 0, data.Length);
                                text += Encoding.Unicode.GetString(data, 0, bytesReceived);
                            }
                            catch
                            {
                                break;
                            }
                        } while (bytesReceived == 2 && text[text.Length - 1] != '\n');
                        AddList(text);
                    }
                }
            }
            catch
            {
                tcpClient.Close();
            }
        }
    }

}
