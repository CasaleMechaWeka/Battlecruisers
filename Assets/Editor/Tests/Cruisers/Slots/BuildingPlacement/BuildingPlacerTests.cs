using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace BattleCruisers.Tests.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacerTests
    {
        private IBuildingPlacer _placer;
        private IBuilding _building;
        private ISlot _slot;
        private Transform _slotTransform;

        [SetUp]
        public void TestSetup()
        {
            _placer = new BuildingPlacer();

            _building = Substitute.For<IBuilding>();
            _building.Size.Returns(new Vector2(4, 2));

            _slot = Substitute.For<ISlot>();
            _slotTransform = new GameObject().transform;
            _slot.Transform.Returns(_slotTransform);
            _slot.BuildingPlacementPoint.Returns(new Vector3(0, -0.5f, 0));
        }

        [Test]
        public void PlaceBuilding_SlotFacingRight()
        {
            _slot.Direction.Returns(Direction.Right);

            _placer.PlaceBuilding(_building, _slot);

            _building.Received().Rotation = _slotTransform.rotation;

            float horizontalChange = _building.Size.x / 2;
            Vector3 expectedPosition = _slot.BuildingPlacementPoint + (_slot.Transform.right * horizontalChange);
            _building.Received().Position = expectedPosition;
        }

        [Test]
        public void PlaceBuilding_SlotFacingUp()
        {
            _slot.Direction.Returns(Direction.Up);

            _placer.PlaceBuilding(_building, _slot);

            _building.Received().Rotation = _slotTransform.rotation;

            float verticalChange = _building.Size.y / 2;
            Vector3 expectedPosition = _slot.BuildingPlacementPoint + (_slot.Transform.up * verticalChange);
            _building.Received().Position = expectedPosition;
        }

        [Test]
        public void PlaceBuilding_SlotFacingInvalidDirection_Throws()
        {
            _slot.Direction.Returns(Direction.Down);

            Assert.Throws<ArgumentException>(() => _placer.PlaceBuilding(_building, _slot));
        }
    }
}