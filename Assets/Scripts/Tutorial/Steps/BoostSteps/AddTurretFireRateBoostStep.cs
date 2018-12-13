using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    public class AddTurretFireRateBoostStep : BoostStep
    {
        public AddTurretFireRateBoostStep(
            ITutorialStepArgs args,
            IGlobalBoostProviders globalBoostProviders,
            IBoostProvider boostProvider)
            : base(args, globalBoostProviders, boostProvider)
        {
        }

        protected override void BoostProviderAction()
        {
            _globalBoostProviders.TurretFireRateBoostProviders.Add(_boostProvider);
        }
    }
}
