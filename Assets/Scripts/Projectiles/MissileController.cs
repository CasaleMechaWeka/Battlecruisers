using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class MissileController :
        ProjectileWithTrail<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, 
        ITargetProvider
	{
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

        private RocketTarget _rocketTarget;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;
        public  ITarget Target { get; private set; }

        public override void Initialise(ILocTable commonStrings, IFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            Assert.IsNotNull(missile);
        }

        public override void Activate(TargetProviderActivationArgs<IProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            Logging.Log(Tags.MISSILE, $"Rotation: {transform.rotation.eulerAngles}");

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
            missile.enabled = true;

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(_commonStrings, activationArgs.Parent.Faction, _rigidBody, this);

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
            if (gameObject.activeSelf)
            {
                DestroyProjectile();
            }
        }

		protected override void DestroyProjectile()
		{
            missile.enabled = false;
            _rocketTarget.GameObject.SetActive(false);
            Target.Destroyed -= Target_Destroyed;
			base.DestroyProjectile();
		}
	}
}