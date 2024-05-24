using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using SimpleTCP;
using MongoDB.Driver;
using MongoDB.Bson;


namespace Queuing_System
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private string ipAddress = "192.168.4.103";
        private string portNumber = "8910";
        private MongoClient client;
        private IMongoDatabase Database;
        private DateTime dt = DateTime.Now;
        SimpleTcpServer server;

        private async void Main_Load(object sender, EventArgs e)
        {
            mongodb_connection con = new mongodb_connection();
            con.ConnectToMongoDB();
            client = con.GetClient();
            Database = con.GetDatabase();

            server = new SimpleTcpServer();
            server.Delimiter = 0x13;
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;

            axWindowsMediaPlayer1.URL = @"C:\Users\Administrator\Downloads\Video\LeAnn Rimes - I Need You (Official Music Video).mp4";
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.settings.autoStart = true;
            axWindowsMediaPlayer1.settings.setMode("loop", true);

            clearButton();
            StartServer();
            axWindowsMediaPlayer1.Focus();

            await con.SaveData();
            await RetriveData();
        }

        public async Task RetriveData()
        {
            var collection = Database.GetCollection<BsonDocument>(mongodb_connection.CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt.ToShortDateString());
            var document = await collection.Find(filter).FirstOrDefaultAsync();

            t1.Text = document.GetValue("EmesilBirth", "").AsString;
            t2.Text = document.GetValue("JomaryDeath", "").AsString;
            t3.Text = document.GetValue("HelenMarriage", "").AsString;
            t4.Text = document.GetValue("NikkiCTC", "").AsString;
            t5.Text = document.GetValue("DonCourt", "").AsString;
            t6.Text = document.GetValue("NikkiLegitimationEdorsementsLegitimation", "").AsString;
            t7.Text = document.GetValue("FrechieCorrection", "").AsString;
        }

        public async Task UpdateData(string dt)
        {
                var collection = Database.GetCollection<BsonDocument>(mongodb_connection.CollectionName);
                var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt);
                var update = Builders<BsonDocument>.Update
                    .Set("EmesilBirth", t1.Text)
                    .Set("JomaryDeath", t2.Text)
                    .Set("HelenMarriage", t3.Text)
                    .Set("NikkiCTC", t4.Text)
                    .Set("DonCourt", t5.Text)
                    .Set("NikkiLegitimationEdorsementsLegitimation", t6.Text)
                    .Set("FrechieCorrection", t7.Text);

            var result = await collection.UpdateOneAsync(filter, update);
                if (result.IsAcknowledged && result.ModifiedCount > 0)
                {
                    Console.WriteLine("Document updated successfully.");
                }
                else
                {
                    Console.WriteLine("No documents matched the filter or no modifications were made.");
                }
            
        }

        private void clearButton()
        {
            t1.Text = "";
            t2.Text = "";
            t3.Text = "";
            t4.Text = "";
            t5.Text = "";
            t6.Text = "";
            t7.Text = "";

        }

        private void StartServer()
        {
            try
            {
                //t7.Text += "Server Starting ..." + Environment.NewLine;
                //MessageBox.Show("Server Starting");
                var ip = System.Net.IPAddress.Parse(ipAddress);
                server.Start(ip, Convert.ToInt32(portNumber));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            string message = e.MessageString.Trim((char)server.Delimiter);
            string[] parts = message.Split(new char[] { ':' }, 2);

            if (parts.Length == 2)
            {
                string target = parts[0].Trim();
                string content = parts[1].Trim();

                this.Invoke((MethodInvoker)delegate
                {
                    switch (target)
                    {
                        case "Birth":
                            t1.Text = "";
                            t1.Text = content;
                            break;
                        case "Death":
                            t2.Text = "";
                            t2.Text = content;
                            break;
                        case "Marriage":
                            t3.Text = "";
                            t3.Text = content;
                            break;
                        case "CTC or Negative":
                            t4.Text = "";
                            t4.Text = content;
                            break;
                        case "Court":
                            t5.Text = "";
                            t5.Text = content;
                            break;
                        case "Legitimation, Endorsements, etc":
                            t6.Text = "";
                            t6.Text = content;
                            break;
                        case "Correction":
                            t7.Text = "";
                            t7.Text = content;
                            break;
                        //default:
                        //    t7.Text = "";
                        //    t7.Text = ($"Unknown target: {target}");
                        //    break;
                    }
                });

                e.ReplyLine($"Message received for {target}: {content}");
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    t7.AppendText("Invalid message format: " + message + Environment.NewLine);
                });

                e.ReplyLine("Invalid message format.");
            }
        }

        private void t1_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q1.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void callNumber(string x1, string x2)
        {
            if (x2 != "") { 
            SpeechSynthesizer _ss = new SpeechSynthesizer();
            axWindowsMediaPlayer1.Ctlcontrols.pause();
            for (int i = 1; i<3; i++) { 
            string text = $"Client Number {x2}! Please Proceed to {x1}";
            _ss.Speak(text);
            }
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.Focus();
            }
        }

        private void t2_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q2.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void t3_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q3.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void t4_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q4.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void t5_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q5.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void t6_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q6.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void t7_TextChanged(object sender, EventArgs e)
        {
            TextBox typeText = sender as TextBox;
            string tableName = q7.Text;
            string numberText = typeText.Text;
            callNumber(tableName, numberText);
        }

        private void label15_DoubleClick(object sender, EventArgs e)
        {
            mongodb_connection con = new mongodb_connection();
            con.ConnectToMongoDB();

            var result = MessageBox.Show("Are you sure you want to close the application?", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (result == DialogResult.OK)
            {
                this.Close();
            }
        }

        private async void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            await UpdateData(dt.ToShortDateString());
        }
    }
}
