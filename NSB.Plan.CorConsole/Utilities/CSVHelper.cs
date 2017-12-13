using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LengZX.SharePart.Utilities
{
    /*************************************************************************************
    * CLR版本：       4.0.30319.42000
    * 类 名 称：      CSVHelper
    * 机器名称：      LENGZHENXINGNOK
    * 命名空间：      NSB.XDRAnalyse.Common
    * 文 件 名：      CSVHelper
    * 创建时间：      2015/12/11 11:05:57
    * 作    者：      冷振兴
    * 功    能:
    * 修改时间：
    * 修 改 人：
    *************************************************************************************/

    /// <summary>
    ///     Add Describe Here
    /// </summary>
    public class CSVHelper
    {
        /// <summary>
        ///     读取Csv文件返回DataTable
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static DataTable ReadCsv(string strpath)
        {



            var dt = new DataTable();
            var fs = new FileStream(strpath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.Default);
            //记录每次读取的一行记录
            var strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            var columnCount = 0;
            //标示是否是读取的第一行
            var IsFirst = true;

            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (var i = 0; i < columnCount; i++)
                    {
                        var dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    var dr = dt.NewRow();
                    for (var j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary>
        ///     只读取Csv的第1行标题
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static DataTable ReadCsvOnlyTitle(string strpath)
        {
            var dt = new DataTable();
            var fs = new FileStream(strpath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.Default);

            //记录每次读取的一行记录
            var strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            var columnCount = 0;

            //只读取CSV中第一行的数据
            if ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');

                columnCount = aryLine.Length;
                //创建列
                for (var i = 0; i < columnCount; i++)
                {
                    var dc = new DataColumn(aryLine[i]);
                    dt.Columns.Add(dc);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        /// <summary>
        ///  字典写入csv
        /// </summary>
        /// <param name="DicXdrData"></param>
        /// <param name="strPath"></param>
        public static void WriteTXT(ConcurrentDictionary<double, double[]> DicXdrData, string strPath)
        {
            StreamWriter writer = null;
            var stBtxe = new StringBuilder();
            if (File.Exists(strPath))
            {
                writer = File.AppendText(strPath);
            }
            else
            {
                writer = File.CreateText(strPath);
            }
            try
            {
                foreach (var key in DicXdrData.Keys)
                {
                    stBtxe.Clear();
                    for (var i = 0; i < DicXdrData[key].Length; i++)
                    {
                        stBtxe.Append(DicXdrData[key][i] + ",");
                    }
                    writer.WriteLine(stBtxe.ToString().Substring(0, stBtxe.ToString().Length - 1),
                        Encoding.GetEncoding("gb2312"));
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
                //throw e;
            }
            finally
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }

        /// <summary>
        /// 字典写入csv
        /// </summary>
        /// <param name="DicXdrData"></param>
        /// <param name="strPath"></param>
        public static void WriteTXT(Dictionary<double, double[]> DicXdrData, string strPath)
        {
            StreamWriter writer = null;
            var stBtxe = new StringBuilder();
            if (File.Exists(strPath))
            {
                writer = File.AppendText(strPath);
            }
            else
            {
                writer = File.CreateText(strPath);
            }
            try
            {
                foreach (var key in DicXdrData.Keys)
                {
                    stBtxe.Clear();
                    for (var i = 0; i < DicXdrData[key].Length; i++)
                    {
                        stBtxe.Append(DicXdrData[key][i] + ",");
                    }
                    writer.WriteLine(stBtxe.ToString().Substring(0, stBtxe.ToString().Length - 1),
                        Encoding.GetEncoding("gb2312"));
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
                //throw e;
            }
            finally
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }
        /// <summary>
        /// 字典写入csv
        /// </summary>
        /// <param name="DicXdrData"></param>
        /// <param name="strPath"></param>
        public static void WriteTXT(ConcurrentDictionary<string, string> DicXdrData, string strPath)
        {
            StreamWriter writer = null;
            var stBtxe = new StringBuilder();
            if (File.Exists(strPath))
            {
                writer = File.AppendText(strPath);
            }
            else
            {
                writer = File.CreateText(strPath);
            }
            try
            {
                foreach (var key in DicXdrData.Keys)
                {                    
                    writer.WriteLine(DicXdrData[key]);
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
                //throw e;
            }
            finally
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }
        /// <summary>
        /// 字典写入csv
        /// </summary>
        /// <param name="DicXdrData"></param>
        /// <param name="strPath"></param>
        public static void WriteTXT(Dictionary<string, double[]> DicXdrData, string strPath)
        {
            StreamWriter writer = null;
            var stBtxe = new StringBuilder();
            if (File.Exists(strPath))
            {
                writer = File.AppendText(strPath);
            }
            else
            {
                writer = File.CreateText(strPath);
            }
            try
            {
                foreach (var key in DicXdrData.Keys)
                {
                    stBtxe.Clear();
                    for (int i = 0; i < key.Split('_').Length; i++)
                    {
                        stBtxe.Append(key.Split('_')[i] + ",");
                    }

                    //stBtxe.Append(key.Split('_')[0]+","+ key.Split('_')[1]+",");
                    for (var i = 0; i < DicXdrData[key].Length; i++)
                    {
                        stBtxe.Append(DicXdrData[key][i] + ",");
                    }
                    writer.WriteLine(stBtxe.ToString().Substring(0, stBtxe.ToString().Length - 1));
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
                //throw e;
            }
            finally
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }

        /// <summary>
        ///     List写入CSV
        /// </summary>
        /// <param name="SQLStringList"></param>
        /// <param name="strPath"></param>
        public static void WriteTXT(List<string> SQLStringList, string strPath)
        {
            StreamWriter writer = null;
            if (File.Exists(strPath))
            {
                writer = File.AppendText(strPath);
            }
            else
            {
                writer = File.CreateText(strPath);
            }
            try
            {
                for (var i = 0; i < SQLStringList.Count; i++)
                {
                    writer.WriteLine(SQLStringList[i]);
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
                //throw e;
            }
            finally
            {
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
        }

        public static void WriteTXT(string message, string strPath, int Flag)
        {
            try
            {
                StreamWriter writer = null;
                if (File.Exists(strPath))
                {
                    if (Flag == 1)
                    {
                        File.Delete(strPath);
                        writer = File.CreateText(strPath);
                    }
                    else
                    {
                        writer = File.AppendText(strPath);
                    }
                }
                else
                {
                    writer = File.CreateText(strPath);
                }

                writer.WriteLine(message, Encoding.GetEncoding("gb2312"));
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        /// <summary>
        /// 导出为svc文件,strFileName为要导出的csv格式文件的路径和文件名:比如，"d:\test\test.csv"
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strFileName"></param>
        public static void ExportToSvc(System.Data.DataTable dt, string strFileName)
        {
            string strPath = strFileName;
            if (File.Exists(strPath))
            {
                File.Delete(strPath);
            }
            //先打印标头
            StringBuilder strColu = new StringBuilder();
            StringBuilder strValue = new StringBuilder();
            int i = 0;
            try
            {
                StreamWriter sw = new StreamWriter(new FileStream(strPath, FileMode.CreateNew),
                    Encoding.GetEncoding("GB2312"));

                for (i = 0; i <= dt.Columns.Count - 1; i++)
                {
                    strColu.Append(dt.Columns[i].ColumnName);
                    strColu.Append(",");
                }
                strColu.Remove(strColu.Length - 1, 1); //移出掉最后一个,字符

                sw.WriteLine(strColu);

                foreach (DataRow dr in dt.Rows)
                {
                    strValue.Remove(0, strValue.Length); //移出

                    for (i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        strValue.Append(dr[i].ToString().Replace(',', '，'));
                        strValue.Append(",");
                    }
                    strValue.Remove(strValue.Length - 1, 1); //移出掉最后一个,字符
                    sw.WriteLine(strValue);
                }

                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}