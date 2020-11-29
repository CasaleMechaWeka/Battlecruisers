using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers
{
    public class SamSiteBarrelController : BarrelController
	{
		private IExactMatchTargetFilter _exactMatchTargetFilter;
		private MissileSpawner _missileSpawner;

        public override Vector3 ProjectileSpawnerPosition => _missileSpawner.transform.position;
        public override bool CanFireWithoutTarget => false;

        public override void StaticInitialise()
        {
            base.StaticInitialise();

            _missileSpawner = gameObject.GetComponentInChildren<MissileSpawner>();
            Assert.IsNotNull(_missileSpawner);        
        }

		public async Task InitialiseAsync(IExactMatchTargetFilter targetFilter, IBarrelControllerArgs args)
		{
            await base.InitialiseAsync(args);

			_exactMatchTargetFilter = targetFilter;
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args.Parent, _projectileStats, TurretStats.BurstSize, args.FactoryProvider);

            await _missileSpawner.InitialiseAsync(spawnerArgs, args.SpawnerSoundKey);
		}

        public override void Fire(float angleInDegrees)
		{
			_exactMatchTargetFilter.Target = Target;
			_missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, _exactMatchTargetFilter);
		}
	}
}
