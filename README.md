Extension for unit-of-work pattern for non-web applications.
The scopes are kept consistent across async calls and tasks.

#### Setup:
    _kernel.Bind<IService>().To<Service>().InUnitOfWorkScope();

#### Usage:
    using(UnitOfWorkScope.Create()){
        // resolves, async/await, manual TPL ops, etc    
    }

