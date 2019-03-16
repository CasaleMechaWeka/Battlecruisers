using System;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class FactoryTestGod : MonoBehaviour 
	{
		public Factory factoryFacingRight, factoryFacingLeft;
		public UnitWrapper unitPrefab;

		protected virtual Faction FactoryFacingLeftFaction => Faction.Blues;
		protected virtual Faction FactoryFacingRightFaction => Faction.Blues;

		void Start()
		{
			unitPrefab.Initialise();

			Helper helper = new Helper();

            helper.InitialiseBuilding(factoryFacingRight, FactoryFacingRightFaction, parentCruiserDirection: Direction.Right);
            helper.InitialiseBuilding(factoryFacingLeft, FactoryFacingLeftFaction, parentCruiserDirection: Direction.Left);

			factoryFacingRight.CompletedBuildable += Factory_CompletedBuildable;
			factoryFacingLeft.CompletedBuildable += Factory_CompletedBuildable;

			factoryFacingRight.StartConstruction();
			factoryFacingLeft.StartConstruction();

			OnStart();
		}

		protected virtual void OnStart() { }

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
            ((Factory)sender).StartBuildingUnit(unitPrefab);
		}
	}
}
