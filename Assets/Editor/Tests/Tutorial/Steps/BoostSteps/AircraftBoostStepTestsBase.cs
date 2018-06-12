using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public abstract class AircraftBoostStepTestsBase : TutorialStepTestsBase
    {
        protected IBoostProvidersManager _boostProvidersManager;
        protected IObservableCollection<IBoostProvider> _aircraftBoostProviders;
        protected IBoostProvider _boostProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _aircraftBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            _boostProvidersManager = Substitute.For<IBoostProvidersManager>();
            _boostProvidersManager.AircraftBoostProviders.Returns(_aircraftBoostProviders);

            _boostProvider = Substitute.For<IBoostProvider>();
        }
    }
}
