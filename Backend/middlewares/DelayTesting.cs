using System;
using System.Security.Cryptography.X509Certificates;

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
        _logger.LogWarning("end sleeping");
    }
        await next(context);
    }
}
