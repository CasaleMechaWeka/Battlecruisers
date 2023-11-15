using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotInitialiser
    {
        IDictionary<PvPSlotType, ReadOnlyCollection<PvPSlot>> InitialiseSlots(IPvPCruiser parentCruiser, IList<PvPSlot> slots, IPvPBuildingPlacer buildingPlacer);
    }
}
