using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IdentityServer.Contexts;
using WebApi.IdentityServer.Models;

namespace WebApi.IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddMvc(options => {
            //    options.ReturnHttpNotAcceptable = true; // 在一个请求中，asp.net core 默认返回的内容是json格式， 如果 请求的是xml 格式，不支持。解决方法  把 ReturnHttpNotAcceptable 属性设为true, 如果不支持 ，就会返回406 Not Acceptable.
            //    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()); // 添加 xml 输出格式的支持。
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityServerConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
             {
                 options.Events.RaiseErrorEvents = true;
                 options.Events.RaiseFailureEvents = true;
                 options.Events.RaiseInformationEvents = true;
                 options.Events.RaiseSuccessEvents = true;
             })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();  // 这里需要添加单独的包：IdentityServer4.AspNetIdentity
             //.AddTestUsers(Config.GetTestUsers()); // 在内存中添加测试用户
         
            if (Environment.IsDevelopment())
                builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStatusCodePages();// 启用状态码中间件

            // 自定义输出状态码
            //app.UseStatusCodePages(async context =>
            //{
            //    context.HttpContext.Response.ContentType = "text/plain";
            //    await context.HttpContext.Response.WriteAsync($"the status code is {context.HttpContext.Response.StatusCode}");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseMvc();
        }
    }
}
