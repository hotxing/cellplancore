using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace LengZX.SharePart.Utilities
{
    /*************************************************************************************
    * CLR�汾��       4.0.30319.42000
    * �� �� �ƣ�      CSVHelper
    * �������ƣ�      LENGZHENXINGNOK
    * �����ռ䣺      NSB.XDRAnalyse.Common
    * �� �� ����      CSVHelper
    * ����ʱ�䣺      2015/12/11 11:05:57
    * ��    �ߣ�      ������
    * ��    ��:
    * �޸�ʱ�䣺
    * �� �� �ˣ�
    *************************************************************************************/

    /// <summary>
    ///     Add Describe Here
    /// </summary>
    public class CSVHelper
    {
        /// <summary>
        ///     ��ȡCsv�ļ�����DataTable
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static DataTable ReadCsv(string strpath)
        {



            var dt = new DataTable();
            var fs = new FileStream(strpath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.Default);
            //��¼ÿ�ζ�ȡ��һ�м�¼
            var strLine = "";
            //��¼ÿ�м�¼�еĸ��ֶ�����
            string[] aryLine;
            //��ʾ����
            var columnCount = 0;
            //��ʾ�Ƿ��Ƕ�ȡ�ĵ�һ��
            var IsFirst = true;

            //���ж�ȡCSV�е�����
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //������
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
        ///     ֻ��ȡCsv�ĵ�1�б���
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static DataTable ReadCsvOnlyTitle(string strpath)
        {
            var dt = new DataTable();
            var fs = new FileStream(strpath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.Default);

            //��¼ÿ�ζ�ȡ��һ�м�¼
            var strLine = "";
            //��¼ÿ�м�¼�еĸ��ֶ�����
            string[] aryLine;
            //��ʾ����
            var columnCount = 0;

            //ֻ��ȡCSV�е�һ�е�����
            if ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');

                columnCount = aryLine.Length;
                //������
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
        ///  �ֵ�д��csv
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
                //�Ѵ�����־д����һ���ı��ļ���
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
        /// �ֵ�д��csv
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
                //�Ѵ�����־д����һ���ı��ļ���
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
        /// �ֵ�д��csv
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
                //�Ѵ�����־д����һ���ı��ļ���
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
        /// �ֵ�д��csv
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
                //�Ѵ�����־д����һ���ı��ļ���
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
        ///     Listд��CSV
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
                //�Ѵ�����־д����һ���ı��ļ���
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
                //�Ѵ�����־д����һ���ı��ļ���
            }
        }

        /// <summary>
        /// ����Ϊsvc�ļ�,strFileNameΪҪ������csv��ʽ�ļ���·�����ļ���:���磬"d:\test\test.csv"
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
            //�ȴ�ӡ��ͷ
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
                strColu.Remove(strColu.Length - 1, 1); //�Ƴ������һ��,�ַ�

                sw.WriteLine(strColu);

                foreach (DataRow dr in dt.Rows)
                {
                    strValue.Remove(0, strValue.Length); //�Ƴ�

                    for (i = 0; i <= dt.Columns.Count - 1; i++)
                    {
                        strValue.Append(dr[i].ToString().Replace(',', '��'));
                        strValue.Append(",");
                    }
                    strValue.Remove(strValue.Length - 1, 1); //�Ƴ������һ��,�ַ�
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