using System.Configuration;

namespace OdeToFood.Web.Configuration
{
    public class RedirectSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(RedirectColletion))]
        public RedirectColletion Redirects
        {
            get { return (RedirectColletion)base[""]; }
        }
    }
}