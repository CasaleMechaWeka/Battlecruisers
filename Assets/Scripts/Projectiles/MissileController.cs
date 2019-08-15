using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Projectiles
{
    public class MissileController : 
        ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, 
        ITargetProvider
	{
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

        public  ITarget Target { get; private set; }

        public override void Activate(TargetProviderActivationArgs<IProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

			Target = activationArgs.Target;
            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
			ITargetProvider targetProvider = this;

			MovementController 
                = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody, 
                    maxVelocityProvider, 
                    targetProvider, 
                    _factoryProvider.TargetPositionPredictorFactory);

            _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();

            activationArgs.Target.Destroyed += Target_Destroyed;
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