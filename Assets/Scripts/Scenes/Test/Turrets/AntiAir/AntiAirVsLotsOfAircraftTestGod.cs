using System;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiAir
{
    public class AntiAirVsLotsOfAircraftTestGod : MonoBehaviour
	{
		public UnitWrapper unitPrefab;
		public List<Vector2> patrolPoints;

		void Start()
		{
			Helper helper = new Helper();
   

            // FELIX  Use prefab factory?  Current way modifies prefab :/
            // Initialise prefab
			unitPrefab.Initialise();
            TestAircraftController aircraft = (TestAircraftController)unitPrefab.Buildable;
            aircraft.maxHealth = 30;
            aircraft.maxVelocityInMPerS = 8;
			aircraft.patrolPoints = patrolPoints;
            aircraft.StaticInitialise();


            // Initialise air factory
            AirFactory factory = FindObjectOfType<AirFactory>();
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            factory.CompletedBuildable += Factory_CompletedBuildable;
            factory.StartConstruction();


            // Initialise turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
                turret.StartConstruction();
            }
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
			((Factory)sender).UnitWrapper = unitPrefab;
		}
	}
}
