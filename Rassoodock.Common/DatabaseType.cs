using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rassoodock.Common
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DatabaseType
    {
        [EnumMember(Value = "notsetup")]
        NotSetUp,
        [EnumMember(Value = "mysql")]
        Mysql,
        [EnumMember(Value = "sqlserver")]
        SqlServer,
        [EnumMember(Value = "postgres")]
        PostgreSql,
        [EnumMember(Value = "sqlite")]
        Sqlite
    }
}
