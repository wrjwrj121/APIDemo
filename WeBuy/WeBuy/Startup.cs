using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WeBuy.Common;

namespace WeBuy
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EFCoreContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("SQLStr")));

            //����
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,

                    builder => builder.AllowAnyOrigin()

                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")

                    );

            });

            //// ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VeBuy API",
                    Description = "A simple example ASP.NET Core Web API",
                });
                //������ʾ
                c.SwaggerDoc("Base", new OpenApiInfo { Title = "����ģ��", Version = "Base_V1" });
                c.SwaggerDoc("User", new OpenApiInfo { Title = "�û�ģ��", Version = "User_V1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers(opt => 
            {
                opt.Filters.Add(typeof(GlobalException));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeBuy V1");
                c.SwaggerEndpoint("/swagger/User/swagger.json", "�û�ģ��");  //������ʾ
                c.SwaggerEndpoint("/swagger/Base/swagger.json", "����ģ��");  //������ʾ
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new ConfigureAutofac());
        }
    }
}
