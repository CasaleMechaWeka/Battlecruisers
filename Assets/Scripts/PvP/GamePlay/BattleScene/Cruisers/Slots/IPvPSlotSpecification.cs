using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots
{
    public interface IPvPSlotSpecification
    {
        BuildingFunction BuildingFunction { get; }
        bool PreferFromFront { get; }
        SlotType SlotType { get; }
    }
}

