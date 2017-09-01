using System;
using System.Text;

namespace CSharp_FlowchartToCode_DG
{
    public abstract class Info
    {
        /**
         * author:qixiao
         * update 4.2.0:2017-4-4 16:39:40
         * */

        //版权信息通用模块
        public const string VersionNum = "4.2.0";         //版本号
        public const string Author = "qixiao(柒小)";        //作者
        public static readonly string Description = $"qixiao code builder start at:{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}";

        //设置版权信息
        public static string CopyRight
        {
            
            get
            {
                #region 版权信息
                StringBuilder str = new StringBuilder();    //版本信息
                str.Append("/*********************************************************" + "\r\n");
                str.Append(" * CopyRight: QIXIAO CODE BUILDER. \r\n");
                str.Append(" * Version:" + VersionNum + "\r\n");
                str.Append(" * Author:" + Author + "\r\n");
                str.Append(" * Create:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                str.Append(" * Update:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                str.Append(" * E-mail: dong@qixiao.me | wd8622088@foxmail.com \r\n");
                str.Append(" * Personal WebSit: http://qixiao.me \r\n");
                str.Append(" * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ \r\n");
                str.Append(" * Description:\r\n");
                str.Append(" * Thx , Best Regards ~\r\n");
                str.Append(" *********************************************************/" + "\r\n");
                #endregion
                return str.ToString(); ;
            }
        }

    }
}
