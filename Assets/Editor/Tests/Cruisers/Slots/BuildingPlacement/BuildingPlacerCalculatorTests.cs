using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using BattleCruisers.UI.BattleScene.ProgressBars;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacerCalculatorTests
    {
        private IBuildingPlacerCalculator _calculator;
        private IBuilding _building;
        private ISlot _slot;
        private IHealthBar _healthBar;

        [SetUp]
        public void TestSetup()
        {
            _calculator = new BuildingPlacerCalculator();

            _healthBar = Substitute.For<IHealthBar>();

            _building = Substitute.For<IBuilding>();
            _building.HealthBar.Returns(_healthBar);
            _building.PuzzleRootPoint.Returns(new Vector3(-0.45f, 3.2f, 0));
            _building.Position.Returns(new Vector2(0, 0));

            _slot = Substitute.For<ISlot>();
            _slot.Transform.Rotation.Returns(new Quaternion(1, 2, 3, 4));
            _slot.Transform.Right.Returns(new Vector3(1, 0, 0));
            _slot.Transform.Up.Returns(new Vector3(0, 1, 0));
            _slot.BuildingPlacementPoint.Returns(new Vector3(0.02f, -0.5f, 0));
        }

        [Test]
        public void FindBuildingRotation()
        {
            Assert.AreEqual(_slot.Transform.Rotation, _calculator.FindBuildingRotation(_slot));
        }

        [Test]
        public void FindSpawnPosition()
        {
            float verticalChange = _building.Position.y - _building.PuzzleRootPoint.y;
            float horizontalChange = _building.Position.x - _building.PuzzleRootPoint.x;

            Vector3 expectedPosition = _slot.BuildingPlacementPoint
                + (_slot.Transform.Up * verticalChange)
                + (_slot.Transform.Right * horizontalChange);

            Assert.AreEqual(expectedPosition, _calculator.FindSpawnPosition(_building, _slot));
        }

        [Test]
        public void FindHealthBarOffset_XOffset0()
        {
            _healthBar.Offset.Returns(new Vector2(0, 1));
            Assert.AreEqual(_healthBar.Offset, _calculator.FindHealthBarOffset(_building, _slot));
        }

        [Test]
        public void FindHealthBarOffset_XOffsetNot0_IsNotMirrored()
        {
            _healthBar.Offset.Returns(new Vector2(63, 1));
            _slot.Transform.IsMirroredAcrossYAxis.Returns(false);

            Assert.AreEqual(_healthBar.Offset, _calculator.FindHealthBarOffset(_building, _slot));
        }


        [Test]
        public void FindHealthBarOffset_XOffsetNot0_IsMirrored()
        {
            _healthBar.Offset.Returns(new Vector2(63, 1));
            _slot.Transform.IsMirroredAcrossYAxis.Returns(true);

            Vector2 expectedOffset = new Vector2(-63, 1);

            Assert.AreEqual(expectedOffset, _calculator.FindHealthBarOffset(_building, _slot));
        }
    }
}