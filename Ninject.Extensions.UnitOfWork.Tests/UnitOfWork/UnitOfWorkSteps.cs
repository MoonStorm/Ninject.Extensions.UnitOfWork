namespace Ninject.Extensions.UnitOfWork.Tests.UnitOfWork
{
    using System.Linq;
    using System.Threading.Tasks;
    using Ninject;
    using Ninject.Extensions.UnitOfWork;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public sealed class UnitOfWorkSteps
    {
        private ApplicationIocTestContext _testContext;

        public UnitOfWorkSteps(ApplicationIocTestContext testContext)
        {
            _testContext = testContext;
        }

        [Given(@"I have set up the application IOC container")]
        public void GivenIHaveSetUpTheApplicationIOCContainer()
        {
            _testContext.ComponentContainer = new StandardKernel();
        }

        [When(@"I register the sample IOC service with an application unit-of-work binding")]
        public void WhenIRegisterTheSampleIOCServiceWithAnApplicationUnit_Of_WorkBinding()
        {
            _testContext.ComponentContainer.Bind<ITestService>().To<TestService>().InUnitOfWorkScope();
        }

        [When(@"I start a new application unit of work")]
        public void WhenIStartANewApplicationUnitOfWork()
        {
            var newUnitOfWork = UnitOfWorkScope.Create();
            _testContext.UnitOfWorkContexts.Push(newUnitOfWork);
        }

        [When(@"I resolve the sample service through the application IOC container")]
        public void WhenIResolveTheSampleServiceThroughTheApplicationIOCContainer()
        {
            var resolvedComponent = _testContext.ComponentContainer.Get<ITestService>();
            _testContext.Resolves.Add(resolvedComponent);
        }

        [When(@"I resolve the sample service in a new task through the application IOC container")]
        public void WhenIResolveTheSampleServiceInANewTaskThroughTheApplicationIOCContainer()
        {
            Task.Delay(300)
                .ContinueWith(previousTask => this.WhenIResolveTheSampleServiceThroughTheApplicationIOCContainer())
                .GetAwaiter()
                .GetResult();
        }

        [When(@"I resolve the sample service async through the application IOC container")]
        public void WhenIResolveTheSampleServiceAsyncThroughTheApplicationIOCContainer()
        {
            this.ResolveSampleServiceAsync().GetAwaiter().GetResult();
        }

        [Then(@"the sample services (.*) resolved through the application IOC container should be the same")]
        public void ThenSampleServicesResolvedThroughTheApplicationIOCContainerShouldBeTheSame(int[] indexes)
        {
            var resolves = indexes.Select(index => _testContext.Resolves[index]);
            Assert.That(resolves, Is.All.SameAs(resolves.First()));
        }

        [Then(@"the sample services (.*) resolved through the application IOC container should not be the same")]
        public void ThenSampleServicesResolvedThroughTheApplicationIOCContainerShouldNotBeTheSame(int[] indexes)
        {
            var resolves = indexes.Select(index => _testContext.Resolves[index]);
            Assert.That(resolves, Is.Unique);
        }

        [Then(@"the sample services (.*) resolved through the application IOC container should be disposed")]
        public void ThenTheSampleServicesResolvedThroughTheApplicationIOCContainerShouldBeDisposed(int[] indexes)
        {
            var resolves = indexes.Select(index => ((TestService)_testContext.Resolves[index]).IsDisposed);
            Assert.That(resolves, Is.All.EqualTo(true));
        }

        [Then(@"the sample services (.*) resolved through the application IOC container should not be disposed")]
        public void ThenTheSampleServicesResolvedThroughTheApplicationIOCContainerShouldNotBeDisposed(int[] indexes)
        {
            var resolves = indexes.Select(index => ((TestService)_testContext.Resolves[index]).IsDisposed);
            Assert.That(resolves, Is.All.EqualTo(false));
        }

        [When(@"I end the current application unit of work")]
        public void WhenIEndTheCurrentApplicationUnitOfWork()
        {
            _testContext.UnitOfWorkContexts.Pop().Dispose();
        }

        [StepArgumentTransformation()]
        public int[] InStringToIntsTransform(string intArrayRepresentation)
        {
            return intArrayRepresentation.Split(',').Select(textInt => int.Parse(textInt.Trim())).ToArray();
        }

        private async Task ResolveSampleServiceAsync()
        {
            await Task.Delay(300);
            this.WhenIResolveTheSampleServiceThroughTheApplicationIOCContainer();
        }
    }
}
