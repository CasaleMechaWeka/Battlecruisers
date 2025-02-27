using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class PvPBarrelController : NetworkBehaviour, IBarrelController
    {
        private IPvPBarrelAdjustmentHelper _adjustmentHelper;
        private IPvPBarrelFiringHelper _firingHelper;
        private IUpdater _updater;
        protected IPvPParticleSystemGroup _muzzleFlash;
        protected IAnimation _barrelAnimation;
        private ITarget _parent;
        protected ITargetFilter _targetFilter;
        protected IFireIntervalManager _fireIntervalManager;

        protected IProjectileStats _projectileStats;
        public IProjectileStats ProjectileStats => _projectileStats;

        private PvPTurretStats _baseTurretStats;
        private ITurretStatsWrapper _turretStatsWrapper;
        public ITurretStats TurretStats => _turretStatsWrapper;

        public ITarget Target { get; set; }
        public ITarget CurrentTarget => Target;
        public bool IsSourceMirrored => transform.IsMirrored();
        protected virtual int NumOfBarrels => 1;
        public Transform Transform => transform;
        public float BarrelAngleInDegrees => Transform.rotation.eulerAngles.z;

        [SerializeField] private List<SpriteRenderer> manualRenderers = new List<SpriteRenderer>();
        public List<SpriteRenderer> Renderers { get; private set; }

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

        [SerializeField] protected bool disableAnimator = false;

        public virtual void StaticInitialise()
        {
            // Initialize the Renderers list
            Renderers = new List<SpriteRenderer>();

            // Add manually assigned renderers
            Renderers.AddRange(manualRenderers);

            // Add child SpriteRenderers
            Renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

            _projectileStats = GetProjectileStats();
            _baseTurretStats = SetupTurretStats();
            _turretStatsWrapper = new PvPTurretStatsWrapper(_baseTurretStats);
            _fireIntervalManager = SetupFireIntervalManager(TurretStats);

            IPvPParticleSystemGroupInitialiser muzzleFlashInitialiser = transform.FindNamedComponent<IPvPParticleSystemGroupInitialiser>("MuzzleFlash");
            _muzzleFlash = muzzleFlashInitialiser.CreateParticleSystemGroup();
        }

        protected virtual IProjectileStats GetProjectileStats()
        {
            PvPProjectileStats projectileStats = GetComponent<PvPProjectileStats>();
            Assert.IsNotNull(projectileStats);
            return projectileStats;
        }

        protected virtual PvPTurretStats SetupTurretStats()
        {
            PvPTurretStats turretStats = gameObject.GetComponent<PvPTurretStats>();
            Assert.IsNotNull(turretStats);
            turretStats.Initialise();
            return turretStats;
        }

        public virtual async void ApplyVariantStats(IPvPBuilding building)
        {
            int variantIndex = building.variantIndex;
            if (variantIndex != -1)
            {
                if (variantIndex != -1)
                {
                    VariantPrefab variant = await PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                    // turret stats
                    _baseTurretStats.ApplyVariantStats(variant.statVariant);
                    GetComponent<PvPProjectileStats>().ApplyVariantStats(variant.statVariant);
                }
            }
        }
        public virtual async void ApplyVariantStats(IPvPUnit unit)
        {
            int variantIndex = unit.variantIndex;
            if (variantIndex != -1)
            {
                VariantPrefab variant = await PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory.GetVariant(StaticPrefabKeys.Variants.GetVariantKey(variantIndex));
                // turret stats
                _baseTurretStats.ApplyVariantStats(variant.statVariant);
                GetComponent<PvPProjectileStats>().ApplyVariantStats(variant.statVariant);
            }
        }

        protected virtual IFireIntervalManager SetupFireIntervalManager(ITurretStats turretStats)
        {
            PvPFireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<PvPFireIntervalManagerInitialiser>();
            Assert.IsNotNull(fireIntervalManagerInitialiser);
            return fireIntervalManagerInitialiser.Initialise(turretStats);
        }

        protected virtual IDamageCapability FindDamageCapabilities()
        {
            float damagePerS = NumOfBarrels * _projectileStats.Damage * TurretStats.MeanFireRatePerS;
            return new PvPDamageCapability(damagePerS, TurretStats.AttackCapabilities);
        }


        // should be called by Server
        public async Task InitialiseAsync(IPvPBarrelControllerArgs args)
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
                = new PvPBarrelAdjustmentHelper(
                    this,
                    args.TargetPositionPredictor,
                    args.TargetPositionValidator,
                    args.AngleCalculator,
                    args.RotationMovementController,
                    args.AngleLimiter,
                    args.AttackablePositionFinder);

            _firingHelper
                = new PvPBarrelFiringHelper(
                    this,
                    args.AccuracyAdjuster,
                    _fireIntervalManager,
                    CreateFirer(args));

            await InternalInitialiseAsync(args);

            _updater = args.Updater;

            _updater.Updated += _updater_Updated;
        }


        // should be called by Client
        public async Task InitialiseAsync_PvPClient(IPvPBarrelControllerArgs args)
        {
            Assert.IsNotNull(args);
            _parent = args.Parent;
            _barrelAnimation = GetBarrelFiringAnimation(args);
        }

        protected virtual IPvPBarrelFirer CreateFirer(IPvPBarrelControllerArgs args)
        {
            return new PvPBarrelFirer(
                    this,
                    disableAnimator ? null : GetBarrelFiringAnimation(args),
                    _muzzleFlash);
        }

        protected virtual IAnimation GetBarrelFiringAnimation(IPvPBarrelControllerArgs args)
        {
            return args.BarrelFiringAnimation;
        }


#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected virtual async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args) { }
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (this != null)
            {
                _fireIntervalManager.ProcessTimeInterval(_updater.DeltaTime);
                PvPBarrelAdjustmentResult adjustmentResult = _adjustmentHelper.AdjustTurretBarrel();
                bool wasFireSuccessful = _firingHelper.TryFire(adjustmentResult);
                if (!wasFireSuccessful)
                {
                    CeaseFire();
                }
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
        public override void OnDestroy()
        {
            base.OnDestroy();
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

