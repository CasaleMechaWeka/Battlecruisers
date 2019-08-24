using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using System;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class EnemyBlockingSpawningTestGod : TestGodBase 
	{
		public Factory leftFactory, rightFactory;
		public UnitWrapper archonPrefab, attackBoatPrefab;

		protected override void Start()
		{
            base.Start();

			archonPrefab.Initialise();
            attackBoatPrefab.Initialise();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

            // Factory building archon
            helper.InitialiseBuilding(leftFactory, Faction.Reds, parentCruiserDirection: Direction.Right);
			leftFactory.CompletedBuildable += LeftFactory_CompletedBuildable;
			leftFactory.StartConstruction();

            // Factory building attack boats
            helper.InitialiseBuilding(rightFactory, Faction.Blues, parentCruiserDirection: Direction.Left);
			rightFactory.CompletedBuildable += RightFactory_CompletedBuildable;
			rightFactory.StartConstruction();
		}

		private void LeftFactory_CompletedBuildable(object sender, EventArgs e)
        {
            Invoke("StartBuildingArchon", 3);
        }

        private void StartBuildingArchon()
        {
            leftFactory.StartBuildingUnit(archonPrefab);
        }

		private void RightFactory_CompletedBuildable(object sender, EventArgs e)
        {
            rightFactory.StartBuildingUnit(attackBoatPrefab);
		}
	}
}
