using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace LengZX.SharePart.Utilities
{
    /*************************************************************************************
    * CLR版本：       4.0.30319.42000
    * 类 名 称：      CompressHelper
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
    ///  解压缩GZ
    /// </summary>
    public class CompressHelper
    {
        /// <summary>
        ///  GZ压缩
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="zipFile"></param>
        public static void Compress(string srcFile, string zipFile)
        {
            var fsSrc = File.Open(srcFile, FileMode.Open);
            var fsDes = File.Create(zipFile);
            var compress = new GZipStream(fsDes, CompressionMode.Compress);

            try
            {
                var buffer = new byte[fsSrc.Length];
                fsSrc.Read(buffer, 0, (int)fsSrc.Length);

                compress.Write(buffer, 0, buffer.Length);
                compress.Flush();
            }
            finally
            {
                compress.Close();
                fsDes.Close();
                fsSrc.Close();
            }
        }

        /// <summary>
        ///     GZ解压
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="desFile"></param>
        public static void Decompress(string zipFile, string desFile)
        {
            var fsSrc = File.Open(zipFile, FileMode.Open);
            var compress = new GZipStream(fsSrc, CompressionMode.Decompress);
            if (File.Exists(desFile))
            {
                File.Delete(desFile);
            }

            var fsDes = File.Create(desFile);

            try
            {
                var buffer = new byte[1024 * 10];

                var i = 0;
                while ((i = compress.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (i > 0)
                    {
                        fsDes.Write(buffer, 0, i);
                    }
                }
                fsDes.Flush();
            }
            finally
            {
                fsDes.Close();
                compress.Close();
                fsSrc.Close();
            }
        }

        public static byte[] Decompress(string strzipFile)
        {
            MemoryStream tempMs = new MemoryStream();
            if (strzipFile.IndexOf(".gz") > 0)
            {
                using (var fsSrc = File.Open(strzipFile, FileMode.Open, FileAccess.Read))
                {
                    var Decompress = new GZipStream(fsSrc, CompressionMode.Decompress);
                    Decompress.CopyTo(tempMs);
                    Decompress.Close();
                }
            }
            else if (strzipFile.IndexOf(".zip") > 0)
            {
                using (ZipArchive zipArchive = System.IO.Compression.ZipFile.Open(strzipFile, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                    {
                        if (entry.Name.ToUpper().Contains("XML"))
                        {
                            using (System.IO.Stream stream = entry.Open())
                            {
                                int b = -1;
                                while ((b = stream.ReadByte()) != -1)
                                {
                                    tempMs.WriteByte((byte)b);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            return tempMs.ToArray();
        }

        #region 压缩解压object

        /// <summary>
        /// zip
        /// </summary>
        /// <param name="DataOriginal"></param>
        /// <returns></returns>
        public static byte[] CompressionObject(object DataOriginal)
        {
            if (DataOriginal == null) return null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            bFormatter.Serialize(mStream, DataOriginal);
            byte[] bytes = mStream.ToArray();
            MemoryStream oStream = new MemoryStream();
            DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Flush();
            zipStream.Close();
            return oStream.ToArray();
        }

        public static object DecompressionObject(byte[] bytes)
        {
            if (bytes == null) return null;
            MemoryStream mStream = new MemoryStream(bytes);
            mStream.Seek(0, SeekOrigin.Begin);
            DeflateStream unZipStream = new DeflateStream(mStream, CompressionMode.Decompress, true);
            object dsResult = null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            dsResult = (object)bFormatter.Deserialize(unZipStream);
            return dsResult;
        }

        #endregion 压缩解压object

        /// <summary>
        ///  返回字节流
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] bytes)
        {
            using (var tempMs = new MemoryStream())
            {
                using (var ms = new MemoryStream(bytes))
                {
                    var Decompress = new GZipStream(ms, CompressionMode.Decompress);
                    Decompress.CopyTo(tempMs);
                    Decompress.Close();
                    return tempMs.ToArray();
                }
            }
        }

        /// <summary>
        /// 压缩字节流成GZ字节流
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytes)
        {
            using (var ms = new MemoryStream())
            {
                var Compress = new GZipStream(ms, CompressionMode.Compress);
                Compress.Write(bytes, 0, bytes.Length);
                Compress.Close();
                return ms.ToArray();
            }
        }
    }
}