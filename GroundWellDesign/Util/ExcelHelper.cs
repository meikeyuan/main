using Aspose.Cells;
using GroundWellDesign.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GroundWellDesign
{
    public class ExcelHelper
    {
        #region ExportToExcel

        // 提取DataGrid数据到DataTable。
        public static DataTable ExtractDataTable(DataGrid dataGrid)
        {
            if (dataGrid == null)
                return null;

            DataTable dt = new DataTable();
            // 构建表头  
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                if (dataGrid.Columns[i].Visibility == System.Windows.Visibility.Visible)  // 只导出可见列  
                {
                    dt.Columns.Add(dataGrid.Columns[i].Header.ToString());
                }
            }

            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                var dataGridRow = dataGrid.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow;
                if (dataGridRow.Visibility != System.Windows.Visibility.Visible)
                {
                    continue;
                }
                DataRow row = dt.NewRow();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    if (dataGrid.Columns[j].Visibility == System.Windows.Visibility.Visible)
                    {
                        Object obj = dataGrid.Columns[j].GetCellContent(dataGrid.Items[i]);
                        if ((obj as TextBlock) != null)
                        {
                            row[j] = (obj as TextBlock).Text.ToString();
                        }
                        else if ((obj as System.Windows.Controls.ComboBox) != null)
                        {
                            row[j] = (obj as System.Windows.Controls.ComboBox).Text.ToString();
                        }
                    }
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        // 将DataTable对象转为Excel文件。
        public static bool ExportExcelWithAspose(DataTable dt, string path)
        {
            if(dt == null || path == null)
            {
                return false;
            }

            bool succeed = false;
            try
            {
                Aspose.Cells.License li = new Aspose.Cells.License();
                string lic = Resources.License;
                li.SetLicense(lic);

                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];

                cellSheet.Name = dt.TableName;

                int rowIndex = 0;
                int colIndex = 0;
                int colCount = dt.Columns.Count;
                int rowCount = dt.Rows.Count;

                //列名的处理
                for (int i = 0; i < colCount; i++)
                {
                    cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
                    cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;
                    cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";
                    colIndex++;
                }

                Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                style.Font.Name = "Arial";
                style.Font.Size = 10;
                Aspose.Cells.StyleFlag styleFlag = new Aspose.Cells.StyleFlag();
                cellSheet.Cells.ApplyStyle(style, styleFlag);

                rowIndex++;

                for (int i = 0; i < rowCount; i++)
                {
                    colIndex = 0;
                    for (int j = 0; j < colCount; j++)
                    {
                        cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
                        colIndex++;
                    }
                    rowIndex++;
                }
                cellSheet.AutoFitColumns();

                path = Path.GetFullPath(path);
                workbook.Save(path);
                succeed = true;
            }
            catch (Exception ex)
            {
                App.logger.Fatal(Resources.ExportExcelError, ex);
                succeed = false;
            }

            return succeed;
        }

        #endregion  // ExportToExcel


        #region LoadFromExcel

        // 从Excel文件读取DataTable。
        public static DataTable LoadExcelWithAspose(string path, bool showTitle = true)
        {
            try
            {
                Aspose.Cells.License li = new Aspose.Cells.License();
                string lic = Resources.License;
                li.SetLicense(lic);

                Workbook workbook = new Workbook();
                workbook.Open(path);
                Cells cells = workbook.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTableAsString(cells.Start.Row, cells.Start.Column, cells.Rows.Count, cells.Columns.Count, showTitle);

                return dt;
            }
            catch(Exception ex)
            {
                App.logger.Fatal(Resources.LoadExcelError, ex);
                return null;
            }
        }

        #endregion  // LoadFromExcel
    }
}
