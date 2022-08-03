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
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography;

namespace Paint
{
    public partial class DrawTogether_host : Form
    {
        int cursor = 0;
        // Menu form
        private Menu menuForm;
        private string hostIP;
        private string roomCode;

        // Host variables
        private Host host;
        private string displayName;

        // Graphic variables
        private Point current = new Point();
        private Point old = new Point();
        private Pen p = new Pen(Color.Black, 5);
        private ColorDialog cd = new ColorDialog();
        public Graphics g;
        public Bitmap bm;

        // Shape variables
        private Rectangle rect = new Rectangle();
        private Point triangleAnchor = new Point();
        private Point triangleVertex = new Point();

        private int initialX;
        private int initialY;

        protected toolsManagement tools = new toolsManagement();

        public string createCodeRoom()
        {
            // Connect to MongoDB
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/Room?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("Room");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Room");
            string code;

            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Empty;
            long docsCount;

            do
            {
                // Create random 8-byte length for room code
                code = "";
                Random _random = new Random();
                for (int i = 0; i < 8; i++)
                {
                    code += Convert.ToChar(_random.Next(97, 122));
                }

                // Check if this room code is already available
                filter = builder.Eq("code", code);
                docsCount = collection.Find(filter).CountDocuments();
            } while (docsCount == 1);

            // Add to database
            BsonDocument userData = new BsonDocument()
                .Add("ip", hostIP)
                .Add("code", code);
            collection.InsertOne(userData);

            return code;
        }

        void AddUser (string s)
        {
            lviOnlUser.Items.Add(new ListViewItem { Text = s });
        }

        public DrawTogether_host(string displayName, string hostIP, Menu menuForm)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            this.menuForm = menuForm;
            this.hostIP = hostIP;
            this.displayName = displayName;
            userName_label.Text = $"Name: {displayName}\n";

            bm = new Bitmap(drawBoard.Width, drawBoard.Height);
            drawBoard.Image = bm;
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

            try
            {
                host = new Host(this);
                roomCode = createCodeRoom();
                lblroomCode.Text = roomCode;
                AddUser(displayName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DrawTogether_host_FormClosed(object sender, FormClosedEventArgs e)
        {
            MongoClient dbClient = new MongoClient("mongodb+srv://pscarlet89:Puncute123@cluster0.29drf.mongodb.net/Room?retryWrites=true&w=majority");
            IMongoDatabase database = dbClient.GetDatabase("Room");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Room");
            var builder = Builders<BsonDocument>.Filter;
            var filter = builder.Eq("code", roomCode);
            collection.DeleteOne(filter);

            host.closeHost();
            menuForm.Show();
        }

        // Method
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

        // Event
        private void mess_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                string message = mess_textBox.Text.Trim();

                if (message != "")
                {
                    message = $"{DateTime.Now}  {displayName}: {message}\n";
                    host.sendDataAllClients(1, message);
                    chatBox.Text += message;
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
            else if (tools.isFillColorActive)
            {
                FloodFill(bm, e.Location, p.Color);
            }

            drawBoard.Image = bm;
            host.sendImageAllClients(drawBoard.Image);
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
            host.sendImageAllClients(drawBoard.Image);
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

        //Amusement
        private void Meo_MouseMove(object sender, MouseEventArgs e)
        {
            meow.Text = "Meow~";
        }

        private void Meo_MouseLeave(object sender, EventArgs e)
        {
            meow.Text = "";
        }

        private void sayHi_MouseMove(object sender, MouseEventArgs e)
        {
            Hii.Text = "Hi^^";
        }

        private void sayHi_MouseLeave(object sender, EventArgs e)
        {
            Hii.Text = "";
        }

        private void penTool_MouseMove(object sender, MouseEventArgs e)
        {
            pen.Text = "Pen";
        }

        private void penTool_MouseLeave(object sender, EventArgs e)
        {
            pen.Text = "";
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
            fillColor.Text = "Fill Corlor";
        }

        private void fillColorTool_MouseLeave(object sender, EventArgs e)
        {
            fillColor.Text = "";
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

        private void clearAll_MouseMove(object sender, MouseEventArgs e)
        {
            Clear.Text = "Clear All";
        }

        private void clearAll_MouseLeave(object sender, EventArgs e)
        {
            Clear.Text = "";
        }

        private void colorPicker_MouseMove(object sender, MouseEventArgs e)
        {
            Colors.Text = "Colors";
        }

        private void colorPicker_MouseLeave(object sender, EventArgs e)
        {
            Colors.Text = "";
        }

        private void triangleTool_MouseMove(object sender, MouseEventArgs e)
        {
            triangle.Text = "Triangle";
        }

        private void triangleTool_MouseLeave(object sender, EventArgs e)
        {
            triangle.Text = "";
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
    }


    public class toolsManagement
    {
        public bool isPenActive;
        public bool isRectangleActive;
        public bool isCircleActive;
        public bool isTriangleActive;
        public bool isEraserActive;
        public bool isFillColorActive;

        public ArrayList toolList = new ArrayList(10);

        public toolsManagement()
        {
            isPenActive = false;
            isRectangleActive = false;
            isCircleActive = false;
            isTriangleActive = false;
            isEraserActive = false;
            isFillColorActive = false;
        }

        public void deactivateAllTools()
        {
            isPenActive = false;
            isRectangleActive = false;
            isCircleActive = false;
            isTriangleActive = false;
            isEraserActive = false;
            isFillColorActive = false;
        }

        public void activeTool(Panel toolField, ref bool toolStatus)
        {
            deactivateAllTools();
            toolStatus = !toolStatus;
            foreach (Panel tool in toolList)
            {
                tool.BorderStyle = BorderStyle.None;
            }

            toolField.BorderStyle = toolStatus ? BorderStyle.FixedSingle : BorderStyle.None;
        }
    }

    public class Host
    {
        // Host variables
        private Socket listenerSocket = new Socket(
            AddressFamily.InterNetwork, // Trả về họ địa chỉ của địa chỉ IP
            SocketType.Stream,          // Kiểu kết nối socket: dùng luồng Stream để nhận dữ liệu
            ProtocolType.Tcp            // Sử dụng giao thức kết nối TCP
        );
        private IPEndPoint ipepServer = new IPEndPoint(IPAddress.Any, 8080);
        private ArrayList connections = new ArrayList(100);
        private ArrayList networkStreams = new ArrayList(100);
        private DrawTogether_host app;

        public Host(DrawTogether_host currentApp)
        {
            app = currentApp;
            listenerSocket.Bind(ipepServer);
            listenerSocket.Listen(-1);
            for (int i = 0; i < 10; i++)
            {
                createNewListenThread();
            }
        }

        public void closeHost()
        {
            listenerSocket.Close();
        }

        public void createNewListenThread()
        {
            Thread newThread = new Thread(new ThreadStart(listener));
            newThread.IsBackground = true;
            newThread.Start();
        }

        public void sendImageData(NetworkStream ns, Image img)
        {
            BinaryWriter writer = new BinaryWriter(ns);

            // save the image to a MemoryStream
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            // get the image buffer
            byte[] buffer = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(buffer, 0, buffer.Length);

            // write the size of the image buffer
            writer.Write(buffer.Length);

            // write the actual buffer
            writer.Write(buffer);
        }

        public void sendTextData(int mode, Socket clientSocket, string str = "")
        {
            string sendStr = str.Trim() != "" ? (mode.ToString() + str) : mode.ToString();
            Byte[] data = Encoding.Unicode.GetBytes(sendStr);
            clientSocket.Send(data);
        }
            
        public Image getImageData(NetworkStream ns)
        {
            BinaryReader reader = new BinaryReader(ns);

            // read how big the image buffer is
            int ctBytes = reader.ReadInt32();

            // read the image buffer into a MemoryStream
            MemoryStream ms = new MemoryStream(reader.ReadBytes(ctBytes));

            // get the image from the MemoryStream
            Image img = Image.FromStream(ms);
            return img;
        }

        public void sendDataAllClients(int mode, string str = "")
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Socket temp = (Socket)connections[i];
                if (temp.Connected)
                {
                    sendTextData(mode, temp, str);
                }
            }
        }

        public void sendImageAllClients(Image temp)
        {
            sendDataAllClients(2);
            Image img = temp;

            for (int i = 0; i < networkStreams.Count; i++)
            {
                NetworkStream ns = (NetworkStream)networkStreams[i];
                sendImageData(ns, img);
            }
        }

        void AddUser(string name)
        {
            app.lviOnlUser.Items.Add(new ListViewItem { Text = name });
        }

        public void sendList()
        {
            string list = "";
            for (int i = 0; i < app.lviOnlUser.Items.Count; i++)
            {
                char splitChar = (i == app.lviOnlUser.Items.Count - 1) ? ' ' : '@';
                list += app.lviOnlUser.Items[i].Text + splitChar;
            }
            list = list.Replace("\n", "");
            sendDataAllClients(3, list + "\n");
        }

        public void RemoveUser (string user)
        {
            for (int i = 0; i < app.lviOnlUser.Items.Count; i++)
            {
                if (app.lviOnlUser.Items[i].Text == user)
                    app.lviOnlUser.Items[i].Remove();
            }
        }

        public void listener()
        {
            string displayName = "";
            int bytesReceived;
            Byte[] checkRecvData = new Byte[2];
            Byte[] recv = new Byte[2];

            // Nhận dữ liệu gửi về từ client
            try
            {
                Socket clientSocket = listenerSocket.Accept();
                NetworkStream ns = new NetworkStream(clientSocket);
                connections.Add(clientSocket);
                networkStreams.Add(ns);

                sendTextData(2, clientSocket);
                sendImageData(ns, app.drawBoard.Image);

                if (clientSocket.Connected)
                {
                    do
                    {
                        try
                        {
                            bytesReceived = clientSocket.Receive(recv);
                            displayName += Encoding.Unicode.GetString(recv);
                        }
                        catch
                        {
                            break;
                        }
                    } while (bytesReceived == 2 && displayName[displayName.Length - 1] != '\n');

                    displayName = displayName.Substring(1, displayName.Length - 2);
                }

                while (clientSocket.Connected)
                {
                    string text = "";

                    bytesReceived = clientSocket.Receive(checkRecvData);
                    string check = Encoding.Unicode.GetString(checkRecvData);

                    if (check == "1")
                    {
                        do
                        {
                            try
                            {
                                bytesReceived = clientSocket.Receive(recv);
                                text += Encoding.Unicode.GetString(recv);
                            }
                            catch
                            {
                                break;
                            }
                        } while (bytesReceived == 2 && text[text.Length - 1] != '\n');

                        app.chatBox.Text += text;
                        sendDataAllClients(1, text);

                        if (text.Contains($"{ displayName } has quit the room\n"))
                        {
                            RemoveUser($"{displayName}");
                            break;
                        }
                    }
                    else if (check == "2")
                    {
                        Image img = getImageData(ns);
                        sendImageAllClients(img);
                        app.bm = (Bitmap)img;
                        app.g = Graphics.FromImage(app.bm);
                        app.drawBoard.Image = app.bm;
                    }
                    else if (check == "3")
                    {
                        do
                        {
                            try
                            {
                                bytesReceived = clientSocket.Receive(recv);
                                text += Encoding.Unicode.GetString(recv);
                            }
                            catch
                            {
                                break;
                            }
                        } while (bytesReceived == 2 && text[text.Length - 1] != '\n');

                        text = text.Replace("\n", "");

                        AddUser(text);
                        sendList();
                    }
                }

                connections.Remove(clientSocket);
                networkStreams.Remove(ns);
                ns.Close();
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Disconnect(true);

                if (connections.Count > 0)
                {
                    sendList();
                }

                if (listenerSocket.Connected)
                {
                    createNewListenThread();
                }
            }
            catch
            {
                if (listenerSocket.Connected)
                {
                    createNewListenThread();
                }
            }
        }
    }
}
