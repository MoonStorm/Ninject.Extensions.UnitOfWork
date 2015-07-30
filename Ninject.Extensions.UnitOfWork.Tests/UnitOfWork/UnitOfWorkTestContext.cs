namespace Ninject.Extensions.UnitOfWork.Tests.UnitOfWork
{
    using System;
    using System.Collections.Generic;
    using Ninject;

    public sealed class ApplicationIocTestContext
    {
        public ApplicationIocTestContext()
        {
            this.Resolves = new List<object>();
            this.UnitOfWorkContexts = new Stack<IDisposable>();
        }

        public IKernel ComponentContainer { get; set; }

        public List<object> Resolves { get; private set; }

        public Stack<IDisposable> UnitOfWorkContexts { get; set; }
    }
}
