using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Tutorial.Steps.BoostSteps
{
    public class RemoveAircraftBoostStep : BoostStep
    {
        public RemoveAircraftBoostStep(
            ITutorialStepArgs args,
            GlobalBoostProviders globalBoostProviders,
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
