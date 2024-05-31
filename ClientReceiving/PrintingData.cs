using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientReceiving
{
    public partial class PrintingData : Form
    {

        public string Type;
        public string valx;
        public DateTime dt = DateTime.Now;

        public PrintingData()
        {
            InitializeComponent();
        }

        private void Printing_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
            printRDLC();
        }
        private void printRDLC()
        {
            List<ReportParameter> reportParameters = new List<ReportParameter>();

            ReportParameter param1 = new ReportParameter("TypeOfEntry", Type);
            ReportParameter param2 = new ReportParameter("TotalValue", valx);
            ReportParameter param3 = new ReportParameter("DateNow", dt.ToString());
            reportParameters.Add(param1);
            reportParameters.Add(param2);
            reportParameters.Add(param3);

            reportViewer1.LocalReport.SetParameters(reportParameters);
            reportViewer1.RefreshReport();

        }
    }
}
