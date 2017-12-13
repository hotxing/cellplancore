using System;
using System.Collections.Generic;
using System.Data;

namespace LengZX.SharePart.Utilities
{
    public class DatatableUtil
    {
        /// <summary>
        /// �޸����ݱ�DataTableĳһ�е����ͺͼ�¼ֵ(��ȷ���裺1.��¡��ṹ��2.�޸������ͣ�3.�޸ļ�¼ֵ��4.����ϣ���Ľ��)
        /// </summary>
        /// <param name="argDataTable">���ݱ�DataTable</param>
        /// <returns>���ݱ�DataTable</returns>
        private DataTable UpdateDataTable(DataTable argDataTable)
        {
            DataTable dtResult = new DataTable();
            //��¡��ṹ
            dtResult = argDataTable.Clone();
            foreach (DataColumn col in dtResult.Columns)
            {
                if (col.ColumnName == "age")
                {
                    //�޸�������
                    col.DataType = typeof(String);
                }     
            }
            foreach (DataRow row in argDataTable.Rows)
            {
                DataRow rowNew = dtResult.NewRow();
                rowNew["MemberId"] = row["MemberId"];
                rowNew["NickName"] = row["NickName"];
                //�޸ļ�¼ֵ
                rowNew["age"] = row["age"] + "��";
                dtResult.Rows.Add(rowNew);
            }
            //����ϣ���Ľ��
            return dtResult;
        }

        public static DataTable ToDataTable(DataRow[] rows)
        {
            if (rows == null || rows.Length == 0) return null;
            DataTable tmp = rows[0].Table.Clone(); // ����DataRow�ı�ṹ
            foreach (DataRow row in rows)
                tmp.Rows.Add(row.ItemArray); // ��DataRow��ӵ�DataTable��
            return tmp;
        }

        /// <summary>
        /// ����fieldName��sourceTable��ѡ������ظ����У�
        /// ���Ұ���sourceTable�����е��С�
        /// </summary>
        /// <param name="sourceTable">Դ��</param>
        /// <param name="fieldName">�ֶ�</param>
        /// <returns>һ���µĲ����ظ��е�DataTable</returns>
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
        /// datatable�б���
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
        /// <param name="src_dt">Դ��</param>
        /// <param name="columnIndex">��Ҫ����������������</param>
        /// <param name="columnValue">��Ҫ������ֵ������</param>
        /// <param name="key">�����������</param>
        /// <returns></returns>
        public static DataTable ColumnToRow(DataTable src_dt, int columnIndex, int columnValue, int key)
        //columnIndex ������������������
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
                    if (d == columnIndex || d == columnValue) continue; //����Ǳ�������������,������
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
                            if (i == columnValue || i == columnValue) continue; //����Ǳ�������������,������
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

        public static List<string> SelectDistinct(DataTable SourceTable, int key) //ȥ���ظ���
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