using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Slots.BuildingPlacement;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    // FELIX  NEXT :D
    public class SlotAccessorTests
    {
        private ISlotAccessor _slotAccessor;
        private ISlot _platformSlot, _deckSlot1, _deckSlot2;
        private IBuilding _building;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _platformSlot = CreateSlot(index: 2, type: SlotType.Platform);
            _deckSlot1 = CreateSlot(index: 3, type: SlotType.Deck);
            _deckSlot2 = CreateSlot(index: 4, type: SlotType.Deck);

            IDictionary<SlotType, ReadOnlyCollection<ISlot>> slots = new Dictionary<SlotType, ReadOnlyCollection<ISlot>>();

            ReadOnlyCollection<ISlot> platformSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>()
            {
                _platformSlot
            });
            slots.Add(SlotType.Platform, platformSlots);

            ReadOnlyCollection<ISlot> deckSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>()
            {
                _deckSlot1,
                _deckSlot2
            });
            slots.Add(SlotType.Deck, deckSlots);

            _slotAccessor = new SlotAccessor(slots);

            _building = Substitute.For<IBuilding>();
        }

        private ISlot CreateSlot(int index, SlotType type)
        {
            ISlot slot = Substitute.For<ISlot>();
            slot.Index.Returns(index);
            slot.Type.Returns(type);
            return slot;
        }

        #region IsSlotAvailable
        [Test]
        public void IsSlotAvailable_NotFree_ReturnsFalse()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Platform, BuildingFunction.Shield, preferCruiserFront: false);

            _platformSlot.IsFree.Returns(false);

            Assert.IsFalse(_slotAccessor.IsSlotAvailable(desiredSpecification));
        }

        [Test]
        public void IsSlotAvailable_IsFree_NonMatchingFunction_NonGenericDesiredFunction_ReturnsFalse()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Platform, BuildingFunction.Shield, preferCruiserFront: false);

            _platformSlot.IsFree.Returns(true);
            _platformSlot.BuildingFunctionAffinity.Returns(BuildingFunction.Generic);

            Assert.IsFalse(_slotAccessor.IsSlotAvailable(desiredSpecification));
        }

        [Test]
        public void IsSlotAvailable_IsFree_IsMatchingFunction_ReturnsTrue()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Platform, BuildingFunction.Shield, preferCruiserFront: false);

            _platformSlot.IsFree.Returns(true);
            _platformSlot.BuildingFunctionAffinity.Returns(BuildingFunction.Shield);

            Assert.IsTrue(_slotAccessor.IsSlotAvailable(desiredSpecification));
        }

        [Test]
        public void IsSlotAvailable_IsFree_NonMatchingFunction_GenericDesiredFunction_ReturnsTrue()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: false);

            _platformSlot.IsFree.Returns(true);
            _platformSlot.BuildingFunctionAffinity.Returns(BuildingFunction.Shield);

            Assert.IsTrue(_slotAccessor.IsSlotAvailable(desiredSpecification));
        }
        #endregion IsSlotAvailable



        #region GetFreeSlot
        [Test]
        public void GetFreeSlot_PreferFront_ReturnsFrontSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: true);

            _deckSlot1.IsFree.Returns(true);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_deckSlot1, freeSlot);
        }

        [Test]
        public void GetFreeSlot_PreferBack_ReturnsBackSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: false);

            _deckSlot1.IsFree.Returns(true);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_deckSlot2, freeSlot);
        }

        [Test]
        public void GetFreeSlot_ReturnsOnlyFreeSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: false);

            _deckSlot1.IsFree.Returns(false);
            _deckSlot2.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_deckSlot2, freeSlot);
        }
        #endregion GetFreeSlot

        //[Test]
        //public void GetSlotCount()
        //{
        //    Assert.AreEqual(2, _slotAccessor.GetSlotCount(SlotType.Deck));
        //    Assert.AreEqual(1, _slotAccessor.GetSlotCount(_platformSlot.Type));
        //}

        //[Test]
        //public void GetSlots()
        //{
        //    // FELIX
        //}

        //#region GetFreeSlots
        //[Test]
        //public void GetFreeSlots_NonExistantType_Throws()
        //{
        //    Assert.Throws<UnityAsserts.AssertionException>(() => _slotAccessor.GetFreeSlots(SlotType.Utility));
        //}

        //[Test]
        //public void GetFreeSlots()
        //{
        //    _deckSlot1.IsFree.Returns(true);
        //    _deckSlot2.IsFree.Returns(false);

        //    ReadOnlyCollection<ISlot> deckSlots = _slotAccessor.GetFreeSlots(SlotType.Deck);

        //    Assert.AreEqual(1, deckSlots.Count);
        //    Assert.IsTrue(deckSlots.Contains(_deckSlot1));
        //}
        //#endregion GetFreeSlots
    }
}
