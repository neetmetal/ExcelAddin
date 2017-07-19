using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.OleDb;
using System.Configuration;

namespace MatchPDFreport
{
    class WorkFlow
    {
        public void InputExcel(DataTable dt)
        {
            Excel.Workbook activeworkbook = Globals.EIMAddIn.Application.ActiveWorkbook;
            Excel.Worksheet activesheet = activeworkbook.ActiveSheet;
            int RowCount = 1;
            int usedCol = 0;//已使用列数
            foreach (DataColumn dc in dt.Columns)//循环写入表头
            {
                int index = dt.Columns.IndexOf(dc);
                activesheet.Cells[1, index + 1] = dc.ColumnName;
                usedCol++;//将已使用的列数存入变量
            }
            
            activesheet.Cells[1, usedCol + 1] = "是否匹配";
            activesheet.Cells[1, usedCol + 2] = "发票号";
            activesheet.Cells[1, usedCol + 3] = "客户名";
            activesheet.Cells[1, usedCol + 4] = "金额";
            activesheet.Cells[1, usedCol + 5] = "税额";
            usedCol++;
            foreach (DataRow dr in dt.Rows)//循环PDF的datatable
            {
                RowCount++;
                
                for (int i = 0; i < dt.Columns.Count; i++)//循环写入PDF的datatable一行数据
                {
                    activesheet.Cells[RowCount, i + 1] = dr[i].ToString();
                    
                }                
                DataTable tempDT = QueryDB(dr["发票号码"].ToString());//根据当前行的发票号码在数据库中查询对应行
                ;//将起始列往后移2列
                foreach (DataRow tempDr in tempDT.Rows)//循环数据库返回的datatable
                {
                    if (!MatchDataRow(dr, tempDr))//比对两个datarow数据
                    {
                        activesheet.Cells[RowCount, usedCol] = "不匹配";
                        for (int j = 0; j < tempDT.Columns.Count; j++)
                        {
                            activesheet.Cells[RowCount, usedCol + j + 1] = tempDr[j].ToString();
                        }
                    }
                    else
                    {
                        activesheet.Cells[RowCount, usedCol] = "匹配";
                        for (int j = 0; j < tempDT.Columns.Count; j++)
                        {
                            activesheet.Cells[RowCount, usedCol + j + 1] = tempDr[j].ToString();
                        }
                    }
                }
            }
        }
        private bool MatchDataRow(DataRow drInPDF, DataRow drInDB)
        {
            bool re = true;
            foreach (DataColumn dc in drInPDF.Table.Columns)
            {
                switch (dc.ColumnName)
                {
                    case "购方名称":
                        if (drInPDF[dc].ToString() != drInDB["INV_CLIENT"].ToString())
                        {
                            re = false;
                        }
                        break;
                    case "合计税额":
                        if (double.Parse(drInPDF[dc].ToString()) != double.Parse(drInDB["TAX_AMOUNT"].ToString()))
                        {
                            re = false;
                        }
                        break;
                    case "合计金额":
                        if (double.Parse(drInPDF[dc].ToString()) != double.Parse(drInDB["AMOUNT"].ToString()))
                        {
                            re = false;
                        }
                        break;
                }
            }
            return re;
        }
        private DataTable QueryDB(string INVNO)
        {
            string sql = "select INV_INVOICE_NO,INV_CLIENT,sum(INV_AMOUNT) as AMOUNT,sum(INV_TAX_AMOUNT) as TAX_AMOUNT from VAT_DETAIL where INV_STATUS=1 and INV_INVOICE_NO='" + int.Parse(INVNO) + "' or INV_INVOICE_NO='" + INVNO + "' group by INV_INVOICE_NO,INV_CLIENT";
            SQLconnection myCon = new SQLconnection();
            DataTable queryResult = myCon.QueryDataTable(sql);
            /*if (queryResult.Rows.Count == 0)
            {
                sql = "select INV_INVOICE_NO,INV_CLIENT,sum(INV_AMOUNT) as AMOUNT,sum(INV_TAX_AMOUNT) as TAX_AMOUNT from VAT_DETAIL where INV_STATUS=1 and INV_INVOICE_NO='" + INVNO + "' group by INV_INVOICE_NO,INV_CLIENT";
                queryResult = myCon.QueryDataTable(sql);
            }*/
            return queryResult;
        }
    }
}
