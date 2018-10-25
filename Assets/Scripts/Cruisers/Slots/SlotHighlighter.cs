using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX   Copy tests from SlotWrapper :)
    public class SlotHighlighter : ISlotHighlighter
    {
        private readonly ISlotAccessor _slotAccessor;
        private readonly ISlotFilter _highlightableFilter;
		private SlotType? _highlightedSlotType;

		private ISlot _highlightedSlot;
        private ISlot HighlightedSlot
        {
            get { return _highlightedSlot; }
            set
            {
                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisible = false;
                }

                _highlightedSlot = value;

                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisible = true;
                }
            }
        }

        public SlotHighlighter(ISlotAccessor slotAccessor, ISlotFilter highlightableFilter)
		{
            Helper.AssertIsNotNull(slotAccessor, highlightableFilter);

            _slotAccessor = slotAccessor;
            _highlightableFilter = highlightableFilter;
        }

		// Only highlight one slot type at a time
		public void HighlightAvailableSlots(SlotType slotType)
		{
			if (_highlightedSlotType != slotType)
			{
				UnhighlightSlots();

				_highlightedSlotType = slotType;

				foreach (ISlot slot in _slotAccessor.GetSlots(slotType))
				{
                    if (_highlightableFilter.IsMatch(slot))
					{
                        slot.IsVisible = true;
					}
				}
			}
		}

		public void UnhighlightSlots()
		{
			if (_highlightedSlotType != null)
			{
				UnhighlightSlots((SlotType)_highlightedSlotType);
				_highlightedSlotType = null;
			}

            HighlightedSlot = null;
		}

		private void UnhighlightSlots(SlotType slotType)
		{
			foreach (ISlot slot in _slotAccessor.GetSlots(slotType))
			{
                slot.IsVisible = false;
			}
		}

		public ISlot GetFreeSlot(SlotSpecification slotSpecification)
		{
            return slotSpecification.PreferFromFront ?
                _slotAccessor.GetSlots(slotSpecification.SlotType).First(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction)) :
                _slotAccessor.GetSlots(slotSpecification.SlotType).Last(slot => FreeSlotFilter(slot, slotSpecification.BuildingFunction));
		}

        private bool FreeSlotFilter(ISlot slot, BuildingFunction buildingFunction)
        {
            return
                slot.IsFree
                && (buildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == buildingFunction);
        }

		public int GetSlotCount(SlotType slotType)
		{
			return _slotAccessor.GetSlots(slotType).Count;
		}
		
        public void HighlightBuildingSlot(IBuilding building)
        {
            HighlightedSlot = GetSlot(building);
            Assert.IsNotNull(HighlightedSlot);
        }
        
        private ISlot GetSlot(IBuilding building)
        {
            return
                _slotAccessor.GetSlots(building.SlotSpecification.SlotType)
                    .FirstOrDefault(slot => ReferenceEquals(slot.Building, building));
        }

        public ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType)
        {
            List<ISlot> freeSlots
                = _slotAccessor.GetSlots(slotType)
                    .Where(slot => slot.IsFree)
                    .ToList();

            return freeSlots.AsReadOnly();
        }
    }
}
