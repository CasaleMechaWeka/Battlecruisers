using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class MissileController : ProjectileController, ITargetProvider
	{
        private IMovementController _dummyMovementController;

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

            // FELIX  Add timeout, so projectile is eventually destroyed
			//DestroyProjectile();
		}

		protected override void DestroyProjectile()
		{
			Target.Destroyed -= Target_Destroyed;
			base.DestroyProjectile();
		}
	}
}