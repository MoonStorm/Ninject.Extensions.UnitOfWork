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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="syntax"></param>
        /// <returns></returns>
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
