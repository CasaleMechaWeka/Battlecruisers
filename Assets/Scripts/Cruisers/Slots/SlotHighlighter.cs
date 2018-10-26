using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;
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

        private bool FreeSlotFilter(ISlot slot, BuildingFunction buildingFunction)
        {
            return
                slot.IsFree
                && (buildingFunction == BuildingFunction.Generic
                    || slot.BuildingFunctionAffinity == buildingFunction);
        }

        public void HighlightBuildingSlot(IBuilding building)
        {
            HighlightedSlot = _slotAccessor.GetSlot(building);
            Assert.IsNotNull(HighlightedSlot);
        }
    }
}
