using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Controls;

namespace MatchPDFreport
{
    public partial class DBsetting : Form
    {        
        public DBsetting()
        {
            InitializeComponent();
        }
        
        private void DBsetting_Load(object sender, EventArgs e)
        {            
            ElementHost eh = new ElementHost();
            eh.Dock = DockStyle.Fill;
            DBsettingUC dbsetting = new DBsettingUC();
            dbsetting.InitializeComponent();            
            MainPanel.Controls.Add(eh);
            eh.Child = dbsetting;
            //dbsetting.FormClose += new DBsettingUC.FormCloseEventHandler(this.userControl_FormClose);//第二种添加方法
            dbsetting.FormClose += this.userControl_FormClose;//注册方法
            
        }
        private void userControl_FormClose(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
