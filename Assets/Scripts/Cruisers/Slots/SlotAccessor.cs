using BattleCruisers.Buildables.Buildings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotAccessor
    {
        private readonly IDictionary<SlotType, ReadOnlyCollection<Slot>> _slots;
        private readonly ReadOnlyCollection<Slot> _antiShipSlots;

        public SlotAccessor(IDictionary<SlotType, ReadOnlyCollection<Slot>> slots)
        {
            Assert.IsNotNull(slots);

            _slots = slots;
            _antiShipSlots
                = slots[SlotType.Deck]
                    //.Where(slot => slot.BuildingFunctionAffinity == BuildingFunction.AntiShip)
                    .ToList()
                    .AsReadOnly();
        }

        public bool IsSlotAvailable(ISlotSpecification slotSpecification)
        {
            return
                _slots[slotSpecification.SlotType]
                    .Any(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
        }

        public bool IsSlotAvailableForPlayer(ISlotSpecification slotSpecification)
        {
            ReadOnlyCollection<Slot> slots = GetSlots(slotSpecification);
            return slots.Any(slot => slot.IsFree);
        }

        public ReadOnlyCollection<Slot> GetSlots(ISlotSpecification slotSpecification)
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

        public ReadOnlyCollection<Slot> GetAllSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));
            return _slots[slotType];
        }

        public IList<Slot> GetFreeSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));

            return
                _slots[slotType]
                    .Where(slot => slot.IsFree)
                    .ToList();
        }

        public Slot GetFreeSlot(ISlotSpecification slotSpecification)
        {
            return slotSpecification.PreferFromFront ?
                _slots[slotSpecification.SlotType].First(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction)) :
                _slots[slotSpecification.SlotType].Last(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
        }

        private bool FreeSlotFilter(Slot slot, BuildingFunction desiredBuildingFunction)
        {
            return
                slot.IsFree
                && (desiredBuildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == desiredBuildingFunction);
        }

        public Slot GetSlot(IBuilding building)
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

    public class CompositeSlotAccessor : SlotAccessor
    {
        private readonly SlotAccessor[] _accessors;

        public CompositeSlotAccessor(SlotAccessor[] accessors)
            : base(CreateCombinedSlots(accessors))
        {
            _accessors = accessors;
        }

        private static IDictionary<SlotType, ReadOnlyCollection<Slot>> CreateCombinedSlots(SlotAccessor[] accessors)
        {
            var combinedSlots = new Dictionary<SlotType, ReadOnlyCollection<Slot>>();

            foreach (SlotType slotType in System.Enum.GetValues(typeof(SlotType)))
            {
                var allSlotsOfType = new List<Slot>();
                foreach (var accessor in accessors)
                {
                    allSlotsOfType.AddRange(accessor.GetAllSlots(slotType));
                }
                combinedSlots[slotType] = allSlotsOfType.AsReadOnly();
            }

            return combinedSlots;
        }
    }
}
