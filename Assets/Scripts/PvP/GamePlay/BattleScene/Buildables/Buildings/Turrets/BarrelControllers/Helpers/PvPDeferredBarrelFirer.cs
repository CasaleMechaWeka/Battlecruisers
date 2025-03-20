using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPDeferredBarrelFirer : IBarrelFirer
    {
        private readonly IBarrelFirer _coreFirer;
        private readonly ConstantDeferrer _deferrer;

        public PvPDeferredBarrelFirer(IBarrelFirer coreFirer, ConstantDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(coreFirer, deferrer);

            _coreFirer = coreFirer;
            _deferrer = deferrer;
        }

        public void Fire(float fireAngleInDegrees)
        {
            // Logging.Verbose(Tags.BARREL_CONTROLLER, $"fireAngleInDegrees: {fireAngleInDegrees}");
            _deferrer.Defer(() => _coreFirer.Fire(fireAngleInDegrees));
        }
    }
}