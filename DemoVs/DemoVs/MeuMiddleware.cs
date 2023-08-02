using Serilog;
using System.Diagnostics;

namespace DemoVs {
    public class TemplateMiddleware {
        private readonly RequestDelegate _next;

        public TemplateMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            
            
            await _next(httpContext);
        }
    }

    public class LogTempoMiddleware {
        private readonly RequestDelegate _next;

        public LogTempoMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            // faz algo antes
            var sw = Stopwatch.StartNew();


            // Chama o proximo:
            await _next(httpContext);

            sw.Stop();

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information($"A execução demorou {sw.Elapsed.TotalMilliseconds}ms ({sw.Elapsed.TotalSeconds} segundos)");
        }
    }

    public static class SerilogExtensions {
        public static void AddSerilog(this WebApplicationBuilder builder) {
            builder.Host.UseSerilog();
        }
    }

    public static class LogTempoMiddlewareExtensions {
        public static void UseLogTempo(this WebApplication app) {
            app.UseMiddleware<LogTempoMiddleware>();
        }
    }
}
