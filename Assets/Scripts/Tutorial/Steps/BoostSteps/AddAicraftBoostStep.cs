using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Test :P
    public class AddAircraftBoostStep : AircraftBoostStep
    {
        public AddAircraftBoostStep(
            ITutorialStepArgs args,
            IBoostProvidersManager boostProvidersManager,
            IBoostProvider boostProvider)
            : base(args, boostProvidersManager, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _boostProvidersManager.AircraftBoostProviders.Add(_boostProvider);
        }
    }
}
