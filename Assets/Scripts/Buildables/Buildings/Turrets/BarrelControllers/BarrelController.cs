using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class BarrelController : MonoBehaviour, IBarrelController
    {
        [SerializeField] protected bool disableAnimator = false;
        private BarrelAdjustmentHelper _adjustmentHelper;
        private IBarrelFiringHelper _firingHelper;
        private IUpdater _updater;
        private IParticleSystemGroup _muzzleFlash;
        private ITarget _parent;
        protected ITargetFilter _targetFilter;
        protected FireIntervalManager _fireIntervalManager;

        protected IProjectileStats _projectileStats;
        public IProjectileStats ProjectileStats => _projectileStats;

        protected TurretStats _baseTurretStats;
        private ITurretStatsWrapper _turretStatsWrapper;
        public ITurretStats TurretStats => _turretStatsWrapper;

        public ITarget Target { get; set; }
        public ITarget CurrentTarget => Target;
        public bool IsSourceMirrored => transform.IsMirrored();
        protected virtual int NumOfBarrels => 1;
        public Transform Transform => transform;
        public float BarrelAngleInDegrees => Transform.rotation.eulerAngles.z;


        protected List<TargetType> attackCapabilities;


        public SpriteRenderer[] Renderers { get; private set; }

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
            Renderers = GetComponentsInChildren<SpriteRenderer>();

            _projectileStats = GetProjectileStats();
            _baseTurretStats = SetupTurretStats();
            _turretStatsWrapper = new TurretStatsWrapper(_baseTurretStats);
            _fireIntervalManager = SetupFireIntervalManager(TurretStats);

            IParticleSystemGroupInitialiser muzzleFlashInitialiser = transform.FindNamedComponent<IParticleSystemGroupInitialiser>("MuzzleFlash");
            _muzzleFlash = muzzleFlashInitialiser.CreateParticleSystemGroup();
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

            // Get the attack capabilities from the BasicTurretStats component
            attackCapabilities = turretStats.AttackCapabilities.ToList();

            return turretStats;
        }

        public virtual void ApplyVariantStats(IBuilding building)
        {
            int variantIndex = building.variantIndex;
            if (variantIndex != -1)
            {
                if (variantIndex != -1)
                {
                    VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                    // turret stats
                    _baseTurretStats.ApplyVariantStats(variant.statVariant);
                    GetComponent<ProjectileStats>().ApplyVariantStats(variant.statVariant);
                }
            }
        }
        public virtual void ApplyVariantStats(IUnit unit)
        {
            int variantIndex = unit.variantIndex;
            if (variantIndex != -1)
            {
                VariantPrefab variant = PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                // turret stats
                _baseTurretStats.ApplyVariantStats(variant.statVariant);
                GetComponent<ProjectileStats>().ApplyVariantStats(variant.statVariant);
            }
        }

        protected virtual FireIntervalManager SetupFireIntervalManager(ITurretStats turretStats)
        {
            FireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<FireIntervalManagerInitialiser>();
            Assert.IsNotNull(fireIntervalManagerInitialiser);
            return fireIntervalManagerInitialiser.Initialise(turretStats);
        }

        protected virtual IDamageCapability FindDamageCapabilities()
        {
            float damagePerS = NumOfBarrels * _projectileStats.Damage * TurretStats.MeanFireRatePerS;
            return new DamageCapability(damagePerS, attackCapabilities);
        }


        public async Task InitialiseAsync(BarrelControllerArgs args, bool doDebug = false)
        {
            Assert.IsNotNull(args);

            _parent = args.Parent;
            _targetFilter = args.TargetFilter;
            _turretStatsWrapper.TurretStats
                = args.CruiserSpecificFactories.TurretStatsFactory.CreateBoostedTurretStats(
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
                    args.AngleLimiter);

            _firingHelper
                = new BarrelFiringHelper(
                    this,
                    args.AccuracyAdjuster,
                    _fireIntervalManager,
                    CreateFirer(args),
                    doDebug);

            await InternalInitialiseAsync(args);

            _updater = args.Updater;
            _updater.Updated += _updater_Updated;
        }

        protected virtual IBarrelFirer CreateFirer(BarrelControllerArgs args)
        {
            return new BarrelFirer(
                this,
                disableAnimator ? null : GetBarrelFiringAnimation(args),  // Only get animation if not disabled
                _muzzleFlash);
        }

        protected virtual IAnimation GetBarrelFiringAnimation(BarrelControllerArgs args)
        {
            return args.BarrelFiringAnimation;
        }


#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected virtual async Task InternalInitialiseAsync(BarrelControllerArgs args) { }
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

        private void _updater_Updated(object sender, EventArgs e)
        {
            _fireIntervalManager.ProcessTimeInterval(_updater.DeltaTime);
            BarrelAdjustmentResult adjustmentResult = _adjustmentHelper.AdjustTurretBarrel();
            bool wasFireSuccessful = _firingHelper.TryFire(adjustmentResult);

            if (!wasFireSuccessful)
            {
                CeaseFire();
            }
        }

        public abstract void Fire(float angleInDegrees);

        protected virtual void CeaseFire() { }

        public virtual void CleanUp()
        {
            Target = null;
            // Clear burst fire state
            _baseTurretStats.Initialise();

            if (_updater != null)
            {
                _updater.Updated -= _updater_Updated;
            }
        }

        public override string ToString()
        {
            return base.ToString() + $" Parent: {_parent}";
        }
    }
}
