using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NSB.Plan.CorConsole
{
     public static class PublicModule
    {
        
        //Var ed = FileHelper.GetEncoding(this.strMenu);
        //var  lstTemp = FileHelper.ReadCsv(this.strMenu, ed);
        public static string strErrorLogPath = "log" + Path.DirectorySeparatorChar + "OperatorLog.txt";

        public static string strFilePath_Projection = "Include"+Path.DirectorySeparatorChar+"GIS"+Path.DirectorySeparatorChar+"planet"+Path.DirectorySeparatorChar+"clutter"+Path.DirectorySeparatorChar+"projection";
        public static string strFilePath_Clutter_Index = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" + Path.DirectorySeparatorChar + "index";
        public static string strFilePath_Clutter_Menu = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" + Path.DirectorySeparatorChar + "menu";
        public static string strPath_Clutter = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" ;
        public static string strFilePath_Data_Cell = "Include" + Path.DirectorySeparatorChar + "Data" + Path.DirectorySeparatorChar + "GC.csv";

        //public static string strFilePath_Clutter_Index = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" + Path.DirectorySeparatorChar + "projection";
        //public static string strFilePath_Clutter_Index = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" + Path.DirectorySeparatorChar + "projection";
        //public static string strFilePath_Clutter_Index = "Include" + Path.DirectorySeparatorChar + "GIS" + Path.DirectorySeparatorChar + "planet" + Path.DirectorySeparatorChar + "clutter" + Path.DirectorySeparatorChar + "projection";
        //Include"+Path.DirectorySeparatorChar+"GIS"+Path.DirectorySeparatorChar+"planet"+Path.DirectorySeparatorChar+"clutter"+Path.DirectorySeparatorChar+"projection
    }
    public struct ClutterDomoModel
    {
        public string FileName;
        public double GridX1;
        public double GridX2;
        public double GridY1;
        public double GridY2;
        public double DIM;
        public double Division;
        public double RowsCount;
        public double ColsCount;
    }
}
