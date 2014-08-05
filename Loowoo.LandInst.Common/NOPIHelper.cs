using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Common
{
    public class NOPIHelper
    {
        public static List<string> ReadSimpleColumns(string filePath, int columnRowIndex = 0, int sheetIndex = 0)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var workbook = new HSSFWorkbook(fileStream);
                var sheet = workbook.GetSheetAt(sheetIndex);
                var result = new List<string>();

                var row = sheet.GetRow(columnRowIndex);
                foreach (var cell in row.Cells)
                {
                    result.Add(cell.StringCellValue);
                }

                return result;
            }
        }

        public static List<List<object>> ReadExcelData(string filePath, int dataRowIndex, int sheetIndex = 0)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var workbook = new HSSFWorkbook(fileStream);
                var sheet = workbook.GetSheetAt(sheetIndex);
                var data = new List<List<object>>();

                for (var i = dataRowIndex; i < sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    var rowData = new List<object>();
                    var isBlankRow = true;
                    foreach (var cell in row.Cells)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Numeric:
                                rowData.Add(cell.NumericCellValue);
                                isBlankRow = false;
                                break;
                            case CellType.String:
                                rowData.Add(cell.StringCellValue);
                                isBlankRow = false;
                                break;
                            default:
                                rowData.Add(null);
                                break;
                        }
                    }
                    if (!isBlankRow)
                    {
                        data.Add(rowData);
                    }
                }
                return data;
            }
        }
    }
}
