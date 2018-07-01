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

        private ITurretStats _baseTurretStats;
        private ITurretStatsWrapper _turretStatsWrapper;
        public ITurretStats TurretStats { get { return _turretStatsWrapper; } }

        public ITarget Target { get; set; }
        public bool IsSourceMirrored { get { return transform.IsMirrored(); } }
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

        public abstract Vector3 ProjectileSpawnerPosition { get; }

        public virtual void StaticInitialise()
        {
            // Usually > 0, but can be 0 (invisible barrel controller for fighters)
            Renderers = GetComponentsInChildren<Renderer>();

            _projectileStats = GetProjectileStats();
            _baseTurretStats = SetupTurretStats();
            _turretStatsWrapper = new TurretStatsWrapper(_baseTurretStats);
            _fireIntervalManager = SetupFireIntervalManager(TurretStats);
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
		
		protected virtual IFireIntervalManager SetupFireIntervalManager(ITurretStats turretStats)
		{
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			fireIntervalManager.Initialise(turretStats);
			return fireIntervalManager;
		}

        protected virtual IDamageCapability FindDamageCapabilities()
        {
            float damagePerS = NumOfBarrels * _projectileStats.Damage * TurretStats.MeanFireRatePerS;
            return new DamageCapability(damagePerS, TurretStats.AttackCapabilities);
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
            _turretStatsWrapper.TurretStats = args.FactoryProvider.TurretStatsFactory.CreateBoostedTurretStats(_baseTurretStats, args.LocalBoostProviders);
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

        // FELIX  Test this method somehow :/  Move functionality to testable class?  (Not MonoBehaviour :P)
        /// <returns><c>true</c>, if successfully fired, <c>false</c> otherwise.</returns>
        private bool TryFire()
        {
            if ((Target == null || Target.IsDestroyed)
                && !TurretStats.IsInBurst)
            {
                // No alive target to shoot
                Logging.Verbose(Tags.BARREL_CONTROLLER, "No alive target to shoot");
                return false;
            }

            // FELIX
            //Logging.Verbose(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

            float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 predictedTargetPosition = _targetPositionPredictor.PredictTargetPosition(ProjectileSpawnerPosition, Target, _projectileStats.MaxVelocityInMPerS, currentAngleInRadians);

            if (!_targetPositionValidator.IsValid(predictedTargetPosition, ProjectileSpawnerPosition, IsSourceMirrored))
            {
                // Target position is invalid
                // FELIX
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

            if ((!isOnTarget && !TurretStats.IsInBurst)
                || !_fireIntervalManager.ShouldFire())
            {
                // FELIX
                //Logging.Log("isOnTarget: " + isOnTarget + "  IsInBurst: " + TurretStats.IsInBurst);
                //Logging.Log("_fireIntervalManager.ShouldFire(): " + _fireIntervalManager.ShouldFire());

                // Not on target or haven't waited fire interval
                // FELIX
                Logging.Verbose(Tags.BARREL_CONTROLLER, "Not on target or haven't waited fire interval");
                return false;
            }

            // Burst fires happen even if we are no longer on target, so we may miss
            // the target in this case.  Hence use the actual angle our turret barrel
            // is at, instead of the perfect desired angle.
            float fireAngle = TurretStats.IsInBurst ? transform.rotation.eulerAngles.z : limitedDesiredAngle;
            Logging.Log(Tags.BARREL_CONTROLLER, "TryFire()  fireAngle: " + fireAngle + "  transform.rotation.eulerAngles.z: " + transform.rotation.eulerAngles.z);
            fireAngle = _accuracyAdjuster.FindAngleInDegrees(fireAngle, ProjectileSpawnerPosition, predictedTargetPosition, IsSourceMirrored);

            Fire(fireAngle);
            _fireIntervalManager.OnFired();

            return true;
        }

        public abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }
	}
}
