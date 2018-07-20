using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreDMS
{
    public class ReflectionTypeLoadExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        Logger logger;
        public ReflectionTypeLoadExceptionLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            //_logger = loggerFactory.CreateLogger<ReflectionTypeLoadExceptionLoggingMiddleware>();
            var logDir = configuration.GetValue<string>(ConfigKeys.LogDir);
            logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logDir, "consoleapp.log"))
                .CreateLogger();
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var reflectionTypeLoadException = ex as ReflectionTypeLoadException;
                if (reflectionTypeLoadException != null && reflectionTypeLoadException.LoaderExceptions != null)
                {
                    logger.Error("ReflectionTypeLoadException {0}", ex);
                    logger.Error("Loader exceptions messages: ");
                    foreach (var exception in reflectionTypeLoadException.LoaderExceptions)
                    {
                        logger.Error("ex {0}", exception);
                    }
                }
                throw;
            }
        }
    }
}
