using System.Threading.Tasks;
using BattleCruisers.Effects;
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
        private IDeferrer _deferrer;

        public float delayInMs;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            IAnimationInitialiser barrelAnimationInitialiser = GetComponent<IAnimationInitialiser>();
            Assert.IsNotNull(barrelAnimationInitialiser);
            _barrelAnimation = barrelAnimationInitialiser.CreateAnimation();
        }

        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
        {
            await base.InternalInitialiseAsync(args);
            _deferrer = args.FactoryProvider.DeferrerProvider.Deferrer; ;
        }

        protected override IAnimation GetBarrelFiringAnimation(IBarrelControllerArgs args)
        {
            return _barrelAnimation;
        }

        public override void Fire(float angleInDegrees)
        {
            _deferrer.Defer(() => base.Fire(angleInDegrees), delayInMs / 1000);
        }
    }
}