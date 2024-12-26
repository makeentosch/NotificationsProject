using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SpectreConsole;

namespace Core.Connections.Logging;

public static class LoggingExtension
{
    public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder, IWebHostEnvironment env)
    {
        builder.Host.UseSerilog((context, _, loggerConfiguration) =>
        {
            var logTemplate = "{Timestamp:HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}";
            
            loggerConfiguration
                .MinimumLevel.Information()
                .WriteTo.SpectreConsole(logTemplate)
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elastic:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"logs-{env.ApplicationName.ToLower().Replace('.', '-')}-{DateTime.UtcNow:yyyy-MM}",
                    MinimumLogEventLevel = LogEventLevel.Information
                })
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(context.Configuration);

                var root = env.ContentRootPath;
                Directory.CreateDirectory(Path.Combine(root, "logs"));

                loggerConfiguration.WriteTo.File("logs/logs.txt", rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8, outputTemplate: logTemplate);
        });

        return builder;
    }
}