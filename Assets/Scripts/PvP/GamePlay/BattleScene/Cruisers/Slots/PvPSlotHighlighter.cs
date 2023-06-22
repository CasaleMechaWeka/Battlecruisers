using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPSlotHighlighter : IPvPSlotHighlighter
    {
        private readonly IPvPSlotAccessor _slotAccessor;
        private readonly IPvPSlotFilter _highlightableFilter;
        private IPvPSlotSpecification _highlightedSlotSpec;
        public bool isHighlighting = false;
        private IPvPSlot _highlightedSlot;
        private IPvPSlot HighlightedSlot
        {
            get { return _highlightedSlot; }
            set
            {
                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisibleRederer = false;
                }

                _highlightedSlot = value;

                if (_highlightedSlot != null)
                {
                    _highlightedSlot.IsVisibleRederer = true;
                }
            }
        }

        public PvPSlotHighlighter(
            IPvPSlotAccessor slotAccessor,
            IPvPSlotFilter highlightableFilter,
            IPvPCruiserBuildingMonitor parentCruiserBuildingMonitor)
        {
            Helper.AssertIsNotNull(slotAccessor, highlightableFilter, parentCruiserBuildingMonitor);

            _slotAccessor = slotAccessor;
            _highlightableFilter = highlightableFilter;

            parentCruiserBuildingMonitor.BuildingDestroyed += ParentCruiser_BuildingDestroyed;
        }

        private void ParentCruiser_BuildingDestroyed(object sender, PvPBuildingDestroyedEventArgs e)
        {
            Logging.LogMethod(Tags.SLOTS);

            if (_highlightedSlotSpec != null
                && _highlightedSlotSpec.SlotType == e.DestroyedBuilding.SlotSpecification.SlotType)
            {
                HighlightAvailableSlotsCurrent();
            }
        }

        // Only highlight one slot type at a time
        public bool HighlightAvailableSlots(IPvPSlotSpecification slotSpecification)
        {
            Logging.LogMethod(Tags.SLOTS);

            UnhighlightSlots();

            bool wasAnySlotHighlighted = false;
            _highlightedSlotSpec = slotSpecification;

            foreach (IPvPSlot slot in _slotAccessor.GetSlots(slotSpecification))
            {
                if (_highlightableFilter.IsMatch(slot))
                {
                    slot.stopBuildingPlacementFeedback();
                    slot.IsVisibleRederer = true;
                    wasAnySlotHighlighted = true;
                }
            }

            return wasAnySlotHighlighted;
        }

        public void HighlightAvailableSlotsCurrent()
        {
            if (_highlightedSlotSpec != null)
            {
                HighlightAvailableSlots(_highlightedSlotSpec);
            }
        }

        public void HighlightSlots(IPvPSlotSpecification slotSpecification)
        {
            Logging.LogMethod(Tags.SLOTS);

            UnhighlightSlots();

            _highlightedSlotSpec = slotSpecification;

            foreach (IPvPSlot slot in _slotAccessor.GetSlots(slotSpecification))
            {
                slot.IsVisibleRederer = true;
            }
        }

        public void UnhighlightSlots()
        {
            Logging.LogMethod(Tags.SLOTS);

            if (_highlightedSlotSpec != null)
            {
                UnhighlightSlots(_highlightedSlotSpec);
                _highlightedSlotSpec = null;
            }

            HighlightedSlot = null;
        }

        private void UnhighlightSlots(IPvPSlotSpecification slotSpecification)
        {
            foreach (IPvPSlot slot in _slotAccessor.GetSlots(slotSpecification))
            {
                slot.IsVisibleRederer = false;
            }
        }

        public void HighlightBuildingSlot(IPvPBuilding building)
        {
            Logging.LogMethod(Tags.SLOTS);

            HighlightedSlot = _slotAccessor.GetSlot(building);
            Assert.IsNotNull(HighlightedSlot);
        }

        public void refresh()
        {
            HighlightAvailableSlotsCurrent();
        }
    }
}
