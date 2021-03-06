﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace CoreDMS
{
    class Program
    {
        public static void Main(string[] args)
        {
            string executionDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
#if DEBUG
            Console.WriteLine("Starting server");
            Console.WriteLine($"executionDir: {executionDir}");
#endif
            var builder = new ConfigurationBuilder()
#if (DEBUG == false)
                .SetBasePath(executionDir)
#endif
                .AddEnvironmentVariables("COREDMS_");

            var config = builder.Build();

            BuildWebHost(args, config).Run();

        }

        public static IWebHost BuildWebHost(string[] args, IConfigurationRoot config)
        {
            var url = config.GetValue<string>(ConfigKeys.AppUrl);
            if (string.IsNullOrEmpty(url))
            {
                url = "http://localhost:5000";
            }
            var host = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .MinimumLevel.Debug()
                    .WriteTo.Console())
                .UseUrls(url)                
                .Build();
            return host;
        }
    }
}
