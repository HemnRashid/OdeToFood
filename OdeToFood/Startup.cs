using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OdeToFood.Data;
using OdeToFood.Services;

namespace OdeToFood
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // många services måste läggas till explicit beoende på vad man vill appen ska göra.
            // AddSingelton is a way to tell ASP.Net Core to if any component across the entire application need an I Greeter,
            //give them an instance of greeter and use samme instance throughout the application everywhere for every request.
            services.AddSingleton<IGreeter, Greeter>();

            // Scoped is just a way to telling ASP .net core, any component that needs IRestaurantData create an new instance for each http request and reused that instance throughout that one request.
            // after that you can throw it away and create another instance for the next request. 
            // That is an HTTP request scoped lifetime for my service and thats typically what you would want for a data access component.
            //services.AddScoped<IRestaurantData, InMemoryRestaurants>();


            // denna metod är inte bästa valet och användsw här för demo syfte. denna metod kan ställa till problem när multi-users och insättning görs eftersom Imemory använder List collection som inte är multithread safe. 
            // använder statisk mock data från en list


            // services.AddSingleton<IRestaurantData, InMemoryRestaurants>();

            services.AddDbContext<OdeToFoodDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("OdeToFood"))); // Db entity framework services and connection to the sql connectionstring from the json configuration file.
            services.AddScoped<IRestaurantData, SqlRestaurantData>(); // single logical thread


            // lägger till tjänst/service för att hantera vyer och skriva ut de i html. bla.
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; // forcing the user to authenticate with openidconnect
            })
            .AddOpenIdConnect(options =>
            {
                _configuration.Bind("AzureAd", options); // binder det som finns i appsettings till dess namn. dvs ClientId och Authority
            })
            .AddCookie();
            // dessa två ovan används sedan av middleware app.UseAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IGreeter greeter, ILogger<Startup> logger)
        {



            // Alla riktiga middleware som man installerar börjar med Usexxx, dessa är alla tilläggs metoder för IApplicationBulder.. finns många extentions ladda ner som nuget paket.
            // tex


            // här skapar jag en liten middleware för att logga requesten och sedan lämna tillbaka den till next.
            // denna hörs en gång per http request
            //app.Use(next=> 
            //    {
            //    return  async context => 
            //    {
            //        logger.LogInformation("request incoming ");
            //        if(context.Request.Path.StartsWithSegments("/mym"))
            //        {
            //            await context.Response.WriteAsync("Hit!!");
            //        }
            //        else
            //        {
            //            await next(context);
            //            logger.LogInformation("Response outgoing");
            //        }

            //    };
            //    });

            // denna är en enkel middleware tillägg.
            // innebär att welcome page triggas när den får en path som är "/wp" annars kommer mitt greeting att visas.
            //app.UseWelcomePage(new WelcomePageOptions {Path="/wp" });


            // servicen IhostingEnvoriment env hämtar state från launchSettings.json under properties.
            // en middleware för use developerExceptionPage
            if (env.IsDevelopment())
            {
                // en middleware används som lyssnare för att ge mer detalierad info tillbaka till användaren, vilkeet används för debugging om vad som gick fel när ett expetion sker.
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            // OBS. en middleware som är en substitute  för både app.use default files och usestaticfiles middleware.
            //app.UseFileServer();

            // 1.en middleware som anger en default file .. index.html är default som standard.
            // denna rad måste komma före app.UseStaticFiles. 
            // app.UseDefaultFiles();


            // 2. En middleware som tillåter att visa statiska filer som ligger under wwwroot sturkturen.
            app.UseStaticFiles();

            app.UseNodeModules(env.ContentRootPath);
            

            // MVC middleware, forward reqest to a controller class.
            //app.UseMvcWithDefaultRoute();

            app.UseAuthentication();

            app.UseMvc(configureRoutes);


            // registerar simpel middleware 
            // app.Run anväds mycket sällan, kanske för enklare saker som middleware som tex skriver direkt till response eller visa upp.text.. det går göra lite json och html bearbeting här men inte för mycket annat.
            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Error!");
            //    var myGreeting = greeter.GetMessageOfTheDay();
            //    await context.Response.WriteAsync($"{myGreeting}: {env.EnvironmentName}");
            //});
        }

        private void configureRoutes(IRouteBuilder routeBuilder)
        {
            // /Home/Index/4  -- Mvc automatiskt vet att den ska till HomeController.. dvs den lägger till Controller i namnet.
            // /Controller/Action/Id
            // ? innebär optional
            // tilldelining innebär vad som ska vara default ifall inget routing ska anges.
            // {Controller} blir namnet på controllern, och {action} blir metod namneen i controllern.
            // Det här sättet är den konventionella sättet att ange routing(dirigering).
            // finns ett annat sätt som heter "attribute routing"
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
