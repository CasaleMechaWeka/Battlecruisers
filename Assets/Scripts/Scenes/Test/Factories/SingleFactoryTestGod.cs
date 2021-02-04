using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class SingleFactoryTestGod : TestGodBase
	{
		public Factory factory;
		public UnitWrapper unitPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(factory, unitPrefab);

            return new List<GameObject>()
            {
                factory.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
			unitPrefab.StaticInitialise();

            ICruiser leftCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);

            helper.InitialiseBuilding(
                factory, 
                parentCruiserDirection: leftCruiser.Direction, 
                parentCruiser: leftCruiser);

			factory.CompletedBuildable += Factory_CompletedBuildable;
			factory.StartConstruction();

            Helper.SetupFactoryForUnitMonitor(factory, leftCruiser);
		}

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
            ((Factory)sender).StartBuildingUnit(unitPrefab);
		}
	}
}
