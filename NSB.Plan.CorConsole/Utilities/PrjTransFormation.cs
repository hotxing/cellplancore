using System;

namespace  LengZX.SharePart.Utilities
{
    /*************************************************************************************
   * CLR 版本：4.0.30319.269
   * 类 名 称：
   * 机器名称：2012-0203-2023
   * 命名空间：BLL_Base.BLL
   * 文 件 名：Class1
   * 创建时间：2012/3/11 14:52:09
   * 作    者：冷振兴
   * 修改时间：
   * 修 改 人：
   *************************************************************************************/

    /// <summary>
    ///Ellipsoid,
    /// <summary>
    /// Summary description for PrjTransFormation.
    /// </summary>   
    public struct LBCoord
    {
        public double lon; //经度
        public double lat; //纬度

        public LBCoord(double _lon,double _lat)
        {
            this.lon = _lon;
            this.lat = _lat;           
        }
    }

    public struct XYCoord
    {
        public double X;
        public double Y;

        public XYCoord(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public enum ellipsoid
    {
        Everest,
        Bassel,
        Clarke1880,
        Clarke1866,
        Hayford,
        Krassovsky,
        IUGG,
        IAG75,
        WGS84
    }

    public class Ellipsoid
    {
        public double a;

        public double b;
        public double alpha;
        public double e1;
        public double e2;

        public Ellipsoid()
        {
            //
            // TODO: Add constructor logic here
            //
            this.a = 6378245.0;

            b = 6356863.0188;
            alpha = (6378245.0 - 6356863.0188)/6378245.0;
            this.e1 = Math.Sqrt(2*alpha - alpha*alpha);
            this.e2 = Math.Sqrt(this.e1*this.e1/(1 - this.e1*this.e1));
        }

        public Ellipsoid(double _a, double _b)
        {
            this.a = _a;
            this.b = _b;
            this.alpha = (_a - _b)/_a;
            this.e1 = Math.Sqrt(2*this.alpha - this.alpha*this.alpha);
            this.e2 = Math.Sqrt(this.e1*this.e1/(1 - this.e1*this.e1));
        }

        public Ellipsoid(ellipsoid _ellipsoid)
        {
            switch (_ellipsoid)
            {
                case ellipsoid.Everest:
                    this.a = 6377276;
                    this.b = 6356075;
                    break;
                case ellipsoid.Bassel:
                    this.a = 6377379;
                    this.b = 6356079;
                    break;
                case ellipsoid.Clarke1880:
                    this.a = 6378249;
                    this.b = 6356515;
                    break;
                case ellipsoid.Clarke1866:
                    this.a = 6378206.400;
                    this.b = 6356584;
                    break;
                case ellipsoid.Hayford:
                    this.a = 6378388;
                    this.b = 6356912;
                    break;
                case ellipsoid.Krassovsky:
                    this.a = 6378245;
                    this.b = 6356863.0188;
                    break;
                case ellipsoid.IUGG:
                    this.a = 6378160;
                    this.b = 6356775;
                    break;
                case ellipsoid.IAG75:
                    this.a = 6378140;
                    this.b = 6356755.2882;
                    break;
                case ellipsoid.WGS84:
                    this.a = 6378137;
                    this.b = 6356752.3142;
                    break;
            }
            this.alpha = (this.a - this.b)/this.a;
            this.e1 = Math.Sqrt(2*this.alpha - this.alpha*this.alpha);
            this.e2 = Math.Sqrt(this.e1*this.e1/(1 - this.e1*this.e1));
        }
    }

    public class tools
    {
        public tools()
        {
            //
            // TODO: Add constructor logic here30°17'00"N

            //
        }

        public static double toDecimal(string strDms)
        {
            if ((strDms = strDms.Trim()) == "") return 0.0;
            char lastChr = strDms.ToCharArray()[strDms.ToCharArray().Length - 1];
            if (Char.IsLetter(lastChr))
                strDms = strDms.Remove(strDms.Length - 1, 1);


            char[] spliter = new char[] {'°', '\'', '\"'};
            string[] dms = strDms.Split(spliter);
            double result = 0.0;
            for (int i = 0; i < dms.Length - 1; i++)
            {
                double index = Math.Pow(60.0, (double) i);
                double temp = Convert.ToDouble(dms[i]);
                result += temp/index;
            }
            if (Char.ToUpper(lastChr) == 'W' || Char.ToUpper(lastChr) == 'S') result = -result;
            return result;
        }

        public static double toRad(double decimalDegree)
        {
            return decimalDegree*Math.PI/180.0;
        }

        public static double toDegree(double rad)
        {
            return rad/Math.PI*180.0;
        }

        public static double toUSSurveyFeet(double meters)
        {
            return meters*20925832.16/6378206.400;
        }

        public static string toDms(double decimalDegree)
        {
            string result = "";
            if (decimalDegree < 0) decimalDegree = -decimalDegree;
            double temp = decimalDegree;
            double[] tempArray = new double[3] {0.0, 0.0, 0.0};
            for (int i = 0; i < 3; i++)
            {
                tempArray[i] = Math.Floor(temp);
                temp -= tempArray[i];
                temp *= 60;
            }
            result = tempArray[0].ToString() + "°" + tempArray[1].ToString() + "\'" + tempArray[2].ToString() + "\"";
            return result;
        }
    }

    public class PrjTransFormation
    {
        public PrjTransFormation()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static XYCoord albers(ellipsoid ellip, LBCoord _lbc, double _log1, double _log2, double _log0,
            double _lat0, ref string result)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);

            double e = _ellip.e1;

            result += "e:" + e.ToString() + " a:" + _ellip.a.ToString() + "\n";
            result += "-------------------------------------\n";

            double l1 = _log1*Math.PI/180.0;
            double l2 = _log2*Math.PI/180.0;

            double l0 = _log0*Math.PI/180.0;
            double lmt0 = _lat0*Math.PI/180.0;

            double l = _lbc.lon*Math.PI/180.0;
            double lmt = _lbc.lat*Math.PI/180.0;


            result += "l1:" + l1.ToString() + "\n";

            result += "l2:" + l2.ToString() + "\n";
            result += "l0:" + l0.ToString() + "\n";
            result += "lmt0:" + lmt0.ToString() + "\n";
            result += "l:" + l.ToString() + "\n";
            result += "lmt:" + lmt.ToString() + "\n";
            result += "-----------------------\n";


            double alpha0 = getalpha(_ellip, l0);
            double alpha1 = getalpha(_ellip, l1);
            double alpha2 = getalpha(_ellip, l2);

            double alpha = getalpha(_ellip, l);

            result += "alpha0:" + alpha0.ToString() + "\n";
            result += "alpha1:" + alpha1.ToString() + "\n";
            result += "alpha2:" + alpha2.ToString() + "\n";
            result += "alpha:" + alpha.ToString() + "\n";


            double m1 = Math.Cos(l1)/Math.Pow(1 - e*e*Math.Sin(l1)*Math.Sin(l1), 0.5);
            double m2 = Math.Cos(l2)/Math.Pow(1 - e*e*Math.Sin(l2)*Math.Sin(l2), 0.5);

            result += "m1:" + m1.ToString() + "\n";
            result += "m2:" + m2.ToString() + "\n";

            double n = (m1*m1 - m2*m2)/(alpha2 - alpha1);
            //double c=_ellip.a*_ellip.a*(m1*m1+n*alpha1);
            double c = (m1*m1*alpha2 - m2*m2*alpha1)/(alpha2 - alpha1);
            result += "n:" + n.ToString() + "\n";
            result += "c:" + c.ToString() + "\n";

            double tht = n*(lmt - lmt0);
            double rol = (_ellip.a*Math.Sqrt(c - n*alpha))/n;
            double rol0 = (_ellip.a*Math.Sqrt(c - n*alpha0))/n;

            result += "tht:" + tht.ToString() + "\n";
            result += "rol:" + rol.ToString() + "\n";
            result += "rol0:" + rol0.ToString() + "\n";

            double E = rol*Math.Sin(tht);
            double N = rol0 - (rol*Math.Cos(tht));
            XYCoord xyc;
            xyc.X = E;
            xyc.Y = N;

            return xyc;
        }

        public static LBCoord reVersAlbers(ellipsoid ellip, XYCoord _xyc, double _log1, double _log2, double _log0,
            double _lat0, ref string result)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);

            double e = _ellip.e1;

            result += "e:" + e.ToString() + " a:" + _ellip.a.ToString() + "\n";
            result += "-------------------------------------\n";

            double l1 = _log1*Math.PI/180.0;
            double l2 = _log2*Math.PI/180.0;

            double l0 = _log0*Math.PI/180.0;
            double lmt0 = _lat0*Math.PI/180.0;

            double E = _xyc.X;
            double N = _xyc.Y;


            result += "l1:" + l1.ToString() + "\n";

            result += "l2:" + l2.ToString() + "\n";
            result += "l0:" + l0.ToString() + "\n";
            result += "lmt0:" + lmt0.ToString() + "\n";
            result += "E:" + E.ToString() + "\n";
            result += "N:" + N.ToString() + "\n";
            result += "-----------------------\n";


            double alpha1 = getalpha(_ellip, l1);
            double alpha2 = getalpha(_ellip, l2);
            double alpha0 = getalpha(_ellip, l0);


            double m1 = Math.Cos(l1)/Math.Sqrt(1 - e*e*Math.Sin(l1)*Math.Sin(l1));
            double m2 = Math.Cos(l2)/Math.Sqrt(1 - e*e*Math.Sin(l2)*Math.Sin(l2));


            result += "m1:" + m1.ToString() + "\nm2:" + m2.ToString() + "\n";
            double n = (m1*m1 - m2*m2)/(alpha2 - alpha1);
            double c = (m1*m1*alpha2 - m2*m2*alpha1)/(alpha2 - alpha1);

            result += "n:" + n.ToString() + "\nc:" + c.ToString() + "\n";

            double rol0 = (_ellip.a*Math.Sqrt(c - n*alpha0))/n;
            double tht = Math.Atan(E/(rol0 - N));
            double rol = Math.Sqrt(E*E + (rol0 - N)*(rol0 - N));
            double alpha = (c - rol*rol*n*n/(_ellip.a*_ellip.a))/n;
            //λO + (θ / n) ?' + (e2/3 + 31e4/180 + 517e6/5040) . sin 2?'] + [(23e4/360 + 251e6/3780) . sin 4?']
            //+ [(761e6/45360) . sin 6?']
            result += "rol0:" + rol0.ToString() + "\nrol:" + rol.ToString() + "\n";
            double beta = Math.Asin(alpha/(1 - (1 - e*e)/(2*e)*Math.Log((1 - e)/(1 + e), Math.E)));

            double lmt = (lmt0 + (tht/n))/Math.PI*180.0;

            double l = beta + (e*e/3 + 31*Math.Pow(e, 4)/180 + 517*Math.Pow(e, 6)/5040)*Math.Sin(2*beta) +
                       (23*Math.Pow(e, 4)/360
                        + 251*Math.Pow(e, 6)/3780)*Math.Sin(4*beta) + 761*Math.Pow(e, 6)/45360*Math.Sin(6*beta);

            return new LBCoord(lmt, (l/Math.PI*180.0));
        }

        private static double getalpha(Ellipsoid _ellip, double fai)
        {
            double e = _ellip.e1;
            double a = _ellip.a;
            double sin = Math.Sin(fai);
            double result = 0.0;
            result = (1 - e*e);
            double denominator = sin/(1 - e*e*sin*sin);
            //result=result/denominator;
            double ln = Math.Log((1 - e*sin)/(1 + e*sin), Math.E);
            denominator -= ln/(2*e);
            result *= denominator;
            return result;
            //return (1-e*e)*(Math.Sin(fai)/(1-e*e*Math.Sin(fai)*Math.Sin(fai)))-(((1/(2*e)))*Math.Log10((1-e*Math.Sin(fai))/(1+e*Math.Sin(fai))));
        }

        public static XYCoord lambert(ellipsoid ellip, LBCoord _lbc, double _log1, double _log2, double _log0,
            double _lat0, ref string result)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e = _ellip.e1;

            //result+="ellipsoid.Clarke1866\n"; 

            result += "e:" + e.ToString() + " a:" + _ellip.a.ToString() + "\n";
            result += "-------------------------------------\n";
            double log1 = _log1*Math.PI/180.0;
            double log2 = _log2*Math.PI/180.0;

            result += "log1:" + log1.ToString() + "\n";
            result += "log1:" + log2.ToString() + "\n";


            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;

            result += "log0:" + log0.ToString() + "\n";
            result += "lat0:" + lat0.ToString() + "\n";

            double log = _lbc.lon * Math.PI/180.0;
            double lat = _lbc.lat*Math.PI/180.0;

            result += "log:" + log.ToString() + "\n";
            result += "lat:" + lat.ToString() + "\n";


            //double jiao=Math.Asin(e*Math.Sin(l));

            result += "-------------------------------------\n";

            double m1 = Math.Cos(log1)/Math.Sqrt(1 - e*e*Math.Sin(log1)*Math.Sin(log1));
            double m2 = Math.Cos(log2)/Math.Sqrt(1 - e*e*Math.Sin(log2)*Math.Sin(log2));

            result += "m1:" + m1.ToString() + "\n";
            result += "m1:" + m2.ToString() + "\n";

            double t1 = Lam_gett(log1, e);
            double t2 = Lam_gett(log2, e);
            double tF = Lam_gett(log0, e);
            double t = Lam_gett(log, e);

            result += "t1:" + t1.ToString() + "\n";
            result += "t2:" + t2.ToString() + "\n";
            result += "tF:" + tF.ToString() + "\n";
            result += "t:" + t.ToString() + "\n";

            double n = Math.Log(m1/m2, Math.E)/Math.Log(t1/t2, Math.E);

            double F = m1/(n*Math.Pow(t1, n));

            double r = _ellip.a*F*Math.Pow(t, n);
            double rF = _ellip.a*(F*Math.Pow(tF, n));


            double tht = n*(lat - lat0);

            result += "a:" + _ellip.a.ToString() + "\n";
            result += "n:" + n.ToString() + "\n";
            result += "F:" + F.ToString() + "\n";
            result += "r:" + r.ToString() + "\n";
            result += "rF:" + rF.ToString() + "\n";
            result += "tht:" + tht.ToString() + "\n";

            return new XYCoord(r*Math.Sin(tht), rF - r*Math.Cos(tht));
        }

        public static LBCoord reversLambert(ellipsoid ellip, XYCoord _xyc, double _log1, double _log2, double _log0,
            double _lat0, ref string result)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);

            double e = _ellip.e1;

            result += "e:" + e.ToString() + " a:" + _ellip.a.ToString() + "\n";
            result += "-------------------------------------\n";

            double log1 = _log1*Math.PI/180.0;
            double log2 = _log2*Math.PI/180.0;

            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;

            double E = _xyc.X;
            double N = _xyc.Y;


            result += "l1:" + log1.ToString() + "\n";

            result += "l2:" + log2.ToString() + "\n";
            result += "log0:" + log0.ToString() + "\n";
            result += "lat0:" + lat0.ToString() + "\n";
            result += "E:" + E.ToString() + "\n";
            result += "N:" + N.ToString() + "\n";
            result += "-----------------------\n";

            double m1 = Math.Cos(log1)/Math.Sqrt(1 - e*e*Math.Sin(log1)*Math.Sin(log1));
            double m2 = Math.Cos(log2)/Math.Sqrt(1 - e*e*Math.Sin(log2)*Math.Sin(log2));

            result += "m1:" + m1.ToString() + "\n";
            result += "m1:" + m2.ToString() + "\n";

            double t1 = Lam_gett(log1, e);
            double t2 = Lam_gett(log2, e);
            double tF = Lam_gett(log0, e);


            result += "t1:" + t1.ToString() + "\n";
            result += "t2:" + t2.ToString() + "\n";
            result += "tF:" + tF.ToString() + "\n";


            double n = Math.Log(m1/m2, Math.E)/Math.Log(t1/t2, Math.E);
            double F = m1/(n*Math.Pow(t1, n));
            double rF = _ellip.a*(F*Math.Pow(tF, n));


            result += "n:" + n.ToString() + "\n";
            result += "F:" + F.ToString() + "\n";
            result += "rF:" + rF.ToString() + "\n";


            double EF = 0;
            double NF = 0.0;
            double tht = Math.Atan((E - EF)/(rF - (N - NF)));
            double r = Math.Sqrt((E - EF)*(E - EF) + (rF - (N - NF))*(rF - (N - NF)));
            if (n < 0) r = -r;
            double t = Math.Pow(r/(_ellip.a*F), 1/n);

            result += "tht:" + tht.ToString() + "\n";
            result += "r:" + r.ToString() + "\n";
            result += "t:" + t.ToString() + "\n";

            double log = Math.PI/2 - 2*Math.Atan(t);
            double prelog = log + 1;
            while (log != prelog)
            {
                prelog = log;
                log = Math.PI/2 - 2*Math.Atan(t*Math.Pow((1 - e*Math.Sin(log))/(1 + e*Math.Sin(log)), e/2));
            }
            double lat = tht/n + lat0;
            return new LBCoord(lat/Math.PI*180, log/Math.PI*180);
        }

        private static double Lam_gett(double log, double e)
        {
            //tan(π/4 – ?/2)/[(1 – e sin?)/(1 + e sin?)] e/2 for t1, t2, t F and t using ?1, ?2, ?F and ?
            //respectively
            //n = (ln m1 – ln m2)/(ln t1 – ln t2)

            return Math.Tan(Math.PI/4 - log/2)/Math.Pow((1 - e*Math.Sin(log))/(1 + e*Math.Sin(log)), e/2);
        }

        public static XYCoord UTM(ellipsoid ellip, bool isSouth_hemisphere, LBCoord _lbc, double _log0, double _lat0)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;
            double k0 = 0.9996;
            double FE = 500000;
            double FN = 0;
            if (isSouth_hemisphere)
                FN = 10000000;

            double a = _ellip.a;
            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;

            double log = _lbc.lon*Math.PI/180.0;
            double lat = _lbc.lat*Math.PI/180.0;


            double T = Math.Tan(log)*Math.Tan(log);
            double C = e1*e1*Math.Pow(Math.Cos(log), 2)/(1 - e1*e1);
            double A = (lat - lat0)*Math.Cos(log);
            double v = a/Math.Pow(1 - e1*e1*Math.Sin(log)*Math.Sin(log), 0.5);
            double M = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log -
                          (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log) +
                          (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log) -
                          (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log));
            double M0 = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log0 -
                           (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log0) +
                           (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log0) -
                           (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log0));



            double E = FE +
                       k0*v*(A + (1 - T + C)*Math.Pow(A, 3)/6 + (5 - 18*T + T*T + 72*C - 58*e2*e2)*Math.Pow(A, 5)/120);
            double N = FN +
                       k0*
                       (M - M0 +
                        v*Math.Tan(log)*
                        (A*A/2 + (5 - T + 9*C + 4*C*C)*Math.Pow(A, 4)/24 +
                         (61 - 58*T + T*T + 600*C - 330*e2*e2)*Math.Pow(A, 6)/720));

            return new XYCoord(E, N);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ellip">椭圆体</param>
        /// <param name="isSouth_hemisphere">是否南半球 UTM的“false easting”值为500km，而南半球UTM带的“false northing”为10000km</param>
        /// <param name="_xyc">投影经纬度</param>
        /// <param name="_log0">起始经度</param>
        /// <param name="_lat0">起始维度</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static LBCoord reverseUTM(ellipsoid ellip, bool isSouth_hemisphere, XYCoord _xyc, double _log0, double _lat0)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;
            double k0 = 0.9996;
            double FE = 500000;
            double FN = 0;
            if (isSouth_hemisphere)
                FN = 10000000;

            double a = _ellip.a;        

            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;         

            double E = _xyc.X;
            double N = _xyc.Y;          

            double M0 = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log0 -
                           (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log0) +
                           (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log0) -
                           (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log0));
            double M1 = M0 + (N - FN)/k0;
            double miu1 = M1/(a*(1 - e1*e1/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256));
            double _e1 = (1 - Math.Sqrt(1 - e1*e1))/(1 + Math.Sqrt(1 - e1*e1));
            double log1 = miu1 +
                          ((3*_e1/2 - 27*_e1*_e1*_e1/32)*Math.Sin(2*miu1) +
                           (21*_e1*_e1/16 - 55*Math.Pow(_e1, 4)/32)*Math.Sin(4*miu1)
                           + (151*Math.Pow(_e1, 3)/96)*Math.Sin(6*miu1) + (1097*Math.Pow(_e1, 4))*Math.Sin(8*miu1));

            double T1 = Math.Pow(Math.Tan(log1), 2);
            double C1 = Math.Pow(e2*Math.Cos(log1), 2);

            double v1 = a/Math.Sqrt(1 - Math.Pow(e1*Math.Sin(log1), 2));
            double D = (E - FE)/(v1*k0);

            double rol1 = a*(1 - e1*e1)/Math.Pow(1 - Math.Pow(e1*Math.Sin(log1), 2), 1.5);


            double log = log1 -
                         (v1*Math.Tan(log1)/rol1)*(D*D/2 - (5 + 3*T1 + 10*C1 - 4*C1*C1 - 9*e2*e2)*Math.Pow(D, 4)/24
                                                   +
                                                   (61 + 90*T1 + 298*C1 + 45*T1 - 252*e2*e2 - 3*C1*C1)*Math.Pow(D, 6)/
                                                   720);

            double lat = lat0 +
                            (D - (1 + 2*T1 + C1)*Math.Pow(D, 3)/6 +
                             (5 - 2*C1 + 28*T1 - 3*C1*C1 + 8*Math.Pow(e2, 2) + 24*T1*T1)*Math.Pow(D, 5)/120)/
                            Math.Cos(log1);
          
            return new LBCoord(tools.toDegree(lat), tools.toDegree(log));
        }

        /// <summary>
        /// Gauss_Kruger（BL转XY）
        /// </summary>
        /// <param name="ellip">椭球体</param>
        /// <param name="_lbc">经纬度</param>
        /// <param name="_log0">起始纬度</param>
        /// <param name="_lat0">中央经度</param>
        /// <param name="result"></param>
        /// <returns>BL</returns>
        public static XYCoord Gauss_Kruger(ellipsoid ellip, LBCoord _lbc, double _log0, double _lat0)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;
            double k0 = 1;
            double FE = 500000;
            double FN = 0;
            double a = _ellip.a;
            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;    

            double log = _lbc.lon*Math.PI/180.0;
            double lat = _lbc.lat*Math.PI/180.0;

            double T = Math.Tan(log)*Math.Tan(log);
            double C = e1*e1*Math.Pow(Math.Cos(log), 2)/(1 - e1*e1);
            double A = (lat - lat0)*Math.Cos(log);
            double v = a/Math.Pow(1 - e1*e1*Math.Sin(log)*Math.Sin(log), 0.5);
            double M = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log -
                          (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log) +
                          (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log) -
                          (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log));
            double M0 = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log0 -
                           (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log0) +
                           (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log0) -
                           (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log0));

            double E = FE +
                       k0*v*(A + (1 - T + C)*Math.Pow(A, 3)/6 + (5 - 18*T + T*T + 72*C - 58*e2*e2)*Math.Pow(A, 5)/120);
            double N = FN +
                       k0*
                       (M - M0 +
                        v*Math.Tan(log)*
                        (A*A/2 + (5 - T + 9*C + 4*C*C)*Math.Pow(A, 4)/24 +
                         (61 - 58*T + T*T + 600*C - 330*e2*e2)*Math.Pow(A, 6)/720));

            return new XYCoord(E, N);
        }

        /// <summary>
        /// Gauss_Kruger（XY转BL）
        /// </summary>
        /// <param name="ellip">椭球体</param>
        /// <param name="_xyc">经纬度</param>
        /// <param name="_log0">起始经度</param>
        /// <param name="_lat0">中央经度</param>
        /// <param name="result"></param>
        /// <returns>BL</returns>
        public static LBCoord reverseGauss_Kruger(ellipsoid ellip, XYCoord _xyc, double _log0, double _lat0)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;
            double k0 = 1;
            double FE = 500000;
            double FN = 0;


            double a = _ellip.a;

          
            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;

           

            double E = _xyc.X;
            double N = _xyc.Y;

           
            double M0 = a*((1 - Math.Pow(e1, 2)/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256)*log0 -
                           (3*Math.Pow(e1, 2)/8 + 3*Math.Pow(e1, 4)/32 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(2*log0) +
                           (15*Math.Pow(e1, 4)/256 + 45*Math.Pow(e1, 6)/1024)*Math.Sin(4*log0) -
                           (35*Math.Pow(e1, 6)/3027)*Math.Sin(6*log0));
            double M1 = M0 + (N - FN)/k0;
            double miu1 = M1/(a*(1 - e1*e1/4 - 3*Math.Pow(e1, 4)/64 - 5*Math.Pow(e1, 6)/256));
            double _e1 = (1 - Math.Sqrt(1 - e1*e1))/(1 + Math.Sqrt(1 - e1*e1));
            double log1 = miu1 +
                          ((3*_e1/2 - 27*_e1*_e1*_e1/32)*Math.Sin(2*miu1) +
                           (21*_e1*_e1/16 - 55*Math.Pow(_e1, 4)/32)*Math.Sin(4*miu1)
                           + (151*Math.Pow(_e1, 3)/96)*Math.Sin(6*miu1) + (1097*Math.Pow(_e1, 4))*Math.Sin(8*miu1));

            double T1 = Math.Pow(Math.Tan(log1), 2);
            double C1 = Math.Pow(e2*Math.Cos(log1), 2);

            double v1 = a/Math.Sqrt(1 - Math.Pow(e1*Math.Sin(log1), 2));
            double D = (E - FE)/(v1*k0);

            double rol1 = a*(1 - e1*e1)/Math.Pow(1 - Math.Pow(e1*Math.Sin(log1), 2), 1.5);

            double log = log1 - (v1*Math.Tan(log1)/rol1)*(D*D/2 - (5 + 3*T1 + C1 - 9*T1*C1)*Math.Pow(D, 4)/24
                                                          + (61 + 90*T1 + 45*T1*T1)*Math.Pow(D, 6)/720);

            double lat = lat0 +
                            (D - (1 + 2*T1 + C1)*Math.Pow(D, 3)/6 +
                             (5 + 6*C1 + 28*T1 + 8*T1*C1 + 24*T1*T1)*Math.Pow(D, 5)/120)/Math.Cos(log1);

            return new LBCoord(tools.toDegree(lat), tools.toDegree(log));
        }

        public static XYCoord Mercator(ellipsoid ellip, LBCoord _lbc, double _log1, double _log0, double _lat0 )
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;

            double FE = 0;
            double FN = 0;

            //result+="ellipsoid.Clarke1866\n";
            double a = _ellip.a; 
            double log0 = _log0*Math.PI/180.0;
            double lat0 = _lat0*Math.PI/180.0;
            double log = _lbc.lon * Math.PI/180.0;
            double lat = _lbc.lat*Math.PI/180.0;        

            double log1 = tools.toRad(_log1);
            double k0 = Math.Cos(log1)/Math.Sqrt(1 - Math.Pow(e1*Math.Sin(log1), 2));

            double E = FE + a*k0*(lat - lat0);
            double N = FN +
                       a*k0*
                       Math.Log(
                           Math.Tan(Math.PI/4 + log/2)*Math.Pow((1 - e1*Math.Sin(log))/(1 + e1*Math.Sin(log)), e1/2),
                           Math.E);

            return new XYCoord(E, N);
        }

        public static LBCoord reverseMercator(ellipsoid ellip, XYCoord _xyc, double _log1, double _log0, double _lat0)
        {
            Ellipsoid _ellip = new Ellipsoid(ellip);
            double e1 = _ellip.e1;
            double e2 = _ellip.e2;

            double FE = 0;
            double FN = 0;

            //result+="ellipsoid.Clarke1866\n";
            double a = _ellip.a;

            //a=6377563.396;
            //e1=Math.Sqrt(0.00667054);
            //e2=Math.Sqrt(0.00671534);           

            double lat0 = _lat0*Math.PI/180.0;            

            double E = _xyc.X;
            double N = _xyc.Y;
            
            double log1 = tools.toRad(_log1);
            double k0 = Math.Cos(log1)/Math.Sqrt(1 - Math.Pow(e1*Math.Sin(log1), 2));

            double t = Math.Pow(Math.E, (FN - N)/(a*k0));
            double x = Math.PI/2 - 2*Math.Atan(t);
            double log = x +
                         (e1*e1/2 + 5*Math.Pow(e1, 4)/24 + Math.Pow(e1, 6)/12 + 13*Math.Pow(e1, 8)/360)*Math.Sin(2*x) +
                         (7*Math.Pow(e1, 4)/48 + 29*Math.Pow(e1, 6)/240 + 811*Math.Pow(e1, 8)/11520)*Math.Sin(4*x)
                         + (7*Math.Pow(e1, 6)/120 + 81*Math.Pow(e1, 8)/1120)*Math.Sin(6*x) +
                         (4279*Math.Pow(e1, 8)/161280)*Math.Sin(8*x);
            //double log=log1;
            //double templog=log+1;
            //while(log!=templog)
            //{   
            //	templog=log;
            //	log=Math.PI/2-2*Math.Atan(Math.Pow(Math.E,-N/k0)*Math.Pow(Math.E,e1/2*Math.Log((1-e1*Math.Sin(log))/(1+e1*Math.Sin(log)),Math.E)));
            //}
            double lat = (E - FE)/(a*k0) + lat0;            
            return new LBCoord(tools.toDegree(lat), tools.toDegree(log));
        }
    }
}