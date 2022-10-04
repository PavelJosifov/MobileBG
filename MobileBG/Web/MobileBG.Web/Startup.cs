﻿namespace MobileBG.Web;

using MobileBG.Web.Hubs;
using System.Reflection;
using System.Text;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(
            //options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));
            options => options.UseSqlServer("Server=.;Database=MobileBG;Trusted_Connection=True;MultipleActiveResultSets=true"));

        services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
            .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<CookiePolicyOptions>(
            options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

        services.AddControllersWithViews(
            options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
        services.AddRazorPages();
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(this.configuration);
        services.AddSignalR();

        // Data repositories
        services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IDbQueryRunner, DbQueryRunner>();

        // Application services
        services.AddTransient<IEmailSender, NullMessageSender>();
        services.AddTransient<ICarService, CarService>();
        services.AddTransient<IModelService, ModelService>();
        services.AddTransient<IDropDownDataService, DropDownDataService>();
        services.AddTransient<ICloudinaryService, CloudinaryService>();
        services.AddTransient<IImageService, ImageService>();
        services.AddTransient<IStatsService, StatsService>();
        services.AddTransient<IMakeService, MakeService>();
        services.AddTransient<ICityService, CityService>();
        services.AddTransient<IBlogService, BlogService>();
        services.AddTransient<IEmailSender>(
                 serviceProvider => new SendGridEmailSender(Common.Encoding.Base64Decode(this.configuration.GetSection("SendGrid:ApiKey").Value)));

        services.AddAuthentication().AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = this.configuration["Facebook:AppId"];
            facebookOptions.AppSecret = this.configuration["Facebook:AppSecret"];
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

        // Seed data on application startup
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
            new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(
            endpoints =>
                {
                    endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                    endpoints.MapHub<ChatHub>("/chatHub");
                });
    }
}
