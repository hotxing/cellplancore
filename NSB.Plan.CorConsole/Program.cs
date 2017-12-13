using System;

namespace NSB.Plan.CorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //BLL.ProgramExe.MainExe();
            BLL.AnalyseGis bllAnalyseGis = new BLL.AnalyseGis();
            bllAnalyseGis.Main();
        }
    }
}
