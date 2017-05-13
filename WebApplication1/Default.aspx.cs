using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            DataTable table1 = new DataTable("patients");
            table1.Columns.Add("name");
            table1.Columns.Add("id");
            table1.Rows.Add("sam", 1);
            table1.Rows.Add("mark", 2);

            DataTable table2 = new DataTable("medications");
            table2.Columns.Add("id");
            table2.Columns.Add("medication");
            table2.Rows.Add(1, "atenolol");
            table2.Rows.Add(2, "amoxicillin");

            // Create a DataSet and put both tables in it.
            DataSet set = new DataSet("office");
            set.Tables.Add(table1);
            set.Tables.Add(table2);

            GenerateExcel2007(@"c:\xx\xx.xlsx", set);

        }

        public void GenerateExcel2007(string p_strPath, DataSet p_dsSrc)
        {
            using (ExcelPackage objExcelPackage = new ExcelPackage())
            {
                foreach (DataTable dtSrc in p_dsSrc.Tables)
                {
                    //Create the worksheet    
                    ExcelWorksheet objWorksheet = objExcelPackage.Workbook.Worksheets.Add(dtSrc.TableName);
                    //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1    
                    objWorksheet.Cells["A1"].LoadFromDataTable(dtSrc, true);
                    //objWorksheet.Cells.Style.Font.SetFromFont(new Font("Calibri", 10));
                    objWorksheet.Cells.AutoFitColumns();
                    //Format the header    
                    using (ExcelRange objRange = objWorksheet.Cells["A1:XFD1"])
                    {
                        objRange.Style.Font.Bold = true;
                        objRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        objRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        objRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //objRange.Style.Fill.BackgroundColor.SetColor(Color.FromA#eaeaea);    
            }
                }

                //Write it back to the client    
                if (File.Exists(p_strPath))
                    File.Delete(p_strPath);

                //Create excel file on physical disk    
                FileStream objFileStrm = File.Create(p_strPath);
                objFileStrm.Close();

                //Write content to excel file    
                File.WriteAllBytes(p_strPath, objExcelPackage.GetAsByteArray());
            }
        }
    }
}