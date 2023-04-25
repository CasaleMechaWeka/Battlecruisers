using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots.BuildingPlacement;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotInitialiser
    {
        IDictionary<PvPSlotType, ReadOnlyCollection<IPvPSlot>> InitialiseSlots(IPvPCruiser parentCruiser, IList<IPvPSlot> slots, IPvPBuildingPlacer buildingPlacer);
    }
}
