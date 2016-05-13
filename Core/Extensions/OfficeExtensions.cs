//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Interop.Excel;
//using System.Web;

//namespace Core.Extensions
//{
//    /// <summary>
//    /// 导出excel操作类
//    /// </summary>
//    public class OfficeExtend
//    {
//        #region DataTable导出Word
//        /// <summary>
//        /// DataTable导出Word
//        /// </summary>
//        /// <param name="dt">导出的数据DataTable</param>
//        /// <param name="isColname">是否显示列名</param>
//        public static void OutPutWordDT(System.Data.DataTable dt, bool isColname)
//        {
//            Object Nothing = System.Reflection.Missing.Value;
//            Microsoft.Office.Interop.Word.Application oword = new Microsoft.Office.Interop.Word.Application();//word Application
//            Microsoft.Office.Interop.Word.Document odoc = oword.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);//文档
//            odoc.Paragraphs.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

//            try
//            {
//                //在word以表格形式存储数据
//                Microsoft.Office.Interop.Word.Table otable = odoc.Tables.Add(oword.Selection.Range, dt.Rows.Count + 1, dt.Columns.Count, ref Nothing, ref Nothing);
//                otable.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphThaiJustify;//设置对其方式
//                otable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;//设置表格边框样式

//                if(isColname)//列名称
//                {
//                    int intcol = 0;
//                    for(int ii = 0; ii < dt.Columns.Count; ii++)
//                    {
//                        intcol += 1;
//                        otable.Cell(1, intcol).Range.Text = dt.Columns[ii].ColumnName;
//                        otable.Cell(1, intcol).Range.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;//设置单元格样式
//                    }
//                }

//                //写表格内容
//                int intRow = 1;
//                for(int ii = 0; ii < dt.Rows.Count; ii++)
//                {
//                    intRow += 1;
//                    int intCol = 0;
//                    for(int jj = 0; jj < dt.Columns.Count; jj++)
//                    {
//                        intCol += 1;
//                        otable.Cell(intRow, intCol).Range.Text = dt.Rows[ii][jj].ToString();
//                        otable.Cell(intRow, intCol).Range.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;//设置单元格样式
//                    }
//                }
//                oword.Visible = true;
//            }
//            catch(Exception)
//            {
//            }
//            finally
//            {
//                System.Diagnostics.Process[] CurrentProcess = System.Diagnostics.Process.GetProcessesByName("WINWORD");
//                for(int i = 0; i < CurrentProcess.Length; i++)
//                {
//                    if(CurrentProcess[i].MainWindowHandle.ToInt32() == 0)
//                    {
//                        try
//                        {
//                            CurrentProcess[i].Kill();
//                        }
//                        catch
//                        {
//                        }
//                    }
//                }
//            }
//        }
//        #endregion DataTable导出Word

//        #region DataTable导出Excel

//        /// <summary>
//        /// DataTable导出Excel
//        /// </summary>
//        /// <param name="dt">DataTable</param>
//        public void OutPutExcelDT(System.Data.DataTable dt, bool iscolumnname)
//        {
//            try
//            {
//                //实例化一个Excel.Application对象   
//                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

//                //新增加一个工作簿，Workbook是直接保存，不会弹出保存对话框，加上Application会弹出保存对话框，值为false会报错   
//                excel.Application.Workbooks.Add(true);

//                //让后台执行设置为不可见，为true的话会看到打开一个Excel，然后数据在往里写   
//                excel.Visible = true;

//                if(iscolumnname)
//                {
//                    //生成Excel中列头名称   
//                    for(int i = 0; i < dt.Columns.Count; i++)
//                    {
//                        excel.Cells[1, i + 1] = dt.Columns[i].ColumnName;//输出DataGridView列头名   
//                    }
//                }

//                //把DataGridView当前页的数据保存在Excel中   
//                if(dt.Rows.Count > 0)
//                {
//                    for(int i = 0; i < dt.Rows.Count; i++)//控制Excel中行，上下的距离，就是可以到Excel最下的行数，比数据长了报错，比数据短了会显示不完   
//                    {
//                        for(int j = 0; j < dt.Columns.Count; j++)//控制Excel中列，左右的距离，就是可以到Excel最右的列数，比数据长了报错，比数据短了会显示不完   
//                        {
//                            string str = dt.Rows[i][j].ToString();
//                            excel.Cells[i + 2, j + 1] = "'" + str;//i控制行，从Excel中第2行开始输出第一行数据，j控制列，从Excel中第1列输出第1列数据，"'" +是以string形式保存，所以遇到数字不会转成16进制   
//                        }
//                    }
//                }
//                //设置弹出保存和覆盖的询问提示框   
//                excel.DisplayAlerts = true;
//                excel.AlertBeforeOverwriting = true;
//            }
//            catch(Exception ex)
//            {
//                throw ex;
//            }
//            finally
//            {
//                System.Diagnostics.Process[] CurrentProcess = System.Diagnostics.Process.GetProcessesByName("WINEXCEL");
//                for(int i = 0; i < CurrentProcess.Length; i++)
//                {
//                    if(CurrentProcess[i].MainWindowHandle.ToInt32() == 0)
//                    {
//                        try
//                        {
//                            CurrentProcess[i].Kill();
//                        }
//                        catch
//                        {
//                        }
//                    }
//                }
//            }
//        }

//        #endregion DataTbale导出Excel

//        #region List<T>导出excel
//        /// <summary> 
//        /// 将一组对象导出成EXCEL 
//        /// </summary> 
//        /// <typeparam name="T">要导出对象的类型</typeparam> 
//        /// <param name="objList">对象集合</param> 
//        /// <param name="FileName">导出后的文件名</param> 
//        /// <param name="columnInfo">列名信息</param> 
//        public void ExportExcel<T>(List<T> objList, string FileName, Dictionary<string, string> columnInfo)
//        {
//            if (columnInfo.Count == 0) { return; }
//            //生成EXCEL的HTML 
//            string excelStr = "";
//            Type myType = typeof(T);
//            //根据反射从传递进来的属性名信息得到要显示的属性 
//            List<System.Reflection.PropertyInfo> myPro = new List<System.Reflection.PropertyInfo>();
//            foreach (string cName in columnInfo.Keys)
//            {
//                System.Reflection.PropertyInfo p = myType.GetProperty(cName);
//                if (p != null)
//                {
//                    myPro.Add(p);
//                    excelStr += columnInfo[cName] + "\t";
//                }
//            }
//            //如果没有找到可用的属性则结束 
//            if (myPro.Count == 0) { return; }
//            excelStr += "\n";
//            foreach (T obj in objList)
//            {
//                for (int i = 0; i < myPro.Count; i++)
//                {
//                    excelStr += myPro[i].GetValue(obj, null) + "\t";
//                }
//                excelStr += "\n";
//            }
//            //输出EXCEL 
//            HttpResponse rs = System.Web.HttpContext.Current.Response;
//            rs.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
//            rs.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(FileName, Encoding.UTF8));
//            rs.ContentType = "application/ms-excel";
//            rs.Write(excelStr);
//            rs.End();
//        }
//        #endregion
//    }
//}
