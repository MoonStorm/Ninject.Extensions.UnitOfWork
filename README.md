Extension that enables the usage of unit-of-work patterns in non-web applications. 
You could look at this as being the equivalent of InRequestScope from web environments in console applications and services.
The unit-of-work scopes are kept consistent across async calls and tasks.

#### Setup:
    _kernel.Bind<IService>().To<Service>().InUnitOfWorkScope();

#### Usage:
    using(UnitOfWorkScope.Create()){
        // resolves, async/await, manual TPL ops, etc    
    }

**[Download from NuGet and enjoy!](https://www.nuget.org/packages/Ninject.Extensions.UnitOfWork/)**

