using BattleCruisers.Buildables.Boost;

// FELIX  Change namespace to "BoostSteps"
namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class AddAircraftBoostStepNEW : BoostStepNEW
    {
        public AddAircraftBoostStepNEW(
            ITutorialStepArgsNEW args,
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
