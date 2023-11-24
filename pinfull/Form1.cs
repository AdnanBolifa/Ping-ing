using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace pinfull {
    public partial class Form1 : Form {
        
        public Form1() {
            InitializeComponent();
            InitializeTimer();
            LoadServers();
            this.FormClosing += Form1_FormClosing;

        }
        private List<ServerInfo> servers = new List<ServerInfo>();
        private List<Label> serverStatusLabels = new List<Label>();
        private Timer timer;
        private const string ServersFilePath = "servers.txt";

        private void PaintServers(object sender, PaintEventArgs e) {
            // Create a Graphics object from the form
            Graphics g = e.Graphics;

            // Set the maximum number of servers per row
            int serversPerRow = 3;

            // Iterate through the list of servers
            for (int i = 0; i < servers.Count; i++) {
                // Set the rectangle dimensions and position based on the server index
                int rectangleWidth = 200;
                int rectangleHeight = 100;
                int margin = 20; // Margin between rectangles
                int row = i / serversPerRow;
                int col = i % serversPerRow;

                int rectangleX = 50 + col * (rectangleWidth + margin);
                int rectangleY = 50 + row * (rectangleHeight + margin);

                // Create a solid brush with the server's status color
                Brush brush = new SolidBrush(servers[i].StatusColor);

                // Draw the rectangle for each server
                g.FillRectangle(brush, rectangleX, rectangleY, rectangleWidth, rectangleHeight);

                // Set the font and color for text
                Font font = new Font("Arial", 12);
                Brush textBrush = new SolidBrush(Color.White);

                // Set the server information
                string serverName = "Name: " + servers[i].Name;
                string ipAddress = "IP: " + servers[i].Ip;
                string location = "Location: " + servers[i].Location;

                // Draw text inside the rectangle for each server
                g.DrawString(serverName, font, textBrush, rectangleX + 10, rectangleY + 10);
                g.DrawString(ipAddress, font, textBrush, rectangleX + 10, rectangleY + 30);
                g.DrawString(location, font, textBrush, rectangleX + 10, rectangleY + 50);

                // Display ping time if available
                if (servers[i].PingTime >= 0) {
                    string pingTimeText = $"Ping: {servers[i].PingTime}ms";
                    g.DrawString(pingTimeText, font, textBrush, rectangleX + 10, rectangleY + 70);
                }
            }
        }
        private void LoadServers() {
            if (File.Exists(ServersFilePath)) {
                try {
                    string[] lines = File.ReadAllLines(ServersFilePath);

                    foreach (string line in lines) {
                        string[] parts = line.Split(',');

                        if (parts.Length == 4) {
                            ServerInfo server = new ServerInfo {
                                Name = parts[0],
                                Ip = parts[1],
                                Description = parts[2],
                                Location = parts[3]
                            };

                            servers.Add(server);
                            //AddServerToUI(server);
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show($"Error loading servers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void SaveServers() {
            try {
                using (StreamWriter writer = new StreamWriter(ServersFilePath)) {
                    foreach (ServerInfo server in servers) {
                        writer.WriteLine($"{server.Name},{server.Ip},{server.Description},{server.Location}");
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Error saving servers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            SaveServers();
        }
        
        private void InitializeTimer() {
            timer = new Timer();
            timer.Interval = 2000; // 2 seconds
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        int ping = 0;
        private void Timer_Tick(object sender, EventArgs e) {
            foreach (var server in servers) {
                if (PingServer(server.Ip, out long pingTime)) {
                    // Server responded, set color to green
                    Console.WriteLine($"Pinged! Time: {pingTime}ms");
                    server.StatusColor = Color.Green;
                    server.PingTime = pingTime;
                } else {
                    // Server didn't respond, set color to red
                    server.StatusColor = Color.Red;
                    server.PingTime = -1; // Indicate no ping
                }
            }

            // Increment the ping counter
            png.Text = ping++.ToString();

            // Trigger repainting of the form
            Invalidate();
        }

        private bool PingServer(string address, out long pingTime) {
            try {
                Ping ping = new Ping();
                PingReply reply = ping.Send(address, 5000); // 5000 ms timeout

                if (reply != null && reply.Status == IPStatus.Success) {
                    // Server responded
                    pingTime = reply.RoundtripTime;
                    return true;
                } else {
                    // Server did not respond
                    pingTime = -1; // Set to indicate no ping
                    return false;
                }
            } catch (Exception) {
                // An error occurred (e.g., invalid address)
                pingTime = -1; // Set to indicate no ping
                return false;
            }
        }


        private void add_Click(object sender, EventArgs e) {
            // Open a form to add a new server
            AddServer addServerForm = new AddServer();
            DialogResult result = addServerForm.ShowDialog();

            // If the user clicks OK, add the server to the list and update the UI
            if (result == DialogResult.OK) {
                ServerInfo newServer = addServerForm.Server;
                servers.Add(newServer);
                //AddServerToUI(newServer);
            }
        }

        private void Form1_Load_1(object sender, EventArgs e) {
            this.Paint += new PaintEventHandler(PaintServers);
        }
    }
}
