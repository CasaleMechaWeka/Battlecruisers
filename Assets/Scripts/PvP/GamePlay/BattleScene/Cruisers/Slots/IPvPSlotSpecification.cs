using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotSpecification
    {
        PvPBuildingFunction BuildingFunction { get; }
        bool PreferFromFront { get; }
        PvPSlotType SlotType { get; }
    }
}

