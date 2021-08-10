using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BethanysPieShop.Areas.Identity.IdentityHostingStartup))]
namespace BethanysPieShop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}