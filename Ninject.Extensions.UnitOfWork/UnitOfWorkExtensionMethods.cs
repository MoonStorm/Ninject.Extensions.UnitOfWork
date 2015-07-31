namespace Ninject.Extensions.UnitOfWork
{
    using System;
    using Ninject.Syntax;
    
    /// <summary>
    /// Extension methods for unit-of-work patterns.
    /// </summary>
    public static class UnitOfWorkExtensionMethods
    {
        /// <summary>
        /// Sets up the behavior of a binding for one instance per unit of work. 
        /// A unit of work must be available at the time the binding is used for resolve. 
        /// Use <see cref="UnitOfWorkScope.Create"/> to create a scope of type unit of work and remember to dispose it when you're done with it.
        /// </summary>
        public static IBindingNamedWithOrOnSyntax<T> InUnitOfWorkScope<T>(this IBindingInSyntax<T> syntax)
        {
            return syntax.InScope(
                context =>
                    {
                        var currentUnitOfWork = UnitOfWorkScope.Current;
                        if (currentUnitOfWork == null)
                        {
                            throw new InvalidOperationException($"No unit-of-work context present for the IOC resolve of component {context?.Request?.Service}");
                        }
                        return currentUnitOfWork;
                    });
        }
    }
}
