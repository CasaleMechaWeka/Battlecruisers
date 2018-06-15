using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class RemoveAircraftBoostStep : AircraftBoostStep
    {
        public RemoveAircraftBoostStep(
            ITutorialStepArgs args,
            IGlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _globalBoostProviders.AircraftBoostProviders.Remove(_boostProvider);
        }
    }
}
