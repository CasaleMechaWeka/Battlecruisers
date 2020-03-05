using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class SlotAccessorTests
    {
        private ISlotAccessor _slotAccessor;
        private IDictionary<SlotType, ReadOnlyCollection<ISlot>> _slots;
        private ISlot _platformSlot, _genericDeckSlot, _antiShipDeckSlot;
        private IBuilding _building;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _platformSlot = CreateSlot(index: 2, type: SlotType.Platform, BuildingFunction.Generic);
            _genericDeckSlot = CreateSlot(index: 3, type: SlotType.Deck, BuildingFunction.Generic);
            _antiShipDeckSlot = CreateSlot(index: 4, type: SlotType.Deck, BuildingFunction.AntiShip);

            _slots = new Dictionary<SlotType, ReadOnlyCollection<ISlot>>();

            ReadOnlyCollection<ISlot> platformSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>()
            {
                _platformSlot
            });
            _slots.Add(SlotType.Platform, platformSlots);

            ReadOnlyCollection<ISlot> deckSlots = new ReadOnlyCollection<ISlot>(new List<ISlot>()
            {
                _genericDeckSlot,
                _antiShipDeckSlot
            });
            _slots.Add(SlotType.Deck, deckSlots);

            _slotAccessor = new SlotAccessor(_slots);

            _building = Substitute.For<IBuilding>();
        }

        private ISlot CreateSlot(int index, SlotType type, BuildingFunction slotAffinity)
        {
            ISlot slot = Substitute.For<ISlot>();
            slot.Index.Returns(index);
            slot.Type.Returns(type);
            slot.BuildingFunctionAffinity.Returns(slotAffinity);
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

        #region GetSlots
        [Test]
        public void GetSlots_NonExistantSlotType_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _slotAccessor.GetSlots(new SlotSpecification(SlotType.Mast)));
        }

        [Test]
        public void GetSlots_ExistantSlotType_ReturnsSlots()
        {
            ReadOnlyCollection<ISlot> deckSlots = _slotAccessor.GetSlots(new SlotSpecification(SlotType.Deck));
            Assert.AreSame(_slots[SlotType.Deck], deckSlots);
        }

        [Test]
        public void GetSlots_AntiShipSlots()
        {
            ReadOnlyCollection<ISlot> antiShipSlots = _slotAccessor.GetSlots(new SlotSpecification(SlotType.Deck, BuildingFunction.AntiShip));
            Assert.AreEqual(1, antiShipSlots.Count);
            Assert.AreSame(_antiShipDeckSlot, antiShipSlots[0]);
        }
        #endregion GetSlots

        #region GetFreeSlot
        [Test]
        public void GetFreeSlot_PreferFront_ReturnsFrontSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: true);

            _genericDeckSlot.IsFree.Returns(true);
            _antiShipDeckSlot.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_genericDeckSlot, freeSlot);
        }

        [Test]
        public void GetFreeSlot_PreferBack_ReturnsBackSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: false);

            _genericDeckSlot.IsFree.Returns(true);
            _antiShipDeckSlot.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_antiShipDeckSlot, freeSlot);
        }

        [Test]
        public void GetFreeSlot_ReturnsOnlyFreeSlot()
        {
            SlotSpecification desiredSpecification = new SlotSpecification(SlotType.Deck, BuildingFunction.Generic, preferCruiserFront: false);

            _genericDeckSlot.IsFree.Returns(false);
            _antiShipDeckSlot.IsFree.Returns(true);

            ISlot freeSlot = _slotAccessor.GetFreeSlot(desiredSpecification);

            Assert.AreSame(_antiShipDeckSlot, freeSlot);
        }
        #endregion GetFreeSlot

        #region GetSlot
        [Test]
        public void GetSlot_NonExistantSlotType_Throws()
        {
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Utility, default, default);
            _building.SlotSpecification.Returns(slotSpecification);

            Assert.Throws<UnityAsserts.AssertionException>(() => _slotAccessor.GetSlot(_building));
        }

        [Test]
        public void GetSlot_NoSlotForBuilding_ReturnsNull()
        {
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Deck, default, default);
            _building.SlotSpecification.Returns(slotSpecification);

            Assert.IsNull(_slotAccessor.GetSlot(_building));
        }

        [Test]
        public void GetSlot_ReturnsBuildingSlot()
        {
            SlotSpecification slotSpecification = new SlotSpecification(SlotType.Deck, default, default);
            _building.SlotSpecification.Returns(slotSpecification);
            _antiShipDeckSlot.Building.Value.Returns(_building);

            Assert.AreSame(_antiShipDeckSlot, _slotAccessor.GetSlot(_building));
        }
        #endregion GetSlot

        [Test]
        public void GetSlotCount()
        {
            Assert.AreEqual(2, _slotAccessor.GetSlotCount(SlotType.Deck));
            Assert.AreEqual(1, _slotAccessor.GetSlotCount(_platformSlot.Type));
        }
    }
}
