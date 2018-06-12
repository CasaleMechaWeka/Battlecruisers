using BattleCruisers.Buildables.Boost;
using BattleCruisers.Tutorial.Steps;
using BattleCruisers.Tutorial.Steps.EnemyCruiser;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public class DummyBoostStep : AircraftBoostStep
    {
        public int BoostActionCount { get; private set; }

        public DummyBoostStep(ITutorialStepArgs args, IBoostProvidersManager boostProvidersManager, IBoostProvider boostProvider)
            : base(args, boostProvidersManager, boostProvider)
        {
            BoostActionCount = 0;
        }

        protected override void BoostProviderAction()
        {
            BoostActionCount++;
        }
    }

    // FELIX  Create base test class :P
    public class AircraftBoostStepTests : TutorialStepTestsBase
    {
        private DummyBoostStep _boostStep;
        private IBoostProvidersManager _boostProvidersManager;
        private IObservableCollection<IBoostProvider> _aircraftBoostProviders;
        private IBoostProvider _boostProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _aircraftBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            _boostProvidersManager = Substitute.For<IBoostProvidersManager>();
            _boostProvidersManager.AircraftBoostProviders.Returns(_aircraftBoostProviders);

            _boostProvider = Substitute.For<IBoostProvider>();

            _boostStep = new DummyBoostStep(_args, _boostProvidersManager, _boostProvider);
        }

        [Test]
        public void Start_RunsBoostAction_AndCompletes()
        {
            _boostStep.Start(_completionCallback);

            Assert.AreEqual(1, _boostStep.BoostActionCount);
            Assert.AreEqual(1, _callbackCounter);
        }
    }
}
