using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Tutorial.Steps.BoostSteps
{
    public class AddAircraftBoostStep : BoostStep
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
