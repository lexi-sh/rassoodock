using System;

namespace Rassoodock.Common
{
    public class SqlAttribute : Attribute
    {
        public string SqlString { get; set; }

        public SqlAttribute(string sqlString)
        {
            SqlString = sqlString;
        }
    }
}
