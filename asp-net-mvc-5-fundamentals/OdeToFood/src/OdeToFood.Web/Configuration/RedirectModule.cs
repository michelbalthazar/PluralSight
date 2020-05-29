using System;
using System.Web;
using System.Web.Configuration;

namespace OdeToFood.Web.Configuration
{
    public class RedirectModule : IHttpModule
    {

        private HttpApplication _context;

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            _context = context;
            context.MapRequestHandler += RedirectUrls;
        }

        public void RedirectUrls(object src, EventArgs args)
        {
            //RedirectSection section = (RedirectSection)WebConfigurationManager.GetWebApplicationSection("redirects");
            //foreach (Redirect item in section.Redirects)
            //{
            //    if (item.Old == _context.Request.RequestContext.HttpContext.Request.RawUrl)
            //    {
            //        _context.Response.Redirect(item.New);
            //    }
            //}
        }
    }
}