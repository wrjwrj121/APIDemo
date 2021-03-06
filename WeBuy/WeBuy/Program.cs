using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace WeBuy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureLogging((hostingContext, builder) =>
            {
                //过滤掉系统默认的一些日志
                builder.AddFilter("System", LogLevel.Error);
                builder.AddFilter("Microsoft", LogLevel.Error);
                ////builder.AddFilter("Blog.Core.AuthHelper.ApiResponseHandler", LogLevel.Error);

                ////可配置文件
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config");
                builder.AddLog4Net("log4net.config", true);

            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();



                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
