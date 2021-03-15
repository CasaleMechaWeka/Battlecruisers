using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Turrets
{
    public class SmartMissileSpawnerTestGod : SmartMissileTestGod
	{
		private ITargetFilter _targetFilter;

		public SmartMissileSpawner missileSpawner;

        protected override async Task InitialiseMissileAsync(Helper helper, ICruiser redCruiser)
        {
			Assert.IsNotNull(missileSpawner);
			_targetFilter = new FactionAndTargetTypeFilter(Faction.Reds, projectileStats.AttackCapabilities);

			ITarget parent = Substitute.For<ITarget>();
			int burstSize = 1;
			BuildableInitialisationArgs args = helper.CreateBuildableInitialisationArgs(enemyCruiser: redCruiser);
			IProjectileSpawnerArgs spawnerArgs = new ProjectileSpawnerArgs(parent, projectileStats, burstSize, args.FactoryProvider, args.CruiserSpecificFactories, args.EnemyCruiser);

			await missileSpawner.InitialiseAsync(spawnerArgs, SoundKeys.Firing.Missile, projectileStats);

			InvokeRepeating("FireMissile", time: 0.5f, repeatRate: 2);
		}

		private void FireMissile()
		{
			missileSpawner.SpawnMissile(angleInDegrees: 90, isSourceMirrored: false, _targetFilter);
		}
	}
}
