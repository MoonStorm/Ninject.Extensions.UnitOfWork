namespace Ninject.Extensions.UnitOfWork.Tests.UnitOfWork
{
    using System;
    using System.Threading;

    public class TestService : ITestService, IDisposable
    {
        private static int _currentInstanceId = -1;

        public TestService()
        {
            this.CurrentInstanceId = Interlocked.Increment(ref _currentInstanceId);
        }

        public int CurrentInstanceId { get; private set; }
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            this.IsDisposed = true;
        }
    }
}
