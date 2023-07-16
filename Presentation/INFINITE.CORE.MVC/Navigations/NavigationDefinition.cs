namespace INFINITE.CORE.MVC.Navigations
{
    public class NavigationDefinition
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Target { get; set; }
        public List<string> RequiredPermissions { get; set; }
        public List<NavigationDefinition> Child = new List<NavigationDefinition>();
        public List<string> AuthenticatedPermissions { get; set; }
        public NavigationDefinition(string title, string url, string icon, List<string>? requiredPermissions = null)
        {
            Title = title;
            Url = url;
            Icon = icon;
            if (requiredPermissions != null)
            {
                RequiredPermissions = requiredPermissions;
            }
        }

        public NavigationDefinition AddItem(NavigationDefinition navigation)
        {
            Child.Add(navigation);
            return this;
        }

        public List<string> GetAllPermissions()
        {
            var permissions = new List<string>();
            SetPermissions(this, permissions);
            return permissions.GroupBy(x => x).Select(x => x.Key).ToList();
        }

        private void SetPermissions(NavigationDefinition navigation, List<string> permissions)
        {
            permissions.AddRange(navigation.RequiredPermissions);
            if (navigation.Child.Count > 0)
            {
                foreach (var item in navigation.Child)
                {
                    SetPermissions(item, permissions);
                }
            }
        }
    }
}
