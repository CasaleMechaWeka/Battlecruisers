using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class SlotHighlighterTests
    {
        private ISlotHighlighter _slotHighlighter;
        private ISlotAccessor _slotAccessor;
        private ISlotFilter _highlightableFilter;
        private ISlot _slot1, _slot2;
        private ReadOnlyCollection<ISlot> _slotsToReturn1, _slotsToReturn2;
        private IList<ISlot> _mutableSlotsToReturn1, _mutableSlotsToReturn2;
        private IBuilding _building;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _slotAccessor = Substitute.For<ISlotAccessor>();
            _highlightableFilter = Substitute.For<ISlotFilter>();

            // FELIX Fix test :P
            _slotHighlighter = new SlotHighlighter(_slotAccessor, _highlightableFilter, null);

            _slot1 = Substitute.For<ISlot>();
            _slot1.Type.Returns(SlotType.Deck);

            _slot2 = Substitute.For<ISlot>();
            _slot2.Type.Returns(SlotType.Platform);

            _mutableSlotsToReturn1 = new List<ISlot>();
            _slotsToReturn1 = new ReadOnlyCollection<ISlot>(_mutableSlotsToReturn1);

            _mutableSlotsToReturn2 = new List<ISlot>();
            _slotsToReturn2 = new ReadOnlyCollection<ISlot>(_mutableSlotsToReturn2);

            _building = Substitute.For<IBuilding>();
        }

        #region HighlightAvailableSlots
        [Test]
        public void HighlightAvailableSlots_HighlightsMatchingSlots()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slot2.Type).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);
            
            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);

            _slot2.Received().IsVisible = true;
        }

        [Test]
        public void HighlightAvailableSlots_DoesNotHighlightsNonMatchingSlots()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slot2.Type).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(false);

            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);

            _slot2.DidNotReceive().IsVisible = true;
        }

        [Test]
        public void HighlightAvailableSlots_HighlightHighlightedSlotType_DoesNothing()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slot2.Type).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);

            // First highlight
            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);
            _slot2.Received().IsVisible = true;

            _slot2.ClearReceivedCalls();

            // Second highlight, SAME slot type
            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);
            _slot2.DidNotReceive().IsVisible = true;
        }

        [Test]
        public void HighlightAvailableSlots_UnhighlightsCurrentlyHighlightedSlots_HighlightsFreeSlots()
        {
            // First highlight
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slot2.Type).Returns(_slotsToReturn2);

            _highlightableFilter.IsMatch(_slot2).Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);
            _slot2.Received().IsVisible = true;

            // Second highlight, different slot type
            _mutableSlotsToReturn1.Add(_slot1);
            _slotAccessor.GetSlots(_slot1.Type).Returns(_slotsToReturn1);

            _highlightableFilter.IsMatch(_slot1).Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slot1.Type);

            _slot2.Received().IsVisible = false;
            _slot1.Received().IsVisible = true;
        }
        #endregion HighlightAvailableSlots

        #region UnhighlightSlots
        [Test]
        public void UnhighlightSlots_FreeSlotsOfTypeVisible_Unhighlights()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slot2.Type).Returns(_slotsToReturn2);

            // Highlight slots
            _slot2.IsFree.Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slot2.Type);

            // Unhighlight slots
            _slotHighlighter.UnhighlightSlots();
            _slot2.Received().IsVisible = false;
        }

        [Test]
        public void UnhighlightSlots_SingleBuildingSlotVisible_Unhighlights()
        {
            // Highlight single slot
            HighlightBuildingSlot();

            // Unhighlight single slot
            _slotHighlighter.UnhighlightSlots();
            _slot2.Received().IsVisible = false;
        }
        #endregion UnhighlightSlots

        #region HighlightBuildingSlot
        [Test]
        public void HighlightBuildingSlot()
        {
            _slotAccessor.GetSlot(_building).Returns(_slot2);
            _slotHighlighter.HighlightBuildingSlot(_building);
            _slot2.Received().IsVisible = true;
        }

        [Test]
        public void HighlightBuildingSlot_NoSlotForBuilding_Throws()
        {
            _slotAccessor.GetSlot(_building).Returns((ISlot)null);
            Assert.Throws<UnityAsserts.AssertionException>(() => _slotHighlighter.HighlightBuildingSlot(_building));
        }
        #endregion HighlightBuildingSlot
    }
}
