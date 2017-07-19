using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Markup;
using System.Windows.Controls;
using Microsoft.Office.Core;
using Excel=Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace MatchPDFreport
{
    public partial class PDFMapping
    {
        public void MatchPDF_Load(object sender, RibbonUIEventArgs e)
        {

        }
        private void BrowsePDF_Click(object sender, RibbonControlEventArgs e)
        {
            LoadPDF fm = new LoadPDF();
            WorkFlow wf = new WorkFlow();
            fm.GDT += wf.InputExcel;
            fm.ShowDialog();                     
        }

        private void btnDBset_Click(object sender, RibbonControlEventArgs e)
        {
            DBsetting db = new DBsetting();
            db.ShowDialog();
        }
    }
}
