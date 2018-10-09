﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OdeToFood
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGreeter, Greeter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IGreeter greeter, ILogger<Startup> logger)
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


            // en middleware för use developerExceptionPage
            if (env.IsDevelopment())
            {
                // en middleware används som lyssnare för att ge mer detalierad info tillbaka till användaren, vilkeet används för debugging om vad som gick fel när ett expetion sker.
                app.UseDeveloperExceptionPage();
            }

            // registerar simpel middleware 
            // app.Run anväds mycket sällan, kanske för enklare saker som middleware som tex skriver direkt till response eller visa upp.text.. det går göra lite json och html bearbeting här men inte för mycket annat.
            app.Run(async (context) =>
            {
                throw new Exception("Error!");
                var myGreeting = greeter.GetMessageOfTheDay();
                await context.Response.WriteAsync(myGreeting);
            });
        }
    }
}
