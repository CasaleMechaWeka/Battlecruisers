using BattleCruisers.Buildables.Buildings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    public class SlotAccessor : ISlotAccessor
    {
		private readonly IDictionary<SlotType, ReadOnlyCollection<ISlot>> _slots;
        private readonly ReadOnlyCollection<ISlot> _antiShipSlots;

        public SlotAccessor(IDictionary<SlotType, ReadOnlyCollection<ISlot>> slots)
		{
            Assert.IsNotNull(slots);

            _slots = slots;
            _antiShipSlots
                = slots[SlotType.Deck]
                    .Where(slot => slot.BuildingFunctionAffinity == BuildingFunction.AntiShip)
                    .ToList()
                    .AsReadOnly();
        }

		public bool IsSlotAvailable(SlotSpecification slotSpecification)
		{
			return 
                _slots[slotSpecification.SlotType]
                    .Any(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
		}

        public ReadOnlyCollection<ISlot> GetSlots(SlotSpecification slotSpecification)
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

        public IList<ISlot> GetFreeSlots(SlotType slotType)
        {
            Assert.IsTrue(_slots.ContainsKey(slotType));

            return
                _slots[slotType]
                    .Where(slot => slot.IsFree)
                    .ToList();
        }

        public ISlot GetFreeSlot(SlotSpecification slotSpecification)
		{
            return slotSpecification.PreferFromFront ?
                _slots[slotSpecification.SlotType].First(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction)) :
                _slots[slotSpecification.SlotType].Last(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
		}

        private bool FreeSlotFilter(ISlot slot, BuildingFunction desiredBuildingFunction)
        {
            return
                slot.IsFree
                && (desiredBuildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == desiredBuildingFunction);
        }
		
        public ISlot GetSlot(IBuilding building)
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
