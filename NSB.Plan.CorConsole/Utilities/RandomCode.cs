using System;
using System.Threading;

namespace LengZX.SharePart.Utilities
{
    public class RandomCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="n">验证码的个数</param>
        /// <returns>返回生成的随机数</returns>
        public static string RandomNum(int n) //
        {
            string strchar =
                "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] VcArray = strchar.Split(',');
            string VNum = ""; //
            int temp = -1; //记录上次随机数值，尽量避免产生几个一样的随机数
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < n + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(61);
                if (temp != -1 && temp == t)
                {
                    return RandomNum(n);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum; //返回生成的随机数
        }

        /// <summary>
        /// 返回一个19位不重复的长整形
        /// </summary>
        /// <returns></returns>
        public static Int64 RandomNum()
        {
            //int intTemp = 0;
            //long tick = DateTime.Now.Ticks;
            //Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            //StringBuilder sb = new StringBuilder(19);
            //for (int i = 1; i <= 19; i++)
            //{
            //    sb.Append(ran.Next(0, 10));
            //}
            //return Int64.Parse(sb.ToString());
            Thread.Sleep(100);

            Random rand = new Random();
            string strNum = "";
            strNum = DateTime.Now.ToString("yyMMddHHmmssffff") + rand.Next(100, 999).ToString();
            return Int64.Parse(strNum);
        }

        public static string getGUID()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();          
            return guid.ToString();
        }
    }
}