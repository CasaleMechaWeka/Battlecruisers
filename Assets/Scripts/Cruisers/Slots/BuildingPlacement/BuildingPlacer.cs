using BattleCruisers.Buildables.Buildings;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots.BuildingPlacement
{
    public class BuildingPlacer : IBuildingPlacer
    {
        private readonly IBuildingPlacerCalculator _calculator;

        public BuildingPlacer(IBuildingPlacerCalculator calculator)
        {
            Assert.IsNotNull(calculator);
            _calculator = calculator;
        }

        public void PlaceBuilding(IBuilding buildingToPlace, ISlot parentSlot)
        {
            buildingToPlace.Rotation = _calculator.FindBuildingRotation(parentSlot);
            buildingToPlace.Position = _calculator.FindSpawnPosition(buildingToPlace, parentSlot);
            buildingToPlace.HealthBar.Offset = _calculator.FindHealthBarOffset(buildingToPlace, parentSlot);
        }
    }
}