using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    /// <summary>
    /// By default targets the enemy cruiser.
    /// 
    /// Detects nearby targets, and switches to them.
    /// 
    /// Once a target has been detected:
    /// + Turns off target detection
    /// + Speeds up (FELIX  Need new movement controller :P)
    /// </summary>
    public class SmartMissileController :
        ProjectileWithTrail<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, 
        ITargetProvider,
        ITargetConsumer
	{
        private IDeferrer _deferrer;
        private IMovementController _dummyMovementController;
        private ITargetProcessor _targetProcessor;

        private const float MISSILE_POST_TARGET_DESTROYED_LIFETIME_IN_S = 2;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;

        private ITarget _target;
        public  ITarget Target
        {
            get => _target;
            set
            {
                if (_target != null)
                {
                    _target.Destroyed -= _target_Destroyed;
                }

                if (value == null)
                {
                    // Keep initial non null target
                    return;
                }

                _target = value;

                _target.Destroyed += _target_Destroyed;

                // Only care about first target. Hence can clean up target processor
                CleanUpTargetProcessor();
            }
        }

        public override void Initialise(ILocTable commonStrings, IFactoryProvider factoryProvider)
        {
            base.Initialise(commonStrings, factoryProvider);
            Assert.IsNotNull(missile);
        }

        public override void Activate(TargetProviderActivationArgs<IProjectileStats> activationArgs)
        {
            base.Activate(activationArgs);

            // Should be the enemy cruiser
			Target = activationArgs.Target;
            Assert.IsTrue(Target is ICruiser);

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

            // FELIX  Create target producer

            missile.enabled = true;
		}

        private void ReleaseMissile()
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
            _target.Destroyed -= _target_Destroyed;
            CleanUpTargetProcessor();
			base.DestroyProjectile();
		}

        private void _target_Destroyed(object sender, DestroyedEventArgs e)
        {
            e.DestroyedTarget.Destroyed -= _target_Destroyed;
            ReleaseMissile();
        }

        private void CleanUpTargetProcessor()
        {
            if (_targetProcessor != null)
            {
                _targetProcessor.RemoveTargetConsumer(this);
                _targetProcessor.DisposeManagedState();
                _targetProcessor = null;
            }
        }
    }
}