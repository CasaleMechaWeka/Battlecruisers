using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Copy tests :)
    // FELIX  Rename, type :P
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
