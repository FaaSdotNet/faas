using AutoMapper;
using FaaS.Entities.Configuration;
using FaaS.Entities.Repositories;
using FaaS.MVC.Configuration;
using FaaS.Services;
using FaaS.Services.Configuration;
using FaaS.Services.RandomId;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace FaaS.MVC
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            env.ConfigureNLog("nlog.config");

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure model mappings
            var mapper = new MapperConfiguration(cfg =>
            {
                ViewModelsMapperConfiguration.InitializeMappings(cfg);
                ServicesMapperConfiguration.InitializeMappings(cfg);
            }).CreateMapper();

            // Do not allow application to start with broken configuration. Fail fast.
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            services.AddSingleton(mapper);

            // Add framework services.
            services.AddMvc().AddJsonOptions(o =>
            {
                var serializerSettings = o.SerializerSettings;
                // Pretty-print
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                serializerSettings.Formatting = Formatting.Indented;
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                serializerSettings.Converters.Add(new StringEnumConverter());

                JsonConvert.DefaultSettings = () => serializerSettings;
            });

            // Singleton - There will be at most one instance of the registered service type and the container will hold on to that instance until the container is disposed or goes out of scope. Clients will always receive that same instance from the container.
            services
                .AddSingleton<IRandomIdService, RandomIdService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            // Scoped - For every request within an implicitly or explicitly defined scope.
            services
                .Configure<ConnectionOptions>(options => options.ConnectionString = Configuration.GetConnectionString("FaaSConnection"))
                .AddScoped<IFaaSService, FaaSService>();

            // Transient - A new instance of the service type will be created each time the service is requested from the container.
            // If multiple consumers depend on the service within the same graph, each consumer will get its own new instance of the given service.
            services
                .AddTransient<IProjectRepository, ProjectRepository>()
                .AddTransient<IFormRepository, FormRepository>()
                .AddTransient<IElementValueRepository, ElementValueRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<ISessionRepository, SessionRepository>()
                .AddTransient<IElementRepository, ElementRepository>();

            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();

            app.UseStatusCodePages();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "home",
                    template: "Home",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "id",
                    template: "{controller=Forms}/{action=Index}/{id}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{codename?}");
            });
        }
    }
}
