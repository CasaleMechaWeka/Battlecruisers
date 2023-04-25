using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement
{
    public class PvPBuildingPlacer : IPvPBuildingPlacer
    {
        private readonly IPvPBuildingPlacerCalculator _calculator;

        public PvPBuildingPlacer(IPvPBuildingPlacerCalculator calculator)
        {
            Assert.IsNotNull(calculator);
            _calculator = calculator;
        }

        public void PlaceBuilding(IPvPBuilding buildingToPlace, IPvPSlot parentSlot)
        {
            buildingToPlace.Rotation = _calculator.FindBuildingRotation(parentSlot);
            buildingToPlace.Position = _calculator.FindSpawnPosition(buildingToPlace, parentSlot);
            buildingToPlace.HealthBar.Offset = _calculator.FindHealthBarOffset(buildingToPlace, parentSlot);
        }
    }
}
