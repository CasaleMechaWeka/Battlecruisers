using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Tutorial.Highlighting.Masked;
using BattleCruisers.Tutorial.Providers;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Steps.Providers
{
    public class SlotProviderTests
    {
        private IItemProvider<ISlot> _slotProvider;
        private IItemProvider<IMaskHighlightable> _highlightableProvider;
        private IItemProvider<IClickableEmitter> _clickableProvider;

        private ISlotAccessor _slotAccessor;
        private SlotSpecification _slotSpecification;

        private ISlot _slot;

        [SetUp]
        public void SetuUp()
        {
            _slotSpecification = new SlotSpecification(SlotType.Platform, BuildingFunction.Generic, preferCruiserFront: true);
            _slotAccessor = Substitute.For<ISlotAccessor>();

            SlotProvider provider = new SlotProvider(_slotAccessor, _slotSpecification);

            _slotProvider = provider;
            _highlightableProvider = provider;
            _clickableProvider = provider;

            _slot = Substitute.For<ISlot>();
            _slotAccessor.GetFreeSlot(_slotSpecification).Returns(_slot);
        }

        [Test]
        public void ProvidersReturnSlot()
        {
            Assert.AreSame(_slot, _slotProvider.FindItem());
            Assert.AreSame(_slot, _highlightableProvider.FindItem());
            Assert.AreSame(_slot, _clickableProvider.FindItem());
        }

        [Test]
        public void SlotsAreCached()
        {
            _slotProvider.FindItem();
            _slotAccessor.Received().GetFreeSlot(_slotSpecification);
            _slotAccessor.ClearReceivedCalls();

            _slotProvider.FindItem();
            _slotAccessor.DidNotReceive().GetFreeSlot(_slotSpecification);
        }
    }
}
