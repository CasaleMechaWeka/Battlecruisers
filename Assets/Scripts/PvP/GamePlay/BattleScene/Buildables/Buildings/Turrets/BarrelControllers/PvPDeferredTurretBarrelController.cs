using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPDeferredTurretBarrelController : PvPShellTurretBarrelController
    {
        public float delayInS;

        public override void StaticInitialise()
        {
            base.StaticInitialise();
            Assert.IsTrue(delayInS >= 0);
        }

        protected override IPvPBarrelFirer CreateFirer(IPvPBarrelControllerArgs args)
        {
            return
                new PvPDeferredBarrelFirer(
                    base.CreateFirer(args),
                    new ConstantDeferrer(args.FactoryProvider.DeferrerProvider.Deferrer, delayInS));
        }
    }
}