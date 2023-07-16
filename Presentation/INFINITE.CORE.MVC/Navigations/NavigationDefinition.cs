namespace INFINITE.CORE.MVC.Navigations
{
    public class NavigationDefinition
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<string> RequiredPermissions { get; set; }

        public NavigationDefinition(string title, string url, string icon, List<string> requiredPermissions)
        {
            Title = title;
            Url = url;
            Icon = icon;
            RequiredPermissions = requiredPermissions;
        }
    }
}
