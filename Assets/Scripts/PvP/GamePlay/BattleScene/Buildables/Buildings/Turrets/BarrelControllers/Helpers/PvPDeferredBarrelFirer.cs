using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers
{
    public class PvPDeferredBarrelFirer : IPvPBarrelFirer
    {
        private readonly IPvPBarrelFirer _coreFirer;
        private readonly IPvPConstantDeferrer _deferrer;

        public PvPDeferredBarrelFirer(IPvPBarrelFirer coreFirer, IPvPConstantDeferrer deferrer)
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