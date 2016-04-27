using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using ProtoBuf.Meta;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Web.Logging;
using WotDossier.Web.Logic;
using WotDossier.Web.Middleware;
using WotDossier.Web.Models;
using WotDossier.Web.Services;

namespace WotDossier.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddEntityFramework()
                .AddSqlServer().AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]))
                .AddNpgsql().AddDbContext<dossierContext>(options =>
                    options.UseNpgsql(Configuration["Data:PostgresqlConnection:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc().AddJsonOptions(options =>
             {
                 options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
             });

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<SyncManager, SyncManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            File.Create("config1.nlog");
            var xmlLoggingConfiguration = new  XmlLoggingConfiguration("config.nlog");
            loggerFactory.AddNLog(new LogFactory(xmlLoggingConfiguration));
            
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
                
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>()
                             .Database.Migrate();
                    }
                }
                catch { }
            }

            app.UseIISPlatformHandler(options => options.AuthenticationDescriptions.Clear());

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseRequestMiddleware();

            // To configure external authentication please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            InitProtobuf();

            Console.Write("Hello world");
        }

        private static void InitProtobuf()
        {
            RuntimeTypeModel.Default.AllowParseableTypes = true;
            RuntimeTypeModel.Default.AutoAddMissingTypes = true;

            RuntimeTypeModel.Default.Add<ClientStat>();
            RuntimeTypeModel.Default.Add<EntityBase>()
                .AddSubType(100, RuntimeTypeModel.Default.Add<PlayerEntity>().Type)
                .AddSubType(200, RuntimeTypeModel.Default.Add<TankEntity>().Type)
                .AddSubType(300, RuntimeTypeModel.Default.Add<StatisticEntity>()
                                    .AddSubType(400, RuntimeTypeModel.Default.Add<RandomBattlesStatisticEntity>().Type)
                                 .Type)
                .AddSubType(700, RuntimeTypeModel.Default.Add<RandomBattlesAchievementsEntity>().Type)
                                    //.AddSubType(500, RuntimeTypeModel.Default.Add<TankStatisticEntityBase>()
                                    //                    .AddSubType(600, RuntimeTypeModel.Default.Add<TankRandomBattlesStatisticEntity>().Type).Type)
                                    ;
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }

    public static class RuntimeTypeModelExt
    {
        public static MetaType Add<T>(this RuntimeTypeModel model)
        {
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.IsDefined(typeof(DataMemberAttribute), false) && prop.CanWrite).Select(x => x.Name).ToArray();

            return model.Add(typeof(T), true).Add(propertyInfos);
        }
    }
}
