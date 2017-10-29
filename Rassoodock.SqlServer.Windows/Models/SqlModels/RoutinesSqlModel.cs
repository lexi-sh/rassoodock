namespace Rassoodock.SqlServer.Windows.Models.SqlModels
{
    public class RoutinesSqlModel
    {
        public string definition { get; set; }

        public string Specific_Schema { get; set; }

        public string Specific_Name { get; set; }

        public bool uses_ansi_nulls { get; set; }

        public bool uses_quoted_identifier { get; set; }
    }
}
