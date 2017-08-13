using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
	public class AntiAirTurretsTestGod : MonoBehaviour
	{
		public UnitWrapper unitPrefab;
		public List<Vector2> patrolPoints;

		void Start()
		{
			Helper helper = new Helper();
   
            // Initialise prefab
			unitPrefab.Initialise();
            TestAircraftController aircraft = (TestAircraftController)unitPrefab.Buildable;
            aircraft.maxHealth = 30;
            aircraft.maxVelocityInMPerS = 8;
			aircraft.patrolPoints = patrolPoints;


            // Initialise air factory
            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuildable(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            factory.CompletedBuildable += Factory_CompletedBuildable;
            factory.StartConstruction();

            // Initialise turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuildable(turret, Faction.Reds);
                turret.StartConstruction();
            }
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
			((Factory)sender).UnitWrapper = unitPrefab;
		}
	}
}
