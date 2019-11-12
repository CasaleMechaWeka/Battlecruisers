using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Slots
{
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

        public SlotHighlighter(
            ISlotAccessor slotAccessor, 
            ISlotFilter highlightableFilter, 
            ICruiserBuildingMonitor parentCruiserBuildingMonitor)
		{
            Helper.AssertIsNotNull(slotAccessor, highlightableFilter, parentCruiserBuildingMonitor);

            _slotAccessor = slotAccessor;
            _highlightableFilter = highlightableFilter;

            parentCruiserBuildingMonitor.BuildingDestroyed += ParentCruiser_BuildingDestroyed;
        }

        private void ParentCruiser_BuildingDestroyed(object sender, BuildingDestroyedEventArgs e)
        {
            if (_highlightedSlotType != null)
            {
                HighlightAvailableSlots((SlotType)_highlightedSlotType);
            }
        }

        // Only highlight one slot type at a time
        public bool HighlightAvailableSlots(SlotType slotType)
		{
			UnhighlightSlots();

            bool wasAnySlotHighlighted = false;
			_highlightedSlotType = slotType;

			foreach (ISlot slot in _slotAccessor.GetSlots(slotType))
			{
                if (_highlightableFilter.IsMatch(slot))
				{
                    slot.IsVisible = true;
                    wasAnySlotHighlighted = true;
				}
			}

            return wasAnySlotHighlighted;
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

        public void HighlightBuildingSlot(IBuilding building)
        {
            HighlightedSlot = _slotAccessor.GetSlot(building);
            Assert.IsNotNull(HighlightedSlot);
        }
    }
}
