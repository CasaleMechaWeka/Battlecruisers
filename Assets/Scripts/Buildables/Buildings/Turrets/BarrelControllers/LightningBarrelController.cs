using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Projectiles.Spawners.Beams.Lightning;
using BattleCruisers.Utils;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class LightningBarrelController : BarrelController
	{
        [SerializeField]
        private TurretStats _turretStats;
        [SerializeField]
        private LightningEmitter _lightningEmitter;

        public override Vector3 ProjectileSpawnerPosition => _lightningEmitter.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
		{
            base.StaticInitialise();
            Helper.AssertIsNotNull(_turretStats, _lightningEmitter);
        }

        protected override TurretStats SetupTurretStats()
        {
            Assert.IsNotNull(_turretStats);
            _turretStats.Initialise();
            return _turretStats;
        }

#pragma warning disable 1998  // This async method lacks 'await' operators and will run synchronously
        protected override async Task InternalInitialiseAsync(IBarrelControllerArgs args)
        {
            _lightningEmitter
                .Initialise(
                    args.TargetFilter, 
                    _projectileStats.Damage, 
                    args.Parent,
                    args.FactoryProvider.SettingsManager);
        }
#pragma warning restore 1998  // This async method lacks 'await' operators and will run synchronously

        public override void Fire(float angleInDegrees)
		{
			_lightningEmitter.FireBeam(angleInDegrees, transform.IsMirrored());
		}

        public override void CleanUp()
        {
            base.CleanUp();
            _lightningEmitter.DisposeManagedState();
        }
    }
}

