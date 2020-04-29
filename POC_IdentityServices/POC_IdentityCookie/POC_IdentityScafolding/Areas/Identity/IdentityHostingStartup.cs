using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POC_IdentityScafolding.Areas.Identity.Data;
using POC_IdentityScafolding.Data;

[assembly: HostingStartup(typeof(POC_IdentityScafolding.Areas.Identity.IdentityHostingStartup))]
namespace POC_IdentityScafolding.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthenticationsContextPOC>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthenticationsContextConnection")));// Jorge

                services.AddDefaultIdentity<ApplicationUserPOC>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<AuthenticationsContextPOC>();// Jorge
            });
        }
    }
}