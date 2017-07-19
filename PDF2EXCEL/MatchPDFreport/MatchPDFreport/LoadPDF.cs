using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using PDF2DT;
using System.Windows.Controls;


namespace MatchPDFreport
{
    
    public partial class LoadPDF : Form
    {
        private ElementHost ctrlHost;
        private PDF2DT.PDF2DT Pdf2DT;
        public delegate void GetDataTable(DataTable dt);
        public event GetDataTable GDT;

        public LoadPDF()
        {
            InitializeComponent();
               
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ctrlHost = new ElementHost();
            ctrlHost.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(ctrlHost);
            Pdf2DT = new PDF2DT.PDF2DT();
            Pdf2DT.InitializeComponent();
            ctrlHost.Child = Pdf2DT;
            DataTable dt = new DataTable();
            Pdf2DT.postDataTable += WriteDataTable;
            Pdf2DT.FormClose += userControl_FormClose;

        }
        private void WriteDataTable(DataTable dt)
        {

            if (GDT != null)
            {
                GDT(dt);
            }

        }
        private void userControl_FormClose(object sender, EventArgs e)
        {
            this.Close();            
        }
    }
}
