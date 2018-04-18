using System.Collections.Generic;
using System.Linq;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Xml;

namespace Utility
{

    public class ExcelHelper
    {
        #region 导入复杂表头Excel

        /// <summary>
        /// 将Excel文件中的数据读出到DataSet中
        /// </summary>
        /// <param name="file">Excel文件路径</param>
        /// <param name="xmlpath">xml文件路径</param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string file, string xmlpath)
        {
            DataSet ds = new DataSet();
            //表头信息
            List<ExcelRegular> headerList = GetXMLInfo(xmlpath);
            int PageNoCol = headerList.Find(e => e.HeaderRegular != null).HeaderRegular["PageNoCol"];
            int lastHeaderRowIndex = headerList.Find(e => e.HeaderRegular != null).HeaderRegular["lastHeaderRow"];

            //指定单元格信息
            List<ExcelCell> cellList = GetCellXMLInfo(xmlpath);
            string result = string.Empty;

            Dictionary<int, int> _rowCount = new Dictionary<int, int>();
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook;
                if (file.ToLower().EndsWith(".xls"))
                    workbook = new HSSFWorkbook(fs);
                else
                    workbook = new XSSFWorkbook(fs);


                #region EXCEL 工作簿
                #region Datum-Hole
                ISheet DatumHole = workbook.GetSheet("Datum-Hole");
                if (null != DatumHole)
                    ds.Tables.Add(ReadExcel(DatumHole, headerList, cellList, PageNoCol, lastHeaderRowIndex));
                #endregion

                #region Datum-Surface
                ISheet DatumSurface = workbook.GetSheet("Datum-Surface");
                if (null != DatumSurface)
                    ds.Tables.Add(ReadExcel(DatumSurface, headerList, cellList, PageNoCol, lastHeaderRowIndex));

                #endregion

                #region Hole
                ISheet Hole = workbook.GetSheet("Hole");
                if (null != Hole)
                    ds.Tables.Add(ReadExcel(Hole, headerList, cellList, PageNoCol, lastHeaderRowIndex));
                #endregion

                #region Surface
                ISheet Surface = workbook.GetSheet("Surface");
                if (null != Surface)
                    ds.Tables.Add(ReadExcel(Surface, headerList, cellList, PageNoCol, lastHeaderRowIndex));
                #endregion

                #region Trim
                ISheet Trim = workbook.GetSheet("Trim");
                if (null != Trim)
                    ds.Tables.Add(ReadExcel(Trim, headerList, cellList, PageNoCol, lastHeaderRowIndex));
                #endregion
                #endregion
            }
            return ds;
        }


        public static DataTable ReadExcel(ISheet sheet, List<ExcelRegular> headerList, List<ExcelCell> cellList,int PageNoCol,int lastHeaderRowIndex)
        {
            #region datatable
            DataTable dt = new DataTable(sheet.SheetName);
            dt.Columns.Add(new DataColumn("Project"));
            dt.Columns.Add(new DataColumn("PartNo"));
            dt.Columns.Add(new DataColumn("PartName"));
            dt.Columns.Add(new DataColumn("SheetName"));
            dt.Columns.Add(new DataColumn("Pages"));
            foreach (var item in headerList)
            {
                dt.Columns.Add(new DataColumn(item.PropertyName));
            }
            #endregion

            string ProjectId = string.Empty;
            string PublicId = string.Empty;
            string PublicName = string.Empty;

            Dictionary<string, string> Title = new Dictionary<string, string>();

            foreach (var cell in cellList)
            {
                var obj = sheet.GetRow(cell.CellY).GetCell(cell.CellX);
                var name = cell.CellName;
                var value = string.Empty;

                if (obj.CellType == CellType.Formula)
                {
                    try
                    {
                        value = obj.StringCellValue;
                    }
                    catch
                    {
                        value = obj.NumericCellValue.ToString();
                    }
                }
                else
                {
                    value = obj.ToString();
                }


                Title.Add(name, value);
            }

            GetExcelHeaders(sheet, ref headerList);
            int Page = 1;

            int LastRowNum = 0;
            for (int j = sheet.LastRowNum; j > 0; j--)
            {
                var row = sheet.GetRow(j);
                var cel = row.GetCell(PageNoCol);
                if (null != cel && !string.IsNullOrEmpty(cel.ToString().Trim()))
                {
                    LastRowNum = j;
                    break;
                }
            }


            #region 生成数据
            for (int j = lastHeaderRowIndex; j <= LastRowNum-1; j++)
            {
                var row = sheet.GetRow(j);
                var cel= GetValueTypeForXLSX(row.GetCell(PageNoCol) as ICell);
                if (null != cel && !string.IsNullOrEmpty(cel.ToString().Trim()) && cel.ToString().Trim().StartsWith("页码 Page No."))
                {
                    Page++;
                    continue;
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    dr["Project"] = Title["Project"];
                    dr["PartNo"] = Title["PartNo"];
                    dr["PartName"] = Title["PartName"];
                    dr["SheetName"] = sheet.SheetName;
                    dr["Pages"] = Page.ToString();

                    var LineHasValue = false;

                    foreach (var item in headerList.Where(b => !string.IsNullOrEmpty(b.HeaderText)))
                    {
                        if (item.CellNumber >= 0)
                        {
                            var value = GetValueTypeForXLSX(sheet.GetRow(j).GetCell(item.CellNumber) as ICell);
                            dr[item.PropertyName] = value;

                            if (null != value && !string.IsNullOrEmpty(value.ToString().Trim()))
                                LineHasValue = true;
                        }
                        else
                        {
                            dr[item.PropertyName] = string.Empty;
                        }
                    }
                    if (LineHasValue)
                        dt.Rows.Add(dr);
                }
            }
            #endregion

            return dt;
        }

        /// <summary>
        /// 获取单元格类型(xlsx)
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueTypeForXLSX(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:
                    return null;
                case CellType.Boolean: //BOOLEAN:
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:
                    return cell.NumericCellValue;
                case CellType.String: //STRING:
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:
                    try { return cell.StringCellValue; } catch  { return cell.NumericCellValue; }
                default:
                    return "=" + cell.CellFormula;
            }
        }

        /// <summary>
        ///  读取XML表头配置信息集
        /// </summary>
        /// <param name="xmlpath"></param>
        /// <returns></returns>
        public static List<ExcelRegular> GetXMLInfo(string xmlpath)
        {
            var reader = new XmlTextReader(xmlpath);
            var doc = new XmlDocument();
            doc.Load(reader);

            var headerList = new List<ExcelRegular>();
            int i = 0;
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var header = new ExcelRegular();

                if (node.Attributes["firstHeaderRow"] != null)
                {
                    header.HeaderRegular.Add("firstHeaderRow", int.Parse(node.Attributes["firstHeaderRow"].Value));
                    i = 1;
                }
                if (node.Attributes["lastHeaderRow"] != null)
                {
                    header.HeaderRegular.Add("lastHeaderRow", int.Parse(node.Attributes["lastHeaderRow"].Value));
                    i = 1;
                }
                if (node.Attributes["sheetCount"] != null)
                {
                    header.HeaderRegular.Add("sheetCount", int.Parse(node.Attributes["sheetCount"].Value));
                    i = 1;
                }
                if (node.Attributes["PageNoCol"] != null)
                {
                    header.HeaderRegular.Add("PageNoCol", int.Parse(node.Attributes["PageNoCol"].Value));
                    i = 1;
                }
                if (node.Attributes["headerText"] != null)
                {
                    header.HeaderText = node.Attributes["headerText"].Value;
                    i = 1;
                }
                if (node.Attributes["propertyName"] != null)
                {
                    header.PropertyName = node.Attributes["propertyName"].Value;
                    i = 1;
                }
                if (node.Attributes["dataType"] != null)
                {
                    header.DataType = node.Attributes["dataType"].Value;
                    i = 1;
                }
                if (i == 1)
                {
                    headerList.Add(header);
                    i = 0;
                }
            }
            return headerList;
        }

        /// <summary>
        ///  读取XML单元格配置信息集
        /// </summary>
        /// <param name="xmlpath"></param>
        /// <returns></returns>
        public static List<ExcelCell> GetCellXMLInfo(string xmlpath)
        {
            var reader = new XmlTextReader(xmlpath);
            var doc = new XmlDocument();
            doc.Load(reader);

            var cellList = new List<ExcelCell>();
            int i = 0;
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var cell = new ExcelCell();
                if (node.Attributes["cellName"] != null)
                {
                    cell.CellName = node.Attributes["cellName"].Value;
                    i = 1;
                }
                if (node.Attributes["cellX"] != null)
                {
                    cell.CellX = int.Parse(node.Attributes["cellX"].Value);
                    i = 1;
                }
                if (node.Attributes["cellY"] != null)
                {
                    cell.CellY = int.Parse(node.Attributes["cellY"].Value);
                    i = 1;
                }
                if (i == 1)
                {
                    cellList.Add(cell);
                    i = 0;
                }
            }
            return cellList;
        }

        /// <summary>
        /// 循环获取表头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="uploadExcelFileResult"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void GetExcelHeaders(ISheet sheet, ref List<ExcelRegular> list)
        {
            //复杂表头开始行
            int firstHeaderRowIndex = list.Find(e => e.HeaderRegular != null).HeaderRegular["firstHeaderRow"];
            //复杂表头结束行
            int lastHeaderRowIndex = list.Find(e => e.HeaderRegular != null).HeaderRegular["lastHeaderRow"];

            var NewDict = new Dictionary<string, int>();

            #region 循环获得表头
            for (int i = firstHeaderRowIndex - 1; i < lastHeaderRowIndex; i++)
            {
                IRow headerRow = sheet.GetRow(i);
                int cellCount = headerRow.LastCellNum;

                for (int j = headerRow.FirstCellNum; j < cellCount; j++)
                {
                    var cellValue = GetValueTypeForXLSX(headerRow.GetCell(j) as ICell);
                    if (cellValue != null)
                    {
                        string strValue = cellValue.ToString().Trim();
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            strValue = strValue.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                            NewDict.Add(strValue, j);
                        }
                    }
                }
            }
            #endregion

            //校验XML配置信息

            foreach (var item in list.Where(b=>!string.IsNullOrEmpty(b.HeaderText)))
            {
                if (NewDict.ContainsKey(item.HeaderText))
                {
                    item.CellNumber = NewDict[item.HeaderText];
                }
                else
                {
                    item.CellNumber = -1;
                }
            }
        }

        /// <summary>
        /// 去除空值
        /// </summary>
        /// <param name="cellValue"></param>
        public static void ReplaceSpace(ref string cellValue)
        {
            cellValue = TruncateString(cellValue, new char[] { ' ' }, new char[] { '　' });
        }

        /// <summary>
        /// 对字符串做空格剔除处理
        /// </summary>
        /// <param name="originalWord"></param>
        /// <param name="spiltWord1"></param>
        /// <param name="spiltWord2"></param>
        /// <returns></returns>
        private static string TruncateString(string originalWord, char[] spiltWord1, char[] spiltWord2)
        {
            var result = "";
            var valueReplaceDbcCase = originalWord.Split(spiltWord1);

            if (valueReplaceDbcCase.Count() > 1)
            {
                for (int i = 0; i < valueReplaceDbcCase.Count(); i++)
                {
                    if (valueReplaceDbcCase[i] != "" && valueReplaceDbcCase[i] != " " &&
                        valueReplaceDbcCase[i] != "　")
                    {
                        result += TruncateString(valueReplaceDbcCase[i], spiltWord2, new char[0]);
                    }
                }
            }
            else
            {
                if (spiltWord2.Any())
                {
                    result = TruncateString(originalWord, spiltWord2, new char[0]);
                }
                else
                {
                    result = originalWord;
                }
            }

            return result;
        }
        #endregion

        /// <summary>
        /// 判断选择的文件的文件格式是否为Excel文档
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool Judge_FileName(string FileName)
        {
            if (string.IsNullOrEmpty(FileName))
                return false;
            int iPos = FileName.IndexOf(".");
            if (iPos < 0)
                return false;
            string afterStr = FileName.Substring(iPos + 1, FileName.Length - iPos - 1);
            if (string.Equals(afterStr.ToUpper(), "XLS") || string.Equals(afterStr.ToUpper(), "XLSX"))
                return true;
            else
                return false;
        }
    }
}
