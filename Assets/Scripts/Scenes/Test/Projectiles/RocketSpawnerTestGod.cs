using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using NSubstitute;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
    public class RocketSpawnerTestGod : MonoBehaviour 
	{
		private RocketSpawner _rocketSpawner;
		private IBuilding _target;
		private IExactMatchTargetFilter _targetFilter;

		public RocketController rocketPrefab;

		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			_target = FindObjectOfType<Building>();
            helper.InitialiseBuilding(_target, Faction.Blues);
			_target.StartConstruction();
			_target.Destroyed += (sender, e) => CancelInvoke("FireRocket");


			// Setup rocket spawner
			_rocketSpawner = FindObjectOfType<RocketSpawner>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

			ITarget parent = Substitute.For<ITarget>();
            parent.Faction.Returns(Faction.Reds);
            ICruisingProjectileStats rocketStats = GetComponent<CruisingProjectileStats>();
            int burstSize = 1;
            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);

            _rocketSpawner.Initialise(parent, rocketStats, burstSize, args.FactoryProvider);

			InvokeRepeating("FireRocket", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireRocket()
		{
			_rocketSpawner.SpawnRocket(angleInDegrees: 90, isSourceMirrored: false, target: _target, targetFilter: _targetFilter);
		}
	}
}
