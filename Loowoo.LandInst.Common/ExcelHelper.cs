using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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

    public static class ExcelHelper
    {

        public static HSSFWorkbook GetExcel(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                return new HSSFWorkbook(fileStream);
            }
        }

        public static void InsertRow(this ISheet sheet, int startRowIndex, int count)
        {
            sheet.ShiftRows(startRowIndex, sheet.LastRowNum, count, true, false);
            var templateRow = sheet.GetRow(startRowIndex + count);
            for (var rowIndex = startRowIndex; rowIndex < startRowIndex + count; rowIndex++)
            {
                var row = sheet.CreateRow(rowIndex);
                if (templateRow.RowStyle != null)
                {
                    row.RowStyle = templateRow.RowStyle;
                }
                row.Height = templateRow.Height;

                for (var cellIndex = 0; cellIndex < templateRow.Cells.Count; cellIndex++)
                {
                    var cellTemplate = templateRow.Cells[cellIndex];
                    var cell = row.CreateCell(cellIndex, cellTemplate.CellType);
                    cell.SetCellValue(cellTemplate.StringCellValue);
                    if (cellTemplate.CellStyle != null)
                    {
                        cell.CellStyle = cellTemplate.CellStyle;
                    }
                }
            }
        }

        public static Stream ToStream(this HSSFWorkbook workbook, List<ExcelCell> sheetValues, int sheetIndex = 0)
        {
            var data = new Dictionary<int, List<ExcelCell>>();
            data.Add(sheetIndex, sheetValues);
            return workbook.ToStream(data);
        }

        public static Stream ToStream(this HSSFWorkbook workbook, Dictionary<int, List<ExcelCell>> sheetValues)
        {
            foreach (var kv in sheetValues)
            {
                var sheet = workbook.GetSheetAt(kv.Key);

                foreach (var cellValue in kv.Value)
                {
                    var rowIndex = cellValue.Row;
                    var cellIndex = cellValue.Cell;
                    var value = cellValue.Value;

                    var row = sheet.GetRow(rowIndex);
                    var cell = row.GetCell(cellIndex);
                    cell.SetCellValue(value);
                }
            }

            var result = new MemoryStream();
            workbook.Write(result);
            return result;
        }


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