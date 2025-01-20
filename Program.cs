using BookStore.Data;
using BookStore.Middleware;
using BookStore.Models;
using BookStore.RegistrationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BookStore
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            RegisterService(builder);


            var app = builder.Build();

            ConfigurationApp(app);
        }

        public static void RegisterService(WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            // DB settings
            var settings = new DatabaseSettings();
            builder.Configuration.GetSection("DatabaseSettings").Bind(settings);
            builder.Services.AddSingleton(settings);



            // Injecting
            builder.Services.AddDbContext<BookStoreContext>(options =>
                options.UseSqlite(
                    settings.ConnectionString
                    ?? throw new InvalidOperationException("Connection string 'BookStoreContext' not found.")
                    )
                );

            //builder.Services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            //});

            // Only use if the reverse porxy doesn't handle HTTPS redirection
            //builder.Services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 5001;
            //});

            // identity
            builder.Services.AddDefaultIdentity<UserIdentity>(options =>
            {

            }).AddEntityFrameworkStores<BookStoreContext>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("CanUpdateBook",
                policyBuilder => policyBuilder.AddRequirements(
                    new CanUpdateBook((byte)Permissions.Manager)));


            // if DI is required > Scoped else Singleton
            builder.Services.AddScoped<IAuthorizationHandler, IsManagerHandler>();
            builder.Services.AddLogging(options =>
            {
                options.AddConsole();
                options.AddDebug();
            });

        }


        public static void ConfigurationApp(WebApplication app)
        {

            //app.UseForwardedHeaders();


            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<BookStoreContext>();
                context.Database.EnsureCreated();
                SeedData.Initilize(service);

            }
            app.UseMiddleware<ErrorHandling>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseRequestDuration();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Books/Error", "?statusCode={0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseRouting();

            // Order for the both are important
            app.UseAuthentication();
            app.UseAuthorization();
            // *****
            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Books}/{action=Home}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }

    }
}





