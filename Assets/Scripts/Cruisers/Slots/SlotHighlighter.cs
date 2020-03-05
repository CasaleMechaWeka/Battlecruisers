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
        private SlotSpecification _highlightedSlotSpec;

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
            if (_highlightedSlotSpec != null)
            {
                HighlightAvailableSlots(_highlightedSlotSpec);
            }
        }

        // Only highlight one slot type at a time
        public bool HighlightAvailableSlots(SlotSpecification slotSpecification)
		{
			UnhighlightSlots();

            bool wasAnySlotHighlighted = false;
			_highlightedSlotSpec = slotSpecification;

			foreach (ISlot slot in _slotAccessor.GetSlots(slotSpecification))
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
			if (_highlightedSlotSpec != null)
			{
				UnhighlightSlots(_highlightedSlotSpec);
				_highlightedSlotSpec = null;
			}

            HighlightedSlot = null;
		}

		private void UnhighlightSlots(SlotSpecification slotSpecification)
		{
			foreach (ISlot slot in _slotAccessor.GetSlots(slotSpecification))
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
