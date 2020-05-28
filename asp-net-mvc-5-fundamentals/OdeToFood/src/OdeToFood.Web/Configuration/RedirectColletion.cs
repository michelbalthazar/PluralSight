using System.Configuration;

namespace OdeToFood.Web.Configuration
{
    public class RedirectColletion : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Redirect();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Redirect)element).Title;
        }
    }
}