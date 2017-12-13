/*
 * 日志类，不同于其他开源项目需要引用dll 
 * 代码简短，支持多线程写同一日志文件不冲突。
 * 主要代码也来自网络，进行过优化和格式修改
 * wisecn  2017-1-13 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;


namespace LengZX.SharePart.Utilities
{
    public class LogThread
    {
        private static readonly Thread WriteThread;
        private static readonly Queue<string> MsgQueue;
        private static readonly object FileLock;
        private static readonly string FilePath;

        static LogThread()
        {
            FileLock = new object();
            FilePath = "log"+Path.DirectorySeparatorChar;
            WriteThread = new Thread(WriteMsg);
            WriteThread.IsBackground = true;
            MsgQueue = new Queue<string>();
            WriteThread.Start();
        }

        public static void WriteInfoLog(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{0}[{1}]{2}\r\n", " Info", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), msg));
            Monitor.Exit(MsgQueue);
        }
        public static void WriteErrorLog(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{0}[{1}]{2}\r\n", "Error", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), msg));
            Monitor.Exit(MsgQueue);
        }
        public static void WriteDebugLog(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{0}[{1}]{2}\r\n", "Debug", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), msg));
            Monitor.Exit(MsgQueue);
        }
        private static void WriteMsg()
        {
            while (true)
            {
                if (MsgQueue.Count > 0)
                {
                    Monitor.Enter(MsgQueue);
                    string msg = MsgQueue.Dequeue();
                    Monitor.Exit(MsgQueue);

                    Monitor.Enter(FileLock);

                    string _path = FilePath + msg.Substring(0, 5).Trim() + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM");

                    if (!Directory.Exists(_path))
                    {
                        Directory.CreateDirectory(_path);
                    }
                    string fileName = _path + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                    msg = msg.Remove(0, 5);
                    var logStreamWriter = new StreamWriter(fileName, true);
                    logStreamWriter.WriteLine(msg);
                    logStreamWriter.Close();
                    logStreamWriter.Dispose();
                    Monitor.Exit(FileLock);
                    if (GetFileSize(fileName) > 1024 * 2)
                    {
                        CopyToBak(fileName);
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(5);
                }
            }
        }


        private static long GetFileSize(string fileName)
        {
            long strRe = 0;
            if (File.Exists(fileName))
            {
                Monitor.Enter(FileLock);
                var myFs = new FileStream(fileName, FileMode.Open);
                strRe = myFs.Length / 1024;
                myFs.Close();
                myFs.Dispose();
                Monitor.Exit(FileLock);
            }
            return strRe;
        }
        private static void CopyToBak(string sFileName)
        {
            int fileCount = 0;
            string sBakName = "";
            Monitor.Enter(FileLock);
            do
            {
                fileCount++;
                sBakName = sFileName + "." + fileCount + ".BAK";
            }
            while (File.Exists(sBakName));

            File.Copy(sFileName, sBakName);
            File.Delete(sFileName);
            Monitor.Exit(FileLock);
        }
    }
}
