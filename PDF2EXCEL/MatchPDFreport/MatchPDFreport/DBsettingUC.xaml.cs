using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Text.RegularExpressions;

namespace MatchPDFreport
{
    /// <summary>
    /// DBsettingUC.xaml 的交互逻辑
    /// </summary>
    public partial class DBsettingUC : UserControl
    {
        const string conName="con";
        public DBsettingUC()
        {
            InitializeComponent();
            initialInterface();
        }
        public delegate void FormCloseEventHandler(object sender,EventArgs e);//声明委托
        public event FormCloseEventHandler FormClose;//声明事件
        private void initialInterface()//初始化文本框值
        {            
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (cfg.ConnectionStrings.ConnectionStrings[conName] != null)
            {
                string conStr = cfg.ConnectionStrings.ConnectionStrings[conName].ToString();
                string conProvider = cfg.ConnectionStrings.ConnectionStrings[conName].ProviderName;
                Regex reg = new Regex(@"\w*");
                switch (conProvider)
                {
                    case "System.Data.SqlClient":
                        reg = new Regex(@"(?<=Data Source=)\d+\.\d+\.\d+\.\d+");//正则取出connectionstring中的IP地址
                        TBmssqlIP.Text = reg.Match(conStr).Value;
                        reg = new Regex(@"(?<=,)\d+");
                        TBmssqlPort.Text = reg.Match(conStr).Value;
                        reg = new Regex(@"(?<=User ID=)\w+");
                        TBmssqlUN.Text = reg.Match(conStr).Value;
                        reg = new Regex(@"(?<=Password=)\w+");
                        TBmssqlPW.Text = reg.Match(conStr).Value;
                        reg = new Regex(@"(?<=Initial Catalog=)\w+");
                        TBmssqlName.Text = reg.Match(conStr).Value;
                        break;
                    case "Microsoft.Jet.OLEDB.4.0":
                        reg = new Regex(@"(?<=Data Source=).*(?=;Jet OLEDB)");
                        TBaccesspath.Text = reg.Match(conStr).Value;
                        if (string.IsNullOrEmpty(TBaccesspath.Text))
                        {
                            reg = new Regex(@"(?<=Data Source=).*(?=;User Id=admin)");
                            TBaccesspath.Text = reg.Match(conStr).Value;
                        }
                        reg = new Regex(@"(?<=;Jet OLEDB:Database Password=)\w*");
                        TBaccessPW.Text = reg.Match(conStr).Value;
                        break;
                }
            }
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".mdb";
            ofd.Filter = "mdb file|*.mdb";
            if (ofd.ShowDialog() == true)
            {
                TBaccesspath.Text = ofd.FileName;
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {                        
            string conProvider;
            string conUserName;
            string conPassword=string.Empty;
            string conIP;
            string conPort;
            string conDBName;
            string conStr=string.Empty;          
            if (RBmdb.IsChecked.Value)
            {
                conProvider = "Microsoft.Jet.OLEDB.4.0";
                conStr = "Provider=" + conProvider;
                if (CheckEntryEmpty(TBaccesspath, "数据库地址不能为空！"))
                {
                    return;
                }
                else
                {
                    conIP = ";Data Source=" + TBaccesspath.Text.Trim();
                    conStr += conIP;
                }
                if (!string.IsNullOrEmpty(TBaccessPW.Text.Trim()))
                {
                    conPassword = ";Jet OLEDB:Database Password=" + TBaccessPW.Text.Trim()+";";
                    conStr += conPassword;
                }
                else
                {
                    conStr += ";User Id=admin;Password=;";
                }
                //conStr = "Provider=" + conProvider + ";Data Source=" + conIP + ";Jet OLEDB:Database Password=" + conPassword + ";";
            }
            else
            {
                conProvider = "System.Data.SqlClient";
                if (CheckEntryEmpty(TBmssqlIP,"数据库地址不能为空！"))
                {
                    return;
                }
                else
                {
                    conIP = "Data Source=" + TBmssqlIP.Text.Trim();
                    conStr = conIP;
                }
                if (!string.IsNullOrEmpty(TBmssqlPort.Text.Trim()))
                {
                    conPort = "," + TBmssqlPort.Text.Trim();
                    conStr += conPort;
                }
                if (CheckEntryEmpty(TBmssqlName, "数据库名称不能为空！"))
                {
                    return;    
                }
                else
                {
                    conDBName = ";Initial Catalog=" + TBmssqlName.Text.Trim();
                    conStr += conDBName;
                }
                if (CheckEntryEmpty(TBmssqlUN, "用户名不能为空！"))
                {
                    return;
                }
                else
                {
                    conUserName = ";User ID=" + TBmssqlUN.Text.Trim();
                    conStr += conUserName;
                }
                if (CheckEntryEmpty(TBmssqlPW, "密码不能为空！"))
                {
                    return;
                }
                else
                {
                    conPassword = ";Password=" + TBmssqlPW.Text.Trim();
                    conStr += conPassword;
                }
               /*if (conProvider != string.Empty && conUserName != string.Empty && conPassword != string.Empty && conIP != string.Empty && conPort != string.Empty && conDBName != string.Empty)
                {
                    conStr = "Data Source=" + conIP + "," + conPort + ";Initial Catalog=" + conDBName + ";User ID=" + conUserName + ";Password=" + conPassword + ";";
                }*/
            }
            bool isModified = false;
            if (ConfigurationManager.ConnectionStrings[conName] != null)
            {
                isModified = true;
            }  
            ConnectionStringSettings newCon = new ConnectionStringSettings(conName, conStr,conProvider);
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isModified)
            {
                cfg.ConnectionStrings.ConnectionStrings.Remove(conName);//如果存在先移除KEY
                cfg.ConnectionStrings.ConnectionStrings.Add(newCon);
            }
            else
                cfg.ConnectionStrings.ConnectionStrings.Add(newCon);
            cfg.Save(ConfigurationSaveMode.Modified);//保存config
            ConfigurationManager.RefreshSection("connectionStrings");//刷新config
            if (this.FormClose != null)
            {
                FormClose(this, new EventArgs());//调用注册方法。关闭对话框
            }
        }
        private bool CheckEntryEmpty(TextBox tb, string message)
        {
            if (string.IsNullOrEmpty(tb.Text.Trim()))
            {
                tb.Text = message;
                return true;
            }
            else
                return false;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (this.FormClose != null)
            {
                FormClose(this, new EventArgs());//调用注册方法。关闭对话框
            }
        }
        private void GDmdb_Checked(object sender, RoutedEventArgs e)
        {
                GDmssql.Visibility = Visibility.Hidden;
                GDaccess.Visibility = Visibility.Visible;
        }
        private void RBmssql_Checked(object sender, RoutedEventArgs e)
        {
            GDmssql.Visibility = Visibility.Visible;
            GDaccess.Visibility = Visibility.Hidden;
        }

    }
}
