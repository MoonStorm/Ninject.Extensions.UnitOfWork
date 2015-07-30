namespace Ninject.Extensions.UnitOfWork.Tests.Common
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// Records the state of a test. 
    /// This class must not be inherited otherwise the IOC inside specflow would create multiple instances of this class.
    /// </summary>
    public sealed class ExceptionRecorderTestContext
    {
        private volatile Exception[] _recordedExceptions = new Exception[0];

        public ExceptionRecorderTestContext()
        {
        }

        public Exception[] RecordedExceptions
        {
            get
            {
                return this._recordedExceptions;
            }
        }

        public void RecordException(Exception ex)
        {
            var recordedExceptions = _recordedExceptions;
            _recordedExceptions = recordedExceptions.Concat(new Exception[] { ex }).ToArray();
        }

        public void PreviousStepsShouldHaveErroredWith<TException>() where TException : Exception
        {
            var recordedExceptions = _recordedExceptions;
            var lastException = recordedExceptions.OfType<TException>().LastOrDefault();
            Assert.That(lastException, Is.Not.Null, $"Expected {typeof(TException)} in the previous steps");
            _recordedExceptions = recordedExceptions.Except(new Exception[] { lastException }).ToArray();
        }
    }
}
