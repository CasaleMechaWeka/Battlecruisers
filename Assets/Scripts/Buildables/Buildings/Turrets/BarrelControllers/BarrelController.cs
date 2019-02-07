using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class BarrelController : MonoBehaviour, IBarrelController
    {
        private IBarrelAdjustmentHelper _adjustmentHelper;
        private IBarrelFiringHelper _firingHelper;
        private IFireIntervalManager _fireIntervalManager;
        private bool _isCleanedUp;
        protected ITargetFilter _targetFilter;
		
        protected IProjectileStats _projectileStats;
        public IProjectileStats ProjectileStats { get { return _projectileStats; } }

        private ITurretStats _baseTurretStats;
        private ITurretStatsWrapper _turretStatsWrapper;
        public ITurretStats TurretStats { get { return _turretStatsWrapper; } }

        public ITarget Target { get; set; }
        public ITarget CurrentTarget { get { return Target; } }
        public bool IsSourceMirrored { get { return transform.IsMirrored(); } }
        protected virtual int NumOfBarrels { get { return 1; } }
        public Transform Transform { get { return transform; } }
        public float BarrelAngleInDegrees { get { return Transform.rotation.eulerAngles.z; } }

        private bool IsInitialised { get { return _targetFilter != null; } }
        private bool IsActive { get { return IsInitialised && !_isCleanedUp; } }
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
        public abstract bool CanFireWithoutTarget { get; }

        public virtual void StaticInitialise()
        {
            // Usually > 0, but can be 0 (invisible barrel controller for fighters)
            Renderers = GetComponentsInChildren<Renderer>();

            _projectileStats = GetProjectileStats();
            _baseTurretStats = SetupTurretStats();
            _turretStatsWrapper = new TurretStatsWrapper(_baseTurretStats);
            _fireIntervalManager = SetupFireIntervalManager(TurretStats);
            _isCleanedUp = false;
        }
		
		protected virtual IProjectileStats GetProjectileStats()
		{
			ProjectileStats projectileStats = GetComponent<ProjectileStats>();
			Assert.IsNotNull(projectileStats);
			return projectileStats;
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
			FireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<FireIntervalManagerInitialiser>();
			Assert.IsNotNull(fireIntervalManagerInitialiser);
			return fireIntervalManagerInitialiser.Initialise(turretStats);
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
            _turretStatsWrapper.TurretStats 
                = args.FactoryProvider.Turrets.TurretStatsFactory.CreateBoostedTurretStats(
                    _baseTurretStats, 
                    args.LocalBoostProviders,
                    args.GlobalFireRateBoostProviders);

            _adjustmentHelper
                = new BarrelAdjustmentHelper(
                    this,
                    args.TargetPositionPredictor,
                    args.TargetPositionValidator,
                    args.AngleCalculator,
                    args.RotationMovementController,
                    args.AngleLimiter,
                    args.AttackablePositionFinder);

            _firingHelper = new BarrelFiringHelper(this, args.AccuracyAdjuster, _fireIntervalManager);
        }

		void FixedUpdate()
        {
            if (!IsActive)
            {
                return;
            }

            _fireIntervalManager.ProcessTimeInterval(Time.deltaTime);
            BarrelAdjustmentResult adjustmentResult = _adjustmentHelper.AdjustTurretBarrel();
            bool wasFireSuccessful = _firingHelper.TryFire(adjustmentResult);

            if (!wasFireSuccessful)
            {
                CeaseFire();
            }
        }

        public abstract void Fire(float angleInDegrees);

		protected virtual void CeaseFire() { }

        public void CleanUp()
        {
            _isCleanedUp = true;
        }
    }
}
