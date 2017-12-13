using System;
using System.Collections.Generic;
using System.IO;

namespace LengZX.SharePart.Utilities
{
    public class LogRecord
    {
        private static object lockWriteTXT = new object();
        private static object lockCsvWriteTXT = new object();

        public static void writeLog(string message)
        {
            try
            {
                string filePath = "log" + Path.DirectorySeparatorChar + "RecordLog.txt";
                StreamWriter writer = null;
                if (File.Exists(filePath))
                {
                    writer = File.AppendText(filePath);
                }
                else
                {
                    writer = File.CreateText(filePath);
                }
                writer.WriteLine(String.Format("[{0}]:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message),
                    System.Text.Encoding.GetEncoding("gb2312"));
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void writeLog(string message, string strPath)
        {
            try
            {
                lock (lockWriteTXT)
                {
                    StreamWriter writer = null;
                    if (Directory.Exists("log") == false)
                    {
                        Directory.CreateDirectory("log");
                    }
                    if (File.Exists(strPath))
                    {
                        writer = File.AppendText(strPath);
                    }
                    else
                    {
                        writer = File.CreateText(strPath);
                    }
                    writer.WriteLine(String.Format("{0},{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message));
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void writeLogYYYYMMDD(string message, string strPath)
        {
            try
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
                writer.WriteLine(String.Format("{0:yyyy-MM-dd}:{1}", System.DateTime.Now, message),
                    System.Text.Encoding.GetEncoding("gb2312"));
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void writeLogANCYYYYMMDD(string message, string strPath)
        {
            try
            {
                StreamWriter writer = null;
                if (FileUtil.IsExistFile(strPath) == true) ;
                {
                    writer = File.AppendText(strPath);
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                     "《=================REGION================》");
                    writer.WriteLine(message);
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                                     "《=================REGION================》");
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void writeBSS(string message, string strPath)
        {
            try
            {
                StreamWriter writer = null;
                //File.AppendAllText(strPath,message,System.Text.Encoding.GetEncoding(936));

                if (File.Exists(strPath))
                {
                    writer = File.AppendText(strPath);
                }
                else
                {
                    writer = File.CreateText(strPath);
                }
                writer.WriteLine(message);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void writeSomcsys(string message, string strPath)
        {
            try
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
                writer.WriteLine(message);
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void WriteTXT(String message, string strPath)
        {
            try
            {
                lock (lockCsvWriteTXT)
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
                    writer.WriteLine(message);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void WriteTXT(String message, string strPath, int Flag)
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

                writer.WriteLine(message);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中
            }
        }

        public static void WriteTXT(List<String> SQLStringList, string strPath)
        {
            StreamWriter writer = null;
            try
            {
                if (File.Exists(strPath))
                {
                    writer = File.AppendText(strPath);
                }
                else
                {
                    writer = File.CreateText(strPath);
                }

                for (int i = 0; i < SQLStringList.Count; i++)
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
    }
}