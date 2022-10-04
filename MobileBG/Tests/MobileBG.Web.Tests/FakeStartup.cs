using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MobileBG.Data;

namespace MobileBG.Web.Tests
{
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
