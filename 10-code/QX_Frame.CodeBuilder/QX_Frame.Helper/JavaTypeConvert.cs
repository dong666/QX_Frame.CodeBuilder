namespace CSharp_FlowchartToCode_DG.QX_Frame.Helper
{
    public class JavaTypeConvert
    {
        public static string SqlTypeStringToJavaTypeString(string sqlTypeString)
        {
            switch (sqlTypeString)
            {
                case "varchar": return "String";
                case "bit": return "Boolean";
                case "bigint": return "BigInteger";
                case "longtext": return "String";
                case "datetime": return "Timestamp";
                case "date": return "Date";
                case "time": return "Time";
                case "int": return "Integer";
                case "tinyint": return "Integer";
                case "decimal": return "BigDecimal";
                case "double": return "Double";
                case "binary": return "byte[]";
                case "char": return "String";
                case "timestamp": return "Timestamp";
                case "set": return "String";
                case "enum": return "String";
                case "float": return "Float";
                case "longblob": return "byte[]";
                case "mediumtext": return "String";
                case "mediumblob": return "byte[]";
                case "smallint": return "Integer ";
                case "text": return "String";
                case "blob": return "byte[]";
                case "geometry": return "Geometry";
                case "year": return "Date";
                case "mediumint": return "Integer ";
            }
            return string.Empty;
        }
    }
}
