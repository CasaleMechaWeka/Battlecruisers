using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System;

namespace BattleCruisers.Scenes.Test.Turrets.AntiShip
{
    public class AntiShipTurretLotsOfShipsTestGod : TestGodBase 
	{
        public UnitWrapper unitPrefab;

		protected override void Start()
		{
            base.Start();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

            unitPrefab.StaticInitialise();

            // Factory
            Factory factory = FindObjectOfType<Factory>();
            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            helper.InitialiseBuilding(factory, Faction.Blues, parentCruiserDirection: Direction.Right, parentCruiser: blueCruiser);
            factory.CompletedBuildable += Factory_CompletedBuildable;
            factory.StartConstruction();
            Helper.SetupFactoryForUnitMonitor(factory, blueCruiser);

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
