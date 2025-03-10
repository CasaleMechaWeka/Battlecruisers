using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class EnemyBlockingSpawningTestGod : TestGodBase 
	{
		public Factory leftFactory, rightFactory;
		public UnitWrapper archonPrefab, attackBoatPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            return new List<GameObject>()
            {
                leftFactory.GameObject,
                rightFactory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			archonPrefab.StaticInitialise(helper.CommonStrings);
            attackBoatPrefab.StaticInitialise(helper.CommonStrings);

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
