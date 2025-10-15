using CRMApp.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CRMApp.Middleware
{
    public interface IAppLogger
    {
        void LogToFile(Exception ex);
        void LogToFile(Type type, string message);

    }
    public class AppExceptionHandlerAndLogger(IConfiguration configuration) : IExceptionHandler, IAppLogger
    {
        private readonly string filePath = configuration.GetSection("Logging:LogFilePath")["FilePath"];
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {

            LogToFile(exception);
            var response = new ErrorResponse()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Title = exception.Message,
                ExceptionMessage = exception.Message
            };

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            return true;
        }

        public void LogToFile(Exception ex)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("------------------Exception-----------------------");
                writer.WriteLine($"Time Stamp: {DateTime.Now.ToString("hh:mm:ss")}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine("---------------------------------------------------");
                writer.WriteLine();
            }
        }

        public void LogToFile(Type type, string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"------------------{type.ToString()}-----------------------");
                writer.WriteLine($"Time Stamp: {DateTime.Now.ToString("hh:mm:ss")}");
                writer.WriteLine($"Message: {message}");
                writer.WriteLine("---------------------------------------------------");
                writer.WriteLine();
            }
        }
    }

    public enum Type
    {
        Information,
        Warning
    }

}
