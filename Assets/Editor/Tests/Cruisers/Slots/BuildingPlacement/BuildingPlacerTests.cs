using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacerTests
    {
        private IBuildingPlacer _placer;
        private IBuilding _building;
        private ISlot _slot;

        [SetUp]
        public void TestSetup()
        {
            _placer = new BuildingPlacer();

            _building = Substitute.For<IBuilding>();
            _building.PuzzleRootPoint.Returns(new Vector3(-0.45f, 3.2f, 0));
            _building.Position.Returns(new Vector2(0, 0));

            _slot = Substitute.For<ISlot>();
            _slot.Transform.Rotation.Returns(new Quaternion(1, 2, 3, 4));
            _slot.Transform.Right.Returns(new Vector3(1, 0, 0));
            _slot.Transform.Up.Returns(new Vector3(0, 1, 0));
            _slot.BuildingPlacementPoint.Returns(new Vector3(0.02f, -0.5f, 0));
        }

        [Test]
        public void PlaceBuilding_SlotFacingRight()
        {
            float verticalChange = _building.Position.y - _building.PuzzleRootPoint.y;
            float horizontalChange = _building.Position.x - _building.PuzzleRootPoint.x;

            Vector3 expectedPosition = _slot.BuildingPlacementPoint
                + (_slot.Transform.Up * verticalChange)
                + (_slot.Transform.Right * horizontalChange);

            _placer.PlaceBuilding(_building, _slot);

            _building.Received().Rotation = _slot.Transform.Rotation;

            _building.Received().Position = expectedPosition;
        }
    }
}