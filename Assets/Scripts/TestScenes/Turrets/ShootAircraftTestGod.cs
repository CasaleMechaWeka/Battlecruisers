using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets;
using BattleCruisers.Targets.TargetFinders;
using BattleCruisers.Targets.TargetProcessors;
using BattleCruisers.TestScenes.Utilities;
using BattleCruisers.Units.Aircraft;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.TestScenes.Turrets
{
	public class ShootAircraftTestGod : MonoBehaviour 
	{
		public BomberController bomber;
		public DefensiveTurret turret;

		void Start() 
		{
			Helper helper = new Helper();

			// Set up turret
			helper.InitialiseBuildable(turret, faction: Faction.Reds);
			turret.StartConstruction();

			// The enemy cruiser is added as a target by the global target finder.
			// So pretend the cruiser game object is the turret.
			ICruiser enemyCruiser = Substitute.For<ICruiser>();
			enemyCruiser.GameObject.Returns(turret.GameObject);
			ITargetFinder targetFinder = new GlobalTargetFinder(enemyCruiser);
			ITargetProcessor targetProcessor = new TargetProcessor(targetFinder);
			ITargetsFactory targetsFactory = Substitute.For<ITargetsFactory>();
			targetsFactory.GlobalTargetProcessor.Returns(targetProcessor);

			// Set up bomber
			helper.InitialiseBuildable(bomber, faction: Faction.Blues, targetsFactory: targetsFactory);
			bomber.StartConstruction();
		}
	}
}
