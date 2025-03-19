using CarRental.Models;
using System.Text.Json;

namespace CarRental.MiddleWare
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed with request pipeline
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred.");
                var serviceResponse = new ServiceResponse<string>();

                string log = string.Empty;

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                
                var endpoint = context.GetEndpoint().ToString().Replace('.', '-');

                serviceResponse.Message = "An unexpected error occurred!";
                serviceResponse.StatusCode = StatusCodes.Status500InternalServerError;


                try
                {
                    HelperServices.WriteExceptionToFile(ex, endpoint);
                }
                catch (Exception logEx)
                {
                    Console.WriteLine("Failed to log exception.");
                }

                await context.Response.WriteAsJsonAsync(serviceResponse);
            }
        }
    }
}
