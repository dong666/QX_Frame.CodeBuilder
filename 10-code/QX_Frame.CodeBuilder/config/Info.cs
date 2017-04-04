﻿using System;
using System.Text;

namespace CSharp_FlowchartToCode_DG
{
    public abstract class Info
    {
        //版权信息通用模块
        public static readonly string VersionNum = "4.1.0";         //版本号
        public static readonly string Author = "qixiao(柒小)";        //作者
        public static readonly string Description = $"qixiao code builder start at:{DateTime.Now.ToString("yyyyMMdd HH:mm:ss")}";

        //设置版权信息
        public static string CopyRight
        {
            get
            {
                #region 版权信息
                StringBuilder str = new StringBuilder();    //版本信息
                str.Append("\t" + "/// <summary>" + "\r\n");
                str.Append("\t" + "/// copyright qixiao code builder ->" + "\r\n");
                str.Append("\t" + "/// version:" + VersionNum + "\r\n");
                str.Append("\t" + "/// author:" + Author + "\r\n");
                str.Append("\t" + "/// time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                str.Append("\t" + "/// </summary>" + "\r\n");
                #endregion
                return str.ToString(); ;
            }
        }

    }
}