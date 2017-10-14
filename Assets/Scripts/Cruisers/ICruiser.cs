﻿using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Drones;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public interface ICruiser : ICruiserController, ITarget
    {
        IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        Direction Direction { get; }
        Vector2 Size { get; }
        float YAdjustmentInM { get; }
        Sprite Sprite { get; }
        IFogOfWar Fog { get; }
        IRepairManager RepairManager { get; }

        IBuilding ConstructSelectedBuilding(ISlot slot);
    }
}
