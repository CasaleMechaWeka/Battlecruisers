using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class DeferredTurretBarrelController : ShellTurretBarrelController
    {
        public float delayInS;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsTrue(delayInS >= 0);
        }

        protected override IBarrelFirer CreateFirer(BarrelControllerArgs args)
        {
            return
                new DeferredBarrelFirer(
                    base.CreateFirer(args),
                    new ConstantDeferrer(FactoryProvider.DeferrerProvider.Deferrer, delayInS));
        }
    }
}