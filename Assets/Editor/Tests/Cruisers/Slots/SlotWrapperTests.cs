using System.Collections.Generic;
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
        private ISlot _frontSlot, _middleSlot, _backSlot;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _parentCruiser = Substitute.For<ICruiser>();

            _frontSlot = CreateSlot(index: 1, type: SlotType.Bow);
            _middleSlot = CreateSlot(index: 2, type: SlotType.Platform);
            _backSlot = CreateSlot(index: 3, type: SlotType.Stern);

            // Create slots out of order on purpose, because slot wrapper should order slots
            IList<ISlot> slots = new List<ISlot>()
            {
				_middleSlot,
                _frontSlot,
                _backSlot
            };

            _slotWrapper = new SlotWrapper(_parentCruiser, slots);
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
                    && !neighbours.Contains(_backSlot)
            ));

            _middleSlot.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_frontSlot)
                    && neighbours.Contains(_backSlot)
            ));

            _backSlot.Received().Initialise(_parentCruiser, Arg.Is<IList<ISlot>>(
                neighbours =>
                    neighbours.Contains(_middleSlot)
                    && !neighbours.Contains(_frontSlot)
            ));
        }
    }
}
