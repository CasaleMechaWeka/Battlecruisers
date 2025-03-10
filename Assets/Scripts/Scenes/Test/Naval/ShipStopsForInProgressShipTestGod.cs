using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using System.Collections.Generic;
using UnityEngine;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Factories
{
    public class ShipStopsForInProgressShipTestGod : TestGodBase
    {
        public NavalFactory leftFactory, rightFactory;
        public Building targetOnRight;
        public UnitWrapper leftBoatPrefab, rightBoatPrefab;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(leftFactory, rightFactory, targetOnRight, leftBoatPrefab, rightBoatPrefab);

            return new List<GameObject>
            {
                leftFactory.GameObject,
                rightFactory.GameObject,
                targetOnRight.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            ICruiser blueCruiser = helper.CreateCruiser(Direction.Right, Faction.Blues);
            ICruiser redCruiser = helper.CreateCruiser(Direction.Left, Faction.Reds);

            // Setup target
            helper.InitialiseBuilding(targetOnRight, Faction.Reds);
            targetOnRight.StartConstruction();
            IUserChosenTargetManager userChosenTargetManager = new UserChosenTargetManager();
            userChosenTargetManager.Target = targetOnRight;

            // Setup left factory
            helper
                .InitialiseBuilding(
                    leftFactory,
                    Faction.Blues,
                    parentCruiserDirection: blueCruiser.Direction,
                    parentCruiser: blueCruiser,
                    enemyCruiser: redCruiser,
                    userChosenTargetManager: userChosenTargetManager);

            Helper.SetupFactoryForUnitMonitor(leftFactory, blueCruiser);
            leftFactory.CompletedBuildable += (sender, e) => leftFactory.StartBuildingUnit(leftBoatPrefab);
            leftFactory.StartConstruction();

            // Setup right factory
            helper
                .InitialiseBuilding(
                    rightFactory,
                    Faction.Reds,
                    parentCruiserDirection: redCruiser.Direction,
                    parentCruiser: redCruiser,
                    enemyCruiser: blueCruiser);

            Helper.SetupFactoryForUnitMonitor(rightFactory, redCruiser);
            rightFactory.CompletedBuildable += (sender, e) => rightFactory.StartBuildingUnit(rightBoatPrefab);
            rightFactory.StartConstruction();
        }
    }
}