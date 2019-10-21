# XPike.Extensions.DependencyInjection

[![Build Status](https://dev.azure.com/xpike/xpike/_apis/build/status/xpike-extensions?branchName=master)](https://dev.azure.com/xpike/xpike/_build/latest?definitionId=1&branchName=master)
![Nuget](https://img.shields.io/nuget/v/XPike.Extensions.DependencyInjection)

## Introduction

This library provides extensions/enhancements to
[Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/2.1.0).
It is independent of the xPike paved-road SDKs and takes no dependencies on the rest of xPike.

## Usage

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

### Using Microsoft.Extensions.DependencyInjection in Legacy WebAPI



## Building and Testing

Building from source and running unit tests requires a Windows machine with:

* .Net Core 3.0 SDK
* .Net Framework 4.6.1 Developer Pack

## Issues

Issues are tracked on [GitHub](https://github.com/xpike/microsoft-extensions/issues). Anyone is welcome to file a bug,
an enhancement request, or ask a general question. We ask that bug reports include:

1. A detailed description of the problem
2. Steps to reproduce
3. Expected results
4. Actual results
5. Version of the package xPike
6. Version of the .Net runtime

## Contributing

See our [contributing guidelines](https://github.com/xpike/documentation/blob/master/docfx_project/articles/contributing.md)
in our documentation for information on how to contribute to xPike.

## License

xPike is licensed under the [MIT License](LICENSE).
