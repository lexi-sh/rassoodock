using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Rassoodock.Common
{
    public class LinkedDatabase
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public DatabaseType DatabaseType { get; set; }

        public string FolderLocation { get; set; }
    }
}
