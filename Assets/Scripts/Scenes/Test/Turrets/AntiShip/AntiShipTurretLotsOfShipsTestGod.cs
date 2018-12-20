using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretLotsOfShipsTestGod : MonoBehaviour 
	{
        public UnitWrapper unitPrefab;

		void Start()
		{
			Helper helper = new Helper();

            unitPrefab.Initialise();
            unitPrefab.Buildable.StaticInitialise();

            // Factory
            Factory factory = FindObjectOfType<Factory>();
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right);
            factory.CompletedBuildable += Factory_CompletedBuildable;
            factory.StartConstruction();

            // Turrets
            TurretController[] turrets = FindObjectsOfType<TurretController>();
            foreach (TurretController turret in turrets)
            {
                helper.InitialiseBuilding(turret, Faction.Reds);
			    turret.StartConstruction();
            }
		}

        private void Factory_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(unitPrefab);
        }
    }
}
