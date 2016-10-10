using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Loowoo.LandInst.Common
{
    public class ExcelCell
    {
        public ExcelCell(int row, int cell, string value)
        {
            Row = row;
            Cell = cell;
            Value = value;
        }

        public int Row { get; set; }

        public int Cell { get; set; }

        public string Value { get; set; }
    }

    public class NOPIHelper
    {
        private static IWorkbook GetWorkbook(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
               return WorkbookFactory.Create(fileStream);
            }
        }

        public static Stream WriteCell(string filePath, List<ExcelCell> values, int sheetIndex = 0)
        {
            var workbook = GetWorkbook(filePath);
            var sheet = workbook.GetSheetAt(sheetIndex);

            foreach (var cellValue in values)
            {
                var rowIndex = cellValue.Row;
                var cellIndex = cellValue.Cell;
                var value = cellValue.Value;

                var row = sheet.GetRow(rowIndex);
                var cell = row.GetCell(cellIndex);
                cell.SetCellValue(value);
            }
            var result = new MemoryStream();
            workbook.Write(result);
            return result;
        }


        public static Stream WriteCell(string filePath, Dictionary<int, List<ExcelCell>> sheetValues)
        {
            var workbook = GetWorkbook(filePath);
            foreach (var kv in sheetValues)
            {
                var sheet = workbook.GetSheetAt(kv.Key);

                foreach (var cellValue in kv.Value)
                {
                    var rowIndex = cellValue.Row;
                    var cellIndex = cellValue.Cell;
                    var value = cellValue.Value;

                    var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);

                    var cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);

                    cell.SetCellValue(value);
                }
            }
            var result = new MemoryStream();
            workbook.Write(result);
            return result;
        }


        public static List<string> ReadSimpleColumns(Stream stream, int columnRowIndex = 0, int sheetIndex = 0)
        {
            var workbook = WorkbookFactory.Create(stream);
            var sheet = workbook.GetSheetAt(sheetIndex);
            var result = new List<string>();

            var row = sheet.GetRow(columnRowIndex);
            foreach (var cell in row.Cells)
            {
                result.Add(cell.StringCellValue);
            }

            return result;
        }

        public static List<List<object>> ReadExcelData(Stream stream, int dataRowIndex, int sheetIndex = 0)
        {
            var workbook = WorkbookFactory.Create(stream);
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