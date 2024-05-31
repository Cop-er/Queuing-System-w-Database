using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClientReceiving
{
    public partial class ReceivingPrint : Form
    {
        public ReceivingPrint()
        {
            InitializeComponent();
        }

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

        private static string tType;
        private static object tFinal;

        private static string handlingPerson;


        private static void codingName(string choiceOf)
        {
            if (choiceOf == "button1")
            {
                handlingPerson = "D";
            }
            else if (choiceOf == "button2")
            {
                handlingPerson = "J";
            }
            else if (choiceOf == "button3")
            {
                handlingPerson = "H";
            }
            else if (choiceOf == "button4")
            {
                handlingPerson = "I";
            }
            else if (choiceOf == "button5")
            {
                handlingPerson = "N";
            }
            else if (choiceOf == "button6")
            {
                handlingPerson = "F";
            }
            else if (choiceOf == "button7")
            {
                handlingPerson = "D";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            incrementNumber(1);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void UpdatedDataNow ()
        {
            button1.Tag = t1.ToString();
            button2.Tag = t2.ToString();
            button3.Tag = t3.ToString();
            button4.Tag = t4.ToString();
            button5.Tag = t5.ToString();
            button6.Tag = t6.ToString();
            button7.Tag = t7.ToString();
        }

        private static void printRDLC(object sender)
        {

            if (sender is Button xmen)
            { 
            tType = xmen.Text;
            tFinal = xmen.Tag;
                codingName(xmen.Name);


                Console.WriteLine(xmen.Name);

                PrintingData pd = new PrintingData();
                pd.Type = $"{tType}";
                pd.valx = $"{handlingPerson}{tFinal.ToString()}";
            pd.ShowDialog();

            }
        }

        private void printNumbering()
        {
            printPreviewDialog1.Document = printDocument1;
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 285, 200);
            printPreviewDialog1.ShowDialog();
        }

        private static void clickBot(object sender)
        {
            if (sender is Button x)
            {
                tType = x.Text;
                tFinal = x.Tag;
                MessageBox.Show($"{tType} {tFinal}");
            }
        }


        private async void incrementNumber(int index)
        {
            try
            {
                string labelName = "label" + index;
                string counterName = "t" + index;
                string tableNumber = "Table " + index;

                var counterField = this.GetType().GetField(counterName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                int counterValue = (int)counterField.GetValue(this);
                counterValue++;
                counterField.SetValue(this, counterValue);

                await UpdateData(dt.ToShortDateString());
                Label label = this.Controls.Find(labelName, true).FirstOrDefault() as Label;
                label.Text = $"{tableNumber}   <Total Clients : {counterValue}>";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private async void ReceivingPrint_Load(object sender, EventArgs e)
        {

            label1.Text = "Total = 0";
            label2.Text = "Total = 0";
            label3.Text = "Total = 0";
            label4.Text = "Total = 0";
            label5.Text = "Total = 0";
            label6.Text = "Total = 0";
            label7.Text = "Total = 0";

            try { 
            MongodbConnection con = new MongodbConnection();
            con.ConnectToMongoDB();
            clientDatabase = con.GetClient();
            Database = con.GetDatabase();
            await con.SaveData();
            await RetriveData();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                label1.Text = $"Total = {t1}";
                label2.Text = $"Total = {t2}";
                label3.Text = $"Total = {t3}";
                label4.Text = $"Total = {t4}";
                label5.Text = $"Total = {t5}";
                label6.Text = $"Total = {t6}";
                label7.Text = $"Total = {t7}";
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


            

            return true;
        }

        public async Task UpdateData(string dtx)
        {
            var collection = Database.GetCollection<BsonDocument>(MongodbConnection.CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("DateString", dtx);
            var update = Builders<BsonDocument>.Update
                .Set("EmesilBirth", t1)
                .Set("JomaryDeath", t2)
                .Set("HelenMarriage", t3)
                .Set("NikkiCTC", t4)
                .Set("DonCourt", t5)
                .Set("NikkiLegitimationEdorsementsLegitimation", t6)
                .Set("FrechieCorrection", t7);

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
            



        private void button2_Click(object sender, EventArgs e)
        {
            incrementNumber(2);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            incrementNumber(3);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            incrementNumber(4);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            incrementNumber(5);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            incrementNumber(7);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            incrementNumber(6);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            incrementNumber(5);
            UpdatedDataNow();
            printRDLC(sender);
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString($"{tType}" , new Font("Arial", 12), Brushes.Black, new Point(10,10));
            e.Graphics.DrawString($"{tFinal}" , new Font("Arial", 40), Brushes.Black, new Point(10,30));
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
