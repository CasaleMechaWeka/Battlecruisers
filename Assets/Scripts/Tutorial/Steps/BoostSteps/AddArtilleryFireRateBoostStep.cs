using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;

namespace BattleCruisers.Tutorial.Steps.BoostSteps
{
    public class AddArtilleryFireRateBoostStep : BoostStep
    {
        public AddArtilleryFireRateBoostStep(
            ITutorialStepArgs args,
            IGlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _globalBoostProviders.OffenseFireRateBoostProviders.Add(_boostProvider);
        }
    }
}
