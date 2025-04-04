using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Effects;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    /// <summary>
    /// The firing animation is per turret barrel, instead of for the whole
    /// turret building (ie, no animation outside of the turret barrel).
    /// Hence need a custom way of grabbing this animation.
    /// </summary>
    public class BroadsidesTurretBarrelController : ShellTurretBarrelController
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

        protected override IAnimation GetBarrelFiringAnimation(BarrelControllerArgs args)
        {
            return _barrelAnimation;
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