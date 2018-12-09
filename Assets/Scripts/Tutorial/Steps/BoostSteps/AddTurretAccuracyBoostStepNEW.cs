using BattleCruisers.Buildables.Boost;

namespace BattleCruisers.Tutorial.Steps.EnemyCruiser
{
    // FELIX  Copy tests :)
    public class AddTurretAccuracyBoostStepNEW : BoostStepNEW
    {
        public AddTurretAccuracyBoostStepNEW(
            ITutorialStepArgsNEW args,
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
