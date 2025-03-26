using BattleCruisers.Buildables;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
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

        private const float SELF_DETONATION_TIMER = 1.75f;
        private const float SELF_DETONATION_VARIANCE = .5f;

        private RocketTarget _rocketTarget;

        public SpriteRenderer missile;

        protected override float TrailLifetimeInS => 3;
        public ITarget Target { get; private set; }

        private TargetProviderActivationArgs<IProjectileStats> _activationArgs;

        public override void Initialise(FactoryProvider factoryProvider)
        {
            base.Initialise(factoryProvider);

            _rocketTarget = GetComponentInChildren<RocketTarget>();
            Assert.IsNotNull(_rocketTarget);

            Assert.IsNotNull(missile);
        }

        public override void Activate(TargetProviderActivationArgs<IProjectileStats> activationArgs)
        {
            _activationArgs = activationArgs;
            base.Activate(_activationArgs);

            Logging.Log(Tags.MISSILE, $"Rotation: {transform.rotation.eulerAngles}");

            Target = _activationArgs.Target;
            _deferrer = _factoryProvider.DeferrerProvider.Deferrer;

            IVelocityProvider maxVelocityProvider = _factoryProvider.MovementControllerFactory.CreateStaticVelocityProvider(activationArgs.ProjectileStats.MaxVelocityInMPerS);
            ITargetProvider targetProvider = this;

            MovementController
                = _factoryProvider.MovementControllerFactory.CreateMissileMovementController(
                    _rigidBody,
                    maxVelocityProvider,
                    targetProvider);

            _dummyMovementController = _factoryProvider.MovementControllerFactory.CreateDummyMovementController();
            missile.enabled = true;

            _rocketTarget.GameObject.SetActive(true);
            _rocketTarget.Initialise(_activationArgs.Parent.Faction, _rigidBody, this);

            _activationArgs.Target.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            // Let missile keep current velocity
            MovementController = _dummyMovementController;

            // Destroy missile eventually (in case it does not hit a matching target)
            _deferrer.Defer(ConditionalDestroy, Random.Range(SELF_DETONATION_TIMER, SELF_DETONATION_TIMER + SELF_DETONATION_VARIANCE));
        }


        private void ConditionalDestroy()
        {
            if (gameObject.activeSelf && missile.enabled)
                DestroyProjectile();
        }

        // Override the OnImpactCleanUp to NOT call base.OnImpactCleanUp() since that would 
        // hide ALL effects including trails
        protected override void OnImpactCleanUp()
        {
            // Do NOT call base.OnImpactCleanUp() as it would hide the trail
            // Instead, manually implement the parts we need:
            
            // Stop movement but don't touch the trail
            _rigidBody.velocity = Vector2.zero;
            MovementController = null;
            
            // Disable collision
            GetComponent<Collider2D>().enabled = false;
            
            // Hide the missile sprite only, not the trail
            missile.enabled = false;
            
            // Deactivate rocket target but keep the main object active for trail lifetime
            _rocketTarget.GameObject.SetActive(false);
            
            // Unsubscribe from target destruction event
            if (_activationArgs != null && Target != null)
            {
                Target.Destroyed -= Target_Destroyed;
            }
        }

        protected override void DestroyProjectile()
        {
            // Show explosion and hide the sprite
            ShowExplosion();
            OnImpactCleanUp();
            
            // Invoke destroyed and defer cleanup after trail lifetime
            InvokeDestroyed();
            _deferrer.Defer(OnTrailsDoneCleanup, TrailLifetimeInS);
        }
        
        // Add our own cleanup method for when trails are done
        private void OnTrailsDoneCleanup()
        {
            gameObject.SetActive(false);
            InvokeDeactivated();
        }
    }
}