using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
namespace PDF2DT
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class PDF2DT : UserControl
    {
        public DataTable DT { get; private set; }
        public delegate void PostDataTable(DataTable dt);
        public event PostDataTable postDataTable;
        public delegate void FormCloseEventHandler(object sender, EventArgs e);//声明委托
        public event FormCloseEventHandler FormClose;//声明事件
        public PDF2DT()
        {
            InitializeComponent();
        }

        DataTable dt;
        public string readPDFfile(string fileName)
        {
            StringBuilder text = new StringBuilder();
            PdfReader PDFreader = new PdfReader(fileName);
            List<string> pagelist=new List<string>();            
            for (int page = 1; page <= PDFreader.NumberOfPages; page++)
            {
                ITextExtractionStrategy extract=new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(PDFreader, page, extract);                
                string byteText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));                
                text.Append(byteText);                
            }
            PDFreader.Close();        
            return text.ToString();
        }
        private void splitStr(DataTable dt,string str)
        {
            DataRow dr = dt.NewRow();
            foreach (RegStr rr in regValue(str))
            {                
                switch(rr.FieldName)
                {
                    case "invDate":
                        dr["开票日期"] = rr.FieldValue.Trim();
                        break;
                    case "invNo":
                        dr["发票号码"] = rr.FieldValue.Trim();
                        break;
                    case "invLineNo":
                        dr["序号"] = rr.FieldValue.Trim();
                        break;
                    case "invKind":
                        dr["发票种类"] = rr.FieldValue.Trim();
                        break;
                    case "invClient":
                        dr["购方名称"] = rr.FieldValue.Trim();
                        break;
                    case "invItem":
                        dr["主要商品名称"] = rr.FieldValue.Trim();
                        break;
                    case "invTaxRate":
                        dr["税率"] = rr.FieldValue.Trim();
                        break;
                    case "invTaxAmount":
                        dr["合计税额"] = rr.FieldValue.Trim();
                        break;
                    case "invYNcancle":
                        dr["是否作废"] = rr.FieldValue.Trim();
                        break;
                }
            }
            dt.Rows.Add(dr);
        }
        private void browse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".pdf";
            ofd.Filter = "pdf file|*.pdf";
            if (ofd.ShowDialog() == true)
            {
                filePath.Text = ofd.FileName;
            }
        }
        private void read_Click(object sender, RoutedEventArgs e)
        {
            string text;
            if (filePath.Text.Trim() != string.Empty)
            {
                text = readPDFfile(filePath.Text.Trim());
            }
            else
                return;
            StringReader sr = new StringReader(text);
            string tempStr;
            dt = excelTable();
            while ((tempStr = sr.ReadLine()) !=null )
            {               
                if (tempStr.Contains("普通发票"))
                {
                    splitStr(dt, tempStr);
                }
                else if (tempStr.Contains("专用发票"))
                {
                    splitStr(dt, tempStr);
                }
            }
            DT = this.dt;
            if (this.postDataTable != null)
            {
                postDataTable(this.dt);
            }
            if (this.FormClose != null)
            {
                FormClose(this, new EventArgs());
            }
        }
        private DataTable excelTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("序号",typeof(int));
            dt.Columns.Add("发票种类", typeof(string));
            dt.Columns.Add("发票号码", typeof(string));
            dt.Columns.Add("购方名称", typeof(string));
            dt.Columns.Add("开票日期", typeof(string));
            dt.Columns.Add("主要商品名称", typeof(string));
            //dt.Columns.Add("税率", typeof(string));
            dt.Columns.Add("合计税额", typeof(double));
            dt.Columns.Add("是否作废", typeof(string));
            return dt;
        }
        private List<RegStr> makeReglist()
        {     
            List<RegStr> mk=new List<RegStr>();
            RegStr rs1 = new RegStr() { FieldName="invDate",FieldValue= @"\d{4}[\/-]\d{1,2}[\/-]\d{1,2}"};
            RegStr rs2 = new RegStr() { FieldName="invNo",FieldValue= @"\d{8}"};
            RegStr rs3 = new RegStr() { FieldName="invLineNo", FieldValue=@"\s\d+\s"};
            RegStr rs4 = new RegStr() { FieldName="invKind",FieldValue= @"(普通发票|专用发票)"};
            RegStr rs5 = new RegStr() { FieldName = "invClient", FieldValue = @"(?<=\d{8}\s).+(?=\s*\d{4}\/\d{1,2}\/\d{1,2})" };
            RegStr rs6 = new RegStr() { FieldName = "invItem", FieldValue = @"(?<=\d{4}\/\d+\/\d+\s+|\d{4}\-\d+\-\d+\s+)\w+" };
            //RegStr rs7 = new RegStr() { FieldName = "invTaxRate", FieldValue = @"\w+\%" };
            RegStr rs8 = new RegStr() { FieldName = "invTaxAmount", FieldValue = @"(?<=\w+\%\s+)\-*\d+\.\d+" };
            RegStr rs9 = new RegStr() { FieldName = "invYNcancle", FieldValue = @"\w$" };            
            mk.Add(rs1);
            mk.Add(rs2);
            mk.Add(rs3);
            mk.Add(rs4);
            mk.Add(rs5);
            mk.Add(rs6);
            //mk.Add(rs7);
            mk.Add(rs8);
            mk.Add(rs9);
            return mk;                      
        }
        private List<RegStr> regValue(string str)
        {
            List<RegStr> regList = makeReglist();
            List<RegStr> regResult = new List<RegStr>();
            foreach(RegStr x in regList)
            {
                Regex reg=new Regex(x.FieldValue);
                Match match = reg.Match(str);
                RegStr rr = new RegStr() { FieldName = x.FieldName, FieldValue = match.Value };
                regResult.Add(rr);
            }
            return regResult;
        }

    }
    internal class RegStr
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
    
}
