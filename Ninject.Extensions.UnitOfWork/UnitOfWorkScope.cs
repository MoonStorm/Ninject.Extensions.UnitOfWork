namespace Ninject.Extensions.UnitOfWork
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using Ninject.Infrastructure.Disposal;

    /// <summary>
    /// Unit-of-work scope
    /// </summary>
    [Serializable]
    public sealed class UnitOfWorkScope : MarshalByRefObject, IDisposable, INotifyWhenDisposed
    {
        private const string CallContextCurrentContextKey = "NinjectIocUnitOfWork";
        private bool _isDisposed;
        private readonly UnitOfWorkScope _priorContext;
        public event EventHandler Disposed;

        private UnitOfWorkScope()
        {
            this.Disposed += (sender, ev) => { };
            _priorContext = Current;
            Current = this;
        }

        /// <summary>
        /// Returns true if the context was disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
        }

        /// <summary>
        /// Gets or sets the current unit of work context.
        /// </summary>
        internal static UnitOfWorkScope Current
        {
            get
            {
                return CallContext.LogicalGetData(CallContextCurrentContextKey) as UnitOfWorkScope;
            }
            private set
            {
                if (ReferenceEquals(value, null))
                {
                    CallContext.FreeNamedDataSlot(CallContextCurrentContextKey);
                }
                else
                {
                    CallContext.LogicalSetData(CallContextCurrentContextKey, value);
                }
            }
        }

        /// <summary>
        /// Creates a new unit-of-work scope. Remember to call <see cref="Dispose"/> when you're done with it.
        /// </summary>
        public static UnitOfWorkScope Create()
        {
            return new UnitOfWorkScope();
        }

        /// <summary>
        /// Disposes all the components resolved in the current scope.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Current = _priorContext;
                this.Disposed(this, new EventArgs());
            }
        }
    }
}
