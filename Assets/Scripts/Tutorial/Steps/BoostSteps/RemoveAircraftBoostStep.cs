using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class RemoveAircraftBoostStep : AircraftBoostStep
    {
        public RemoveAircraftBoostStep(
            ITutorialStepArgs args,
            IBoostProvidersManager boostProvidersManager,
            IBoostProvider boostProvider)
            : base(args, boostProvidersManager, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _boostProvidersManager.AircraftBoostProviders.Remove(_boostProvider);
        }
    }
}
