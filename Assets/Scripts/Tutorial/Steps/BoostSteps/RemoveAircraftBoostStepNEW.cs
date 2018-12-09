using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Copy tests :)
    public class RemoveAircraftBoostStepNEW : BoostStepNEW
    {
        public RemoveAircraftBoostStepNEW(
            ITutorialStepArgsNEW args,
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
