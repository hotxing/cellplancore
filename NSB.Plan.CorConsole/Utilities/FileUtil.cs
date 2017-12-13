using System;
using System.Web;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Collections;

namespace LengZX.SharePart.Utilities
{
    /// <summary> 
    /// 文件操作类 
    /// </summary> 
    public static class FileUtil
    {
        #region 检测指定目录是否存在

        /// <summary> 
        /// 检测指定目录是否存在 
        /// </summary> 
        /// <param name="directoryPath">目录的绝对路径</param>         
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        #endregion

        #region 检测指定文件是否存在

        /// <summary> 
        /// 检测指定文件是否存在,如果存在则返回true。 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region 检测指定目录是否为空

        /// <summary> 
        /// 检测指定目录是否为空 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param>         
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件 
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }
                //判断是否存在文件夹 
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //txtlogHelper  .(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }

        #endregion

        #region 检测指定目录中是否存在指定的文件

        /// <summary> 
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法. 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>         
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表 
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);
                //判断指定文件是否存在 
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary> 
        /// 检测指定目录中是否存在指定的文件 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>  
        /// <param name="isSearchChild">是否搜索子目录</param> 
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表 
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);
                //判断指定文件是否存在 
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return false;
            }
        }

        #endregion

        #region 创建一个目录

        /// <summary> 
        /// 创建一个目录 
        /// </summary> 
        /// <param name="directoryPath">目录的绝对路径</param> 
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录 
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion

        #region 创建一个文件

        /// <summary> 
        /// 创建一个文件。 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件 
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象 
                    FileInfo file = new FileInfo(filePath);
                    //创建文件 
                    FileStream fs = file.Create();
                    //关闭文件流 
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        /// <summary> 
        /// 创建一个文件,并将字节流写入文件。 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="buffer">二进制流数据</param> 
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件 
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象 
                    FileInfo file = new FileInfo(filePath);
                    //创建文件 
                    FileStream fs = file.Create();
                    //写入二进制流 
                    fs.Write(buffer, 0, buffer.Length);
                    //关闭文件流 
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }

        #endregion

        #region 获取文本文件的行数

        /// <summary> 
        /// 获取文本文件的行数 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中 
            string[] rows = File.ReadAllLines(filePath);
            //返回行数 
            return rows.Length;
        }

        #endregion

        #region 获取一个文件的长度

        /// <summary> 
        /// 获取一个文件的长度,单位为Byte 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象 
            FileInfo fi = new FileInfo(filePath);
            //获取文件的大小 
            return (int)fi.Length;
        }

        /// <summary> 
        /// 获取一个文件的长度,单位为KB 
        /// </summary> 
        /// <param name="filePath">文件的路径</param>         


        ///// <summary> 
        ///// 获取一个文件的长度,单位为MB 
        ///// </summary> 
        ///// <param name="filePath">文件的路径</param>         
        //public static double GetFileSizeByMB(string filePath)
        //{
        //    //创建一个文件对象 
        //    FileInfo fi = new FileInfo(filePath);
        //    //获取文件的大小 
        //    return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length)/1024/1024, 1);
        //}

        #endregion

        #region 获取指定目录中的文件列表

        /// <summary> 
        /// 获取指定目录中所有文件列表 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param>         
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常 
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            //获取文件列表 
            return Directory.GetFiles(directoryPath);
        }

        /// <summary> 
        /// 获取指定目录及子目录中所有文件列表 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param> 
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常 
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 获取指定目录中的子目录列表

        /// <summary> 
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法. 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param>         
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 获取指定目录及子目录中所有子目录列表 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。 
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param> 
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 向文本文件写入内容

        /// <summary> 
        /// 向文本文件中写入内容 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="content">写入的内容</param>         
        public static void WriteText(string filePath, string content)
        {
            //向文件写入内容 
            File.WriteAllText(filePath, content);
        }

        #endregion

        #region 向文本文件的尾部追加内容

        /// <summary> 
        /// 向文本文件的尾部追加内容 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="content">写入的内容</param> 
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }

        #endregion

        #region 将现有文件的内容复制到新文件中

        /// <summary> 
        /// 将源文件的内容复制到目标文件中 
        /// </summary> 
        /// <param name="sourceFilePath">源文件的绝对路径</param> 
        /// <param name="destFilePath">目标文件的绝对路径</param> 
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }

        #endregion

        #region 将文件移动到指定目录

        /// <summary> 
        /// 将文件移动到指定目录 
        /// </summary> 
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param> 
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param> 
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称 
            string sourceFileName = GetFileName(sourceFilePath);
            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除 
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录 
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }

        #endregion

        #region 将流读取到缓冲区中

        /// <summary> 
        /// 将流读取到缓冲区中 
        /// </summary> 
        /// <param name="stream">原始流</param> 
        //public static byte[] StreamToBytes(Stream stream)
        //{
        //    try
        //    {
        //        //创建缓冲区 
        //        byte[] buffer = new byte[stream.Length];
        //        //读取流 
        //        stream.Read(buffer, 0, ConvertHelper.ToInt32(stream.Length));
        //        //返回流 
        //        return buffer;
        //    }
        //    catch (Exception ex)
        //    {
        //        //CommonUtil.LogHelper  WriteTraceLog(TraceLogLevel.Error, ex.Message);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //关闭流 
        //        stream.Close();
        //    }
        //}

        #endregion

        #region 将文件读取到缓冲区中

        /// <summary> 
        /// 将文件读取到缓冲区中 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        public static byte[] FileToBytes(string filePath)
        {
            //获取文件的大小  
            int fileSize = GetFileSize(filePath);
            //创建一个临时缓冲区 
            byte[] buffer = new byte[fileSize];
            //创建一个文件流 
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Open(FileMode.Open);
            try
            {
                //将文件流读入缓冲区 
                fs.Read(buffer, 0, fileSize);
                return buffer;
            }
            catch (IOException ex)
            {
                //this.log.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
            finally
            {
                //关闭文件流 
                fs.Close();
            }
        }

        #endregion

        #region 将文件读取到字符串中

        /// <summary> 
        /// 将文件读取到字符串中 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        //public static string FileToString(string filePath)
        //{
        //    //return FileToString(filePath, BaseInfo.DefaultEncoding);
        //}
        /// <summary> 
        /// 将文件读取到字符串中 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        /// <param name="encoding">字符编码</param> 
        public static string FileToString(string filePath, Encoding encoding)
        {
            //创建流读取器 
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                //读取流 
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                //TxtLogHelper txtlogHelper = new TxtLogHelper();

                //txtlogHelper..WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
            finally
            {
                //关闭流读取器 
                reader.Close();
            }
        }

        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )

        /// <summary> 
        /// 从文件的绝对路径中获取文件名( 包含扩展名 ) 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetFileName(string filePath)
        {
            //获取文件的名称 
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }

        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )

        /// <summary> 
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 ) 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称 
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }

        #endregion

        #region 从文件的绝对路径中获取扩展名

        /// <summary> 
        /// 从文件的绝对路径中获取扩展名 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static string GetExtension(string filePath)
        {
            //获取文件的名称 
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }

        #endregion

        #region 清空指定目录

        /// <summary> 
        /// 清空指定目录下所有文件及子目录,但该目录依然保存. 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件 
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                //删除目录中所有的子目录 
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        #endregion

        #region 清空文件内容

        /// <summary> 
        /// 清空文件内容 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        public static void ClearFile(string filePath)
        {
            //删除文件 
            File.Delete(filePath);
            //重新创建该文件 
            CreateFile(filePath);
        }

        #endregion

        #region 删除指定文件

        /// <summary> 
        /// 删除指定文件 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param> 
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion

        #region 删除指定目录

        /// <summary> 
        /// 删除指定目录及其所有子目录 
        /// </summary> 
        /// <param name="directoryPath">指定目录的绝对路径</param> 
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        #endregion


        /// <summary>
        /// 拷贝的文件夹到两位一个文件夹
        /// </summary>
        /// <param name="strFromPath">源文件夹</param>
        /// <param name="strToPath">目的文件夹</param>
        public static void CopyFolder(string strFromPath, string strToPath)
        {
            //如果源文件夹不存在，则创建
            if (!Directory.Exists(strFromPath))
            {
                Directory.CreateDirectory(strFromPath);
            }

            //取得要拷贝的文件夹名
            string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("\\") +
                                                         1, strFromPath.Length - strFromPath.LastIndexOf("\\") - 1);

            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
            if (!Directory.Exists(strToPath + "\\" + strFolderName))
            {
                Directory.CreateDirectory(strToPath + "\\" + strFolderName);
            }
            //创建数组保存源文件夹下的文件名
            string[] strFiles = Directory.GetFiles(strFromPath);

            //循环拷贝文件
            for (int i = 0; i < strFiles.Length; i++)
            {
                //取得拷贝的文件名，只取文件名，地址截掉。
                string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1,
                    strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
                //开始拷贝文件,true表示覆盖同名文件
                File.Copy(strFiles[i], strToPath + "\\" + strFolderName + "\\" + strFileName, true);
            }

            //创建DirectoryInfo实例
            DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
            //取得源文件夹下的所有子文件夹名称
            DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
            for (int j = 0; j < ZiPath.Length; j++)
            {
                //获取所有子文件夹名
                string strZiPath = strFromPath + "\\" + ZiPath[j].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
                CopyFolder(strZiPath, strToPath + "\\" + strFolderName);
            }
        }

        public static void ChangeTxtContent(string TxtFile, string OldItem_Rivername, string NewItem_Rivername,
            string OldItem_chainage, string NewItem_chainage, string OldItem_sectionID, string NewItem_sectionID)
        {
            StreamReader tr = new StreamReader(TxtFile);
            tr.ReadToEnd().Replace(OldItem_Rivername, NewItem_Rivername);
            tr.ReadToEnd().Replace(OldItem_chainage, NewItem_chainage);
            tr.ReadToEnd().Replace(OldItem_sectionID, NewItem_sectionID);
            tr.Close();
        }

        public static int GetTxtFileLineNum(string filepath)
        {
            int linecount = 0;
            using (StreamReader reader = new StreamReader(filepath))
            {
                while (reader.ReadLine() != null)
                {
                    linecount++;
                }
                //Console.WriteLine("共有 {0} 行，耗时： {1}", linecount, watch.Elapsed);
            }
            return linecount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static List<string> ReadCsv(string strpath, Encoding ed)
        {
            List<string> listData = new List<string>();
            if (File.Exists(strpath) == true)
            {
                FileStream fs = new FileStream(strpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                StreamReader sr = new StreamReader(fs, ed);
                //记录每次读取的一行记录
                string strLine = "";
                //逐行读取CSV中的数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    listData.Add(strLine);
                }
                sr.Close();
                fs.Close();
            }

            return listData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strpath"></param>
        /// <returns></returns>
        public static List<string> ReadCsv(string strpath)
        {
            StreamReader objReader = new StreamReader(strpath);
            string sLine = "";
            List<string> LineList = new List<string>();
            while (sLine != null)
            {
                sLine = objReader.ReadLine().Replace("\"", "");
                if (sLine != null && !sLine.Equals(""))
                    LineList.Add(sLine);
            }
            objReader.Close();
            return LineList;
        }
        /// <summary>
        /// 取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }

        /// <summary>
        /// 取得一个文本文件的编码方式。
        /// </summary>
        /// <param name="fileName">文件名。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            Encoding targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }

        /// <summary>
        /// 取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream">文本文件流。</param>
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>
        /// <returns></returns>
        public static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }

                //根据文件流的前4个字节判断Encoding
                //Unicode {0xFF, 0xFE};
                //BE-Unicode {0xFE, 0xFF};
                //UTF8 = {0xEF, 0xBB, 0xBF};
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8
                {
                    targetEncoding = Encoding.UTF8;
                }

                //恢复Seek位置      
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }



        /// <summary>
        /// 向文本文件中写一行记录
        /// </summary>
        /// <param name="message"></param>
        /// <param name="strPath"></param>
        public static void WriteFile(string message, string strPath)
        {
            try
            {
                //string filePath = Application.StartupPath + "\\Log\\RecordLog.txt";
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
            catch (Exception e)
            {
                //把错误日志写到另一个文本文件中 
            }
        }

        /// <summary>
        /// 向文本文件中写字符串 flag -1 删除源文件
        /// </summary>
        /// <param name="message"></param>
        /// <param name="strPath"></param>
        /// <param name="flag"></param>
        public static void WriteFile(string message, string strPath, int flag)
        {
            try
            {
                //string filePath = Application.StartupPath + "\\Log\\RecordLog.txt";
                StreamWriter writer = null;
                if (File.Exists(strPath))
                {
                    if (flag == 1)
                    {
                        File.Delete(strPath);
                        writer = File.CreateText(strPath);
                        writer.Flush();
                        writer.Close();
                    }
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

        /// <summary>
        /// 和属性一起使用listFile获取pathname文件下面所有的文件
        /// </summary>
        /// <param name="pathname"></param>
        public static void GetAllFiles(string pathname)
        {
            string[] subFiles = Directory.GetFiles(pathname);
            foreach (string subFile in subFiles)
            {
                listFile.Add(subFile);
            }
            string[] subDirs = Directory.GetDirectories(pathname);
            foreach (string subDir in subDirs)
            {
                GetAllFiles(subDir);
            }
        }



        /// <summary>
        /// 存储文件数和GetAllFiles()一起使用
        /// </summary>
        public static List<string> listFile = new List<string>();
    }
}