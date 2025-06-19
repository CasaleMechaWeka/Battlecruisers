using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class SlotInitialiserTests
    {
        [Test]
        public void InitialiseSlots()
        {
            ICruiser parentCruiser = Substitute.For<ICruiser>();

            // Deck2, Deck1, Platform, Bow
            ISlot frontSlot = CreateSlot(index: 1, type: SlotType.Bow);
            ISlot middleSlot = CreateSlot(index: 2, type: SlotType.Platform);
            ISlot deckSlot1 = CreateSlot(index: 3, type: SlotType.Deck);
            ISlot deckSlot2 = CreateSlot(index: 4, type: SlotType.Deck);

            // Create slots out of order on purpose, because slot wrapper should order slots
            IList<ISlot> slots = new List<ISlot>()
            {
                middleSlot,
                frontSlot,
                deckSlot2,
                deckSlot1
            };

            SlotInitialiser slotInitialiser = new SlotInitialiser();

            slotInitialiser.InitialiseSlots(parentCruiser, slots);

            frontSlot.Received().Initialise(parentCruiser, Arg.Is<ReadOnlyCollection<ISlot>>(
                neighbours =>
                    neighbours.Contains(middleSlot)
                    && !neighbours.Contains(deckSlot1)
                    && !neighbours.Contains(deckSlot2)
            ));

            middleSlot.Received().Initialise(parentCruiser, Arg.Is<ReadOnlyCollection<ISlot>>(
                neighbours =>
                    neighbours.Contains(frontSlot)
                    && neighbours.Contains(deckSlot1)
                    && !neighbours.Contains(deckSlot2)
            ));

            deckSlot1.Received().Initialise(parentCruiser, Arg.Is<ReadOnlyCollection<ISlot>>(
                neighbours =>
                    neighbours.Contains(middleSlot)
                    && neighbours.Contains(deckSlot2)
                    && !neighbours.Contains(frontSlot)
            ));

            deckSlot2.Received().Initialise(parentCruiser, Arg.Is<ReadOnlyCollection<ISlot>>(
                neighbours =>
                    neighbours.Contains(deckSlot1)
                    && !neighbours.Contains(middleSlot)
                    && !neighbours.Contains(frontSlot)
            ));
        }

        private ISlot CreateSlot(int index, SlotType type)
        {
            ISlot slot = Substitute.For<ISlot>();
            slot.Index.Returns(index);
            slot.Type.Returns(type);
            return slot;
        }
    }
}