using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class MissileSpawnerTestGod : TestGodBase
	{
		private MissileSpawner _missileSpawner;
		private TestAircraftController _target;
		private IExactMatchTargetFilter _targetFilter;

		public List<Vector2> targetPatrolPoints;

        protected override List<GameObject> GetGameObjects()
        {
			_target = FindObjectOfType<TestAircraftController>();

            return new List<GameObject>()
            {
                _target.GameObject
            };
        }

        protected override async Task SetupAsync(Helper helper)
        {
			// Setup target
			_target.PatrolPoints = targetPatrolPoints;
            helper.InitialiseUnit(_target);
            _target.StartConstruction();


			// Setup missile spawner
			_missileSpawner = FindObjectOfType<MissileSpawner>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

			ITarget parent = Substitute.For<ITarget>();
            ProjectileStats stats = GetComponent<ProjectileStats>();
            int burstSize = 1;
			BuildableInitialisationArgs args = helper.CreateBuildableInitialisationArgs();
            IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(parent, stats, burstSize, args.FactoryProvider, args.CruiserSpecificFactories, args.EnemyCruiser);

            await _missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile);

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireMissile()
		{
			_missileSpawner.SpawnMissile(angleInDegrees: 45, isSourceMirrored: false, target: _target, targetFilter: _targetFilter);
		}
	}
}
