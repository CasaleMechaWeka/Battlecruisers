using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class BarrelController : MonoBehaviour, IBarrelController
    {
        protected IFireIntervalManager _fireIntervalManager;
        protected ITargetFilter _targetFilter;
        protected ITargetPositionPredictor _targetPositionPredictor;
        protected IAngleCalculator _angleCalculator;
        protected IRotationMovementController _rotationMovementController;
        private IAccuracyAdjuster _accuracyAdjuster;
        private ITargetPositionValidator _targetPositionValidator;
        private IAngleLimiter _angleLimiter;
		
        protected IProjectileStats _projectileStats;
        public IProjectileStats ProjectileStats { get { return _projectileStats; } }

        protected TurretStats _turretStats;
        public ITurretStats TurretStats { get { return _turretStats; } }

        public ITarget Target { get; set; }
        protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }
        protected virtual int NumOfBarrels { get { return 1; } }
        public Transform Transform { get { return transform; } }

        private bool IsInitialised { get { return _targetFilter != null; } }
        public Renderer[] Renderers { get; private set; }

        // Initialise lazily, because requires child class StaticInitialise()s to have completed.
        private IDamageCapability _damageCapability;
        public IDamageCapability DamageCapability
        {
            get
            {
                if (_damageCapability == null)
                {
                    _damageCapability = FindDamageCapabilities();
                }
                return _damageCapability;
            }
        }

        public float BoostMultiplier
        {
            // FELIX
            get { return 0; }
            set {  }
        }

        protected abstract Vector3 ProjectileSpawnerPosition { get; }

        public virtual void StaticInitialise()
        {
            // Usually > 0, but can be 0 (invisible barrel controller for fighters)
            Renderers = GetComponentsInChildren<Renderer>();

            _projectileStats = GetProjectileStats();
            _turretStats = SetupTurretStats();
            _fireIntervalManager = SetupFireIntervalManager(_turretStats);
        }
		
		protected virtual IProjectileStats GetProjectileStats()
		{
			ProjectileStats projectileStats = GetComponent<ProjectileStats>();
			Assert.IsNotNull(projectileStats);
			return new ProjectileStatsWrapper(projectileStats);
		}

        protected virtual TurretStats SetupTurretStats()
        {
            TurretStats turretStats = gameObject.GetComponent<TurretStats>();
            Assert.IsNotNull(turretStats);
            turretStats.Initialise();
            return turretStats;
        }
		
		protected virtual IFireIntervalManager SetupFireIntervalManager(TurretStats turretStats)
		{
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			fireIntervalManager.Initialise(turretStats);
			return fireIntervalManager;
		}

        protected virtual IDamageCapability FindDamageCapabilities()
        {
            float damagePerS = NumOfBarrels * _projectileStats.Damage * _turretStats.MeanFireRatePerS;
            return new DamageCapability(damagePerS, _turretStats.AttackCapabilities);
        }

        public virtual void Initialise(IBarrelControllerArgs args)
		{
            Assert.IsNotNull(args);

            _targetFilter = args.TargetFilter;
            _targetPositionPredictor = args.TargetPositionPredictor;
			_angleCalculator = args.AngleCalculator;
			_rotationMovementController = args.RotationMovementController;
            _accuracyAdjuster = args.AccuracyAdjuster;
            _targetPositionValidator = args.TargetPositionValidator;
            _angleLimiter = args.AngleLimiter;
		}

		void FixedUpdate()
        {
            if (!IsInitialised)
            {
                return;
            }

            bool wasFireSuccessful = TryFire();

            if (!wasFireSuccessful)
            {
                CeaseFire();
            }
        }

        /// <returns><c>true</c>, if successfully fired, <c>false</c> otherwise.</returns>
        private bool TryFire()
        {
            if (Target == null || Target.IsDestroyed)
            {
                // No alive target to shoot
                return false;
            }

            Logging.Verbose(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

            float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 predictedTargetPosition = _targetPositionPredictor.PredictTargetPosition(ProjectileSpawnerPosition, Target, _projectileStats.MaxVelocityInMPerS, currentAngleInRadians);

            if (!_targetPositionValidator.IsValid(predictedTargetPosition, ProjectileSpawnerPosition, IsSourceMirrored))
            {
                // Target position is invalid
                Logging.Verbose(Tags.BARREL_CONTROLLER, "Target position is invalid");
                return false;
            }

            float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(ProjectileSpawnerPosition, predictedTargetPosition, IsSourceMirrored, _projectileStats.MaxVelocityInMPerS);
            bool isOnTarget = _rotationMovementController.IsOnTarget(desiredAngleInDegrees);
            float limitedDesiredAngle = _angleLimiter.LimitAngle(desiredAngleInDegrees);

            if (!isOnTarget)
            {
                _rotationMovementController.AdjustRotation(limitedDesiredAngle);
            }

            if ((!isOnTarget && !_turretStats.IsInBurst)
                || !_fireIntervalManager.ShouldFire())
            {
                // Not on target or haven't waited fire interval
                Logging.Verbose(Tags.BARREL_CONTROLLER, "Not on target or haven't waited fire interval");
                return false;
            }

            // Burst fires happen even if we are no longer on target, so we may miss
            // the target in this case.  Hence use the actual angle our turret barrel
            // is at, instead of the perfect desired angle.
            float fireAngle = _turretStats.IsInBurst ? transform.rotation.eulerAngles.z : limitedDesiredAngle;
            Logging.Log(Tags.BARREL_CONTROLLER, "TryFire()  fireAngle: " + fireAngle + "  transform.rotation.eulerAngles.z: " + transform.rotation.eulerAngles.z);
            fireAngle = _accuracyAdjuster.FindAngleInDegrees(fireAngle, ProjectileSpawnerPosition, predictedTargetPosition, IsSourceMirrored);

            Fire(fireAngle);
            _fireIntervalManager.OnFired();

            return true;
        }

        protected abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }
	}
}
