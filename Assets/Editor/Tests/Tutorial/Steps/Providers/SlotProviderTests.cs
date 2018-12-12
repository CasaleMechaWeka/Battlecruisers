//using BattleCruisers.Buildables.Buildings;
//using BattleCruisers.Cruisers.Slots;
//using BattleCruisers.Tutorial.Highlighting;
//using BattleCruisers.Tutorial.Providers;
//using BattleCruisers.Tutorial.Steps.Providers;
//using NSubstitute;
//using NUnit.Framework;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;

//namespace BattleCruisers.Tests.Tutorial.Steps.Providers
//{
//    public class SlotProviderTests
//    {
//        private IItemProvider<ISlot> _slotsProvider;
//        private IItemProvider<IHighlightable> _highlightablesProvider;
//        private IItemProvider<IClickableEmitter> _clickablesProvider;

//        private ISlotAccessor _slotAccessor;
//        private SlotSpecification _slotSpecificationPreferFront, _slotSpecificationPreferRear;

//        private ReadOnlyCollection<ISlot> _slots;
//        private ISlot _slot1, _slot2;

//        [SetUp]
//        public void SetuUp()
//        {
//            _slotAccessor = Substitute.For<ISlotAccessor>();
//            _slotSpecificationPreferFront = new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: true);
//            _slotSpecificationPreferRear = new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: false);

//            _slot1 = Substitute.For<ISlot>();
//            _slot2 = Substitute.For<ISlot>();

//            _slots = new ReadOnlyCollection<ISlot>(
//                new List<ISlot>()
//                {
//                    _slot1,
//                    _slot2
//                });
//        }

//        private void CreateProvider(SlotSpecification specification)
//        {
//            SlotProvider provider = new SlotProvider(_slotAccessor, specification);
            
//            _slotsProvider = provider;
//            _highlightablesProvider = provider;
//            _clickablesProvider = provider;
//        }

//        [Test]
//        public void PreferFrontmostSlot_ReturnsSingleSlot()
//        {
//            CreateProvider(_slotSpecificationPreferFront);

//            _slotAccessor.GetFreeSlot(_slotSpecificationPreferFront).Returns(_slot1);

//            Assert.AreEqual(1, _slotsProvider.FindItems().Count);
//            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot1));

//            Assert.AreEqual(1, _highlightablesProvider.FindItems().Count);
//            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot1));

//            Assert.AreEqual(1, _clickablesProvider.FindItems().Count);
//            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot1));
//        }

//        [Test]
//        public void DoNotPreferFrontmostSlot_ReturnsMultipleSlots()
//        {
//            CreateProvider(_slotSpecificationPreferRear);

//            _slotAccessor.GetFreeSlots(_slotSpecificationPreferRear.SlotType).Returns(_slots);

//            Assert.AreEqual(2, _slotsProvider.FindItems().Count);
//            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot1));
//            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot2));

//            Assert.AreEqual(2, _highlightablesProvider.FindItems().Count);
//            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot1));
//            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot2));

//            Assert.AreEqual(2, _clickablesProvider.FindItems().Count);
//            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot1));
//            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot2));
//        }

//        [Test]
//        public void SlotsAreCached()
//        {
//            CreateProvider(_slotSpecificationPreferFront);

//            _slotAccessor.GetFreeSlot(_slotSpecificationPreferFront).Returns(_slot1);

//            _slotsProvider.FindItems();
//            _slotAccessor.Received().GetFreeSlot(_slotSpecificationPreferFront);
//            _slotAccessor.ClearReceivedCalls();

//            _slotsProvider.FindItems();
//            _slotAccessor.DidNotReceive().GetFreeSlot(_slotSpecificationPreferFront);
//        }
//    }
//}
