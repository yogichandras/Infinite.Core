using System.Reflection;

namespace INFINITE.CORE.Data.Provider
{
    public static class PermissionNames
    {
        public const string Pages_Home = "Pages.Home";
        public const string Pages_Users = "Pages.Users";
        public const string Pages_Roles = "Pages.Roles";
        public const string Pages_Config = "Pages.Config";
    }

    public static class Permissions
    {
        public static List<string> List()
        {
            List<string> permissionList = new List<string>();

            Type permissionType = typeof(PermissionNames);
            FieldInfo[] fields = permissionType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string permissionName = (string)field.GetValue(null);
                    permissionList.Add(permissionName);
                }
            }

            return permissionList;
        }
    }
}
