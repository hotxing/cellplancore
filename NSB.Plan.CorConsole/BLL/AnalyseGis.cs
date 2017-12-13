using LengZX.SharePart.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static NSB.Plan.CorConsole.PublicModule;

namespace NSB.Plan.CorConsole.BLL
{
    /*************************************************************************************
    * CLR版本：       4.0.30319.42000
    * 类 名 称：      AnalyseGis
    * 机器名称：      LENGNOKIA
    * 命名空间：      NSB.Plan.CorConsole.BLL
    * 文 件 名：      AnalyseGis
    * 创建时间：      2017/11/23 11:50:52
    * 作    者：      冷振兴
    * 功    能:
    * 修改时间：
    * 修 改 人：
    *************************************************************************************/
    /// <summary>
    /// Add Describe Here
    /// </summary>
	public class AnalyseGis
	{
        #region 公共方法

        /// <summary>
        /// 1公参导入并转换投影GPS-UTM（Atoll PR）
        /// 2
        /// </summary>
        public void Main()
        {
            var end = FileUtil.GetEncoding(PublicModule.strFilePath_Projection);
            var lstFilePath_Projection = FileUtil.ReadCsv(PublicModule.strFilePath_Projection, end);
            var lstFilePath_Clutter_Index = FileUtil.ReadCsv(PublicModule.strFilePath_Clutter_Index, end);
            var lstFilePath_Clutter_Menu = FileUtil.ReadCsv(PublicModule.strFilePath_Clutter_Menu, end);
            //栅格类型存入字典表
            foreach (var item in lstFilePath_Clutter_Menu)
            {
                var arrDic = System.Text.RegularExpressions.Regex.Split(item.Trim(), @"\s{1,}");
                dicMenu.TryAdd(int.Parse(arrDic[0]), arrDic[1]);
               
            }
            
            if (lstFilePath_Projection.Count>0)
            {
                string[] strPro = Regex.Split(lstFilePath_Projection[3], @"\s+");
                double.TryParse(strPro[1], out dblCenX);

                //南北半球判断
                if (double.Parse(strPro[2]) == 500000)
                {
                    isSouth_hemisphere = false;
                }
                else
                {
                    isSouth_hemisphere = true;
                }

                dicDataCell = GetDic_DataCell();
                AnalyseClutterIndex(lstFilePath_Clutter_Index);
                GetClutter();
            }            
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// double[intpci,intpcarrierfr,log,lat,px,py]
        /// 公参导入并转换投影GPS-UTM（Atoll PR）
        /// </summary>
        /// <returns></returns>
        private Dictionary<long, double[]> GetDic_DataCell()
        {
            var end = FileUtil.GetEncoding(PublicModule.strFilePath_Data_Cell);
            System.IO.StreamReader objReader = new StreamReader(PublicModule.strFilePath_Data_Cell, end);
            var dicDataCell = new Dictionary<long, double[]>();
            string sLine = "";
            List<string> LineList = new List<string>();
            bool IsFirst = true;
            long lgKey;
            while (sLine != null)
            {
                try
                {
                    if (IsFirst == true)
                    {
                        sLine = objReader.ReadLine().Replace("\"", "");
                        IsFirst = false;
                    }
                    else
                    {
                        sLine = objReader.ReadLine();
                        if (sLine != null && !sLine.Equals(""))
                        {
                            lgKey = long.Parse(sLine.Split(',')[10]);

                            double[] dbl = new double[6];
                            if (double.TryParse(sLine.Split(',')[12], out dbl[0]) == true && double.TryParse(sLine.Split(',')[63], out dbl[1]) == true
                                && double.TryParse(sLine.Split(',')[35], out dbl[2]) == true && double.TryParse(sLine.Split(',')[34], out dbl[3]) == true)
                            {
                                var entXYCoord = PrjTransFormation.UTM(ellipsoid.WGS84, isSouth_hemisphere, new LBCoord(dbl[2], dbl[3]), 0, dblCenX);
                                dbl[4] = Math.Round( entXYCoord.X,2);
                                dbl[5] = Math.Round(entXYCoord.Y,2);

                                dicDataCell.TryAdd(lgKey, dbl);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                   
                }              
            }
            objReader.Close();
            return dicDataCell;
        }
        /// <summary>
        /// 绑定clutterindex中的文件名；
        /// </summary>
        private void AnalyseClutterIndex(List<string> lstFilePath_Clutter_Index)
        {           
            //this.listbClutter.DataSource = fileList;
            foreach (string item in lstFilePath_Clutter_Index)
            {
                listClutterDomoModel.Add(GetHeadFile(item));
            }
            //GetHeadFile(txtDomobPath.Text.Substring(0, txtDomobPath.Text.LastIndexOf('\\')) + "\\index.txt");
        }

        private ClutterDomoModel GetHeadFile(string headFileName)
        {
            ClutterDomoModel headFile = new ClutterDomoModel();
            string[] tempStrings;
            //FileStream fs = File.OpenRead(headFileName);
            //StreamReader sr = new StreamReader(fs);
            Regex regex = new Regex(@"\s+");

            string tempString = regex.Replace(headFileName, " ");

            //tempString = sr.ReadLine();
            tempStrings = tempString.Split(' ');
            if (tempStrings.Length > 0)
            {
                headFile.FileName = tempStrings[0];
                headFile.GridX1 = (int)Math.Round(float.Parse(tempStrings[1]), 0);
                headFile.GridX2 = (int)Math.Round(float.Parse(tempStrings[2]), 0);
                headFile.GridY1 = (int)Math.Round(float.Parse(tempStrings[3]), 0);
                headFile.GridY2 = (int)Math.Round(float.Parse(tempStrings[4]), 0);
                headFile.Division = (int)float.Parse(tempStrings[5]);
                //headFile.RowsCount = (headFile.GridX2 - headFile.GridX1) / headFile.Division;
                //headFile.ColsCount = (headFile.GridY2 - headFile.GridY1) / headFile.Division;
                headFile.RowsCount = (headFile.GridY2 - headFile.GridY1) / headFile.Division;
                headFile.ColsCount = (headFile.GridX2 - headFile.GridX1) / headFile.Division;
            }
            //fs.Close();
            headFile.DIM = 50;
            return headFile;
        }
               
        private void GetClutter()
        {
            if (listClutterDomoModel.Count > 0)
            {
                int intGridTypeNew = 0;
                double dblCount = 0;
                
                foreach (ClutterDomoModel model in listClutterDomoModel)
                {
                    FileStream fs =
                        new FileStream(
                            String.Format("{0}" + Path.DirectorySeparatorChar + "{1}", PublicModule.strPath_Clutter, model.FileName),
                            FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    arrarrIndex = new double[int.Parse(Math.Round(model.RowsCount,0).ToString())+1,int.Parse(Math.Round(model.ColsCount,0).ToString()) + 1];
                    for (int row = 0; row < model.RowsCount; row++)
                    {
                        for (int col = 0; col < model.ColsCount; col++)
                        {
                            try
                            {
                                intGridTypeNew = br.ReadByte() + br.ReadByte();
                                dblCount++;
                            }
                            catch
                            {
                                break;
                            }
                            if (dicMenu.ContainsKey(intGridTypeNew) == true)
                            {
                                arrarrIndex[row, col] = intGridTypeNew;                            
                            }
                            else
                            {
                                arrarrIndex[row, col] = -1;
                            }
                        }
                    }                  
                }
            }
        }

        #endregion

        #region 属性声明

        /// <summary>
        /// 存放公参信息并转为UTM
        /// </summary>
        Dictionary<long, double[]> dicDataCell = new Dictionary<long, double[]>();
        /// <summary>
        /// 存放clutter信息
        /// </summary>
        double[,] arrarrIndex = null;


        private double dblCenX = 0;
        private bool isSouth_hemisphere = false;
       
        //Clutter列表和实体
        List<ClutterDomoModel> listClutterDomoModel = new List<ClutterDomoModel>();

        Dictionary<int, string> dicMenu = new Dictionary<int, string>();
       
        #endregion
    }
}
