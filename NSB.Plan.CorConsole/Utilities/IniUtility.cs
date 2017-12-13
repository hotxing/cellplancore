using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LengZX.SharePart.Utilities
{
    /*************************************************************************************
    * CLR�汾��       4.0.30319.42000
    * �� �� �ƣ�      IniUtility
    * �������ƣ�      LENGZHENXINGNOK
    * �����ռ䣺      NSB.XDRAnalyse.Common
    * �� �� ����      Class1
    * ����ʱ�䣺      2015/12/11 11:05:57
    * ��    �ߣ�      ������
    * ��    ��:
    * �޸�ʱ�䣺
    * �� �� �ˣ�
    *************************************************************************************/

    /// <summary>
    ///     INI�ļ��Ĳ�����
    /// </summary>
    public class IniUtility
    {
        public Dictionary<string, string> configData;
        public string fullFileName;

        public void Config(string _fullFileName)
        {
            configData = new Dictionary<string, string>();
            fullFileName = _fullFileName;

            //var hasCfgFile = File.Exists(_fullFileName);
            //if (hasCfgFile == false)
            //{
            //    var writer = new StreamWriter(File.Create(_fullFileName), Encoding.Default);
            //    writer.Close();
            //}

            FileStream fs = new FileStream(@_fullFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader reader = new StreamReader(fs, Encoding.Default);

            //var reader = new StreamReader(@_fullFileName, Encoding.Default);
            string line;

            var indx = 0;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith(";") || string.IsNullOrEmpty(line))
                    configData.Add(";" + indx++, line);
                else
                {
                    var key_value = line.Split('=');
                    if (key_value.Length >= 2)
                        configData.Add(key_value[0], key_value[1]);
                    else
                        configData.Add(";" + indx++, line);
                }
            }
            reader.Close();
        }

        public string get(string key)
        {
            if (configData.Count <= 0)
                return null;
            if (configData.ContainsKey(key))
                return configData[key];
            return null;
        }

        public void set(string key, string value)
        {
            if (configData.ContainsKey(key))
                configData[key] = value;
            else
                configData.Add(key, value);
        }

        public void save()
        {
            var writer = new StreamWriter(fullFileName, false, Encoding.Default);
            IDictionaryEnumerator enu = configData.GetEnumerator();
            while (enu.MoveNext())
            {
                if (enu.Key.ToString().StartsWith(";"))
                    writer.WriteLine(enu.Value);
                else
                    writer.WriteLine(enu.Key + "=" + enu.Value);
            }
            writer.Close();
        }
    }
}