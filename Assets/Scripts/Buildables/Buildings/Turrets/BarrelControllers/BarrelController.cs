using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
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
    public abstract class BarrelController : MonoBehaviour, ITargetConsumer
    {
        protected IProjectileStats _projectileStats;
        protected IFireIntervalManager _fireIntervalManager;
        protected ITargetFilter _targetFilter;
        protected IAngleCalculator _angleCalculator;
        protected IRotationMovementController _rotationMovementController;

        public ITarget Target { get; set; }
        protected bool IsSourceMirrored { get { return transform.IsMirrored(); } }

        public TurretStats TurretStats { get; private set; }
        private bool IsInitialised { get { return _targetFilter != null; } }
        public Renderer[] Renderers { get; private set; }

		public virtual void StaticInitialise()
        {
            // Usually > 0, but can be 0 (invisible barrel controller for fighters)
            Renderers = GetComponentsInChildren<Renderer>();

            _projectileStats = GetProjectileStats();
            TurretStats = SetupTurretStats();
            _fireIntervalManager = SetupFireIntervalManager(TurretStats);
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
            // FELIX  Check this finds inactive projectile stats?
            ProjectileStats projectileStats = GetComponent<ProjectileStats>();
            //IProjectileStats projectileStats = GetComponentInChildren<IProjectileStats>(includeInactive: true);
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

        public virtual void Initialise(ITargetFilter targetFilter, IAngleCalculator angleCalculator, IRotationMovementController rotationMovementController)
		{
			_targetFilter = targetFilter;
			_angleCalculator = angleCalculator;
			_rotationMovementController = rotationMovementController;
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
				float desiredAngleInDegrees = _angleCalculator.FindDesiredAngle(transform.position, Target, IsSourceMirrored, TurretStats.bulletVelocityInMPerS, currentAngleInRadians);

				bool isOnTarget = _rotationMovementController.IsOnTarget(desiredAngleInDegrees);

				if (!isOnTarget)
				{
					_rotationMovementController.AdjustRotation(desiredAngleInDegrees);
				}

				if ((isOnTarget || TurretStats.IsInBurst)
				    && _fireIntervalManager.ShouldFire())
				{
					// Burst fires happen even if we are no longer on target, so we may miss
					// the target in this case.  Hence use the actual angle our turret barrel
					// is at, instead of the perfect desired angle.
					float fireAngle = TurretStats.IsInBurst ? transform.rotation.eulerAngles.z : desiredAngleInDegrees;

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
