using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class AddAircraftBoostStep : AircraftBoostStep
    {
        public AddAircraftBoostStep(
            ITutorialStepArgs args,
            IGlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _globalBoostProviders.AircraftBoostProviders.Add(_boostProvider);
        }
    }
}
