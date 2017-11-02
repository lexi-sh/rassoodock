namespace Rassoodock.SqlServer.Models.Domain
{
    public class ObjectPermission
    {
        public string StateDescription { get; set; } // GRANT, GRANT_WITH_GRANT_OPTION, REVOKE, DENY

        public string PermissionName { get; set; } // SELECT, INSERT

        public string User { get; set; }
    }
}
