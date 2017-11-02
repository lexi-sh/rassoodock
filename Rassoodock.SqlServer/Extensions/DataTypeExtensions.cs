using System.Linq;
using System.Reflection;
using Rassoodock.Common;
using Rassoodock.SqlServer.Models.Domain;

namespace Rassoodock.SqlServer.Extensions
{
    public static class DataTypeExtensions
    {
        public static string ToSql(this DataTypeCls dt)
        {
            var type = typeof(DataType);
            var memInfo = type.GetMember(dt.DataType.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(SqlAttribute), false);
            return ((SqlAttribute) attributes.First()).SqlString;
        }
    }
}
