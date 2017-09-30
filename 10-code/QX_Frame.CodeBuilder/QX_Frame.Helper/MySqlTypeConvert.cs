﻿/*********************************************************
 * CopyRight: QIXIAO CODE BUILDER. 
 * Version:5.0.0
 * Author:qixiao(柒小)
 * Create:2017-09-30 09:38:03
 * Update:2017-09-30 09:38:03
 * E-mail: dong@qixiao.me | wd8622088@foxmail.com 
 * GitHub: https://github.com/dong666 
 * Personal web site: http://qixiao.me 
 * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ 
 * Description:
 * Thx , Best Regards ~
 *********************************************************/

namespace CSharp_FlowchartToCode_DG.QX_Frame.Helper
{
    internal class MySqlTypeConvert
    {
        public static string SqlTypeStringToJavaTypeString(string mySqlTypeString)
        {
            switch (mySqlTypeString)
            {
                case "bit": return "Boolean";
                case "binary": return "byte[]";
                case "blob": return "byte[]";
                case "mediumblob": return "byte[]";
                case "longblob": return "byte[]";
                case "int": return "Integer";
                case "smallint": return "Integer ";
                case "mediumint": return "Integer ";
                case "bigint": return "BigInteger";
                case "tinyint": return "Integer";
                case "float": return "Float";
                case "double": return "Double";
                case "decimal": return "BigDecimal";
                case "char": return "String";
                case "varchar": return "String";
                case "mediumtext": return "String";
                case "text": return "String";
                case "longtext": return "String";
                case "enum": return "String";
                case "set": return "String";
                case "date": return "Date";
                case "datetime": return "Timestamp";
                case "year": return "Date";
                case "time": return "Time";
                case "timestamp": return "Timestamp";
                case "geometry": return "Geometry";
                default:return "Object";
            }
        }
    }
}
