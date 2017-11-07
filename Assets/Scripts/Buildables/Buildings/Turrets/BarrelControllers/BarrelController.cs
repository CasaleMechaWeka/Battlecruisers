using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class BarrelController : MonoBehaviour, ITargetConsumer, IBoostable
    {
        protected IFireIntervalManager _fireIntervalManager;
        protected ITargetFilter _targetFilter;
        protected ITargetPositionPredictor _targetPositionPredictor;
        protected IAngleCalculator _angleCalculator;
        protected IRotationMovementController _rotationMovementController;
        private IAccuracyAdjuster _accuracyAdjuster;
        private ITargetPositionValidator _targetPositionValidator;
		
        protected IProjectileStats _projectileStats;
        public IProjectileStats ProjectileStats { get { return _projectileStats; } }

        protected TurretStats _turretStats;
        public ITurretStats TurretStats { get { return _turretStats; } }

        public ITarget Target { get; set; }
        protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }
        protected virtual int NumOfBarrels { get { return 1; } }

        private bool IsInitialised { get { return _targetFilter != null; } }
        public Renderer[] Renderers { get; private set; }

        // Laziliy initialise, because requires StaticInitialise of this and all
        // child classes to complete first.
        private float _damagePerS;
        public virtual float DamagePerS
        {
            get
            {
                if (_damagePerS == default(float))
                {
                    _damagePerS = NumOfBarrels * _projectileStats.Damage * _turretStats.MeanFireRatePerS;
                }
                return _damagePerS;
            }
        }

        public float BoostMultiplier
        {
            get { return _turretStats.BoostMultiplier; }
            set { _turretStats.BoostMultiplier = value; }
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

        protected virtual TurretStats SetupTurretStats()
        {
            TurretStats turretStats = gameObject.GetComponent<TurretStats>();
            Assert.IsNotNull(turretStats);
            turretStats.Initialise();
            return turretStats;
        }

        protected virtual IProjectileStats GetProjectileStats()
        {
            ProjectileStats projectileStats = GetComponent<ProjectileStats>();
            Assert.IsNotNull(projectileStats);
            return new ProjectileStatsWrapper(projectileStats);
        }
		
		protected virtual IFireIntervalManager SetupFireIntervalManager(TurretStats turretStats)
		{
			FireIntervalManager fireIntervalManager = gameObject.GetComponent<FireIntervalManager>();
			Assert.IsNotNull(fireIntervalManager);
			fireIntervalManager.Initialise(turretStats);
			return fireIntervalManager;
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
		}

		void FixedUpdate()
		{
			if (!IsInitialised)
			{
				return;
			}

			if (Target != null && !Target.IsDestroyed)
			{
                Logging.Verbose(Tags.BARREL_CONTROLLER, "Target.Velocity: " + Target.Velocity);

				float currentAngleInRadians = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
                Vector2 predictedTargetPosition = _targetPositionPredictor.PredictTargetPosition(ProjectileSpawnerPosition, Target, _projectileStats.MaxVelocityInMPerS, currentAngleInRadians);

                float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(ProjectileSpawnerPosition, predictedTargetPosition, IsSourceMirrored, _projectileStats.MaxVelocityInMPerS);

				bool isOnTarget = _rotationMovementController.IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					_rotationMovementController.AdjustRotation(desiredAngleInDegrees);
				}

				if ((isOnTarget || _turretStats.IsInBurst)
				    && _fireIntervalManager.ShouldFire())
				{
					// Burst fires happen even if we are no longer on target, so we may miss
					// the target in this case.  Hence use the actual angle our turret barrel
					// is at, instead of the perfect desired angle.
					float fireAngle = _turretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

                    fireAngle = _accuracyAdjuster.FindAngleInDegrees(fireAngle, ProjectileSpawnerPosition, predictedTargetPosition, IsSourceMirrored);

					Fire(fireAngle);
                    _fireIntervalManager.OnFired();
				}
				else
				{
					CeaseFire();
				}
			}
			else
			{
				CeaseFire();
			}
		}

		protected abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }
	}
}
