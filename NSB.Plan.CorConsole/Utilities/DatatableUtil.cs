using System;
using System.Collections.Generic;
using System.Data;

namespace LengZX.SharePart.Utilities
{
    public class DatatableUtil
    {
        /// <summary>
        /// 修改数据表DataTable某一列的类型和记录值(正确步骤：1.克隆表结构，2.修改列类型，3.修改记录值，4.返回希望的结果)
        /// </summary>
        /// <param name="argDataTable">数据表DataTable</param>
        /// <returns>数据表DataTable</returns>
        private DataTable UpdateDataTable(DataTable argDataTable)
        {
            DataTable dtResult = new DataTable();
            //克隆表结构
            dtResult = argDataTable.Clone();
            foreach (DataColumn col in dtResult.Columns)
            {
                if (col.ColumnName == "age")
                {
                    //修改列类型
                    col.DataType = typeof(String);
                }     
            }
            foreach (DataRow row in argDataTable.Rows)
            {
                DataRow rowNew = dtResult.NewRow();
                rowNew["MemberId"] = row["MemberId"];
                rowNew["NickName"] = row["NickName"];
                //修改记录值
                rowNew["age"] = row["age"] + "岁";
                dtResult.Rows.Add(rowNew);
            }
            //返回希望的结果
            return dtResult;
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone(); // 复制DataRow的表结构
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray); // 将DataRow添加到DataTable中
            return tmp;
        }

        /// <summary>
        /// 按照fieldName从sourceTable中选择出不重复的行，
        /// 并且包含sourceTable中所有的列。
        /// </summary>
        /// <param name="sourceTable">源表</param>
        /// <param name="fieldName">字段</param>
        /// <returns>一个新的不含重复行的DataTable</returns>
        public static DataTable Distinct(DataTable sourceTable, string fieldName)
        {
            DataTable dt = sourceTable.Clone();
            object lastValue = null;
            foreach (DataRow dr in sourceTable.Select("", fieldName))
            {
                if (lastValue == null || !(ColumnEqual(lastValue, dr[fieldName])))
                {
                    lastValue = dr[fieldName];
                    dt.Rows.Add(dr.ItemArray);
                }
            }
            return dt;
        }

        private static bool ColumnEqual(object objectA, object objectB)
        {
            if (objectA == DBNull.Value && objectB == DBNull.Value)
            {
                return true;
            }
            if (objectA == DBNull.Value || objectB == DBNull.Value)
            {
                return false;
            }
            return (objectA.Equals(objectB));
        }

        /// <summary>
        /// datatable行变列
        /// </summary>
        /// <param name="p_Table"></param>
        /// <returns></returns>
        public static DataTable GetReverseTable(DataTable p_Table)
        {
            DataTable _Table = new DataTable();
            for (int i = 0; i != p_Table.Rows.Count + 1; i++)
            {
                _Table.Columns.Add("Column" + i.ToString());
            }

            for (int i = 0; i != p_Table.Columns.Count; i++)
            {
                object[] _ObjectValue = new object[p_Table.Rows.Count + 1];
                _ObjectValue[0] = p_Table.Columns[i].ColumnName;
                for (int z = 0; z != p_Table.Rows.Count; z++)
                {
                    _ObjectValue[z + 1] = p_Table.Rows[z][i];
                }
                _Table.Rows.Add(_ObjectValue);
            }
            return _Table;
        }

        /// <summary>
        /// ColumnToRow
        /// </summary>
        /// <param name="src_dt">源表</param>
        /// <param name="columnIndex">需要行做列名的列索引</param>
        /// <param name="columnValue">需要行做列值列索引</param>
        /// <param name="key">分组的列索引</param>
        /// <returns></returns>
        public static DataTable ColumnToRow(DataTable src_dt, int columnIndex, int columnValue, int key)
        //columnIndex 用来当作新列名的列
        {
            int n = src_dt.Columns.Count;
            DataTable dt = new DataTable();
            List<string> li = SelectDistinct(src_dt, key);
            List<string> lt = SelectDistinct(src_dt, columnIndex);
            for (int d = 0; d < n; d++)
            {
                if (d == columnIndex)
                {
                    foreach (var item in lt)
                    {
                        dt.Columns.Add(item);
                    }
                }
                else
                {
                    if (d == columnIndex || d == columnValue) continue; //如果是被当作列名的列,则跳过
                    dt.Columns.Add(src_dt.Columns[d].ColumnName);
                }
            }
            int j = 0;
            string newColumnName = null;
            DataRow new_dr = null;
            foreach (var item in li)
            {
                new_dr = dt.NewRow();
                for (int i = 0; i < n; i++)
                {
                    foreach (DataRow dr2 in src_dt.Select(src_dt.Columns[key].ColumnName + "='" + item + "'"))
                    {
                        if (i == columnIndex)
                        {
                            newColumnName = dr2[columnIndex].ToString();

                            new_dr[newColumnName] = dr2[columnValue];
                        }
                        else
                        {
                            if (i == columnValue || i == columnValue) continue; //如果是被当作列名的列,则跳过
                            newColumnName = src_dt.Columns[i].ColumnName;
                            new_dr[newColumnName] = dr2[i];
                        }
                    }
                }
                dt.Rows.Add(new_dr);
                newColumnName = null;
                j++;
            }
            return dt;
        }

        public static List<string> SelectDistinct(DataTable SourceTable, int key) //去除重复项
        {
            List<string> li = new List<string>();
            if (key == -1)
            {
                li.Add("All");
                return li;
            }
            else
            {
                foreach (DataRow dr in SourceTable.Rows)
                {
                    if (!li.Contains(dr[key].ToString()))
                    {
                        li.Add(dr[key].ToString());
                    }
                }
                return li;
            }
        }
    }
}