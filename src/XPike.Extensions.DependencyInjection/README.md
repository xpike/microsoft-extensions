# XPike.Extensions.DependencyInjection

## Introduction

This library provides extensions/enhancements to
[Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/2.1.0).
It is independent of the xPike paved-road SDKs and takes no dependencies on the rest of xPike.

## Features

### Verifying the Container

A common mistake we make is forgetting to register a type with the container. Another is having a singleton depend on a
transient (or other lesser scoped) registration. xPike Extensions solves this by providing the cabability to verify
your `IServiceProvider` instance at startup.

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddMvc();
	...
    services.AddServiceProviderVerification();
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.ApplicationServices.Verify();
    ...
    app.UseMvc();
}
```

This ensures that:

* All root objects and their dependencies can be resolved.
* Singletons only depend on other singletons.
* Scoped registrations only depend on singletons and other scoped registrations.

This can signifcantly reduce production issues caused by missing or improper registrations not identified during
testing.

There is a performance hit at startup. The impact depends on the number of registrations. 
You may want to only verify the container in non-production environments. Assuming you have your
`ASPNETCORE_ENVIRONMENT` set properly for each environment, you can simply...

```cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    if (!env.IsProduction())
        app.ApplicationServices.Verify();

    ...
    app.UseMvc();
}
```
