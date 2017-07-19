namespace MatchPDFreport
{
    partial class PDFMapping : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public PDFMapping()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.EIM = this.Factory.CreateRibbonTab();
            this.MainMenu = this.Factory.CreateRibbonGroup();
            this.BrowsePDF = this.Factory.CreateRibbonButton();
            this.groupSet = this.Factory.CreateRibbonGroup();
            this.btnDBset = this.Factory.CreateRibbonButton();
            this.EIM.SuspendLayout();
            this.MainMenu.SuspendLayout();
            this.groupSet.SuspendLayout();
            // 
            // EIM
            // 
            this.EIM.Groups.Add(this.MainMenu);
            this.EIM.Groups.Add(this.groupSet);
            this.EIM.Label = "EIM Mapping";
            this.EIM.Name = "EIM";
            // 
            // MainMenu
            // 
            this.MainMenu.Items.Add(this.BrowsePDF);
            this.MainMenu.Label = "Main Menu";
            this.MainMenu.Name = "MainMenu";
            // 
            // BrowsePDF
            // 
            this.BrowsePDF.Image = global::MatchPDFreport.Properties.Resources.PDF_128;
            this.BrowsePDF.Label = "打开金税PDF";
            this.BrowsePDF.Name = "BrowsePDF";
            this.BrowsePDF.ScreenTip = "browse PDF";
            this.BrowsePDF.ShowImage = true;
            this.BrowsePDF.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BrowsePDF_Click);
            // 
            // groupSet
            // 
            this.groupSet.Items.Add(this.btnDBset);
            this.groupSet.Label = "Setting";
            this.groupSet.Name = "groupSet";
            // 
            // btnDBset
            // 
            this.btnDBset.Label = "设置EIM数据库";
            this.btnDBset.Name = "btnDBset";
            this.btnDBset.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDBset_Click);
            // 
            // PDFMapping
            // 
            this.Name = "PDFMapping";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.EIM);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MatchPDF_Load);
            this.EIM.ResumeLayout(false);
            this.EIM.PerformLayout();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.groupSet.ResumeLayout(false);
            this.groupSet.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab EIM;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup MainMenu;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton BrowsePDF;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupSet;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDBset;

    }

    partial class ThisRibbonCollection
    {
        internal PDFMapping MatchPDF
        {
            get { return this.GetRibbon<PDFMapping>(); }
        }
    }
}
