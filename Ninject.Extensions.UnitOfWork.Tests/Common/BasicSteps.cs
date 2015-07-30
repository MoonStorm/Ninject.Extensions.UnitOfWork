namespace Ninject.Extensions.UnitOfWork.Tests.Common
{
    using System.Linq;
    using BoDi;
    using TechTalk.SpecFlow;

    [Binding]
    public class BasicSteps
    {
        private readonly ExceptionRecorderTestContext _testContext;

        public BasicSteps(IObjectContainer specflowContainer, ExceptionRecorderTestContext testContext)
        {
            _testContext = testContext;
        }

        [BeforeScenario]
        public void ScenarioStart()
        {
        }

        [AfterScenario]
        public void ScenarioEnd()
        {
            // if exceptions were stored but not consumed, now it's a good time to fail the test
            var firstEncounteredException = _testContext.RecordedExceptions.FirstOrDefault();
            if (firstEncounteredException != null)
            {
                throw firstEncounteredException;
            }
        }

    }
}
