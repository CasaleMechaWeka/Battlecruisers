using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Tutorial.Steps.BoostSteps
{
    public class AddTurretAccuracyBoostStep : BoostStep
    {
        public AddTurretAccuracyBoostStep(
            ITutorialStepArgs args,
            IGlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _globalBoostProviders.TurretAccuracyBoostProviders.Add(_boostProvider);
        }
    }
}
