## Hisar
Cross-Platform Modular Component Development Infrastructure

>[Latest release on Nuget](https://www.nuget.org/packages/NetCoreStack.Hisar/)


## Why Hisar?
The purpose of this framework is that eliminate the **monolithic application challenges** and allow us to develop component (module) without main application dependency. Thus the component can be run and test as a standalone web application. When you complete development requirements and ready to deploy the component then just pack it or copy the output to the related directory of the main application.

> **MsBuild** , **NuGet** or **[dotnet pack](https://docs.microsoft.com/en-us/dotnet/articles/core/tools/dotnet-pack)** tools can be used for packaging. In this way, the component will be able to part of the main hosting application and available to upload **NuGet** or **MyGet** package repositories. 

Despite the other framework you don't need to reference any component (module) to main application but if you want the component to act as part of the main application you can copy the output .dll to ExternalComponents folder of main application so the Hisar platform engine resolves all dependencies, controllers, views, contents and then registers the component with convention name.

For example; you got a component which is name **Hisar.Component.Carousel**. For convention, Hisar engine resolves that namespace as a **Carousel** component when to act as a part of the main application. For this component, controllers will work as an **Area** with http://\<domainname\>/guideline URL prefix in main application.

**Startup.cs** is the bootstrap - starter class for the ASP.NET Core web application. We tried to preserve as much as possible in the same way that file except 

    public static void ConfigureRoutes(IRouteBuilder routes)

for the method mentioned above and

    .UseStartup<DefaultHisarStartup<Startup>>()

our custom startup wrapper to handle component and application acts. For more details, you can check the test projects source code in this repository.

## Tools
[Hisar Web Cli](https://github.com/NetCoreStack/Tools) tool provides manage extensibility and templating of components. You don't need extra gulp or grunt tooling and scripting behaviors. .NET Core CLI tools extensibility model has various tooling features. **Hisar Web Cli** is built on top of it.

## Contributing to Repository
 - Fork and clone locally.
 - Build the solution with Visual Studio 2017.
 - Create a topic specific branch in git.
 - Send a Pull Request.

## Prerequisites
> [ASP.NET Core](https://github.com/aspnet/Home)

## License
> The [MIT licence](https://github.com/NetCoreStack/Hisar/blob/master/LICENSE.txt)