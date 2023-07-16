namespace INFINITE.CORE.MVC.Navigations
{
    public class NavigationContext
    {
        public List<NavigationDefinition> MainMenu = new List<NavigationDefinition>();
        public NavigationContext AddItem(NavigationDefinition navigation)
        {
            MainMenu.Add(navigation);
            return this;
        }
    }
}
