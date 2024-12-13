using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Backend.middlewares;

public class DelayTesting(ILogger<DelayTesting> logger,RequestDelegate next,IHostEnvironment env) 
{
    public ILogger<DelayTesting> _logger=logger;
    public IHostEnvironment env=env;
    public RequestDelegate next=next;
    public async Task InvokeAsync(HttpContext context)
    {  if(env.IsDevelopment()){
         _logger.LogWarning("start sleeping");
        Thread.Sleep(500);
        // var l=context;
        // context.Request.EnableBuffering();
        // var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8).ReadToEndAsync();
        // context.Request.Body.Seek(0, SeekOrigin.Begin);
        // var jsonObject = JsonSerializer.Deserialize<object>(requestBody);
        // var r=JsonSerializer.Serialize(jsonObject);
        // _logger.LogWarning($"Request Body: {r}");
       // _logger.LogWarning($"Request Body As String: {requestBody.ReadAsStringAsync()}");
        _logger.LogWarning("end sleeping");
    }
        await next(context);
    }
}
