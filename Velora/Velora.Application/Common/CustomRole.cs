namespace Velora.Application.Common
{
    public class CustomRole
    {
        public const string MasterAdmin = "MasterAdmin";
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public const string WorkspaceAdmin = MasterAdmin + "," + Admin;
    }
}
