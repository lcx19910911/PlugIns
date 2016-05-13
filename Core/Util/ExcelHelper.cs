using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public class ExcelHelper
    {
        public string FileName { get; set; }
        private HSSFWorkbook Workbook { get; set; }
        private HSSFSheet Sheet { get; set; }

        public ExcelHelper(string fileName)
        {
            FileName = fileName;
            Workbook = new HSSFWorkbook();
        }
        public ExcelHelper()
        {
        }

        public HSSFWorkbook GetWorkbook()
        {
            return this.Workbook;
        }

        /// <summary>
        /// 从输入流读
        /// </summary>
        /// <param name="inputStream">输入流</param>
        public void Read(Stream inputStream)
        {
            Workbook = new HSSFWorkbook(inputStream);
        }

        public void ReadFile(string fileName)
        {
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                Workbook = new HSSFWorkbook(file);
                file.Close();
            }
        }
        public void WriteFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Create);
            Workbook.Write(file);
            file.Close();
        }
        public MemoryStream WriteMemoryStream()
        {
            MemoryStream stream = new MemoryStream();
            Workbook.Write(stream);
            stream.Close();
            return stream;
        }

        public byte[] WriteByte()
        {
            MemoryStream stream = new MemoryStream();
            Workbook.Write(stream);
            byte[] buffer = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(buffer, 0, buffer.Length);
            stream.Close();
            return buffer;
        }



        public void SetSheet(int sheetIndex)
        {
            if (sheetIndex < 0)
            {
                Sheet = (HSSFSheet)Workbook.GetSheetAt(0);
            }
            else
            {
                Sheet = (HSSFSheet)Workbook.GetSheetAt(sheetIndex);
            }
        }

        public void CreateSheet(string sheetName)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                Sheet = (HSSFSheet)Workbook.CreateSheet("Sheet1");
            }
            else
            {
                Sheet = (HSSFSheet)Workbook.CreateSheet(sheetName);
            }
        }

        #region 读操作

        public string ReadFomula(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return "";
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);
                if (record.GetCell(column) != null)
                {
                    return record.GetCell(column).ToString();
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public double ReadNumeric(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return 0;
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);
                if (record.GetCell(column) != null)
                {
                    return record.GetCell(column).NumericCellValue;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public DateTime ReadDate(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return DateTime.Now;
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);

                if (record.GetCell(column) != null)
                {

                    return record.GetCell(column).DateCellValue; ;
                    //-lzq
                    //string date = record.GetCell(column).StringCellValue;
                    //date = date.Replace("-", "");
                    //date = date.Replace("/", "");
                    //DateTime dt = DateTime.ParseExact(date, "yyyyMMdd", null);
                    //return dt;
                }
                else
                {
                    return DateTime.Now;
                }
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public bool ReadBoolean(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return false;
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);
                if (record.GetCell(column) != null)
                {
                    return record.GetCell(column).BooleanCellValue;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public string ReadText(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return "";
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);
                if (record.GetCell(column) != null)
                {
                    if (record.GetCell(column).CellType == CellType.Numeric)
                    {
                        return record.GetCell(column).ToString();
                    }
                    else if (record.GetCell(column).CellType == CellType.Blank || record.GetCell(column).CellType == CellType.Error || record.GetCell(column).CellType == CellType.Unknown)
                    {
                        return "";
                    }
                    else
                    {
                        return record.GetCell(column).StringCellValue;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public byte ReadError(int row, int column)
        {
            if (row < 0 || column < 0)
            {
                return 0;
            }
            try
            {
                HSSFRow record = (HSSFRow)Sheet.GetRow(row);
                return record.GetCell(column).ErrorCellValue;
            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region 写操作

        public ICell WriteFomula(string fomulaValue, int row, int column)
        {
            if (row < 0)
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            HSSFRow record = (HSSFRow)Sheet.GetRow(row);
            if (record == null)
            {
                record = (HSSFRow)Sheet.CreateRow(row);
            }
            ICell cell = record.GetCell(column);
            if (cell == null)
            {
                cell = record.CreateCell(column);
            }
            cell.SetCellValue(fomulaValue);
            return cell;

        }

        public ICell WriteNumeric(double numericValue, int row, int column)
        {
            if (row < 0)
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            HSSFRow record = (HSSFRow)Sheet.GetRow(row);
            if (record == null)
            {
                record = (HSSFRow)Sheet.CreateRow(row);
            }
            ICell cell = record.GetCell(column);
            if (cell == null)
            {
                cell = record.CreateCell(column);
            }
            cell.SetCellValue(numericValue.ToString());
            return cell;
        }

        public ICell WriteDate(DateTime datetimeValue, int row, int column)
        {
            if (row < 0)
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            HSSFRow record = (HSSFRow)Sheet.GetRow(row);
            if (record == null)
            {
                record = (HSSFRow)Sheet.CreateRow(row);
            }
            ICell cell = record.GetCell(column);
            if (cell == null)
            {
                cell = record.CreateCell(column);
            }
            cell.SetCellValue(datetimeValue.ToString());
            return cell;
        }

        public ICell WriteBoolean(bool booleanValue, int row, int column)
        {
            if (row < 0)
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            HSSFRow record = (HSSFRow)Sheet.GetRow(row);
            if (record == null)
            {
                record = (HSSFRow)Sheet.CreateRow(row);
            }
            ICell cell = record.GetCell(column);
            if (cell == null)
            {
                cell = record.CreateCell(column);
            }
            cell.SetCellValue(booleanValue.ToString());
            return cell;
        }

        public ICell WriteText(string textValue, int row, int column)
        {
            if (row < 0)
            {
                row = 0;
            }
            if (column < 0)
            {
                column = 0;
            }
            HSSFRow record = (HSSFRow)Sheet.GetRow(row);
            if (record == null)
            {
                record = (HSSFRow)Sheet.CreateRow(row);
            }
            ICell cell = record.GetCell(column);
            if (cell == null)
            {
                cell = record.CreateCell(column);
            }
            cell.SetCellValue(textValue);
            return cell;
        }

        #endregion

        public int GetNumberOfSheets()
        {
            return Workbook != null ? Workbook.NumberOfSheets : 0;
        }


        public int GetFirstRowNum()
        {

            return Sheet.FirstRowNum;
        }

        public int GetLastRowNum()
        {

            return Sheet.LastRowNum;
        }

        public int GetRowCount()
        {

            return Sheet.LastRowNum;
        }

        public int GetFirstCellNum(int rowIndex)
        {
            HSSFRow row = (HSSFRow)Sheet.GetRow(rowIndex);

            if (row == null)
            {
                return -1;
            }

            return row.FirstCellNum;
        }

        public int GetLastCellNum(int rowIndex)
        {
            HSSFRow row = (HSSFRow)Sheet.GetRow(rowIndex);

            if (row == null)
            {
                return -1;
            }

            return row.LastCellNum;
        }

        public int GetColumnCount(int rowIndex)
        {
            HSSFRow row = (HSSFRow)Sheet.GetRow(rowIndex);

            if (row == null)
            {
                return -1;
            }

            return row.LastCellNum;
        }

        public void Close()
        {
            Workbook = null;
            Sheet = null;
        }


    }
}
