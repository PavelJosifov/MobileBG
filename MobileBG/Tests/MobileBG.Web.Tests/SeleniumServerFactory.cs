namespace MobileBG.Web.Tests;
using System;
using System.Linq;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileBG.Data;

public sealed class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    public SeleniumServerFactory()
    {
        this.ClientOptions.BaseAddress = new Uri("https://localhost");
        var host = WebHost.CreateDefaultBuilder(Array.Empty<string>()).UseStartup<TStartup>().Build();
        host.Start();
        this.RootUri = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault();
        var testServer = new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
    }

    public string RootUri { get; set; }

    public class FakeStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
           options => options.UseSqlServer("Server=.;Database=MobileBG;Trusted_Connection=True;MultipleActiveResultSets=true"));
        }

        public void Configure()
        {
        }
    }
}
