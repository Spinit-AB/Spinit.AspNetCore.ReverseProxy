Spinit.AspNetCore.ReverseProxy
========================================

![Licence: MIT](https://img.shields.io/github/license/Spinit-AB/Spinit.AspNetCore.ReverseProxy.svg)
![Azure DevOps builds](https://img.shields.io/azure-devops/build/spinitforce/d7ddce33-e90d-4c48-9976-24d1676759e2/11/master.svg)
![Azure DevOps tests](https://img.shields.io/azure-devops/tests/spinitforce/d7ddce33-e90d-4c48-9976-24d1676759e2/11.svg?compact_message)
![Nuget](https://img.shields.io/nuget/v/Spinit.AspNetCore.ReverseProxy.svg)


A simple reverse proxy implementation for use in an ASP.NET Core application.

This reverse proxy implementation is inspired by how Mvc uses routing to actions. 

Install
-------

Package Manager:

```console
PM> Install-Package Spinit.AspNetCore.ReverseProxy
```

.NET CLI:
```console
> dotnet add package Spinit.AspNetCore.ReverseProxy
```

Basic Usage
-----------

### Add to services

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        ...
        .AddReverseProxy(options => 
        {
            // optional configuration
        });
}
```

### Add to HTTP pipeline

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{

    app
        ...
        .UseMvc() // recommendation is to use Mvc before the reverse proxy to allow proxy overrides in controllers
        .UseReverseProxy(c =>
        {
            c.MapRoute(
                "proxy/http/{*uri}", // the route template to use, see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing
                rd => new Uri($"http://{rd.Values["uri"]}", UriKind.Absolute) // supply a function that given the route data from the template should return the proxy uri 
            );
            c.MapRoute(
                ...
            );
        });
}
```

Advanced Usage
--------------

### Add a filter to intercept or modify proxy requests

Filters are inspired by Mvc filters and is an extension point for acting before a proxy request is sent.

```csharp
public class MyLoggingFilter : IReverseProxyFilter
{
    private readonly ILogger _logger;

    public MyLoggingFilter(ILogger<MyLoggingFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExecutingAsync(ReverseProxyExecutingContext context)
    {      
        // here you can modify the outgoing proxy request before it is sent
        _logger.LogDebug($"Calling proxy: {context.ProxyRequest.RequestUri.ToString()}");
        return Task.CompletedTask;
    }
}

public void ConfigureServices(IServiceCollection services)
{

    services
        ...
        .AddReverseProxy(options => 
        {
            options.Filters.Add<MyLoggingFilter>()
        });
}
```

### Use the reverse proxy in an mvc controller action

The `IReverseProxy` can be used in a controller if this is desired.

```csharp
[Route("api/[controller]")]
[ApiController]
public class ProxyController : ControllerBase
{
    private readonly IReverseProxy _reverseProxy;

    public ProxyController(IReverseProxy reverseProxy)
    {
        _reverseProxy = reverseProxy;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var proxyUri = new Uri("http://myapiserver:7331/api/");
        var proxyResponse = await _reverseProxy.ExecuteAsync(Request, proxyUri).ConfigureAwait(false);
        return new ResponseResult(proxyResponse); // use ResponseResult to wrap the proxyResponse in a IActionResult implementation
    }
}
```