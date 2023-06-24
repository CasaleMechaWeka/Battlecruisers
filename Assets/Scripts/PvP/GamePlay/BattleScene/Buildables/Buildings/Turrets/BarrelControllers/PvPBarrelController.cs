using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public abstract class PvPBarrelController : NetworkBehaviour, IPvPBarrelController
    {
        private IPvPBarrelAdjustmentHelper _adjustmentHelper;
        private IPvPBarrelFiringHelper _firingHelper;
        private IPvPUpdater _updater;
        protected IPvPParticleSystemGroup _muzzleFlash;
        protected IPvPAnimation _barrelAnimation;
        private IPvPTarget _parent;
        protected IPvPTargetFilter _targetFilter;
        protected IPvPFireIntervalManager _fireIntervalManager;

        protected IPvPProjectileStats _projectileStats;
        public IPvPProjectileStats ProjectileStats => _projectileStats;

        private PvPTurretStats _baseTurretStats;
        private IPvPTurretStatsWrapper _turretStatsWrapper;
        public IPvPTurretStats pvpTurretStats => _turretStatsWrapper;

        public IPvPTarget Target { get; set; }
        public IPvPTarget CurrentTarget => Target;
        public bool IsSourceMirrored => transform.IsMirrored();
        protected virtual int NumOfBarrels => 1;
        public Transform Transform => transform;
        public float BarrelAngleInDegrees => Transform.rotation.eulerAngles.z;

        public SpriteRenderer[] Renderers { get; private set; }

        // Initialise lazily, because requires child class StaticInitialise()s to have completed.
        private IPvPDamageCapability _damageCapability;
        public IPvPDamageCapability DamageCapability
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
            _turretStatsWrapper = new PvPTurretStatsWrapper(_baseTurretStats);
            _fireIntervalManager = SetupFireIntervalManager(pvpTurretStats);

            IPvPParticleSystemGroupInitialiser muzzleFlashInitialiser = transform.FindNamedComponent<IPvPParticleSystemGroupInitialiser>("MuzzleFlash");
            _muzzleFlash = muzzleFlashInitialiser.CreateParticleSystemGroup();
        }

        protected virtual IPvPProjectileStats GetProjectileStats()
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

        protected virtual IPvPFireIntervalManager SetupFireIntervalManager(IPvPTurretStats turretStats)
        {
            PvPFireIntervalManagerInitialiser fireIntervalManagerInitialiser = gameObject.GetComponent<PvPFireIntervalManagerInitialiser>();
            Assert.IsNotNull(fireIntervalManagerInitialiser);
            return fireIntervalManagerInitialiser.Initialise(turretStats);
        }

        protected virtual IPvPDamageCapability FindDamageCapabilities()
        {
            float damagePerS = NumOfBarrels * _projectileStats.Damage * pvpTurretStats.MeanFireRatePerS;
            return new PvPDamageCapability(damagePerS, pvpTurretStats.AttackCapabilities);
        }


        // should be called by Server
        public async Task InitialiseAsync(IPvPBarrelControllerArgs args)
        {
            Assert.IsNotNull(args);

            _parent = args.Parent;
            _targetFilter = args.TargetFilter;
            _turretStatsWrapper.pvpTurretStats
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
            return
                new PvPBarrelFirer(
                    this,
                    GetBarrelFiringAnimation(args),
                    _muzzleFlash);
        }

        protected virtual IPvPAnimation GetBarrelFiringAnimation(IPvPBarrelControllerArgs args)
        {
            return args.BarrelFiringAnimation;
        }


#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected virtual async Task InternalInitialiseAsync(IPvPBarrelControllerArgs args) { }
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

        private void _updater_Updated(object sender, EventArgs e)
        {
            _fireIntervalManager.ProcessTimeInterval(_updater.DeltaTime);
            PvPBarrelAdjustmentResult adjustmentResult = _adjustmentHelper.AdjustTurretBarrel();
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

