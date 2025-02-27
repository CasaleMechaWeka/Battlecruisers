using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlotAccessor : IPvPSlotAccessor
    {
        private readonly IDictionary<SlotType, ReadOnlyCollection<PvPSlot>> _slots;
        private readonly ReadOnlyCollection<PvPSlot> _antiShipSlots;

        public PvPSlotAccessor(IDictionary<SlotType, ReadOnlyCollection<PvPSlot>> slots)
        {
            Assert.IsNotNull(slots);

            _slots = slots;
            _antiShipSlots
                = slots[SlotType.Deck]
                    //.Where(slot => slot.BuildingFunctionAffinity == BuildingFunction.AntiShip)
                    .ToList()
                    .AsReadOnly();
        }

        public bool IsSlotAvailable(IPvPSlotSpecification slotSpecification)
        {
            return
                _slots[slotSpecification.SlotType]
                    .Any(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
        }

        public bool IsSlotAvailableForPlayer(IPvPSlotSpecification slotSpecification)
        {
            ReadOnlyCollection<PvPSlot> slots = GetSlots(slotSpecification);
            return slots.Any(slot => slot.IsFree);
        }

        public ReadOnlyCollection<PvPSlot> GetSlots(IPvPSlotSpecification slotSpecification)
        {
            Assert.IsNotNull(slotSpecification);
            Assert.IsTrue(_slots.ContainsKey(slotSpecification.SlotType));

            if (slotSpecification.BuildingFunction == BuildingFunction.AntiShip
                && slotSpecification.SlotType == SlotType.Deck)
            {
                return _antiShipSlots;
            }
            else
            {
                return _slots[slotSpecification.SlotType];
            }
        }

        public IList<PvPSlot> GetFreeSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));

            return
                _slots[slotType]
                    .Where(slot => slot.IsFree)
                    .ToList();
        }

        public IPvPSlot GetFreeSlot(IPvPSlotSpecification slotSpecification)
        {
            return slotSpecification.PreferFromFront ?
                _slots[slotSpecification.SlotType].First(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction)) :
                _slots[slotSpecification.SlotType].Last(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
        }

        private bool FreeSlotFilter(IPvPSlot slot, BuildingFunction desiredBuildingFunction)
        {
            return
                slot.IsFree
                && (desiredBuildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == desiredBuildingFunction);
        }

        public IPvPSlot GetSlot(IPvPBuilding building)
        {
            Assert.IsTrue(_slots.ContainsKey(building.SlotSpecification.SlotType));

            return
                _slots[building.SlotSpecification.SlotType]
                    .FirstOrDefault(slot => ReferenceEquals(slot.Building.Value, building));
        }

        public int GetSlotCount(SlotType slotType)
        {
            return _slots[slotType].Count;
        }
    }
}
