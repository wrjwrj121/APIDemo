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
            //��ȡ�����ļ���Ϣ����Ϊ�ǹ��õģ����Ծ�ͳһ�������ļ���
            string secret = Configuration.GetValue<string>("JWT:Secrete");//��Կ���������Կ���Ȳ�����16
            string issuer_z = Configuration.GetValue<string>("JWT:Issuer");//�䷢��
            string audience_z = Configuration.GetValue<string>("JWT:Audience");//���շ�

            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //�Ƿ���֤��Կ
                    ValidateIssuerSigningKey = true,
                    //��Կ����סһ��Ҫ���ڵ���16λ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                    //�Ƿ���֤�䷢��
                    ValidateIssuer = true,
                    //�䷢��
                    ValidIssuer = issuer_z,

                    //�Ƿ���֤���շ�
                    ValidateAudience = true,
                    ValidAudience = audience_z,

                    //����Ҫ�г�ʱʱ��
                    RequireExpirationTime = true,
                    //�Ƿ���֤��ʱ��������exp��nbfʱ��Ч��ͬʱ����ClockSkew
                    ValidateLifetime = true,

                    //����һ������ʱ�䣬ϵͳĬ����5���ӣ������ã���Token��Чʱ����ǹ���ʱ��+������õĻ���ʱ��
                    ClockSkew = TimeSpan.FromSeconds(50)

                };
            });
            #endregion

            //����
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


            //// ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("Authorize", new OpenApiInfo
                {
                    Version = "Auth",
                    Title = "VeBuy API",
                    Description = "A simple example ASP.NET Core Web API",
                });
                //������ʾ
                c.SwaggerDoc("Base", new OpenApiInfo { Title = "����ģ��", Version = "Base_V1" });
                c.SwaggerDoc("User", new OpenApiInfo { Title = "�û�ģ��", Version = "User_V1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT��Ȩֱ�����¿�������Bearer {token}(ע������֮����һ���ո�)",
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
                c.SwaggerEndpoint("/swagger/Authorize/swagger.json", "��Ȩ");
                c.SwaggerEndpoint("/swagger/User/swagger.json", "�û�ģ��");  //������ʾ
                c.SwaggerEndpoint("/swagger/Base/swagger.json", "����ģ��");  //������ʾ
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
