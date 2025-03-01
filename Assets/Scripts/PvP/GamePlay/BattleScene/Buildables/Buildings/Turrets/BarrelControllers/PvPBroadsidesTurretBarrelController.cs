using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    /// <summary>
    /// The firing animation is per turret barrel, instead of for the whole
    /// turret building (ie, no animation outside of the turret barrel).
    /// Hence need a custom way of grabbing this animation.
    /// </summary>
    public class PvPBroadsidesTurretBarrelController : PvPShellTurretBarrelController
    {
        private IAnimation _barrelAnimation;

        public float delayInS;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            Assert.IsTrue(delayInS >= 0);

            IAnimationInitialiser barrelAnimationInitialiser = GetComponent<IAnimationInitialiser>();
            Assert.IsNotNull(barrelAnimationInitialiser);
            _barrelAnimation = barrelAnimationInitialiser.CreateAnimation();
        }

        protected override IAnimation GetBarrelFiringAnimation(IPvPBarrelControllerArgs args)
        {
            return _barrelAnimation;
        }

        protected override IBarrelFirer CreateFirer(IPvPBarrelControllerArgs args)
        {
            return
                new PvPDeferredBarrelFirer(
                    base.CreateFirer(args),
                    new ConstantDeferrer(args.FactoryProvider.DeferrerProvider.Deferrer, delayInS));
        }
    }
}