using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class DeferredBarrelFirer : IBarrelFirer
    {
        private readonly IBarrelFirer _coreFirer;
        private readonly ConstantDeferrer _deferrer;

        public DeferredBarrelFirer(IBarrelFirer coreFirer, ConstantDeferrer deferrer)
        {
            Helper.AssertIsNotNull(coreFirer, deferrer);

            _coreFirer = coreFirer;
            _deferrer = deferrer;
        }

        public void Fire(float fireAngleInDegrees)
        {
            Logging.Verbose(Tags.BARREL_CONTROLLER, $"fireAngleInDegrees: {fireAngleInDegrees}");
            _deferrer.Defer(() => _coreFirer.Fire(fireAngleInDegrees));
        }
    }
}