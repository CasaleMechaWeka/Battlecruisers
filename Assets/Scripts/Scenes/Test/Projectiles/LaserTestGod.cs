using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test
{
	public class LaserTestGod : MonoBehaviour 
	{
		private Buildable _target;
		private LaserEmitter _laserEmitter;

		void Start () 
		{
			Faction enemyFaction = Faction.Blues;

			// Setup target
			Helper helper = new Helper();
			_target = GameObject.FindObjectOfType<Buildable>();
			helper.InitialiseBuildable(_target, enemyFaction);
			_target.StartConstruction();

			// Setup laser
			ITargetFilter targetFilter = new FactionAndTargetTypeFilter(enemyFaction, TargetType.Buildings, TargetType.Cruiser, TargetType.Ships);

			_laserEmitter = GameObject.FindObjectOfType<LaserEmitter>();
			// FELIX
//			_laserEmitter.Initialise(isMirrored: true);
			_laserEmitter.Initialise(targetFilter, isMirrored: false);
			_laserEmitter.StartLaser();
		}
	}
}
