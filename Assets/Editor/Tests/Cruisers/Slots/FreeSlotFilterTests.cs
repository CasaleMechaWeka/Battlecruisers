using BattleCruisers.Cruisers.Slots;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Slots
{
    public class FreeSlotFilterTests
    {
        private ISlotFilter _filter;
        private ISlot _slot;

        [SetUp]
        public void SetuUp()
        {
            _filter = new FreeSlotFilter();
            _slot = Substitute.For<ISlot>();
        }

        [Test]
        public void IsMatch_True()
        {
            _slot.IsFree.Returns(true);
            Assert.IsTrue(_filter.IsMatch(_slot));
        }

        [Test]
        public void IsMatch_False()
        {
            _slot.IsFree.Returns(false);
            Assert.IsFalse(_filter.IsMatch(_slot));
        }
    }
}
