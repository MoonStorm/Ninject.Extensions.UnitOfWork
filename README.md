Extension for unit-of-work pattern for non-web applications.

#### Setup:
    _kernel.Bind<IService>().To<Service>().InUnitOfWorkScope();

#### Usage:
    using(UnitOfWorkScope.Create()){
        // resolves, async/await, manual TPL ops, etc    
    }

