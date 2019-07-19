using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CaliSpeed.SignalRHubs;
using Microsoft.AspNetCore.SignalR;

namespace CaliSpeed
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.UseWebSockets();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR
            (
                routes =>
                {
                    routes.MapHub<ExampleHub>("/ExampleHub");
                    routes.MapHub<GameHub>("/GameHub");
                }
            );

            // Get a hub context to use later to send from outside of a hub
            app.Use(async (context, next) =>
            {
                var hubContext = context.RequestServices.GetRequiredService<IHubContext<GameHub>>();
                Rooms.RoomFactory.setup(hubContext);

                await next.Invoke();

            });

            app.UseMvc();

            //app.Run(async (context) =>
            //{
            //  await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
