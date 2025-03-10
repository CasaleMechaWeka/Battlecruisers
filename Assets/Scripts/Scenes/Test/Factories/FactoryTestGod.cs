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
    public class FactoryTestGod : TestGodBase
    {
        public Factory factoryFacingRight, factoryFacingLeft;
        public UnitWrapper facingRightUnit, facingLeftUnit;

        protected virtual Faction FactoryFacingLeftFaction => Faction.Blues;
        protected virtual Faction FactoryFacingRightFaction => Faction.Blues;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(factoryFacingLeft, factoryFacingRight, facingLeftUnit, facingRightUnit);

            return new List<GameObject>()
            {
                factoryFacingRight.GameObject,
                factoryFacingLeft.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            facingRightUnit.StaticInitialise(helper.CommonStrings);
            facingLeftUnit.StaticInitialise(helper.CommonStrings);

            ICruiser leftCruiser = helper.CreateCruiser(Direction.Right, FactoryFacingRightFaction);
            ICruiser rightCruiser = helper.CreateCruiser(Direction.Left, FactoryFacingLeftFaction);

            helper.InitialiseBuilding(
                factoryFacingRight,
                FactoryFacingRightFaction,
                parentCruiserDirection: leftCruiser.Direction,
                parentCruiser: leftCruiser,
                enemyCruiser: rightCruiser);

            helper.InitialiseBuilding(
                factoryFacingLeft,
                FactoryFacingLeftFaction,
                parentCruiserDirection: rightCruiser.Direction,
                parentCruiser: rightCruiser,
                enemyCruiser: leftCruiser);

            factoryFacingRight.CompletedBuildable += FactoryFacingRight_CompletedBuildable;
            factoryFacingLeft.CompletedBuildable += FactoryFacingLeft_CompletedBuildable; ;

            factoryFacingRight.StartConstruction();
            factoryFacingLeft.StartConstruction();

            Helper.SetupFactoryForUnitMonitor(factoryFacingRight, leftCruiser);
            Helper.SetupFactoryForUnitMonitor(factoryFacingLeft, rightCruiser);

            OnStart();
        }

        private void FactoryFacingLeft_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(facingLeftUnit);
        }

        private void FactoryFacingRight_CompletedBuildable(object sender, EventArgs e)
        {
            ((Factory)sender).StartBuildingUnit(facingRightUnit);
        }

        protected virtual void OnStart() { }
    }
}