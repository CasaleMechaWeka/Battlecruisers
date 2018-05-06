using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class SlotsWrapperTests
    {
        private ISlotWrapper _slotWrapper;
        private ICruiser _parentCruiser;
        private ISlotFilter _highlightableFilter, _clickableFilter;
        private ISlot _frontSlot, _middleSlot, _deckSlot1, _deckSlot2;
        private IBuilding _building;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _parentCruiser = Substitute.For<ICruiser>();
            _highlightableFilter = Substitute.For<ISlotFilter>();
            _clickableFilter = Substitute.For<ISlotFilter>();

            // Deck2, Deck1, Platform, Bow
            _frontSlot = CreateSlot(index: 1, type: SlotType.Bow);
            _middleSlot = CreateSlot(index: 2, type: SlotType.Platform);
            _deckSlot1 = CreateSlot(index: 3, type: SlotType.Deck);
            _deckSlot2 = CreateSlot(index: 4, type: SlotType.Deck);

            // Create slots out of order on purpose, because slot wrapper should order slots
            IList<ISlot> slots = new List<ISlot>()
            {
                _middleSlot,
                _frontSlot,
                _deckSlot2,
                _deckSlot1
            };

            _slotWrapper = new SlotWrapper(_parentCruiser, slots, _highlightableFilter, _clickableFilter);

            _building = Substitute.For<IBuilding>();
            SlotType middleSlotType = _middleSlot.Type;
            _building.SlotType.Returns(middleSlotType);
        }

        private ISlot CreateSlot(int index, SlotType type)
        {
            ISlot slot = Substitute.For<ISlot>();
            slot.Index.Returns(index);
            slot.Type.Returns(type);
            return slot;
        }

        [Test]
        public void Constructor_SetsUpSlots()
        {
            _frontSlot.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_middleSlot)
                    && !neighbours.Contains(_deckSlot1)
                    && !neighbours.Contains(_deckSlot2)
            ),
            _clickableFilter);

            _middleSlot.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_frontSlot)
                    && neighbours.Contains(_deckSlot1)
                    && !neighbours.Contains(_deckSlot2)
            ),
            _clickableFilter);

            _deckSlot1.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_middleSlot)
                    && neighbours.Contains(_deckSlot2)
                    && !neighbours.Contains(_frontSlot)
            ),
            _clickableFilter);

            _deckSlot2.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_deckSlot1)
                    && !neighbours.Contains(_middleSlot)
                    && !neighbours.Contains(_frontSlot)
            ),
            _clickableFilter);
        }

        #region IsSlotAvailable
        [Test]
        public void IsSlotAvailable_No()
        {
            _middleSlot.IsFree.Returns(false);
            Assert.IsFalse(_slotWrapper.IsSlotAvailable(SlotType.Platform));
        }

        [Test]
        public void IsSlotAvailable_Yes()
        {
            _middleSlot.IsFree.Returns(true);
            Assert.IsTrue(_slotWrapper.IsSlotAvailable(SlotType.Platform));
        }
        #endregion IsSlotAvailable

        [Test]
        public void ShowAllSlots()
        {
            _slotWrapper.HideAllSlots();
            _slotWrapper.ShowAllSlots();

            Assert.IsTrue(_frontSlot.IsVisible);
            Assert.IsTrue(_middleSlot.IsVisible);
            Assert.IsTrue(_deckSlot1.IsVisible);
        }

        [Test]
        public void HideAllSlots()
        {
            _slotWrapper.ShowAllSlots();
            _slotWrapper.HideAllSlots();

            Assert.IsFalse(_frontSlot.IsVisible);
            Assert.IsFalse(_middleSlot.IsVisible);
            Assert.IsFalse(_deckSlot1.IsVisible);
        }

        #region HighlightAvailableSlots
        [Test]
        public void HighlightAvailableSlots_HighlightsMatchingSlots()
        {
            _highlightableFilter.IsMatch(_middleSlot).Returns(true);

            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);

            _highlightableFilter.Received().IsMatch(_middleSlot);
            _middleSlot.Received().HighlightSlot();
        }

        [Test]
        public void HighlightAvailableSlots_DoesNotHighlightsNonMatchingSlots()
        {
            _highlightableFilter.IsMatch(_middleSlot).Returns(false);

            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);

            _highlightableFilter.Received().IsMatch(_middleSlot);
            _middleSlot.DidNotReceive().HighlightSlot();
        }

        [Test]
        public void HighlightAvailableSlots_HighlightHighlightedSlotType_DoesNothing()
        {
            // First highlight
            _highlightableFilter.IsMatch(_middleSlot).Returns(true);
            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);
            _middleSlot.Received().HighlightSlot();

            _middleSlot.ClearReceivedCalls();

            // Second highlight, SAME slot type
            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);
            _middleSlot.DidNotReceive().HighlightSlot();
        }

        [Test]
        public void HighlightAvailableSlots_UnhighlightsCurrentlyHighlightedSlots_HighlightsFreeSlots()
        {
            // First highlight
            _highlightableFilter.IsMatch(_middleSlot).Returns(true);
            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);
            _middleSlot.Received().HighlightSlot();

            // Second highlight, different slot type
            _highlightableFilter.IsMatch(_frontSlot).Returns(true);
            _slotWrapper.HighlightAvailableSlots(_frontSlot.Type);
            _middleSlot.Received().UnhighlightSlot();
            _frontSlot.Received().HighlightSlot();
        }
        #endregion HighlightAvailableSlots

        #region HighlightBuildingSlot
        [Test]
        public void HighlightBuildingSlot()
        {
            _middleSlot.Building.Returns(_building);
            _slotWrapper.HighlightBuildingSlot(_building);
            _middleSlot.Received().HighlightSlot();
            Assert.IsTrue(_middleSlot.IsVisible);
        }

        [Test]
        public void HighlightBuildingSlot_NoSlotForBuilding_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _slotWrapper.HighlightBuildingSlot(_building));
        }
        #endregion HighlightBuildingSlot

        #region UnhighlightSlots
        [Test]
        public void UnhighlightSlots_UnhighlightsHighlightedSlots()
        {
            // Highlight slots
            _middleSlot.IsFree.Returns(true);
            _slotWrapper.HighlightAvailableSlots(_middleSlot.Type);

            // Unhighlight slots
            _slotWrapper.UnhighlightSlots();
            _middleSlot.Received().UnhighlightSlot();
        }

        [Test]
        public void UnhighlightSlots_MultipleSlotsAreVisible_SingleSlot_UnhighlightedButVisible()
        {
            // Ensure multiple slots are visible
            _slotWrapper.ShowAllSlots();

            // Highlight single slot
            HighlightBuildingSlot();

            // Unhighlight single slot
            _slotWrapper.UnhighlightSlots();
            _middleSlot.Received().UnhighlightSlot();
            Assert.IsTrue(_middleSlot.IsVisible);
        }

        [Test]
        public void UnhighlightSlots_MultipleSlotsAreNotVisible_SingleSlot_UnhighlightedAndInvisible()
        {
            // Highlight single slot
            HighlightBuildingSlot();

            // Unhighlight single slot
            _slotWrapper.UnhighlightSlots();
            _middleSlot.Received().UnhighlightSlot();
            Assert.IsFalse(_middleSlot.IsVisible);
        }
        #endregion UnhighlightSlots

        #region GetFreeSlot
        [Test]
        public void GetFreeSlot_PreferFront_ReturnsFrontSlot()
        {
            _deckSlot1.IsFree.Returns(true);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotWrapper.GetFreeSlot(SlotType.Deck, preferFromFront: true);

            Assert.AreSame(_deckSlot1, freeSlot);
        }

        [Test]
        public void GetFreeSlot_PreferBack_ReturnsBackSlot()
        {
            _deckSlot1.IsFree.Returns(true);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotWrapper.GetFreeSlot(SlotType.Deck, preferFromFront: false);

            Assert.AreSame(_deckSlot2, freeSlot);
        }

        [Test]
        public void GetFreeSlot_ReturnsOnlyFreeSlot()
        {
            _deckSlot1.IsFree.Returns(false);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotWrapper.GetFreeSlot(SlotType.Deck, preferFromFront: true);

            Assert.AreSame(_deckSlot2, freeSlot);
        }
        #endregion GetFreeSlot

        [Test]
        public void GetSlotCount()
        {
            Assert.AreEqual(2, _slotWrapper.GetSlotCount(SlotType.Deck));
            Assert.AreEqual(1, _slotWrapper.GetSlotCount(_middleSlot.Type));
        }

        #region GetSlotsForType
        [Test]
        public void GetSlotsForType_NonExistantType_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _slotWrapper.GetSlotsForType(SlotType.Utility));
        }

        [Test]
        public void GetSlotsForType()
        {
            ReadOnlyCollection<ISlot> deckSlots = _slotWrapper.GetSlotsForType(SlotType.Deck);
            Assert.AreEqual(2, deckSlots.Count);
            Assert.IsTrue(deckSlots.Contains(_deckSlot1));
            Assert.IsTrue(deckSlots.Contains(_deckSlot2));
        }
        #endregion GetSlotsForType
    }
}
