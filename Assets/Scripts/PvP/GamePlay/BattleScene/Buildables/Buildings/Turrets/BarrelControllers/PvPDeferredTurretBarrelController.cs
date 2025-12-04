using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
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

        protected override IBarrelFirer CreateFirer(IPvPBarrelControllerArgs args)
        {
            return
                new DeferredBarrelFirer(
                    base.CreateFirer(args),
                    new ConstantDeferrer(PvPFactoryProvider.DeferrerProvider.Deferrer, delayInS));
        }
    }
}