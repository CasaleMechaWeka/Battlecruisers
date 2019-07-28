using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using System;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class FactoryTestGod : TestGodBase
	{
		public Factory factoryFacingRight, factoryFacingLeft;
		public UnitWrapper unitPrefab;

		protected virtual Faction FactoryFacingLeftFaction => Faction.Blues;
		protected virtual Faction FactoryFacingRightFaction => Faction.Blues;

		protected override void Start()
		{
            base.Start();

			unitPrefab.Initialise();

			Helper helper = new Helper(updaterProvider: _updaterProvider);

            ICruiser leftCruiser = helper.CreateCruiser(Direction.Right, FactoryFacingRightFaction);
            ICruiser rightCruiser = helper.CreateCruiser(Direction.Left, FactoryFacingLeftFaction);

            helper.InitialiseBuilding(factoryFacingRight, FactoryFacingRightFaction, parentCruiserDirection: leftCruiser.Direction, enemyCruiser: rightCruiser);
            helper.InitialiseBuilding(factoryFacingLeft, FactoryFacingLeftFaction, parentCruiserDirection: rightCruiser.Direction, enemyCruiser: leftCruiser);

			factoryFacingRight.CompletedBuildable += Factory_CompletedBuildable;
			factoryFacingLeft.CompletedBuildable += Factory_CompletedBuildable;

			factoryFacingRight.StartConstruction();
			factoryFacingLeft.StartConstruction();

            Helper.SetupFactoryForUnitMonitor(factoryFacingRight, rightCruiser);
            Helper.SetupFactoryForUnitMonitor(factoryFacingLeft, leftCruiser);

            OnStart();
		}

		protected virtual void OnStart() { }

		private void Factory_CompletedBuildable(object sender, EventArgs e)
		{
            ((Factory)sender).StartBuildingUnit(unitPrefab);
		}
	}
}
