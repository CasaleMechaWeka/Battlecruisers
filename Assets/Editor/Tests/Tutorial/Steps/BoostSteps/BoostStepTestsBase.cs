using BattleCruisers.Buildables.Boost;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.BoostSteps
{
    public abstract class BoostStepTestsBase : TutorialStepTestsBase
    {
        protected IGlobalBoostProviders _globalBoostProviders;
        protected IBoostProvider _boostProvider;

        [SetUp]
        public override void SetuUp()
        {
            base.SetuUp();

            _globalBoostProviders = Substitute.For<IGlobalBoostProviders>();

            IObservableCollection<IBoostProvider> aircraftBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            _globalBoostProviders.AircraftBoostProviders.Returns(aircraftBoostProviders);

            IObservableCollection<IBoostProvider> turretAccuracyBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            _globalBoostProviders.TurretAccuracyBoostProviders.Returns(turretAccuracyBoostProviders);

            IObservableCollection<IBoostProvider> turretFireRateBoostProviders = Substitute.For<IObservableCollection<IBoostProvider>>();
            _globalBoostProviders.TurretFireRateBoostProviders.Returns(turretFireRateBoostProviders);

            _boostProvider = Substitute.For<IBoostProvider>();
        }
    }
}
