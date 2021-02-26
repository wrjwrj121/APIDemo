using AspNetCoreRateLimit;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WeBuy.Common;
using WeBuy.Common.AutoProfile;
using WeBuy.Controllers.Base;

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
            #region ip
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddDbContext<EFCoreContext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("SQLStr")));

           
            #region jwt
            //jwt
            //获取配置文件信息，因为是共用的，所以就统一放配置文件了
            string secret = Configuration.GetValue<string>("JWT:Secrete");//秘钥，这里的秘钥长度不低于16
            string issuer_z = Configuration.GetValue<string>("JWT:Issuer");//颁发方
            string audience_z = Configuration.GetValue<string>("JWT:Audience");//接收方

            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否验证秘钥
                    ValidateIssuerSigningKey = true,
                    //秘钥，记住一定要大于等于16位
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                    //是否验证颁发者
                    ValidateIssuer = true,
                    //颁发者
                    ValidIssuer = issuer_z,

                    //是否验证接收方
                    ValidateAudience = true,
                    ValidAudience = audience_z,

                    //必须要有超时时间
                    RequireExpirationTime = true,
                    //是否验证超时，当设置exp和nbf时有效，同时启用ClockSkew
                    ValidateLifetime = true,

                    //这是一个缓冲时间，系统默认是5分钟，可设置；则Token有效时间就是过期时间+这个设置的缓冲时间
                    ClockSkew = TimeSpan.FromSeconds(50)

                };
            });
            #endregion

            //跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,

                    builder => builder.AllowAnyOrigin()

                    .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS").AllowAnyOrigin()

                    );

            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(MyAllowSpecificOrigins,

            //        builder => builder.WithOrigins("http://localhost:8081")
            //       .AllowAnyHeader()
            //       .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
            //       .AllowCredentials());

            //});


            //// 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Authorize", new OpenApiInfo
                {
                    Version = "Auth",
                    Title = "VeBuy API",
                    Description = "A simple example ASP.NET Core Web API",
                });
                //分组显示
                c.SwaggerDoc("Base", new OpenApiInfo { Title = "基础模块", Version = "Base_V1" });
                c.SwaggerDoc("User", new OpenApiInfo { Title = "用户模块", Version = "User_V1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权直接在下框中输入Bearer {token}(注意两者之间是一个空格)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
                       {
                           new OpenApiSecurityScheme
                           {
                                Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                               }
                            }, new List<string>()
                         }
                     });
            });

            services.AddSignalR();

            services.AddControllers(opt => 
            {
                opt.Filters.Add<ActionFilter>();

                opt.Filters.Add(typeof(GlobalException));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/Authorize/swagger.json", "鉴权");
                c.SwaggerEndpoint("/swagger/User/swagger.json", "用户模块");  //分组显示
                c.SwaggerEndpoint("/swagger/Base/swagger.json", "基础模块");  //分组显示
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);

            //app.UseCors("SignalR");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new ConfigureAutofac());
        }
    }
}
