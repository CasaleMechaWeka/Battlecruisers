using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Copy tests :)
    public class AddTurretFireRateBoostStepNEW : BoostStepNEW
    {
        public AddTurretFireRateBoostStepNEW(
            ITutorialStepArgsNEW args,
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
