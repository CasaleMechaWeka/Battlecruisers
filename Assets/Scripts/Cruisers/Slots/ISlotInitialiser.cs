using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotInitialiser
    {
        IDictionary<SlotType, ReadOnlyCollection<ISlot>> InitialiseSlots(ICruiser parentCruiser, IList<ISlot> slots, IBuildingPlacer buildingPlacer);
    }
}