using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Web.Controllers;

namespace Web
{
    public class Startup
    {
        static List<Assembly> assList = new List<Assembly>();

        /// <summary>
        /// 启动网站 启动成功后可以通过http://IP:port/swagger 访问api
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="controllerList">controller所在的assembly</param>
        public static void StartWeb(int port, List<Assembly> controllerList)
        {
            assList = controllerList ?? new List<Assembly>();
            CreateWebHostBuilder(port).Build().Run();
        }
        /// <summary>
        /// 启动网站 启动成功后可以通过http://IP:port/swagger 访问api
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="controller">controller所在的assembly</param>
        public static void StartWeb(int port, Assembly controller)
        {
            assList = new List<Assembly>();
            if (controller != null)
            {
                assList.Add(controller);
            }

            StartWeb(port, assList);
        }

        /// <summary>
        /// 
        /// </summary>
        public static IServiceCollection Services { get; set; }

        static IWebHostBuilder CreateWebHostBuilder(int port)
        {
            string contentPath = Path.Combine(Directory.GetCurrentDirectory());
            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            var host = new WebHostBuilder()
                   .UseKestrel()
                   .UseUrls("http://*:" + port)
                   .UseContentRoot(contentPath)//表示应用程序所在的默认文件夹地址，如 MVC 中视图的查询根目录
                                               //.UseWebRoot(contentPath) 用来指定可让外部可访问的静态资源路径，默认为wwwroot，并且是以contentRoot为根目录
                   .UseStartup<Startup>();
            return host;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = "XMWEB";
                o.DefaultChallengeScheme = "XMWEB";
                o.DefaultSignInScheme = "XMWEB";
            }).AddCookie("XMWEB", o =>
            {
                o.LoginPath = "/account/login";
            });

            //注入 TestManager,controler中才能使用
            //services.AddTransient<TestManager>();

            var moduleBuild = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //直接启动不能载入controll,需要手动加入
            assList.ForEach((ass) => { moduleBuild.AddApplicationPart(ass); });

            services.AddCors();
            //string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SwaggerXml");
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WinHost.xml");

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Info { Title = "web api", Version = "v1" });

                option.DescribeAllEnumsAsStrings();

                // 为 Swagger JSON and UI设置xml文档注释路径
                option.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
            });

            app.UseCors(cors => cors.AllowAnyOrigin());

            app.UseDefaultFiles();//支持默认首页

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
