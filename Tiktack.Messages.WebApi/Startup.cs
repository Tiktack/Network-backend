using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Tiktack.Common.DataAccessLayer.Repositories;
using Tiktack.Messaging.BusinessLayer.Providers;
using Tiktack.Messaging.DataAccessLayer.Entities;
using Tiktack.Messaging.DataAccessLayer.Infrastructure;
using Tiktack.Messaging.WebApi.Helpers;
using Tiktack.Messaging.WebApi.Hubs;

namespace Tiktack.Messaging.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:3000", "http://10.26.7.68:3000", "http://10.26.7.68:4005", "http://localhost:4005")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            #endregion

            #region Authentication
            var token = "";
            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];
                options.SaveToken = true;
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        token = accessToken;
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.StartsWithSegments("/messaging"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            #endregion


            services.AddScoped(x => new RequestProvider(token));


            services.AddHealthChecks();


            services.AddDbContext<DbContext, MessagingDBContext>(options => options.UseSqlServer(Configuration["ConnectionString"]));


            //services.AddAuthorization(options => options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain))));

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            services.AddTransient<UnitOfWork>();
            services.AddTransient<IRepository<UserInfoDBLayer>, Repository<UserInfoDBLayer>>();
            services.AddTransient<IRepository<Message>, Repository<Message>>();
            services.AddTransient<IUserProvider, UserProvider>();
            services.AddTransient<MessageProvider>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DbContext>();
                context.Database.EnsureCreated();
            }
            //app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessagingHub>("/messaging");
                endpoints.MapHealthChecks("/health");
            });
        }

    }
}
