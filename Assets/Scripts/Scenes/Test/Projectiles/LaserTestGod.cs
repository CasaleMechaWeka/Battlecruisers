using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Spawners;
using BattleCruisers.Scenes.Test.Utilities;
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
			// Setup target
			Helper helper = new Helper();
			_target = GameObject.FindObjectOfType<Buildable>();
			helper.InitialiseBuildable(_target, Faction.Blues);
			_target.StartConstruction();

			// Setup laser
			_laserEmitter = GameObject.FindObjectOfType<LaserEmitter>();
//			_laserEmitter.Initialise(isMirrored: true);
			_laserEmitter.Initialise(isMirrored: false);
			_laserEmitter.StartLaser();
		}
	}
}
