using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Drones;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public interface ICruiser : ICruiserController, ITarget
	{
		BuildingWrapper SelectedBuildingPrefab { get; set; }
		IDroneManager DroneManager { get; }
		IDroneConsumerProvider DroneConsumerProvider { get; }
		Direction Direction { get; }
		Vector2 Size { get; }
		float YAdjustmentInM { get; }
		Sprite Sprite { get; }

        Building ConstructSelectedBuilding(ISlot slot);
    }
}
