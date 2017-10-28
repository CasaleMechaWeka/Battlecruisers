using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Projectiles
{
    public class MissileController : ProjectileController, ITargetProvider
	{
		public  ITarget Target { get; private set; }

		public void Initialise(
            IProjectileStats missileStats, 
            Vector2 initialVelocityInMPerS, 
            ITargetFilter targetFilter, 
            ITarget target,
            IFactoryProvider factoryProvider)
		{
            base.Initialise(missileStats, initialVelocityInMPerS, targetFilter, factoryProvider);

			Target = target;

            IVelocityProvider maxVelocityProvider = factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(missileStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			_movementController 
                = factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    factoryProvider.TargetPositionPredictorFactory);

			target.Destroyed += Target_Destroyed;
		}

		private void Target_Destroyed(object sender, DestroyedEventArgs e)
		{
			DestroyProjectile();
		}

		protected override void DestroyProjectile()
		{
			Target.Destroyed -= Target_Destroyed;
			base.DestroyProjectile();
		}
	}
}