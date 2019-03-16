using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Targets.TargetFinders.Filters;
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

		public void Initialise(IExactMatchTargetFilter targetFilter, IBarrelControllerArgs args)
		{
            base.Initialise(args);

			_exactMatchTargetFilter = targetFilter;
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(args.Parent, _projectileStats, TurretStats.BurstSize, args.FactoryProvider);

            _missileSpawner.Initialise(spawnerArgs);
		}

        public override void Fire(float angleInDegrees)
		{
			_exactMatchTargetFilter.Target = Target;
			_missileSpawner.SpawnMissile(angleInDegrees, IsSourceMirrored, Target, _exactMatchTargetFilter);
		}
	}
}
