using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Buildables.Units.Aircraft;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class RocketSpawnerTestGod : MonoBehaviour 
	{
		private RocketSpawner _rocketSpawner;
		private Buildable _target;
		private IExactMatchTargetFilter _targetFilter;

		public RocketController rocketPrefab;

		void Start()
		{
			// Setup target
			Helper helper = new Helper();
			_target = GameObject.FindObjectOfType<Buildable>();
			helper.InitialiseBuildable(_target, Faction.Blues);
			_target.StartConstruction();
			_target.Destroyed += (sender, e) => CancelInvoke("FireRocket");


			// Setup rocket spawner
			_rocketSpawner = GameObject.FindObjectOfType<RocketSpawner>();
			_targetFilter = new ExactMatchTargetFilter() 
			{
				Target = _target
			};

			RocketStats rocketStats = new RocketStats(rocketPrefab: rocketPrefab, damage: 50, maxVelocityInMPerS: 10, cruisingAltitudeInM: 25);
			_rocketSpawner.Initialise(rocketStats, new MovementControllerFactory());

			InvokeRepeating("FireRocket", time: 0.5f, repeatRate: 0.5f);
		}

		private void FireRocket()
		{
			_rocketSpawner.SpawnRocket(angleInDegrees: 90, isSourceMirrored: false, target: _target, targetFilter: _targetFilter, faction: Faction.Reds);
		}
	}
}
