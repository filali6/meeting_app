using System.Text.Json;
using Backend.Exceptions;

namespace Backend.middlewares;

public class ApiExceptionMiddleware(RequestDelegate next,ILogger<ApiExceptionMiddleware> logger,IHostEnvironment env)
{
    public IHostEnvironment env=env;
    public required ILogger Logger=logger;
    public RequestDelegate _next=next;
    public async Task InvokeAsync(HttpContext context){
        try{
            await  _next(context);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex,ex.Message);
            context.Response.ContentType="application/json";
            context.Response.StatusCode=(int)StatusCodes.Status500InternalServerError;
            var response=env.IsDevelopment()
                ? new ApiExceptions(context.Response.StatusCode,ex.Message,ex.StackTrace)
                :new ApiExceptions(context.Response.StatusCode,ex.Message,"Server Error");
                var  options = new JsonSerializerOptions{
                    PropertyNamingPolicy=JsonNamingPolicy.CamelCase
                };
            var json =JsonSerializer.Serialize(response,options);
            Logger.LogInformation("res json");
             Logger.LogInformation(json);
            await context.Response.WriteAsJsonAsync(response);
        }
    }

}
