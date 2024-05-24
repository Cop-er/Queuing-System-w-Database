using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTCP;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Queu_Numbers
{
    public partial class Form1 : Form
    {
        login logs;
        public Form1(login lgn)
        {
            InitializeComponent();
            this.logs = lgn;
        }

        private string ipAddress = "192.168.4.103";
        private string portNumber = "8910";
        private string c1 = "";
        private string limiter = "";

        private MongoClient clientDatabase;
        private IMongoDatabase Database;
        private DateTime dt = DateTime.Now;

        private int t1 = 0;
        private int t2 = 0;
        private int t3 = 0;
        private int t4 = 0;
        private int t5 = 0;
        private int t6 = 0;
        private int t7 = 0;

        SimpleTcpClient client;

        private async void button1_Click(object sender, EventArgs e)
        {
            var x = await RetriveData();
            if (x)
            {
                passData("add");
                x = false;
            }
        }

        private async void passData(string nk)
        {
            try
            {
                if (int.TryParse(label1.Text, out int number))
                {
                    foreach (Control control in tableLayoutPanel3.Controls)
                    {
                        if (control is RadioButton radioButton)
                        {
                            if (radioButton.Checked)
                            {
                                limiter = radioButton.Tag.ToString();
                            }
                        }
                    }

                    int limiterx = int.Parse(limiter);
                    if (number <= limiterx)
                    {


                        label1.Text = "***";
                        this.Enabled = false;
                        await Task.Delay(1000);

                        int incrementedNumber = number;
                        if (nk == "add")
                        {
                            incrementedNumber = number + 1;
                        }
                        else if (nk == "sub")
                        {
                            incrementedNumber = number - 1;
                        }

                        scanRadio();

                        string message = $"{c1}:" + incrementedNumber.ToString();

                        client.WriteLineAndGetReply(message, TimeSpan.FromSeconds(2));
                        await Task.Delay(8000);
                        label1.Text = incrementedNumber.ToString();


                    }
                    else
                    {
                        label1.Text = number.ToString();
                        MessageBox.Show("wew");
                    }
                }
                else
                {
                    textBox1.Text += "Error: Invalid number format in label1" + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += $"Error: {ex.Message}" + Environment.NewLine;
            }
            finally
            {
                this.Enabled = true;
            }
        }


        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            try { 
            string message = e.MessageString.Trim((char)client.Delimiter);
            textBox1.Invoke((MethodInvoker)delegate
            {
                textBox1.AppendText(message + Environment.NewLine);
            });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            
        }

        private void startConnect()
        {
            try
            {
                int port = Convert.ToInt32(portNumber);
                client.Connect(ipAddress, port);
            }
            catch
            {
                MessageBox.Show($"Error: Queuing Main System is currently not running" + Environment.NewLine);
                //this.Close();
            }
        }

        private void limits()
        {
            string vl = this.Text;
            switch (vl)
            {
                case "FLOI":
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                    radioButton3.Enabled = true;
                    radioButton4.Enabled = true;
                    radioButton5.Enabled = true;
                    radioButton6.Enabled = true;
                    radioButton7.Enabled = true;
                    radioButton1.Checked = true;
                    break;
                case "EMESIL":
                    radioButton1.Enabled = true;
                    radioButton1.Checked = true;
                    break;
                case "JOMARY":
                    radioButton2.Enabled = true;
                    radioButton4.Enabled = true;
                    radioButton4.Checked = true;
                    break;
                case "HELEN":
                    radioButton3.Enabled = true;
                    radioButton3.Checked = true;
                    break;
                case "NIKKI":
                    radioButton4.Enabled = true;
                    radioButton6.Enabled = true;
                    radioButton4.Checked = true;
                    break;
                case "DON":
                    radioButton5.Enabled = true;
                    radioButton5.Checked = true;
                    break;
                case "FRECHIE":
                    radioButton7.Enabled = true;
                    radioButton7.Checked = true;
                    break;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Text = logs.textBox1.Text.ToUpper();

            MongodbConnection con = new MongodbConnection();
            con.ConnectToMongoDB();
            clientDatabase = con.GetClient();
            Database = con.GetDatabase();
            await con.SaveData();
            await RetriveData();

            limits();

            try
            {
                //radioButton1.Checked = true;
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
            startConnect();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }


        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var x = await RetriveData();
            if (x)
            {
                passData("sub");
                x = false;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var x = await RetriveData();
            if (x)
            {
                passData("");
                x = false;
            }
        }

        private async void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t1.ToString()}";
        }

        private void scanRadio ()
        {

            TableLayoutPanel tableLayoutPanel3 = null;
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is TableLayoutPanel panel && control.Name == "tableLayoutPanel3")
                {
                    tableLayoutPanel3 = panel;
                    break;
                }
            }
            if (tableLayoutPanel3 != null)
            {
                foreach (Control control in tableLayoutPanel3.Controls)
                {
                    if (control is RadioButton radioButton && radioButton.Checked)
                    {
                        string selectedText = radioButton.Text;
                        c1 = selectedText;
                        return;
                    }
                }
            }
        }

        public async Task<bool> RetriveData()
        {
            var collection = Database.GetCollection<BsonDocument>(MongodbConnection.CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dt.ToShortDateString());
            var document = await collection.Find(filter).FirstOrDefaultAsync();

            t1 = document.GetValue("EmesilBirth", "").AsInt32;
            t2 = document.GetValue("JomaryDeath", "").AsInt32;
            t3 = document.GetValue("HelenMarriage", "").AsInt32;
            t4 = document.GetValue("NikkiCTC", "").AsInt32;
            t5 = document.GetValue("DonCourt", "").AsInt32;
            t6 = document.GetValue("NikkiLegitimationEdorsementsLegitimation", "").AsInt32;
            t7 = document.GetValue("FrechieCorrection", "").AsInt32;

            radioButton1.Tag = t1;
            radioButton2.Tag = t2;
            radioButton3.Tag = t3;
            radioButton4.Tag = t4;
            radioButton5.Tag = t5;
            radioButton6.Tag = t6;
            radioButton7.Tag = t7;

            return true;
        }

        private async void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t2.ToString()}";


        }

        private async void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t3.ToString()}";

        }

        private async void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t4.ToString()}";

        }

        private async void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t5.ToString()}";

        }

        private async void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t6.ToString()}";

        }

        private async void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            await RetriveData();
            label2.Text = $"Logs and Maximum Client Number: {t7.ToString()}";

        }
    }
}
