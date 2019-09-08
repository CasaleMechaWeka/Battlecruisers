using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.UI.BattleScene.ProgressBars;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacerTests
    {
        private IBuildingPlacer _placer;
        private IBuildingPlacerCalculator _calculator;
        private IBuilding _building;
        private ISlot _slot;
        private IHealthBar _healthBar;

        [SetUp]
        public void TestSetup()
        {
            _calculator = Substitute.For<IBuildingPlacerCalculator>();
            _placer = new BuildingPlacer(_calculator);

            _healthBar = Substitute.For<IHealthBar>();
            _building = Substitute.For<IBuilding>();
            _building.HealthBar.Returns(_healthBar);
            _slot = Substitute.For<ISlot>();
        }

        [Test]
        public void PlaceBuilding()
        {
            Quaternion expectedRotation = new Quaternion(4, 3, 2, 1);
            _calculator.FindBuildingRotation(_slot).Returns(expectedRotation);

            Vector3 expectedSpawnPosition = new Vector3(1, 2, 3);
            _calculator.FindSpawnPosition(_building, _slot).Returns(expectedSpawnPosition);

            Vector3 expectedHealthBarOffset = new Vector3(4, 5, 6);
            _calculator.FindHealthBarOffset(_building, _slot).Returns(expectedHealthBarOffset);

            _placer.PlaceBuilding(_building, _slot);

            _building.Received().Rotation = expectedRotation;
            _building.Received().Position = expectedSpawnPosition;
            _healthBar.Received().Offset = expectedHealthBarOffset;
        }
    }
}