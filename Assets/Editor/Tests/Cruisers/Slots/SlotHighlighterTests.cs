using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tests.Utils.Extensions;
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
        private ICruiserBuildingMonitor _parentCruiserBuildingMonitor;
        private ISlot _slot1, _slot2;
        private ISlotSpecification _slotSpec1, _slotSpec2;
        private ReadOnlyCollection<ISlot> _slotsToReturn1, _slotsToReturn2;
        private IList<ISlot> _mutableSlotsToReturn1, _mutableSlotsToReturn2;
        private IBuilding _building;

        [SetUp]
        public void SetuUp()
        {
            _slotAccessor = Substitute.For<ISlotAccessor>();
            _highlightableFilter = Substitute.For<ISlotFilter>();
            _parentCruiserBuildingMonitor = Substitute.For<ICruiserBuildingMonitor>();

            _slotHighlighter = new SlotHighlighter(_slotAccessor, _highlightableFilter, _parentCruiserBuildingMonitor);

            _slot1 = Substitute.For<ISlot>();
            _slot1.Type.Returns(SlotType.Deck);
            _slotSpec1 = new SlotSpecification(_slot1.Type);

            _slot2 = Substitute.For<ISlot>();
            _slot2.Type.Returns(SlotType.Platform);
            _slotSpec2 = new SlotSpecification(_slot2.Type);

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
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);
            
            bool wasAnySlotHighlighted = _slotHighlighter.HighlightAvailableSlots(_slotSpec2);

            _slot2.Received().IsVisible = true;
            Assert.IsTrue(wasAnySlotHighlighted);
        }

        [Test]
        public void HighlightAvailableSlots_DoesNotHighlightsNonMatchingSlots()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(false);

            bool wasAnySlotHighlighted = _slotHighlighter.HighlightAvailableSlots(_slotSpec2);

            _slot2.DidNotReceive().IsVisible = true;
            Assert.IsFalse(wasAnySlotHighlighted);
        }

        [Test]
        public void HighlightAvailableSlots_UnhighlightsCurrentlyHighlightedSlots_HighlightsFreeSlots()
        {
            // First highlight
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);

            _highlightableFilter.IsMatch(_slot2).Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slotSpec2);
            _slot2.Received().IsVisible = true;

            // Second highlight
            _mutableSlotsToReturn1.Add(_slot1);
            _slotAccessor.GetSlots(_slotSpec1).Returns(_slotsToReturn1);

            _highlightableFilter.IsMatch(_slot1).Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slotSpec1);

            _slot2.Received().IsVisible = false;
            _slot1.Received().IsVisible = true;
        }
        #endregion HighlightAvailableSlots

        [Test]
        public void HighlightSlots()
        {
            // First highlight
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);

            _highlightableFilter.IsMatch(_slot2).Returns(false);  // Highlights non-matching slots
            _slotHighlighter.HighlightSlots(_slotSpec2);
            _slot2.Received().IsVisible = true;

            // Second highlight
            _mutableSlotsToReturn1.Add(_slot1);
            _slotAccessor.GetSlots(_slotSpec1).Returns(_slotsToReturn1);

            _highlightableFilter.IsMatch(_slot1).Returns(false);  // Highlights non-matching slots
            _slotHighlighter.HighlightSlots(_slotSpec1);

            _slot2.Received().IsVisible = false;
            _slot1.Received().IsVisible = true;
        }

        #region UnhighlightSlots
        [Test]
        public void UnhighlightSlots_FreeSlotsOfTypeVisible_Unhighlights()
        {
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);

            // Highlight slots
            _slot2.IsFree.Returns(true);
            _slotHighlighter.HighlightAvailableSlots(_slotSpec2);

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

        #region BuildingDestroyed event
        [Test]
        public void BuildingDestroyed_AreNotHighlightingSlots_DoesNothing()
        {
            // Make a slot match the highlightable filter
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);

            // Building destroyed
            _parentCruiserBuildingMonitor.EmitBuildingDestroyed(null);

            // Received no highlight refresh
            _slot2.DidNotReceiveWithAnyArgs().IsVisible = default;
        }

        [Test]
        public void BuildingDestroyed_AreHighlightingNonMatchingSlots_DoesNothing()
        {
            // Highlight slots
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);

            _slotHighlighter.HighlightAvailableSlots(_slotSpec2);

            _slot2.Received().IsVisible = true;

            // Building destroyed
            _slot2.ClearReceivedCalls();
            _building.SlotSpecification.Returns(_slotSpec1);
            _parentCruiserBuildingMonitor.EmitBuildingDestroyed(_building);

            // Received no highlight refresh
            _slot2.DidNotReceiveWithAnyArgs().IsVisible = default;
        }

        [Test]
        public void BuildingDestroyed_AreHighlightingMatchingSlotType_Rehighlights()
        {
            // Highlight slots
            _mutableSlotsToReturn2.Add(_slot2);
            _slotAccessor.GetSlots(_slotSpec2).Returns(_slotsToReturn2);
            _highlightableFilter.IsMatch(_slot2).Returns(true);

            _slotHighlighter.HighlightAvailableSlots(_slotSpec2);

            _slot2.Received().IsVisible = true;

            // Building destroyed
            ISlotSpecification sameSlotType = new SlotSpecification(_slot2.Type, BuildingFunction.Shield, preferCruiserFront: false);
            _slot2.ClearReceivedCalls();
            _building.SlotSpecification.Returns(sameSlotType);
            _parentCruiserBuildingMonitor.EmitBuildingDestroyed(_building);

            // Received highlight refresh
            _slot2.Received().IsVisible = false;
            _slot2.Received().IsVisible = true;
        }
        #endregion BuildingDestroyed event
    }
}
