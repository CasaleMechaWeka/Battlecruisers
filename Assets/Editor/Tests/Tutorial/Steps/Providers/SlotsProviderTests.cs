using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class SlotsProviderTests
    {
        private IListProvider<ISlot> _slotsProvider;
        private IListProvider<IHighlightable> _highlightablesProvider;
        private IListProvider<IClickableEmitter> _clickablesProvider;

        private ISlotWrapper _slotWrapper;
        private SlotType _slotType;
        private bool _preferFrontmostSlot;

        private ReadOnlyCollection<ISlot> _slots;
        private ISlot _slot1, _slot2;

        [SetUp]
        public void SetuUp()
        {
            _slotWrapper = Substitute.For<ISlotWrapper>();
            _slotType = SlotType.Platform;

            _slot1 = Substitute.For<ISlot>();
            _slot2 = Substitute.For<ISlot>();

            _slots = new ReadOnlyCollection<ISlot>(
                new List<ISlot>()
                {
                    _slot1,
                    _slot2
                });
        }

        private void CreateProvider()
        {
            SlotsProvider provider = new SlotsProvider(_slotWrapper, _slotType, _preferFrontmostSlot);
            
            _slotsProvider = provider;
            _highlightablesProvider = provider;
            _clickablesProvider = provider;
        }

        [Test]
        public void PreferFrontmostSlot_ReturnsSingleSlot()
        {
            _preferFrontmostSlot = true;
            CreateProvider();

			_slotWrapper.GetFreeSlot(_slotType, _preferFrontmostSlot).Returns(_slot1);

            Assert.AreEqual(1, _slotsProvider.FindItems().Count);
            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot1));

            Assert.AreEqual(1, _highlightablesProvider.FindItems().Count);
            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot1));

            Assert.AreEqual(1, _clickablesProvider.FindItems().Count);
            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot1));
        }

        [Test]
        public void DoNotPreferFrontmostSlot_ReturnsMultipleSlots()
        {
            _preferFrontmostSlot = false;
            CreateProvider();

            _slotWrapper.GetFreeSlots(_slotType).Returns(_slots);

            Assert.AreEqual(2, _slotsProvider.FindItems().Count);
            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot1));
            Assert.IsTrue(_slotsProvider.FindItems().Contains(_slot2));

            Assert.AreEqual(2, _highlightablesProvider.FindItems().Count);
            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot1));
            Assert.IsTrue(_highlightablesProvider.FindItems().Contains(_slot2));

            Assert.AreEqual(2, _clickablesProvider.FindItems().Count);
            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot1));
            Assert.IsTrue(_clickablesProvider.FindItems().Contains(_slot2));
        }

        [Test]
        public void SlotsAreCached()
        {
            _preferFrontmostSlot = true;
            CreateProvider();

            _slotWrapper.GetFreeSlot(_slotType, _preferFrontmostSlot).Returns(_slot1);

            _slotsProvider.FindItems();
            _slotWrapper.Received().GetFreeSlot(_slotType, _preferFrontmostSlot);
            _slotWrapper.ClearReceivedCalls();

            _slotsProvider.FindItems();
            _slotWrapper.DidNotReceive().GetFreeSlot(_slotType, _preferFrontmostSlot);
        }
    }
}
