using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Threading;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class MissileController : ProjectileController, ITargetProvider
	{
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

        public  ITarget Target { get; private set; }

		public void Initialise(
            IProjectileStats missileStats, 
            Vector2 initialVelocityInMPerS, 
            ITargetFilter targetFilter, 
            ITarget target,
            IFactoryProvider factoryProvider,
            ITarget parent)
		{
            base.Initialise(missileStats, initialVelocityInMPerS, targetFilter, factoryProvider, parent);

			Target = target;
            _deferrer = factoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(missileStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			MovementController 
                = factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    factoryProvider.TargetPositionPredictorFactory);

            _dummyMovementController = factoryProvider.MovementControllerFactory.CreateDummyMovementController();

			target.Destroyed += Target_Destroyed;
		}

		private void Target_Destroyed(object sender, DestroyedEventArgs e)
		{
            // Let missile keep current velocity
            MovementController = _dummyMovementController;

            // Destroy missile eventually (in case it does not hit a matching target)
            _deferrer.Defer(ConditionalDestroy, MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S);
		}

        private void ConditionalDestroy()
        {
            if (this != null)
            {
                DestroyProjectile();
            }
        }

		protected override void DestroyProjectile()
		{
			Target.Destroyed -= Target_Destroyed;
			base.DestroyProjectile();
		}
	}
}